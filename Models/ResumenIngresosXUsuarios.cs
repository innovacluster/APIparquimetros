using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class ResumenIngresosXUsuarios
    {

        public string Usuario { get; set; }
        public double SaldoMesAnterior { get; set; }
        public double Saldo { get; set; }
        public double Comision { get; set; }
        public double TotalCobrado { get; set; }
        public double SaldoDelMes { get; set; }

        public double Cargos { get; set; }
        public double ComisionMov { get; set; }
        public double ComisionTotal { get; set; }
        public double SaldoFinal { get; set; }
    }
}
