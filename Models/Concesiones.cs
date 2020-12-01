using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Concesiones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [StringLength(maximumLength: 100)]
        public string str_clave { get; set; }
        public string str_razon_social{get; set;}
        public string str_domicilio { get; set; }
        public string str_nombre_cliente { get; set; }
        [StringLength(maximumLength:15)]
        [Phone]
        public string str_telefono { get; set; }
        [EmailAddress]
        public string str_email { get; set; }
        public string str_rfc { get; set; }
        public string str_notas { get; set; }
        public string str_poligono { get; set; }
        public int int_licencias { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }
        public Double dbl_costo_licencia { get; set; }
        public DateTime dtm_fecha_ingreso { get; set; }
        public DateTime dtm_fecha_activacion_licencia{ get; set; }
        public string str_tipo { get; set; }
        public int intidciudad { get; set; }
        public int intidciudad_cat{ get; set; }
        public string str_ciudad { get; set; }
        public Boolean bit_status { get; set; }
    }


    public class ConcesionesConUsers {

        public int id { get; set; }
        [StringLength(maximumLength: 100)]
        public string str_clave { get; set; }
        public string str_razon_social { get; set; }
        public string str_domicilio { get; set; }
        public string str_ciudad { get; set; }
        public string str_nombre_cliente { get; set; }
        [StringLength(maximumLength: 15)]
        [Phone]
        public string str_telefono { get; set; }
        [EmailAddress]
        public string str_email { get; set; }
        public string str_rfc { get; set; }
        public string str_notas { get; set; }
        public string str_poligono { get; set; }
        public int int_licencias { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }
        public Double dbl_costo_licencia { get; set; }
        public DateTime dtm_fecha_ingreso { get; set; }
        public DateTime dtm_fecha_activacion_licencia { get; set; }
        public string str_tipo { get; set; }
        public int intidciudad { get; set; }
        public Boolean bit_status { get; set; }
        public IList<ApplicationUser> cuentas {get; set;}
        public object opciones { get; set; }
        public IList<Tarifas> tarifas { get; set; }

    }


    public class ConcesionesConUsersIngresar
    {

        public int id { get; set; }
        [StringLength(maximumLength: 100)]
        public string str_clave { get; set; }
        public string str_razon_social { get; set; }
        public string str_domicilio { get; set; }
        public string str_nombre_cliente { get; set; }
        [StringLength(maximumLength: 15)]
        [Phone]
        public string str_telefono { get; set; }
        [EmailAddress]
        public string str_email { get; set; }
        public string str_rfc { get; set; }
        public string str_notas { get; set; }
        public string str_poligono { get; set; }
        public int int_licencias { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }
        public Double dbl_costo_licencia { get; set; }
        public DateTime dtm_fecha_ingreso { get; set; }
        public DateTime dtm_fecha_activacion_licencia { get; set; }
        public string str_tipo { get; set; }
        public int intidciudad { get; set; }
        public int intidciudad_cat { get; set; }
        public string str_ciudad { get; set; }
        public Boolean bit_status { get; set; }
        public IList<ApplicationUser> cuentas { get; set; }
        public object opciones { get; set; }
        //public IList<ConcesionesOpcionesIngresar> opciones { get; set; }
        public IList<Tarifas> tarifas { get; set; }

    }

    public class AgregarLicencias {
        public IList<ApplicationUser> cuentas { get; set; }

    }
}
