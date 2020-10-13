using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Lugares
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
        public Boolean bit_status { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_latitud { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_longitud { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_lugar { get; set; }
        [ForeignKey("tbzonas")]
        public int int_id_zona_id { get; set; }
        public Zonas tbzonas { get; set; }
        [ForeignKey("tbconcesiones")]
        public int intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }


    }
}
