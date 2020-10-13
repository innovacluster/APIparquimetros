using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Services;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IniciarIHostedService : Controller
    {
        private readonly MultaHostedService _multaHostedService;
        public IniciarIHostedService(IHostedService hostedService)
        {
            _multaHostedService = hostedService as MultaHostedService;
        }

        [HttpPut("mtdIniciarIHostedService")]
        public async Task<ActionResult> mtdIniciarIhostedService()
        {
            try
            {

                _multaHostedService.StopAsync(new System.Threading.CancellationToken());
                await _multaHostedService.StartAsync(new System.Threading.CancellationToken());
                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message});
            }
        }
    }
}
