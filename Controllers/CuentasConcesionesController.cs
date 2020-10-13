using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CuentasConcesionesController : Controller
    {
        public readonly ApplicationDbContext context;

        public CuentasConcesionesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //[HttpGet("mtdLogin")]
        //public async Task<ActionResult<UserToken>> mtdLogin([FromBody] UsuariosConcesiones user)
        //{
          

        //    var response = await context.tbusersconcesiones.FirstOrDefaultAsync(x => x.str_email == user.str_email);
        //    string pwd = mtdDesEncriptar(response.str_pwd);

        //    if (user.str_pwd == pwd)
        //    {
        //        return await BuildToken();
        //    }

        //    else {
        //        return Json(new { token = "Usuario o contraseña incorrectos" });
        //    }
          
        //}


        [NonAction]
        public string mtdDesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        [NonAction]
        private async Task<UserToken> BuildToken()
        {
            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddHours(12);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               expires: expiration);
           
                return new UserToken()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration,
                   
                };

           
        }
    }
}
