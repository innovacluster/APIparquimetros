using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Vehiculos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [StringLength(maximumLength: 225)]
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        [StringLength(maximumLength: 255)]
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_color { get; set; }
        [StringLength(maximumLength: 200)]
        public string str_modelo { get; set; }
        [StringLength(maximumLength: 20)]

        public string str_marca { get; set; }
        [Required]
        public string str_placas { get; set; }
        [ForeignKey("NetUsers")]
        public string int_id_usuario_id { get; set; }
        // //esta linea se comento para que en las consultas no llevara la lista info del usuario revisar si no afecta la llave foranea int_id_usuario_id
        //public ApplicationUser NetUsers { get; set; }

        //[ForeignKey("tbconcesiones")]
        //public int intidconcesion_id { get; set; }

        //public Concesiones tbconcesiones { get; set; }
    }
}
