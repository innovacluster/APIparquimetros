using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;
using WebApiParquimetros.Services;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MultasController : Controller
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ApplicationDbContext context;
        private readonly IEmailSender _emailSender;
       // private static readonly string UrlFecha = "https://www.jobtool.online/restapis/servicioEdadGenero/post.php?opcion=30";
        private static readonly HttpClient client = new HttpClient();
        private BackgroundWorker worker;
       
        public MultasController(ApplicationDbContext context, IEmailSender emailSender)
        {
            this.context = context;
            _emailSender = emailSender;

        }

        [HttpGet("mtdConsultarMultas")]
        public async Task<ActionResult<IEnumerable<Multas>>> mtdConsultarMultas()
        {
            try
            {
                var response = await context.tbmultas.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarMultasXId")]
        public async Task<ActionResult<Multas>> mtdConsultarMultasXId(int id)
        {
            try
            {
                var response = await context.tbmultas.FirstOrDefaultAsync(x => x.id == id);
                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarMultasXIdConcesion")]
        public async Task<ActionResult<Multas>> mtdConsultarMultasXIdConcesion(int intIdConcesion)
        {
            try
            {
                var response = await context.tbmultas.FirstOrDefaultAsync(x => x.intidconcesion_id == intIdConcesion);
                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarMultasXFechas")]
        public async Task<ActionResult<IEnumerable<Multas>>> mtdConsultarMultasXFechas(DateTime dtmFechaInicio, DateTime? dtmFechaFin, int intIdConcesion)
        {
            try
            {
                if (dtmFechaFin == null)
                {
                    var response = await context.tbmultas.Where(x => x.dtm_fecha.Date == dtmFechaInicio.Date && x.intidconcesion_id == intIdConcesion).ToListAsync();
                    if (response == null)
                    {
                        return NotFound();
                    }
                    return response;
                }
                else {

                    var response = await context.tbmultas.Where(x => x.dtm_fecha >= dtmFechaInicio.Date && x.dtm_fecha.Date <= dtmFechaFin.Value.Date && x.intidconcesion_id == intIdConcesion).ToListAsync();
                    if (response == null)
                    {
                        return NotFound();
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdMultas")]
        public async Task<IEnumerable<Movimientos>> mtdMultas()
        {
            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();

            DateTime time = DateTime.Now;


            var movimientos = await context.tbmovimientos.Where(x=> x.dtm_hora_fin.Date == time.Date && x.str_comentarios == "APARCADO").ToListAsync();
            return movimientos;

        }

        [HttpPost("mtdIngresarMulta")]
        public async Task<ActionResult<Multas>> mtdIngresarMulta([FromBody] Multas multas)
        {
            //DateTime time = DateTime.Now;
            //var zonaHorariaWindows = "Central Standard Time (Mexico)";
            //var fechaHoraConversion = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, zonaHorariaWindows);
            //ParametrosController par = new ParametrosController(context);
            //ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();


            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();

            DateTime time = DateTime.Now;


            string strResult = "";
            int intIdMulta = 0;

            multas.created_date = time;
            multas.created_by = multas.created_by.ToString();
            multas.last_modified_by = multas.created_by;


            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        multas.dtm_fecha = time;
                        multas.bit_status = true;
                        multas.str_tipo_multa = "MULTA AUTOMATICA";
                        context.tbmultas.Add(multas);
                        await context.SaveChangesAsync();
                        intIdMulta = multas.id;

                        var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multas.int_id_movimiento_id);
                        response.str_comentarios = "MULTA";
                        response.boolean_multa = true;
                        response.int_id_multa = intIdMulta;
                        await context.SaveChangesAsync();

                        var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id_zona == response.int_id_espacio);
                        if (response.int_id_espacio == null)
                        {
                            context.tbdetallemovimientos.Add(new DetalleMovimientos()
                            {
                                int_idmovimiento = multas.int_id_movimiento_id.Value,
                                int_id_usuario_id = response.int_id_usuario_id,
                                int_duracion = response.int_tiempo,
                                dtm_horaInicio = time,
                                dtm_horaFin = time,
                                flt_importe = 0.0,
                                flt_saldo_anterior = 0.0,
                                flt_saldo_fin = 0.0,
                                str_observaciones = response.str_comentarios

                            });
                            context.SaveChanges();
                        }
                        else {
                            context.tbdetallemovimientos.Add(new DetalleMovimientos()
                            {
                                int_idmovimiento = multas.int_id_movimiento_id.Value,
                                int_idespacio = response.int_id_espacio.Value,
                                int_id_usuario_id = response.int_id_usuario_id,
                                int_id_zona = espacio.id_zona,
                                int_duracion = response.int_tiempo,
                                dtm_horaInicio = time,
                                dtm_horaFin = time,
                                flt_importe = 0.0,
                                flt_saldo_anterior = 0.0,
                                flt_saldo_fin = 0.0,
                                str_observaciones = response.str_comentarios

                            });


                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            });
            if (strResult == "")
            {
                return Ok();
            }
            else
            {
                return Json(new { idMovimiento = strResult });
            }

        }

        [HttpPost("mtdIngresarMultaFisica")]
        public async Task<ActionResult<Multas>> mtdIngresarMultaFisica([FromBody] Multas multas)
        {
            try
            {
                //DateTime time = DateTime.Now;
                //var zonaHorariaWindows = "Central Standard Time (Mexico)";
                //var fechaHoraConversion = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, zonaHorariaWindows);
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();
                DateTime time = horadeTransaccion.Value;



                string strResult = "";
                int intIdMulta = 0;

                
                    multas.created_date = time;
                    multas.created_by = multas.created_by.ToString();
                    multas.last_modified_by = multas.created_by;

                    multas.dtm_fecha = time;
                    multas.bit_status = true;
                    multas.flt_monto = 0.0;
                    multas.str_tipo_multa = "MULTA FISICA";
                    context.tbmultas.Add(multas);
                    await context.SaveChangesAsync();
                    intIdMulta = multas.id;

                
               
               
                return Ok();

            }
            catch (Exception ex )
            {

                return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdMultaGarantizada")]
        public async Task<ActionResult<Multas>> mtdMultaGarantizada([FromBody] Multas multa)
        {
            string strResult = "";
            int idMovNuevo = 0;

            var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == multa.id);
            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
            var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);

            if (espacio == null)
            {
              strResult =  await mtdMultaGarantizadaSE(multa);
              return Json(new { idMovimiento = strResult });
            }
            var strategy = context.Database.CreateExecutionStrategy();
            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();
            DateTime time = DateTime.Now;
            await strategy.ExecuteAsync(async () =>
            {

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                      
                        multaResponse.str_documento_garantia = multa.str_documento_garantia;
                        multaResponse.str_tipo_multa = "MULTA GARANTIZADA";
                        multaResponse.bit_status = false;
                        await context.SaveChangesAsync();
                        var movimiento = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                        var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimiento.int_id_usuario_id);

                        context.tbdetallemulta.Add(new DetalleMulta()
                        {
                            int_id_multa = multa.id,
                            bit_status = true,
                            dtmFecha = time,
                            str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                            flt_monto = 0,
                            str_comentarios = "MULTA GARANTIZADA"
                        });
                        context.SaveChanges();

                        response.str_comentarios = "MULTA GARANTIZADA CON " + multa.str_documento_garantia;
                        response.bit_status = false;
                        await context.SaveChangesAsync();
            
                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                            int_idespacio = response.int_id_espacio.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_id_zona = espacio.id_zona,
                            int_duracion = 0,
                            dtm_horaInicio = time,
                            dtm_horaFin = time,
                            flt_importe = 0.0,
                            flt_saldo_anterior = 0.0,
                            flt_saldo_fin = 0.0,
                            str_observaciones = response.str_comentarios
                        });

                        context.SaveChanges();

                        
                        espacio.bit_ocupado = false;
                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            });
            if (strResult == "")
            {
                return Ok();
            }
            else
            {
                return Json(new { idMovimiento = strResult });
            }

        }


        [NonAction]
        public async Task<String> mtdMultaGarantizadaSE(Multas multa)
        {
            string strResult = "";
            int idMovNuevo = 0;
            var strategy = context.Database.CreateExecutionStrategy();

            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();
            DateTime time = DateTime.Now;
            await strategy.ExecuteAsync(async () =>
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == multa.id);
                        multaResponse.str_documento_garantia = multa.str_documento_garantia;
                        multaResponse.str_tipo_multa = "MULTA GARANTIZADA";
                        multaResponse.bit_status = false;
                        await context.SaveChangesAsync();
                        var movimiento = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                        var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimiento.int_id_usuario_id);

                        context.tbdetallemulta.Add(new DetalleMulta()
                        {
                            int_id_multa = multa.id,
                            bit_status = true,
                            dtmFecha = time,
                            str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                            flt_monto = 0.00,
                            str_comentarios = "MULTA GARANTIZADA"
                        });
                        context.SaveChanges();

                        var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                        response.str_comentarios = "MULTA GARANTIZADA CON " + multa.str_documento_garantia;
                        response.bit_status = false;
                        await context.SaveChangesAsync();
                     

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_duracion = 0,
                            dtm_horaInicio = time,
                            dtm_horaFin = time,
                            flt_importe = 0.00,
                            flt_saldo_anterior = 0.00,
                            flt_saldo_fin = 0.00,
                            str_observaciones = response.str_comentarios
                        });

                        context.SaveChanges();
                       

                        transaction.Commit();

                        await _emailSender.SendEmailAsync(usuario.Email, "Multa garantizada",
                          usuario.UserName + " Su multa ha sido garantizada con " + multa.str_documento_garantia +". Recuerde realizar el pago de la multa generada para proceder con la devolucion de su documento. ");
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;

                    }
                }
                return strResult;
            });
            return strResult;

        }

        [HttpPut("mtdMultaFuga")]
        public async Task<ActionResult<Multas>> mtdMultaFuga([FromBody] Multas multa)
        {

            string strResult = "";
            int idMovNuevo = 0;
            var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == multa.id);
            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
            var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);

            if (espacio == null)
            {
              strResult = await mtdMultaFugaSE(multa);
              return Json(new { idMovimiento = strResult });

            }
            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();
            DateTime time = DateTime.Now;

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                      
                        multaResponse.bit_status = false;
                        multaResponse.str_tipo_multa = "MULTA FUGA";
                        multaResponse.str_clave_candado = multa.str_clave_candado;
                        await context.SaveChangesAsync();


                       
                        response.str_comentarios = "FUGA";
                        response.bit_status = false;
                        await context.SaveChangesAsync();

                        var movimiento = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                        var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimiento.int_id_usuario_id);

                        context.tbdetallemulta.Add(new DetalleMulta()
                        {
                            int_id_multa = multa.id,
                            bit_status = true,
                            dtmFecha = time,
                            str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                            flt_monto = 0,
                            str_comentarios = "MULTA FUGA"
                        });
                        context.SaveChanges();

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = multa.int_id_movimiento_id.Value,
                            int_idespacio = response.int_id_espacio.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_id_zona = espacio.id_zona,
                            int_duracion = 0,
                            dtm_horaInicio =time,
                            dtm_horaFin = time,
                            flt_importe = 0.0,
                            flt_saldo_anterior = 0.0,
                            flt_saldo_fin = 0.0,
                            str_observaciones = response.str_comentarios

                        });

                        context.SaveChanges();

                       
                        espacio.bit_ocupado = false;
                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            });
            if (strResult == "")
            {
                return Ok();
            }
            else
            {
                return Json(new { idMovimiento = strResult });
            }
        }

        [NonAction]
        public async Task<String> mtdMultaFugaSE([FromBody] Multas multa)
        {

            string strResult = "";
            int idMovNuevo = 0;
            var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == multa.id);
            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
            var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);

            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();
            DateTime time = DateTime.Now;

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        multaResponse.bit_status = false;
                        multaResponse.str_tipo_multa = "MULTA FUGA";
                        multaResponse.str_clave_candado = multa.str_clave_candado;
                        await context.SaveChangesAsync();

                        response.str_comentarios = "FUGA";
                        response.bit_status = false;
                        await context.SaveChangesAsync();

                        var movimiento = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                        var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimiento.int_id_usuario_id);

                        context.tbdetallemulta.Add(new DetalleMulta()
                        {
                            int_id_multa = multa.id,
                            bit_status = true,
                            dtmFecha = time,
                            str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                            flt_monto = 0,
                            str_comentarios = "MULTA FUGA"
                        });
                        context.SaveChanges();

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = multa.int_id_movimiento_id.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_duracion = 0,
                            dtm_horaInicio = time,
                            dtm_horaFin = time,
                            flt_importe = 0.0,
                            flt_saldo_anterior = 0.0,
                            flt_saldo_fin = 0.0,
                            str_observaciones = response.str_comentarios

                        });

                        context.SaveChanges();

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
                return strResult;
            });
            if (strResult == "")
            {
                return strResult;
            }
            else
            {
                return strResult;
            }
        }

        [HttpPut("mtdMultaDespuesDeLas10")]
        public async Task<ActionResult<Multas>> mtdMultaDespuesDeLas10([FromBody] Multas multa)
        {
            string strResult = "";
            int idMovNuevo = 0;
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == multa.id);
                        multaResponse.bit_status = false;
                        multaResponse.str_tipo_multa = "MULTA DESPUES DE LAS 10";
                        await context.SaveChangesAsync();


                        var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                        response.str_comentarios = "MULTA DESPUES DE LAS 10";
                        response.bit_status = false;
                        await context.SaveChangesAsync();

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                            int_idespacio = response.int_id_espacio.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_id_zona = 1,
                            int_duracion = response.int_tiempo,
                            dtm_horaInicio = response.dt_hora_inicio,
                            dtm_horaFin = response.dtm_hora_fin,
                            flt_importe = 0.0,
                            flt_saldo_anterior = 0.0,
                            flt_saldo_fin = 0.0,
                            str_observaciones = response.str_comentarios

                        });

                        context.SaveChanges();

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            });
            if (strResult == "")
            {
                return Ok();
            }
            else
            {
                return Json(new { idMovimiento = strResult });
            }
        }

        [NonAction]
        [HttpPut("mtdPagarMulta")]
        public async Task<ActionResult<Multas>> mtdPagarMulta(int intIdMulta, string strIdUsuario)
        {
            string strResult = "";
            int idMovNuevo = 0;
            var strategy = context.Database.CreateExecutionStrategy();

            var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == intIdMulta);
            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
            var saldo = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == strIdUsuario);
            var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);

            await strategy.ExecuteAsync(async () =>
            {

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        multaResponse.bit_status = true;
                        multaResponse.str_tipo_pago ="VIRTUAL";
                        
                        await context.SaveChangesAsync();
                      
                        response.str_comentarios = "MULTA PAGADA";
                        response.bit_status = false;
                        await context.SaveChangesAsync();

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                            int_idespacio = response.int_id_espacio.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_id_zona = espacio.id_zona,
                            int_duracion = response.int_tiempo,
                            dtm_horaInicio = response.dt_hora_inicio,
                            dtm_horaFin = response.dtm_hora_fin,
                            flt_importe = response.flt_monto,
                            flt_saldo_anterior = 500.00,
                            flt_saldo_fin = 400.00,
                            str_observaciones = response.str_comentarios

                        });

                        context.SaveChanges();

                      
                        espacio.bit_ocupado = false;
                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                    }
                }
            });
            if (strResult == "")
            {
                return Ok();
            }
            else
            {
                return Json(new { idMovimiento = strResult });
            }
        }

        [HttpPut("mtdPagarMultaPreliminar")]
        public async Task<ActionResult<Multas>> mtdPagarMultaPreliminar(int intIdMulta, string strIdUsuario, Double dblMonto)
        {
            string strResult = "";
            int idMovNuevo = 0;
            var strategy = context.Database.CreateExecutionStrategy();

            var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == intIdMulta);
            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
            var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);
            var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> horaTransaccion = par.mtdObtenerFechaMexico();
            DateTime time = horaTransaccion.Value;

            if (mtdEjecutarPasarelaPagos())
            {

                    await strategy.ExecuteAsync(async () =>
                    {

                        using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                multaResponse.bit_status = false;
                                multaResponse.str_tipo_pago = "TARJETA";
                                await context.SaveChangesAsync();

                                response.str_comentarios = "MULTA PAGADA";
                                response.bit_status = false;
                               // response.str_comentarios = "DESAPARCADO";
                                await context.SaveChangesAsync();

                                if (espacio != null)
                                {
                                    context.tbdetallemovimientos.Add(new DetalleMovimientos()
                                    {
                                        int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                                        int_idespacio = response.int_id_espacio.Value,
                                        int_id_usuario_id = response.int_id_usuario_id,
                                        int_id_zona = espacio.id_zona,
                                        int_duracion = response.int_tiempo,
                                        dtm_horaInicio = response.dt_hora_inicio,
                                        dtm_horaFin = response.dtm_hora_fin,
                                        flt_importe = dblMonto,
                                        flt_saldo_anterior = 0.00,
                                        flt_saldo_fin = 0.00,
                                        str_observaciones = "MULTA PAGADA",

                                    });

                                    context.SaveChanges();

                                    espacio.bit_ocupado = false;
                                    await context.SaveChangesAsync();
                                }
                                else
                                {
                                    context.tbdetallemovimientos.Add(new DetalleMovimientos()
                                    {
                                        int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                                        str_latitud = response.str_latitud,
                                        str_longitud = response.str_longitud,
                                        int_id_usuario_id = strIdUsuario,
                                        int_duracion = response.int_tiempo,
                                        dtm_horaInicio = response.dt_hora_inicio,
                                        dtm_horaFin = response.dtm_hora_fin,
                                        flt_importe = dblMonto,
                                        flt_saldo_anterior = 0.00,
                                        flt_saldo_fin = 0.00,
                                        str_observaciones = "MULTA PAGADA",

                                    });

                                    context.SaveChanges();
                                }

                                
                                context.tbdetallemulta.Add(new DetalleMulta()
                                {
                                    int_id_multa = intIdMulta,
                                    bit_status = false,
                                    dtmFecha = time,
                                    str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                                    flt_monto = dblMonto,
                                    str_comentarios = "MULTA PAGADA"
                                });
                                context.SaveChanges();

                                transaction.Commit();
                            }

                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                strResult = ex.Message;
                            }
                        }
                    });

                

            }

            if (strResult == "")
            {
                return Ok();
            }
            else
            {
                return Json(new { idMovimiento = strResult });
            }

        }
        [NonAction]
        public bool mtdEjecutarPasarelaPagos()
        {
            return true;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="multas"></param>
        /// <returns></returns>
        [HttpPut("mtdConfirmarMulta")]
        public async Task<ActionResult> mtdConfirmarMulta(int intIdMulta, [FromBody] Multas multas)
        {

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> horaTransaccion = par.mtdObtenerFechaMexico();
            DateTime time = horaTransaccion.Value;

           
            try
            {

                var response = await context.tbmultas.FirstOrDefaultAsync(x => x.id == intIdMulta);
                if (response.id != intIdMulta)
                {
                    return BadRequest();
                }

                var IDusuario = await context.tbmovimientos.FirstOrDefaultAsync(x => x.int_id_multa == intIdMulta);

                var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == IDusuario.int_id_usuario_id );

                response.last_modified_by = multas.last_modified_by;
                response.last_modified_date = time;
                response.dtm_fecha = time;
                response.str_clave_candado = multas.str_clave_candado;
                response.str_no_parquimetro = multas.str_no_parquimetro;
                response.str_id_agente_id = multas.str_id_agente_id;
                response.flt_monto = multas.flt_monto;
                response.str_motivo = multas.str_motivo;
                response.str_folio_multa = multas.str_folio_multa;
                response.str_Estado = multas.str_Estado;
                response.str_ubicacion = multas.str_ubicacion;
                response.str_fundamento = multas.str_fundamento;
                response.str_articulo = multas.str_articulo;
                response.str_categoria = multas.str_categoria;
                response.str_clave = multas.str_clave;
                response.str_tipo_pago = multas.str_tipo_pago;
                response.str_tipo_multa = "MULTA AUT. CON CANDADO";
                response.intidconcesion_id = multas.intidconcesion_id;
                await context.SaveChangesAsync();

                context.tbdetallemulta.Add(new DetalleMulta()
                {
                    int_id_multa = intIdMulta,
                    bit_status = true,
                    dtmFecha = time,
                    str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                    flt_monto = 0,
                    str_comentarios = "MULTA DESPUES DE LAS 10"
                });
                context.SaveChanges();


                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }


        }

        [HttpPut("mtdActualizaMultas")]
        public async Task<ActionResult> mtdActualizaMultas(int id, [FromBody] Multas multas)
        {
            var response = await context.tbmultas.FirstOrDefaultAsync(x => x.id == id);
            if (response.id != id)
            {
                return BadRequest();
            }
           
            response.last_modified_by = multas.last_modified_by;
            response.last_modified_date = DateTime.Now;
            response.dtm_fecha = multas.dtm_fecha;
            response.flt_monto = multas.flt_monto;
            response.str_motivo = multas.str_motivo;
            response.str_id_agente_id = multas.str_id_agente_id;
            response.int_id_movimiento_id = multas.int_id_movimiento_id;
            //response.int_id_saldo_id = multas.int_id_saldo_id;
            response.int_id_vehiculo_id = multas.int_id_vehiculo_id;
            response.intidconcesion_id = multas.intidconcesion_id;
            try
            {
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }

            
        }

        [HttpDelete("mtdBajaMultas")]
        public async Task<ActionResult<Multas>> mtdBajaMultas(int id)
        {
            try
            {
                var response = await context.tbmultas.FirstOrDefaultAsync(x => x.id == id);
                if (response == null)
                {
                    return NotFound();
                }
                response.bit_status = false;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdReactivarMultas")]
        public async Task<ActionResult<Multas>> mtdReactivarMultas(int id)
        {
            try
            {
                var response = await context.tbmultas.FirstOrDefaultAsync(x => x.id == id);
                if (response == null)
                {
                    return NotFound();
                }
                response.bit_status = true;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }


        [HttpGet("DP10")]
        public async Task  mtdRealizarMultaDp10()
        {
                
        //var scope = scopeFactory.CreateScope();
        //    var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var movimientos = await context.tbmovimientos.Where(x => x.str_comentarios == "MULTA").ToListAsync();

            if (movimientos.Count != 0)
            {
                foreach (var item in movimientos)
                {
                    var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == item.int_id_multa);
                    var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);

                   
                    var strategy = context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () =>
                     {

                         using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                         {
                             try
                             {
                               
                                 multaResponse.bit_status = false;
                                 multaResponse.str_tipo_multa = "MULTA DESPUES DE LAS 10";
                                 await context.SaveChangesAsync();


                                
                                 response.str_comentarios = "MULTA DESPUES DE LAS 10";
                                 response.bit_status = false;
                                 await context.SaveChangesAsync();


                                 var movimiento = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
                                 var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimiento.int_id_usuario_id);

                                 context.tbdetallemulta.Add(new DetalleMulta()
                                 {
                                     int_id_multa = item.int_id_multa.Value,
                                     bit_status = true,
                                     dtmFecha = DateTime.Now,
                                     str_usuario = usuario.strNombre + " " + usuario.strApellidos,
                                     flt_monto = 0,
                                     str_comentarios = "MULTA DESPUES DE LAS 10"
                                 });
                                 context.SaveChanges();

                                 if (response.int_id_espacio == null)
                                 {
                                     context.tbdetallemovimientos.Add(new DetalleMovimientos()
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

                                     context.SaveChanges();
                                 }
                                 else
                                 {
                                     context.tbdetallemovimientos.Add(new DetalleMovimientos()
                                     {
                                         int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
                                         int_idespacio = response.int_id_espacio.Value,
                                         int_id_usuario_id = response.int_id_usuario_id,
                                         int_id_zona = 1,
                                         int_duracion = response.int_tiempo,
                                         dtm_horaInicio = response.dt_hora_inicio,
                                         dtm_horaFin = response.dtm_hora_fin,
                                         flt_importe = 0.0,
                                         flt_saldo_anterior = 0.0,
                                         flt_saldo_fin = 0.0,
                                         str_observaciones = response.str_comentarios

                                     });

                                     context.SaveChanges();

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

        //[NonAction]
        //public async Task mtdRealizarMultaDp10SE()
        //{
        //    var strategy = context.Database.CreateExecutionStrategy();
        //    await strategy.ExecuteAsync(async () =>
        //    {

        //        using (IDbContextTransaction transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                var multaResponse = await context.tbmultas.FirstOrDefaultAsync(x => x.id == item.int_id_multa);

        //                multaResponse.bit_status = false;
        //                multaResponse.str_tipo_multa = "MULTA DESPUES DE LAS 10";
        //                await context.SaveChangesAsync();

        //                var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);


        //                response.str_comentarios = "MULTA DESPUES DE LAS 10";
        //                response.bit_status = false;
        //                await context.SaveChangesAsync();


        //                var movimiento = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == multaResponse.int_id_movimiento_id);
        //                var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimiento.int_id_usuario_id);

        //                context.tbdetallemulta.Add(new DetalleMulta()
        //                {
        //                    int_id_multa = item.int_id_multa.Value,
        //                    bit_status = true,
        //                    dtmFecha = DateTime.Now,
        //                    str_usuario = usuario.strNombre + " " + usuario.strApellidos,
        //                    flt_monto = 0,
        //                    str_comentarios = "MULTA GARANTIZADA"
        //                });
        //                context.SaveChanges();

        //                context.tbdetallemovimientos.Add(new DetalleMovimientos()
        //                {
        //                    int_idmovimiento = multaResponse.int_id_movimiento_id.Value,
        //                    int_idespacio = response.int_id_espacio.Value,
        //                    int_id_usuario_id = response.int_id_usuario_id,
        //                    int_id_zona = 1,
        //                    int_duracion = response.int_tiempo,
        //                    dtm_horaInicio = response.dt_hora_inicio,
        //                    dtm_horaFin = response.dtm_hora_fin,
        //                    flt_importe = 0.0,
        //                    flt_saldo_anterior = 0.0,
        //                    flt_saldo_fin = 0.0,
        //                    str_observaciones = response.str_comentarios

        //                });

        //                context.SaveChanges();

        //                transaction.Commit();
        //            }

        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();


        //            }
        //        }
        //    });

        //}
    }
}
