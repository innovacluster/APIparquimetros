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
    public class CatCiudadesController : Controller
    {
        private readonly ApplicationDbContext context;
        public CatCiudadesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarCiudades")]
        public async Task<ActionResult<IEnumerable<CatCiudades>>> mtdConsultarCiudades()
        {
            try
            {
                var query = await (from c in context.tbcatciudades
                             where !(from ciudades in context.tbciudades
                                     select ciudades.int_id_ciudad)
                                    .Contains(c.id)
                             select c).ToListAsync();

            


                return query;
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
