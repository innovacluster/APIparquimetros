using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Models
{
    public class UsuariosConcesiones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [EmailAddress]
        public string str_email { get; set; }
        public string str_pwd { get; set; }
        [ForeignKey("tbconcesiones")]
        public int int_id_concesion { get; set; }
        public Concesiones tbconcesiones { get; set; }

    }
}
