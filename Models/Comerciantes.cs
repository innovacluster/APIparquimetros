﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Comerciantes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_apellido_ap { get; set; }
        [StringLength(maximumLength: 250)]
        public string str_apellido_mat { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_nombre { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_comerciante { get; set; }
        [StringLength(maximumLength: 50)]
        public string str_telefono { get; set; }
        [StringLength(maximumLength: 250)]
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        [StringLength(maximumLength: 250)]
        public string last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public Boolean bit_status { get; set; }

        [ForeignKey("tbconcesiones")]
        public int intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }
    }
}
