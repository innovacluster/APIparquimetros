using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public string Id { get; set; }

        public IList permisos { get; set; }

        public int intIdCiudad { get; set; }

        public string strCiudad { get; set; }

        public string strLatitud { get; set; }
        public string strLongitud { get; set; }

        public System.Nullable<int> intIdConcesion { get; set; }
        public string strConcesion { get; set; }
        public string strRuta { get; set; }
        public string strNombreUsuario { get; set; }





    }
}
