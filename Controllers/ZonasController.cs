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
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

using Newtonsoft.Json.Linq;

using WebApiParquimetros.Controllers;
using System.Collections;
using System.Data;

namespace WebApiParkimetrosPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ZonasController : Controller
    {
        public readonly ApplicationDbContext context;
        string strError;

        public ZonasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("mtdConsultarZonas")]
        public async Task<ActionResult<IEnumerable<Zonas>>> mtdConsultarZonas()
        {
            try
            {
                var response = await context.tbzonas.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }

        }
        [HttpGet("mtdObtenerTodasZonas")]
        public async Task<ActionResult<DataTable>> mtdObtenerTodasZonas()
        {
            try
            {
                int intsecciones = 0;
                int intespacios = 0;

                DataTable table = new DataTable("Zonas");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "id";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "str_descripcion";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "str_color";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Object");
                column.ColumnName = "str_poligono";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "int_id_ciudad_id";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "intidconcesion_id";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "str_latitud";
                column.ReadOnly = false;
                table.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "str_longitud";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "intTotalSecciones";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "intTotalEspacios";
                column.ReadOnly = false;
                table.Columns.Add(column);

                var lsZonas = await context.tbzonas.Where(x => x.bit_status == true).ToListAsync();
             
                foreach (var item in lsZonas)
                {
                    var lsZona1 = await context.tbzonas.FirstOrDefaultAsync(x => x.id == item.id);
                    var lsSecciones1 = await context.tbsecciones.Where(x => x.intidzona_id == item.id).ToListAsync();
                    var lsEspacios1 = await context.tbespacios.Where(x => x.id_zona == item.id).ToListAsync();

                    
                    string zona = lsZona1.str_poligono.Replace("\r", string.Empty);

                    string zona2 = zona.Replace("\n", string.Empty);
                   string zona3 = zona2.Replace(@"\",string.Empty);
                    string cadena = zona3.Replace(" ", "");

                    object zonaf = JsonConvert.DeserializeObject(cadena);

                    //JObject obj = JObject.Parse(lsZona1.str_poligono);

                    //string myString = zona.Replace("\r\n", string.Empty);

                    row = table.NewRow();
                    row["id"] = lsZona1.id;
                    row["str_descripcion"] = lsZona1.str_descripcion;
                    row["str_color"] = lsZona1.str_color;
                    row["str_poligono"] = zonaf;
                    row["int_id_ciudad_id"] = lsZona1.int_id_ciudad_id;
                    row["intidconcesion_id"] = lsZona1.intidconcesion_id;
                    row["str_latitud"] = lsZona1.str_latitud;
                    row["str_longitud"] = lsZona1.str_longitud;
                    row["intTotalSecciones"] = lsSecciones1.Count;
                    row["intTotalEspacios"] = lsEspacios1.Count;
                    table.Rows.Add(row);
                }

                return table;

            }

            catch (Exception ex)
            {
                return Json(new { Zonas = ex.Message });
            }

        }

        [HttpGet("mtdConsultarZonasXId")]
        public async Task<ActionResult<Zonas>> mtdConsultarZonasXId(int id)
        {
            try
            {
                var response = await context.tbzonas.FirstOrDefaultAsync(x => x.id == id);
                if (response == null)
                {
                    return NoContent();
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

        [HttpGet("mtdZonasXCiudad")]
        public async Task<ActionResult<ICollection<Zonas>>> mtdZonasXCiudad(int int_id_ciudad_id)
        {

            try
            {
                var response = await context.tbzonas.Where(x => x.int_id_ciudad_id == int_id_ciudad_id).OrderBy(x => x.int_id_ciudad_id).ToListAsync();

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

        private Task<ICollection<Zonas>> AllAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("mtdInsertarZona")]
        public async Task<ActionResult<Zonas>> mtdInsertarZona([FromBody] Zonas zona)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                zona.created_date = horadeTransaccion.Value;
                zona.last_modified_date = horadeTransaccion.Value;
                context.tbzonas.Add(zona);
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

        private static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        [HttpPost("mtdInsertarZonaConSecciones")]
        public async Task<ActionResult<Feacture>> mtdInsertarZonaConSecciones([FromBody] Zonas zona)
        {
            try
            {
                int idZona = 0;
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();
                string jsonFeature = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[-73.989308, 40.741895]},\"properties\":{\"name\":\"New York\"}}";
                // string jsonFeature = zona.str_poligono;
                // var results = JsonConvert.DeserializeObject<Feacture>(zona.str_poligono);
                dynamic results = JsonConvert.DeserializeObject<dynamic>(zona.str_poligono);

                string zon = results.ToString();

                var zona1 = new Zonas()
                {
                    created_by = zona.created_by,
                    created_date = horadeTransaccion.Value,
                    bit_status = true,
                    str_descripcion = zona.str_descripcion,
                    str_color = zona.str_color,
                    str_poligono = zon,
                    int_id_ciudad_id = zona.int_id_ciudad_id,
                    intidconcesion_id = zona.intidconcesion_id,
                    str_latitud = zona.str_latitud,
                    str_longitud = zona.str_longitud

                };
                context.tbzonas.Add(zona1);
                context.SaveChanges();
                idZona = zona1.id;


                foreach (var token in results["features"])
                {
                        string strT = token.ToString();
                        string tipo = token.geometry.type;
                        string strClave = token.properties.desc;
                        //string strColor = token.properties.color;
                        string strPoligono = token.ToString();
                      

                    if (tipo == "LineString")
                    {
                        context.tbsecciones.Add(new Secciones()
                        {
                            str_seccion = strClave,
                            str_color = zona.str_color,
                            str_poligono = strPoligono,
                            intidzona_id = idZona,
                            bit_status = true


                        });

                        context.SaveChanges();
                    }
                    else {

                        var strCor = token.geometry.coordinates;
                        string strL = strCor[1];
                        string strLong = strCor[0];
                        context.tbespacios.Add(new Espacios()
                        {
                            str_clave = strClave,
                            str_marcador = strPoligono,
                            str_color = zona.str_color,
                            bit_ocupado = false,
                            id_zona = idZona,
                            str_latitud = strL,
                            str_longitud = strLong,
                            bit_status = true
                        }); ;

                        context.SaveChanges();
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }


        [HttpPut("mtdModificarZonaConSeciones")]
        public async Task<ActionResult<Zonas>> mtdModificarZona(int intIdZona, [FromBody] Zonas zona)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();
                var response = await context.tbzonas.FirstOrDefaultAsync(x => x.id == intIdZona);

                dynamic results = JsonConvert.DeserializeObject<dynamic>(zona.str_poligono);

                string zon = results.ToString();

                if (response.id != intIdZona)
                {
                    return NotFound();
                }

                response.last_modified_by = zona.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value; ;
                response.str_descripcion = zona.str_descripcion;
                response.str_latitud = zona.str_latitud;
                response.str_longitud = zona.str_longitud;
                response.str_color = zona.str_color;
                response.str_poligono = zon;
                response.int_id_ciudad_id = zona.int_id_ciudad_id;
                response.intidconcesion_id = zona.intidconcesion_id;

                await context.SaveChangesAsync();
                //return Ok();

                context.tbsecciones.Where(x => x.intidzona_id == intIdZona).ToList().ForEach(x => context.tbsecciones.Remove(x));
                await context.SaveChangesAsync();

                context.tbespacios.Where(x => x.id_zona == intIdZona).ToList().ForEach(x => context.tbespacios.Remove(x));
                await context.SaveChangesAsync();

                foreach (var token in results["features"])
                {
                    string strT = token.ToString();
                    string tipo = token.geometry.type;
                    string strClave = token.properties.desc;
                    //string strColor = token.properties.color;
                    string strPoligono = token.ToString();


                    if (tipo == "LineString")
                    {
                        context.tbsecciones.Add(new Secciones()
                        {
                            str_seccion = strClave,
                            str_color = zona.str_color,
                            str_poligono = strPoligono,
                            intidzona_id = intIdZona,
                            bit_status = true

                        });

                        context.SaveChanges();
                    }
                    else
                    {
                        var strCor = token.geometry.coordinates;
                        string strL = strCor[0];
                        string strLong = strCor[1];
                        context.tbespacios.Add(new Espacios()
                        {
                            str_clave = strClave,
                            str_marcador = strPoligono,
                            str_color = zona.str_color,
                            bit_ocupado = false,
                            id_zona = intIdZona,
                            str_latitud = strL,
                            str_longitud = strLong,
                            bit_status = true
                        }); ;

                        context.SaveChanges();
                    }
                }

                return Ok();

            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("token", ex.Message);
                //return BadRequest(ModelState);
                return Json(new { token = ex.Message });
            }
        }

        [HttpPut("mtdBajaZona")]
        public async Task<ActionResult> mtdBajaZona(int intIdZona)
        {
            try
            {
                var response = await context.tbzonas.FirstOrDefaultAsync(x => x.id == intIdZona);

                if (response.id != intIdZona)
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

        [HttpDelete("mtdEliminarZona")]
        public async Task<ActionResult> mtdEliminarZona(int intIdZona)
        {
            try
            {
                var response = await context.tbzonas.FirstOrDefaultAsync(x => x.id == intIdZona);
                var secciones = await context.tbsecciones.Where(x=> x.intidzona_id == intIdZona).ToListAsync();
                var espacios = await context.tbespacios.Where(x => x.id_zona == intIdZona).ToListAsync();

                    if (response.id != intIdZona)
                    {
                        return NotFound();
                    }

                    context.Remove(response);
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
