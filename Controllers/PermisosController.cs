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
    public class PermisosController:Controller
    {
        private readonly ApplicationDbContext context;

        public PermisosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarPermisos")]
        public async Task<ActionResult<IEnumerable<Permisos>>> mtdConsultarPermisos()
        {
            try
            {
                var response = await context.tbpermisos.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarPermisosXId")]
        public async Task<ActionResult<Permisos>> mtdConsultarPermisosXId(int id)
        {
            try
            {
                var response = await context.tbpermisos.FirstOrDefaultAsync(x => x.id == id);
                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpPost("mtdIngresarPermisos")]
        public async Task<ActionResult<Permisos>> mtdIngresarPermisos([FromBody] Permisos permisos)
        {
            try
            {
                context.tbpermisos.Add(permisos);
                await context.SaveChangesAsync();
                return Ok();
            }

            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }

        [HttpPut("mtdActualizaPermisos")]
        public async Task<ActionResult> mtdActualizaPermisos(int id, [FromBody] Permisos permisos)
        {
            var response = await context.tbpermisos.FirstOrDefaultAsync(x => x.id == id);
            if (response.id != id)
            {
                return BadRequest();
            }
          
            try
            {
                response.id_rol = permisos.id_rol;
                response.id_opcion = permisos.id_opcion;
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }


        }


    }
}
