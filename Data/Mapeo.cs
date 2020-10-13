using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Data
{
    public class Mapeo
    {
        [NonAction]
        public MovimientosJoin MapToValueMovimientos(NpgsqlDataReader reader)
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
        public MovimientosJoin MapToValueMovimientos2(NpgsqlDataReader reader)
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
        public MovimientosJoin MapToValueMovimientosse(NpgsqlDataReader reader)
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
        [NonAction]
        public MovimientosJoin MapToValueMovimientos2se(NpgsqlDataReader reader)
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
    }
}
