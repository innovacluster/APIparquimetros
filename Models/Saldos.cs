using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Saldos
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
        public DateTime dtmfecha { get; set; }
        [Required]

        public double flt_monto { get; set; }
        public Double flt_monto_final { get; set; }
        [Required]
        
        public Double flt_monto_inicial { get; set; }

  
        [StringLength(maximumLength: 50)]
        public string str_forma_pago { get; set; }

        [StringLength(maximumLength: 20)]
        public string str_tipo_recarga { get; set; }
        public Double flt_porcentaje_comision { get; set; }
        public Double flt_total_con_comision { get; set; }
   
        [ForeignKey("NetUsers")]
        public string int_id_usuario_id { get; set; }
        public string int_id_usuario_trans { get; set; }

        public ApplicationUser NetUsers { get; set; }

        [ForeignKey("tbconcesiones")]
        public System.Nullable<int> intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }

    }
}
