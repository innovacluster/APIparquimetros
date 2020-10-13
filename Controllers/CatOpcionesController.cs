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
    public class CatOpcionesController : Controller
    {
        public readonly ApplicationDbContext context;

            public CatOpcionesController(ApplicationDbContext context)
            {
                this.context = context;
            }

        [HttpGet("mtdConsultarOpciones")]
        public async Task<ActionResult<IEnumerable<CatalogoOpciones>>> mtdConsultarOpciones()
        {
            try
            {
                var response = await context.tbcatopciones.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
               return Json(new { token = ex.Message});
            }
        }

        


    }
}
