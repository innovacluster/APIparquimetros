using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class RendicionCuentas
    {
        public int No { get; set; }
        public int IntIdMovmiento { get; set; }
        public string StrTipo { get; set; }
        public string Email { get; set; }
        public string StrSo { get; set; }
        public DateTime DtmFecha { get; set; }
        public double Monto { get; set; }
        public string StrPlaca { get; set; }
    }
}
