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
    public class DetalleMultaController : Controller
    {
        public readonly ApplicationDbContext context;

        public DetalleMultaController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet("mtdConsultarDetMultaXId")]
        public async Task<ActionResult<IEnumerable<DetalleMulta>>> mtdConsultarMultasXId(int intIdMulta)
        {
            try
            {
                var response = await context.tbdetallemulta.Where(x => x.int_id_multa == intIdMulta).ToListAsync();
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
    }
}
