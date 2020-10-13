using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class DetalleMulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        public int int_id_multa { get; set; }
        public Boolean bit_status { get; set; }
        public DateTime dtmFecha { get; set; }
        public string str_usuario { get; set; }
        public Double flt_monto { get; set; }
        public string str_comentarios { get; set; }


    }
}
