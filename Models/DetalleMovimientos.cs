using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class DetalleMovimientos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [ForeignKey("tbmovimientos")]
        public int int_idmovimiento { get; set; }
        public Movimientos tbmovimientos { get; set; }
        [ForeignKey("tbespacios")]
        public System.Nullable<int> int_idespacio {get; set; }
        public Espacios tbespacios { get; set; }
        [ForeignKey("NetUsers")]
        public string int_id_usuario_id { get; set; }
        public ApplicationUser NetUsers { get; set; }
        [ForeignKey("tbzonas")]
        public System.Nullable<int> int_id_zona { get; set; }
        public Zonas tbzonas { get; set; }
        public int int_duracion { get; set; }
        public DateTime dtm_horaInicio { get; set; }
        public DateTime dtm_horaFin { get; set; }
        public Double flt_importe { get; set; }
        public Double flt_porcentaje_comision { get; set; }
        public Double flt_monto_porcentaje { get; set; }
        public Double flt_total_con_comision { get; set; }
        public Double flt_descuentos { get; set; }
        public Double flt_saldo_anterior { get; set; }
        public Double flt_saldo_fin { get; set; }
      
        public string str_observaciones { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }

    }
    public class DetalleMovimientosJoin
    {
        public int id { get; set; }
      
        public int int_idmovimiento { get; set; }
        public string str_placas { get; set; }
        public string str_usuario { get; set; }
        public string str_observaciones { get; set; }
        public int int_duracion { get; set; }
        public DateTime dt_hora_inicio { get; set; }
        public DateTime dt_hora_fin { get; set; }
        public Double ftl_importe { get; set; }
        public Double flt_descuentos { get; set; }

    }

    public class DetalleIngresos
    {
        public int id { get; set; }

        public int int_idmovimiento { get; set; }
        public string str_so { get; set; }
        public Double ftl_importe { get; set; }

        public string str_observaciones { get; set; }
        public DateTime dt_hora_inicio { get; set; }
        public Double flt_descuentos { get; set; }
        public int int_transacciones { get; set; }

    }

}
