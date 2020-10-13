using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Tarjetas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [StringLength(maximumLength: 225)]
        public string created_by { get; set;}
        public DateTime created_date { get; set; }
        [StringLength(maximumLength: 225)]
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set;}
        public long dc_mano_vigencia { get; set;}
        public long dcm_mes_vigencia { get; set;}
        [StringLength(maximumLength: 50)]
        public string str_referencia_tarjeta { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_sistema_tarjeta { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_tarjeta { get; set; }
        [StringLength(maximumLength: 200)]
        public string str_titular { get; set;}
        [ForeignKey("NetUsers")]
        public string int_id_usuario_id { get; set; }
        public ApplicationUser NetUsers { get; set;}
        [ForeignKey("tbconcesiones")]
        public System.Nullable<int> intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }

    }
}
