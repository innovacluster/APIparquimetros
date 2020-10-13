
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Controllers;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Services
{
    public class MultaDP10Job : IJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<MultaDP10Job> _logger;
        private readonly IServiceScopeFactory scopeFactory;
        public MultaDP10Job(ILogger<MultaDP10Job> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
            //_dbContext = dbContext;
        }
        public Task  Execute(IJobExecutionContext context)
        {


            _ = mtdRealizarMultaDp10();
            return Task.CompletedTask;
        }

        public async  Task mtdRealizarMultaDp10()
        {
           
            var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //ParametrosController par = new ParametrosController(_dbContext);
            //ActionResult<DateTime> time = par.mtdObtenerFechaMexico();
            DateTime time = DateTime.Now;

            var movimientos = await _dbContext.tbmovimientos.Where(x => x.str_comentarios == "MULTA").ToListAsync();

                if (movimientos.Count != 0)
                {
                    foreach (var item in movimientos)
                    {

                        var strategy = _dbContext.Database.CreateExecutionStrategy();
                        await strategy.ExecuteAsync(async () =>
                        {
                            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    var multaResponse = await _dbContext.tbmultas.FirstOrDefaultAsync(x => x.id == item.int_id_multa);
                                    multaResponse.bit_status = false;
                                    multaResponse.str_tipo_multa = "MULTA DESPUES DE LAS 10";
                                    await _dbContext.SaveChangesAsync();


                                    var response = await _dbContext.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                                    response.str_comentarios = "MULTA DESPUES DE LAS 10";
                                    response.bit_status = false;
                                    await _dbContext.SaveChangesAsync();


                                    var movimiento = await _dbContext.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                                    var usuario = await _dbContext.NetUsers.FirstOrDefaultAsync(x => x.Id == movimiento.int_id_usuario_id);

                                    _dbContext.tbdetallemulta.Add(new DetalleMulta()
                                    {
                                        int_id_multa = item.int_id_multa.Value,
                                        bit_status = true,
                                        dtmFecha = time,
                                        str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                                        flt_monto = 0,
                                        str_comentarios = "MULTA DESPUES DE LAS 10"
                                    });
                                    _dbContext.SaveChanges();

                                    var espacio = await _dbContext.tbespacios.FirstOrDefaultAsync(x => x.id == item.int_id_espacio);
                                    if (response.int_id_espacio == null)
                                    {
                                        _dbContext.tbdetallemovimientos.Add(new DetalleMovimientos()
                                        {
                                            int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                                            int_id_usuario_id = response.int_id_usuario_id,
                                            int_duracion = response.int_tiempo,
                                            dtm_horaInicio = response.dt_hora_inicio,
                                            dtm_horaFin = response.dtm_hora_fin,
                                            flt_importe = 0.0,
                                            flt_saldo_anterior = 0.0,
                                            flt_saldo_fin = 0.0,
                                            str_observaciones = response.str_comentarios

                                        });
                                        _dbContext.SaveChanges();

                                    }
                                    else {

                                        _dbContext.tbdetallemovimientos.Add(new DetalleMovimientos()
                                        {
                                            int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                                            int_idespacio = response.int_id_espacio.Value,
                                            int_id_usuario_id = response.int_id_usuario_id,
                                            int_id_zona = espacio.id_zona,
                                            int_duracion = response.int_tiempo,
                                            dtm_horaInicio = response.dt_hora_inicio,
                                            dtm_horaFin = response.dtm_hora_fin,
                                            flt_importe = 0.0,
                                            flt_saldo_anterior = 0.0,
                                            flt_saldo_fin = 0.0,
                                            str_observaciones = response.str_comentarios

                                        });

                                        _dbContext.SaveChanges();
                                    }


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
