using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }
        public string strNombre { get; set; }
        public string strApellidos { get; set; }
        [Required]
        public override string Email { get; set; }

        public string str_rfc { get; set; }
        public string str_razon_social { get; set; }
        public string str_direccion { get; set; }
        public string str_cp { get; set; }
        public Double dbl_saldo_actual { get; set; }
        public Double dbl_saldo_anterior{ get; set; }
        
        [ForeignKey("tbconcesiones")]
        public System.Nullable<int> intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }
        [ForeignKey("tbtiposusuarios")]
        public int intIdTipoUsuario { get; set; }
        public TiposUsuarios tbtiposusuarios { get; set; }
        [ForeignKey("tbciudades")]
        public System.Nullable<int> intidciudad { get; set; }
        public Ciudades tbciudades { get; set; }

        [ForeignKey("tbzonas")]
        public System.Nullable<int> intidzona { get; set;  }
        public Zonas tbzonas { get; set; }


 




    }


  
}
