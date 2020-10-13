using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DetalleMovimientosController: Controller
    {
        private readonly ApplicationDbContext context;
        private readonly string _connectionString;
        public DetalleMovimientosController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet("mtdConsultaDetalleMovimientos")]
        public async Task<ActionResult<List<DetalleMovimientosJoin>>> mtdConsultaDetalleMovimientos(string dtmFecha, string dtmFechaFin, int intIdConcesion)
        {
            try
            {
                if (dtmFechaFin == null)
                {
                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultardetallexdia('" + dtmFecha + "'"+"," + intIdConcesion + ")", sql))
                        {
                            var response = new List<DetalleMovimientosJoin>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueDetalleMovimientosJoin(drd));
                            }
                            return response;

                        }
                    }

                }
                else {
                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultardetallexfechas('" + dtmFecha + "'" + "," + "'" + dtmFechaFin + "'" +","+intIdConcesion+ ")", sql))
                        {
                            var response = new List<DetalleMovimientosJoin>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueDetalleMovimientosJoin(drd));
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

        [HttpGet("mtdConsultarDetalleXIdMov")]
        public async Task<ActionResult<ICollection<DetalleMovimientos>>> mtdConsultarDetalleXIdMov(int intIdMovimiento)
        {
            try
            {
                
                    using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultardetallexidmov('" + intIdMovimiento + "'" + ")", sql))
                        {
                            var response = new List<DetalleMovimientos>();

                            await sql.OpenAsync();

                            NpgsqlDataReader drd = cmd.ExecuteReader();
                            while (await drd.ReadAsync())
                            {
                                response.Add(MapToValueDetMovimientos(drd));
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

        [HttpGet("mtdConsultarDetalleXIdMovSE")]
        public async Task<ActionResult<ICollection<DetalleMovimientos>>> mtdConsultarDetalleXIdMovSE(int intIdMovimiento)
        {
            try
            {

                using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from public.fncconsultardetallexidmov('" + intIdMovimiento + "'" + ")", sql))
                    {
                        var response = new List<DetalleMovimientos>();

                        await sql.OpenAsync();

                        NpgsqlDataReader drd = cmd.ExecuteReader();
                        while (await drd.ReadAsync())
                        {
                            response.Add(MapToValueDetMovimientos(drd));
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
        private DetalleMovimientos MapToValueDetMovimientos(NpgsqlDataReader reader)
        {
            return new DetalleMovimientos()
            {
                id = reader["id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["id"],
                int_idespacio =  reader["int_idespacio"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_idespacio"],
                int_id_usuario_id = reader["int_id_usuario_id"].ToString(),
                int_idmovimiento = reader["int_idmovimiento"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_idmovimiento"],
                int_id_zona = reader["int_id_zona"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_id_zona"],
                int_duracion = reader["int_duracion"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_duracion"],
                dtm_horaInicio = reader["dtm_horainicio"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_horainicio"],
                dtm_horaFin = reader["dtm_horafin"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dtm_horafin"],
                flt_importe = reader["flt_importe"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_importe"],
                flt_descuentos = reader["flt_descuentos"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_descuentos"],
                flt_saldo_anterior = reader["flt_saldo_anterior"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_saldo_anterior"],
                flt_saldo_fin = reader["flt_saldo_fin"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_saldo_fin"],
                str_observaciones = reader["str_observaciones"].ToString()
               
            };

        }

        [NonAction]
        private DetalleMovimientosJoin MapToValueDetalleMovimientosJoin(NpgsqlDataReader reader)
        {
            return new DetalleMovimientosJoin()
            {
                id = reader["id"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["id"],
                int_idmovimiento = reader["int_idmovimiento"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_idmovimiento"],
                str_placas = reader["str_placas"].ToString(),
                str_usuario = reader["strnombre"].ToString() + " " + reader["strapellidos"].ToString(),
                str_observaciones = reader["str_observaciones"].ToString(),
                int_duracion = reader["int_duracion"] == DBNull.Value ? Convert.ToInt32(0) : (int)reader["int_duracion"],
                dt_hora_inicio = reader["dt_hora_inicio"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dt_hora_inicio"],
                dt_hora_fin = reader["dt_hora_fin"] == DBNull.Value ? Convert.ToDateTime(null) : (DateTime)reader["dt_hora_fin"],
                ftl_importe = reader["flt_importe"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_importe"],
                flt_descuentos = reader["flt_descuentos"] == DBNull.Value ? Convert.ToDouble(0) : (double)reader["flt_descuentos"]
               
            };

        }


    }
}
