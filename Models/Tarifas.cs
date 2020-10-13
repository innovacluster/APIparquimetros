using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Tarifas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        //       public string str_tipo { get; set; }
        public Double flt_tarifa_min { get; set;}
        public int int_tiempo_minimo { get; set; }
        public Double flt_tarifa_max { get; set; }
        public int int_tiempo_maximo { get; set; }
        public Double flt_tarifa_intervalo { get; set; }
        public int int_intervalo_minutos { get; set; }
        public Boolean bool_cobro_fraccion { get; set; }

        [ForeignKey("tbconcesiones")]
        public int intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }
    }
}
