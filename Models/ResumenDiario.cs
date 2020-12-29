using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class ResumenDiario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [ForeignKey("tbconcesiones")]
        public int int_id_consecion { get; set; }
        public Concesiones tbconcesiones { get; set; }
        public DateTime dtm_fecha { get; set; }
        public int int_dia { get; set; }
        public int int_mes { get; set; }
        public int int_anio { get; set; }
        public string str_dia_semama { get; set; }
        public DateTime dtm_dia_anterior { get; set; }
        public string str_dia_sem_ant { get; set; }
        public int int_ios { get; set; }
        public int int_ant_ios { get; set; }
        public int int_por_ios { get; set; }
        public int int_autos_ios { get; set;  }
        public int int_autos_ant_ios { get; set; }
        public int int_autos_por_ios { get; set; }
        public Double dec_ios { get; set; }
        public Double dec_ant_ios { get; set; }
        public Double dec_por_ios { get; set; }
        public int int_andriod { get; set; }
        public int int_ant_andriod { get; set; }
        public Double int_por_andriod { get; set; }
        public int int_autos_andriod { get; set; }
        public int int_autos_ant_andriod { get; set; }
        public int int_autos_por_andriod { get; set; }
        public int int_total_autos { get; set; }
        public Double dec_andriod { get; set; }
        public Double dec_ant_andriod { get; set; }
        public Double dec_por_andriod { get; set; }
        public int int_total { get; set; }
        public int int_total_ant { get; set; }
        public int int_por_ant_total {get; set;}
        public Double dec_total { get; set; }
        public Double dec_total_ant { get; set; }
        public Double dec_por_ant_total { get; set; }
    }

   
}
