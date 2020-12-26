using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;
using WebApiParquimetros.Services;
using WebApiParquimetros.ViewModels;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : Controller
    {
        //private static readonly string UrlFecha = "https://www.jobtool.online/restapis/servicioEdadGenero/post.php?opcion=30";
        //private static readonly HttpClient client = new HttpClient();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleManager<IdentityUserRole<string>> _userRolManager;
        int intdSaldo = 0;
        private readonly ApplicationDbContext context;

        private readonly ILogger<CuentasController> _logger;

        private readonly IEmailSender _emailSender;
        string strIdUsuario = "";
        public CuentasController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
                  ILogger<CuentasController> logger,
        IConfiguration configuration, IEmailSender emailSender, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            this.context = context;
        }


        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model, string returnUrl)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();


                var usermail = await _userManager.FindByEmailAsync(model.Email);
                var usuario = await _userManager.FindByNameAsync(model.UserName);
                var tipous = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.strTipoUsuario == model.Rol);

                if (usermail == null && usuario == null)
                {
                    //String responseString = await client.GetStringAsync(UrlFecha);
                    //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
                    //string strFecha = fecha.resultado.ToString();

                   // DateTime horadeTransaccion = DateTime.Parse(strFecha);

                    if (tipous.strTipoUsuario == "MOVIL")
                    {
                        ActionResult<bool> ar = await mtdCreaUsuarioMovil(model, horadeTransaccion.Value, tipous.id);
                        if (ar.Value == true)
                        {
                            return Ok();

                        }
                        else { return Json(new { token = "Username or password invalid" }); }
                    }
                    else {
                        var user = new ApplicationUser
                        {
                            strNombre = model.strNombre,
                            strApellidos = model.strApellidos,
                            PhoneNumber = model.PhoneNumber,
                            str_rfc = model.str_rfc,
                            str_direccion = model.str_direccion,
                            str_cp = model.str_cp,
                            str_razon_social = model.str_razon_social,
                            UserName = model.UserName,
                            Email = model.Email,
                            created_by = model.created_by,
                            created_date = horadeTransaccion.Value,
                            last_modified_by = model.last_modified_by,
                            last_modified_date = horadeTransaccion.Value,
                            intidconcesion_id = model.intidconcesion_id,
                            intIdTipoUsuario = tipous.id,
                            intidciudad = model.intidciudad,
                            bit_status = true

                        };
                        strIdUsuario = user.Id;

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                            var confirmationLink = Url.Action("ConfirmarEmail", "Cuentas",
                            new { userId = user.Id, code = code }, Request.Scheme);
                            _logger.Log(LogLevel.Warning, confirmationLink);

                            await _emailSender.SendEmailAsync(user.Email, "Confirme su cuenta de correo",
                               "Por favor confirme su cuenta de correo haciendo clic en el siguiente enlace: <a href=\"" + confirmationLink + "\">link</a>");

                            return Ok();

                        }
                        else
                        {
                            return Json(new { token = "Username or password invalid" });
                        }
                    }
                }

                else
                {
                    return Json(new { token = "El correo o usuario ya se encuentra registrado" });
                }

            }

            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }

        [NonAction]
        public async Task<ActionResult<bool>> mtdCreaUsuarioMovil(UserInfo model, DateTime horadeTransaccion, int intIdTipoUsuario)
        {
            try
            {

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    created_by = model.created_by,
                    created_date = horadeTransaccion,
                    last_modified_by = model.last_modified_by,
                    last_modified_date = horadeTransaccion,
                    intidconcesion_id = null,
                    intIdTipoUsuario = intIdTipoUsuario,
                    str_rfc = model.str_rfc,
                    str_direccion = model.str_direccion,
                    str_cp = model.str_cp,
                    str_razon_social = model.str_razon_social,
                    dbl_saldo_actual = 0.0,
                    dbl_saldo_anterior = 0.0,
                    intidciudad = null,
                    bit_status = true

                };
                strIdUsuario = user.Id;

                var result = await _userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    //int intSaldo = await mtdCrearSaldo(strIdUsuario);

                    //if (intdSaldo != 0)
                    //{
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action("ConfirmarEmail", "Cuentas",
                        new { userId = user.Id, code = code }, Request.Scheme);
                        _logger.Log(LogLevel.Warning, confirmationLink);

                        await _emailSender.SendEmailAsync(user.Email, "Confirme su cuenta de correo",
                           "Por favor confirme su cuenta de correo haciendo clic en el siguiente enlace: <a href=\"" + confirmationLink + "\">link</a>");
                        return true;
                    //}
                 
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }

        }

        [NonAction]
        public  async Task<int> mtdCrearSaldo(string strIdUser)
        {

            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time = par.mtdObtenerFechaMexico(); 
               // DateTime time = DateTime.Now; 

                var saldo = new Saldos
                {
                    created_by = strIdUser,
                    created_date = time.Value,
                    bit_status = true,
                    dtmfecha = time.Value,
                    flt_monto_final = 0.0,
                    flt_monto_inicial = 0.0,
                    str_forma_pago = " ",
                    str_tipo_recarga = " ",
                    int_id_usuario_id = strIdUser,
                    int_id_usuario_trans = strIdUser,
                    intidconcesion_id = null
                };

               await context.AddAsync(saldo);
                await context.SaveChangesAsync();
                intdSaldo = saldo.id;
                return intdSaldo;
            }
            catch (Exception ex)
            {
               return 0;
            }

        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost("CrearUsuariosWeb")]
        //public async Task<ActionResult<UserToken>> CrearUsuariosWeb([FromBody] UserInfo model)
        //{
        //    try
        //    {
        //        ParametrosController par = new ParametrosController(context);
        //        ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

        //        var nomUsu = await context.NetUsers.FirstOrDefaultAsync(x => x.strNombre == model.strNombre && x.strApellidos == model.strApellidos);
        //        var usuario = await _userManager.FindByNameAsync(model.UserName);

        //        var Rol = _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == model.Rol);
        //        var tipous = await context.tbtiposusuarios.FirstOrDefaultAsync(x=> x.strTipoUsuario == model.Rol);


        //        if (nomUsu == null && usuario == null)
        //        {

        //            if (model.intidconcesion_id == 0)
        //            {
        //                var user = new ApplicationUser
        //                {
        //                    strNombre = model.strNombre,
        //                    strApellidos = model.strApellidos,
        //                    PhoneNumber = model.PhoneNumber,
        //                    UserName = model.UserName,
        //                    Email = model.Email,
        //                    created_by = model.created_by,
        //                    created_date = horadeTransaccion.Value,
        //                    last_modified_by = model.last_modified_by,
        //                    last_modified_date = horadeTransaccion.Value,
        //                    intidconcesion_id = null,
        //                    EmailConfirmed = true,
        //                    intIdTipoUsuario = tipous.id,
        //                    intidciudad = 4,
        //                    bit_status = true

        //                };

        //                var result = await _userManager.CreateAsync(user, model.Password);


        //                if (result.Succeeded)
        //                {

        //                    //return BuildToken(model, new List<string>(), user.Id);
        //                    return Ok();
        //                }
        //                else
        //                {

        //                    return Json(new { token = "Username or password invalid" });
        //                }
        //            }
        //            else
        //            {
        //                var user = new ApplicationUser
        //                {
        //                    strNombre = model.strNombre,
        //                    strApellidos = model.strApellidos,
        //                    PhoneNumber = model.PhoneNumber,
        //                    UserName = model.UserName,
        //                    Email = model.Email,
        //                    created_by = model.created_by,
        //                    created_date = horadeTransaccion.Value,
        //                    last_modified_by = model.last_modified_by,
        //                    last_modified_date = horadeTransaccion.Value,
        //                    intidconcesion_id = model.intidconcesion_id,
        //                    EmailConfirmed = true,
        //                    intIdTipoUsuario = tipous.id,
        //                    intidciudad = 4,
        //                    bit_status = true

        //                };

        //                var result = await _userManager.CreateAsync(user, model.Password);


        //                if (result.Succeeded)
        //                {
        //                    // return BuildToken(model, new List<string>(), null);
        //                    return Ok();

        //                }
        //                else
        //                {

        //                    return Json(new { token = "Username or password invalid" });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { token = "El correo o usuario ya se encuentra registrado" });
        //        }

        //    }

        //    catch (Exception ex)
        //    {

        //        return Json(new { token = ex.Message });
        //    }

        //}

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost("CrearRol")]
        //public async Task<ActionResult> CreateRol([FromBody] UserRoles model)
        //{
        //    try
        //    {

        //        var rol = new IdentityRole
        //        {
        //            Name = model.Name


        //        };

        //        var result = await _roleManager.CreateAsync(rol);

        //        if (result.Succeeded)
        //        {
        //            //return Ok("Role Create");
        //            return Json(new { token = "Role Create" });
        //        }
        //        else
        //        {
        //            //return BadRequest("Rol existente");
        //            return Json(new { token = "Rol existente" });

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        //ModelState.AddModelError("token", ex.Message);
        //        //return BadRequest(ModelState);
        //        return Json(new { token = ex.Message });
        //    }
        //}

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpGet("ObtenerRoles")]
        //public async Task<ActionResult<IEnumerable<IdentityRole>>> mtdObtenerRoles()
        //{
        //    try
        //    {
        //        var response = await _roleManager.Roles.ToListAsync();
        //        return response;

        //        //
        //        //var response = new List<UserRoles>();
        //        //result.ForEach(item => response.Add(
        //        //    new UserRoles()
        //        //    {
        //        //        Id = item.Id,
        //        //        Name = item.Name

        //        //    })); ;
        //        //return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ModelState.AddModelError("token", ex.Message);
        //        //return BadRequest(ModelState);
        //        return Json(new { token = ex.Message });
        //    }

        //}

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ConfirmarEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmarEmail(string userId, string code)
        {
            //si no hay usuario o token de verifiacion
            if (userId == null || code == null)
            {
                //ModelState.AddModelError(string.Empty, "ERROR: No hay un link de validacion valido.");
                //return BadRequest(ModelState);
                return Json(new { token = "ERROR: No hay un link de validacion valido." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            //si el usuario es nulo
            if (user == null)
            {
                //ModelState.AddModelError(string.Empty, $"El ID de usuario { userId} no es valido");
                //return BadRequest(ModelState);
                return Json(new { token = $"El ID de usuario { userId} no es valido" });

            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            //si el resultado es exitosoLog
            if (result.Succeeded)
            {
                return View("EmailVerificationConfirmation");
            }
            //_logger.Log(LogLevel.Warning, "Email cannot be confirmed");
            //return RedirectToAction("Error");

            //ModelState.AddModelError("token", "No se puede confirmar el Email.");
            //return BadRequest(ModelState);
            return Json(new { token = "No se puede confirmar el Email." });
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login(string strCliente,[FromBody] UserInfo userInfo)
        {
            try
            {
                var result = await _userManager.FindByNameOrEmailAsync(userInfo.UserName, userInfo.Password);
               
                if (result != null)
                {
                    var resultC = await _signInManager.PasswordSignInAsync(result.UserName, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                    if (resultC.Succeeded)
                 
                    {
                        int inttipoU = result.intIdTipoUsuario;

                        var usuario = await _userManager.FindByNameAsync(result.UserName);

                        if (usuario != null && !usuario.EmailConfirmed &&
                                   (await _userManager.CheckPasswordAsync(usuario, userInfo.Password)))
                        {
                            return Json(new { token = "Email not confirmed yet" });
                        }

                        if (usuario != null)
                        {
                            if (usuario.bit_status)
                            {
                                var tipo = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.id == inttipoU);

                                if (strCliente == "WEB")
                                {
                                    if (tipo.strTipoUsuario == "ADMINISTRADOR DE CONCESION" || tipo.strTipoUsuario == "RADIO OPERADORA" || tipo.strTipoUsuario == "EGRESOS")
                                    {
                                        string strId = result.Id;
                                        var roles = await _userManager.GetRolesAsync(result);
                                        return await BuildToken(userInfo, roles, strId, inttipoU);
                                    }
                                    else
                                    {
                                        return Json(new { token = "No autorizado" });
                                    }

                                }

                                else if (strCliente == "ANDROID" || strCliente == "IOS")
                                {
                                    if (tipo.strTipoUsuario == "MOVIL")
                                    {
                                        string strId = result.Id;
                                        var roles = await _userManager.GetRolesAsync(result);
                                        return await BuildToken(userInfo, roles, strId, inttipoU);
                                    }
                                    else
                                    {
                                        return Json(new { token = "No autorizado" });
                                    }



                                }
                                else if (strCliente == "MONITOREO")
                                {
                                    if (tipo.strTipoUsuario == "AGENTE VIAL")
                                    {
                                        string strId = result.Id;
                                        var roles = await _userManager.GetRolesAsync(result);
                                        return await BuildToken(userInfo, roles, strId, inttipoU);
                                    }
                                    else
                                    {
                                        return Json(new { token = "No autorizado" });
                                    }

                                }
                            }
                            else {
                                return Json(new { token = "Su usuario se encuentra inactivo. Consulte con el equipo de soporte." });
                            }
                            
                            
                           
                            
                        }
                        else
                        {
                            
                            return Json(new { token = "Invalid login attempt." });
                        }  
                    }

                }

                //ModelState.AddModelError("token", "Invalid login attempt.");
                //return BadRequest(ModelState);
                return Json(new { token = "Invalid login attempt." });
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });

            }
        }
         [HttpGet]
        private async Task<UserToken> BuildToken(UserInfo userInfo, IList<string> roles, string Id, int inttipoUsuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                new Claim("miValor", "Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

           
            foreach (var rol in roles)

            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddHours(12);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);
            string strZona = " ";
            Concesiones concesion = null;
            var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == Id);
            var tipo = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.id == inttipoUsuario);
            if (tipo.strTipoUsuario == "AGENTE VIAL")
            {
                var zona = await context.tbzonas.FirstOrDefaultAsync(x => x.id == usuario.intidzona);
                 strZona = zona.str_poligono;
            }
               var permisos = await context.tbpermisos.Where(x => x.id_rol == tipo.id).OrderBy(x => x.id).ToListAsync();

            if (tipo.strTipoUsuario != "MOVIL")
            {
                 concesion = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == usuario.intidconcesion_id);
            }
           

            var ciudad = await context.tbciudades.FirstOrDefaultAsync(x => x.id == usuario.intidciudad);

            if (concesion == null && ciudad == null)
            {
                return new UserToken()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration,
                    Id = Id,
                    permisos = permisos,
                   
                    strRuta = strZona,
                    strNombreUsuario = usuario.strNombre + " " + usuario.strApellidos
                };

            }
            else {

                return new UserToken()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration,
                    Id = Id,
                    permisos = permisos,
                    intIdCiudad = ciudad.id,
                    strCiudad = ciudad.str_ciudad,
                    strLatitud = ciudad.str_latitud,
                    strLongitud = ciudad.str_longitud,
                    intIdConcesion = concesion.id,
                    strConcesion = concesion.str_razon_social,
                    strRuta = strZona,
                    strNombreUsuario = usuario.strNombre + " " + usuario.strApellidos
                };
            }
           
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("mtdConsultarUsuarios")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> mtdConsultarUsuarios()
        {
            try
            {
                var response = await context.NetUsers.Include(x => x.tbtiposusuarios)
                                                      .Include(x => x.tbconcesiones).ToListAsync();

                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });

            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("mtdConsultarUsuariosWeb")]
        public async Task<ActionResult<IEnumerable<ConsultarUsuariosWeb>>> mtdConsultarUsuariosWeb(bool status = true)
        {
            try
            {
                List<ConsultarUsuariosWeb> lstItems = new List<ConsultarUsuariosWeb>();
                var response = await context.NetUsers.Where(x => x.intIdTipoUsuario != 1 && x.bit_status == status).Include(x => x.tbtiposusuarios).Include(x => x.tbconcesiones).ToListAsync(); ;

                
                var parUsuario = await context.tbparametros.FirstOrDefaultAsync(x=> x.intidconcesion_id == null);
                string str_nombrecliente = "";
                int idConcesion = 0;

                foreach (var item in response)
                {
                    if (item.tbconcesiones == null)
                    {
                        str_nombrecliente = parUsuario.str_descrip_us_admin;
                       // item.tbconcesiones.id = 0;

                    }
                    else {
                        str_nombrecliente = item.tbconcesiones.str_nombre_cliente;
                        idConcesion = item.tbconcesiones.id;
                    }

                    var element = new ConsultarUsuariosWeb()
                    {
                        id = item.Id,
                        strNombre = item.strNombre,
                        strApellidos = item.strApellidos,
                        userName = item.UserName,
                        email = item.Email,
                        bit_status = item.bit_status,
                        int_id_concesion = idConcesion,
                        str_nombre_cliente = str_nombrecliente,
                        strTipoUsuario = item.tbtiposusuarios.strTipoUsuario,
                        phoneNumber = item.PhoneNumber,
                        intidciudad = item.intidciudad.Value,
                        intIdTipoUsuario = item.intIdTipoUsuario,
                        str_rfc = item.str_rfc,
                        str_direccion = item.str_direccion,
                        str_cp = item.str_cp,
                        str_razon_social = item.str_razon_social

                    };

                    lstItems.Add(element);
                }
                return lstItems;
            
                
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });

            }

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("mtdConsultarUsuariosXId")]
        public async Task<ActionResult<ApplicationUser>> mtdObtenerUsuariosXId(string id)
        {
            try
            {
                var response = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == id);

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("mtdEditarUsuario")]
        public async Task<ActionResult<ApplicationUser>> EditarUsuario([FromBody] UserInfo model, string id)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();
                var user = await _userManager.FindByIdAsync(id);


                if (user.Id != id)
                {
                    return NotFound();
                }
               

                if (model.intidconcesion_id == 0)
                {


                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.strNombre = model.strNombre;
                    user.strApellidos = model.strApellidos;
                    user.PhoneNumber = model.PhoneNumber;
                    user.last_modified_by = model.last_modified_by;
                    user.intidconcesion_id = model.intidconcesion_id;
                    user.last_modified_date = horadeTransaccion.Value;
                    user.created_by = model.created_by;
                    user.str_rfc = model.str_rfc;
                    user.str_direccion = model.str_direccion;
                    user.str_cp = model.str_cp;
                    user.str_razon_social = model.str_razon_social;
                    user.intidciudad = null;
                    user.intidconcesion_id = null;

                }
                else
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.strNombre = model.strNombre;
                    user.strApellidos = model.strApellidos; 
                    user.PhoneNumber = model.PhoneNumber;
                    user.last_modified_by = model.last_modified_by;
                    user.intidconcesion_id = model.intidconcesion_id;
                    user.last_modified_date =horadeTransaccion.Value;
                    user.created_by = model.created_by;
                    user.str_rfc = model.str_rfc;
                    user.str_direccion = model.str_direccion;
                    user.str_cp = model.str_cp;
                    user.str_razon_social = model.str_razon_social;
                    user.intidciudad = model.intidciudad;
                }


                var result = await _userManager.UpdateAsync(user);
                // await _userManager.RemoveFromRoleAsync(user, model.Rol);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("mtdBajaUsuario")]
        public async Task<ActionResult<ApplicationUser>> mtdBajaUsuario(string id)
        {
            try
            {

                var response = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == id);

                if (response == null)
                {
                    return NotFound();

                }
                response.bit_status = false;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("mtdBajaAgente")]
        public async Task<ActionResult<ApplicationUser>> mtdBajaAgente(string email)
        {
            try
            {

                var response = await context.NetUsers.FirstOrDefaultAsync(x => x.Email == email);

                if (response == null)
                {
                    return NotFound();

                }
                response.bit_status = false;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("mtdReactivarAgente")]
        public async Task<ActionResult<ApplicationUser>> mtdReactivarAgente(string email)
        {
            try
            {

                var response = await context.NetUsers.FirstOrDefaultAsync(x => x.Email == email);

                if (response == null)
                {
                    return NotFound();

                }
                response.bit_status = true;
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


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("CambiarPassword")]
        public async Task<ActionResult<ApplicationUser>> CambiarPassword([FromBody] UserInfo model, string id)
        {
            try
            {
                Match myMatch = System.Text.RegularExpressions.Regex.Match(model.Password, @"^(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$");
                if (!myMatch.Success)
                {
                    return Json(new { token = "Contraseña inválida"});
                }
                else
                {
                    var user = await _userManager.FindByIdAsync(id);
                    await _userManager.RemovePasswordAsync(user);
                    var result = await _userManager.AddPasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);

                return Json(new { token = ex.Message });
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("CambiarPasswordXCorreo")]
        public async Task<ActionResult<ApplicationUser>> CambiarPasswordXCorreo([FromBody] UserInfo model, string email)
        {
            try
            {
                Match myMatch = System.Text.RegularExpressions.Regex.Match(model.Password, @"^(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$");
                if (!myMatch.Success)
                {
                    return Json(new { token = "Contraseña inválida" });
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    await _userManager.RemovePasswordAsync(user);
                    var result = await _userManager.AddPasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);

                return Json(new { token = ex.Message });
            }
        }

        //[HttpPost("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Find the user by email
        //        var user = await _userManager.FindByEmailAsync(model.strCorreo);
        //        // If the user is found AND Email is confirmed
        //        if (user != null && await _userManager.IsEmailConfirmedAsync(user))
        //        {
        //            // Generate the reset password token
        //            var recToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        //           // var pass = await _userManager.PasswordHasher.HashPassword(user);

        //            // Build the password reset link
        //            var passwordResetLink = Url.Action("ResetPassword", "Cuentas",
        //                    new { strCorreo = model.strCorreo, recToken = recToken }, Request.Scheme);

        //            // Log the password reset link
        //            _logger.Log(LogLevel.Warning, passwordResetLink);

        //            //CUERPO DEL MENSAJE
        //            await _emailSender.SendEmailAsync(user.Email, "Reseteo de contrase�a",
        //               "Hola \"" + user + "\". Recibimos una solicitud para recuperar su contrase�a, entre en el siguiente enlace para reestablecerla: <a href=\"" + passwordResetLink + "\">link</a> <br/> Si no fue usted le sugerimos ignore este mensaje.");
        //            // "Recibimos una solicitud para recuperar su contrase�a, entre en el siguiente enlace para reestablecerla: <a href=\"" + passwordResetLink + "\">link</a> <br/> Si no fue usted le sugerimos ignore este mensaje.");
        //            //



        //            // Send the user to Forgot Password Confirmation view
        //            return Ok("Restablecimiento correcto");
        //        }

        //        // To avoid account enumeration and brute force attacks, don't
        //        // reveal that the user does not exist or is not confirmed
        //        return Ok("Restablecimiento correcto");
        //    }

        //    ModelState.AddModelError(string.Empty, "No se enviaron datos.");
        //    return BadRequest(ModelState);
        //}

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("mtdReactivarUsuario")]
        public async Task<ActionResult<ApplicationUser>> mtdReactivarUsuario(string id)
        {
            try
            {
                var response = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == id);

                if (response == null)
                {
                    return NotFound();

                }
                response.bit_status = true;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.email);
                // If the user is found AND Email is confirmed
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var recToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Cuentas",
                            new { strCorreo = model.email, recToken = recToken }, Request.Scheme);

                    // Log the password reset link
                    _logger.Log(LogLevel.Warning, passwordResetLink);

                    //CUERPO DEL MENSAJE
                    await _emailSender.SendEmailAsync(user.Email, "Reseteo de contraseña",
                       "Hola \"" + user + "\". Recibimos una solicitud para recuperar su contraseña, entre en el siguiente enlace para reestablecerla: <a href=\"" + passwordResetLink + "\">link</a> <br/> Si no fue usted le sugerimos ignore este mensaje.");
                    // "Recibimos una solicitud para recuperar su contrase�a, entre en el siguiente enlace para reestablecerla: <a href=\"" + passwordResetLink + "\">link</a> <br/> Si no fue usted le sugerimos ignore este mensaje.");
                    //

                    // Send the user to Forgot Password Confirmation view
                    return Json(new { token = "Restablecimiento correcto" });
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
              
                return Json(new { token = "Restablecimiento correcto" });
            }

            return Json(new { token = "No se enviaron los datos" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string recToken, string strCorreo)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (recToken == null || strCorreo == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            // return View("ForgotPasswordConfirmation");

            ResetPasswordViewModel model = new ResetPasswordViewModel() { strCorreo = strCorreo, recToken = recToken };
            return View(model);
            // return Ok("Vista ResetPassword");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email

                var user = await _userManager.FindByEmailAsync(model.strCorreo);

                if (user != null)
                {
                    // reset the user password
                    var result = await _userManager.ResetPasswordAsync(user, model.recToken, model.Password);
                    if (result.Succeeded)
                    {
                        //return View("ResetPasswordConfirmation");
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return Ok("Error");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }


    }

}
