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
    public class VehiculosController: Controller
    {
        private readonly ApplicationDbContext context;

        public VehiculosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarVehiculos")]
        public async Task<ActionResult<IEnumerable<Vehiculos>>> mtdConsultarVehiculos()
        {
            try
            {
                var response = await context.tbvehiculos.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarVehiculosXId")]
        public async Task<ActionResult<Vehiculos>> mtdConsultarVehiculosXId(int id)
        {
            try
            {

                var response = await context.tbvehiculos.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpGet("mtdConsultarVehiculosXIdUsuario")]
        public async Task<ActionResult<List<Vehiculos>>> mtdConsultarVehiculosXIdUsuario(string id)
        {

                try
                {
                    var response = await context.tbvehiculos.Where(x => x.int_id_usuario_id == id && x.bit_status == true).OrderBy(x => x.int_id_usuario_id).ToListAsync();
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

        [HttpPost("mtdIngresarVehiculo")]
        public async Task<ActionResult<Vehiculos>> mtdIngresarVehiculo([FromBody] Vehiculos vehiculos)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                var response = await context.tbvehiculos.FirstOrDefaultAsync(x => x.str_placas == vehiculos.str_placas);
                if (response == null)
                {
                    vehiculos.created_date = horadeTransaccion.Value;
                    vehiculos.last_modified_date = horadeTransaccion.Value;
                    context.tbvehiculos.Add(vehiculos);
                    await context.SaveChangesAsync();
                    return Ok(); 
                }

                //ModelState.AddModelError("token", "La placas que intenta registrar ya se encuentra registradas");
                //return BadRequest(ModelState);
                return Json(new { token = "La placas que intenta registrar ya se encuentra registradas" });
            }

            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }

        }

        [HttpPut("mtdActualizaVehiculo")]
        public async Task<ActionResult> mtdActualizaVehiculo(int id, [FromBody] Vehiculos vehiculos)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                var response = await context.tbvehiculos.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }
                response.last_modified_by = vehiculos.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value;
                response.bit_status = vehiculos.bit_status;
                response.str_color = vehiculos.str_color;
                response.str_modelo = vehiculos.str_modelo;
                response.str_marca = vehiculos.str_marca;
                response.str_placas = vehiculos.str_placas;
               // response.intidconcesion_id = vehiculos.intidconcesion_id;

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

        [HttpDelete("mtdBajaVehiculo")]
        public async Task<ActionResult<Vehiculos>> mtdBajaVehiculo(int id)
        {
            try
            {
                var response = await context.tbvehiculos.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPut("mtdReactivarVehiculo")]
        public async Task<ActionResult<Vehiculos>> mtdReactivarVehiculo(int id)
        {
            try
            {
                var response = await context.tbvehiculos.FirstOrDefaultAsync(x => x.id == id);
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
