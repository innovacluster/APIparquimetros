using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class ResumenMensual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [ForeignKey("tbconcesiones")]
        public int int_id_consecion { get; set; }
        public Concesiones tbconcesiones { get; set; }
        public DateTime dtm_fecha_inicio { get; set; }
        public DateTime dtm_fecha_fin { get; set; }
        public string str_mes { get; set; }
        public int int_anio { get; set; }
        public DateTime dtm_mes_anterior { get; set; }
        public int int_mes_ios { get; set; }
        public int int_mes_ant_ios { get; set; }
        public Double int_mes_por_ios { get; set; }
        public int int_mes_autos_ios { get; set; }
        public int int_mes_autos_ant_ios { get; set; }
        public Double int_mes_autos_por_ios { get; set; }
        public Double dec_mes_ios { get; set; }
        public Double dec_mes_ant_ios { get; set; }
        public Double dec_mes_por_ios { get; set; }
        public int int_mes_andriod { get; set; }
        public int int_mes_ant_andriod { get; set; }
        public Double int_mes_por_andriod { get; set; }
        public int int_mes_autos_andriod { get; set; }
        public int int_mes_autos_ant_andriod { get; set; }
        public Double int_mes_autos_por_andriod { get; set; }
        public int int_mes_total_autos { get; set; }
        public Double dec_mes_andriod { get; set; }
        public Double dec_mes_ant_andriod { get; set; }
        public Double dec_mes_por_andriod { get; set; }
        public int int_mes_total { get; set; }
        public int int_mes_total_ant { get; set; }
        public Double int_mes_por_total { get; set; }

        public Double dec_mes_total { get; set; }
        public Double dec_mes_total_ant { get; set; }
        public Double dec_mes_por_total { get; set; }

    }
}
