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
    public class TarifasController: Controller
    {
        private readonly ApplicationDbContext context;

        public TarifasController (ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarTarifas")]
        public async Task<ActionResult<IEnumerable<Tarifas>>> mtdConsultarTarifas()
        {
            try
            {

                var response = await context.tbtarifas.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarTarifasXId")]
        public async Task<ActionResult<Tarifas>> mtdConsultarTarifasXId(int id)
        {
            try
            {
                var response = await context.tbtarifas.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpGet("mtdConsultarTarifasXIdConcesion")]
        public async Task<ActionResult<Tarifas>> mtdConsultarTarifasXIdConcesion(int idConcesion)
        {
            try
            {
                var response = await context.tbtarifas.FirstOrDefaultAsync(x => x.intidconcesion_id == idConcesion);
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

        [HttpPost("mtdIngresarTarifas")]
        public async Task<ActionResult<Tarifas>> mtdIngresarTarifas([FromBody] Tarifas tarifas)
        {
            try
            {
                context.tbtarifas.Add(tarifas);
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

        [HttpPut("mtdActualizaTarifas")]
        public async Task<ActionResult> mtdActualizaTarifas(int id, [FromBody] Tarifas tarifas)
        {
            var response = await context.tbtarifas.FirstOrDefaultAsync(x => x.id == id);
            if (response.id != id)
            {
                return BadRequest();
            }
            //response.fltTarifa = tarifas.fltTarifa;
            //response.dtmVigencia = tarifas.dtmVigencia;
            //response.fltIVA = tarifas.fltIVA;
            //response.fltImpuestos = tarifas.fltImpuestos;

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
    }
}
