﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Ciudades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [StringLength(maximumLength: 250)]
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        [StringLength(maximumLength: 250)]
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }
        [StringLength(maximumLength: 200)]
        public string str_ciudad { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_latitud { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_longitud { get; set; }
        public string str_desc_ciudad { get; set; }

        [ForeignKey("tbcatciudades")]
        public System.Nullable<int> int_id_ciudad { get; set; }
        public CatCiudades tbcatciudades { get; set; }



    }
}
