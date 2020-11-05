using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;
using WebApiParquimetros.Services;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SaldosController: Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IEmailSender _emailSender;
        public SaldosController (ApplicationDbContext context, IEmailSender emailSender)
        {
            this.context = context;
            _emailSender = emailSender;
        }

        [HttpGet("mtdConsultarSaldos")]
        public async Task<ActionResult<IEnumerable<Saldos>>> mtdConsultarSaldos()
        {
            try
            {
                var response = await context.tbsaldo.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarSaldosXId")]
        public async Task<ActionResult<Saldos>> mtdConsultarSaldosXId(int id)
        {
            try
            {
                var response = await context.tbsaldo.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpGet("mtdConsultarSaldoXIdUsuario")]
        public async Task<ActionResult<ApplicationUser>> mtdConsultarSaldoXIdUsuario(string intIdUsuario)
        {
            try
            {
                var response = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == intIdUsuario);
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

        [HttpPost("mtdIngresarSaldo")]
        public async Task<ActionResult<Saldos>> mtdIngresarSaldo([FromBody] Saldos saldos)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                saldos.created_date = horadeTransaccion.Value;
                saldos.last_modified_date = horadeTransaccion.Value;
                context.tbsaldo.Add(saldos);
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

        [HttpPut("mtdRecargarSaldo")]
        public async Task<ActionResult> mtdRecargarSaldo(double fltMonto, [FromBody] Saldos saldos)
        {
            string strresult = " ";
            double dblSaldoInicial = 0;
            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                    ParametrosController par = new ParametrosController(context);
                    ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();
                    DateTime time = horadeTransaccion.Value;
                    //DateTime time = DateTime.Now;
                    //Se ocomento esta linea por actualizacion 
                    //var response = await context.tbsaldo.FirstOrDefaultAsync(x => x.id == id);

                    //var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == movimientos.intidconcesion_id && x.str_tipo == "RECARGA");

                    var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == saldos.int_id_usuario_trans);
                        dblSaldoInicial =usuario.dbl_saldo_actual;

                        usuario.dbl_saldo_actual = usuario.dbl_saldo_actual +  fltMonto;
                        usuario.dbl_saldo_anterior = dblSaldoInicial;

                    var comision = await context.tbparametros.Where(x => x.intidconcesion_id == null).FirstOrDefaultAsync();

                    double c = comision.PorcentajeComisionRecarga / 100;

                    double total = fltMonto * c;
                        //Se comento esta linea por actualizacion 
                        //response.last_modified_by = saldos.last_modified_by;
                        //response.last_modified_date = horadeTransaccion.Value;
                        //response.dtmfecha = horadeTransaccion.Value;
                        //response.flt_monto_final = response.flt_monto_final + fltMonto;
                        //response.flt_monto_inicial = dblSaldoInicial;
                        //response.int_id_usuario_trans = saldos.int_id_usuario_trans;
                        //response.str_forma_pago = saldos.str_forma_pago;
                        //response.str_tipo_recarga = saldos.str_tipo_recarga;

                    //response.intidconcesion_id = saldos.intidconcesion_id;

                    await context.SaveChangesAsync();

                        context.tbsaldo.Add(new Saldos()
                        {
                            created_by = saldos.created_by,
                           // created_date = horadeTransaccion.Value,
                            dtmfecha = time,
                            last_modified_date = time,
                            flt_monto_inicial = dblSaldoInicial,
                            flt_monto_final = usuario.dbl_saldo_actual,
                            str_forma_pago = "VIRTUAL",
                            str_tipo_recarga= "RECARGA",
                            flt_monto = fltMonto,
                            flt_porcentaje_comision = total,
                            flt_total_con_comision = fltMonto + total,
                           int_id_usuario_id = usuario.Id,
                           int_id_usuario_trans =  usuario.Id

                        });;

                      await  context.SaveChangesAsync();
                        await _emailSender.SendEmailAsync(usuario.Email, "Recarga de saldo",
                                       "La recarga de saldo se ha realizado exitosamente. Monto $" + fltMonto + "MXN.<br/> Si usted no reconoce este movimiento comuniquese con el equipo de soporte");

                        transaction.Commit();
                        
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strresult = ex.Message;
                        

                    }
                    
                }
            //});
            return Ok();
        }

        //[HttpPut("mtPruebaCorreo")]
        //public async Task<ActionResult> mtPruebaCorreo()
        //{
        //    string strresult = "";

        //    try
        //    {

              
        //    await _emailSender.SendEmailAsync("dulce.siteweb@gmail.com", "Recarga de saldo",
        //                       "La recarga de saldo se ha realizado exitosamente. Monto $" + 0.00 + "MXN.<br/> Si usted no reconoce este movimiento comuniquese con el equipo de soporte");


        //    }

        //    catch (Exception ex)
        //    {

        //        strresult = ex.Message;


        //    }
        //    return Ok();

        //}
           
       
        [HttpPut("mtdActualizaSaldos")]
        public async Task<ActionResult> mtdActualizaSaldos(int id, [FromBody] Saldos saldos)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                var response = await context.tbsaldo.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }
             
                response.last_modified_by = saldos.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value;
                response.dtmfecha = saldos.dtmfecha;
                response.flt_monto_final = saldos.flt_monto_final;
                response.flt_monto_inicial = saldos.flt_monto_inicial;
                response.int_id_usuario_trans = saldos.int_id_usuario_trans;
                response.str_forma_pago = saldos.str_forma_pago;
                response.str_tipo_recarga = saldos.str_tipo_recarga;
                response.int_id_usuario_id = saldos.int_id_usuario_id;
                response.intidconcesion_id = saldos.intidconcesion_id;

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

        [HttpDelete("mtdBajaSaldo")]
        public async Task<ActionResult<Saldos>> mtdBajaSaldo(int id)
        {
            try
            {
                var response = await context.tbsaldo.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPut("mtdReactivarSaldo")]
        public async Task<ActionResult<Saldos>> mtdReactivarSaldo(int id)
        {
            try
            {
                var response = await context.tbsaldo.FirstOrDefaultAsync(x => x.id == id);
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

    }
}
