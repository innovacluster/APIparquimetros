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
    public class ComerciantesController: Controller
    {
        private readonly ApplicationDbContext context;

        public ComerciantesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarComerciantes")]
        public async Task<ActionResult<IEnumerable<Comerciantes>>> mtdConsultarComerciantes()
        {
            try
            {
                var response = await context.tbcomerciantes.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {

                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
            
        }

        [HttpGet("mtdConsultarComerciantesXId")]
        public async Task<ActionResult<Comerciantes>> mtdConsultarComerciantesXId(int id)
        {
            try
            {
                var response = await context.tbcomerciantes.FirstOrDefaultAsync(x => x.id == id);
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
        //Duda con la validacion de comerciantes
        [HttpPost("mtdIngresarComerciante")]
        public async Task<ActionResult> mtdIngresarComerciante([FromBody] Comerciantes comerciantes)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();
                string strComercianteN = comerciantes.str_nombre + ' ' + comerciantes.str_apellido_ap + ' ' + comerciantes.str_apellido_mat;
                var response = await context.tbcomerciantes.FirstOrDefaultAsync(x => x.str_nombre + ' ' + x.str_apellido_ap + x.str_apellido_mat == strComercianteN);
                
                if (response == null)
                {
                    comerciantes.created_date = horadeTransaccion.Value;
                    comerciantes.last_modified_date = horadeTransaccion.Value;
                    context.tbcomerciantes.Add(comerciantes);
                    await context.SaveChangesAsync();
                    return Ok(); 
                }
                //ModelState.AddModelError("token", "El comerciante ya se encuentra registrado");
                //return BadRequest(ModelState);
                return Json(new { token = "El comerciante ya se encuentra registrado" });
            }

            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdActualizaComerciante")]
        public async Task<ActionResult> mtdActualizaComerciante(int id, [FromBody] Comerciantes comerciantes)
        {
            try
            {
                var response = await context.tbcomerciantes.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }
                response.str_apellido_ap = comerciantes.str_apellido_ap;
                response.str_apellido_mat = comerciantes.str_apellido_mat;
                response.str_nombre = comerciantes.str_nombre;
                response.str_comerciante = comerciantes.str_comerciante;
                response.str_telefono = comerciantes.str_telefono;
                response.last_modified_by = comerciantes.last_modified_by;
                response.last_modified_date = DateTime.Now;
                response.intidconcesion_id = comerciantes.intidconcesion_id;


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


        [HttpDelete("mtdBajaComerciante")]
        public async Task<ActionResult<Comerciantes>> mtdBajaComerciante(int id)
        {
            try
            {
                var response = await context.tbcomerciantes.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPut("mtdReactivarComerciante")]
        public async Task<ActionResult<Comerciantes>> mtdReactivarComerciante(int id)
        {
            try
            {
                var response = await context.tbcomerciantes.FirstOrDefaultAsync(x => x.id == id);
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
