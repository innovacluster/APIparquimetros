using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Permisos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [ForeignKey("tbtiposusuarios")]
        public int id_rol { get; set; }
        public TiposUsuarios tbtiposusuarios { get; set; }

        [ForeignKey("tbopciones")]
        public int id_opcion { get; set; }
        public Opciones tbopciones { get; set; }

    }
}
