using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Controllers
{
    public class ConsultarUsuariosWeb
    {
        public string id { get; set; }
        public string strNombre { get; set; }
        public string strApellidos { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public bool bit_status { get; set; }
        public int int_id_concesion { get; set; }
       public string str_nombre_cliente { get; set; }
        public string strTipoUsuario { get; set; }

    }
}
