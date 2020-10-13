using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TarjetasController: Controller
    {
        private readonly ApplicationDbContext context;

        public TarjetasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarTarjetas")]
        public async Task<ActionResult<IEnumerable<Tarjetas>>> mtdConsultarTarjetas()
        {
            try
            {
                var response = await context.tbtarjetas.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarTarjetasXId")]
        public async Task<ActionResult<Tarjetas>> mtdConsultarTarjetasXId(int id)
        {
            try
            {
                var response = await context.tbtarjetas.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPost("mtdIngresarTarjeta")]
        public async Task<ActionResult<Tarjetas>> mtdIngresarTarjeta([FromBody] Tarjetas tarjetas)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                tarjetas.created_date = horadeTransaccion.Value;
                tarjetas.last_modified_date = horadeTransaccion.Value;
                context.tbtarjetas.Add(tarjetas);
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

        [HttpPut("mtdActualizaTarjeta")]
        public async Task<ActionResult> mtdActualizaTarjeta(int id, [FromBody] Tarjetas tarjetas)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                var response = await context.tbtarjetas.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }
               
                response.last_modified_by = tarjetas.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value;
                response.bit_status = tarjetas.bit_status;
                response.dc_mano_vigencia = tarjetas.dc_mano_vigencia;
                response.dcm_mes_vigencia = tarjetas.dcm_mes_vigencia;
                response.str_referencia_tarjeta = tarjetas.str_referencia_tarjeta;
                response.str_sistema_tarjeta = tarjetas.str_sistema_tarjeta;
                response.str_tarjeta = tarjetas.str_tarjeta;
                response.str_titular = tarjetas.str_titular;
                response.int_id_usuario_id = tarjetas.int_id_usuario_id;
                response.intidconcesion_id = tarjetas.intidconcesion_id;

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

        [HttpDelete("mtdBajaTarjeta")]
        public async Task<ActionResult<Tarjetas>> mtdBajaTarjeta(int id)
        {
            try
            {
                var response = await context.tbtarjetas.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPut("mtdReactivarTarjeta")]
        public async Task<ActionResult<Tarjetas>> mtdReactivarTarjeta(int id)
        {
            try
            {
                var response = await context.tbtarjetas.FirstOrDefaultAsync(x => x.id == id);
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
