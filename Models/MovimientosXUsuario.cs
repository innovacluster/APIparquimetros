using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class MovimientosXUsuario
    {
        public int No { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Concesion { get; set; }
        public double SaldoAnterior { get; set; }
        public int Tiempo { get; set; }
        public double Monto { get; set; }
        public double Comision { get; set; }
        public double Cargo { get; set; }
        public double TiempoDevuelto { get; set; }
        public double MontoDevuelto { get; set; }
        public double ComisionDevuelta { get; set; }
        public double MontoTotalDevolucion { get; set; }
        public int TiempoTotal { get; set; }
        public double MontoTotal { get; set; }
    }
}
