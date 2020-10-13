using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;
using WebApiParquimetros.Services;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParkingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public readonly ApplicationDbContext context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CuentasController> _logger;

        private readonly IEmailSender _emailSender;

        public ParkingController(UserManager<ApplicationUser> userManager,ApplicationDbContext context, IEmailSender emailSender)
        {
            this._userManager = userManager;
            this.context = context;
            this._emailSender = emailSender;
        }
        [HttpPost("mtdInicioParking")]
        public async Task<IActionResult> mtdInicioParking([FromBody] ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.email);
                if (user != null)
                {

                    //CUERPO DEL MENSAJE
                    await _emailSender.SendEmailAsync(user.Email, "Inicio de Parking",
                       "Bienvenido \"" + user + "\"Ha iniciado su tiempo de aparcamiento, le recomedamos estar pendiente de las notificaciones que le llegarán a su teléfono cuando se cerque su tiempo de expiración. <br/> Recuerde que puede extender su tiempo de parqueo si asi lo requiere. ");
                    /// "Recibimos una solicitud para recuperar su contrase�a, entre en el siguiente enlace para reestablecerla: <a href=\"" + passwordResetLink + "\">link</a> <br/> Si no fue usted le sugerimos ignore este mensaje.");
                    //

                    // Send the user to Forgot Password Confirmation view
                    return Json(new { token = "Correo enviado" });
                }
            }
            return Json(new { token = "No se enviaron los datos" });
        }

    }


}
