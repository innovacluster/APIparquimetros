using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public class CronJob2MultaDP10: CronJobService
    {
        private readonly ILogger<CronJob2MultaDP10> _logger;
        private readonly IServiceScopeFactory scopeFactory;

        public CronJob2MultaDP10(IScheduleConfig<CronJob2MultaDP10> config, ILogger<CronJob2MultaDP10> logger, IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob Multa despues de las 10.");
            return base.StartAsync(cancellationToken);
        }

        public  override  Task DoWork(CancellationToken cancellationToken)
        {
            var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            ParametrosController par = new ParametrosController(_dbContext);
            ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();
            DateTime time = time1.Value;

            // DateTime time = DateTime.Now;

            var movimientos =  _dbContext.tbmovimientos.Where(x => x.str_comentarios == "MULTA").ToList();

            if (movimientos.Count != 0)
            {
                foreach (var item in movimientos)
                {

                    var strategy = _dbContext.Database.CreateExecutionStrategy();
                     strategy.Execute( () =>
                    {
                        using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                var multaResponse =  _dbContext.tbmultas.FirstOrDefault(x => x.id == item.int_id_multa);
                                multaResponse.bit_status = false;
                                multaResponse.str_tipo_multa = "MULTA DESPUES DE LAS 10";
                                 _dbContext.SaveChanges();


                                var response =  _dbContext.tbmovimientos.FirstOrDefault(x => x.id == multaResponse.int_id_movimiento_id);
                                response.str_comentarios = "MULTA DESPUES DE LAS 10";
                                response.bit_status = false;
                                 _dbContext.SaveChanges();


                                var movimiento =  _dbContext.tbmovimientos.FirstOrDefault(x => x.id == multaResponse.int_id_movimiento_id);
                                var usuario =  _dbContext.NetUsers.FirstOrDefault(x => x.Id == movimiento.int_id_usuario_id);

                                _dbContext.tbdetallemulta.Add(new DetalleMulta()
                                {
                                    int_id_multa = item.int_id_multa.Value,
                                    bit_status = true,
                                    dtmFecha = time,
                                    str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                                    flt_monto = 0.00,
                                    str_comentarios = "MULTA DESPUES DE LAS 10"
                                });
                                _dbContext.SaveChanges();

                                var espacio =  _dbContext.tbespacios.FirstOrDefault(x => x.id == item.int_id_espacio);
                                if (response.int_id_espacio == null)
                                {
                                    _dbContext.tbdetallemovimientos.Add(new DetalleMovimientos()
                                    {
                                        int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                                        int_id_usuario_id = response.int_id_usuario_id,
                                        int_duracion = response.int_tiempo,
                                        dtm_horaInicio = response.dt_hora_inicio,
                                        dtm_horaFin = response.dtm_hora_fin,
                                        flt_importe = 0.00,
                                        flt_saldo_anterior = 0.00,
                                        flt_saldo_fin = 0.00,
                                        str_observaciones = response.str_comentarios

                                    });
                                    _dbContext.SaveChanges();

                                }
                                else
                                {

                                    _dbContext.tbdetallemovimientos.Add(new DetalleMovimientos()
                                    {
                                        int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                                        int_idespacio = response.int_id_espacio.Value,
                                        int_id_usuario_id = response.int_id_usuario_id,
                                        int_id_zona = espacio.id_zona,
                                        int_duracion = response.int_tiempo,
                                        dtm_horaInicio = response.dt_hora_inicio,
                                        dtm_horaFin = response.dtm_hora_fin,
                                        flt_importe = 0.00,
                                        flt_saldo_anterior = 0.00,
                                        flt_saldo_fin = 0.00,
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

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 2 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
