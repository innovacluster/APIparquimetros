using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Controllers;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Services
{
    public class ResumenMensualJob : IJob
    {
        private readonly IServiceScopeFactory scopeFactory;
        public ResumenMensualJob(ILogger<ResumenMensualJob> logger, IServiceScopeFactory scopeFactory)
        {

            this.scopeFactory = scopeFactory;
            //_dbContext = dbContext;
        }

        public Task Execute(IJobExecutionContext context)
        {

            _ = mtdRealizarResumenMensual();
            return Task.CompletedTask;
        }

        public async Task mtdRealizarResumenMensual()
        {
            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            int intIdMulta = 0;


            ParametrosController par = new ParametrosController(dbContext);
            ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();
            DateTime time = time1.Value;
            //DateTime time = DateTime.Now;

            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(time.Month);

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
                DateTime dia = time;
                //----------------------
                Double int_mes_por_ios = 0;
                Double int_mes_autos_por_ios = 0;
                Double dec_mes_por_ios = 0;
                Double int_mes_por_andriod = 0;
                Double int_mes_autos_por_andriod = 0;
                Double dec_mes_por_andriod = 0;
                Double int_mes_por_total = 0;
                Double dec_mes_por_total = 0;

                string mesInsertar = "";

                int sumaTransMesIos = 0;
                int sumaTransMesAndroid = 0;
                int sumaAutosIos = 0;
                int sumaAutosAndriod = 0;
                double dblingrMesIOs = 0;
                double dblingrMesAndroid = 0;

                int intMesAnt = dia.Month - 1;

                string strMes = "";
                switch (intMesAnt)
                {
                    case 1:
                        strMes = "January";
                        break;
                    case 2:
                        strMes = "February";
                        break;
                    case 3:
                        strMes = "March";
                        break;
                    case 4:
                        strMes = "April";
                        break;
                    case 5:
                        strMes = "May";
                        break;
                    case 6:
                        strMes = "June";
                        break;
                    case 7:
                        strMes = "July";
                        break;
                    case 8:
                        strMes = "August";
                        break;
                    case 9:
                        strMes = "September";
                        break;
                    case 10:
                        strMes = "October";
                        break;
                    case 11:
                        strMes = "November";
                        break;
                    case 12:
                        strMes = "December";
                        break;
                }




                intMesAnt += 1;
                //Asi obtenemos el primer dia del mes actual
                DateTime oPrimerDiaDelMes = new DateTime(dia.Year, intMesAnt, 1);

                DateTime oPrimerDiaDelMesInserta = oPrimerDiaDelMes;
                //Y de la siguiente forma obtenemos el ultimo dia del mes
                //agregamos 1 mes al objeto anterior y restamos 1 día.
                //oPrimerDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
                DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
                var concesiones = dbContext.tbconcesiones.ToList();

                foreach (var concns in concesiones)
                {
                    var resumenMesAnterior = dbContext.tbresumenmensual.FirstOrDefault(x => x.str_mes == strMes && x.int_id_consecion == concns.id);

                    if (resumenMesAnterior != null)
                    {
                        intAutosSemAnteriorIos = resumenMesAnterior.int_mes_autos_ios;
                        intAutosSemAnteriorAndroid = resumenMesAnterior.int_mes_autos_andriod;



                        int dias = oUltimoDiaDelMes.Date.Date.Day;
                        dias += 1;

                        for (int i = 1; i < dias; i++)
                        {
                            sumaTransMesIos += await dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(t => t.int_ios);
                            sumaTransMesAndroid += await dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(t => t.int_andriod);
                            sumaAutosIos += await dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(t => t.int_autos_ios);
                            sumaAutosAndriod += await dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(t => t.int_autos_andriod);
                            dblingrMesIOs += await dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(t => t.dec_ios);
                            dblingrMesAndroid += await dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date >= oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(t => t.dec_andriod);
                            oPrimerDiaDelMes = oPrimerDiaDelMes.AddDays(1);
                        }

                        //var registrosDeMes = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);

                        oPrimerDiaDelMes = oPrimerDiaDelMes.AddDays(-31);

                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransMesIos + sumaTransMesAndroid;
                        double dblIngTotales = dblingrMesIOs + dblingrMesAndroid;

                        if (resumenMesAnterior.int_mes_ios != 0)
                        {
                            int_mes_por_ios = (((double)sumaTransMesIos / (double)resumenMesAnterior.int_mes_ios) - 1);
                            int_mes_por_ios = int_mes_por_ios * 100;
                        }
                        else
                        {
                            if (sumaTransMesIos > 0)
                            {
                                int_mes_por_ios = 100;
                            }
                        }
                        if (resumenMesAnterior.int_mes_autos_ios != 0)
                        {
                            int_mes_autos_por_ios = (((double)sumaAutosIos / (double)resumenMesAnterior.int_mes_autos_ios) - 1);
                            int_mes_autos_por_ios = int_mes_autos_por_ios * 100;
                        }
                        else
                        {
                            if (sumaAutosIos > 0)
                            {
                                int_mes_autos_por_ios = 100;
                            }
                        }
                        if (resumenMesAnterior.dec_mes_ios != 0)
                        {
                            dec_mes_por_ios = ((dblingrMesIOs / resumenMesAnterior.dec_mes_ios) - 1);
                            dec_mes_por_ios = dec_mes_por_ios * 100;
                        }
                        else
                        {
                            if (dblingrMesIOs > 0)
                            {
                                dec_mes_por_ios = 100;
                            }
                        }
                        if (resumenMesAnterior.int_mes_andriod != 0)
                        {
                            int_mes_por_andriod = (((double)sumaTransMesAndroid / (double)resumenMesAnterior.int_mes_andriod) - 1);
                            int_mes_por_andriod = int_mes_por_andriod * 100;
                        }
                        else
                        {
                            if (sumaTransMesAndroid > 0)
                            {
                                int_mes_por_andriod = 100;
                            }
                        }
                        if (resumenMesAnterior.int_mes_autos_andriod != 0)
                        {
                            int_mes_autos_por_andriod = (((double)sumaAutosAndriod / (double)resumenMesAnterior.int_mes_autos_andriod) - 1);
                            int_mes_autos_por_andriod = int_mes_autos_por_andriod * 100;
                        }
                        else
                        {
                            if (sumaAutosAndriod > 0)
                            {
                                int_mes_autos_por_andriod = 100;
                            }
                        }

                        if (resumenMesAnterior.dec_mes_andriod != 0)
                        {
                            dec_mes_por_andriod = ((dblingrMesAndroid / resumenMesAnterior.dec_mes_andriod) - 1);
                            dec_mes_por_andriod = dec_mes_por_andriod * 100;

                        }
                        else
                        {
                            if (dblingrMesAndroid > 0)
                            {
                                dec_mes_por_andriod = 100;
                            }
                        }
                        if (resumenMesAnterior.int_mes_total != 0)
                        {
                            int_mes_por_total = (((double)transTotales / (double)resumenMesAnterior.int_mes_total) - 1);
                            int_mes_por_total = int_mes_por_total * 100;
                        }
                        else
                        {
                            if (transTotales > 0)
                            {
                                int_mes_por_total = 100;
                            }
                        }
                        if (resumenMesAnterior.dec_mes_total != 0)
                        {
                            dec_mes_por_total = ((dblIngTotales / resumenMesAnterior.dec_mes_total) - 1);
                            dec_mes_por_total = dec_mes_por_total * 100;
                        }
                        else
                        {
                            if (dblIngTotales > 0)
                            {
                                dec_mes_por_total = 100;
                            }
                        }

                        var strategy = dbContext.Database.CreateExecutionStrategy();

                        strategy.Execute(() =>
                        {

                            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    dbContext.tbresumenmensual.Add(new ResumenMensual()
                                    {
                                        int_id_consecion = concns.id,
                                        dtm_fecha_inicio = oPrimerDiaDelMes,
                                        dtm_fecha_fin = oUltimoDiaDelMes,
                                        str_mes = monthName,
                                        int_anio = time.Year,
                                        //dtm_mes_anterior =
                                        int_mes_ios = sumaTransMesIos,
                                        int_mes_ant_ios = resumenMesAnterior.int_mes_ios,
                                        int_mes_por_ios = int_mes_por_ios,
                                        int_mes_autos_ios = sumaAutosIos,
                                        int_mes_autos_ant_ios = resumenMesAnterior.int_mes_autos_ios,
                                        int_mes_autos_por_ios = int_mes_autos_por_ios,
                                        dec_mes_ios = dblingrMesIOs,
                                        dec_mes_ant_ios = resumenMesAnterior.dec_mes_ios,
                                        dec_mes_por_ios = dec_mes_por_ios,
                                        int_mes_andriod = sumaTransMesAndroid,
                                        int_mes_ant_andriod = resumenMesAnterior.int_mes_andriod,
                                        int_mes_por_andriod = int_mes_por_andriod,
                                        int_mes_autos_andriod = sumaAutosAndriod,
                                        int_mes_autos_ant_andriod = resumenMesAnterior.int_mes_autos_andriod,
                                        int_mes_autos_por_andriod = int_mes_autos_por_andriod,
                                        int_mes_total_autos = totalAutos,
                                        dec_mes_andriod = dblingrMesAndroid,
                                        dec_mes_ant_andriod = resumenMesAnterior.dec_mes_andriod,
                                        dec_mes_por_andriod = dec_mes_por_andriod,
                                        int_mes_total = transTotales,
                                        int_mes_total_ant = resumenMesAnterior.int_mes_total,
                                        int_mes_por_total = int_mes_por_total,
                                        dec_mes_total = dblIngTotales,
                                        dec_mes_total_ant = resumenMesAnterior.dec_mes_total,
                                        dec_mes_por_total = dec_mes_por_total

                                    });
                                    dbContext.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                                dblTotalXDiaIos = 0;
                                dblTotalXDiaAndriod = 0;
                                intTransIos = 0;
                                intTransAndriod = 0;
                                intAutosSemAnteriorIos = 0;
                                intAutosSemAnteriorAndroid = 0;
                                totalIngresos = 0;
                                intNoSemanaActual = 0;
                                dia = time;
                                //----------------------
                                int_mes_por_ios = 0;
                                int_mes_autos_por_ios = 0;
                                dec_mes_por_ios = 0;
                                int_mes_por_andriod = 0;
                                int_mes_autos_por_andriod = 0;
                                dec_mes_por_andriod = 0;
                                int_mes_por_total = 0;
                                dec_mes_por_total = 0;


                                sumaTransMesIos = 0;
                                sumaTransMesAndroid = 0;
                                sumaAutosIos = 0;
                                sumaAutosAndriod = 0;
                                dblingrMesIOs = 0;
                                dblingrMesAndroid = 0;

                            }
                        });



                    }
                    else
                    {
                        //intAutosSemAnteriorIos = resumenMesAnterior.int_mes_autos_ios;
                        //intAutosSemAnteriorAndroid = resumenMesAnterior.int_mes_autos_andriod;

                        //var registrosDeMes = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);


                        int dias = oUltimoDiaDelMes.Date.Day;
                        dias += 1;

                        for (int i = 1; i < dias; i++)
                        {
                            sumaTransMesIos += dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date == oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(t => t.int_ios);
                            sumaTransMesAndroid += dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date == oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(t => t.int_andriod);
                            sumaAutosIos += dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date == oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(t => t.int_autos_ios);
                            sumaAutosAndriod += dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date == oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(t => t.int_autos_andriod);
                            dblingrMesIOs += dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date == oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(t => t.dec_ios);
                            dblingrMesAndroid += dbContext.tbresumendiario.Where(x => x.dtm_fecha.Date == oPrimerDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(t => t.dec_andriod);
                            oPrimerDiaDelMes = oPrimerDiaDelMes.AddDays(1);
                        }

                        oPrimerDiaDelMes = oPrimerDiaDelMes.AddDays(-31);

                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransMesIos + sumaTransMesAndroid;
                        double dblIngTotales = dblingrMesIOs + dblingrMesAndroid;


                        if (sumaTransMesIos > 0)
                        {
                            int_mes_por_ios = 100;
                        }

                        if (sumaAutosIos > 0)
                        {
                            int_mes_autos_por_ios = 100;
                        }

                        if (dblingrMesIOs > 0)
                        {
                            dec_mes_por_ios = 100;
                        }

                        if (sumaTransMesAndroid > 0)
                        {
                            int_mes_por_andriod = 100;
                        }

                        if (sumaAutosAndriod > 0)
                        {
                            int_mes_autos_por_andriod = 100;
                        }

                        if (dblingrMesAndroid > 0)
                        {
                            dec_mes_por_andriod = 100;
                        }

                        if (transTotales > 0)
                        {
                            int_mes_por_total = 100;
                        }

                        if (dblIngTotales > 0)
                        {
                            dec_mes_por_total = 100;
                        }


                        var strategy = dbContext.Database.CreateExecutionStrategy();

                        strategy.Execute(() =>
                        {

                            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                            {
                                try
                                {

                                    dbContext.tbresumenmensual.Add(new ResumenMensual()
                                    {
                                        int_id_consecion = concns.id,
                                        dtm_fecha_inicio = oPrimerDiaDelMes,
                                        dtm_fecha_fin = oUltimoDiaDelMes,
                                        str_mes = monthName,
                                        int_anio = time.Year,
                                        //dtm_mes_anterior =
                                        int_mes_ios = sumaTransMesIos,
                                        int_mes_ant_ios = 0,
                                        int_mes_por_ios = int_mes_por_ios,
                                        int_mes_autos_ios = sumaAutosIos,
                                        int_mes_autos_ant_ios = 0,
                                        int_mes_autos_por_ios = int_mes_autos_por_ios,
                                        dec_mes_ios = dblingrMesIOs,
                                        dec_mes_ant_ios = 0,
                                        dec_mes_por_ios = dec_mes_por_ios,
                                        int_mes_andriod = sumaTransMesAndroid,
                                        int_mes_ant_andriod = 0,
                                        int_mes_por_andriod = int_mes_por_andriod,
                                        int_mes_autos_andriod = sumaAutosAndriod,
                                        int_mes_autos_ant_andriod = 0,
                                        int_mes_autos_por_andriod = int_mes_autos_por_andriod,
                                        int_mes_total_autos = totalAutos,
                                        dec_mes_andriod = dblingrMesAndroid,
                                        dec_mes_ant_andriod = 0,
                                        dec_mes_por_andriod = dec_mes_por_andriod,
                                        int_mes_total = transTotales,
                                        int_mes_total_ant = 0,
                                        int_mes_por_total = int_mes_por_total,
                                        dec_mes_total = dblIngTotales,
                                        dec_mes_total_ant = 0,
                                        dec_mes_por_total = dec_mes_por_total
                                    });

                                    dbContext.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        });


                        intAutosSemAnteriorIos = 0;
                        intAutosSemAnteriorAndroid = 0;

                    }

                    dblTotalXDiaIos = 0;
                    dblTotalXDiaAndriod = 0;
                    intTransIos = 0;
                    intTransAndriod = 0;
                    intAutosSemAnteriorIos = 0;
                    intAutosSemAnteriorAndroid = 0;
                    totalIngresos = 0;
                    intNoSemanaActual = 0;
                    dia = time;
                    //----------------------
                    int_mes_por_ios = 0;
                    int_mes_autos_por_ios = 0;
                    dec_mes_por_ios = 0;
                    int_mes_por_andriod = 0;
                    int_mes_autos_por_andriod = 0;
                    dec_mes_por_andriod = 0;
                    int_mes_por_total = 0;
                    dec_mes_por_total = 0;
                    sumaTransMesIos = 0;
                    sumaTransMesAndroid = 0;
                    sumaAutosIos = 0;
                    sumaAutosAndriod = 0;
                    dblingrMesIOs = 0;
                    dblingrMesAndroid = 0;
                 

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            //try
            //{
            //    double dblTotalXDiaIos = 0;
            //    double dblTotalXDiaAndriod = 0;
            //    int intTransIos = 0;
            //    int intTransAndriod = 0;
            //    int intAutosSemAnteriorIos = 0;
            //    int intAutosSemAnteriorAndroid = 0;
            //    double totalIngresos = 0;
            //    int intNoSemanaActual = 0;
            //    DateTime dia = time;
            //    ----------------------
            //    int int_mes_por_ios = 0;
            //    int int_mes_autos_por_ios = 0;
            //    Double dec_mes_por_ios = 0;
            //    int int_mes_por_andriod = 0;
            //    int int_mes_autos_por_andriod = 0;
            //    Double dec_mes_por_andriod = 0;
            //    int int_mes_por_total = 0;
            //    Double dec_mes_por_total = 0;


            //    var concesiones = dbContext.tbconcesiones.ToList();

            //    foreach (var concns in concesiones)
            //    {
            //        int intMesAnt = dia.Month - 1;

            //        string strMes = "";
            //        switch (intMesAnt)
            //        {
            //            case 1:
            //                strMes = "January";
            //                break;
            //            case 2:
            //                strMes = "February";
            //                break;
            //            case 3:
            //                strMes = "March";
            //                break;
            //            case 4:
            //                strMes = "April";
            //                break;
            //            case 5:
            //                strMes = "May";
            //                break;
            //            case 6:
            //                strMes = "June";
            //                break;
            //            case 7:
            //                strMes = "July";
            //                break;
            //            case 8:
            //                strMes = "August";
            //                break;
            //            case 9:
            //                strMes = "September";
            //                break;
            //            case 10:
            //                strMes = "October";
            //                break;
            //            case 11:
            //                strMes = "November";
            //                break;
            //            case 12:
            //                strMes = "December";
            //                break;
            //        }
            //        Asi obtenemos el primer dia del mes actual
            //        DateTime oPrimerDiaDelMes = new DateTime(dia.Year, intMesAnt, 1);

            //        Y de la siguiente forma obtenemos el ultimo dia del mes
            //        agregamos 1 mes al objeto anterior y restamos 1 día.
            //        DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            //        var resumenMesAnterior = dbContext.tbresumenmensual.FirstOrDefault(x => x.str_mes == strMes && x.int_id_consecion == concns.id);


            //        if (resumenMesAnterior != null)
            //        {
            //            intAutosSemAnteriorIos = resumenMesAnterior.int_mes_autos_ios;
            //            intAutosSemAnteriorAndroid = resumenMesAnterior.int_mes_autos_andriod;

            //            var registrosDeMes = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);

            //            int sumaTransMesIos = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_ios);
            //            int sumaTransMesAndroid = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_andriod);
            //            int sumaAutosIos = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_autos_ios);
            //            int sumaAutosAndriod = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_autos_andriod);
            //            double dblingrMesIOs = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_sem_ios);
            //            double dblingrMesAndroid = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_sem_andriod);


            //            int totalAutos = sumaAutosAndriod + sumaAutosIos;
            //            int transTotales = sumaTransMesIos + sumaTransMesAndroid;
            //            double dblIngTotales = dblingrMesIOs + dblingrMesAndroid;

            //            if (resumenMesAnterior.int_mes_ios != 0)
            //            {
            //                int_mes_por_ios = ((sumaTransMesIos / resumenMesAnterior.int_mes_ios) - 1);
            //                int_mes_por_ios = int_mes_por_ios * 100;
            //            }
            //            else
            //            {
            //                if (sumaTransMesIos > 0)
            //                {
            //                    int_mes_por_ios = 100;
            //                }
            //            }
            //            if (resumenMesAnterior.int_mes_autos_ios != 0)
            //            {
            //                int_mes_autos_por_ios = ((sumaAutosIos / resumenMesAnterior.int_mes_autos_ios) - 1);
            //                int_mes_autos_por_ios = int_mes_autos_por_ios * 100;
            //            }
            //            else
            //            {
            //                if (sumaAutosIos > 0)
            //                {
            //                    int_mes_autos_por_ios = 100;
            //                }
            //            }
            //            if (resumenMesAnterior.dec_mes_ios != 0)
            //            {
            //                dec_mes_por_ios = ((dblingrMesIOs / resumenMesAnterior.dec_mes_ios) - 1);
            //                dec_mes_por_ios = dec_mes_por_ios * 100;
            //            }
            //            else
            //            {
            //                if (dblingrMesIOs > 0)
            //                {
            //                    dec_mes_por_ios = 100;
            //                }
            //            }
            //            if (resumenMesAnterior.int_mes_andriod != 0)
            //            {
            //                int_mes_por_andriod = ((sumaTransMesAndroid / resumenMesAnterior.int_mes_andriod) - 1);
            //                int_mes_por_andriod = int_mes_por_andriod * 100;
            //            }
            //            else
            //            {
            //                if (sumaTransMesAndroid > 0)
            //                {
            //                    int_mes_por_andriod = 100;
            //                }
            //            }
            //            if (resumenMesAnterior.int_mes_autos_andriod != 0)
            //            {
            //                int_mes_autos_por_andriod = ((sumaAutosAndriod / resumenMesAnterior.int_mes_autos_andriod) - 1);
            //                int_mes_autos_por_andriod = int_mes_autos_por_andriod * 100;
            //            }
            //            else
            //            {
            //                if (sumaAutosAndriod > 0)
            //                {
            //                    int_mes_autos_por_andriod = 100;
            //                }
            //            }

            //            if (resumenMesAnterior.dec_mes_andriod != 0)
            //            {
            //                dec_mes_por_andriod = ((dblingrMesAndroid / resumenMesAnterior.dec_mes_andriod) - 1);
            //                dec_mes_por_andriod = dec_mes_por_andriod * 100;

            //            }
            //            else
            //            {
            //                if (dblingrMesAndroid > 0)
            //                {
            //                    dec_mes_por_andriod = 100;
            //                }
            //            }
            //            if (resumenMesAnterior.int_mes_total != 0)
            //            {
            //                int_mes_por_total = ((transTotales / resumenMesAnterior.int_mes_total) - 1);
            //                int_mes_por_total = int_mes_por_total * 100;
            //            }
            //            else
            //            {
            //                if (transTotales > 0)
            //                {
            //                    int_mes_por_total = 100;
            //                }
            //            }
            //            if (resumenMesAnterior.dec_mes_total != 0)
            //            {
            //                dec_mes_por_total = ((dblIngTotales / resumenMesAnterior.dec_mes_total) - 1);
            //                dec_mes_por_total = dec_mes_por_total * 100;
            //            }
            //            else
            //            {
            //                if (dblIngTotales > 0)
            //                {
            //                    dec_mes_por_total = 100;
            //                }
            //            }

            //            var strategy = dbContext.Database.CreateExecutionStrategy();

            //            strategy.Execute(() =>
            //            {

            //                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            //                {
            //                    try
            //                    {
            //                        dbContext.tbresumenmensual.Add(new ResumenMensual()
            //                        {
            //                            int_id_consecion = concns.id,
            //                            dtm_fecha_inicio = oPrimerDiaDelMes,
            //                            dtm_fecha_fin = oUltimoDiaDelMes,
            //                            str_mes = strMes,
            //                            int_anio = time.Year,
            //                            dtm_mes_anterior =
            //                            int_mes_ios = sumaTransMesIos,
            //                            int_mes_ant_ios = resumenMesAnterior.int_mes_ios,
            //                            int_mes_por_ios = int_mes_por_ios,
            //                            int_mes_autos_ios = sumaAutosIos,
            //                            int_mes_autos_ant_ios = resumenMesAnterior.int_mes_autos_ios,
            //                            int_mes_autos_por_ios = int_mes_autos_por_ios,
            //                            dec_mes_ios = dblingrMesIOs,
            //                            dec_mes_ant_ios = resumenMesAnterior.dec_mes_ios,
            //                            dec_mes_por_ios = dec_mes_por_ios,
            //                            int_mes_andriod = sumaTransMesAndroid,
            //                            int_mes_ant_andriod = resumenMesAnterior.int_mes_andriod,
            //                            int_mes_por_andriod = int_mes_por_andriod,
            //                            int_mes_autos_andriod = sumaAutosAndriod,
            //                            int_mes_autos_ant_andriod = resumenMesAnterior.int_mes_autos_andriod,
            //                            int_mes_autos_por_andriod = int_mes_autos_por_andriod,
            //                            int_mes_total_autos = totalAutos,
            //                            dec_mes_andriod = dblingrMesAndroid,
            //                            dec_mes_ant_andriod = resumenMesAnterior.dec_mes_andriod,
            //                            dec_mes_por_andriod = dec_mes_por_andriod,
            //                            int_mes_total = transTotales,
            //                            int_mes_total_ant = resumenMesAnterior.int_mes_total,
            //                            int_mes_por_total = int_mes_por_total,
            //                            dec_mes_total = dblIngTotales,
            //                            dec_mes_total_ant = resumenMesAnterior.dec_mes_total,
            //                            dec_mes_por_total = dec_mes_por_total

            //                        });
            //                        dbContext.SaveChanges();
            //                        transaction.Commit();
            //                    }
            //                    catch (Exception)
            //                    {

            //                        throw;
            //                    }
            //                    intAutosSemAnteriorIos = 0;
            //                    intAutosSemAnteriorAndroid = 0;
            //                }
            //            });



            //        }
            //        else
            //        {
            //            intAutosSemAnteriorIos = resumenMesAnterior.int_mes_autos_ios;
            //            intAutosSemAnteriorAndroid = resumenMesAnterior.int_mes_autos_andriod;

            //            var registrosDeMes = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);

            //            int sumaTransMesIos = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_ios);
            //            int sumaTransMesAndroid = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_andriod);
            //            int sumaAutosIos = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_autos_ios);
            //            int sumaAutosAndriod = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.int_sem_autos_andriod);
            //            double dblingrMesIOs = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_sem_ios);
            //            double dblingrMesAndroid = dbContext.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).Sum(i => i.dec_sem_andriod);


            //            int totalAutos = sumaAutosAndriod + sumaAutosIos;
            //            int transTotales = sumaTransMesIos + sumaTransMesAndroid;
            //            double dblIngTotales = dblingrMesIOs + dblingrMesAndroid;


            //            if (sumaTransMesIos > 0)
            //            {
            //                int_mes_por_ios = 100;
            //            }

            //            if (sumaAutosIos > 0)
            //            {
            //                int_mes_autos_por_ios = 100;
            //            }

            //            if (dblingrMesIOs > 0)
            //            {
            //                dec_mes_por_ios = 100;
            //            }

            //            if (sumaTransMesAndroid > 0)
            //            {
            //                int_mes_por_andriod = 100;
            //            }

            //            if (sumaAutosAndriod > 0)
            //            {
            //                int_mes_autos_por_andriod = 100;
            //            }

            //            if (dblingrMesAndroid > 0)
            //            {
            //                dec_mes_por_andriod = 100;
            //            }

            //            if (transTotales > 0)
            //            {
            //                int_mes_por_total = 100;
            //            }

            //            if (dblIngTotales > 0)
            //            {
            //                dec_mes_por_total = 100;
            //            }


            //            var strategy = dbContext.Database.CreateExecutionStrategy();

            //            strategy.Execute(() =>
            //            {

            //                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            //                {
            //                    try
            //                    {

            //                        dbContext.tbresumenmensual.Add(new ResumenMensual()
            //                        {
            //                            int_id_consecion = concns.id,
            //                            dtm_fecha_inicio = oPrimerDiaDelMes,
            //                            dtm_fecha_fin = oUltimoDiaDelMes,
            //                            str_mes = strMes,
            //                            int_anio = time.Year,
            //                            dtm_mes_anterior =
            //                            int_mes_ios = sumaTransMesIos,
            //                            int_mes_ant_ios = 0,
            //                            int_mes_por_ios = int_mes_por_ios,
            //                            int_mes_autos_ios = sumaAutosIos,
            //                            int_mes_autos_ant_ios = 0,
            //                            int_mes_autos_por_ios = int_mes_autos_por_ios,
            //                            dec_mes_ios = dblingrMesIOs,
            //                            dec_mes_ant_ios = 0,
            //                            dec_mes_por_ios = dec_mes_por_ios,
            //                            int_mes_andriod = sumaTransMesAndroid,
            //                            int_mes_ant_andriod = 0,
            //                            int_mes_por_andriod = int_mes_por_andriod,
            //                            int_mes_autos_andriod = sumaAutosAndriod,
            //                            int_mes_autos_ant_andriod = 0,
            //                            int_mes_autos_por_andriod = int_mes_autos_por_andriod,
            //                            int_mes_total_autos = totalAutos,
            //                            dec_mes_andriod = dblingrMesAndroid,
            //                            dec_mes_ant_andriod = 0,
            //                            dec_mes_por_andriod = dec_mes_por_andriod,
            //                            int_mes_total = transTotales,
            //                            int_mes_total_ant = 0,
            //                            int_mes_por_total = int_mes_por_total,
            //                            dec_mes_total = dblIngTotales,
            //                            dec_mes_total_ant = 0,
            //                            dec_mes_por_total = dec_mes_por_total
            //                        });

            //                        dbContext.SaveChanges();
            //                        transaction.Commit();
            //                    }
            //                    catch (Exception)
            //                    {
            //                        throw;
            //                    }
            //                }
            //            });


            //            intAutosSemAnteriorIos = 0;
            //            intAutosSemAnteriorAndroid = 0;

            //        }

            //    }


            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}








        }


    }
}
