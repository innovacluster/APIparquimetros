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
    public class ReumenMensualController : Controller
    {
        public readonly ApplicationDbContext context;

        public ReumenMensualController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("mtdConsultarResumenMensual")]
        public async Task<ActionResult<IEnumerable<ResumenMensual>>> mtdConsultarResumenMensual()
        {
            try
            {
                var response = await context.tbresumenmensual.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpPost("mtdInsertarResumenMensual")]
        public async Task<ActionResult> mtdInsertarResumenMensual()
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
                    //Asi obtenemos el primer dia del mes actual
                    DateTime oPrimerDiaDelMes = new DateTime(dia.Year, intMesAnt, 1);

                    //Y de la siguiente forma obtenemos el ultimo dia del mes
                    //agregamos 1 mes al objeto anterior y restamos 1 día.
                    DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

                    var resumenMesAnterior = await context.tbresumenmensual.FirstOrDefaultAsync(x => x.str_mes == strMes && x.int_id_consecion == concns.id);


                    if (resumenMesAnterior != null)
                    {
                        intAutosSemAnteriorIos = resumenMesAnterior.int_mes_autos_ios;
                        intAutosSemAnteriorAndroid = resumenMesAnterior.int_mes_autos_andriod;

                        //var registrosDeMes = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);

                        int sumaTransMesIos = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_ios);
                        int sumaTransMesAndroid = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_andriod);
                        int sumaAutosIos = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_autos_ios);
                        int sumaAutosAndriod = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_autos_andriod);
                        double dblingrMesIOs = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_sem_ios);
                        double dblingrMesAndroid = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_sem_andriod);


                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransMesIos + sumaTransMesAndroid;
                        double dblIngTotales = dblingrMesIOs + dblingrMesAndroid;

                        context.tbresumenmensual.Add(new ResumenMensual()
                        {
                            int_id_consecion = concns.id,
                            dtm_fecha_inicio = oPrimerDiaDelMes,
                            dtm_fecha_fin = oUltimoDiaDelMes,
                            str_mes = strMes,
                            int_anio = DateTime.Now.Year,
                            //dtm_mes_anterior =
                            int_mes_ios = sumaTransMesIos,
                            int_mes_ant_ios = resumenMesAnterior.int_mes_ios,
                            int_mes_por_ios = ((sumaTransMesIos / resumenMesAnterior.int_mes_ios) - 1),
                            int_mes_autos_ios = sumaAutosIos,
                            int_mes_autos_ant_ios = resumenMesAnterior.int_mes_autos_ios,
                            int_mes_autos_por_ios = ((sumaAutosIos / resumenMesAnterior.int_mes_autos_ios) - 1),
                            dec_mes_ios = dblingrMesIOs,
                            dec_mes_ant_ios = resumenMesAnterior.dec_mes_ios,
                            dec_mes_por_ios = ((dblingrMesIOs / resumenMesAnterior.dec_mes_ios) - 1),
                            int_mes_andriod = sumaTransMesAndroid,
                            int_mes_ant_andriod = resumenMesAnterior.int_mes_andriod,
                            int_mes_por_andriod = ((sumaTransMesAndroid / resumenMesAnterior.int_mes_andriod) - 1),
                            int_mes_autos_andriod = sumaAutosAndriod,
                            int_mes_autos_ant_andriod = resumenMesAnterior.int_mes_autos_andriod,
                            int_mes_autos_por_andriod = ((sumaAutosAndriod / resumenMesAnterior.int_mes_autos_andriod) - 1),
                            int_mes_total_autos = totalAutos,
                            dec_mes_andriod = dblingrMesAndroid,
                            dec_mes_ant_andriod = resumenMesAnterior.dec_mes_andriod,
                            dec_mes_por_andriod = ((dblingrMesAndroid / resumenMesAnterior.dec_mes_andriod) - 1),
                            int_mes_total = transTotales,
                            int_mes_total_ant =  resumenMesAnterior.int_mes_total,
                            int_mes_por_total = ((transTotales / resumenMesAnterior.int_mes_total) - 1),
                            dec_mes_total = dblIngTotales,
                            dec_mes_total_ant = resumenMesAnterior.dec_mes_total,
                            dec_mes_por_total = ((dblIngTotales / resumenMesAnterior.dec_mes_total) - 1)

                        }); 
                        context.SaveChanges();
                        intAutosSemAnteriorIos = 0;
                        intAutosSemAnteriorAndroid = 0;
                    }
                    else
                    {

                        intAutosSemAnteriorIos = resumenMesAnterior.int_mes_autos_ios;
                        intAutosSemAnteriorAndroid = resumenMesAnterior.int_mes_autos_andriod;

                        //var registrosDeMes = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_ios);

                        int sumaTransMesIos = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_ios);
                        int sumaTransMesAndroid = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_andriod);
                        int sumaAutosIos = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_autos_ios);
                        int sumaAutosAndriod = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.int_sem_autos_andriod);
                        double dblingrMesIOs = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_sem_ios);
                        double dblingrMesAndroid = await context.tbresumensemanal.Where(x => x.dtm_fecha_inicio.Date >= oPrimerDiaDelMes.Date && x.dtm_fecha_fin.Date <= oUltimoDiaDelMes.Date && x.int_id_consecion == concns.id).SumAsync(i => i.dec_sem_andriod);


                        int totalAutos = sumaAutosAndriod + sumaAutosIos;
                        int transTotales = sumaTransMesIos + sumaTransMesAndroid;
                        double dblIngTotales = dblingrMesIOs + dblingrMesAndroid;

                        context.tbresumenmensual.Add(new ResumenMensual()
                        {
                            int_id_consecion = concns.id,
                            dtm_fecha_inicio = oPrimerDiaDelMes,
                            dtm_fecha_fin = oUltimoDiaDelMes,
                            str_mes = strMes,
                            int_anio = DateTime.Now.Year,
                            //dtm_mes_anterior =
                            int_mes_ios = sumaTransMesIos,
                            int_mes_ant_ios = 0,
                            int_mes_por_ios = 100,
                            int_mes_autos_ios = sumaAutosIos,
                            int_mes_autos_ant_ios = 0,
                            int_mes_autos_por_ios = 100,
                            dec_mes_ios = dblingrMesIOs,
                            dec_mes_ant_ios = 0,
                            dec_mes_por_ios =100,
                            int_mes_andriod = sumaTransMesAndroid,
                            int_mes_ant_andriod =0,
                            int_mes_por_andriod = 100,
                            int_mes_autos_andriod = sumaAutosAndriod,
                            int_mes_autos_ant_andriod =0,
                            int_mes_autos_por_andriod = 100,
                            int_mes_total_autos = totalAutos,
                            dec_mes_andriod = dblingrMesAndroid,
                            dec_mes_ant_andriod =0,
                            dec_mes_por_andriod = 100,
                            int_mes_total = transTotales,
                            int_mes_total_ant = 0,
                            int_mes_por_total =100,
                            dec_mes_total = dblIngTotales,
                            dec_mes_total_ant = 0,
                            dec_mes_por_total =100

                        });
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
