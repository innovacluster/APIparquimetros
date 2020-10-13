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
    public class EspaciosController : Controller
    {
        public readonly ApplicationDbContext context;

        public EspaciosController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("mtdObtenerTodosEspacios")]
        public async Task<ActionResult<IEnumerable<Espacios>>> mtdObtenerTodosEspacios()
        {
            try
            {
                var response = await context.tbespacios.Include(x => x.tbzonas).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdObtenerEspaciosXId")]
        public async Task<ActionResult<Espacios>> mtdObtenerEspaciosXId(int id)
        {
            try
            {
                var response = await context.tbespacios.FirstOrDefaultAsync(x => x.id == id);
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdObtenerEspaciosXIdZona")]
        public async Task<ActionResult<IEnumerable<Espacios>>> mtdObtenerEspaciosXIdZona(int intIdZona)
        {
            try
            {
                var response = await context.tbespacios.Where(x => x.id_zona == intIdZona && x.bit_status == true && x.bit_ocupado == false).OrderBy(x => x.id_zona).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }
        [HttpPost("mtdInsertaEspacio")]
        public async Task<ActionResult> mtdInsertarEspacio([FromBody] Espacios espacio)
        {
            try
            {
                espacio.bit_status = false;
                context.tbespacios.Add(espacio);
                await context.SaveChangesAsync();
                return Ok();
            } 
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdActualizaEspacio")]
        public async Task<ActionResult> mtdActualizEspacio(int id,[FromBody] Espacios espacio)
        {
            try
            {
                var response = await context.tbespacios.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return NotFound();
                }
                response.str_clave = espacio.str_clave;
                response.str_marcador = espacio.str_marcador;
                response.str_color = espacio.str_color;
                response.id_zona = espacio.id_zona;

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
