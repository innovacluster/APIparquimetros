using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Espacios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        public string str_clave { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }
        //public string str_direccion { get; set; }
        public string str_marcador { get; set; }
        public string str_color { get; set; }
        public Boolean bit_status { get; set; }
        public Boolean bit_ocupado { get; set; }


        [ForeignKey("tbzonas")]
        public int id_zona { get; set; }
        public Zonas tbzonas { get; set; }


    }
}
