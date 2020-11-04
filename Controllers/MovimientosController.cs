using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;
using WebApiParquimetros.Services;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MovimientosController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext context;
        Boolean bolDevolucion = false;
       // private static readonly string UrlFecha = "https://www.jobtool.online/restapis/servicioEdadGenero/post.php?opcion=30";
       // private static readonly HttpClient client = new HttpClient();
        private readonly string _connectionString;
        public MovimientosController(ApplicationDbContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            this.context = context;
            _emailSender = emailSender;
            _connectionString = configuration.GetConnectionString("DefaultConnectionString");

        }

        /// <summary>
        /// Método get que obtiene todos los movimientos activos e inactivos existentes en la bd.
        /// </summary>
        /// <returns></returns>
        [HttpGet("mtdConsultarMovimientos")]
        public async Task<ActionResult<IEnumerable<Movimientos>>> mtdConsultarMovimientos()
        {
            try
            {
                var response = await context.tbmovimientos.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        /// <summary>
        /// Método que permite obtener los movimientos activos.
        /// </summary>
        /// <returns></returns>
        [HttpGet("mtdConsultarMovimientosActivos")]
        public async Task<ActionResult<IEnumerable<Movimientos>>> mtdConsultarMovimientosActivos(DateTime dtmFecha)
        {
            try
            {
                // var response = await context.tbespacios.Include(x => x.tbzonas).ToListAsync();
                var response = await context.tbmovimientos.Where(x => x.dtm_hora_fin.Date == dtmFecha.Date & x.bit_status == true)
                                    .Include(x => x.tbvehiculos).Include(y => y.tbespacios).OrderBy(x => x.id).ToListAsync();

                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        /// <summary>
        /// Método Get que obtiene un movimiento de acuerdo al id solicitado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("mtdConsultarMovimientosXId")]//ObtenerMovimientosXId
        public async Task<ActionResult<Movimientos>> mtdConsultarMovimientosXId(int id)
        {
            try
            {
                var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == id);
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


        [HttpGet("mtdObtenerMovimientosUsuario")]
        public async Task<ActionResult<List<Movimientos>>> mtdObtenerMovimientosUsuario(string idUsuario, int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

               // DateTime time = DateTime.Now;

                var response = await context.tbmovimientos.Where(x => x.int_id_usuario_id == idUsuario && x.dt_hora_inicio.Date.Month == time.Value.Date.Month).ToListAsync();
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


        [HttpGet("mtdConsultarMovimientosXId2")]
        public async Task<ActionResult<List<MovimientosXId>>> mtdConsultarMovimientosXId2(int id)
        {
            try
            {
                //var movimiento = await context.tbmovimientos.FirstOrDefaultAsync(x=> x.id == id);

                //var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == movimiento.int_id_espacio);

                var response = (from mov in context.tbmovimientos
                                join esp in context.tbespacios on mov.int_id_espacio equals esp.id
                                join zonad in context.tbzonas on esp.id_zona equals zonad.id
                                where mov.id == id
                                select new MovimientosXId()
                                {
                                    id = mov.id,
                                    created_by = mov.created_by,
                                    created_date = mov.created_date.Value,
                                    last_modified_by = mov.last_modified_by,
                                    last_modified_date = mov.last_modified_date,
                                    bit_status = mov.bit_status,
                                    str_placa = mov.str_placa,
                                    boolean_auto_recarga = mov.boolean_auto_recarga,
                                    boolean_multa = mov.boolean_multa,
                                    dt_hora_inicio = mov.dt_hora_inicio,
                                    dtm_fecha_insercion_descuento = mov.dtm_fecha_insercion_descuento,
                                    dtm_fecha_descuento = mov.dtm_fecha_descuento,
                                    dtm_hora_fin = mov.dtm_hora_fin,
                                    flt_moneda_saldo_previo_descuento = mov.flt_moneda_saldo_previo_descuento,
                                    flt_monto = mov.flt_monto,
                                    flt_saldo_previo_descuento = mov.flt_saldo_previo_descuento,
                                    flt_valor_descuento = mov.flt_valor_descuento,
                                    flt_valor_devuelto = mov.flt_valor_devuelto,
                                    flt_valor_final_descuento = mov.flt_valor_final_descuento,
                                    str_cambio_descuento = mov.str_cambio_descuento,
                                    str_codigo_autorizacion = mov.str_codigo_autorizacion,
                                    str_codigo_transaccion = mov.str_codigo_transaccion,
                                    str_comentarios = mov.str_comentarios,
                                    str_hash_tarjeta = mov.str_hash_tarjeta,
                                    str_instalacion = mov.str_instalacion,
                                    str_instalacion_abrv = mov.str_instalacion_abrv,
                                    str_moneda_valor_descuento = mov.str_moneda_valor_descuento,
                                    str_referencia_operacion = mov.str_referencia_operacion,
                                    str_so = mov.str_so,
                                    str_tipo = mov.str_tipo,
                                    str_versionapp = mov.str_versionapp,
                                    int_id_zona = zonad.id,
                                    str_nombre_zona = zonad.str_descripcion,
                                    int_id_espacio = mov.int_id_espacio.Value,
                                    //int_id_saldo_id = mov.int_id_saldo_id,
                                    int_id_usuario_id = mov.int_id_usuario_id,
                                    int_id_vehiculo_id = mov.int_id_vehiculo_id,
                                    intidconcesion_id = mov.intidconcesion_id,
                                    int_tiempo = mov.int_tiempo,
                                    int_id_multa = mov.int_id_multa
                                }).OrderByDescending(x => x.dt_hora_inicio).ToList();

                //}

                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarMovimientosXId2SE")]
        public async Task<ActionResult<List<MovimientosXId>>> mtdConsultarMovimientosXId2SE(int id)
        {
            try
            {
                var response = (from mov in context.tbmovimientos
                                where mov.id == id
                                select new MovimientosXId()
                                {
                                    id = mov.id,
                                    created_by = mov.created_by,
                                    created_date = mov.created_date.Value,
                                    last_modified_by = mov.last_modified_by,
                                    last_modified_date = mov.last_modified_date,
                                    bit_status = mov.bit_status,
                                    str_placa = mov.str_placa,
                                    boolean_auto_recarga = mov.boolean_auto_recarga,
                                    boolean_multa = mov.boolean_multa,
                                    dt_hora_inicio = mov.dt_hora_inicio,
                                    dtm_fecha_insercion_descuento = mov.dtm_fecha_insercion_descuento,
                                    dtm_fecha_descuento = mov.dtm_fecha_descuento,
                                    dtm_hora_fin = mov.dtm_hora_fin,
                                    flt_moneda_saldo_previo_descuento = mov.flt_moneda_saldo_previo_descuento,
                                    flt_monto = mov.flt_monto,
                                    flt_saldo_previo_descuento = mov.flt_saldo_previo_descuento,
                                    flt_valor_descuento = mov.flt_valor_descuento,
                                    flt_valor_devuelto = mov.flt_valor_devuelto,
                                    flt_valor_final_descuento = mov.flt_valor_final_descuento,
                                    str_cambio_descuento = mov.str_cambio_descuento,
                                    str_codigo_autorizacion = mov.str_codigo_autorizacion,
                                    str_codigo_transaccion = mov.str_codigo_transaccion,
                                    str_comentarios = mov.str_comentarios,
                                    str_hash_tarjeta = mov.str_hash_tarjeta,
                                    str_instalacion = mov.str_instalacion,
                                    str_instalacion_abrv = mov.str_instalacion_abrv,
                                    str_moneda_valor_descuento = mov.str_moneda_valor_descuento,
                                    str_referencia_operacion = mov.str_referencia_operacion,
                                    str_so = mov.str_so,
                                    str_tipo = mov.str_tipo,
                                    str_versionapp = mov.str_versionapp,
                                    str_latitud = mov.str_latitud,
                                    str_longitud = mov.str_longitud,
                                   // int_id_saldo_id = mov.int_id_saldo_id.Value,
                                    int_id_usuario_id = mov.int_id_usuario_id,
                                    int_id_vehiculo_id = mov.int_id_vehiculo_id,
                                    intidconcesion_id = mov.intidconcesion_id,
                                    int_tiempo = mov.int_tiempo,
                                    int_id_multa = mov.int_id_multa
                                }).OrderByDescending(x => x.dt_hora_inicio).ToList();

                //}

                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarMovimientosActivosXIdUsuario")]
        public async Task<ActionResult<List<MovimientosActivosInactivos>>> mtdConsultarMovimientosXId(string strIdUsuario)
        {
            try
            {

                var response = (from mov in context.tbmovimientos
                                join esp in context.tbespacios on mov.int_id_espacio equals esp.id
                                join zonad in context.tbzonas on esp.id_zona equals zonad.id
                                where mov.int_id_usuario_id == strIdUsuario && mov.bit_status == true
                                select new MovimientosActivosInactivos()
                                {
                                    id = mov.id,
                                    created_by = mov.created_by,
                                    created_date = mov.created_date.Value,
                                    last_modified_by = mov.last_modified_by,
                                    last_modified_date = mov.last_modified_date,
                                    bit_status = mov.bit_status,
                                    str_placa = mov.str_placa,
                                    boolean_auto_recarga = mov.boolean_auto_recarga,
                                    boolean_multa = mov.boolean_multa,
                                    dt_hora_inicio = mov.dt_hora_inicio,
                                    dtm_fecha_insercion_descuento = mov.dtm_fecha_insercion_descuento,
                                    dtm_fecha_descuento = mov.dtm_fecha_descuento,
                                    dtm_hora_fin = mov.dtm_hora_fin,
                                    flt_moneda_saldo_previo_descuento = mov.flt_moneda_saldo_previo_descuento,
                                    flt_monto = mov.flt_monto,
                                    flt_saldo_previo_descuento = mov.flt_saldo_previo_descuento,
                                    flt_valor_descuento = mov.flt_valor_descuento,
                                    flt_valor_devuelto = mov.flt_valor_devuelto,
                                    flt_valor_final_descuento = mov.flt_valor_final_descuento,
                                    str_cambio_descuento = mov.str_cambio_descuento,
                                    str_codigo_autorizacion = mov.str_codigo_autorizacion,
                                    str_codigo_transaccion = mov.str_codigo_transaccion,
                                    str_comentarios = mov.str_comentarios,
                                    str_hash_tarjeta = mov.str_hash_tarjeta,
                                    str_instalacion = mov.str_instalacion,
                                    str_instalacion_abrv = mov.str_instalacion_abrv,
                                    str_moneda_valor_descuento = mov.str_moneda_valor_descuento,
                                    str_referencia_operacion = mov.str_referencia_operacion,
                                    str_so = mov.str_so,
                                    str_tipo = mov.str_tipo,
                                    str_versionapp = mov.str_versionapp,
                                    str_nombre_zona = zonad.str_descripcion,
                                    int_id_espacio = mov.int_id_espacio.Value,
                                    int_id_saldo_id = mov.int_id_saldo_id.Value,
                                    int_id_usuario_id = mov.int_id_usuario_id,
                                    int_id_vehiculo_id = mov.int_id_vehiculo_id,
                                    intidconcesion_id = mov.intidconcesion_id,
                                    int_tiempo = mov.int_tiempo,
                                    int_id_multa = mov.int_id_multa
                                }).OrderByDescending(x => x.dt_hora_inicio).ToList();

                //}

                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        /// <summary>
        ///  Metodo para la consulta de movimientos activos X id de usuario. Esta consulta es para registros que en donde la cocesionaria tiene espacios.
        /// </summary>
        /// <param name="strIdUsuario"></param>
        /// <returns></returns>
        [HttpGet("mtdConsultarMovimientosActivosXIdUsuarioSE")]
        public async Task<ActionResult<List<MovimientosActivosInactivos>>> mtdConsultarMovimientosXIdSE(string strIdUsuario)
        {
            try
            {
                var response = (from mov in context.tbmovimientos
                                where mov.int_id_usuario_id == strIdUsuario && mov.bit_status == true
                                select new MovimientosActivosInactivos()
                                {
                                    id = mov.id,
                                    created_by = mov.created_by,
                                    created_date = mov.created_date.Value,
                                    last_modified_by = mov.last_modified_by,
                                    last_modified_date = mov.last_modified_date,
                                    bit_status = mov.bit_status,
                                    str_placa = mov.str_placa,
                                    boolean_auto_recarga = mov.boolean_auto_recarga,
                                    boolean_multa = mov.boolean_multa,
                                    dt_hora_inicio = mov.dt_hora_inicio,
                                    dtm_fecha_insercion_descuento = mov.dtm_fecha_insercion_descuento,
                                    dtm_fecha_descuento = mov.dtm_fecha_descuento,
                                    dtm_hora_fin = mov.dtm_hora_fin,
                                    flt_moneda_saldo_previo_descuento = mov.flt_moneda_saldo_previo_descuento,
                                    flt_monto = mov.flt_monto,
                                    flt_saldo_previo_descuento = mov.flt_saldo_previo_descuento,
                                    flt_valor_descuento = mov.flt_valor_descuento,
                                    flt_valor_devuelto = mov.flt_valor_devuelto,
                                    flt_valor_final_descuento = mov.flt_valor_final_descuento,
                                    str_cambio_descuento = mov.str_cambio_descuento,
                                    str_codigo_autorizacion = mov.str_codigo_autorizacion,
                                    str_codigo_transaccion = mov.str_codigo_transaccion,
                                    str_comentarios = mov.str_comentarios,
                                    str_hash_tarjeta = mov.str_hash_tarjeta,
                                    str_instalacion = mov.str_instalacion,
                                    str_instalacion_abrv = mov.str_instalacion_abrv,
                                    str_moneda_valor_descuento = mov.str_moneda_valor_descuento,
                                    str_referencia_operacion = mov.str_referencia_operacion,
                                    str_so = mov.str_so,
                                    str_tipo = mov.str_tipo,
                                    str_versionapp = mov.str_versionapp,
                                    str_latitud = mov.str_latitud,
                                    str_longitud = mov.str_longitud,
                                    //int_id_saldo_id = mov.int_id_saldo_id.Value,
                                    int_id_usuario_id = mov.int_id_usuario_id,
                                    int_id_vehiculo_id = mov.int_id_vehiculo_id,
                                    intidconcesion_id = mov.intidconcesion_id,
                                    int_tiempo = mov.int_tiempo,
                                    int_id_multa = mov.int_id_multa
                                }).OrderByDescending(x => x.dt_hora_inicio).ToList();

                //}

                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarMovimientosInactivosXIdUsuario")]
        public async Task<ActionResult<ICollection<MovimientosActivosInactivos>>> mtdConsultarMovimientosInactivosXIdUsuario(string strIdUsuario)
        {
            try
            {

                var response = (from mov in context.tbmovimientos
                                join esp in context.tbespacios on mov.int_id_espacio equals esp.id
                                join zonad in context.tbzonas on esp.id_zona equals zonad.id
                                where mov.int_id_usuario_id == strIdUsuario && mov.bit_status == false
                                select new MovimientosActivosInactivos()
                                {
                                    id = mov.id,
                                    created_by = mov.created_by,
                                    created_date = mov.created_date.Value,
                                    last_modified_by = mov.last_modified_by,
                                    last_modified_date = mov.last_modified_date,
                                    bit_status = mov.bit_status,
                                    str_placa = mov.str_placa,
                                    boolean_auto_recarga = mov.boolean_auto_recarga,
                                    boolean_multa = mov.boolean_multa,
                                    dt_hora_inicio = mov.dt_hora_inicio,
                                    dtm_fecha_insercion_descuento = mov.dtm_fecha_insercion_descuento,
                                    dtm_fecha_descuento = mov.dtm_fecha_descuento,
                                    dtm_hora_fin = mov.dtm_hora_fin,
                                    flt_moneda_saldo_previo_descuento = mov.flt_moneda_saldo_previo_descuento,
                                    flt_monto = mov.flt_monto,
                                    flt_saldo_previo_descuento = mov.flt_saldo_previo_descuento,
                                    flt_valor_descuento = mov.flt_valor_descuento,
                                    flt_valor_devuelto = mov.flt_valor_devuelto,
                                    flt_valor_final_descuento = mov.flt_valor_final_descuento,
                                    str_cambio_descuento = mov.str_cambio_descuento,
                                    str_codigo_autorizacion = mov.str_codigo_autorizacion,
                                    str_codigo_transaccion = mov.str_codigo_transaccion,
                                    str_comentarios = mov.str_comentarios,
                                    str_hash_tarjeta = mov.str_hash_tarjeta,
                                    str_instalacion = mov.str_instalacion,
                                    str_instalacion_abrv = mov.str_instalacion_abrv,
                                    str_moneda_valor_descuento = mov.str_moneda_valor_descuento,
                                    str_referencia_operacion = mov.str_referencia_operacion,
                                    str_so = mov.str_so,
                                    str_tipo = mov.str_tipo,
                                    str_versionapp = mov.str_versionapp,
                                    str_nombre_zona = zonad.str_descripcion,
                                    int_id_espacio = mov.int_id_espacio.Value,
                                   // int_id_saldo_id = mov.int_id_saldo_id.Value,
                                    int_id_usuario_id = mov.int_id_usuario_id,
                                    int_id_vehiculo_id = mov.int_id_vehiculo_id,
                                    intidconcesion_id = mov.intidconcesion_id,
                                    int_tiempo = mov.int_tiempo,
                                    int_id_multa = mov.int_id_multa
                                }).OrderByDescending(x => x.dt_hora_inicio).ToList();
                //}

                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        /// <summary>
        /// Metodo para la consulta de movimientos inactivos X id de usuario. Esta consulta es para registros que no tiene asocido un id de espacio.
        /// </summary>
        /// <param name="strIdUsuario"></param>
        /// <returns></returns>
        [HttpGet("mtdConsultarMovimientosInactivosXIdUsuarioSE")]
        public async Task<ActionResult<ICollection<MovimientosActivosInactivos>>> mtdConsultarMovimientosInactivosXIdUsuarioSE(string strIdUsuario)
        {
            try
            {

                var response = (from mov in context.tbmovimientos
                                where mov.int_id_usuario_id == strIdUsuario && mov.bit_status == false
                                select new MovimientosActivosInactivos()
                                {
                                    id = mov.id,
                                    created_by = mov.created_by,
                                    created_date = mov.created_date.Value,
                                    last_modified_by = mov.last_modified_by,
                                    last_modified_date = mov.last_modified_date,
                                    bit_status = mov.bit_status,
                                    str_placa = mov.str_placa,
                                    boolean_auto_recarga = mov.boolean_auto_recarga,
                                    boolean_multa = mov.boolean_multa,
                                    dt_hora_inicio = mov.dt_hora_inicio,
                                    dtm_fecha_insercion_descuento = mov.dtm_fecha_insercion_descuento,
                                    dtm_fecha_descuento = mov.dtm_fecha_descuento,
                                    dtm_hora_fin = mov.dtm_hora_fin,
                                    flt_moneda_saldo_previo_descuento = mov.flt_moneda_saldo_previo_descuento,
                                    flt_monto = mov.flt_monto,
                                    flt_saldo_previo_descuento = mov.flt_saldo_previo_descuento,
                                    flt_valor_descuento = mov.flt_valor_descuento,
                                    flt_valor_devuelto = mov.flt_valor_devuelto,
                                    flt_valor_final_descuento = mov.flt_valor_final_descuento,
                                    str_cambio_descuento = mov.str_cambio_descuento,
                                    str_codigo_autorizacion = mov.str_codigo_autorizacion,
                                    str_codigo_transaccion = mov.str_codigo_transaccion,
                                    str_comentarios = mov.str_comentarios,
                                    str_hash_tarjeta = mov.str_hash_tarjeta,
                                    str_instalacion = mov.str_instalacion,
                                    str_instalacion_abrv = mov.str_instalacion_abrv,
                                    str_moneda_valor_descuento = mov.str_moneda_valor_descuento,
                                    str_referencia_operacion = mov.str_referencia_operacion,
                                    str_so = mov.str_so,
                                    str_tipo = mov.str_tipo,
                                    str_versionapp = mov.str_versionapp,
                                    str_latitud = mov.str_latitud,
                                    str_longitud = mov.str_longitud,
                                    //int_id_saldo_id = mov.int_id_saldo_id.Value,
                                    int_id_usuario_id = mov.int_id_usuario_id,
                                    int_id_vehiculo_id = mov.int_id_vehiculo_id,
                                    intidconcesion_id = mov.intidconcesion_id,
                                    int_tiempo = mov.int_tiempo,
                                    int_id_multa = mov.int_id_multa
                                }).OrderByDescending(x => x.dt_hora_inicio).ToList();
                //}

                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }


        [HttpGet("mtdConsultarXDia")]
        public async Task<ActionResult<ICollection<Movimientos>>> mtdConsultarXDia(DateTime dtmFecha)
        {

            try
            {
                var response = await context.tbmovimientos.Where(x => x.dt_hora_inicio.Date == dtmFecha.Date).OrderBy(x => x.id).OrderBy(x => x.dt_hora_inicio).ToListAsync();
                if (response == null)
                {
                    return NotFound();
                }
                return response;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        //[HttpGet("mtdConsultarXRangoFechas")]
        //public async Task<ActionResult<ICollection<MovimientosJoin>>> mtdConsultarXRangoFechas(DateTime dtmFechaInicio, DateTime dtmFechaFin)
        //{

        //    try
        //    {
        //        // var par = await context.tbparametros.FirstOrDefaultAsync();
        //        var result = (from mov in context.tbmovimientos
        //                      join esp in context.tbespacios on mov.int_id_espacio equals esp.id
        //                      join users in context.NetUsers on mov.int_id_usuario_id equals users.Id
        //                      join zona in context.tbzonas on esp.id_zona equals zona.id
        //                      join detalle in context.tbdetallemovimientos on mov.id equals detalle.int_idmovimiento
        //                      where mov.dt_hora_inicio.Date <= dtmFechaInicio.Date && mov.dt_hora_inicio.Date >= dtmFechaFin.Date
        //                      select new MovimientosJoin()
        //                      {
        //                          id = mov.id,
        //                          str_status = "",
        //                          str_placa = mov.str_placa,
        //                          int_tiempo = mov.int_tiempo,
        //                          dt_hora_inicio = mov.dt_hora_inicio,
        //                          dtm_hora_fin = mov.dtm_hora_fin,
        //                          int_timpo_restante = 0,
        //                          str_tiemporest = "",
        //                          str_tiempo = "",
        //                          int_id_espacio = esp.id,
        //                          str_clave_esp = esp.str_clave,
        //                          str_marcador = esp.str_marcador,
        //                          str_descripcion_zona = zona.str_descripcion,
        //                          str_comentarios = mov.str_comentarios,
        //                          Email = users.Email,
        //                          str_nombre_completo = users.strNombre + ' ' + users.strApellidos,
        //                          str_rfc = users.str_rfc,
        //                          str_razon_social = users.str_razon_social,
        //                          bit_status = mov.bit_status,
        //                          boolean_auto_recarga = mov.boolean_auto_recarga,
        //                          detalleMovimientos = context.tbdetallemovimientos.Where(x => x.int_idmovimiento == mov.id).ToList(),
        //                          //datosUsuario = context.NetUsers.Where(x=> x.Id == mov.int_id_usuario_id).ToList()
        //                      }).Distinct().ToList();

        //        var resultado = result.GroupBy(x => x.id).Select(grp => grp.First()).ToList(); //18800 ticks

        //        //lstMovJ = response;
        //        if (result == null)
        //        {
        //            return NotFound();
        //        }
        //        return resultado;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { token = ex.Message });
        //    }

        //}

        /// <summary>
        /// Método que obtiene los movimientos de acuerdo a una fecha en especifica.
        /// </summary>
        /// <param name="dtmFecha"></param>
        /// <param name="dtmFechaFin"></param>           
        /// <returns></returns>
        [HttpGet("mtdConsultarMovMonitoreoXDia")]
        public async Task<ActionResult<ICollection<MovimientosJoin>>> mtdConsultarMovMonitoreoXDiaPrueba(string dtmFecha, string dtmFechaFin, int intIdConcesion)
        {
            try
            {
                if (dtmFechaFin == null)
                {


                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarmovmonitoreoxdia('" + dtmFecha + "'" +","+ intIdConcesion+")", sql))
                        {
                            var response = new List<MovimientosJoin>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueMovimientos(drd));
                            }
                            return response;

                        }
                    }

                }
                else
                {

                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarmovmonitoreoxfechas('" + dtmFecha + "'" + "," + "'" + dtmFechaFin + "'" + ","+ intIdConcesion+")", sql))
                        {
                            var response = new List<MovimientosJoin>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueMovimientos2(drd));
                            }
                            return response;

                        }


                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdConsultarTodosMovimientos")]
        public async Task<ActionResult<ICollection<Movimientos>>> mtdConsultarTodosMovimientos(string dtmFecha, string dtmFechaFin, int intIdConcesion)
        {
            try
            {
                
                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultartodomovimientos('" + dtmFecha + "'" + "," + "'" + dtmFechaFin + "'" + "," + intIdConcesion + ")", sql))
                        {
                            var response = new List<Movimientos>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueMovimientosMovTodo(drd));
                            }
                            return response;

                        }


                    }
                

            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }


        [NonAction]
        private MovimientosJoin MapToValueMovimientos(NpgsqlDataReader reader)
        {
            return new MovimientosJoin()
            {
                id = reader["id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["id"],
                str_status = "",
                str_placa = reader["str_placa"].ToString(),
                int_tiempo = reader["int_tiempo"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_tiempo"],
                flt_monto = reader["flt_monto"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_monto"],
                dt_hora_inicio = reader["dt_hora_inicio"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dt_hora_inicio"],
                dtm_hora_fin = reader["dtm_hora_fin"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_hora_fin"],
                str_tiemporest = "",
                str_tiempo = "",
                int_id_espacio = reader["int_id_espacio"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_id_espacio"],
                str_clave_esp = reader["str_clave"].ToString(),
                str_marcador = reader["str_marcador"].ToString(),
                str_descripcion_zona = reader["str_descripcion"].ToString(),
                str_comentarios = reader["str_comentarios"].ToString(),
                int_timpo_restante = 0,
                Email = reader["email"].ToString(),
                str_nombre_completo = reader["strnombre"].ToString() + " " + reader["strapellidos"].ToString(),
                str_razon_social = reader["str_razon_social"].ToString(),
                str_rfc = reader["str_rfc"].ToString(),
                bit_status = reader["bit_status"] == DBNull.Value ? false : (bool)reader["bit_status"],
                boolean_auto_recarga = reader["boolean_auto_recarga"] == DBNull.Value ? false : (bool)reader["boolean_auto_recarga"],
            };


        }
        [NonAction]
        private MovimientosJoin MapToValueMovimientos2(NpgsqlDataReader reader)
        {
            return new MovimientosJoin()
            {
                id = reader["id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["id"],
                str_status = "",
                str_placa = reader["str_placa"].ToString(),
                int_tiempo = reader["int_tiempo"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_tiempo"],
                flt_monto = reader["flt_monto"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_monto"],
                dt_hora_inicio = reader["dt_hora_inicio"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dt_hora_inicio"],
                dtm_hora_fin = reader["dtm_hora_fin"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_hora_fin"],
                str_tiemporest = "",
                str_tiempo = "",
                int_id_espacio = reader["int_id_espacio"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_id_espacio"],
                str_clave_esp = reader["str_clave"].ToString(),
                str_marcador = reader["str_marcador"].ToString(),
                str_descripcion_zona = reader["str_descripcion"].ToString(),
                str_comentarios = reader["str_comentarios"].ToString(),
                int_timpo_restante = 0,
                Email = reader["email"].ToString(),
                str_nombre_completo = reader["strnombre"].ToString() + " " + reader["strapellidos"].ToString(),
                str_razon_social = reader["str_razon_social"].ToString(),
                str_rfc = reader["str_rfc"].ToString(),
                bit_status = reader["bit_status"] == DBNull.Value ? false : (bool)reader["bit_status"],
                boolean_auto_recarga = reader["boolean_auto_recarga"] == DBNull.Value ? false : (bool)reader["boolean_auto_recarga"],
            };


        }

        [NonAction]
        private Movimientos MapToValueMovimientosMovTodo(NpgsqlDataReader reader)
        {
            return new Movimientos()
            {
                id = reader["id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["id"],
                created_by = reader["created_by"].ToString(),
                created_date = reader["created_date"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["created_date"],
                last_modified_by = reader["last_modified_by"].ToString(),
                last_modified_date = reader["last_modified_date"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["last_modified_date"],
                bit_status = reader["bit_status"] == DBNull.Value ? false : (bool)reader["bit_status"],
                str_placa = reader["str_placa"].ToString(),
                str_latitud = reader["str_latitud"].ToString(),
                str_longitud = reader["str_longitud"].ToString(),
                boolean_auto_recarga = reader["boolean_auto_recarga"] == DBNull.Value ? false : (bool)reader["boolean_auto_recarga"],
                boolean_multa = reader["boolean_multa"] == DBNull.Value ? false : (bool)reader["boolean_multa"],
                dt_hora_inicio = reader["dt_hora_inicio"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dt_hora_inicio"],
                dtm_fecha_insercion_descuento  = reader["dtm_fecha_insercion_descuento"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_fecha_insercion_descuento"],
                dtm_fecha_descuento = reader["dtm_fecha_descuento"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_fecha_descuento"],
                dtm_hora_fin = reader["dtm_hora_fin"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_hora_fin"],
                int_tiempo = reader["int_tiempo"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_tiempo"],
                flt_moneda_saldo_previo_descuento = reader["flt_moneda_saldo_previo_descuento"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["flt_moneda_saldo_previo_descuento"],
                flt_monto = reader["flt_monto"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["flt_monto"],
                flt_saldo_previo_descuento = reader["flt_saldo_previo_descuento"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["flt_saldo_previo_descuento"],
                flt_valor_descuento = reader["flt_valor_descuento"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["flt_valor_descuento"],
                flt_valor_devuelto = reader["flt_valor_devuelto"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["flt_valor_devuelto"],
                flt_valor_final_descuento = reader["flt_valor_final_descuento"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["flt_valor_final_descuento"],
                str_cambio_descuento = reader["str_cambio_descuento"].ToString(),
                str_codigo_autorizacion = reader["str_codigo_autorizacion"].ToString(),
                str_codigo_transaccion = reader["str_codigo_transaccion"].ToString(),
                str_comentarios = reader["str_comentarios"].ToString(),
                str_hash_tarjeta = reader["str_hash_tarjeta"].ToString(),
                str_instalacion = reader["str_instalacion"].ToString(),
                str_instalacion_abrv = reader["str_instalacion_abrv"].ToString(),
                str_moneda_valor_descuento = reader["str_moneda_valor_descuento"].ToString(),
                str_referencia_operacion = reader["str_referencia_operacion"].ToString(),
                str_so = reader["str_so"].ToString(),
                str_tipo = reader["str_tipo"].ToString(),
                str_versionapp = reader["str_versionapp"].ToString(),
                int_id_espacio = reader["int_id_espacio"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_id_espacio"],
                int_id_saldo_id = reader["int_id_saldo_id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_id_saldo_id"],
                int_id_usuario_id = reader["int_id_usuario_id"].ToString(),
                int_id_vehiculo_id = reader["int_id_vehiculo_id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_id_vehiculo_id"],
                intidconcesion_id = reader["intidconcesion_id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["intidconcesion_id"],
                int_id_multa = reader["int_id_multa"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_id_multa"],
                InsDescription = reader["InsDescription"].ToString(),
                InsShortdesc = reader["InsShortdesc"].ToString(),
                BalanceBefore = reader["BalanceBefore"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["BalanceBefore"],
                TicketNumber = reader["TicketNumber"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["TicketNumber"],
                Sector = reader["Sector"].ToString(),
                Tariff = reader["Tariff"].ToString(),
                DiscountAmountCurrencyId = reader["DiscountAmountCurrencyId"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["DiscountAmountCurrencyId"],
                DiscountBalanceCurrencyId = reader["DiscountBalanceCurrencyId"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["DiscountBalanceCurrencyId"],
                DiscountBalanceBefore = reader["DiscountBalanceBefore"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["DiscountBalanceBefore"],
                ServiceChargeTypeId = reader["ServiceChargeTypeId"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["ServiceChargeTypeId"],
                CardReference = reader["CardReference"].ToString(),
                CardScheme = reader["CardScheme"].ToString(),
                MaskedCardNumber = reader["MaskedCardNumber"].ToString(),
                CardExpirationDate  = reader["CardExpirationDate"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["CardExpirationDate"],
                ExternalId1 = reader["ExternalId1"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["ExternalId1"],
                ExternalId2 = reader["ExternalId2"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["ExternalId2"],
                ExternalId3 = reader["ExternalId3"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["ExternalId3"],
                PercVat1 = reader["PercVat1"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["PercVat1"],
                PercVat2 = reader["PercVat2"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["PercVat2"],
                PartialVat1 = reader["PartialVat1"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["PartialVat1"],
                PercFee = reader["PercFee"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["PercFee"],
                PercFeeTopped = reader["PercFeeTopped"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["PercFeeTopped"],
                PartialPercFee = reader["PartialPercFee"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["PartialPercFee"],
                FixedFee = reader["FixedFee"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["FixedFee"],
                PartialFixedFee = reader["PartialFixedFee"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["PartialFixedFee"],
                TotalAmount = reader["TotalAmount"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["TotalAmount"],
                CuspmrPagateliaNewBalance = reader["CuspmrPagateliaNewBalance"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["CuspmrPagateliaNewBalance"],
                CuspmrType = reader["CuspmrType"].ToString(),
                ShopkeeperOp = reader["ShopkeeperOp"] == DBNull.Value ? false : (bool)reader["ShopkeeperOp"],
                ShopkeeperAmount = reader["ShopkeeperAmount"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["ShopkeeperAmount"],
                ShopkeeperProfit = reader["ShopkeeperProfit"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["ShopkeeperProfit"],
                Plate2 = reader["Plate2"].ToString(),
                Plate3 = reader["Plate3"].ToString(),
                Plate4 = reader["Plate4"].ToString(),
                Plate5 = reader["Plate5"].ToString(),
                Plate6 = reader["Plate6"].ToString(),
                Plate7 = reader["Plate7"].ToString(),
                Plate8 = reader["Plate8"].ToString(),
                Plate9 = reader["Plate9"].ToString(),
                Plate10 = reader["Plate10"].ToString(),
                PermitAutoRenew = reader["PermitAutoRenew"] == DBNull.Value ? false : (bool)reader["PermitAutoRenew"],
                PermitExpiration = reader["PermitExpiration"] == DBNull.Value ? false : (bool)reader["PermitExpiration"],
                TransStatus = reader["TransStatus"] == DBNull.Value ? false : (bool)reader["TransStatus"],
                RefundAmount = reader["RefundAmount"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["RefundAmount"],
                valor_sin_bonificar = reader["valor_sin_bonificar"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["valor_sin_bonificar"],
                bonificacion = reader["bonificacion"] == DBNull.Value ? Convert.ToDouble(0) : (Double)reader["bonificacion"],
                tipo_vehiculo = reader["tipo_vehiculo"].ToString()
               
            };  
        }

        [NonAction]
        private MovimientosJoin MapToValueMovimientosse(NpgsqlDataReader reader)
        {
            return new MovimientosJoin()
            {
                id = reader["id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["id"],
                str_status = "",
                str_placa = reader["str_placa"].ToString(),
                int_tiempo = reader["int_tiempo"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_tiempo"],
                flt_monto = reader["flt_monto"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_monto"],
                dt_hora_inicio = reader["dt_hora_inicio"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dt_hora_inicio"],
                dtm_hora_fin = reader["dtm_hora_fin"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_hora_fin"],
                str_tiemporest = "",
                str_tiempo = "",
                str_comentarios = reader["str_comentarios"].ToString(),
                str_latitud = reader["str_latitud"].ToString(),
                str_longitud = reader["str_longitud"].ToString(),
                int_timpo_restante = 0,
                Email = reader["email"].ToString(),
                str_nombre_completo = reader["strnombre"].ToString() + " " + reader["strapellidos"].ToString(),
                str_razon_social = reader["str_razon_social"].ToString(),
                str_rfc = reader["str_rfc"].ToString(),
                bit_status = reader["bit_status"] == DBNull.Value ? false : (bool)reader["bit_status"],
                boolean_auto_recarga = reader["boolean_auto_recarga"] == DBNull.Value ? false : (bool)reader["boolean_auto_recarga"],
            };


        }
        [NonAction]
        private MovimientosJoin MapToValueMovimientos2se(NpgsqlDataReader reader)
        {
            return new MovimientosJoin()
            {
                id = reader["id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["id"],
                str_status = "",
                str_placa = reader["str_placa"].ToString(),
                int_tiempo = reader["int_tiempo"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_tiempo"],
                flt_monto = reader["flt_monto"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_monto"],
                dt_hora_inicio = reader["dt_hora_inicio"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dt_hora_inicio"],
                dtm_hora_fin = reader["dtm_hora_fin"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_hora_fin"],
                str_tiemporest = "",
                str_tiempo = "",
                str_comentarios = reader["str_comentarios"].ToString(),
                int_timpo_restante = 0,
                Email = reader["email"].ToString(),
                str_nombre_completo = reader["strnombre"].ToString() + " " + reader["strapellidos"].ToString(),
                str_razon_social = reader["str_razon_social"].ToString(),
                str_rfc = reader["str_rfc"].ToString(),
                bit_status = reader["bit_status"] == DBNull.Value ? false : (bool)reader["bit_status"],
                boolean_auto_recarga = reader["boolean_auto_recarga"] == DBNull.Value ? false : (bool)reader["boolean_auto_recarga"],
            };


        }

        [HttpGet("mtdConsultarMovMonitoreoXDiaSE")]
        public async Task<ActionResult<ICollection<MovimientosJoin>>> mtdConsultarMovMonitoreoXDiaSE(int intIdConcesion,string dtmFecha, string dtmFechaFin)
        {
            try
            {
                if (dtmFechaFin == null)
                {
                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarmovmonitoreoxdiase('" + dtmFecha + "'" +","+ intIdConcesion+ ")", sql))
                        {
                            var response = new List<MovimientosJoin>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueMovimientosse(drd));
                            }
                            return response;

                        }
                    }

                }
                else
                {

                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarmovmonitoreoxfechasse('" + dtmFecha + "'" + "," + "'" + dtmFechaFin + "'" + ","+ intIdConcesion+")", sql))
                        {
                            var response = new List<MovimientosJoin>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueMovimientos2se(drd));
                            }
                            return response;

                        }


                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        ///// <summary>
        ///// Método Get que obtiene los moviimento de acuerdo a un rango de fechas con entity framwrok
        ///// </summary>
        ///// <param name="intIdConcesion"></param>
        ///// <param name="dtmFechaInicio"></param>
        ///// <param name="dtmFechaFin"></param>  
        ///// <returns></returns>
        //[HttpGet("mtdConsultarMovMonitoreo")]
        //public async Task<ActionResult<List<MovimientosJoin>>> mtdConsultarMovMonitoreo(int intIdConcesion,DateTime dtmFechaInicio, DateTime dtmFechaFin)
        //{
        //    // List<MovimientosJoin> lstMovJ = new List<MovimientosJoin>();
        //    try
        //    {

        //        var response = (from mov in context.tbmovimientos
        //                        join esp in context.tbespacios on mov.int_id_espacio equals esp.id
        //                        join users in context.NetUsers on mov.int_id_usuario_id equals users.Id
        //                        join zona in context.tbzonas on esp.id_zona equals zona.id
        //                        where mov.dt_hora_inicio.Date <= dtmFechaInicio.Date && mov.dt_hora_inicio.Date >= dtmFechaFin.Date && mov.intidconcesion_id == intIdConcesion
        //                        select new MovimientosJoin()
        //                        {
        //                            id = mov.id,
        //                            str_status = "",
        //                            str_placa = mov.str_placa,
        //                            int_tiempo = mov.int_tiempo,
        //                            dt_hora_inicio = mov.dt_hora_inicio,
        //                            dtm_hora_fin = mov.dtm_hora_fin,
        //                            int_timpo_restante = 0,
        //                            str_tiemporest = "",
        //                            str_tiempo = "",
        //                            int_id_espacio = esp.id,
        //                            str_clave_esp = esp.str_clave,
        //                            str_marcador = esp.str_marcador,
        //                            str_descripcion_zona = zona.str_descripcion,
        //                            str_comentarios = mov.str_comentarios,
        //                            Email = users.Email,
        //                            str_nombre_completo = users.strNombre + ' ' + users.strApellidos,
        //                            str_rfc = users.str_rfc,
        //                            str_razon_social = users.str_razon_social,
        //                            bit_status = mov.bit_status,
        //                            boolean_auto_recarga = mov.boolean_auto_recarga,
        //                            detalleMovimientos = context.tbdetallemovimientos.Where(x => x.int_idmovimiento == mov.id).ToList(),
        //                        }).ToList();


        //        //lstMovJ = response;
        //        if (response == null)
        //        {
        //            return NotFound();
        //        }
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { token = ex.Message });
        //    }
        //}

        /// <summary>
        /// Metodo GET que devuelve el historial de movimientos del usuario
        /// </summary>
        /// <param name="strEmail"></param>
        /// <param name="intIdMovimiento"></param>
        /// <param name="intIdConcesion"></param>
        /// <returns></returns>
        [HttpGet("mtdConsultarHistorial")]
        public async Task<ActionResult<MovimientosHistorial>> mtdConsultarHistorial(string strEmail, int intIdMovimiento,int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();

                //String responseString = await client.GetStringAsync(UrlFecha);
                //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
                //string strFecha = fecha.resultado.ToString();

                DateTime time = time1.Value;

                var mov = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);

                var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Email == strEmail);
                var movUsuario = await context.tbmovimientos.Where(x => x.int_id_usuario_id == usuario.Id && x.intidconcesion_id == intIdConcesion).Take(50).ToListAsync();

                //var movUsuario2 = (from x in context.tbmovimientos
                //                   where x.int_id_usuario_id == usuario.Id
                //                   join c in context.tbconcesiones on x.intidconcesion_id equals c.id

                //                   select new {x,c.id,c.str_clave,c.str_razon_social, c.str_nombre_cliente}).Take(50).ToList();

               // int Adia = await context.tbdetallemovimientos.Where(x => x.str_observaciones == "APARCADO" & x.dtm_horaInicio.Date == time.Value.Date & x.int_id_usuario_id == usuario.Id ).CountAsync();

                int Adia2 = await (from dt in context.tbdetallemovimientos
                             join mov1 in context.tbmovimientos on dt.int_idmovimiento equals mov1.id
                             where dt.str_observaciones == "APARCADO" && dt.dtm_horaInicio.Date == time.Date && dt.int_id_usuario_id == usuario.Id && mov1.intidconcesion_id == intIdConcesion
                             select dt).CountAsync();

                //int Ames = await context.tbdetallemovimientos.Where(x => x.str_observaciones == "APARCADO" & x.dtm_horaInicio.Date.Month == time.Date.Month & x.int_id_usuario_id == usuario.Id).CountAsync();
                int Ames2 = await (from dt in context.tbdetallemovimientos
                                   join mov1 in context.tbmovimientos on dt.int_idmovimiento equals mov1.id
                                   where dt.str_observaciones == "APARCADO" & dt.dtm_horaInicio.Date.Month == time.Date.Month && dt.int_id_usuario_id == usuario.Id && mov1.intidconcesion_id == intIdConcesion
                                   select dt).CountAsync();


               // int Ainicio = await context.tbdetallemovimientos.Where(x => x.str_observaciones == "APARCADO" & x.int_id_usuario_id == usuario.Id).CountAsync();

                int Ainicio2 = await (from dt in context.tbdetallemovimientos
                                   join mov1 in context.tbmovimientos on dt.int_idmovimiento equals mov1.id
                                   where dt.str_observaciones == "APARCADO" && dt.int_id_usuario_id == usuario.Id && mov1.intidconcesion_id == intIdConcesion
                                   select dt).CountAsync();

               // int Mdia = await context.tbdetallemovimientos.Where(x => x.str_observaciones.Contains("MULTA") && x.dtm_horaInicio.Date == time.Date && x.int_id_usuario_id == usuario.Id).CountAsync();

                int Mdia2 = await (from dt in context.tbdetallemovimientos
                                      join mov1 in context.tbmovimientos on dt.int_idmovimiento equals mov1.id
                                      where dt.str_observaciones == "MULTA" & dt.dtm_horaInicio.Date == time.Date && dt.int_id_usuario_id == usuario.Id && mov1.intidconcesion_id == intIdConcesion
                                      select dt).CountAsync();


               // int Mmes = await context.tbdetallemovimientos.Where(x => x.str_observaciones.Contains("MULTA") & x.dtm_horaInicio.Date.Month == time.Date.Month & x.int_id_usuario_id == usuario.Id).CountAsync();

                int Mmes2 = await (from dt in context.tbdetallemovimientos
                                   join mov1 in context.tbmovimientos on dt.int_idmovimiento equals mov1.id
                                   where dt.str_observaciones == "MULTA" & dt.dtm_horaInicio.Date.Month == time.Date.Month && dt.int_id_usuario_id == usuario.Id && mov1.intidconcesion_id == intIdConcesion
                                   select dt).CountAsync();

               // int Minicio = await context.tbdetallemovimientos.Where(x => x.str_observaciones.Contains("MULTA") && x.int_id_usuario_id == usuario.Id).CountAsync();

                int Minicio2 = await (from dt in context.tbdetallemovimientos
                                   join mov1 in context.tbmovimientos on dt.int_idmovimiento equals mov1.id
                                   where dt.str_observaciones == "MULTA" && dt.int_id_usuario_id == usuario.Id && mov1.intidconcesion_id == intIdConcesion
                                   select dt).CountAsync();


                var vehiculo = await context.tbvehiculos.FirstOrDefaultAsync(x => x.id == mov.int_id_vehiculo_id);

                // var aparcadosX = await context.tbdetallemovimientos.Where(x => x.str_observaciones == "APARCADO" & x.dtm_horaInicio.Date == DateTime.Now.Date).CountAsync();

                return new MovimientosHistorial()
                {
                    intParkingXdia = Adia2,
                    intParkingXMes = Ames2,
                    intParkingDesdeInicio = Ainicio2,
                    intMultasXDia = Mdia2,
                    intMultasXMes = Mmes2,
                    intMultasDesdeInicio = Minicio2,
                    lstDatosvehiculo = vehiculo,
                    //lstUsuario = usuario,
                    lstMovimientos = movUsuario

                };
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }

        }

        /// <summary>
        /// Método que permite agregar un movimiento
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        [HttpPost("mtdIngresarMovimientos")]
        public async Task<ActionResult<Movimientos>> mtdIngresarMovimientos([FromBody] Movimientos movimientos)
        {
            try
            {

                movimientos.created_date = DateTime.Now;
                movimientos.last_modified_date = DateTime.Now;
                context.tbmovimientos.Add(movimientos);
                await context.SaveChangesAsync();
                return Ok();
            }

            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }

        }


        /// <summary>
        /// Método que permte Aparcar un automóvil
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        [HttpPost("mtdMovAparcar")]
        public async Task<ActionResult<String>> mtdMovAparcar([FromBody] Movimientos movimientos)
        {

            string strResult = "";
            string idMovNuevo = "0";
            double fltSaldoAnterior;
            if (movimientos.int_id_espacio == null)
            {
                strResult = await mtdMovAparcarSE(movimientos);

                return Json(new { idMovimiento = strResult });

            }
            else

            {
                try
                {

                    ParametrosController par = new ParametrosController(context);
                    ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                    //var saldoUsuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimientos.int_id_usuario_id);

                    //DateTime time = DateTime.Now;
                    var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimientos.int_id_usuario_id);
                    fltSaldoAnterior = usuario.dbl_saldo_actual;

                   // int intValidaTiempo = movimientos.int_tiempo;

                    if (usuario.dbl_saldo_actual < movimientos.flt_monto)
                    {
                        strResult = "No tiene saldo suficiente para realizar la operación";
                    }

                    else
                    {

                        var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == movimientos.intidconcesion_id && x.str_tipo == "PARQUIMETRO");
                        var concesion = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == movimientos.intidconcesion_id);
                        Double db_porc_comision = comision.dcm_porcentaje;
                        db_porc_comision = db_porc_comision / 100;

                        Double dbl_comision_cobrada = movimientos.flt_monto * db_porc_comision;
                        Double dbl_total_con_comision = movimientos.flt_monto + dbl_comision_cobrada;

                        movimientos.flt_porcentaje_comision = db_porc_comision;
                        movimientos.flt_monto_porcentaje = dbl_comision_cobrada;
                        movimientos.flt_total_con_comision = dbl_total_con_comision;
                        movimientos.int_tiempo_comprado = movimientos.int_tiempo;
                        movimientos.str_nombre_concesion = concesion.str_nombre_cliente;


                        movimientos.created_date = time.Value;
                        // movimientos.last_modified_date = DateTime.Now;
                        movimientos.last_modified_by = movimientos.created_by;
                        movimientos.dt_hora_inicio = time.Value;
                        DateTime horafin = movimientos.dt_hora_inicio.AddMinutes(movimientos.int_tiempo);
                        movimientos.dtm_fecha_insercion_descuento = time.Value;
                        movimientos.dtm_fecha_descuento = time.Value;
                        movimientos.dtm_hora_fin = horafin;
                        movimientos.flt_saldo_previo_descuento = 0.00;
                        movimientos.flt_valor_descuento = 0.00;
                        movimientos.flt_valor_devuelto = 0.00;
                        movimientos.flt_valor_final_descuento = 0.00;
                        movimientos.str_cambio_descuento = " ";
                        movimientos.str_codigo_autorizacion = " ";
                        movimientos.str_codigo_transaccion = " ";
                        movimientos.str_comentarios = "APARCADO";
                        movimientos.str_hash_tarjeta = " ";
                        movimientos.str_instalacion = " ";
                        movimientos.str_instalacion_abrv = " ";
                        movimientos.str_moneda_valor_descuento = " ";
                        movimientos.str_referencia_operacion = movimientos.dt_hora_inicio.ToString();
                        movimientos.str_tipo = " ";
                        movimientos.str_versionapp = " ";
                        movimientos.bit_status = true;
                       //movimientos.int_tiempo_comprado = movimientos.int_tiempo;

                        //var strategy = context.Database.CreateExecutionStrategy();
                        //await strategy.ExecuteAsync(async () =>
                        //{

                            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                            {
                                try
                                {
                                    int intesp = movimientos.int_id_espacio.Value;
                                    var mov = await context.tbespacios.FirstOrDefaultAsync(x => x.id == intesp);
                                    var zon = await context.tbzonas.FirstOrDefaultAsync(x => x.id == mov.id_zona);

                                   
                                    context.tbmovimientos.Add(movimientos);
                                    await context.SaveChangesAsync();
                                    idMovNuevo = movimientos.id.ToString();

                                    //saldo.flt_monto_final = saldo.flt_monto_final - movimientos.flt_monto;
                                    //saldo.flt_monto_inicial = fltSaldoAnterior;
                                  
                                    //await context.SaveChangesAsync();

                                    usuario.dbl_saldo_anterior = fltSaldoAnterior;
                                    usuario.dbl_saldo_actual = usuario.dbl_saldo_actual - movimientos.flt_monto;
                                    await context.SaveChangesAsync();


                                    context.tbsaldo.Add(new Saldos()
                                    {
                                        created_by = movimientos.created_by,
                                        created_date = time.Value,
                                        dtmfecha = time.Value,
                                        last_modified_date = time.Value,
                                        flt_monto_inicial = fltSaldoAnterior,
                                        flt_monto_final = usuario.dbl_saldo_actual,
                                        str_forma_pago = "VIRTUAL",
                                        str_tipo_recarga = "APARCADO",
                                        int_id_usuario_id = usuario.Id,
                                        int_id_usuario_trans = usuario.Id

                                    });

                                    await context.SaveChangesAsync();

                                    context.tbdetallemovimientos.Add(new DetalleMovimientos()
                                    {
                                        int_idmovimiento = int.Parse(idMovNuevo),
                                        int_idespacio = movimientos.int_id_espacio.Value,
                                        int_id_usuario_id = movimientos.int_id_usuario_id,
                                        int_id_zona = zon.id,
                                        int_duracion = movimientos.int_tiempo,
                                        dtm_horaInicio = movimientos.dt_hora_inicio,
                                        dtm_horaFin = movimientos.dtm_hora_fin,
                                        flt_importe = movimientos.flt_monto,
                                        flt_saldo_anterior = fltSaldoAnterior,
                                        flt_saldo_fin = usuario.dbl_saldo_actual,
                                        str_observaciones = movimientos.str_comentarios,
                                        flt_porcentaje_comision = db_porc_comision,
                                        flt_monto_porcentaje = dbl_comision_cobrada,
                                        flt_total_con_comision = dbl_total_con_comision

                                    }); ;

                                     await context.SaveChangesAsync();

                                    var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == movimientos.int_id_espacio);
                                    if (espacio == null)
                                    {
                                        strResult = "No se encontro espacio";
                                    }

                                    espacio.bit_ocupado = true;
                                    await context.SaveChangesAsync();

                                    transaction.Commit();
                                  
                                }

                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    strResult = ex.Message;
                                    //return Json(new { token = ex.Message });

                                }
                            }
                        //});


                    }
                    if (strResult == "")
                    {
                        await _emailSender.SendEmailAsync(usuario.Email, "Notificación de estacionamiento",
                              "Bienvenido " + usuario.UserName + " su solicitud de estacionamiento con las placas " + movimientos.str_placa + " se ha realizado exitosamente, le recomendamos estar pendiente de las notificaciones que le llegarán a su teléfono cuando se acerque su tiempo de expiración. <br/> Recuerde que puede extender su tiempo de parqueo si asi lo requiere. ");


                        return Json(new { idMovimiento = idMovNuevo });
                    }
                    else
                    {
                        return Json(new { idMovimiento = strResult });
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message + " Id de Movimiento: " + idMovNuevo;
                }
            }

        }


        //[NonAction]
        private async Task<string> mtdMovAparcarSE(Movimientos movimientos)
        {
            string strResult = "";
            string idMovNuevo = "0";
            double fltSaldoAnterior;

            try
            {

                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();
                DateTime time = time1.Value;
                //DateTime time = DateTime.Now; 


                var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimientos.int_id_usuario_id);
                fltSaldoAnterior = usuario.dbl_saldo_actual;
               

               // int intValidaTiempo = movimientos.int_tiempo;

                if (usuario.dbl_saldo_actual < movimientos.flt_monto)
                {
                    strResult = "No tiene saldo suficiente para realizar la operación";
                }

                else
                {
                    var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == movimientos.intidconcesion_id && x.str_tipo == "PARQUIMETRO");
                    var concesion = await context.tbconcesiones.FirstOrDefaultAsync(x => x.id == movimientos.intidconcesion_id);

                    Double db_porc_comision = comision.dcm_porcentaje;
                    db_porc_comision = db_porc_comision / 100;

                    Double dbl_comision_cobrada = movimientos.flt_monto * db_porc_comision;
                    Double dbl_total_con_comision = movimientos.flt_monto + dbl_comision_cobrada;

                    movimientos.flt_porcentaje_comision = db_porc_comision;
                    movimientos.flt_monto_porcentaje = dbl_comision_cobrada;
                    movimientos.flt_total_con_comision = dbl_total_con_comision;
                    movimientos.int_tiempo_comprado = movimientos.int_tiempo;
                    movimientos.str_nombre_concesion = concesion.str_nombre_cliente;

                    movimientos.created_date = time;
                   
                    movimientos.last_modified_by = movimientos.created_by;
                    movimientos.dt_hora_inicio = time;
                    DateTime horafin = movimientos.dt_hora_inicio.AddMinutes(movimientos.int_tiempo);
                    movimientos.dtm_fecha_insercion_descuento = time;
                    movimientos.dtm_fecha_descuento = time;
                    movimientos.dtm_hora_fin = horafin;
                    movimientos.flt_saldo_previo_descuento = 0.00;
                    movimientos.flt_valor_descuento = 0.00;
                    movimientos.flt_valor_devuelto = 0.00;
                    movimientos.flt_valor_final_descuento = 0.00;
                    movimientos.str_cambio_descuento = " ";
                    movimientos.str_codigo_autorizacion = " ";
                    movimientos.str_codigo_transaccion = " ";
                    movimientos.str_comentarios = "APARCADO";
                    movimientos.str_hash_tarjeta = " ";
                    movimientos.str_instalacion = " ";
                    movimientos.str_instalacion_abrv = " ";
                    movimientos.str_moneda_valor_descuento = " ";
                    movimientos.str_referencia_operacion = movimientos.dt_hora_inicio.ToString();
                    movimientos.str_tipo = " ";
                    movimientos.str_versionapp = " ";
                    movimientos.bit_status = true;


                    //var strategy = context.Database.CreateExecutionStrategy();
                    //await strategy.ExecuteAsync(async () =>
                    //{


                    //       ApplicationDbContext db = new ApplicationDbContext(DbContextOptions<ApplicationDbContext> (options =>
                    //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"))));


                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                context.tbmovimientos.Add(movimientos);
                                await context.SaveChangesAsync();
                                idMovNuevo = movimientos.id.ToString();

                              
                                usuario.dbl_saldo_anterior = fltSaldoAnterior;
                                usuario.dbl_saldo_actual = usuario.dbl_saldo_actual - movimientos.flt_monto;
                                await context.SaveChangesAsync();


                                context.tbsaldo.Add(new Saldos()
                                {
                                    created_by = movimientos.created_by,
                                    created_date = time,
                                    dtmfecha = time,
                                    last_modified_date = time,
                                    flt_monto_inicial = fltSaldoAnterior,
                                    flt_monto_final = usuario.dbl_saldo_actual,
                                    str_forma_pago = "VIRTUAL",
                                    str_tipo_recarga = "APARCADO",
                                    int_id_usuario_id = usuario.Id,
                                    int_id_usuario_trans = usuario.Id

                                });

                                context.tbdetallemovimientos.Add(new DetalleMovimientos()
                                {
                                    int_idmovimiento = int.Parse(idMovNuevo),

                                    int_id_usuario_id = movimientos.int_id_usuario_id,
                                    int_duracion = movimientos.int_tiempo,
                                    dtm_horaInicio = movimientos.dt_hora_inicio,
                                    dtm_horaFin = movimientos.dtm_hora_fin,
                                    flt_importe = movimientos.flt_monto,
                                    flt_saldo_anterior = fltSaldoAnterior,
                                    flt_saldo_fin = usuario.dbl_saldo_actual,
                                    str_observaciones = movimientos.str_comentarios,
                                    str_latitud = movimientos.str_latitud,
                                    str_longitud = movimientos.str_longitud,
                                    flt_porcentaje_comision = db_porc_comision,
                                    flt_monto_porcentaje = dbl_comision_cobrada,
                                    flt_total_con_comision = dbl_total_con_comision
                                });

                              await context.SaveChangesAsync();

                               transaction.Commit();
                             
                        }
                        

                        catch (Exception ex)
                            {
                                transaction.Rollback();
                                strResult = ex.Message;

                            }
                        }

                    //});


                }
                if (strResult == "")
                {
                   await mtdEnviarCorreo(usuario.Email,usuario.UserName, movimientos.str_placa,"APARCAR",movimientos.int_tiempo,0,0,false);
                    return idMovNuevo;
                }
                else
                {
                    return strResult;
                }

            }
            catch (Exception ex)
            {

                return ex.Message + " Id de Movimiento: "+ idMovNuevo;
            }

        }

        //[HttpGet("mtdEnviarCorreo")]
        [NonAction]
        public async Task<ActionResult> mtdEnviarCorreo(string strCorreo, string strUserName,string strPlaca,string strOperacion, int intTiempo, double flt_regresar, int intMinutosRegresar, bool bol_dev)
        {
            try
            {
                switch (strOperacion)
                {
                    case "APARCAR":
                        await _emailSender.SendEmailAsync(strCorreo, "Notificación de estacionamiento",
                     "Bienvenido " + strUserName + " su solicitud de estacionamiento con las placas " + strPlaca + " se ha realizado exitosamente, le recomendamos estar pendiente de las notificaciones que le llegarán a su teléfono cuando se acerque su tiempo de expiración. <br/> Recuerde que puede extender su tiempo de parqueo si asi lo requiere. ");
                      break;
                    case "EXTENSION":
                        await _emailSender.SendEmailAsync(strCorreo, "Notificación de extensión de tiempo",
                       "Se realizó exitosamente una extensión de tiempo de " + intTiempo + " minutos a las placas " + strPlaca+ "<br/> si usted no reconoce este movimiento comuniquese con el equipo de soporte.");
                        break;
                    case "DESAPARCADO":
                        if (bol_dev)
                        {
                            await _emailSender.SendEmailAsync(strCorreo, "Notificación de estacionamiento",
                                          strUserName + " su automóvil ha sido desaparcado, le informamos que ha recibido una devolución de $" + flt_regresar + " MXN. correspondiente a " + intMinutosRegresar + " minutos que no han sido utilizados.<br/>Esperamos su estancia haya sido agradable, lo esperamos pronto.<br/> ");

                        }
                        else {

                            await _emailSender.SendEmailAsync(strCorreo, "Notificación de estacionamiento",
                                          strUserName + " su automóvil ha sido desaparcado, esperamos su estancia haya sido agradable, lo esperamos pronto.");
                        }
                       
                        break;
                
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return Json(new { idMovimiento = ex.Message });
            }


        }

        /// <summary>
        /// Método que permite desparcar un automóvil 
        /// </summary>
        /// <param name="intIdMovimiento"></param>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        [HttpPut("mtdMovDesaparcar")]
        public async Task<ActionResult<Movimientos>> mtdMovDesaparcar(int intIdMovimiento, [FromBody] Movimientos movimientos)
        {


            ActionResult<String> actionResult = "";
            string strResult = "";
            //DateTime time = DateTime.Now;

            Double dbleCobrar = 0;
            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);
           // var esp = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);

            if (response.int_id_espacio == null)
            {
                actionResult = await mtdMovDesaparcarSE(intIdMovimiento, movimientos);
                return Json(new { idMovimiento = strResult });
            }

            Double dbleRegresar = 0;
            int intPlanMinutos = 0;
            int intMinutosRegresar = 0;
            int intDatoRegresar = 0;
            int intPlanMinutosNext = 0;
            int intTimepoDeParking = 0;
            Double db_porc_comision = 0;
            Double dbl_monto_comision_regresar = 0;
            Double dbl_total_con_comision = 0;

            // Double dbl_monto_comision_regresar = 0;
            Double dbl_cuanto_cobre = 0;
            Double dbl_monto_ant = 0;
            Double dbl_porc_comision_ant = 0;
            Double dbl_monto_porcentaje_ant = 0;
            Double dbl_total_con_comision_ant = 0;
            Double dbl_saldo_anterior_insertar = 0;

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

            var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);

            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                    var esp = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);
                    var zon = await context.tbzonas.FirstOrDefaultAsync(x => x.id == esp.id_zona);
                        //var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);
                        //var parametros = await context.tbparametros.FirstOrDefaultAsync(x => x.intidconcesion_id == response.intidconcesion_id);
                       
                        dbl_monto_ant = response.flt_monto;
                        dbl_porc_comision_ant = response.flt_porcentaje_comision;
                        dbl_monto_porcentaje_ant = response.flt_monto_porcentaje;
                        dbl_total_con_comision_ant = response.flt_total_con_comision;
                        //var saldoAnterior = await context.tbsaldo.FirstOrDefaultAsync(x => x.int_id_usuario_id == response.int_id_usuario_id);

                      
                        
                        dbl_saldo_anterior_insertar = usuario.dbl_saldo_actual;

                        double dblSaldoA = usuario.dbl_saldo_actual;
                       // int intParametroMinParking = parametros.int_minimo_estacionamiento;
                        int intTiempoRenta = response.int_tiempo;
                        TimeSpan tmsTiempoTranscurrido = response.dt_hora_inicio - time.Value;
                        int horas = tmsTiempoTranscurrido.Hours;
                        int minutos = tmsTiempoTranscurrido.Minutes;
                        int HorasT = horas * 60;
                        int intTotalMinutos = HorasT + minutos;
                        string d = intTotalMinutos.ToString();
                        int intTiempoTranscurridovalor = int.Parse(d.Trim('-'));


                        DataTable dtPlan = await mtdObtenerPlantarifario(response.intidconcesion_id);

                        int intTarifaMin = int.Parse(dtPlan.Rows[0]["Minutos"].ToString());
                        Double dblTarifaMin = Double.Parse(dtPlan.Rows[0]["Tarifa"].ToString());


                        if (intTiempoTranscurridovalor < intTiempoRenta)
                        {
                            intDatoRegresar = intTiempoRenta - intTiempoTranscurridovalor;

                            for (int i = 0; i < dtPlan.Rows.Count; i++)
                            {

                                intPlanMinutos = int.Parse(dtPlan.Rows[i]["Minutos"].ToString());
                                int intNext = int.Parse(dtPlan.Rows[i + 1]["Minutos"].ToString());

                                if (intTiempoTranscurridovalor == intPlanMinutos || intTiempoTranscurridovalor < intTarifaMin)
                                {
                                    if (intTiempoRenta > intTiempoTranscurridovalor)
                                    {
                                        dbleCobrar = int.Parse(dtPlan.Rows[0]["Tarifa"].ToString());
                                        //intPlanMinutosNext = int.Parse(dtPlan.Rows[i + 1]["Minutos"].ToString());
                                        intTimepoDeParking = intPlanMinutos;
                                        intMinutosRegresar = intTiempoRenta - intTarifaMin;
                                        dbleRegresar = response.flt_monto - dblTarifaMin;
                                       
                                       // await context.SaveChangesAsync();
                                        if (dbleRegresar == 0)
                                        {
                                            bolDevolucion = false;
                                        }
                                        else
                                        {
                                            var saldo = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);
                                            var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == movimientos.intidconcesion_id && x.str_tipo == "PARQUIMETRO");
                                            db_porc_comision = comision.dcm_porcentaje;
                                            db_porc_comision = db_porc_comision / 100;
                                            dbl_monto_comision_regresar = dbleRegresar * db_porc_comision;
                                            dbl_total_con_comision = dbleRegresar + dbl_monto_comision_regresar;

                                            //Aqui para saber cuanto cobré
                                            double dbl_comision_de_cobro_final = dbleCobrar * db_porc_comision;
                                            dbl_cuanto_cobre = dbleCobrar + dbl_comision_de_cobro_final;


                                            saldo.dbl_saldo_actual = saldo.dbl_saldo_actual + dbl_total_con_comision;
                                            saldo.dbl_saldo_anterior = dblSaldoA;
                                            await context.SaveChangesAsync();

                                            bolDevolucion = true;
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        intTimepoDeParking = intPlanMinutos;
                                        break;
                                    }

                                }
                                else
                                {
                                    if (intTiempoTranscurridovalor < intNext || intTiempoTranscurridovalor == intNext)
                                    {
                                        dbleCobrar = int.Parse(dtPlan.Rows[i + 1]["Tarifa"].ToString());
                                        intPlanMinutosNext = int.Parse(dtPlan.Rows[i + 1]["Minutos"].ToString());
                                        intTimepoDeParking = intPlanMinutosNext;

                                        intMinutosRegresar = intTiempoRenta - intNext;
                                        dbleRegresar = response.flt_monto - dbleCobrar;

                                        var saldo = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);
                                        var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == movimientos.intidconcesion_id && x.str_tipo == "PARQUIMETRO");
                                        db_porc_comision = comision.dcm_porcentaje;
                                        db_porc_comision = db_porc_comision / 100;
                                        dbl_monto_comision_regresar = dbleRegresar * db_porc_comision;
                                        dbl_total_con_comision = dbleRegresar + dbl_monto_comision_regresar;

                                        saldo.dbl_saldo_actual = saldo.dbl_saldo_actual + dbl_total_con_comision;
                                        saldo.dbl_saldo_anterior = dblSaldoA;
                                        await context.SaveChangesAsync();

                                        bolDevolucion = true;
                                        break;
                                    }

                                }

                            }
                        }

                        var saldos = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);
                        var fltMontoNeg = -System.Math.Abs(dbleRegresar);
                        var intTimepoRegresar = -System.Math.Abs(intMinutosRegresar);
                        if (bolDevolucion)
                        {
                            context.tbdetallemovimientos.Add(new DetalleMovimientos()
                            {
                                int_idmovimiento = intIdMovimiento,
                                int_idespacio = response.int_id_espacio.Value,
                                int_id_usuario_id = response.int_id_usuario_id,
                                int_id_zona = zon.id,
                                int_duracion = intTimepoRegresar,
                                dtm_horaInicio = time.Value,
                                dtm_horaFin = time.Value,
                                flt_descuentos = fltMontoNeg,
                                flt_porcentaje_comision = db_porc_comision,
                                flt_monto_porcentaje = dbl_monto_comision_regresar, 
                                flt_total_con_comision = dbl_total_con_comision,
                                flt_saldo_anterior = dblSaldoA,
                                flt_saldo_fin = saldos.dbl_saldo_actual,
                                str_observaciones = "DEVOLUCIÓN"

                            });
                            await context.SaveChangesAsync();
                         

                            context.tbsaldo.Add(new Saldos()
                            {
                                created_by = saldos.created_by,
                                created_date = time.Value,
                                dtmfecha = time.Value,
                                last_modified_date = time.Value,
                                flt_monto_inicial = dblSaldoA,
                                flt_monto_final = saldos.dbl_saldo_actual,
                                str_forma_pago = "VIRTUAL",
                                str_tipo_recarga = "DEVOLUCION",
                                int_id_usuario_id = usuario.Id,
                                int_id_usuario_trans = usuario.Id

                            });

                            await context.SaveChangesAsync();
                            response.flt_monto = dbleRegresar;
                        }

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = intIdMovimiento,
                            int_idespacio = response.int_id_espacio.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_id_zona = zon.id,
                            int_duracion = intPlanMinutos,
                            dtm_horaInicio = time.Value,
                            dtm_horaFin = time.Value,
                            flt_importe = 0.0,
                            flt_saldo_anterior = dblSaldoA,
                            flt_saldo_fin = saldos.dbl_saldo_actual,
                            str_observaciones = "DESPARCADO"

                        });

                        context.SaveChanges();
                        var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);
                        if (espacio == null)
                        {
                            strResult = "No se encontro espacio";
                        }

                        espacio.bit_ocupado = false;
                        await context.SaveChangesAsync();

                        response.last_modified_date = time.Value;
                        response.last_modified_by = movimientos.last_modified_by;
                        response.str_comentarios = "DESAPARCADO";
                        response.bit_status = false;
                        response.flt_monto = dbl_monto_ant; 
                        response.flt_monto_porcentaje = dbl_monto_porcentaje_ant;
                        response.flt_total_con_comision = dbl_total_con_comision_ant;
                        response.dtm_hora_fin = response.dt_hora_inicio.AddMinutes(intTimepoDeParking);
                        response.int_tiempo = intTimepoDeParking;

                        response.int_tiempo_devuelto = intMinutosRegresar;
                        response.flt_monto_devolucion = dbleRegresar;
                        response.flt_monto_porc_devolucion = dbl_monto_comision_regresar;
                        response.flt_total_dev_con_comision = dbl_total_con_comision;
                        response.flt_monto_real = dbl_cuanto_cobre;


                        await context.SaveChangesAsync();
                       
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            //});



            if (strResult == "")
            {

                if (bolDevolucion)
                {
                    await _emailSender.SendEmailAsync(usuario.Email, "Notificación de estacionamiento",
                                           usuario.UserName + " su automóvil ha sido desaparcado, le informamos que ha recibido una devolución de $" + dbleRegresar + " MXN. correspondiente a " + intMinutosRegresar + " minutos que no han sido utilizados.<br/>Esperamos su estancia haya sido agradable, lo esperamos pronto.<br/> ");

                }
                else
                {
                    await _emailSender.SendEmailAsync(usuario.Email, "Notificación de estacionamiento",
                                         usuario.UserName + " su automóvil ha sido desaparcado, esperamos su estancia haya sido agradable, lo esperamos pronto.");


                }

                bolDevolucion = false;
                String cadena1 = null;
                String cadena2 = null;
                Console.Write(cadena1 == cadena2);
                return Ok();

            }
            else
            {
                return Json(new { idMovimiento = strResult });

            }

        }

        [NonAction]
        public async Task<ActionResult<String>> mtdMovDesaparcarSE(int intIdMovimiento, [FromBody] Movimientos movimientos)
        {

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();
            DateTime time = time1.Value;
            //DateTime time = DateTime.Now;

            string strResult = "";
            Double dbleRegresar = 0;
            int intPlanMinutos = 0;
            int intMinutosRegresar = 0;
            int intDatoRegresar = 0;
            int intPlanMinutosNext = 0;
            int intTimepoDeParking = 0;
            Double db_porc_comision = 0;
            Double dbl_total_con_comision = 0;
            Double dbl_monto_comision_regresar = 0;
            Double dbl_cuanto_cobre = 0;
            Double dbl_monto_ant = 0;
            Double dbl_porc_comision_ant = 0;
            Double dbl_monto_porcentaje_ant = 0;
            Double dbl_total_con_comision_ant = 0;
            Double dbl_saldo_anterior_insertar = 0;

            Double dbleCobrar = 0;
            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);
            var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);

            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        
                        dbl_monto_ant = response.flt_monto;
                        dbl_porc_comision_ant = response.flt_porcentaje_comision;
                        dbl_monto_porcentaje_ant = response.flt_monto_porcentaje;
                        dbl_total_con_comision_ant = response.flt_total_con_comision;
                       
                        dbl_saldo_anterior_insertar = usuario.dbl_saldo_actual;

                        double dblSaldoA = usuario.dbl_saldo_actual;
                       
                        int intTiempoRenta = response.int_tiempo;
                        TimeSpan tmsTiempoTranscurrido = response.dt_hora_inicio - time;
                        int horas = tmsTiempoTranscurrido.Hours;
                        int minutos = tmsTiempoTranscurrido.Minutes;
                        int HorasT = horas * 60;
                        int intTotalMinutos = HorasT + minutos;
                        string d = intTotalMinutos.ToString();
                        int intTiempoTranscurridovalor = int.Parse(d.Trim('-'));


                        DataTable dtPlan = await mtdObtenerPlantarifario(response.intidconcesion_id);

                        int intTarifaMin = int.Parse(dtPlan.Rows[0]["Minutos"].ToString());
                        Double dblTarifaMin = Double.Parse(dtPlan.Rows[0]["Tarifa"].ToString());


                        if (intTiempoTranscurridovalor < intTiempoRenta)
                        {
                            intDatoRegresar = intTiempoRenta - intTiempoTranscurridovalor;

                            for (int i = 0; i < dtPlan.Rows.Count; i++)
                            {

                                intPlanMinutos = int.Parse(dtPlan.Rows[i]["Minutos"].ToString());
                                int intNext = int.Parse(dtPlan.Rows[i + 1]["Minutos"].ToString());

                                if (intTiempoTranscurridovalor == intPlanMinutos || intTiempoTranscurridovalor < intTarifaMin)
                                {
                                    if (intTiempoRenta > intTiempoTranscurridovalor)
                                    {
                                        dbleCobrar = int.Parse(dtPlan.Rows[0]["Tarifa"].ToString());
                                        //intPlanMinutosNext = int.Parse(dtPlan.Rows[i + 1]["Minutos"].ToString());
                                        intTimepoDeParking = intPlanMinutos;
                                        intMinutosRegresar = intTiempoRenta - intTarifaMin;
                                        dbleRegresar = response.flt_monto - dblTarifaMin;
                                      
                                        if (dbleRegresar == 0)
                                        {
                                            bolDevolucion = false;
                                        }
                                        else
                                        {
                                            var saldo = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);
                                            var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == response.intidconcesion_id && x.str_tipo == "PARQUIMETRO");
                                            db_porc_comision = comision.dcm_porcentaje;
                                            db_porc_comision = db_porc_comision / 100;
                                            dbl_monto_comision_regresar = dbleRegresar * db_porc_comision;
                                            dbl_total_con_comision = dbleRegresar + dbl_monto_comision_regresar;

                                            //Aqui para saber cuanto cobré
                                            double dbl_comision_de_cobro_final = dbleCobrar * db_porc_comision;
                                            dbl_cuanto_cobre = dbleCobrar + dbl_comision_de_cobro_final;


                                            saldo.dbl_saldo_actual = saldo.dbl_saldo_actual + dbl_total_con_comision;
                                            saldo.dbl_saldo_anterior = dblSaldoA;
                                            await context.SaveChangesAsync();
                                            bolDevolucion = true;
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        intTimepoDeParking = intPlanMinutos;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (intTiempoTranscurridovalor < intNext || intTiempoTranscurridovalor == intNext)
                                    {
                                        dbleCobrar = int.Parse(dtPlan.Rows[i + 1]["Tarifa"].ToString());
                                        intPlanMinutosNext = int.Parse(dtPlan.Rows[i + 1]["Minutos"].ToString());
                                        intTimepoDeParking = intPlanMinutosNext;

                                        intMinutosRegresar = intTiempoRenta - intNext;

                                        dbleRegresar = response.flt_monto - dbleCobrar;


                                        var saldo = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);
                                        var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == response.intidconcesion_id && x.str_tipo == "PARQUIMETRO");
                                        db_porc_comision = comision.dcm_porcentaje;
                                        db_porc_comision = db_porc_comision / 100;
                                        dbl_monto_comision_regresar = dbleRegresar * db_porc_comision;
                                        dbl_total_con_comision = dbleRegresar + dbl_monto_comision_regresar;


                                        //Aqui para saber cuanto cobré
                                        double dbl_comision_de_cobro_final = dbleCobrar * db_porc_comision;
                                        dbl_cuanto_cobre = dbleCobrar + dbl_comision_de_cobro_final;

                                        saldo.dbl_saldo_actual = saldo.dbl_saldo_actual + dbl_total_con_comision;
                                        saldo.dbl_saldo_anterior = dblSaldoA;
                                        await context.SaveChangesAsync();
                                        bolDevolucion = true;

                                        break;
                                    }
                                }
                            }
                        }

                        var saldos = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == response.int_id_usuario_id);
                        var fltMontoNeg = -System.Math.Abs(dbleRegresar);
                        var intTimepoRegresar = -System.Math.Abs(intMinutosRegresar);
                        if (bolDevolucion)
                        {
                            context.tbdetallemovimientos.Add(new DetalleMovimientos()
                            {
                                int_idmovimiento = intIdMovimiento,
                                int_id_usuario_id = response.int_id_usuario_id,
                                int_duracion = intTimepoRegresar,
                                dtm_horaInicio = time,
                                dtm_horaFin = time,
                                flt_descuentos = fltMontoNeg,
                                flt_porcentaje_comision = db_porc_comision,
                                flt_monto_porcentaje = dbl_monto_comision_regresar,
                                flt_total_con_comision = dbl_total_con_comision,
                                flt_saldo_anterior = dblSaldoA,
                                flt_saldo_fin = saldos.dbl_saldo_actual,
                                str_observaciones = "DEVOLUCIÓN",
                                str_latitud = response.str_latitud,
                                str_longitud = response.str_longitud
                            });
                            await context.SaveChangesAsync();
                            
                            context.tbsaldo.Add(new Saldos()
                            {
                                created_by = saldos.created_by,
                                created_date = time,
                                dtmfecha = time,
                                last_modified_date = time,
                                flt_monto_inicial = dblSaldoA,
                                flt_monto_final = saldos.dbl_saldo_actual,
                                str_forma_pago = "VIRTUAL",
                                str_tipo_recarga = "DESAPARCADO",
                                int_id_usuario_id = usuario.Id,
                                int_id_usuario_trans = usuario.Id

                            });

                            await context.SaveChangesAsync();
                            response.flt_monto = dbleRegresar;

                        }
                        else {
                            dbl_cuanto_cobre = dbl_total_con_comision_ant;
                        }

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = intIdMovimiento,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_duracion = intPlanMinutos,
                            dtm_horaInicio = time,
                            dtm_horaFin = time,
                            flt_importe = 0.0,
                            flt_saldo_anterior = dblSaldoA,
                            flt_saldo_fin = saldos.dbl_saldo_actual,
                            str_observaciones = "DESPARCADO",
                            str_latitud = response.str_latitud,
                            str_longitud = response.str_longitud

                        });

                        await context.SaveChangesAsync();

                        response.flt_saldo_anterior = dbl_saldo_anterior_insertar;
                        response.last_modified_date = time;
                        response.last_modified_by = movimientos.last_modified_by;
                        response.str_comentarios = "DESAPARCADO";
                        response.bit_status = false;
                        response.flt_monto = dbl_monto_ant;
                        response.flt_monto_porcentaje = dbl_monto_porcentaje_ant;
                        response.flt_total_con_comision = dbl_total_con_comision_ant;
                        response.dtm_hora_fin = response.dt_hora_inicio.AddMinutes(intTimepoDeParking);
                        response.int_tiempo = intTimepoDeParking;
                        response.int_tiempo_devuelto = intMinutosRegresar;
                        response.flt_monto_devolucion = dbleRegresar;
                        response.flt_monto_porc_devolucion = dbl_monto_comision_regresar;
                        response.flt_total_dev_con_comision = dbl_total_con_comision;
                        response.flt_monto_real = dbl_cuanto_cobre;

                        await context.SaveChangesAsync();
                       
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;

                    }
                }
            //});

            if (strResult == "")
            {
               
                if (bolDevolucion)
                {
                    //await _emailSender.SendEmailAsync(usuario.Email, "Notificación de estacionamiento",
                    //                       usuario.UserName + " su automóvil ha sido desaparcado, le informamos que ha recibido una devolución de $" + dbleRegresar + " MXN. correspondiente a " + intMinutosRegresar + " minutos que no han sido utilizados.<br/>Esperamos su estancia haya sido agradable, lo esperamos pronto.<br/> ");
                    await mtdEnviarCorreo(usuario.Email, usuario.UserName, response.str_placa, "DESAPARCADO", movimientos.int_tiempo, dbleRegresar, intMinutosRegresar,bolDevolucion);
                }
                else
                {

                    await mtdEnviarCorreo(usuario.Email, usuario.UserName, movimientos.str_placa, "DESAPARCADO", movimientos.int_tiempo, 0, 0, bolDevolucion);
                    //await _emailSender.SendEmailAsync(usuario.Email, "Notificación de estacionamiento",
                    //                     usuario.UserName + " su automóvil ha sido desaparcado, esperamos su estancia haya sido agradable, lo esperamos pronto.");

                }

                bolDevolucion = false;

                return Ok();
            }
            else
            {
                return Json(new { idMovimiento = strResult });
            }

        }

        [HttpGet("mtdObtenerPlanTarifario")]
        public async Task<DataTable> mtdObtenerPlantarifario(int intIdConcesion)
        {
            var tarifa = await context.tbtarifas.FirstOrDefaultAsync(x => x.intidconcesion_id == intIdConcesion);

            DataTable table = new DataTable("Plan tarifario");
            DataColumn column;
            DataRow row;

            // Create first column and add to the DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Minutos";
            column.ReadOnly = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "Tarifa";
            column.ReadOnly = false;
            table.Columns.Add(column);

            int intMinimoEst = tarifa.int_tiempo_minimo;
            int intMaxEst = tarifa.int_tiempo_maximo;
            int intIntervaloEst = tarifa.int_intervalo_minutos;


            row = table.NewRow();
            row["Minutos"] = tarifa.int_tiempo_minimo;
            row["Tarifa"] = tarifa.flt_tarifa_min;
            table.Rows.Add(row);


            for (int e = intMinimoEst; e < intMaxEst; e += intIntervaloEst)
            {
                DataRow lastRow = table.Rows[table.Rows.Count - 1];

                int intMinutos = int.Parse(lastRow["Minutos"].ToString());
                int intTarifa = int.Parse(lastRow["Tarifa"].ToString());

                row = table.NewRow();
                row["Minutos"] = intMinutos + intIntervaloEst;
                row["Tarifa"] = intTarifa + tarifa.flt_tarifa_intervalo;
                table.Rows.Add(row);

            }


            return table;

        }

        /// <summary>
        /// Metodo PUT que permite cancelar el aparcamiento.
        /// </summary>
        /// <param name="intIdMovimiento"></param>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        /// 

        [HttpGet("mtdMovDesocupar")]
        public async Task<ActionResult<Movimientos>> mtdMovDesocupar(int intIdMovimiento, [FromBody] Movimientos movimientos)
        {

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();

            //DateTime time = DateTime.Now;

            string strResult = "";

            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var mov = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);
                        var esp = await context.tbespacios.FirstOrDefaultAsync(x => x.id == mov.int_id_espacio);
                        var zon = await context.tbzonas.FirstOrDefaultAsync(x => x.id == esp.id_zona);

                        var saldo = await context.tbsaldo.FirstOrDefaultAsync(x => x.int_id_usuario_id == mov.int_id_usuario_id);

                        //if (response == null)
                        //{
                        //    return NotFound();
                        //}

                        mov.last_modified_date = time.Value;
                        mov.last_modified_by = movimientos.last_modified_by;
                        mov.str_comentarios = "DESOCUPADO";
                        mov.bit_status = false;


                        await context.SaveChangesAsync();

                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = intIdMovimiento,
                            int_idespacio = mov.int_id_espacio.Value,
                            int_id_usuario_id = mov.int_id_usuario_id,
                            int_id_zona = zon.id,
                            int_duracion = mov.int_tiempo,
                            dtm_horaInicio = time.Value,
                            dtm_horaFin = time.Value,
                            flt_importe = 0.0,
                            flt_saldo_anterior = saldo.flt_monto_final,
                            flt_saldo_fin = saldo.flt_monto_final,
                            str_observaciones = "DESOCUPADO"

                        });

                        context.SaveChanges();
                        var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == movimientos.int_id_espacio);
                        if (espacio == null)
                        {
                            strResult = "No se encontro espacio";
                        }

                        espacio.bit_ocupado = false;
                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            //});

            if (strResult == "")
            {
                return Ok();

            }
            else
            {
                return Json(new { idMovimiento = strResult });

            }

        }
        [HttpPut("mtdMovCancelar")]
        public async Task<ActionResult<Movimientos>> mtdMovCancelar(int intIdMovimiento, [FromBody] Movimientos movimientos)
        {

            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

            //String responseString = await client.GetStringAsync(UrlFecha);
            //dynamic fecha = JsonConvert.DeserializeObject<dynamic>(responseString);
            //string strFecha = fecha.resultado.ToString();

            DateTime time = horadeTransaccion.Value;


            string strResult = "";
            Double dbleRegresar = 0;
            int intPlanMinutos = 0;
            int intMinutosRegresar = 0;
            int intDato = 0;

            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);
                        var mov = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);
                        var zon = await context.tbzonas.FirstOrDefaultAsync(x => x.id == mov.id_zona);

                        var parametros = await context.tbparametros.FirstOrDefaultAsync(x => x.intidconcesion_id == response.intidconcesion_id);
                        var saldoAnterior = await context.tbsaldo.FirstOrDefaultAsync(x => x.int_id_usuario_id == response.int_id_usuario_id);
                        double dblSaldoA = saldoAnterior.flt_monto_final;



                        int intTiempoRenta = response.int_tiempo;
                        TimeSpan tmsTiempoTranscurrido = response.dt_hora_inicio - time;

                        string d = tmsTiempoTranscurrido.Minutes.ToString();
                        int valor = int.Parse(d.Trim('-'));


                        DataTable dtPlan = await mtdObtenerPlantarifario(response.intidconcesion_id);
                        if (valor < intTiempoRenta)
                        {
                            intDato = intTiempoRenta - valor;

                            for (int i = 0; i < dtPlan.Rows.Count; i++)
                            {
                                intPlanMinutos = int.Parse(dtPlan.Rows[i]["Minutos"].ToString());
                                int intNext = int.Parse(dtPlan.Rows[i + 1]["Minutos"].ToString());
                                if (intTiempoRenta >= intPlanMinutos)
                                {
                                    if (valor == intNext)
                                    {

                                        if (valor < intNext)

                                        {
                                            Double dbleCobrar = int.Parse(dtPlan.Rows[i]["Tarifa"].ToString());

                                            intMinutosRegresar = intTiempoRenta - intPlanMinutos;
                                            dbleRegresar = response.flt_monto - dbleCobrar;
                                            var saldo = await context.tbsaldo.FirstOrDefaultAsync(x => x.int_id_usuario_id == response.int_id_usuario_id);
                                            saldo.flt_monto_final = saldo.flt_monto_final + dbleRegresar;
                                            saldo.flt_monto_inicial = dblSaldoA;
                                            await context.SaveChangesAsync();
                                            bolDevolucion = true;
                                            break;
                                            //}
                                        }
                                    }
                                    break;
                                }

                            }

                        }

                        var saldos = await context.tbsaldo.FirstOrDefaultAsync(x => x.int_id_usuario_id == response.int_id_usuario_id);
                        var fltMontoNeg = -System.Math.Abs(dbleRegresar);
                        var intTimepoRegresar = -System.Math.Abs(intPlanMinutos);


                        if (bolDevolucion)
                        {
                            context.tbdetallemovimientos.Add(new DetalleMovimientos()
                            {
                                int_idmovimiento = intIdMovimiento,
                                int_idespacio = response.int_id_espacio.Value,
                                int_id_usuario_id = response.int_id_usuario_id,
                                int_id_zona = zon.id,
                                int_duracion = intTimepoRegresar,
                                dtm_horaInicio = time,
                                dtm_horaFin = time,
                                flt_importe = fltMontoNeg,
                                flt_saldo_anterior = dblSaldoA,
                                flt_saldo_fin = saldos.flt_monto_final,
                                str_observaciones = "DEVOLUCIÓN"

                            });
                            context.SaveChanges();
                        }


                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = intIdMovimiento,
                            int_idespacio = response.int_id_espacio.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_id_zona = zon.id,
                            int_duracion = response.int_tiempo,
                            dtm_horaInicio = time,
                            dtm_horaFin = time,
                            flt_importe = response.flt_monto,
                            flt_saldo_anterior = dblSaldoA,
                            flt_saldo_fin = saldos.flt_monto_final,
                            str_observaciones = "CANCELAR"

                        });

                        context.SaveChanges();

                        var espacio = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);
                        if (espacio == null)
                        {
                            strResult = "No se encontro espacio";
                        }

                        espacio.bit_ocupado = false;
                        await context.SaveChangesAsync();

                        response.last_modified_date = time;
                        response.last_modified_by = movimientos.last_modified_by;
                        response.str_comentarios = "CANCELAR";
                        response.bit_status = false;
                        if (bolDevolucion)
                        {
                            response.flt_monto = fltMontoNeg;
                            response.int_tiempo = intTimepoRegresar;
                        }


                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strResult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            //});



            if (strResult == "")
            {
                bolDevolucion = false;
                return Ok();

            }
            else
            {
                return Json(new { idMovimiento = strResult });

            }

        }

        /// <summary>
        /// Método Put que permite hacer una extensión de tiempo
        /// </summary>
        /// <param name="intIdMovimiento"></param>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        [HttpPut("mtdMovExtenderTiempo")]
        public async Task<ActionResult<Movimientos>> mtdMovAgregarTiempo(int intIdMovimiento, [FromBody] Movimientos movimientos)
        {
            double fltSaldoAnterior;
            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> time1 = par.mtdObtenerFechaMexico();
            DateTime time = time1.Value;
           // DateTime time = DateTime.Now;

            string strresult = " ";

            var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);
            var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimientos.int_id_usuario_id);
            fltSaldoAnterior = usuario.dbl_saldo_actual;
            DateTime horaFinalAnterior = response.dtm_hora_fin;
           
            if ( response.int_id_espacio == null)
            {
                ActionResult actionResult = await mtdMovAgregarTiempoSE(intIdMovimiento, movimientos);
                return actionResult;
            }

            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                     var esp = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);
                    var zon = await context.tbzonas.FirstOrDefaultAsync(x => x.id == esp.id_zona);
                        //int intValidaTiempo = movimientos.int_tiempo;

                        if (usuario.dbl_saldo_actual < movimientos.flt_monto)
                        {
                            strresult = "No tiene saldo suficiente para realizar la operación";

                        }
                        else
                        {
                            var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == movimientos.intidconcesion_id && x.str_tipo == "PARQUIMETRO");

                          
                            Double db_porc_comision = comision.dcm_porcentaje;
                            db_porc_comision = db_porc_comision / 100;

                            Double dbl_comision_cobrada = movimientos.flt_monto * db_porc_comision;
                            Double dbl_total_con_comision = movimientos.flt_monto + dbl_comision_cobrada;

                            usuario.dbl_saldo_actual = usuario.dbl_saldo_actual + dbl_total_con_comision;
                            usuario.dbl_saldo_anterior = fltSaldoAnterior;
                            await context.SaveChangesAsync();

                            response.flt_porcentaje_comision = db_porc_comision;
                            response.flt_monto_porcentaje = response.flt_monto_porcentaje +  dbl_comision_cobrada;
                            response.flt_total_con_comision = response.flt_total_con_comision + dbl_total_con_comision;

                            response.last_modified_date = time;
                            response.last_modified_by = movimientos.last_modified_by;
                            DateTime horafin = response.dtm_hora_fin.AddMinutes(movimientos.int_tiempo);
                            response.int_tiempo = response.int_tiempo + movimientos.int_tiempo;
                            response.int_tiempo_comprado = response.int_tiempo_comprado + movimientos.int_tiempo;

                            response.dtm_hora_fin = horafin;
                            response.flt_monto = response.flt_monto + movimientos.flt_monto;
                            response.int_id_espacio = response.int_id_espacio;
                            response.str_comentarios = "EXTENSIÓN DE TIEMPO " + movimientos.int_tiempo;
                            response.int_id_usuario_id = movimientos.int_id_usuario_id;
                            await context.SaveChangesAsync();

                          
                            context.tbsaldo.Add(new Saldos()
                            {
                                created_by = movimientos.last_modified_by,
                                created_date = time,
                                dtmfecha = time,
                                last_modified_date = time,
                                flt_monto_inicial = fltSaldoAnterior,
                                flt_monto_final = usuario.dbl_saldo_actual,
                                str_forma_pago = "VIRTUAL",
                                str_tipo_recarga = "EXTENSION",
                                int_id_usuario_id = usuario.Id,
                                int_id_usuario_trans = usuario.Id

                            });

                            var saldoV = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimientos.int_id_usuario_id);

                            context.tbdetallemovimientos.Add(new DetalleMovimientos()
                            {
                                int_idmovimiento = intIdMovimiento,
                                int_idespacio = response.int_id_espacio.Value,
                                int_id_usuario_id = response.int_id_usuario_id,
                                int_id_zona = zon.id,
                                int_duracion = movimientos.int_tiempo,
                                dtm_horaInicio = horaFinalAnterior,
                                dtm_horaFin = horafin,
                                flt_importe = movimientos.flt_monto,
                                flt_porcentaje_comision = db_porc_comision,
                                flt_monto_porcentaje = dbl_comision_cobrada,
                                flt_total_con_comision = dbl_total_con_comision,
                                flt_saldo_anterior = fltSaldoAnterior,
                                flt_saldo_fin = saldoV.dbl_saldo_actual,
                                str_observaciones = "EXTENSIÓN DE TIEMPO " + movimientos.int_tiempo

                            });

                            context.SaveChanges();


                            transaction.Commit();
                        }

                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strresult = ex.Message;
                        //return Json(new { token = ex.Message });

                    }
                }
            //});

            if (strresult == "")
            {

                await _emailSender.SendEmailAsync(usuario.Email, "Notificación de extensión de tiempo",
                 "Se realizó exitosamente una extensión de tiempo de " + movimientos.int_tiempo + " minutos a las placas " + movimientos.str_placa + "<br/> si usted no reconoce este movimiento comuniquese con el equipo de soporte.");

                return Ok();
            }
            else
            {
                return Json(new { token = strresult });
            }


        }
        [NonAction]
        /// <returns></returns>
        [HttpPut("mtdMovExtenderTiempoSE")]
        public async Task<ActionResult> mtdMovAgregarTiempoSE(int intIdMovimiento, Movimientos movimientos)
        {
            double fltSaldoAnterior;
            ParametrosController par = new ParametrosController(context);
            ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

           
            string strresult = " ";
            var usuario = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimientos.int_id_usuario_id);

            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{

            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    //var saldo = await context.tbsaldo.FirstOrDefaultAsync(x => x.id == movimientos.int_id_saldo_id);
                    //fltSaldoAnterior = saldo.flt_monto_final;
                    //var parametros = await context.tbparametros.FirstOrDefaultAsync(x => x.intidconcesion_id == movimientos.intidconcesion_id);
                    var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == intIdMovimiento);

                    fltSaldoAnterior = usuario.dbl_saldo_actual;
                    DateTime horaFinalAnterior = response.dtm_hora_fin;
                    //var esp = await context.tbespacios.FirstOrDefaultAsync(x => x.id == response.int_id_espacio);
                    //var zon = await context.tbzonas.FirstOrDefaultAsync(x => x.id == esp.id_zona);
                    int intValidaTiempo = movimientos.int_tiempo;

                    if (usuario.dbl_saldo_actual < movimientos.flt_monto)
                    {
                        strresult = "No tiene saldo suficiente para realizar la operación";

                    }
                    else
                    {
                        var comision = await context.tbcomisiones.FirstOrDefaultAsync(x => x.intidconcesion_id == response.intidconcesion_id && x.str_tipo == "PARQUIMETRO");


                        Double db_porc_comision = comision.dcm_porcentaje;
                        db_porc_comision = db_porc_comision / 100;

                        Double dbl_comision_cobrada = movimientos.flt_monto * db_porc_comision;
                        Double dbl_total_con_comision = movimientos.flt_monto + dbl_comision_cobrada;


                        usuario.dbl_saldo_actual = usuario.dbl_saldo_actual - dbl_total_con_comision;
                        usuario.dbl_saldo_anterior = fltSaldoAnterior;
                        await context.SaveChangesAsync();

                        response.flt_porcentaje_comision = db_porc_comision;
                        response.flt_monto_porcentaje = response.flt_monto_porcentaje + dbl_comision_cobrada;
                        response.flt_total_con_comision = response.flt_total_con_comision + dbl_total_con_comision;

                        response.last_modified_date = time.Value;
                        response.last_modified_by = movimientos.last_modified_by;
                        DateTime horafin = response.dtm_hora_fin.AddMinutes(movimientos.int_tiempo);
                        response.int_tiempo = response.int_tiempo + movimientos.int_tiempo;
                        response.int_tiempo_comprado = response.int_tiempo_comprado + movimientos.int_tiempo;
                        response.dtm_hora_fin = horafin;
                        response.flt_monto = response.flt_monto + movimientos.flt_monto;
                        // response.int_id_espacio = response.int_id_espacio;
                        response.str_comentarios = "EXTENSIÓN DE TIEMPO " + movimientos.int_tiempo;
                        response.int_id_usuario_id = movimientos.int_id_usuario_id;
                        await context.SaveChangesAsync();


                        context.tbsaldo.Add(new Saldos()
                        {
                            created_by = movimientos.last_modified_by,
                            created_date = time.Value,
                            dtmfecha = time.Value,
                            last_modified_date = time.Value,
                            flt_monto_inicial = fltSaldoAnterior,
                            flt_monto_final = usuario.dbl_saldo_actual,
                            str_forma_pago = "VIRTUAL",
                            str_tipo_recarga = "EXTENSION",
                            int_id_usuario_id = usuario.Id,
                            int_id_usuario_trans = usuario.Id

                        });
                        await context.SaveChangesAsync();


                        var saldoV = await context.NetUsers.FirstOrDefaultAsync(x => x.Id == movimientos.int_id_usuario_id);
                        context.tbdetallemovimientos.Add(new DetalleMovimientos()
                        {
                            int_idmovimiento = intIdMovimiento,
                            //int_idespacio = response.int_id_espacio.Value,
                            int_id_usuario_id = response.int_id_usuario_id,
                            int_duracion = movimientos.int_tiempo,
                            dtm_horaInicio = horaFinalAnterior,
                            dtm_horaFin = horafin,
                            flt_importe = movimientos.flt_monto,
                            flt_porcentaje_comision = db_porc_comision,
                            flt_monto_porcentaje = dbl_comision_cobrada,
                            flt_total_con_comision = dbl_total_con_comision,
                            flt_saldo_anterior = fltSaldoAnterior,
                            flt_saldo_fin = saldoV.dbl_saldo_actual,
                            str_observaciones = "EXTENSIÓN DE TIEMPO " + movimientos.int_tiempo,
                            str_latitud = response.str_latitud,
                            str_longitud = response.str_longitud

                        });

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }


                    if (strresult == " ")
                    {
                        await mtdEnviarCorreo(usuario.Email, usuario.UserName, response.str_placa, "EXTENSION", movimientos.int_tiempo, 0, 0, false);
                        return Ok();
                    }
                    else
                    {
                        return Json(new { token = strresult });
                    }

                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    strresult = ex.Message;
                    //return Json(new { token = ex.Message });

                }
            }
            //});
            return Json(new { token = strresult });



        }
        /// <summary>
        /// Método que permite actualizar un movimiento
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        [HttpPut("mtdActualizaMovimientos")]
        public async Task<ActionResult> mtdActualizaMovimientos(int id, [FromBody] Movimientos movimientos)
        {
            try
            {

                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();


                var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == id);
                if (response.id != id)
                {
                    return BadRequest();
                }

                response.last_modified_by = movimientos.last_modified_by;
                response.last_modified_date = horadeTransaccion.Value;
                response.str_placa = movimientos.str_placa;
                response.boolean_auto_recarga = movimientos.boolean_auto_recarga;
                response.boolean_multa = movimientos.boolean_multa;
                response.dt_hora_inicio = movimientos.dt_hora_inicio;
                response.dtm_fecha_insercion_descuento = movimientos.dtm_fecha_insercion_descuento;
                response.dtm_fecha_descuento = movimientos.dtm_fecha_descuento;
                response.dtm_hora_fin = movimientos.dtm_hora_fin;
                response.int_tiempo = movimientos.int_tiempo;
                response.flt_moneda_saldo_previo_descuento = movimientos.flt_moneda_saldo_previo_descuento;
                response.flt_monto = movimientos.flt_monto;
                response.flt_saldo_previo_descuento = movimientos.flt_saldo_previo_descuento;
                response.flt_valor_descuento = movimientos.flt_valor_descuento;
                response.flt_valor_devuelto = movimientos.flt_valor_devuelto;
                response.flt_valor_final_descuento = movimientos.flt_valor_final_descuento;
                response.str_cambio_descuento = movimientos.str_cambio_descuento;
                response.str_codigo_autorizacion = movimientos.str_codigo_autorizacion;
                response.str_codigo_transaccion = movimientos.str_codigo_transaccion;
                response.str_comentarios = movimientos.str_comentarios;
                response.str_hash_tarjeta = movimientos.str_hash_tarjeta;
                response.str_instalacion = movimientos.str_instalacion;
                response.str_instalacion_abrv = movimientos.str_instalacion_abrv;
                response.str_moneda_valor_descuento = movimientos.str_moneda_valor_descuento;
                response.str_referencia_operacion = movimientos.str_referencia_operacion;
                response.str_so = movimientos.str_so;
                response.str_tipo = movimientos.str_tipo;
                response.str_versionapp = movimientos.str_versionapp;
                response.int_id_espacio = movimientos.int_id_espacio;
               // response.int_id_saldo_id = movimientos.int_id_saldo_id;
                response.int_id_usuario_id = movimientos.int_id_usuario_id;
                response.int_id_vehiculo_id = movimientos.int_id_vehiculo_id;
                response.intidconcesion_id = movimientos.intidconcesion_id;
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

        /// <summary>
        /// método que permite dar de baja un movimiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("mtdBajaMovimiento")]
        public async Task<ActionResult<Movimientos>> mtdBajaMovimiento(int id)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == id);
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

        /// <summary>
        /// Método que permite Reactivar un movimiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("mtdReactivarMovimiento")]
        public async Task<ActionResult<Movimientos>> mtdReactivarMovimiento(int id)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> horadeTransaccion = par.mtdObtenerHora();

                var response = await context.tbmovimientos.FirstOrDefaultAsync(x => x.id == id);
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


        //[HttpGet("mtdObtenerIngresos")]
        //public async Task<ActionResult<IEnumerable<DetalleIngresos>>> mtdObtenerIngresos()
        //{
        //    try
        //    {
        //        //ParametrosController par = new ParametrosController(context);
        //        //ActionResult<DateTime> time = par.mtdObtenerFechaMexico();
        //        DateTime date;
        //        DateTime time = DateTime.Now;

        //        DataTable table = new DataTable("IngresosXSemana");
        //        DataColumn column;
        //        DataRow row;

        //        // Create first column and add to the DataTable.
        //        column = new DataColumn();
        //        column.DataType = System.Type.GetType("System.DateTime");
        //        column.ColumnName = "Fecha";
        //        column.ReadOnly = false;
        //        table.Columns.Add(column);

        //        column = new DataColumn();
        //        column.DataType = System.Type.GetType("System.Decimal");
        //        column.ColumnName = "flt_cantidad";
        //        column.ReadOnly = false;
        //        table.Columns.Add(column);


        //        //switch (time.Day)
        //        //{
        //        //    case 1:
        //        //         date = time;
        //        //        break;
        //        //    case 2:
        //        //        date = DateTime.Today.AddDays(1);
        //        //        break;
        //        //    case 3:
        //        //        date = DateTime.Today.AddDays(2);
        //        //        break;
        //        //    case 4:
        //        //        date = DateTime.Today.AddDays(3);
        //        //        break;
        //        //    case 5:
        //        //        date = DateTime.Today.AddDays(4);
        //        //        break;
        //        //    case 6:
        //        //        date = DateTime.Today.AddDays(5);
        //        //        break;


        //        //}

        //        date = DateTime.Today.AddDays(-6);


        //        var selectedDates = new List<DateTime?>();
        //        for (var datos = date; datos <= time; datos = datos.AddDays(1))
        //        {
        //            selectedDates.Add(datos);
        //        }

        //        double bdllTotalXSemana = 0;

        //        var response = (from det in context.tbdetallemovimientos
        //                        join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
        //                        where mov.str_so == "ANDROID" && det.dtm_horaInicio.Date <= time.Date && det.dtm_horaInicio.Date >= date.Date
        //                        select new DetalleIngresos()
        //                        {
        //                            id = det.id,
        //                            int_idmovimiento = det.int_idmovimiento,
        //                            dt_hora_inicio = det.dtm_horaInicio,
        //                            ftl_importe = det.flt_importe,
        //                            flt_descuentos = det.flt_descuentos,
        //                            str_so = mov.str_so,
        //                            str_observaciones = det.str_observaciones

        //                        }).ToList();

        //        foreach (var item in response)
        //        {
        //            bdllTotalXSemana = bdllTotalXSemana + item.ftl_importe;

        //            if (item.flt_descuentos != 0)
        //            {
        //                string d = item.flt_descuentos.ToString();
        //                double flt = double.Parse(d.Trim('-'));

        //                bdllTotalXSemana = bdllTotalXSemana - item.flt_descuentos;
        //            }

        //        }





        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { token = ex.Message });
        //    }
        //}

        /// <summary>
        /// /// Método que devuelve ingresos x dia de aplicaciones Android
        /// </summary>
        /// <returns></returns>
       // [HttpGet("mtdIngresosXDiaAndroid")]
        [NonAction]
        public async Task <IngresosXDiaAndroid> mtdIngresosXDiaAndroid(int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                DateTime timeDate = time.Value;
                double dblTotalXDiaAndriod = 0;
                int intTransAndroid = 0;

                var response = await (from det in context.tbdetallemovimientos
                                      join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                      where mov.str_so == "ANDROID" && det.dtm_horaInicio.Date == timeDate.Date && mov.intidconcesion_id == intIdConcesion
                                      select new DetalleIngresos()
                                      {
                                          id = det.id,
                                          int_idmovimiento = det.int_idmovimiento,
                                          dt_hora_inicio = det.dtm_horaInicio,
                                          ftl_importe = det.flt_importe,
                                          flt_descuentos = det.flt_descuentos,
                                          str_so = mov.str_so,
                                          str_observaciones = det.str_observaciones

                                      }).ToListAsync();

                foreach (var item in response)
                {
                    dblTotalXDiaAndriod = dblTotalXDiaAndriod + item.ftl_importe;

                    if (item.flt_descuentos != 0)
                    {

                        string d = item.flt_descuentos.ToString();
                        double flt = double.Parse(d.Trim('-'));

                        dblTotalXDiaAndriod = dblTotalXDiaAndriod - flt;
                    }
                    if (item.str_observaciones != "DESAPARCADO")
                    {
                        intTransAndroid++;
                    }


                }


                var datosIOS = new IngresosXDiaAndroid()
                {
                    intTransAndroid = intTransAndroid,
                    dblingresosAndroid = dblTotalXDiaAndriod


                };
                return datosIOS;
                
            }
            catch (Exception ex)
            {

                throw;

            }
        }
        /// <summary>
        /// Método que devuelve ingresos x dia de aplicaciones IOS
        /// </summary>
        /// <returns></returns>
        //[HttpGet("mtdIngresosXDiaIos")]
        [NonAction]
        public async Task <IngresosXDiaIOS> mtdIngresosXDiaIos(int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time = par.mtdObtenerFechaMexico();
                int intTransIos = 0;
            
                DateTime timeDate = DateTime.Now;
                double dblTotalXDiaIos = 0;

                var response = await (from det in context.tbdetallemovimientos
                                      join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                      where mov.str_so == "IOS" && det.dtm_horaInicio.Date == timeDate.Date && mov.intidconcesion_id == intIdConcesion
                                      select new DetalleIngresos()
                                      {
                                          id = det.id,
                                          int_idmovimiento = det.int_idmovimiento,
                                          dt_hora_inicio = det.dtm_horaInicio,
                                          ftl_importe = det.flt_importe,
                                          flt_descuentos = det.flt_descuentos,
                                          str_so = mov.str_so,
                                          str_observaciones = det.str_observaciones,
                                          

                                      }).ToListAsync();

                foreach (var item in response)
                {
                    dblTotalXDiaIos = dblTotalXDiaIos + item.ftl_importe;

                    if (item.flt_descuentos != 0)
                    {

                        string d = item.flt_descuentos.ToString();
                        double flt = double.Parse(d.Trim('-'));

                        dblTotalXDiaIos = dblTotalXDiaIos -flt;
                    }
                    if (item.str_observaciones != "DESAPARCADO")
                    {
                        intTransIos++;
                    }

                }

                var datosIOS = new IngresosXDiaIOS()
                {
                    intTransIos = intTransIos,
                    dblingresosIOS = dblTotalXDiaIos


                };
                return datosIOS;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// Método que devuelve el ingreso x semana en Android
        /// </summary>
        /// <returns></returns>
        [HttpGet("mtdObtenerIngresosSemAndroid")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerIngresosSemAndroid(string  dia, int intidConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                DateTime time1 = time.Value;

                DateTime dtDiaAnt = DateTime.MinValue;
                switch (dia)
                {
                    case "Tuesday":
                         dtDiaAnt = DateTime.Now.AddDays(-1);
                        break;
                    case "Wednesday":
                        dtDiaAnt = DateTime.Now.AddDays(-2);
                        break;
                    case "Thursday":
                        dtDiaAnt = DateTime.Now.AddDays(-3);
                        break;
                    case "Friday":
                        dtDiaAnt = DateTime.Now.AddDays(-4);
                        break;
                    case "Saturday":
                        dtDiaAnt = DateTime.Now.AddDays(-5);
                        break;
                     
                }
                string strresult = "";
                //DateTime date;
                //date = DateTime.Today.AddDays(-5);

                //DateTime date;
                //DateTime time = DateTime.Now;

                DataTable table = new DataTable("IngresosXSemana");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Decimal");
                column.ColumnName = "flt_total";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "autos";
                column.ReadOnly = false;
                table.Columns.Add(column);

                //DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos = dtDiaAnt; datos <= time.Value; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {
                    var response = new List<IngresosXSemana>();

                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {

                        DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());

                        string anioF = dia1.Year.ToString();
                        int mesF = dia1.Month;

                        int diaF = dia1.Day;

                        string strMes = "";
                        string strDia = "";

                        //string d = dia1.Date.ToString();
                        //string o = d.Substring(0, 10);
                        //string diaF = o.Substring(0,2);
                        //string mesF = o.Substring(3,3);
                        //string anioF = o.Substring(6);

                        if (mesF < 10)
                        {
                            strMes = 0 + mesF.ToString();
                        }
                        else {
                            strMes = mesF.ToString();
                        }


                        if (diaF < 10)
                        {

                            strDia = 0 + diaF.ToString();

                        }
                        else {
                            strDia = diaF.ToString();
                        }
                        string fechaDeConsulta = anioF+ "/"+strMes+"/" + strDia ;

                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarIngresosXSemDiaAndriod('" + fechaDeConsulta + "'" + ","+ intidConcesion +")", sql))
                        {

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueIngresosXSem(drd));
                            }

                            string devolucion = response[0].fltDevoluciones.ToString();
                            double flt = double.Parse(devolucion.Trim('-'));

                            double total = response[0].fltTotal - flt;

                            row = table.NewRow();
                            row["fecha"] = response[0].dtmFecha;
                            row["flt_total"] = total;
                            row["autos"] = response[0].intAutos;

                            table.Rows.Add(row);
                        }
                        //}


                    }

                }
                return table;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
    }


        [HttpGet("mtdObtenerIngresosSemAndroidL")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerIngresosSemAndroidL(DateTime dtmFechaInicio, DateTime dtmFechaFin, int intidConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                DateTime time1 = time2.Value;

                DateTime dtDiaAnt = DateTime.MinValue;
               
                string strresult = "";
                //DateTime date;
                //date = DateTime.Today.AddDays(-5);

                //DateTime date;
                //DateTime time = DateTime.Now;

                DataTable table = new DataTable("IngresosXSemana");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Decimal");
                column.ColumnName = "flt_total";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "autos";
                column.ReadOnly = false;
                table.Columns.Add(column);

                DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos = dtmFechaInicio; datos <= dtmFechaFin; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {
                    var response = new List<IngresosXSemana>();

                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {

                        DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());

                        string anioF = dia1.Year.ToString();
                        int mesF = dia1.Month;

                        int diaF = dia1.Day;

                        string strMes = "";
                        string strDia = "";

                        //string d = dia1.Date.ToString();
                        //string o = d.Substring(0, 10);
                        //string diaF = o.Substring(0,2);
                        //string mesF = o.Substring(3,3);
                        //string anioF = o.Substring(6);

                        if (mesF < 10)
                        {
                            strMes = 0 + mesF.ToString();
                        }
                        else
                        {
                            strMes = mesF.ToString();
                        }


                        if (diaF < 10)
                        {

                            strDia = 0 + diaF.ToString();

                        }
                        else
                        {
                            strDia = diaF.ToString();
                        }
                        string fechaDeConsulta = anioF + "/" + strMes + "/" + strDia;

                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarIngresosXSemDiaAndriod('" + fechaDeConsulta + "'" + "," + intidConcesion + ")", sql))
                        {

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueIngresosXSem(drd));
                            }

                            string devolucion = response[0].fltDevoluciones.ToString();
                            double flt = double.Parse(devolucion.Trim('-'));

                            double total = response[0].fltTotal - flt;

                            row = table.NewRow();
                            row["fecha"] = response[0].dtmFecha;
                            row["flt_total"] = total;
                            row["autos"] = response[0].intAutos;

                            table.Rows.Add(row);
                        }
                        //}


                    }

                }
                return table;
            }
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }
        /// <summary>
        /// Método que devuelve el ingreso x semana en IOS
        /// </summary>
        /// <returns></returns>
        [HttpGet("mtdObtenerIngresosSemIOS")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerIngresosSemIOS(string dia,int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                DateTime time1 = time2.Value;
                DateTime dtDiaAnt = DateTime.MinValue;
                switch (dia)
                {
                    case "Tuesday":
                        dtDiaAnt = DateTime.Now.AddDays(-1);
                        break;
                    case "Wednesday":
                        dtDiaAnt = DateTime.Now.AddDays(-2);
                        break;
                    case "Thursday":
                        dtDiaAnt = DateTime.Now.AddDays(-3);
                        break;
                    case "Friday":
                        dtDiaAnt = DateTime.Now.AddDays(-4);
                        break;
                    case "Saturday":
                        dtDiaAnt = DateTime.Now.AddDays(-5);
                        break;

                }
                string strresult = "";
                DateTime date;

               // date = DateTime.Today.AddDays(-5);

                //DateTime date;
              

                DataTable table = new DataTable("IngresosXSemana");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Decimal");
                column.ColumnName = "flt_total";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "autos";
                column.ReadOnly = false;
                table.Columns.Add(column);

                DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos = dtDiaAnt; datos <= time; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {
                    var response = new List<IngresosXSemana>();

                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {

                        DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());
                        string anioF = dia1.Year.ToString();
                        int mesF = dia1.Month;

                        int diaF = dia1.Day;

                        string strMes = "";
                        string strDia = "";

                        //string d = dia1.Date.ToString();
                        //string o = d.Substring(0, 10);
                        //string diaF = o.Substring(0,2);
                        //string mesF = o.Substring(3,3);
                        //string anioF = o.Substring(6);

                        if (mesF < 10)
                        {
                            strMes = 0 + mesF.ToString();
                        }
                        else
                        {
                            strMes = mesF.ToString();
                        }


                        if (diaF < 10)
                        {

                            strDia = 0 + diaF.ToString();

                        }
                        else
                        {
                            strDia = diaF.ToString();
                        }
                        string fechaDeConsulta = anioF + "/" + strMes + "/" + strDia;
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarIngresosXSemDiaIOS('" + fechaDeConsulta + "'" + "," + intIdConcesion + ")", sql))
                        {

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueIngresosXSem(drd));
                            }

                            string devolucion = response[0].fltDevoluciones.ToString();
                            double flt = double.Parse(devolucion.Trim('-'));

                            double total = response[0].fltTotal - flt;

                            row = table.NewRow();
                            row["fecha"] = response[0].dtmFecha;
                            row["flt_total"] = total;
                            row["autos"] = response[0].intAutos;
                            table.Rows.Add(row);
                        }
                        //}


                    }

                }
                return table;
            }
            catch (Exception ex)
            {


                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdObtenerIngresosSemIOSL")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerIngresosSemIOSL(DateTime dtmFechaInicio, DateTime dtmFechaFin, int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                DateTime time1 =time2.Value;
                DateTime dtDiaAnt = DateTime.MinValue;
               
                string strresult = "";
                DateTime date;

                // date = DateTime.Today.AddDays(-5);

                //DateTime date;


                DataTable table = new DataTable("IngresosXSemana");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Decimal");
                column.ColumnName = "flt_total";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "autos";
                column.ReadOnly = false;
                table.Columns.Add(column);

                DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos =dtmFechaInicio ; datos <= dtmFechaFin; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {
                    var response = new List<IngresosXSemana>();

                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {

                        DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());
                        string anioF = dia1.Year.ToString();
                        int mesF = dia1.Month;

                        int diaF = dia1.Day;

                        string strMes = "";
                        string strDia = "";

                        //string d = dia1.Date.ToString();
                        //string o = d.Substring(0, 10);
                        //string diaF = o.Substring(0,2);
                        //string mesF = o.Substring(3,3);
                        //string anioF = o.Substring(6);

                        if (mesF < 10)
                        {
                            strMes = 0 + mesF.ToString();
                        }
                        else
                        {
                            strMes = mesF.ToString();
                        }


                        if (diaF < 10)
                        {

                            strDia = 0 + diaF.ToString();

                        }
                        else
                        {
                            strDia = diaF.ToString();
                        }
                        string fechaDeConsulta = anioF + "/" + strMes + "/" + strDia;
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultarIngresosXSemDiaIOS('" + fechaDeConsulta + "'" + "," + intIdConcesion + ")", sql))
                        {

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueIngresosXSem(drd));
                            }

                            string devolucion = response[0].fltDevoluciones.ToString();
                            double flt = double.Parse(devolucion.Trim('-'));

                            double total = response[0].fltTotal - flt;

                            row = table.NewRow();
                            row["fecha"] = response[0].dtmFecha;
                            row["flt_total"] = total;
                            row["autos"] = response[0].intAutos;
                            table.Rows.Add(row);
                        }
                        //}


                    }

                }
                return table;
            }
            catch (Exception ex)
            {


                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdObtenerDatosDashboard")]
        public async Task<ActionResult<DatosDashboard>> mtdObtenerDatosDashboard(int intIdConcesion)
        {
            try
            {

                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                DateTime dia = time.Value;
                DateTime diaAnt = dia.AddDays(-1);
                DateTime date;
                date = DateTime.Today.AddDays(-5);
                int intmesActual = dia.Month;
                int intmesAnt = intmesActual - 1;
                Double dbl_porc_semana = 0;
                Boolean bolLunes = false;
                ResumenSemanal resumenSemanal = null;
                ResumenSemanalAct resumenSemActual = null;
                Double dblSumaTotalAndroid = 0;
                int dblSumaAutosAndroid = 0;
                Double dblSumaTotalIos = 0;
                int dblSumaAutosIos = 0;
                Double dbl_por_dia_ios = 0;
                Double dbl_por_trans_ios = 0;
                Double dblC_cant_ios = 0;
                Double dbl_por_trans_android = 0;
                Double dblC_cant_android = 0;
                double dbl_total_semanal = 0;
                Double dbl_porc_dia_anterior =0 ;
                Double dbl_cantidad_dia_anterior = 0 ;
                Double total_semanal_ios_and = 0;
                Double dbl_dferencia_cant_semanal = 0; 

                ActionResult<DataTable> dt = null ;
                ActionResult<DataTable> dtIos = null;
                ActionResult<DataTable> dtCompSemAndroid = null;
                ActionResult<DataTable> dtCompSemIOS = null;
                DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever 
                DateTime startingDate = DateTime.Today;
                DateTime previousWeekStart = DateTime.MinValue;
                DateTime previousWeekEnd = DateTime.MinValue;

                Double dec_sem_andriod = 0;
                Double dec_sem_ios = 0;

                string strMes = "";
                switch (intmesAnt)
                {
                    case 1:
                        strMes = "January";
                        break;
                    case 2:
                        strMes = "February";
                        break;
                    case 3:
                        strMes = "March";
                        break;
                    case 4:
                        strMes = "April";
                        break;
                    case 5:
                        strMes = "May";
                        break;
                    case 6:
                        strMes = "June";
                        break;
                    case 7:
                        strMes = "July";
                        break;
                    case 8:
                        strMes = "August";
                        break;
                    case 9:
                        strMes = "September";
                        break;
                    case 10:
                        strMes = "October";
                        break;
                    case 11:
                        strMes = "November";
                        break;
                    case 12:
                        strMes = "December";
                        break;
                }

                if (dia.DayOfWeek.ToString() == "Monday")
                {
                     diaAnt = dia.AddDays(-2);
                    // bolLunes = true;
                     weekStart = DayOfWeek.Monday; // or Sunday, or whenever 
                     startingDate = DateTime.Today;

                    while (startingDate.DayOfWeek != weekStart)
                        startingDate = startingDate.AddDays(-1);

                     previousWeekStart = startingDate.AddDays(-7);
                     previousWeekEnd = startingDate.AddDays(-2);
                    resumenSemanal = await context.tbresumensemanal.FirstOrDefaultAsync(x => x.dtm_fecha_inicio.Date == previousWeekStart.Date && x.dtm_fecha_fin.Date == previousWeekEnd.Date && x.int_id_consecion == intIdConcesion);
                    dt = await mtdObtenerIngresosSemAndroidL(previousWeekStart, previousWeekEnd, intIdConcesion);

                    foreach (DataRow row in dt.Value.Rows)
                    {

                        dblSumaTotalAndroid += Convert.ToDouble(row["flt_total"]);
                        dblSumaAutosAndroid += Convert.ToInt32(row["autos"]);

                    }

                    

                    dtIos = await mtdObtenerIngresosSemIOSL(previousWeekStart, previousWeekEnd,intIdConcesion);



                    foreach (DataRow row in dtIos.Value.Rows)
                    {

                        dblSumaTotalIos += Convert.ToDouble(row["flt_total"]);
                        dblSumaAutosIos += Convert.ToInt32(row["autos"]);
                    }

                    
                }

                else
                {
                     dt = await mtdObtenerIngresosSemAndroid(dia.DayOfWeek.ToString(), intIdConcesion);

                   

                    foreach (DataRow row in dt.Value.Rows)
                    {
                    
                        dblSumaTotalAndroid += Convert.ToDouble(row["flt_total"]);
                        dblSumaAutosAndroid += Convert.ToInt32(row["autos"]);

                    }
                   



                    dtIos = await mtdObtenerIngresosSemIOS(dia.DayOfWeek.ToString(),intIdConcesion);

                    

                    foreach (DataRow row in dtIos.Value.Rows)
                    {

                        dblSumaTotalIos += Convert.ToDouble(row["flt_total"]);
                        dblSumaAutosIos += Convert.ToInt32(row["autos"]);


                    }

                     total_semanal_ios_and = dblSumaTotalIos + dblSumaTotalAndroid;

                    System.Globalization.CultureInfo norwCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es");
                    System.Globalization.Calendar cal = norwCulture.Calendar;
                    int intNoSemanaActual = cal.GetWeekOfYear(dia, norwCulture.DateTimeFormat.CalendarWeekRule, norwCulture.DateTimeFormat.FirstDayOfWeek);
                    int inNoSemAnterior = intNoSemanaActual - 1;
                    //int inNoSemAnterior = 35;
                     weekStart = DayOfWeek.Monday; // or Sunday, or whenever 
                     startingDate = DateTime.Today;
                    //DateTime previousWeekEnd = DateTime.MinValue;
                    Double dec_por_sem = 0;
                    while (startingDate.DayOfWeek != weekStart)
                        startingDate = startingDate.AddDays(-1);

                     previousWeekStart = startingDate.AddDays(-7);

                     previousWeekEnd = DateTime.Today;
                    resumenSemanal = await context.tbresumensemanal.FirstOrDefaultAsync(x => x.int_semana == inNoSemAnterior && x.int_id_consecion == intIdConcesion);

                    //if (resumenSemanal != null)
                    //{
                    //     dec_por_sem = ((dblSumaTotalAndroid / resumenSemanal.dec_sem_andriod) - 1);
                    //}
                    //else {
                    //    dec_por_sem = 100;
                    //}

                }

                var resumendiario = await context.tbresumendiario.FirstOrDefaultAsync(x => x.dtm_fecha.Date == diaAnt.Date && x.int_id_consecion == intIdConcesion);

                var resumenMensual = await context.tbresumenmensual.FirstOrDefaultAsync(x => x.str_mes == strMes && x.int_id_consecion == intIdConcesion);

                var datosIOS = await mtdIngresosXDiaIos(intIdConcesion);
                var datosDiaAndriod = await mtdIngresosXDiaAndroid(intIdConcesion);

                //Double dbl_porc_dia_anterior = 0;
                //Double dbl_cantidad_dia_anterior = 0;
                double dbl_total = datosDiaAndriod.dblingresosAndroid + datosIOS.dblingresosIOS;
                if (resumendiario != null)
                {
                    dbl_cantidad_dia_anterior = dbl_total - resumendiario.dec_total;
                    dbl_porc_dia_anterior = ((dbl_total / resumendiario.dec_total) - 1);
                    dbl_porc_dia_anterior = dbl_porc_dia_anterior * 100;

                    if (resumendiario.dec_ios != 0)
                    {
                        dbl_por_trans_ios = ((datosIOS.dblingresosIOS / resumendiario.dec_ios) - 1);
                        dbl_por_trans_ios = dbl_por_trans_ios * 100;
                    }

                    //if (resumendiario.dec_ios != 0)
                    //{
                    //    // int_por_trans_ios = ((datosIOS.intTransIos / resumendiario.int_ios) - 1);
                    //    dbl_por_trans_ios = ((datosIOS.dblingresosIOS / resumendiario.dec_ios) - 1);

                    //}

                    dblC_cant_ios = datosIOS.dblingresosIOS - resumendiario.dec_ios;
                    if (resumendiario.dec_andriod != 0)
                    {
                        //int_por_trans_android = ((datosDiaAndriod.intTransAndroid / resumendiario.int_andriod) - 1);
                        dbl_por_trans_android = ((datosDiaAndriod.dblingresosAndroid / resumendiario.dec_andriod) - 1);
                        dbl_por_trans_android = dbl_por_trans_android * 100;
                    }

                    //Revisar esta parte si esta bien o solo es resumendiario.dec_andriod
                    dblC_cant_android = datosDiaAndriod.dblingresosAndroid - resumendiario.dec_andriod;

                }
                else {
                    dbl_total_semanal = total_semanal_ios_and + 0;
                }


                if (resumenSemanal != null)
                {
                    // dbl_total_semanal= resumenSemanal.dec_sem_total;
                    dbl_porc_semana = ((dbl_total / resumenSemanal.dec_sem_total) - 1);
                    dbl_porc_semana = dbl_porc_semana * 100;
                    dbl_dferencia_cant_semanal = total_semanal_ios_and - resumenSemanal.dec_sem_total;

                }
                else {
                    dbl_dferencia_cant_semanal = total_semanal_ios_and;
                }

                //dblC_cant_android = 100;
                if (bolLunes)
                {
                    dbl_total_semanal = resumenSemanal.dec_sem_total;
                    dec_sem_andriod = 0;
                    dec_sem_ios = 0;
                    dbl_dferencia_cant_semanal = dbl_total - resumenSemanal.dec_sem_total;
                    dbl_porc_semana = ((dbl_total / resumenSemanal.dec_sem_total) - 1);

                }
                else
                {
                    dec_sem_andriod = dblSumaTotalAndroid;
                    dec_sem_ios = dblSumaTotalIos;
                    dbl_total_semanal = total_semanal_ios_and;


                }

                previousWeekStart = startingDate.AddDays(-15);
                previousWeekEnd = startingDate.AddDays(-2);


                //dtCompSemAndroid = await mtdObtenerIngresosSemAndroidL(previousWeekStart, previousWeekEnd,intIdConcesion);
                //dtCompSemIOS = await mtdObtenerIngresosSemIOSL(previousWeekStart, previousWeekEnd,intIdConcesion);


                DatosDashboard datos;

                datos = new DatosDashboard()
                {
                    dblSemanalAndriod = (Math.Truncate(dec_sem_andriod * 100) / 100),
                    dtSemanaAndroid = dt.Value,
                    dblSemanalIOS = dec_sem_ios,
                    dtSemanaIOS = dtIos.Value,

                    dblIngresosDiaAndriod = datosDiaAndriod.dblingresosAndroid,
                    int_porcentaje_andriod = (Math.Truncate(dbl_por_trans_android * 100) / 100) ,
                    dbl_total_ant_andriod = dblC_cant_android,

                    dblIngresosDiaIOS = datosIOS.dblingresosIOS,
                    int_porcentaje_IOS = (Math.Truncate(dbl_por_trans_ios * 100) / 100)  ,
                    dbl_total_ant_IOS = dblC_cant_ios,

                    dbl_total_del_dia = dbl_total,
                    int_porcentaje_total = (Math.Truncate(dbl_porc_dia_anterior * 100) / 100)  ,
                    dbl_total_dia_ant = dbl_cantidad_dia_anterior,

                    dbl_total_ac_semana = dbl_total_semanal,
                    int_porcentaje_semana = (Math.Truncate(dbl_porc_semana * 100) / 100) ,
                    dbl_total_ant_semana = dbl_dferencia_cant_semanal,


                };
                
             

                return datos;
                //return Json(new { dblDiaIOS,dblDiaAndriod,dblC_cant_ios, dblC_cant_android, resumendiario, resumenSemanal, resumenMensual });
            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message } );
            }
        }

  

        /// <summary>
        /// Obtiene los ingresos de Ios y Android de acuerdo al rango de fachas enviadas
        /// </summary>
        /// <param name="intIdConcesion"></param>
        /// <param name="dtmFechaInicio"></param>
        /// <param name="dtmFechaFin"></param>
        /// <returns></returns>
        [HttpGet("mtdObtenerCompIngresos")]
        public async Task<ActionResult> mtdObtenerCompIngresos(int intIdConcesion, DateTime dtmFechaInicio,  DateTime dtmFechaFin)
        {
            try
            {

                //ParametrosController par = new ParametrosController(context);
                //ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                ActionResult<DataTable> dtCompSemAndroid = null;
                ActionResult<DataTable> dtCompSemIOS = null;
                //DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever 
                //DateTime startingDate = DateTime.Today;
                DateTime previousWeekStart = DateTime.MinValue;
                DateTime previousWeekEnd = DateTime.MinValue;

                Double dbl_total_andriod = 0;
                Double dbl_total_ios = 0;
                Double dbl_total = 0;

                //previousWeekStart = startingDate.AddDays(-15);
                //previousWeekEnd = startingDate.AddDays(-2);
                dtCompSemAndroid = await mtdObtenerIngresosSemAndroidEF(dtmFechaInicio, dtmFechaFin,intIdConcesion);
                dtCompSemIOS = await mtdObtenerIngresosSemIOSEF(dtmFechaInicio, dtmFechaFin, intIdConcesion);


                foreach (DataRow row in dtCompSemAndroid.Value.Rows)
                {
                    dbl_total_andriod += Convert.ToDouble(row["flt_total"]);
                }

                foreach (DataRow row in dtCompSemIOS.Value.Rows)
                {
                    dbl_total_ios += Convert.ToDouble(row["flt_total"]);
                }

                dbl_total = dbl_total_andriod + dbl_total_ios;

                return Json(new { dtCompSemAndroid, dtCompSemIOS, dbl_total_andriod, dbl_total_ios, dbl_total });
            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la comprativa de transcciones por dispositivo
        /// </summary>
        /// <param name="intIdConcesion"></param>
        /// <param name="dtmFechaInicio"></param>
        /// <param name="dtmFechaFin"></param>
        /// <returns></returns>
        [HttpGet("mtdObtenerCompTransaciones")]
        public async Task<ActionResult> mtdObtenerCompTransaciones(int intIdConcesion, DateTime dtmFechaInicio, DateTime dtmFechaFin)
        {
            try
            {

                //ParametrosController par = new ParametrosController(context);
                //ActionResult<DateTime> time = par.mtdObtenerFechaMexico();

                ActionResult<DataTable> dtCompTransAndroid = null;
                ActionResult<DataTable> dtCompTransIOS = null;
                //DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever 
                //DateTime startingDate = DateTime.Today;
                DateTime previousWeekStart = DateTime.MinValue;
                DateTime previousWeekEnd = DateTime.MinValue;
                Double dbl_total_andriod = 0;
                Double dbl_total_ios = 0;
                Double dbl_total = 0;
                //previousWeekStart = startingDate.AddDays(-15);
                //previousWeekEnd = startingDate.AddDays(-2);
                dtCompTransAndroid = await mtdObtenerTransSemAndroidEF(dtmFechaInicio, dtmFechaFin, intIdConcesion);
                dtCompTransIOS = await mtdObtenerTransIOSEF(dtmFechaInicio, dtmFechaFin, intIdConcesion);



                foreach (DataRow row in dtCompTransAndroid.Value.Rows)
                {
                    dbl_total_andriod += Convert.ToDouble(row["int_transacciones"]);
                }

                foreach (DataRow row in dtCompTransIOS.Value.Rows)
                {
                    dbl_total_ios += Convert.ToDouble(row["int_transacciones"]);
                }

                dbl_total = dbl_total_andriod + dbl_total_ios;

                return Json(new { dtCompTransAndroid, dtCompTransIOS, dbl_total_andriod, dbl_total_ios, dbl_total });
            }
            catch (Exception ex)
            {

                return Json(new { token = ex.Message });
            }
        }

        /// <summary>
        /// obtiene el resumen de ingresos mensual, es la ultima parte del Dashboard
        /// </summary>
        /// <param name="intIdConcesion"></param>
        /// <returns></returns>
        [HttpGet("mtdObtenerResumenIngresosMensual")]
        //[NonAction]
        public async Task<ActionResult<ResumenIngresosMensual>> mtdObtenerResumenIngresosMensual(int intIdConcesion)
        {
            try
            {
                //ParametrosController par = new ParametrosController(context);
                //ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                // DateTime time1 = time2.Value;
                DateTime time1 = DateTime.Now; ;
                DateTime dtmFechaFin = time1.AddDays(-1);
                DateTime dtDiaAnt = DateTime.MinValue;

                string strresult = "";
                //DateTime date;
                //date = DateTime.Today.AddDays(-5);

                //DateTime date;
                //DateTime time = DateTime.Now;

                DataTable table = new DataTable("Transsacciones");
                DataColumn column;
                DataRow row;
                Double dblSumaMesActualTotal = 0;
                Double dblSumaMesAnteriorTotal = 0;
                int intSumaMesAnteriorTransTotal = 0;
                Double dblPorcentajeIngresos = 0;
                int intPorcentajeTransacciones = 0;



                Double mesAntIOS = 0;
                int mesAntTransIOS = 0;
                int intDiaTransIOS = 0;
                int intDiaTransAndroid = 0;
                Double mesAntAndroid = 0;
                int mesAntTransAndroid = 0;
                int intTransIOSMesActual = 0;
                int intTransAndroidMesActual = 0;
                int intTrans = 0;
                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Double");
                column.ColumnName = "flt_monto";
                column.ReadOnly = false;
                table.Columns.Add(column);



                //DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();


                DateTime fecha1 = new DateTime(time1.Year, time1.Month, 1);

                for (var datos = fecha1; datos <= dtmFechaFin; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {

                    DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());


                    var responseDiario = await context.tbresumendiario.FirstOrDefaultAsync(x => x.dtm_fecha.Date == dia1.Date && x.int_id_consecion == intIdConcesion);

                    if (responseDiario != null)
                    {
                        row = table.NewRow();
                        row["fecha"] = responseDiario.dtm_fecha;
                        row["flt_monto"] = responseDiario.dec_total;

                        table.Rows.Add(row);
                    }
                    else
                    {
                        row = table.NewRow();
                        row["fecha"] = dia1;
                        row["flt_monto"] = 0;
                        table.Rows.Add(row);
                    }
                }



                var datosIOS = await mtdIngresosXDiaIos(intIdConcesion);
                var datosDiaAndriod = await mtdIngresosXDiaAndroid(intIdConcesion);

                Double dbltotal = datosIOS.dblingresosIOS + datosDiaAndriod.dblingresosAndroid;

                row = table.NewRow();
                row["fecha"] = time1;
                row["flt_monto"] = dbltotal;
                table.Rows.Add(row);


                foreach (DataRow rows in table.Rows)
                {

                    dblSumaMesActualTotal += Convert.ToDouble(rows["flt_monto"]);

                }

                DateTime fechaFin = time1.AddMonths(-1);
                DateTime fechaIni = fecha1.AddMonths(-1);
                string strMes = "";
                switch (fechaIni.Month)
                {
                    case 1:
                        strMes = "January";
                        break;
                    case 2:
                        strMes = "February";
                        break;
                    case 3:
                        strMes = "March";
                        break;
                    case 4:
                        strMes = "April";
                        break;
                    case 5:
                        strMes = "May";
                        break;
                    case 6:
                        strMes = "June";
                        break;
                    case 7:
                        strMes = "July";
                        break;
                    case 8:
                        strMes = "August";
                        break;
                    case 9:
                        strMes = "September";
                        break;
                    case 10:
                        strMes = "October";
                        break;
                    case 11:
                        strMes = "November";
                        break;
                    case 12:
                        strMes = "December";
                        break;
                }
                //ActionResult<DataTable> dtIOS = await mtdObtenerIngresosSemIOSEF(fechaIni, fechaFin,intIdConcesion);
                //ActionResult<DataTable> dtAndroid = await mtdObtenerIngresosSemAndroidEF(fechaIni, fechaFin,intIdConcesion);

                //foreach (DataRow rows in dtIOS.Value.Rows)
                //{

                //     mesAntIOS  += Convert.ToDouble(rows["flt_total"]);

                //}
                //foreach (DataRow rows in dtAndroid.Value.Rows)
                //{

                //    mesAntAndroid += Convert.ToDouble(rows["flt_total"]);

                //}
                var mesAntResponse = await context.tbresumenmensual.FirstOrDefaultAsync(x => x.int_id_consecion == intIdConcesion && x.str_mes == strMes);
                Double Porc = 0;

                if (mesAntResponse != null)
                {
                    dblSumaMesAnteriorTotal = mesAntResponse.dec_mes_total;

                    dblPorcentajeIngresos = ((dblSumaMesActualTotal / dblSumaMesAnteriorTotal) - 1);
                     Porc = dblPorcentajeIngresos * 100;

                    intSumaMesAnteriorTransTotal = mesAntResponse.int_mes_total;
                }
                else
                {
                    dblSumaMesAnteriorTotal = 0.00;
                    dblPorcentajeIngresos = 0.00;
                    Porc = 100;

                    intSumaMesAnteriorTransTotal = 0;
                }

                ActionResult<DataTable> dtTransIOS = await mtdObtenerTransIOSEF(fecha1, dtmFechaFin, intIdConcesion);
                ActionResult<DataTable> dtTransAndroid = await mtdObtenerTransSemAndroidEF(fecha1, dtmFechaFin, intIdConcesion);



                foreach (DataRow rows in dtTransIOS.Value.Rows)
                {

                    mesAntTransIOS += Convert.ToInt32(rows["int_transacciones"]);

                }
                foreach (DataRow rows in dtTransAndroid.Value.Rows)
                {

                    mesAntTransAndroid += Convert.ToInt32(rows["int_transacciones"]);

                }

                intSumaMesAnteriorTransTotal = mesAntTransIOS + mesAntTransAndroid;


                ActionResult<DataTable> dtTransIOSDia = await mtdObtenerTransIOSEF(fecha1, time1, intIdConcesion);
                ActionResult<DataTable> dtTransAndroidDia = await mtdObtenerTransSemAndroidEF(fecha1, time1, intIdConcesion);


                foreach (DataRow rows in dtTransIOSDia.Value.Rows)
                {

                    intTransIOSMesActual += Convert.ToInt32(rows["int_transacciones"]);

                }
                foreach (DataRow rows in dtTransAndroidDia.Value.Rows)
                {

                    intTransAndroidMesActual += Convert.ToInt32(rows["int_transacciones"]);

                }

                int intMesActual = intTransIOSMesActual + intTransAndroidMesActual;

                var response = await (from det in context.tbdetallemovimientos
                                      join mov in context.tbmovimientos on det.int_idmovimiento equals mov.id
                                      where det.dtm_horaInicio.Date == time1.Date && mov.intidconcesion_id == intIdConcesion
                                      select new DetalleIngresos()
                                      {
                                          id = det.id,
                                          int_idmovimiento = det.int_idmovimiento,
                                          dt_hora_inicio = det.dtm_horaInicio,
                                          ftl_importe = det.flt_importe,
                                          flt_descuentos = det.flt_descuentos,
                                          str_so = mov.str_so,
                                          str_observaciones = det.str_observaciones,


                                      }).ToListAsync();

                foreach (var item in response)
                {
                    if (item.str_observaciones != "DESAPARCADO")
                    {
                        intTrans++;
                    }

                }

                int intTotalTransMesActual = intMesActual + intTrans;


                intPorcentajeTransacciones = ((intTotalTransMesActual / intSumaMesAnteriorTransTotal) - 1);


                var data = new ResumenIngresosMensual()
                {
                    dtFechas = table,
                    dblMontoMensual = dblSumaMesActualTotal,
                    dblPorcentajeIngresos = Porc,
                    intPorcentajeTransacciones = intPorcentajeTransacciones
                };
                return data;
            }


            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }


        [HttpGet("mtdObtenerIngresosSemAndroidEF")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerIngresosSemAndroidEF(DateTime dtmFechaInicio, DateTime dtmFechaFin, int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                DateTime time1 = time2.Value;

                DateTime dtDiaAnt = DateTime.MinValue;

                string strresult = "";
                //DateTime date;
                //date = DateTime.Today.AddDays(-5);

                //DateTime date;
                //DateTime time = DateTime.Now;

                DataTable table = new DataTable("IngresosXSemana");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Decimal");
                column.ColumnName = "flt_total";
                column.ReadOnly = false;
                table.Columns.Add(column);

           

                DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos = dtmFechaInicio; datos <= dtmFechaFin; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {
                  
                 DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());


                    var response = await context.tbresumendiario.FirstOrDefaultAsync(x=> x.dtm_fecha.Date == dia1.Date && x.int_id_consecion == intIdConcesion);

                    if (response != null)
                    {
                        row = table.NewRow();
                        row["fecha"] = response.dtm_fecha;
                        row["flt_total"] = response.dec_andriod;

                        table.Rows.Add(row);
                    }
                    else {
                        row = table.NewRow();
                        row["fecha"] = dia1;
                        row["flt_total"] = 0;
                        table.Rows.Add(row);
                    }
                }

                return table;
            }
             
            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdObtenerTransAndroidEF")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerTransSemAndroidEF(DateTime dtmFechaInicio, DateTime dtmFechaFin, int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                DateTime time1 = time2.Value;

                DateTime dtDiaAnt = DateTime.MinValue;

                string strresult = "";
                //DateTime date;
                //date = DateTime.Today.AddDays(-5);

                //DateTime date;
                //DateTime time = DateTime.Now;

                DataTable table = new DataTable("Transsacciones");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "int_transacciones";
                column.ReadOnly = false;
                table.Columns.Add(column);



                DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos = dtmFechaInicio; datos <= dtmFechaFin; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {

                    DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());


                    var response = await context.tbresumendiario.FirstOrDefaultAsync(x => x.dtm_fecha.Date == dia1.Date &&  x.int_id_consecion == intIdConcesion);

                    if (response != null)
                    {
                        row = table.NewRow();
                        row["fecha"] = response.dtm_fecha;
                        row["int_transacciones"] = response.int_andriod;

                        table.Rows.Add(row);
                    }
                    else
                    {
                        row = table.NewRow();
                        row["fecha"] = dia1;
                        row["int_transacciones"] = 0;
                        table.Rows.Add(row);
                    }
                }

                return table;
            }

            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }


        [HttpGet("mtdObtenerTransIOSEF")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerTransIOSEF(DateTime dtmFechaInicio, DateTime dtmFechaFin, int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                DateTime time1 = time2.Value;

                DateTime dtDiaAnt = DateTime.MinValue;

                string strresult = "";
                //DateTime date;
                //date = DateTime.Today.AddDays(-5);

                //DateTime date;
                //DateTime time = DateTime.Now;

                DataTable table = new DataTable("Transsacciones");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "int_transacciones";
                column.ReadOnly = false;
                table.Columns.Add(column);



                DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos = dtmFechaInicio; datos <= dtmFechaFin; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {

                    DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());


                    var response = await context.tbresumendiario.FirstOrDefaultAsync(x => x.dtm_fecha.Date == dia1.Date && x.int_id_consecion == intIdConcesion);

                    if (response != null)
                    {
                        row = table.NewRow();
                        row["fecha"] = response.dtm_fecha;
                        row["int_transacciones"] = response.int_ios;

                        table.Rows.Add(row);
                    }
                    else
                    {
                        row = table.NewRow();
                        row["fecha"] = dia1;
                        row["int_transacciones"] = 0;
                        table.Rows.Add(row);
                    }
                }

                return table;
            }

            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

      
        [HttpGet("mtdObtenerIngresosSemIOSEF")]
        //[NonAction]
        public async Task<ActionResult<DataTable>> mtdObtenerIngresosSemIOSEF(DateTime dtmFechaInicio, DateTime dtmFechaFin, int intIdConcesion)
        {
            try
            {
                ParametrosController par = new ParametrosController(context);
                ActionResult<DateTime> time2 = par.mtdObtenerFechaMexico();

                DateTime time1 = time2.Value;

                DateTime dtDiaAnt = DateTime.MinValue;

                string strresult = "";
                //DateTime date;
                //date = DateTime.Today.AddDays(-5);

                //DateTime date;
                //DateTime time = DateTime.Now;

                DataTable table = new DataTable("IngresosXSemana");
                DataColumn column;
                DataRow row;

                // Create first column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "fecha";
                column.ReadOnly = false;
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Decimal");
                column.ColumnName = "flt_total";
                column.ReadOnly = false;
                table.Columns.Add(column);

                DateTime time = time1;
                DateTime[] array = new DateTime[6];
                var selectedDates = new ArrayList();
                for (var datos = dtmFechaInicio; datos <= dtmFechaFin; datos = datos.AddDays(1))
                {
                    selectedDates.Add(datos);
                }

                for (int i = 0; i < selectedDates.Count; i++)
                {

                    DateTime dia1 = DateTime.Parse(selectedDates[i].ToString());


                    var response = await context.tbresumendiario.FirstOrDefaultAsync(x => x.dtm_fecha.Date == dia1.Date && x.int_id_consecion == intIdConcesion);

                    if (response != null)
                    {
                        row = table.NewRow();
                        row["fecha"] = response.dtm_fecha;
                        row["flt_total"] = response.dec_ios;

                        table.Rows.Add(row);
                    }
                    else
                    {
                        row = table.NewRow();
                        row["fecha"] = dia1;
                        row["flt_total"] = 0;
                        table.Rows.Add(row);
                    }
                }

                return table;
            }

            catch (Exception ex)
            {
                return Json(new { token = ex.Message });
            }
        }

        [HttpGet("mtdObtenerIngresosMensuales")]
        public async Task<ActionResult<IngresosMensuales>> mtdObtenerIngresosMensuales()
        {
            //ParametrosController par = new ParametrosController(context);
            //ActionResult<DateTime> horaTransaccion = par.mtdObtenerFechaMexico();
            //DateTime time = horaTransaccion.Value;
            DateTime time = DateTime.Now;
            DateTime mesAnterior = DateTime.Now.Date.AddMonths(-1);

            var recargasMesAnt = await context.tbsaldo.Where(x => x.str_tipo_recarga == "RECARGA" && x.dtmfecha.Date.Month == mesAnterior.Date.Month).ToListAsync();
            var regargasUsuarios = await context.tbsaldo.Where(x => x.str_tipo_recarga == "RECARGA" && x.dtmfecha.Date.Month == time.Date.Month).ToListAsync();

            double recargaUsMesAnt = recargasMesAnt.Sum(x => x.flt_monto);
            double recargasUsuarios= regargasUsuarios.Sum(x => x.flt_monto);
            double comisionRecargas = regargasUsuarios.Sum(x => x.flt_porcentaje_comision);
            double totalCobrado = recargasUsuarios + comisionRecargas;
            double saldoUsuarioMes = recargaUsMesAnt + recargasUsuarios;

            var ventas = await context.tbmovimientos.Where(x => x.dt_hora_inicio.Date.Month == time.Date.Month).ToListAsync();

            double ventasConcesion = ventas.Sum(x=> x.flt_monto);
            double comision = ventas.Sum(x => x.flt_monto_porcentaje);
            double totalCobradoC = ventasConcesion + comision;
            double saldoFinUsuarios = saldoUsuarioMes - totalCobradoC;
            double ingresoXCom = comisionRecargas + comision;

            return new IngresosMensuales()
            {
                //(Math.Truncate(dbl_porc_dia_anterior * 100) / 100)
                SaldoUsuarioMesAnterior = recargaUsMesAnt,
                RecargaUsuario = recargasUsuarios,
                ComisionRecarga = (Math.Round(comisionRecargas * 100) / 100),
                TotalCobradoRecarga = totalCobrado,
                SaldoUsuarioMes = saldoUsuarioMes,
                VentaConcesion = ventasConcesion,
                Comision = comision,
                TotalCobradoCompra = totalCobradoC,
                SaldoFinalUsuaios = saldoFinUsuarios,
                IngresoXComisiones = (Math.Round(ingresoXCom * 100) / 100)
            };
        }

        [NonAction]
        private IngresosXSemana MapToValueIngresosXSem(NpgsqlDataReader reader)
        {
            return new IngresosXSemana()
            {

                fltTotal = reader["total"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["total"],
                fltDevoluciones = reader["devoluciones"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["devoluciones"],
                dtmFecha = reader["fecha"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["fecha"],
                intAutos = reader["autos"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["autos"]


            };


        }

        

    }

}
