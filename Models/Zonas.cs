using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Zonas
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]

        public int id { get; set; }
        [StringLength(maximumLength: 250)]
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        [StringLength(maximumLength: 250)]
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }
        [StringLength(maximumLength: 200)]
        public string str_descripcion { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_latitud { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_longitud { get; set; }
        public string str_color { get; set; }
        [MaxLength]
        public string str_poligono { get; set; }

        [ForeignKey("tbciudades")]
        public int int_id_ciudad_id { get; set; }
        public Ciudades tbciudades { get; set; }

        [ForeignKey("tbconcesiones")]
        public int intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }

    }

    public class ZonasConSecciones
    {
        
        public int id { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }
        public string str_descripcion { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }
        public string str_color { get; set; }
        public string str_poligono { get; set; }
        public int intSecciones { get; set; }
        public int intEspacios { get; set; }
    

    }
}
