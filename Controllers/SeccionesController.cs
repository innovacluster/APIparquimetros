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
    public class SeccionesController:Controller
    {
        public readonly ApplicationDbContext context;
        public SeccionesController( ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdObtenerSecciones")]
        public async Task<ActionResult<IEnumerable<Secciones>>> mtdObtenerSecciones()
        {
            try
            {
                var response = await context.tbsecciones.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdObtenerSeccionXId")]
        public async Task<ActionResult<Secciones>> mtdObtenerSeccionXId(int id)
        {
            try
            {
                var response = await context.tbsecciones.FirstOrDefaultAsync(x=> x.id == id);

                if (response == null)
                {
                    return NotFound();
                }

                return response;

            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message});
                
            }
        }

        [HttpPost("mtdnsertarSeccion")]
        public async Task<ActionResult<Secciones>> mtdnsertaSeccion([FromBody] Secciones seccion)
        {
            try
            {
                context.tbsecciones.Add(seccion);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
               return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdActualizaSeccion")]
        public async Task<ActionResult> mtdActualizaSeccion(int id, [FromBody] Secciones seccion)
        {
            try
            {
                var response = await context.tbsecciones.FirstOrDefaultAsync(x => x.id == id);

                if (response.id != id)
                {
                    return BadRequest();
                }

                response.str_seccion = seccion.str_seccion;
                response.str_color = seccion.str_color;
                response.str_poligono = seccion.str_poligono;
                response.intidzona_id = seccion.intidzona_id;

                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpDelete("mtdBajaSeccion")]
        public async Task<ActionResult> mtdBajaSeccion(int id)
        {
            try
            {
                var response = await context.tbsecciones.FirstOrDefaultAsync(x => x.id == id);

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
                return Json(new { token = ex.Message});
            }
        }

        [HttpPut("mtdReactivaSeccion")]
        public async Task<ActionResult> mtdReactivaSeccion(int id)
        {
            try
            {
                var response = await context.tbsecciones.FirstOrDefaultAsync(x => x.id == id);

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
                return Json(new {token = ex.Message });
            }
        }
    }
}
