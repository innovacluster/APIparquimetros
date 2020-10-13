using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
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
    public class ConcesionesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ConcesionesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.context = context;
            _userManager = userManager;
        }

        [HttpGet("mtdConsultarConcesiones")]
        public async Task<ActionResult<IEnumerable<Concesiones>>> mtdConsultarConcesiones()
        {
            try
            {
                var response = await context.tbconcesiones.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarTodoConcesion")]
        public async Task<ActionResult<IEnumerable<ConcesionesConUsers>>> mtdConsultarTodoConcesion(int intIdConcesion)
        {
            try
            {
                var response =  (from concesion in context.tbconcesiones
                                //join users in context.NetUsers on concesion.id equals users.intidconcesion_id
                                //join opciones in context.tbpcionesconcesion on concesion.id equals opciones.int_id_concesion
                                //join tarifas in context.tbtarifas on concesion.id equals tarifas.intidconcesion_id
                                where concesion.id == intIdConcesion
                                select new ConcesionesConUsers()
                                {
                                    id = concesion.id,
                                    str_clave = concesion.str_clave,
                                    str_tipo = concesion.str_tipo,
                                    str_telefono = concesion.str_telefono,
                                    str_email = concesion.str_email,
                                    dtm_fecha_ingreso = concesion.dtm_fecha_ingreso,
                                    bit_status = concesion.bit_status,
                                    str_poligono = concesion.str_poligono,
                                    dbl_costo_licencia = concesion.dbl_costo_licencia,
                                    dtm_fecha_activacion_licencia = concesion.dtm_fecha_activacion_licencia,
                                    int_licencias = concesion.int_licencias,
                                    str_domicilio = concesion.str_domicilio,
                                    str_nombre_cliente = concesion.str_nombre_cliente,
                                    str_notas = concesion.str_notas,
                                    str_rfc = concesion.str_rfc,
                                    str_razon_social = concesion.str_razon_social,
                                    cuentas = context.NetUsers.Where(x => x.intidconcesion_id == intIdConcesion && x.intIdTipoUsuario == 2).ToList(),
                                    opciones = context.tbpcionesconcesion.Where(x => x.int_id_concesion == intIdConcesion).ToList(),
                                    tarifas = context.tbtarifas.Where(x=> x.intidconcesion_id == intIdConcesion).ToList()
                                }).ToList();

                var resultado = response.GroupBy(x => x.id).Select(grp => grp.First()).ToList();
                if (resultado == null)
                {
                    return NotFound();
                }
                return resultado;

            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        [HttpGet("mtdConsultarConcesionesXId")]
        public async Task<ActionResult<Concesiones>> mtdConsultarConcesionesXId(int id)
        {
            try
            {
                var response = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == id);
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

        [HttpGet("mtdConsultarStatusConcesion")]
        public async Task<ActionResult> mtdConsultarStatusConcesion(int id)
        {
            try
            {
                var response = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == id);

                if (response == null)
                {
                    return NotFound();
                }

                return Json(new { bit_status = response.bit_status});
            }
            catch (Exception ex)
            {
                return Json(new { bit_status = ex.Message });
            }
           


        }

        [HttpGet("mtdConsultarPoligonoXIdCiudad")]
        public async Task<ActionResult<Object>> mtdConsultarPoligonoXIdConcesion(int idCiudad)
        {

            try
            {
                var ciudad = await context.tbciudades.FirstOrDefaultAsync(x => x.id == idCiudad);
                // var response = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == ciudad.intidconcesion_id);
                var response = await context.tbconcesiones.FirstOrDefaultAsync(x => x.intidciudad == idCiudad);

                string zona = response.str_poligono.Replace("\r", string.Empty);

                string zona2 = zona.Replace("\n", string.Empty);
                string zona3 = zona2.Replace(@"\", string.Empty);
                string cadena = zona3.Replace(" ", "");

                object zonaf = JsonConvert.DeserializeObject(cadena);

                if (response == null)
                {
                    return NotFound();
                }
                return zonaf;
            }
            catch (Exception ex)
            {

                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }

        }
        [HttpPost("mtdIngresarConcesiones")]
        public async Task<ActionResult<Concesiones>> mtdIngresarConcesiones([FromBody] ConcesionesConUsersIngresar concesiones)
        {
            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();
            string strIdUsuario = ""; 
            string strResult = "";
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            concesiones.dtm_fecha_ingreso = horadeTransaccion.Value;

                            var tipoUsuario = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.strTipoUsuario == "AGENTE VIAL");

                            var concesion =  new Concesiones
                            {
                                str_clave = concesiones.str_clave,
                                str_latitud = concesiones.str_latitud,
                                str_longitud = concesiones.str_longitud,
                                str_razon_social = concesiones.str_razon_social,
                                str_domicilio = concesiones.str_domicilio,
                                str_nombre_cliente = concesiones.str_nombre_cliente,
                                str_telefono = concesiones.str_telefono,
                                str_email = concesiones.str_email,
                                str_rfc = concesiones.str_rfc,
                                str_notas = concesiones.str_notas,
                                str_poligono = concesiones.str_poligono,
                                int_licencias = concesiones.int_licencias,
                                dbl_costo_licencia = concesiones.dbl_costo_licencia,
                                dtm_fecha_ingreso = concesiones.dtm_fecha_ingreso,
                                dtm_fecha_activacion_licencia = concesiones.dtm_fecha_activacion_licencia,
                                str_tipo = concesiones.str_tipo,
                                intidciudad = concesiones.intidciudad,
                                bit_status = true
                            };

                            context.tbconcesiones.Add(concesion);

                            await context.SaveChangesAsync();

                            foreach (var item in concesiones.cuentas)
                            {
                                var user = new ApplicationUser
                                {
                                    UserName = item.Email,
                                    Email = item.Email,
                                    created_date = horadeTransaccion.Value,
                                    intidconcesion_id = concesion.id,
                                    intIdTipoUsuario = tipoUsuario.id,
                                    intidciudad = concesiones.intidciudad,
                                    EmailConfirmed = true,
                                    bit_status = true

                                };
                                strIdUsuario = user.Id;

                                var result= await _userManager.CreateAsync(user, item.PasswordHash);
                                
                            }


                            var result2 = concesiones.opciones;
                            dynamic results = JsonConvert.DeserializeObject(concesiones.opciones.ToString());


                            foreach (var token in results["int_id_opcion"])
                            {
                                context.tbpcionesconcesion.Add(new ConcesionesOpciones()
                                {
                                    int_id_opcion = token,
                                    int_id_concesion = concesion.id
                                });

                                await context.SaveChangesAsync();
                            }
                            //foreach (var item in concesiones.opciones.to)
                            //{

                            //}



                            //foreach (var item in concesiones.opciones)
                            //{
                            //    
                            //}

                            foreach (var item in concesiones.tarifas)
                            {
                                context.tbtarifas.Add(new Tarifas()
                            {
                                //str_tipo = item.str_tipo,
                                flt_tarifa_min = item.flt_tarifa_min,
                                int_tiempo_minimo = item.int_tiempo_minimo,
                                flt_tarifa_max = item.flt_tarifa_max,
                                int_tiempo_maximo = item.int_tiempo_maximo,
                                flt_tarifa_intervalo = item.flt_tarifa_intervalo,
                                int_intervalo_minutos = item.int_intervalo_minutos,
                                bool_cobro_fraccion = item.bool_cobro_fraccion,
                                intidconcesion_id = concesion.id
                                });

                                await context.SaveChangesAsync();
                            }
                          

                            transaction.Commit();
                        }

                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            strResult = ex.Message;
                          
                        }
                    }
                });

            return Json(new { token = strResult });

        }

        [NonAction]
        public  string mtdEncriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        

        [HttpPut("mtdActualizaConcesion")]
        public async Task<ActionResult> mtdActualizaConcesiones(int id, [FromBody] ConcesionesConUsers concesiones)
        {
            string strIdUsuario = "";
            string strResult = "";

            var concesion = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == id);
            var tipoUsuario = await context.tbtiposusuarios.FirstOrDefaultAsync(x => x.strTipoUsuario == "AGENTE VIAL");
            var usuarios = await context.NetUsers.Where(x => x.intidconcesion_id == id && x.intIdTipoUsuario == tipoUsuario.id).ToListAsync();
            var opcionesC = await context.tbpcionesconcesion.Where(x => x.int_id_concesion == id).ToListAsync();
            var tarifaC =  await context.tbtarifas.FirstOrDefaultAsync(x => x.intidconcesion_id == id);

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();

            if (concesion.id != id)
            {
                return BadRequest();
            }
            
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        concesion.str_clave = concesiones.str_clave;
                        concesion.str_latitud = concesiones.str_latitud;
                        concesion.str_longitud = concesiones.str_longitud;
                        concesion.str_razon_social = concesiones.str_razon_social;
                        concesion.str_domicilio = concesiones.str_domicilio;
                        concesion.str_nombre_cliente = concesiones.str_nombre_cliente;
                        concesion.str_telefono = concesiones.str_telefono;
                        concesion.str_email = concesiones.str_email;
                        concesion.str_rfc = concesiones.str_rfc;
                        concesion.str_notas = concesiones.str_notas;
                        concesion.str_poligono = concesiones.str_poligono;
                        concesion.int_licencias = concesiones.int_licencias;
                        concesion.dbl_costo_licencia = concesiones.dbl_costo_licencia;
                        concesion.dtm_fecha_ingreso = concesiones.dtm_fecha_ingreso;
                      
                        
                        await context.SaveChangesAsync();

                        foreach (var item in concesiones.tarifas)
                        {
                            //tarifaC.str_tipo = item.str_tipo;
                            tarifaC.flt_tarifa_min = item.flt_tarifa_min;
                            tarifaC.int_tiempo_minimo = item.int_tiempo_minimo;
                            tarifaC.flt_tarifa_max = item.flt_tarifa_max;
                            tarifaC.int_tiempo_maximo = item.int_tiempo_maximo;
                            tarifaC.flt_tarifa_intervalo = item.flt_tarifa_intervalo;
                            tarifaC.int_intervalo_minutos = item.int_intervalo_minutos;
                            tarifaC.bool_cobro_fraccion = item.bool_cobro_fraccion;
                            //tarifaC.intidconcesion_id = id;
                            await context.SaveChangesAsync();
                        }
                        
                       
                        for (int i = 0; i < usuarios.Count; i++)
                        {
                            foreach (var item in concesiones.cuentas)
                            {

                                usuarios[i].UserName = item.Email;
                                usuarios[i].Email = item.Email;
                                usuarios[i].created_date = horadeTransaccion.Value;
                                usuarios[i].intidconcesion_id = concesion.id;
                                usuarios[i].intIdTipoUsuario = tipoUsuario.id;
                                usuarios[i].intidciudad = concesiones.intidciudad;

                                await context.SaveChangesAsync();
                                await _userManager.RemovePasswordAsync(usuarios[i]);
                                var result = await _userManager.AddPasswordAsync(usuarios[i], item.PasswordHash);

                                i++;
                            }

                        }
                        

                         context.tbpcionesconcesion.RemoveRange(opcionesC);


                        var result2 = concesiones.opciones;
                        dynamic results = JsonConvert.DeserializeObject(concesiones.opciones.ToString());


                        foreach (var token in results["int_id_opcion"])
                        {
                            context.tbpcionesconcesion.Add(new ConcesionesOpciones()
                            {
                                int_id_opcion = token,
                                int_id_concesion = concesion.id
                            });

                            await context.SaveChangesAsync();
                        }

                        //foreach (var item in concesiones.opciones)
                        //{
                        //    context.tbpcionesconcesion.Add(new ConcesionesOpciones()
                        //    {
                        //        int_id_opcion = item.int_id_opcion,
                        //        int_id_concesion = concesion.id
                        //    });

                        //    await context.SaveChangesAsync();
                        //}

                        
                        

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;

                    }
                }
            });

            return Json(new { token = strResult });
        }

        [HttpPut("mtdAgregarLicencias")]
        public async Task<ActionResult> mtdAgregarLicencias(int intidConcesion, [FromBody] AgregarLicencias licencias)
        {
            try
            {
                var response = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == intidConcesion);

                if (response == null)
                {
                    return NotFound();

                }
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerFechaMexico();

                foreach (var item in licencias.cuentas)
                {
                    var user = new ApplicationUser
                    {
                        UserName = item.Email,
                        Email = item.Email,
                        created_date = horadeTransaccion.Value,
                        intidconcesion_id = intidConcesion,
                        intIdTipoUsuario = 3,
                        intidciudad = response.intidciudad,
                        EmailConfirmed = true,
                        bit_status = true

                    };

                    var result = await _userManager.CreateAsync(user, item.PasswordHash);
                }

                return Ok();
            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }

        }


        [HttpDelete("mtdInabilitarConcesion")]
        public async Task<ActionResult<Concesiones>> mtdBajaConcesiones(int id)
        {
            try
            {
                var response = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == id);
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
                return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdReactivarConcesiones")]
        public async Task<ActionResult<Concesiones>> mtdReactivarConcesiones(int id)
        {
            try
            {
                var response = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == id);
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
                return Json(new { token = ex.Message });
            }
        }
    }

}
