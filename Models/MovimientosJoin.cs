using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class MovimientosJoin
    {
        public int id { get; set; }
        public string str_status { get; set; }
        public string str_placa { get; set; }
        public int int_tiempo { get; set; }
        public Double flt_monto { get; set; }
        public DateTime dt_hora_inicio { get; set; }
        public DateTime dtm_hora_fin { get; set; }
        public string str_tiemporest { get; set; }
        public string str_tiempo { get; set; }
        public int int_id_espacio { get; set; }
        public string str_clave_esp { get; set; }
        public string str_marcador { get; set; }
        public string str_descripcion_zona { get; set; }
        public string str_comentarios { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }
        public int int_timpo_restante { get; set; }
        public string Email {get; set;}
        public string str_nombre_completo { get; set; }
        public string str_razon_social { get; set; }
        public string str_rfc { get; set; }
        public Boolean bit_status { get; set; }
        public Boolean boolean_auto_recarga { get; set; }
        public IList detalleMovimientos { get; set; }
        
    }

    public class MovimientosHistorial
    {
        public int intParkingXdia { get; set;}
        public int intParkingXMes { get; set; }
        public int intParkingDesdeInicio { get; set; }

        public int intMultasXDia { get; set; }
        public int intMultasXMes { get; set; }
        public int intMultasDesdeInicio { get; set; }
        public Vehiculos lstDatosvehiculo { get; set; }
       // public ApplicationUser lstUsuario { get; set; }
        public IList lstMovimientos { get; set; }



    }

    public class MovimientosZonas
    {
        public int id { get; set; }
        
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }
        public string str_placa { get; set; }
        public Boolean boolean_auto_recarga { get; set; }
        public Boolean boolean_multa { get; set; }
        public DateTime dt_hora_inicio { get; set; }
        public DateTime dtm_fecha_insercion_descuento { get; set; }
        public DateTime dtm_fecha_descuento { get; set; }

        public DateTime dtm_hora_fin { get; set; }

        public int int_tiempo { get; set; }

        public Double flt_moneda_saldo_previo_descuento { get; set; }

        public Double flt_monto { get; set; }

        public Double flt_saldo_previo_descuento { get; set; }
        public Double flt_valor_descuento { get; set; }
        public Double flt_valor_devuelto { get; set; }
        public Double flt_valor_final_descuento { get; set; }
        public string str_cambio_descuento { get; set; }
        public string str_codigo_autorizacion { get; set; }
        public string str_codigo_transaccion { get; set; }
        public string str_comentarios { get; set; }
        public string str_hash_tarjeta { get; set; }
        public string str_instalacion { get; set; }
        public string str_instalacion_abrv { get; set; }
        public string str_moneda_valor_descuento { get; set; }
        public string str_referencia_operacion { get; set; }
        public string str_so { get; set; }
        public string str_tipo { get; set; }
        public string str_versionapp { get; set; }
        public string str_descripcion_zona { get; set; }
        public int int_id_espacio { get; set; }
        public int int_id_saldo_id { get; set; }
        public string int_id_usuario_id { get; set; }
        public int int_id_vehiculo_id { get; set; }
        public int intidconcesion_id { get; set; }
        public int int_id_multa { get; set; }



    }

    public class IngresosXSemana {
        public Double fltTotal { get; set; }
        public Double fltDevoluciones { get; set; }
        public DateTime dtmFecha { get; set; }
        public int intAutos { get; set;  }

    }
}
