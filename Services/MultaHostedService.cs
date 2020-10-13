using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Controllers;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Services
{
    public class MultaHostedService : IHostedService, IDisposable

    {
        private Timer timer;
        //private static readonly string UrlFecha = "https://www.jobtool.online/restapis/servicioEdadGenero/post.php?opcion=30";
        //private static readonly HttpClient client = new HttpClient();
        private readonly ApplicationDbContext context;
        public readonly IHostingEnvironment environment;
        private readonly IServiceScopeFactory scopeFactory;
        bool bolInicia = false;
        int count = 0;
        public MultaHostedService(IHostingEnvironment environment, IServiceScopeFactory scopeFactory)
        {
            this.environment = environment;
            this.scopeFactory = scopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            count = count +1;
            if (count == 1)
            {
                return Task.CompletedTask;
            }
            else
            {
                    timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
                return Task.CompletedTask;
            }
            
        }

        private void DoWork(object state)
        {
            mtdRealizarMultaAutomatica();
        }

        private void DoWorkInicia(object state)
        {
            
        }

        public async void mtdRealizarMultaAutomatica()
        {

            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
            int intIdMulta = 0;
            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();

            //DateTime time = DateTime.Now;

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

            var movimientos = await dbContext.tbmovimientos.Where(x => x.dtm_hora_fin.Date == time.Value.Date && x.bit_status == true).ToListAsync();

            if (movimientos.Count != 0)
            {
                foreach (var item in movimientos)
                {
                    if (item.str_comentarios == "APARCADO" || item.str_comentarios.Contains("EXTENSIÓN DE TIEMPO"))
                    {
                        if (time.Value == item.dtm_hora_fin || time.Value > item.dtm_hora_fin)
                        {
                            if (item.int_id_espacio == null)
                            {
                                var strategy = dbContext.Database.CreateExecutionStrategy();
                                await strategy.ExecuteAsync(async () =>
                                {

                                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            var multa = new Multas()
                                            {
                                                created_by = "Api Automatica",
                                                bit_status = true,
                                                dtm_fecha = time.Value,
                                                flt_monto = 0.0,
                                                str_motivo = "Tiempo vencido",
                                                str_tipo_multa = "MULTA AUTOMATICA",
                                                str_documento_garantia = "SIN GARANTÍA",
                                                str_folio_multa = "N/A",
                                                str_placa = item.str_placa,
                                                str_clave_candado = "N/A",
                                                //Falta modelo
                                                //Falta marca
                                                str_id_agente_id = "3766f6fd-ebd5-4ad9-a27c-80255557d959",
                                                int_id_movimiento_id = item.id,
                                                //int_id_saldo_id = item.int_id_saldo_id,
                                                int_id_vehiculo_id = item.int_id_vehiculo_id,
                                                intidconcesion_id = item.intidconcesion_id
                                            };
                                            dbContext.tbmultas.Add(multa);
                                            dbContext.SaveChanges();
                                            intIdMulta = multa.id;

                                            var response = await dbContext.tbmovimientos.FirstOrDefaultAsync(x => x.id == item.id);
                                            response.str_comentarios = "MULTA";
                                            response.boolean_multa = true;
                                            response.int_id_multa = intIdMulta;
                                            dbContext.SaveChanges();

                                            var usuario = await dbContext.NetUsers.FirstOrDefaultAsync(x => x.Id == item.int_id_usuario_id);

                                            dbContext.tbdetallemulta.Add(new DetalleMulta()
                                            {
                                                int_id_multa = intIdMulta,
                                                bit_status = true,
                                                dtmFecha = time.Value,
                                                str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                                                flt_monto = 0,
                                                str_comentarios = "MULTA AUTOMÁTICA"
                                            });
                                            dbContext.SaveChanges();

                                            var espacio = await dbContext.tbespacios.FirstOrDefaultAsync(x => x.id == item.int_id_espacio);

                                            dbContext.tbdetallemovimientos.Add(new DetalleMovimientos()
                                            {
                                                int_idmovimiento = item.id,
                                                int_id_usuario_id = response.int_id_usuario_id,
                                                int_duracion = 0,
                                                dtm_horaInicio = time.Value,
                                                dtm_horaFin = time.Value,
                                                flt_importe = 0.0,
                                                flt_saldo_anterior = 0.0,
                                                flt_saldo_fin = 0.0,
                                                str_observaciones = response.str_comentarios

                                            });

                                            dbContext.SaveChanges();
                                            transaction.Commit();
                                        }

                                        catch (Exception ex)
                                        {
                                            transaction.Rollback();


                                        }
                                    }
                                });
                            }
                            else {
                                var strategy = dbContext.Database.CreateExecutionStrategy();
                                await strategy.ExecuteAsync(async () =>
                                {

                                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            var multa = new Multas()
                                            {
                                                created_by = "Api Automatica",
                                                bit_status = true,
                                                dtm_fecha = time.Value,
                                                flt_monto = 0.0,
                                                str_motivo = "Tiempo vencido",
                                                str_tipo_multa = "MULTA AUTOMATICA",
                                                str_documento_garantia = "SIN GARANTÍA",
                                                str_folio_multa = "N/A",
                                                str_placa = item.str_placa,
                                                str_clave_candado = "N/A",
                                                //Falta modelo
                                                //Falta marca
                                                str_id_agente_id = "98f5553e-5fd9-4ade-bd51-66d6dc2fa9d3",
                                                int_id_movimiento_id = item.id,
                                                //int_id_saldo_id = item.int_id_saldo_id,
                                                int_id_vehiculo_id = item.int_id_vehiculo_id,
                                                intidconcesion_id = item.intidconcesion_id
                                            };
                                            dbContext.tbmultas.Add(multa);
                                            dbContext.SaveChanges();
                                            intIdMulta = multa.id;

                                            var response = await dbContext.tbmovimientos.FirstOrDefaultAsync(x => x.id == item.id);
                                            response.str_comentarios = "MULTA";
                                            response.boolean_multa = true;
                                            response.int_id_multa = intIdMulta;
                                            dbContext.SaveChanges();

                                            var usuario = await dbContext.NetUsers.FirstOrDefaultAsync(x => x.Id == item.int_id_usuario_id);

                                            dbContext.tbdetallemulta.Add(new DetalleMulta()
                                            {
                                                int_id_multa = intIdMulta,
                                                bit_status = true,
                                                dtmFecha = time.Value,
                                                str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                                                flt_monto = 0,
                                                str_comentarios = "MULTA AUTOMÁTICA"
                                            });
                                            dbContext.SaveChanges();

                                            var espacio = await dbContext.tbespacios.FirstOrDefaultAsync(x => x.id == item.int_id_espacio);

                                            dbContext.tbdetallemovimientos.Add(new DetalleMovimientos()
                                            {
                                                int_idmovimiento = item.id,
                                                int_idespacio = response.int_id_espacio.Value,
                                                int_id_usuario_id = response.int_id_usuario_id,
                                                int_id_zona = espacio.id_zona,
                                                int_duracion = 0,
                                                dtm_horaInicio = time.Value,
                                                dtm_horaFin = time.Value,
                                                flt_importe = 0.0,
                                                flt_saldo_anterior = 0.0,
                                                flt_saldo_fin = 0.0,
                                                str_observaciones = response.str_comentarios

                                            });

                                            dbContext.SaveChanges();
                                            transaction.Commit();
                                        }

                                        catch (Exception ex)
                                        {
                                            transaction.Rollback();

                                        }
                                    }
                                });
                            }
                           
                        } 
                    }
                }

            }


        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
