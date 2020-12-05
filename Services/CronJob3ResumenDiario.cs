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
    public class CronJob3ResumenDiario : CronJobService
    {
        private readonly ILogger<CronJob3ResumenDiario> _logger;
        private readonly IServiceScopeFactory scopeFactory;

        public CronJob3ResumenDiario(IScheduleConfig<CronJob3ResumenDiario> config, ILogger<CronJob3ResumenDiario> logger, IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob Resumen Diario starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            int intIdMulta = 0;
            ParametrosController par = new ParametrosController(dbContext);
            ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();
            DateTime time = time1.Value;
            //DateTime time = DateTime.Now;

            try
            {

                double dblTotalXDiaIos = 0;
                double dblTotalXDiaAndriod = 0;
                int intTransIos = 0;
                int intTransAndriod = 0;
                int intAutosDiaAnteriorIos = 0;
                int intAutosDiaAnteriorAndroid = 0;
                double totalIngresos = 0;
               // DateTime dia = DateTime.Now;

                ///////////////////

                int int_porc_ios = 0;
                Double dec_porc_ios = 0;
                int int_porc_andriod = 0;
                Double dec_porc_andriod = 0;
                int int_por_ant_total = 0;
                Double dec_por_ant_total = 0;

                ///////
                int int_autos_por_andriod = 0;
                int int_autos_por_ios = 0;


                var concesiones = dbContext.tbconcesiones.ToList();

                foreach (var concns in concesiones)
                {
                    //Para obtener dato de los autos del dia anterior
                    DateTime diaanterior = time.AddDays(-1);
                    var autosDiaAnteriorIos =  dbContext.tbresumendiario.FirstOrDefault(x => x.dtm_fecha.Date == diaanterior.Date && x.int_id_consecion == concns.id);


                    if (autosDiaAnteriorIos != null)
                    {
                        intAutosDiaAnteriorIos = autosDiaAnteriorIos.int_autos_ios;
                        intAutosDiaAnteriorAndroid = autosDiaAnteriorIos.int_autos_andriod;
                        //Para obtener los detalles de ios
                        var movIOS = (from det in dbContext.tbdetallemovimientos
                                      join mov in dbContext.tbmovimientos on det.int_idmovimiento equals mov.id
                                      where mov.str_so == "IOS" && det.dtm_horaInicio.Date == time.Date
                                      && mov.intidconcesion_id == concns.id
                                      select det).ToList();
                        //Para obtener los detalles de andriod
                        var movAndriod = (from det in dbContext.tbdetallemovimientos
                                          join mov in dbContext.tbmovimientos on det.int_idmovimiento equals mov.id
                                          where mov.str_so == "ANDROID" && det.dtm_horaInicio.Date == time.Date
                                          && mov.intidconcesion_id == concns.id
                                          select det).ToList();

                        //Para obtener las transacciones de ios
                        var movTansacciones = (from mov in dbContext.tbmovimientos
                                               where mov.str_so == "IOS" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio.Date == time.Date
                                               select mov).ToList();

                        //Para obtener las transacciones de andriod
                        var movTansaccionesAndriod = (from mov in dbContext.tbmovimientos
                                                      where mov.str_so == "ANDROID" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio.Date == time.Date
                                                      select mov).ToList();

                        foreach (var item in movIOS)
                        {
                            dblTotalXDiaIos = dblTotalXDiaIos + item.flt_importe;

                            if (item.flt_descuentos != 0)
                            {
                                string d = item.flt_descuentos.ToString();
                                double flt = double.Parse(d.Trim('-'));

                                dblTotalXDiaIos = dblTotalXDiaIos - flt;
                            }
                            if (item.str_observaciones != "DESAPARCADO")
                            {
                                intTransIos++;
                            }
                        }

                        foreach (var item in movAndriod)
                        {
                            dblTotalXDiaAndriod = dblTotalXDiaAndriod + item.flt_importe;

                            if (item.flt_descuentos != 0)
                            {
                                string d = item.flt_descuentos.ToString();
                                double flt = double.Parse(d.Trim('-'));

                                dblTotalXDiaAndriod = dblTotalXDiaAndriod - flt;
                            }
                            if (item.str_observaciones != "DESAPARCADO")
                            {
                                intTransAndriod++;
                            }
                        }

                        int intSumTransacciones = intTransIos + intTransAndriod;
                        Double totalDia = dblTotalXDiaIos + dblTotalXDiaAndriod;


                        if (intAutosDiaAnteriorIos != 0)
                        {
                            int_porc_ios = ((intTransIos / intAutosDiaAnteriorIos) - 1);
                            int_porc_ios = int_porc_ios * 100;
                        }
                        else
                        {
                            if (intTransIos > 0)
                            {
                                int_porc_ios = 100;
                            }
                           
                        }
                        if (autosDiaAnteriorIos.dec_ios != 0)
                        {
                            dec_porc_ios = ((dblTotalXDiaIos / autosDiaAnteriorIos.dec_ios) - 1);
                            dec_porc_ios = dec_porc_ios * 100;
                        }
                        else
                        {
                            if (dblTotalXDiaIos>0)
                            {
                                dec_porc_ios = 100; 
                            }
                        }
                        if (intAutosDiaAnteriorAndroid != 0)
                        {
                            int_porc_andriod = ((intTransAndriod / intAutosDiaAnteriorAndroid) - 1);
                        }
                        else
                        {
                            if (intTransAndriod > 0)
                            {
                                int_porc_andriod = 100; 
                            }
                        }

                        if (autosDiaAnteriorIos.dec_andriod != 0)
                        {
                            dec_porc_andriod = ((dblTotalXDiaAndriod / autosDiaAnteriorIos.dec_andriod) - 1);
                            dec_porc_andriod = dec_porc_andriod * 100;
                        }
                        else
                        {
                            if (dblTotalXDiaAndriod > 0)
                            {
                                dec_porc_andriod = 100; 
                            }
                        }

                        if (autosDiaAnteriorIos.int_total != 0)
                        {
                            int_por_ant_total = (((intSumTransacciones / autosDiaAnteriorIos.int_total) - 1));
                            int_por_ant_total = int_por_ant_total * 100;
                        }
                        else
                        {
                            if (intSumTransacciones>0)
                            {
                                int_por_ant_total = 100; 
                            }
                        }

                        if (autosDiaAnteriorIos.int_total != 0)
                        {
                            dec_por_ant_total = ((totalDia / autosDiaAnteriorIos.int_total) - 1);

                            dec_por_ant_total = dec_por_ant_total * 100;
                        }
                        else
                        {
                            if (totalDia>0)
                            {
                                dec_por_ant_total = 100; 
                            }
                        }
                        if (autosDiaAnteriorIos.int_autos_andriod != 0)
                        {
                            int_autos_por_andriod = ((movTansaccionesAndriod.Count / autosDiaAnteriorIos.int_autos_andriod) - 1);
                            int_autos_por_andriod = int_autos_por_andriod * 100;
                        }
                        if (autosDiaAnteriorIos.int_autos_ios != 0)
                        {
                            int_autos_por_ios = ((movTansacciones.Count / autosDiaAnteriorIos.int_autos_ios) - 1);
                            int_autos_por_ios = int_autos_por_ios * 100;
                        }


                        var strategy = dbContext.Database.CreateExecutionStrategy();
                        strategy.Execute(() =>
                        {

                            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    dbContext.tbresumendiario.Add(new ResumenDiario()
                                    {
                                        int_id_consecion = concns.id,
                                        dtm_fecha = time,
                                        int_dia = time.Day,
                                        int_mes = time.Month,
                                        int_anio =time.Year,
                                        str_dia_semama = time.DayOfWeek.ToString(),
                                        dtm_dia_anterior = autosDiaAnteriorIos.dtm_fecha,
                                        str_dia_sem_ant = autosDiaAnteriorIos.str_dia_semama,
                                        int_ios = intTransIos,
                                        int_ant_ios = autosDiaAnteriorIos.int_ios,
                                        int_por_ios = int_porc_ios,
                                        int_autos_ios = movTansacciones.Count,
                                        int_autos_ant_ios = autosDiaAnteriorIos.int_autos_ios,
                                        dec_ios = dblTotalXDiaIos,
                                        dec_ant_ios = autosDiaAnteriorIos.dec_ios,
                                        dec_por_ios = dec_porc_ios,
                                        int_andriod = intTransAndriod,
                                        int_ant_andriod = autosDiaAnteriorIos.int_andriod,
                                        int_por_andriod = int_porc_andriod,
                                        int_autos_andriod = movTansaccionesAndriod.Count,
                                        int_autos_ant_andriod = autosDiaAnteriorIos.int_autos_andriod,
                                        dec_andriod = dblTotalXDiaAndriod,
                                        dec_ant_andriod = autosDiaAnteriorIos.dec_andriod,
                                        dec_por_andriod = dec_porc_andriod,
                                        int_total = intTransIos + intTransAndriod,
                                        int_total_ant = autosDiaAnteriorIos.int_andriod + autosDiaAnteriorIos.int_ios,
                                        int_por_ant_total = int_por_ant_total,
                                        dec_total = dblTotalXDiaIos + dblTotalXDiaAndriod,
                                        dec_total_ant = autosDiaAnteriorIos.dec_ios + autosDiaAnteriorIos.dec_andriod,
                                        dec_por_ant_total = dec_por_ant_total,
                                        int_total_autos = movTansacciones.Count + movTansaccionesAndriod.Count,
                                        int_autos_por_andriod = int_autos_por_andriod,
                                        int_autos_por_ios = int_autos_por_ios

                                    });
                                    dbContext.SaveChanges();
                                    transaction.Commit();
                                    intTransIos = 0;
                                    intTransAndriod = 0;
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

                        var movIOS = (from det in dbContext.tbdetallemovimientos
                                      join mov in dbContext.tbmovimientos on det.int_idmovimiento equals mov.id
                                      where mov.str_so == "IOS" && det.dtm_horaInicio.Date == time.Date
                                      && mov.intidconcesion_id == concns.id
                                      select det).ToList();
                        //Para obtener los detalles de andriod
                        var movAndriod = (from det in dbContext.tbdetallemovimientos
                                          join mov in dbContext.tbmovimientos on det.int_idmovimiento equals mov.id
                                          where mov.str_so == "ANDROID" && det.dtm_horaInicio.Date == time.Date
                                          && mov.intidconcesion_id == concns.id
                                          select det).ToList();

                        //Para obtener las transacciones de ios
                        var movTansacciones = (from mov in dbContext.tbmovimientos
                                               where mov.str_so == "IOS" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio.Date == time.Date
                                               select mov).ToList();

                        //Para obtener las transacciones de andriod
                        var movTansaccionesAndriod = (from mov in dbContext.tbmovimientos
                                                      where mov.str_so == "ANDROID" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio.Date == time.Date
                                                      select mov).ToList();



                        foreach (var item in movIOS)
                        {
                            dblTotalXDiaIos = dblTotalXDiaIos + item.flt_importe;

                            if (item.flt_descuentos != 0)
                            {
                                string d = item.flt_descuentos.ToString();
                                double flt = double.Parse(d.Trim('-'));

                                dblTotalXDiaIos = dblTotalXDiaIos - item.flt_descuentos;
                            }
                            if (item.str_observaciones != "DESAPARCADO")
                            {
                                intTransIos++;
                            }
                        }

                        foreach (var item in movAndriod)
                        {
                            dblTotalXDiaAndriod = dblTotalXDiaAndriod + item.flt_importe;

                            if (item.flt_descuentos != 0)
                            {
                                string d = item.flt_descuentos.ToString();
                                double flt = double.Parse(d.Trim('-'));

                                dblTotalXDiaAndriod = dblTotalXDiaAndriod - item.flt_descuentos;
                            }
                            if (item.str_observaciones != "DESAPARCADO")
                            {
                                intTransAndriod++;
                            }
                        }

                        Double totalDia = dblTotalXDiaIos + dblTotalXDiaAndriod;
                        int intSumTransacciones = intTransIos + intTransAndriod;
                        DateTime t = time.AddDays(-1);


                      
                            if (intTransIos > 0)
                            {
                                int_porc_ios = 100;
                            }

                        
                       
                            if (dblTotalXDiaIos > 0)
                            {
                                dec_porc_ios = 100;
                            }
                        
                       
                            if (intTransAndriod > 0)
                            {
                                int_porc_andriod = 100;
                            }
                        

                      
                            if (dblTotalXDiaAndriod > 0)
                            {
                                dec_porc_andriod = 100;
                            }
                        

                       
                            if (intSumTransacciones > 0)
                            {
                                int_por_ant_total = 100;
                            }
                        

                      
                            if (totalDia > 0)
                            {
                                dec_por_ant_total = 100;
                            }
                       
                     
                        if (movTansaccionesAndriod.Count > 0)
                        {
                            int_autos_por_andriod = 100;
                        }

                        if (movTansacciones.Count > 0)
                        {
                            int_autos_por_ios = 100;
                        }


                        var strategy = dbContext.Database.CreateExecutionStrategy();
                        strategy.Execute(() =>
                        {

                            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    dbContext.tbresumendiario.Add(new ResumenDiario()
                                    {
                                        int_id_consecion = concns.id,
                                        dtm_fecha = time,
                                        int_dia = time.Day,
                                        int_mes = time.Month,
                                        int_anio = time.Year,
                                        str_dia_semama = time.DayOfWeek.ToString(),
                                        //Aqui falta
                                        dtm_dia_anterior = time.AddDays(-1),
                                        str_dia_sem_ant =t.DayOfWeek.ToString(),
                                        int_ios = intTransIos,
                                        int_ant_ios = 0,
                                        int_por_ios = int_porc_ios,
                                        int_autos_ios = movTansacciones.Count,
                                        int_autos_por_ios = int_autos_por_ios,
                                        int_autos_ant_ios = 0,
                                        dec_ios = dblTotalXDiaIos,
                                        dec_ant_ios = 0,
                                        //*
                                        dec_por_ios = dblTotalXDiaIos,
                                        int_andriod = intTransAndriod,
                                        int_ant_andriod = 0,
                                        //*
                                        int_por_andriod = int_porc_andriod,
                                        int_autos_por_andriod = int_autos_por_andriod,
                                        int_total_autos = movTansacciones.Count + movTansaccionesAndriod.Count,
                                        int_autos_andriod = movTansaccionesAndriod.Count,
                                        int_autos_ant_andriod = 0,
                                        dec_andriod = dblTotalXDiaAndriod,
                                        dec_ant_andriod = 0,
                                        dec_por_andriod = dblTotalXDiaAndriod,
                                        int_total = intTransIos + intTransAndriod,
                                        int_total_ant = 0,
                                        int_por_ant_total = 0,
                                        dec_total = dblTotalXDiaIos + dblTotalXDiaAndriod,
                                        dec_total_ant = 0,
                                        dec_por_ant_total = 0

                                    }); ;
                                    dbContext.SaveChanges();
                                    transaction.Commit();
                                    intTransIos = 0;
                                    intTransAndriod = 0;
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
            catch (Exception ex)
            {
                throw;
            }





        


            return Task.CompletedTask;

        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 3 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
