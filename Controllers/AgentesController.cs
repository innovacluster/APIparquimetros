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
    public class AgentesController : Controller
    {
        public readonly ApplicationDbContext context;

        public AgentesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("mtdObtenerOficiales")]
        public async Task<ActionResult<IEnumerable<Agentes>>> mtdObtenerOficiales()
        {
            try
            {
                var response = await context.tbagentes.ToListAsync();
                return response;

            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }

        }

        [HttpGet("mtdObtenerOficialesXId")]
        public async Task<ActionResult<Agentes>> mtdObtenerOficialesXId(int id)
        {
            try
            {
                var response = await context.tbagentes.FirstOrDefaultAsync(x => x.id == id);

                if (response == null)
                {
                    return NoContent();
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

        [HttpPost("mtdInsertarAgente")]
        public async Task<ActionResult> mtdInsetarOficiales([FromBody] Agentes agente)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();
                var response = await context.tbagentes.FirstOrDefaultAsync(x => x.str_nombre == agente.str_nombre);

                if (response == null)
                {
                    agente.last_modified_date = horadeTransaccion.Value; ;
                    agente.created_date = horadeTransaccion.Value;
                    context.tbagentes.Add(agente);
                    await context.SaveChangesAsync();
                    return Ok(); 
                }

                //ModelState.AddModelError("token", "El agente ya se encuentra registrado");
                //return BadRequest(ModelState);
                return Json(new { token = "El oficial ya se encuentra registrado" });
            }
            catch (Exception ex)
            {
            //    ModelState.AddModelError("token", ex.Message);
            //    return BadRequest(ModelState);
            return Json(new { token = ex.Message });
            }

        }

        [HttpPut("mtdModificaOficial")]
        public async Task<ActionResult> mtdModificarOficial( int id, [FromBody] Agentes agente)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();
                var response = await context.tbagentes.FirstOrDefaultAsync(x => x.id == id);

                if (response.id != id)
                {
                    return NotFound();
                }
                response.intidzona_id = agente.intidzona_id;
                response.last_modified_by = agente.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value;
                response.intidconcesion_id = agente.intidconcesion_id;
                response.str_nombre = agente.str_nombre;

                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
           
        }

        [HttpDelete("mtdBajaOficial")]
        public async Task<ActionResult> mtdBajaOficial(int id)
        {
            try
            {
                var response = await context.tbagentes.FirstOrDefaultAsync(x => x.id == id);

                if (response.id != id)
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

        [HttpPut("mtdReactivarOficial")]
        public async Task<ActionResult> mtdReactivarOficial(int id)
        {
            try
            {
                var response = await context.tbagentes.FirstOrDefaultAsync(x => x.id == id);

                if (response.id != id)
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
            





    