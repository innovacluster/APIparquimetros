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
    public class OpcionesController: Controller
    {
        private readonly ApplicationDbContext context;

        public OpcionesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarOpciones")]
        public async Task<ActionResult<IEnumerable<Opciones>>> mtdConsultarOpciones()
        {
            try
            {

                var response = await context.tbopciones.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarOpcionesXId")]
        public async Task<ActionResult<Opciones>> mtdConsultarOpcionesXId(int id)
        {
            try
            {
                var response = await context.tbopciones.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPost("mtdIngresarOpciones")]
        public async Task<ActionResult<Opciones>> mtdIngresarOpciones([FromBody] Opciones opciones)
        {
            try
            {
              
                    opciones.bit_status = true;
                    context.tbopciones.Add(opciones);
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

        [HttpPut("mtdActualizaOpciones")]
        public async Task<ActionResult> mtdActualizaOpciones(int id, [FromBody] Opciones opciones)
        {
            var response = await context.tbopciones.FirstOrDefaultAsync(x => x.id == id);
            if (response.id != id)
            {
                return BadRequest();
            }
            response.str_opcion = opciones.str_opcion;
            
            try
            {
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }


        }

        [HttpDelete("mtdBajaOpciones")]
        public async Task<ActionResult<Opciones>> mtdBajaMultas(int id)
        {
            try
            {
                var response = await context.tbopciones.FirstOrDefaultAsync(x => x.id == id);
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
                return Json(new { token = ex.Message });
            }
        }

        //[HttpPut("mtdReactivarMultas")]
        //public async Task<ActionResult<Opciones>> mtdReactivarMultas(int id)
        //{
        //    var response = await context.tbopciones.FirstOrDefaultAsync(x => x.id == id);
        //    if (response == null)
        //    {
        //        return NotFound();
        //    }
        //    response.bit_status = true;
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}*/
    }
}
