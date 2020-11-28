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
                var catciudades = await (from c in context.tbcatciudades
                             select c).ToListAsync();

                var ciudades = await (from c in context.tbciudades
                                         select c).ToListAsync();

                var res = from item1 in catciudades
                          where !(ciudades.Any(item2 => item2.int_id_ciudad == item1.id))
                select item1;


                return res.ToList();
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
