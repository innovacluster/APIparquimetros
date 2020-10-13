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
    public class ResumenDiarioController : Controller
    {
        public readonly ApplicationDbContext context;

        public ResumenDiarioController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("mtdConsultarResumenDiario")]
        public async Task<ActionResult<IEnumerable<ResumenDiario>>> mtdConsultarResumenDiario()
        {
            try
            {
                var response = await context.tbresumendiario.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message});
            }
        }

        [HttpGet("mtdConsultarResumenDiarioXId")]
        public async Task<ActionResult<ResumenDiario>> mtdConsultarResumenDiarioXId(int id)
        {
            try
            {
                var response = await context.tbresumendiario.FirstOrDefaultAsync(x => x.id == id);
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarResumenDiarioXFecha")]
        public async Task<ActionResult<IEnumerable<ResumenDiario>>> mtdConsultarResumenDiarioXFecha(int intIdConcesion, DateTime dtmDia)
        {
            try
            {
                var response = await context.tbresumendiario.Where(x=> x.int_id_consecion == intIdConcesion && x.dtm_fecha.Date == dtmDia).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpPost("mtdInsertarResumenDiario")]
        public async Task<ActionResult> InsertarResumenDiario()
        {
            try
            {
                double dblTotalXDiaIos = 0;
                double dblTotalXDiaAndriod = 0;
                int intTransIos = 0;
                int intTransAndriod = 0;
                int intAutosDiaAnteriorIos = 0;
                int intAutosDiaAnteriorAndroid = 0;
                double totalIngresos = 0;
                DateTime dia = DateTime.Now;

                var concesiones = await context.tbconcesiones.ToListAsync();

                foreach (var concns in concesiones)
                {
                    //Para obtener dato de los autos del dia anterior
                    DateTime diaanterior = dia.AddDays(-1);
                    var autosDiaAnteriorIos = await context.tbresumendiario.FirstOrDefaultAsync(x => x.dtm_fecha.Date == diaanterior.Date && x.int_id_consecion == concns.id);


                    if (autosDiaAnteriorIos != null)
                    {
                        intAutosDiaAnteriorIos = autosDiaAnteriorIos.int_autos_ios;
                        intAutosDiaAnteriorAndroid = autosDiaAnteriorIos.int_autos_andriod;
                        //Para obtener los detalles de ios
                        var movIOS = await (from det in context.tbdetallemovimientos
                                            join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                            where mov.str_so == "IOS" && det.dtm_horaInicio.Date == dia.Date
                                            && mov.intidconcesion_id == concns.id
                                            select det).ToListAsync();
                        //Para obtener los detalles de andriod
                        var movAndriod = await (from det in context.tbdetallemovimientos
                                                join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                                where mov.str_so == "ANDRIOD" && det.dtm_horaInicio.Date == dia.Date
                                                && mov.intidconcesion_id == concns.id
                                                select det).ToListAsync();

                        //Para obtener las transacciones de ios
                        var movTansacciones = await (from mov in context.tbmovimientos
                                                     where mov.str_so == "IOS" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio == dia.Date
                                                     select mov).ToListAsync();

                        //Para obtener las transacciones de andriod
                        var movTansaccionesAndriod = await (from mov in context.tbmovimientos
                                                            where mov.str_so == "ANDRIOD" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio == dia.Date
                                                            select mov).ToListAsync();



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

                        //int int_porc_ios = ((intTransIos / intAutosDiaAnteriorIos) - 1);
                        //Double dec_porc_ios = ((dblTotalXDiaIos / autosDiaAnteriorIos.dec_ios) - 1);
                        //int int_porc_andriod = ((intTransAndriod / intAutosDiaAnteriorAndroid) - 1);
                        //Double dec_porc_andriod = ((dblTotalXDiaAndriod / autosDiaAnteriorIos.dec_andriod) - 1);

                        int intSumTransacciones = intTransIos + intTransAndriod;
                        Double totalDia = dblTotalXDiaIos + dblTotalXDiaAndriod;

                        context.tbresumendiario.Add(new ResumenDiario()
                        {
                            int_id_consecion = concns.id,
                            dtm_fecha = DateTime.Now,
                            int_dia = DateTime.Now.Day,
                            int_mes = DateTime.Now.Month,
                            int_anio = DateTime.Now.Year,
                            str_dia_semama = DateTime.Now.DayOfWeek.ToString(),
                            dtm_dia_anterior = autosDiaAnteriorIos.dtm_fecha,
                            str_dia_sem_ant = autosDiaAnteriorIos.str_dia_semama,
                            int_ios = intTransIos,
                            int_ant_ios = autosDiaAnteriorIos.int_ios,
                            int_por_ios = ((intTransIos / intAutosDiaAnteriorIos) - 1),
                            int_autos_ios = movTansacciones.Count,
                            int_autos_ant_ios = autosDiaAnteriorIos.int_autos_ios,
                            dec_ios = dblTotalXDiaIos,
                            dec_ant_ios = autosDiaAnteriorIos.dec_ios,
                            dec_por_ios = ((dblTotalXDiaIos / autosDiaAnteriorIos.dec_ios) - 1),
                            int_andriod = intTransAndriod,
                            int_ant_andriod = autosDiaAnteriorIos.int_andriod,
                            int_por_andriod = ((intTransAndriod / intAutosDiaAnteriorAndroid) - 1),
                            int_autos_andriod = movTansaccionesAndriod.Count,
                            int_autos_ant_andriod = autosDiaAnteriorIos.int_autos_andriod,
                            dec_andriod = dblTotalXDiaAndriod,
                            dec_ant_andriod = autosDiaAnteriorIos.dec_andriod,
                            dec_por_andriod = ((dblTotalXDiaAndriod / autosDiaAnteriorIos.dec_andriod) - 1),
                            int_total = intTransIos + intTransAndriod,
                            int_total_ant = autosDiaAnteriorIos.int_andriod + autosDiaAnteriorIos.int_ios,
                            int_por_ant_total = (((intSumTransacciones / autosDiaAnteriorIos.int_total) - 1)),
                            dec_total = dblTotalXDiaIos + dblTotalXDiaAndriod,
                            dec_total_ant = autosDiaAnteriorIos.dec_ios + autosDiaAnteriorIos.dec_andriod,
                            dec_por_ant_total = ((totalDia / autosDiaAnteriorIos.int_total) - 1)

                        });
                        context.SaveChanges();
                        intTransIos = 0;
                        intTransAndriod = 0;
                    }
                    else {

                        var movIOS = await (from det in context.tbdetallemovimientos
                                            join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                            where mov.str_so == "IOS" && det.dtm_horaInicio.Date == dia.Date
                                            && mov.intidconcesion_id == concns.id
                                            select det).ToListAsync();
                        //Para obtener los detalles de andriod
                        var movAndriod = await (from det in context.tbdetallemovimientos
                                                join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                                where mov.str_so == "ANDRIOD" && det.dtm_horaInicio.Date == dia.Date
                                                && mov.intidconcesion_id == concns.id
                                                select det).ToListAsync();

                        //Para obtener las transacciones de ios
                        var movTansacciones = await (from mov in context.tbmovimientos
                                                     where mov.str_so == "IOS" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio.Date == dia.Date
                                                     select mov).ToListAsync();

                        //Para obtener las transacciones de andriod
                        var movTansaccionesAndriod = await (from mov in context.tbmovimientos
                                                            where mov.str_so == "ANDROID" && mov.intidconcesion_id == concns.id && mov.dt_hora_inicio.Date == dia.Date
                                                            select mov).ToListAsync();



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

                        totalIngresos = intTransIos + intTransAndriod;

                        context.tbresumendiario.Add(new ResumenDiario()
                        {
                            int_id_consecion = concns.id,
                            dtm_fecha = DateTime.Now,
                            int_dia = DateTime.Now.Day,
                            int_mes = DateTime.Now.Month,
                            int_anio = DateTime.Now.Year,
                            str_dia_semama = DateTime.Now.DayOfWeek.ToString(),
                            //dtm_dia_anterior = autosDiaAnteriorIos.dtm_fecha,
                            //str_dia_sem_ant = autosDiaAnteriorIos.str_dia_semama,
                            int_ios = intTransIos,
                            int_ant_ios = 0,
                            int_por_ios = 100,
                            int_autos_ios = movTansacciones.Count,
                            int_autos_por_ios = 100,
                            int_autos_ant_ios = 0,
                            dec_ios = dblTotalXDiaIos,
                            dec_ant_ios = 0,
                            //*
                            dec_por_ios = dblTotalXDiaIos ,
                            int_andriod = intTransAndriod,
                            int_ant_andriod = 0,
                            //*
                            int_por_andriod = 100,
                            int_autos_por_andriod= 100,
                            int_total_autos = movTansacciones.Count + movTansaccionesAndriod.Count,
                            int_autos_andriod = movTansaccionesAndriod.Count,
                            int_autos_ant_andriod = 0,
                            dec_andriod = dblTotalXDiaAndriod,
                            dec_ant_andriod =0,
                            dec_por_andriod = dblTotalXDiaAndriod,
                            int_total = intTransIos + intTransAndriod,
                            int_total_ant = 0,
                            int_por_ant_total = 0,
                            dec_total = dblTotalXDiaIos + dblTotalXDiaAndriod,
                            dec_total_ant = 0,
                            dec_por_ant_total =0

                        });;
                        context.SaveChanges();

                        intTransIos = 0;
                        intTransAndriod = 0;
                    }

                }

                return Json(new { token = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }

       

        [NonAction]
        public async Task<ActionResult<Double>> mtdIngresosXDiaIos()
        {
            try
            {
                //ParametrosController par = new ParametrosController(context);
                //ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                DateTime time = DateTime.Now;
                double dblTotalXDiaIos = 0;

                var response = await (from det in context.tbdetallemovimientos
                                      join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                      where mov.str_so == "IOS" && det.dtm_horaInicio.Date == time.Date
                                      select new DetalleIngresos()
                                      {
                                          id = det.id,
                                          int_idmovimiento = det.int_idmovimiento,
                                          dt_hora_inicio = det.dtm_horaInicio,
                                          ftl_importe = det.flt_importe,
                                          flt_descuentos = det.flt_descuentos,
                                          str_so = mov.str_so,
                                          str_observaciones = det.str_observaciones

                                      }).ToListAsync();

                foreach (var item in response)
                {
                    dblTotalXDiaIos = dblTotalXDiaIos + item.ftl_importe;

                    if (item.flt_descuentos != 0)
                    {

                        string d = item.flt_descuentos.ToString();
                        double flt = double.Parse(d.Trim('-'));

                        dblTotalXDiaIos = dblTotalXDiaIos - item.flt_descuentos;
                    }

                }

                return dblTotalXDiaIos;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [NonAction]
        public async Task<ActionResult<Double>> mtdIngresosXDiaAndroid()
        {
            try
            {
                //ParametrosController par = new ParametrosController(context);
                //ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                DateTime time = DateTime.Now;
                double dblTotalXDiaAndriod = 0;

                var response = await (from det in context.tbdetallemovimientos
                                      join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                      where mov.str_so == "ANDROID" && det.dtm_horaInicio.Date == time.Date
                                      select new DetalleIngresos()
                                      {
                                          id = det.id,
                                          int_idmovimiento = det.int_idmovimiento,
                                          dt_hora_inicio = det.dtm_horaInicio,
                                          ftl_importe = det.flt_importe,
                                          flt_descuentos = det.flt_descuentos,
                                          str_so = mov.str_so,
                                          str_observaciones = det.str_observaciones

                                      }).ToListAsync();

                foreach (var item in response)
                {
                    dblTotalXDiaAndriod = dblTotalXDiaAndriod + item.ftl_importe;

                    if (item.flt_descuentos != 0)
                    {

                        string d = item.flt_descuentos.ToString();
                        double flt = double.Parse(d.Trim('-'));

                        dblTotalXDiaAndriod = dblTotalXDiaAndriod - item.flt_descuentos;
                    }


                }

                return dblTotalXDiaAndriod;

            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });

            }
        }

    }
}
