using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    
        public string Password { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }
        public int intidconcesion_id { get; set; }
        public int intidciudad { get; set; }
        public string Rol { set; get; }
        public string strNombre { get; set; }
        public string strApellidos{ get; set; }
        public string PhoneNumber { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string str_rfc { get; set; }
        public string str_razon_social { get; set; }
        public string str_direccion { get; set; }
        public string str_cp { get; set; }



    }
}
