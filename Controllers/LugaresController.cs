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
    public class LugaresController: Controller
    {
        private readonly ApplicationDbContext context;

        public LugaresController (ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarLugares")]
        public async Task<ActionResult<IEnumerable<Lugares>>> mtdConsultarLugares()
        {
            try
            {
                var response = await context.tblugares.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarLugaresXId")]
        public async Task<ActionResult<Lugares>> mtdConsultarLugaresXId (int id)
        {
            try
            {
                var response = await context.tblugares.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPost("mtdIngresarLugar")]
        public async Task<ActionResult<Lugares>> mtdIngresarLugar([FromBody] Lugares lugares)
        {
            try
            {
                var response = await context.tblugares.FirstOrDefaultAsync(x => x.str_lugar == lugares.str_lugar);
                if (response == null)
                {
                    lugares.created_date = DateTime.Now;
                    lugares.last_modified_date = DateTime.Now;
                    context.tblugares.Add(lugares);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                //ModelState.AddModelError("token", "El registro ya existe");
                //return BadRequest(ModelState);
                return Json(new { token = "El registro ya existe" });
            }

            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }

        }

        [HttpPut("mtdActualizaLugar")]
        public async Task<ActionResult> mtdActualizaLugar(int id, [FromBody] Lugares lugares)
        {
            try
            {
                var response = await context.tblugares.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }
                
                response.last_modified_date = DateTime.Now;
                response.last_modified_by = lugares.last_modified_by;
                response.str_latitud = lugares.str_latitud;
                response.str_longitud = lugares.str_longitud;
                response.str_lugar = lugares.str_lugar;
                response.int_id_zona_id = response.int_id_zona_id;
                response.intidconcesion_id = response.intidconcesion_id;
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

        [HttpDelete("mtdBajaLugar")]
        public async Task<ActionResult<Lugares>> mtdBajaLugar(int id)
        {
            try
            {
                var response = await context.tblugares.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPut("mtdReactivarLugar")]
        public async Task<ActionResult<Comisiones>> mtdReactivarLugar(int id)
        {
            try
            {
                var response = await context.tblugares.FirstOrDefaultAsync(x => x.id == id);
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
