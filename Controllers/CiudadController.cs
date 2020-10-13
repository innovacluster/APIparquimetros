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
    public class CiudadController : Controller
    {
        private readonly ApplicationDbContext context;
        public CiudadController(ApplicationDbContext context)
        {
            this.context= context;
        }

        [HttpGet("mtdConsultarCiudades")]
        public async Task<ActionResult<IEnumerable<Ciudades>>> mtdConsultarCiudades()
        {
            try
            {
                var response = await context.tbciudades.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
            
        }

        [HttpGet("mtdConsultarCiudadesXId")]
        public async Task<ActionResult<Ciudades>> mtdConsultarCiudadesXId(int id)
        {

            try
            {
                var response = await context.tbciudades.FirstOrDefaultAsync(x => x.id == id);

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



        [HttpPost("mtdIngresarCiudad")]
        public async Task<ActionResult> mtdIngresarCiudad([FromBody] Ciudades ciudades)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();

                var response = await context.tbciudades.FirstOrDefaultAsync(x => x.str_ciudad == ciudades.str_ciudad);
                if (response == null)
                {
                    ciudades.created_date = horadeTransaccion.Value;
                    ciudades.last_modified_date = horadeTransaccion.Value;
                    context.tbciudades.Add(ciudades);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    //ModelState.AddModelError("token", "El registro ya existe");
                    //return BadRequest(ModelState);
                    return Json(new { token = "El registro ya existe" });
                }
            }

            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdActualizaCiudad")]
        public async Task<ActionResult> mtdActualizaCiudad(int id, [FromBody] Ciudades ciudades)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();
                
                var response = await context.tbciudades.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }
                response.str_ciudad = ciudades.str_ciudad;
                response.created_by = ciudades.created_by;
                response.last_modified_by = ciudades.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value;
                response.str_latitud = ciudades.str_latitud;
                response.str_longitud = ciudades.str_longitud;
                response.str_ciudad = ciudades.str_ciudad;
                //response.intidconcesion_id = ciudades.intidconcesion_id;

                
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

      

        [HttpDelete("mtdBajaCiudad")]
        public async Task<ActionResult<Ciudades>> mtdBajaCiudad(int id)
        {
            try
            {
                var response = await context.tbciudades.FirstOrDefaultAsync(x => x.id == id);

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



        [HttpPut("mtdReactivarCiudad")]
        public async Task<ActionResult<Ciudades>> mtdReactivarCiudad(int id)
        {
            try
            {
                var response = await context.tbciudades.FirstOrDefaultAsync(x => x.id == id);

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
