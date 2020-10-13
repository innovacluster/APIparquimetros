using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleManager<IdentityUserRole<string>> _userRolManager;
        

        public List<ApplicationUser> Users { get; private set; }

        public RolesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> rolManager)
        {

            this.context = context;
            this._userManager = userManager;
            this._roleManager = rolManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CrearRol")]
        public async Task<ActionResult> CreateRol([FromBody] UserRoles model)
        {
            try
            {

                var rol = new IdentityRole
                {
                    Name = model.Name

                };

                var result = await _roleManager.CreateAsync(rol);

                if (result.Succeeded)
                {
                    //return Ok("Role Create");
                    return Json(new { token = "Role Create" });
                }
                else
                {
                    //return BadRequest("Rol existente");
                    return Json(new { token = "Rol existente" });
                }
            }
            catch (Exception ex)

            {

                return Json(new { token = ex.Message });
            }
        }


        [HttpGet("mtdObtenerRoles")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> mtdObtenerRoles()
        {
            try
            {
                var response = await _roleManager.Roles.ToListAsync();
                return response;


            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }

        }


        [HttpPost("mtdEditarRol")]
        public async Task<ActionResult<UserRoles>> EditarUsuario([FromBody] RolesInfo model, string id)
        {
            try
            {
                var response = await _roleManager.FindByIdAsync(id);

                if (response == null)
                {
                    return NotFound();
                }

                response.Name = model.Name;

                var result = await _roleManager.UpdateAsync(response);

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

                return Json(new { token = ex.Message });
            }
        }


        [HttpGet("mtdConsultarRolXIdUsuario")]
        public async Task<IList<string>> mtdConsultarRolXIdUsuario(string Id)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(Id);
                return await _userManager.GetRolesAsync(usuario);

            }
            catch (Exception ex)
            {
              
                return null;
            


            }

        }

        //[HttpGet("mtdUsersRoles")]
        //public async Task<ActionResult<ApplicationUser>> mtdUserRoles()
        //{
          
        //  var usersAndRoles = new List<UserInfo>(); // Adding this model just to have it in a nice list.
        //  var users = context.NetUsers;
     

        //    foreach (var user in users)
        //        {
        //            foreach (var role in user.roles)
        //            {
        //                usersAndRoles.Add(new UserInfo
        //                {
        //                    UserName = user.UserName,
        //                    Rol = role.Name
        //                };
        //            }
        //        }
            



        //}

    //    [HttpPost("OnPostGetPagination")]
    //    public async Task<ActionResult<ListUserViewModel>> OnPostGetPagination()
    //    {
    //        //IList lstUserRol = null;

    //        var users = await _userManager.Users.ToListAsync();
    //        var result = new List<UserGrid>();

    //        foreach (var v in users)
    //        {
    //            string str = "";

    //            var roles = await _userManager.GetRolesAsync(v);



    //            foreach (var r in roles)
    //            {
    //                var lista = new UserInfo
    //                {
    //                    UserName = users.Us,
    //                    Rol = role.Name
    //                };


    //        }
    //        roles.Add(str);
    //        var model = new ListUserViewModel()
    //        {
    //            users = users.ToList(),
    //            roles = roles.ToList()
    //        };

    //    }
    //        return Ok();

    //}


    //[HttpGet("Obtener")]
    //public async Task<IEnumerable> Obtener(string Id)
    //{
    //    var usuario = await _userManager.FindByIdAsync(Id);

    //    if (usuario != null)
    //    {
    //        return await _userManager.GetRolesAsync(usuario);
    //    }


    //    ////var users= _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToList();
    //    //this.Users = _userManager.Users.Include(await _userManager.GetRolesAsync(v)).ToList();
    //    //return this.Users;

    //}


    //[HttpGet("mtdObtenerRolUsu")]
    //public async Task<IEnumerable> mtddbtenerRolUsu()
    //{
    //    var users = await _userManager.Users.ToListAsync();

    //    //var UserInfo = users.Select(user => new
    //    //{
    //    //    UserName = user.UserName,
    //    //    Rol = user.UserRoles.Where(role => user.UserRoles.Any(userRole => userRole.UserId == user.Id && userRole.RoleId == role.RoleId))
    //    //});
    //    //return UserInfo;


    //    var roles = (from role in context.Roles
    //                 let userRoles = context.UserRoles.Where(ur => ur.UserId == users.).Select(ur => ur.RoleId)
    //                 where userRoles.Contains(role.Id)
    //                 select role
    //    ).ToList();

    //    //foreach (var user in context.NetUsers.Include(u => u.UserRoles).ToList())
    //    //{
    //    //    list.Add(new UserInfo
    //    //    {
    //    //        UserName  = user.UserName,
    //    //        Rol = user.UserRoles
    //    //    });
    //    //}

    //    //var resultado = await context.ApplicationUserRoles.Where(x => x.UserId == Id).OrderBy(x => x.UserId).ToListAsync();


    //    //userManager = _userManager;

    //    //List<string> userids = context.UserRoles.Where(a => a.RoleId == "").Select(b => b.UserId).ToList();
    //    ////The first step: get all user id collection as userids based on role from db.UserRoles

    //    //List<ApplicationUser> listUsers = context.Users.Where(a => userids.Any(c => c == a.Id)).ToList();


    //    //return listUsers;
    //}

    //public virtual ActionResult ListUser()
    //{
    //    var users = UserManager.Users;
    //    var roles = new List<string>();
    //    foreach (var user in users)
    //    {
    //        string str = "";
    //        foreach (var role in UserManager.GetRoles(user.Id))
    //        {
    //            str = (str == "") ? role.ToString() : str + " - " + role.ToString();
    //        }
    //        roles.Add(str);
    //    }
    //    var model = new ListUserViewModel()
    //    {
    //        users = users.ToList(),
    //        roles = roles.ToList()
    //    };
    //    return View(model);
    //}
}
}
