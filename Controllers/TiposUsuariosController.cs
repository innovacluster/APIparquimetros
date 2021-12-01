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
    [Route("api/[Controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TiposUsuariosController: Controller
    {
        public readonly ApplicationDbContext context;

        public TiposUsuariosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdObtenerTiposUsuarios")]
        public async Task<ActionResult<IEnumerable<TiposUsuarios>>> mtdObtenerTiposUsuarios()
        {
            try
            {
                var response = await context.tbtiposusuarios.ToListAsync();
                return response;

            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }

        [HttpGet("mtdObtenerTipoUsuarioXId")]
        public async Task<ActionResult<TiposUsuarios>> mtdObtenerTipoUsuarioXId(int id)
        {
            try
            {
                var response = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpPost("mtdInsertaTipoUsurio")]
        public async Task<ActionResult<TiposUsuarios>> mtdInsertaTipoUsurio([FromBody] TiposUsuarios tipo)
        {
            try
            {
                tipo.bit_status = true;
                context.tbtiposusuarios.Add(tipo);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });

            }

        }

        [HttpPut("mtdActualizaTipoUsuario")]
        public async Task<ActionResult> mtdEditaTipoUsuario(int id, [FromBody] TiposUsuarios tipo)
        {
            try
            {
                var response = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.id == id);

                if (response.id != id)
                {
                    return BadRequest();
                }

                response.strTipoUsuario = tipo.strTipoUsuario;
                response.bit_status = tipo.bit_status;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpDelete("mtdEliminarTipoUsuario")]
        public async Task<ActionResult> mtdEliminaTipoUsuario(int id)
        {
            try
            {
                var response = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.id == id);

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

        [HttpPut("mtdReactivarTipousario")]
        public async Task<ActionResult> mtdReactivarTipousario(int id)
        {
            try
            {
                var response = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.id == id);

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
                return Json(new { token = ex.Message });
            }

        }



    }
}
