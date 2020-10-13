using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Secciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        public string str_seccion { get; set; }
        public string str_color { get; set; }
        public string str_poligono { get; set; }
        public Boolean bit_status { get; set; }
        [ForeignKey("tbzonas")]
        public int intidzona_id { get; set; }
        public Zonas tbzonas { get; set; }



    }
}
