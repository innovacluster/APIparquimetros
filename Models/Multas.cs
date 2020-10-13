using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Multas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }

        [StringLength(maximumLength: 225)]
        public string created_by { get; set; }

        public DateTime created_date { get; set; }

        [StringLength(maximumLength: 225)]
        public string last_modified_by { get; set; }

        public DateTime last_modified_date { get; set; }
        [Required]
        public Boolean bit_status { get; set; }
        [Required]
        public DateTime dtm_fecha { get; set; }
        [Required]
        public Double flt_monto { get; set; }

        [StringLength(maximumLength: 200)]
        public string str_motivo { get; set; }
        public string str_folio_multa { get; set; }
        public string str_placa { get; set; }
        public string str_Estado { get; set; }
        public string str_marca { get; set; }
        public string str_modelo { get; set; }
        public string str_color { get; set; }
        public string str_ubicacion { get; set; }
        public string str_fundamento { get; set; }
        public string str_articulo { get; set; }
        public string str_categoria { get; set; }
        public string str_clave { get; set; }

        public string str_tipo_pago { get; set; }
        public string str_documento_garantia { get; set; }
        public string str_tipo_multa { get; set; }
        public string str_clave_candado { get; set; }
        public DateTime dtm_fecha_multafisica { get; set; }
        public string str_no_parquimetro { get; set; }

        [ForeignKey("NetUsers")]
        public string str_id_agente_id { get; set; }
    
        [ForeignKey("tbmovimientos")]
        public int? int_id_movimiento_id { get; set; }

     
        //[ForeignKey("tbsaldo")]
        //public int? int_id_saldo_id { get; set; }
   
        //[ForeignKey("tbvehiculos")]
        public int? int_id_vehiculo_id { get; set; }

        public ApplicationUser NetUsers { get; set; }
        public Movimientos tbmovimientos { get; set; }
        //public Saldos tbsaldo { get; set; }
        public Vehiculos tbvehiculos { get; set; }

        [ForeignKey("tbconcesiones")]
        public int? intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }



    }
}
