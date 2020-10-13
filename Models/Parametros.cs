using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Parametros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        public Boolean bolUsarNomenclaturaCajones { get; set; }
        public int intTimepoAviso { get; set; }
        public Double flt_Tarifa_minima { get; set; }
        public Double flt_intervalo_tarifa { get; set; }
        public int int_intervalo_estacionamiento { get; set; }
        public int int_minimo_estacionamiento { get; set; }
        public int int_maximo_estacionamiento { get; set; }
        [ForeignKey("tbconcesiones")]
        public System.Nullable<int> intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }

    }
}
