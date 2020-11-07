using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Controllers;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Services
{
    public class MultaJob:IJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<MultaJob> _logger;
        private readonly IServiceScopeFactory scopeFactory;
        public MultaJob(ILogger<MultaJob> logger, IServiceScopeFactory scopeFactory)
        {
       
            this.scopeFactory = scopeFactory;
            //_dbContext = dbContext;
        }

        public Task Execute(IJobExecutionContext context)
        {

            _ = mtdRealizarMultaAutomatica();
            return Task.CompletedTask;
        }

        public async Task mtdRealizarMultaAutomatica()
        {
            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            int intIdMulta = 0;


            //ParametrosController par = new ParametrosController(dbContext);
            //ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();

            //DateTime time = time1.Value;
           DateTime time = DateTime.Now;


            var movimientos = await dbContext.tbmovimientos.Where(x => x.dtm_hora_fin.Date == time.Date && x.bit_status == true).ToListAsync();
            var tipoUsuario = await dbContext.tbtiposusuarios.FirstOrDefaultAsync(x=> x.strTipoUsuario == "AGENTE VIAL");
            var agente = await dbContext.NetUsers.FirstOrDefaultAsync(x=> x.intIdTipoUsuario == tipoUsuario.id);

            if (movimientos.Count != 0)
            {
                foreach (var item in movimientos)
                {
                    if (item.str_comentarios == "APARCADO" || item.str_comentarios.Contains("EXTENSIÓN DE TIEMPO"))
                    {
                        if (time == item.dtm_hora_fin || time > item.dtm_hora_fin)
                        {
                            if (item.int_id_espacio == null)
                            {
                                var vehiculo = await dbContext.tbvehiculos.FirstOrDefaultAsync(x => x.id == item.int_id_vehiculo_id);
                                

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
                                                dtm_fecha = time,
                                                flt_monto = 0.00,
                                                str_motivo = "Tiempo vencido",
                                                str_tipo_multa = "MULTA AUTOMATICA",
                                                str_documento_garantia = "SIN GARANTÍA",
                                                str_folio_multa = "N/A",
                                                str_placa = item.str_placa,
                                                str_clave_candado = "N/A",
                                                str_modelo = vehiculo.str_modelo,
                                                str_marca = vehiculo.str_marca,
                                                str_color = vehiculo.str_color,
                                                str_id_agente_id = agente.Id,
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
                                                dtmFecha = time,
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
                                                dtm_horaInicio = time,
                                                dtm_horaFin = time,
                                                flt_importe = 0.00,
                                                flt_saldo_anterior = 0.00,
                                                flt_saldo_fin = 0.00,
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
                            else
                            {
                                var strategy = dbContext.Database.CreateExecutionStrategy();
                                await strategy.ExecuteAsync(async () =>
                                {
                                    var vehiculo = await dbContext.tbvehiculos.FirstOrDefaultAsync(x => x.id == item.int_id_vehiculo_id);

                                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            var multa = new Multas()
                                            {
                                                created_by = "Api Automatica",
                                                bit_status = true,
                                                dtm_fecha = time,
                                                flt_monto = 0.00,
                                                str_motivo = "Tiempo vencido",
                                                str_tipo_multa = "MULTA AUTOMATICA",
                                                str_documento_garantia = "SIN GARANTÍA",
                                                str_folio_multa = "N/A",
                                                str_placa = item.str_placa,
                                                str_clave_candado = "N/A",
                                                str_modelo = vehiculo.str_modelo,
                                                str_marca = vehiculo.str_marca,
                                                str_color = vehiculo.str_color,
                                                str_id_agente_id = agente.Id,
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
                                                dtmFecha = time,
                                                str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                                                flt_monto = 0.00,
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
                                                dtm_horaInicio = time,
                                                dtm_horaFin = time,
                                                flt_importe = 0.00,
                                                flt_saldo_anterior = 0.00,
                                                flt_saldo_fin = 0.00,
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

        
    }
}
