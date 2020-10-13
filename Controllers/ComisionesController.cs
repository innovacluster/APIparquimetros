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
    public class ComisionesController: Controller
    {
        private readonly ApplicationDbContext context;

        public ComisionesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarComisiones")]
        public async Task<ActionResult<IEnumerable<Comisiones>>> mtdConsultarComerciantes()
        {
            try
            {
                var response = await context.tbcomisiones.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarComisionesXId")]
        public async Task<ActionResult<Comisiones>> mtdConsultarComerciantesXId(int id)
        {
            try
            {
                var response = await context.tbcomisiones.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpGet("mtdConsultarComisionesXIdConcesion")]
        public async Task<ActionResult<Comisiones>> mtdConsultarComisionesXIdConcesion(int intIdConcesion)
        {
            try
            {
                var response = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == intIdConcesion);
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

        [HttpPost("mtdIngresarComision")]
        public async Task<ActionResult> mtdIngresarComerciante([FromBody] Comisiones comisiones)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();

                comisiones.created_date = horadeTransaccion.Value;
                comisiones.last_modified_date = horadeTransaccion.Value;
                context.tbcomisiones.Add(comisiones);
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

        [HttpPut("mtdActualizaComision")]
        public async Task<ActionResult> mtdActualizaComerciante(int id, [FromBody] Comisiones comisiones)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();

                var response = await context.tbcomisiones.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }
                response.dcm_porcentaje = comisiones.dcm_porcentaje;
                response.dcm_valor_fijo = comisiones.dcm_valor_fijo;
                response.str_tipo = comisiones.str_tipo;
                response.last_modified_by = comisiones.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value;
                response.intidconcesion_id = comisiones.intidconcesion_id;
                
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


        [HttpDelete("mtdBajaComision")]
        public async Task<ActionResult<Comisiones>> mtdBajaComerciante(int id)
        {
            try
            {
                var response = await context.tbcomisiones.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPut("mtdReactivarComision")]
        public async Task<ActionResult<Comisiones>> mtdReactivarComerciante(int id)
        {
            try
            {
                var response = await context.tbcomisiones.FirstOrDefaultAsync(x => x.id == id);
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
