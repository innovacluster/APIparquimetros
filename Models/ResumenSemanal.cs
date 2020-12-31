using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class ResumenSemanal
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
        public int int_semana { get; set; }
        public int int_anio { get; set; }
        public int int_semana_ant { get; set; }
        public int int_sem_ios { get; set; }
        public int int_sem_ant_ios { get; set; }
        public Double int_sem_por_ios { get; set; }
        public int int_sem_autos_ios { get; set; }
        public int int_sem_autos_ant_ios { get; set; }
        public Double int_sem_autos_por_ios { get; set; }
        public Double dec_sem_ios { get; set; }
        public Double dec_sem_ant_ios { get; set; }
        public Double dec_sem_por_ios { get; set; }
        public int int_sem_andriod { get; set; }
        public int int_sem_ant_andriod { get; set; }
        public Double int_sem_por_andriod { get; set; }
        public int int_sem_autos_andriod { get; set; }
        public int int_sem_autos_ant_andriod { get; set; }
        public Double int_sem_autos_por_andriod { get; set; }
        public int int_sem_total_autos { get; set; }
        public Double dec_sem_andriod { get; set; }
        public Double dec_sem_ant_andriod { get; set; }
        public Double dec_sem_por_andriod { get; set; }
        public int int_sem_total { get; set; }
        public int int_sem_total_ant { get; set; }
        public Double int_sem_por_ant { get; set; }
        public Double dec_sem_total { get; set; }
        public Double dec_sem_total_ant { get; set; }
        public Double dec_sem_por_total { get; set; }


    }


    public class ResumenSemanalAct
    {
        
        public int id { get; set; }
        public int int_id_consecion { get; set; }
        public DateTime dtm_fecha_inicio { get; set; }
        public DateTime dtm_fecha_fin { get; set; }
        public int int_semana { get; set; }
        public int int_anio { get; set; }
        public int int_semana_ant { get; set; }
        public int int_sem_ios { get; set; }
        public int int_sem_ant_ios { get; set; }
        public int int_sem_por_ios { get; set; }
        public int int_sem_autos_ios { get; set; }
        public int int_sem_autos_ant_ios { get; set; }
        public Double int_sem_autos_por_ios { get; set; }
        public Double dec_sem_ios { get; set; }
        public Double dec_sem_ant_ios { get; set; }
        public Double dec_sem_por_ios { get; set; }
        public int int_sem_andriod { get; set; }
        public int int_sem_ant_andriod { get; set; }
        public int int_sem_por_andriod { get; set; }
        public int int_sem_autos_andriod { get; set; }
        public int int_sem_autos_ant_andriod { get; set; }
        public Double int_sem_autos_por_andriod { get; set; }
        public int int_sem_total_autos { get; set; }
        public Double dec_sem_andriod { get; set; }
        public Double dec_sem_ant_andriod { get; set; }
        public Double dec_sem_por_andriod { get; set; }
        public int int_sem_total { get; set; }
        public int int_sem_total_ant { get; set; }
        public int int_sem_por_ant { get; set; }
        public Double dec_sem_total { get; set; }
        public Double dec_sem_total_ant { get; set; }
        public Double dec_sem_por_total { get; set; }


    }
}
