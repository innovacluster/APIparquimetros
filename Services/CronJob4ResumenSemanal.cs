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
    public class CronJob4ResumenSemanal : CronJobService
    {
        private readonly ILogger<CronJob4ResumenSemanal> _logger;
        private readonly IServiceScopeFactory scopeFactory;

        public CronJob4ResumenSemanal(IScheduleConfig<CronJob4ResumenSemanal> config, ILogger<CronJob4ResumenSemanal> logger, IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 4 Resumen semanal starts.");
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
                int intAutosSemAnteriorIos = 0;
                int intAutosSemAnteriorAndroid = 0;
                double totalIngresos = 0;
                int intNoSemanaActual = 0;
                /////////////
                int int_sem_por_ios = 0;
                int int_sem_autos_por_ios = 0;
                Double dec_sem_por_ios = 0;
                int int_sem_por_andriod = 0;
                int int_sem_autos_por_andriod = 0;
                Double dec_sem_por_andriod = 0;
                int int_sem_por_ant = 0;
                Double dec_sem_por_total = 0;
                /////////
               // Double dec_sem_por_total = 0;


                DateTime dia = time;

                var concesiones =  dbContext.tbconcesiones.ToList();

                foreach (var concns in concesiones)
                {
                    System.Globalization.CultureInfo norwCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es");
                    System.Globalization.Calendar cal = norwCulture.Calendar;
                    intNoSemanaActual = cal.GetWeekOfYear(dia, norwCulture.DateTimeFormat.CalendarWeekRule, norwCulture.DateTimeFormat.FirstDayOfWeek);
                    int inNoSemAnterior = intNoSemanaActual - 1;
                    DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever 
                    DateTime startingDate = time;

                    //while (startingDate.DayOfWeek != weekStart)
                    //    startingDate = startingDate.AddDays(-1);

                   // DateTime previousWeekStart = startingDate.AddDays(-7);
                    DateTime previousWeekStart = startingDate;
                    DateTime previousWeekEnd = startingDate.AddDays(-5);

                    //Para obtener dato de los autos del dia anterior
                    DateTime diaanterior = dia.AddDays(-1);
                    var resumenSemAnterior = dbContext.tbresumensemanal.FirstOrDefault(x => x.int_semana == inNoSemAnterior && x.int_id_consecion == concns.id);


                    if (resumenSemAnterior != null)
                    {
                        intAutosSemAnteriorIos = resumenSemAnterior.int_sem_autos_ios;
                        intAutosSemAnteriorAndroid = resumenSemAnterior.int_sem_autos_andriod;

                        var registrosDeSem =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);

                        int sumaTransSemIos =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).Sum(i => i.int_ios);
                        int sumaTransSemAndroid =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).Sum(i => i.int_andriod);
                        int sumaAutosIos =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).Sum(i => i.int_autos_ios);
                        int sumaAutosAndriod =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).Sum(i => i.int_autos_andriod);
                        double dblingrSemaIOs =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_ios);
                        double dblingrSemaAndroid =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_andriod);


                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransSemIos + sumaTransSemAndroid;
                        double dblIngTotales = dblingrSemaIOs + dblingrSemaAndroid;
                        if (resumenSemAnterior.int_sem_ios != 0)
                        {
                            int_sem_por_ios = ((sumaTransSemIos / resumenSemAnterior.int_sem_ios) - 1);
                            int_sem_por_ios = int_sem_por_ios * 100;
                        }
                        else {
                            if (sumaTransSemIos>0)
                            {
                                int_sem_por_ios = 100; 
                            }
                        }

                        if (resumenSemAnterior.int_sem_autos_ios != 0)
                        {
                            int_sem_autos_por_ios = ((sumaAutosIos / resumenSemAnterior.int_sem_autos_ios) - 1);
                            int_sem_autos_por_ios = int_sem_autos_por_ios * 100;
                        }
                        else {
                            if (sumaAutosIos > 0)
                            {
                                int_sem_autos_por_ios = 100; 
                            }
                        }

                        if (resumenSemAnterior.dec_sem_ios != 0)
                        {
                            dec_sem_por_ios = ((dblingrSemaIOs / resumenSemAnterior.dec_sem_ios) - 1);
                            dec_sem_por_ios = dec_sem_por_ios * 100;
                        }
                        else {
                            if (dblingrSemaIOs > 0)
                            {
                                dec_sem_por_ios = 100; 
                            }
                        }
                        if (resumenSemAnterior.int_sem_andriod != 0)
                        {
                            int_sem_por_andriod = ((sumaTransSemAndroid / resumenSemAnterior.int_sem_andriod) - 1);
                            int_sem_por_andriod = int_sem_por_andriod * 100;
                        }
                        else {
                            int_sem_autos_por_andriod = ((sumaAutosIos / resumenSemAnterior.int_sem_autos_andriod) - 1);
                            int_sem_autos_por_andriod = int_sem_autos_por_andriod * 100;
                        }
                        if (resumenSemAnterior.dec_sem_andriod != 0)
                        {

                            dec_sem_por_andriod = ((dblingrSemaAndroid / resumenSemAnterior.dec_sem_andriod) - 1);
                            dec_sem_por_andriod = dec_sem_por_andriod * 100;
                        }
                        else {
                            if (dblingrSemaAndroid > 0)
                            {
                                dec_sem_por_andriod = 100; 
                            }
                        }
                        if (resumenSemAnterior.int_sem_total != 0)
                        {
                            int_sem_por_ant = ((transTotales / resumenSemAnterior.int_sem_total) - 1);
                            int_sem_por_ant = int_sem_por_ant * 100;
                        }
                        else {
                            if (transTotales > 0)
                            {
                                int_sem_por_ant = 100; 
                            }
                        }
                        if (resumenSemAnterior.dec_sem_total != 0)
                        {
                            dec_sem_por_total = ((dblIngTotales / resumenSemAnterior.dec_sem_total) - 1);
                            dec_sem_por_total = dec_sem_por_total * 100;
                        }
                        else {
                            if (dblIngTotales > 0)
                            {
                                dec_sem_por_total = 100; 
                            }
                        }

                       

                        var strategy = dbContext.Database.CreateExecutionStrategy();

                        strategy.Execute(() =>
                        {

                            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    dbContext.tbresumensemanal.Add(new ResumenSemanal()
                                    {
                                        int_id_consecion = concns.id,
                                        dtm_fecha_inicio = previousWeekStart,
                                        dtm_fecha_fin = previousWeekEnd,
                                        int_semana = intNoSemanaActual,
                                        int_anio = time.Year,
                                        int_semana_ant = resumenSemAnterior.int_semana,
                                        int_sem_ios = sumaTransSemIos,
                                        int_sem_ant_ios = resumenSemAnterior.int_sem_ios,
                                        int_sem_por_ios = int_sem_por_ios,
                                        int_sem_autos_ios = sumaAutosIos,
                                        int_sem_autos_ant_ios = resumenSemAnterior.int_sem_autos_ios,
                                        int_sem_autos_por_ios = int_sem_autos_por_ios,
                                        dec_sem_ios = dblingrSemaIOs,
                                        dec_sem_ant_ios = resumenSemAnterior.dec_sem_ios,
                                        dec_sem_por_ios = dec_sem_por_ios,
                                        int_sem_andriod = sumaTransSemAndroid,
                                        int_sem_ant_andriod = resumenSemAnterior.int_sem_andriod,
                                        int_sem_por_andriod = int_sem_por_andriod,
                                        int_sem_autos_andriod = sumaAutosAndriod,
                                        int_sem_autos_ant_andriod = resumenSemAnterior.int_sem_autos_andriod,
                                        int_sem_autos_por_andriod = int_sem_autos_por_andriod,
                                        int_sem_total_autos = totalAutos,
                                        dec_sem_andriod = dblingrSemaAndroid,
                                        dec_sem_ant_andriod = resumenSemAnterior.dec_sem_andriod,
                                        dec_sem_por_andriod = dec_sem_por_andriod,
                                        int_sem_total = transTotales,
                                        int_sem_total_ant = resumenSemAnterior.int_sem_total,
                                        int_sem_por_ant = int_sem_por_ant,
                                        dec_sem_total = dblIngTotales,
                                        dec_sem_total_ant = resumenSemAnterior.dec_sem_total,
                                        dec_sem_por_total = dec_sem_por_total


                                    });
                                    dbContext.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception)
                                {
                                    transaction.Rollback();
                                }
                            }
                        });

                        dblTotalXDiaIos = 0;
                        dblTotalXDiaAndriod = 0;
                        intTransIos = 0;
                        intTransAndriod = 0;
                        intAutosSemAnteriorIos = 0;
                        intAutosSemAnteriorAndroid = 0;
                        totalIngresos = 0;
                        intNoSemanaActual = 0;
                        /////////////
                        int_sem_por_ios = 0;
                        int_sem_autos_por_ios = 0;
                        dec_sem_por_ios = 0;
                        int_sem_por_andriod = 0;
                        int_sem_autos_por_andriod = 0;
                        dec_sem_por_andriod = 0;
                        int_sem_por_ant = 0;
                        dec_sem_por_total = 0;
                    }
                    else
                    {

                       // intAutosSemAnteriorIos = resumenSemAnterior.int_sem_autos_ios;
                        //intAutosSemAnteriorAndroid = resumenSemAnterior.int_sem_autos_andriod;

                        var registrosDeSem = dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekEnd.Date && x.dtm_fecha.Date <= previousWeekStart.Date && x.int_id_consecion == concns.id).Sum(i => i.int_ios);

                        int sumaTransSemIos =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekEnd.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).Sum(i => i.int_ios);
                        int sumaTransSemAndroid =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekEnd.Date && x.dtm_fecha.Date <= previousWeekStart.Date && x.int_id_consecion == concns.id).Sum(i => i.int_andriod);
                        int sumaAutosIos =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekEnd.Date && x.dtm_fecha.Date <= previousWeekStart.Date && x.int_id_consecion == concns.id).Sum(i => i.int_autos_ios);
                        int sumaAutosAndriod =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekEnd.Date && x.dtm_fecha.Date <= previousWeekStart.Date && x.int_id_consecion == concns.id).Sum(i => i.int_autos_andriod);
                        double dblingrSemaIOs =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekEnd.Date && x.dtm_fecha.Date <= previousWeekStart.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_ios);
                        double dblingrSemaAndroid =  dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekEnd.Date && x.dtm_fecha.Date <= previousWeekStart.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_andriod);


                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransSemIos + sumaTransSemAndroid;
                        double dblIngTotales = dblingrSemaIOs + dblingrSemaAndroid;

                       
                            if (sumaTransSemIos > 0)
                            {
                                int_sem_por_ios = 100;
                            }
                        
                            if (sumaAutosIos > 0)
                            {
                                int_sem_autos_por_ios = 100;
                            }
                      
                            if (dblingrSemaIOs > 0)
                            {
                                dec_sem_por_ios = 100;
                            }

                            if (sumaTransSemAndroid > 0)
                            {
                                int_sem_por_andriod = 100;
                            }

                             if (sumaAutosIos>0)
                            {
                                int_sem_autos_por_andriod = 100;
                            }

                            if (dblingrSemaAndroid > 0)
                            {
                                dec_sem_por_andriod = 100;
                            }
                      
                            if (transTotales > 0)
                            {
                                int_sem_por_ant = 100;
                            }
                        
                            if (dblIngTotales > 0)
                            {
                                dec_sem_por_total = 100;
                            }


                        var strategy = dbContext.Database.CreateExecutionStrategy();

                        strategy.Execute(() =>
                        {

                            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    dbContext.tbresumensemanal.Add(new ResumenSemanal()
                                    {
                                        int_id_consecion = concns.id,
                                        dtm_fecha_inicio = previousWeekEnd,
                                        dtm_fecha_fin = previousWeekStart,
                                        int_semana = intNoSemanaActual,
                                        int_anio = time.Year,
                                        int_semana_ant = inNoSemAnterior,
                                        int_sem_ios = sumaTransSemIos,
                                        int_sem_ant_ios = 0,
                                        int_sem_por_ios = int_sem_por_ios,
                                        int_sem_autos_ios = sumaAutosIos,
                                        int_sem_autos_ant_ios = 0,
                                        int_sem_autos_por_ios = int_sem_autos_por_ios,
                                        dec_sem_ios = dblingrSemaIOs,
                                        dec_sem_ant_ios = 0.00,
                                        dec_sem_por_ios = dec_sem_por_ios,
                                        int_sem_andriod = sumaTransSemAndroid,
                                        int_sem_ant_andriod = 0,
                                        int_sem_por_andriod = int_sem_por_andriod,
                                        int_sem_autos_andriod = sumaAutosAndriod,
                                        int_sem_autos_ant_andriod = 0,
                                        int_sem_autos_por_andriod = int_sem_autos_por_andriod,
                                        int_sem_total_autos = totalAutos,
                                        dec_sem_andriod = dblingrSemaAndroid,
                                        dec_sem_ant_andriod = 0.00,
                                        dec_sem_por_andriod = dec_sem_por_andriod,
                                        int_sem_total = transTotales,
                                        int_sem_total_ant = 0,
                                        int_sem_por_ant = int_sem_por_ant,
                                        dec_sem_total = dblIngTotales,
                                        dec_sem_total_ant = 0.00,
                                        dec_sem_por_total = dec_sem_por_total,


                                    }); ;
                                    dbContext.SaveChanges();
                                    transaction.Commit();

                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                        });


                        dblTotalXDiaIos = 0;
                        dblTotalXDiaAndriod = 0;
                        intTransIos = 0;
                        intTransAndriod = 0;
                        intAutosSemAnteriorIos = 0;
                        intAutosSemAnteriorAndroid = 0;
                        totalIngresos = 0;
                        intNoSemanaActual = 0;
                        /////////////
                        int_sem_por_ios = 0;
                        int_sem_autos_por_ios = 0;
                        dec_sem_por_ios = 0;
                        int_sem_por_andriod = 0;
                        int_sem_autos_por_andriod = 0;
                        dec_sem_por_andriod = 0;
                        int_sem_por_ant = 0;
                        dec_sem_por_total = 0;

                    }

                }

                

            }
            catch (Exception ex)
            {
                
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
