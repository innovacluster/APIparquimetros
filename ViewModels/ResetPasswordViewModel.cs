using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string strCorreo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener almenos {1} caracter.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "La contraseña y la confirmacion deben coincidir")]
        public string ConfirmPassword { get; set; }

        public string recToken { get; set; }
    }
}
