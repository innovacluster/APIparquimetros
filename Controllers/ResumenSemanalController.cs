using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ResumenSemanalController : Controller
    {
        public readonly ApplicationDbContext context;

        public ResumenSemanalController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("mtdConsultarResumenSemanal")]
        public async Task<ActionResult<IEnumerable<ResumenSemanal>>> mtdConsultarResumenDiario()
        {
            try
            {
                var response = await context.tbresumensemanal.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpPost("mtdInsertarResumenSemanal")]
        public async Task<ActionResult> mtdInsertarResumenSemanal()
        {
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
                DateTime dia = DateTime.Now;

                var concesiones = await context.tbconcesiones.ToListAsync();

                foreach (var concns in concesiones)
                {
                    System.Globalization.CultureInfo norwCulture =System.Globalization.CultureInfo.CreateSpecificCulture("es");
                    System.Globalization.Calendar cal = norwCulture.Calendar;
                    intNoSemanaActual  = cal.GetWeekOfYear(dia, norwCulture.DateTimeFormat.CalendarWeekRule, norwCulture.DateTimeFormat.FirstDayOfWeek);
                    int inNoSemAnterior = intNoSemanaActual - 1;
                    DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever 
                    DateTime startingDate = DateTime.Today;

                    while (startingDate.DayOfWeek != weekStart)
                        startingDate = startingDate.AddDays(-1);

                    DateTime previousWeekStart = startingDate.AddDays(-7);
                    DateTime previousWeekEnd = startingDate.AddDays(-2);

                    //Para obtener dato de los autos del dia anterior
                    DateTime diaanterior = dia.AddDays(-1);
                    var resumenSemAnterior = await context.tbresumensemanal.FirstOrDefaultAsync(x => x.int_semana == inNoSemAnterior && x.int_id_consecion == concns.id);


                    if (resumenSemAnterior != null)
                    {
                        intAutosSemAnteriorIos = resumenSemAnterior.int_sem_autos_ios;
                        intAutosSemAnteriorAndroid = resumenSemAnterior.int_sem_autos_andriod;

                        var registrosDeSem = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i=> i.int_ios);

                        int sumaTransSemIos = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);
                        int sumaTransSemAndroid = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_andriod);
                        int sumaAutosIos= await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_autos_ios);
                        int sumaAutosAndriod = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_autos_andriod);
                        double dblingrSemaIOs= await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_ios);
                        double dblingrSemaAndroid = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_andriod);


                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransSemIos + sumaTransSemAndroid;
                        double dblIngTotales = dblingrSemaIOs + dblingrSemaAndroid;
                        context.tbresumensemanal.Add(new ResumenSemanal()
                        {
                            int_id_consecion = concns.id,
                            dtm_fecha_inicio = previousWeekStart,
                            dtm_fecha_fin = previousWeekEnd,
                            int_semana = intNoSemanaActual,
                            int_anio = DateTime.Now.Year,
                            int_semana_ant = resumenSemAnterior.int_semana,
                            int_sem_ios = sumaTransSemIos,
                            int_sem_ant_ios = resumenSemAnterior.int_sem_ios,
                            int_sem_por_ios = ((sumaTransSemIos / resumenSemAnterior.int_sem_ios) - 1),
                            int_sem_autos_ios = sumaAutosIos,
                            int_sem_autos_ant_ios = resumenSemAnterior.int_sem_autos_ios,
                            int_sem_autos_por_ios = ((sumaAutosIos / resumenSemAnterior.int_sem_autos_ios) - 1),
                            dec_sem_ios = dblingrSemaIOs,
                            dec_sem_ant_ios = resumenSemAnterior.dec_sem_ios,
                            dec_sem_por_ios= ((dblingrSemaIOs / resumenSemAnterior.dec_sem_ios) - 1),
                            int_sem_andriod = sumaTransSemAndroid,
                            int_sem_ant_andriod = resumenSemAnterior.int_sem_andriod,
                            int_sem_por_andriod= ((sumaTransSemAndroid / resumenSemAnterior.int_sem_andriod) - 1),
                            int_sem_autos_andriod = sumaAutosAndriod,
                            int_sem_autos_ant_andriod = resumenSemAnterior.int_sem_autos_andriod,
                            int_sem_autos_por_andriod = ((sumaAutosIos / resumenSemAnterior.int_sem_autos_andriod) - 1),
                            int_sem_total_autos = totalAutos,
                            dec_sem_andriod = dblingrSemaAndroid,
                            dec_sem_ant_andriod = resumenSemAnterior.dec_sem_andriod,
                            dec_sem_por_andriod = ((dblingrSemaAndroid / resumenSemAnterior.dec_sem_andriod) - 1),
                            int_sem_total = transTotales,
                            int_sem_total_ant = resumenSemAnterior.int_sem_total,
                            int_sem_por_ant = ((transTotales / resumenSemAnterior.int_sem_total) - 1),
                            dec_sem_total = dblIngTotales,
                            dec_sem_total_ant = resumenSemAnterior.dec_sem_total,
                            dec_sem_por_total = ((dblIngTotales / resumenSemAnterior.dec_sem_total) - 1),


                        });;
                        context.SaveChanges();
                         intAutosSemAnteriorIos = 0;
                         intAutosSemAnteriorAndroid = 0;
                    }
                    else
                    {

                        intAutosSemAnteriorIos = resumenSemAnterior.int_sem_autos_ios;
                        intAutosSemAnteriorAndroid = resumenSemAnterior.int_sem_autos_andriod;

                        var registrosDeSem = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);

                        int sumaTransSemIos = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);
                        int sumaTransSemAndroid = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_andriod);
                        int sumaAutosIos = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_autos_ios);
                        int sumaAutosAndriod = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_autos_andriod);
                        double dblingrSemaIOs = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_ios);
                        double dblingrSemaAndroid = await context.tbresumendiario.Where(x => x.dtm_fecha.Date >= previousWeekStart.Date && x.dtm_fecha.Date <= previousWeekEnd.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_andriod);


                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransSemIos + sumaTransSemAndroid;
                        double dblIngTotales = dblingrSemaIOs + dblingrSemaAndroid;
                        context.tbresumensemanal.Add(new ResumenSemanal()
                        {
                            int_id_consecion = concns.id,
                            dtm_fecha_inicio = previousWeekStart,
                            dtm_fecha_fin = previousWeekEnd,
                            int_semana = intNoSemanaActual,
                            int_anio = DateTime.Now.Year,
                            int_semana_ant = inNoSemAnterior,
                            int_sem_ios = sumaTransSemIos,
                            int_sem_ant_ios = 0,
                            int_sem_por_ios = 100,
                            int_sem_autos_ios = sumaAutosIos,
                            int_sem_autos_ant_ios = 0,
                            int_sem_autos_por_ios = 100,
                            dec_sem_ios = dblingrSemaIOs,
                            dec_sem_ant_ios =0,
                            dec_sem_por_ios = 100,
                            int_sem_andriod = sumaTransSemAndroid,
                            int_sem_ant_andriod = 0,
                            int_sem_por_andriod = 100,
                            int_sem_autos_andriod = sumaAutosAndriod,
                            int_sem_autos_ant_andriod = 0,
                            int_sem_autos_por_andriod =100,
                            int_sem_total_autos = totalAutos,
                            dec_sem_andriod = dblingrSemaAndroid,
                            dec_sem_ant_andriod = 0,
                            dec_sem_por_andriod = 100,
                            int_sem_total = transTotales,
                            int_sem_total_ant = 0,
                            int_sem_por_ant = 100,
                            dec_sem_total = dblIngTotales,
                            dec_sem_total_ant = resumenSemAnterior.dec_sem_total,
                            dec_sem_por_total = 100,


                        }); ;
                        context.SaveChanges();
                        intAutosSemAnteriorIos = 0;
                        intAutosSemAnteriorAndroid = 0;

                    }

                }

                return Json(new { token = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }
    }
}
