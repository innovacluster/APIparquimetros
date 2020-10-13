using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParametrosController: Controller
    {
        public readonly ApplicationDbContext context;
        public ParametrosController(ApplicationDbContext context)
        {
            this.context = context;

        }

        [HttpGet("mtdConsultarParametros")]
        public async Task<ActionResult<IEnumerable<Parametros>>> mtdConsultarParametros()
        {
            try
            {
                var response = await context.tbparametros.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message});
            }

        }

        [HttpGet("mtdConsultarParametroXId")]
        public async Task<ActionResult<Parametros>> mtdConsultarParametroXId(int id)
        {
            try
            {
                var response = await context.tbparametros.FirstOrDefaultAsync(x => x.id == id);
                if (response == null)
                {
                    return NoContent();
                }

                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarParametroXIdConcesion")]
        public async Task<ActionResult<Parametros>> mtdConsultarParametroXIdConcesion(int intIdConcesion)
        {
            try
            {
                var response = await context.tbparametros.FirstOrDefaultAsync(x => x.intidconcesion_id == intIdConcesion);
                if (response == null)
                {
                    return NoContent();
                }

                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpPost("mtdInsertarParametro")]
        public async Task<ActionResult<Parametros>> mtdIngresarParametro([FromBody] Parametros parametro)
        {
            try
            {
               
                context.tbparametros.Add(parametro);
                await context.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }

        [HttpPost("mtdModificarParametro")]
        public async Task<ActionResult<Parametros>> mtdModificarParametro(int id, [FromBody] Parametros parametro)
        {
            try
            {
                var response = await context.tbparametros.FirstOrDefaultAsync(x => x.id == id);

                if (response.id != id)
                {
                    return NotFound();
                }

                response.bolUsarNomenclaturaCajones = parametro.bolUsarNomenclaturaCajones;
                response.intTimepoAviso = parametro.intTimepoAviso;
                response.flt_Tarifa_minima = parametro.flt_Tarifa_minima;
                response.flt_intervalo_tarifa = parametro.flt_intervalo_tarifa;
                response.intidconcesion_id = parametro.intidconcesion_id;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
               return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdObtenerHora")]
       public ActionResult<DateTime> mtdObtenerHora()
      {
            try
            {
                DateTime time;
                DateTime dtmHora = DateTime.Now;
                String hora = dtmHora.ToString("HH");
                String segundo = dtmHora.ToString("ss");
                String minuto = dtmHora.ToString("mm");
                String dia = dtmHora.ToString("dd");
                String mes = dtmHora.ToString("MM");
                String year = dtmHora.ToString("yyyy");
                int horaI = Int16.Parse(hora);
                int diaI = Int16.Parse(dia);
                int mesI = Int16.Parse(mes);
                int horaCorrecta = 0;
                int diaCorrecto = 0;
                int yearI = Int16.Parse(year);

                if (horaI == 0) { horaCorrecta = 24; }
                if (horaI == 1) { horaCorrecta = 25; }
                if (horaI == 2) { horaCorrecta = 26; }
                if (horaI == 3) { horaCorrecta = 27; }
                if (horaI == 4) { horaCorrecta = 28; }
                Console.WriteLine("HoraI = " + horaI);
                horaI = horaI - 5;
                diaCorrecto = diaI;
                if (horaI < 0)
                {
                    diaI = diaI - 1;

                    if (diaI == 0)
                    {
                        if (mesI == 1) { diaCorrecto = 31; }
                        if (mesI == 2) { diaCorrecto = 28; }
                        if (mesI == 3) { diaCorrecto = 31; }
                        if (mesI == 4) { diaCorrecto = 30; }
                        if (mesI == 5) { diaCorrecto = 31; }
                        if (mesI == 6) { diaCorrecto = 30; }
                        if (mesI == 7) { diaCorrecto = 31; }
                        if (mesI == 8) { diaCorrecto = 31; }
                        if (mesI == 9) { diaCorrecto = 30; }
                        if (mesI == 10) { diaCorrecto = 31; }
                        if (mesI == 11) { diaCorrecto = 30; }
                        if (mesI == 12) { diaCorrecto = 31; }
                        mesI = mesI - 1;
                        if (mesI == 0)
                        {
                            mesI = 12;
                            yearI = yearI - 1;
                        }
                    }
                }

                //string formatoFecha = "yyyy-MM-ddTHH:mm:ss";
                horaCorrecta = horaI;
                String formatoFecha = yearI + "-";
                if (mesI < 10)
                {
                    formatoFecha = formatoFecha + "0" + mesI;
                }
                else
                {
                    formatoFecha = formatoFecha + mesI;
                }

                if (diaCorrecto < 10)
                {
                    formatoFecha = formatoFecha + "-0" + diaCorrecto;
                }
                else
                {
                    formatoFecha = formatoFecha + "-" + diaCorrecto;
                }

                if (horaCorrecta < 10)
                {
                    string h = horaCorrecta.ToString();
                    string hr = "0" + h;
                    formatoFecha = formatoFecha + " " + horaCorrecta + ":" + minuto + ":" + segundo;
                    time = DateTime.Parse(formatoFecha);
                   // DateTime oDate = DateTime.ParseExact(formatoFecha, "yyyy-MM-dd HH:mm tt", null);
                }
                else
                {
                    formatoFecha = formatoFecha + " " + horaCorrecta + ":" + minuto + ":" + segundo;
                    time = DateTime.Parse(formatoFecha);
                    //DateTime oDate = DateTime.ParseExact(formatoFecha, "yyyy-MM-dd HH:mm tt", null);
                }


                return time;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdObtenerFechaMexico")]
        public ActionResult<DateTime> mtdObtenerFechaMexico()
        {
            try
            {
                DateTime time1 = DateTime.Now;
                
                var info = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City");

                DateTimeOffset localServerTime = DateTimeOffset.Now;

                DateTime tstTime  = TimeZoneInfo.ConvertTime(time1, TimeZoneInfo.Local, info);
                DateTimeOffset utc = localServerTime.ToUniversalTime();
                return tstTime;
                //return tstTime;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdPruebaFecha2")]
        public ActionResult<object> mtdPruebaFecha2()
        {
            try
            {
                DateTime time1 = DateTime.Now;

                var timeZones = TimeZoneInfo.GetSystemTimeZones();
                return timeZones;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }


        [HttpGet("mtdPruebaFecha3")]
        public ActionResult<object> mtdPruebaFecha3()
        {
            try
            {
                DateTime time1 = DateTime.Now;

               
                return time1;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

    }
}
