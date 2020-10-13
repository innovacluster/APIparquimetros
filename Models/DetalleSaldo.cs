using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class DetalleSaldo
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        public DateTime dtm_fecha { get; set; }
        public Double flt_monto { get; set; }
        public string str_tipo { get; set; }
        public string str_forma_pago { get; set; }
        [ForeignKey("tbsaldo")]
        public int int_id_saldo { get; set; }
        public Saldos tbsaldo { get; set; }
        [ForeignKey("NetUsers")]
        public string int_id_usuario { get; set; }
        public ApplicationUser NetUsers { get; set; }
        

    }
}
