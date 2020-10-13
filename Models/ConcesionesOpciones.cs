using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class ConcesionesOpciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [ForeignKey("tbcatopciones")]
        public System.Nullable<int> int_id_opcion { get; set; }
        [ForeignKey("tbconcesiones")]
        public System.Nullable<int> int_id_concesion { get; set; }
        public Concesiones tbconcesiones { get; set; }
       
        public CatalogoOpciones tbcatopciones { get; set; }
    }

    public class ConcesionesOpcionesIngresar
    {
        public int[] int_id_opcion { get; set; }

    }
}
