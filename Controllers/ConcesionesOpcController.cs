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
    public class ConcesionesOpcController: Controller
    {
        public readonly ApplicationDbContext context;

        public ConcesionesOpcController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet("mtdConsultarOpcionesXIdConcesion")]
        public async Task<ActionResult<IEnumerable<ConcesionesOpciones>>> mtdConsultarOpcionesXIdConcesion(int intIdConcesion)
        {
            try
            {
                var response = await context.tbpcionesconcesion.Where(x => x.int_id_concesion == intIdConcesion).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        //[HttpPost("mtdInsertarOpciones")]
        //public async Task<ActionResult> mtdInsertarOpciones()
        //{


        //}

    }
}
