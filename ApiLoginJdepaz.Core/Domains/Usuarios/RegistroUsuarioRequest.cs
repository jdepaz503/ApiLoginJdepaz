using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Usuarios
{
    public class RegistroUsuarioRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string nombre_user { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 4)]
        public string username { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 5)]
        public string email_user { get; set; }
        [Required]
        public string pass_user { get; set; }
        [Required]
        [Range(0, Int64.MaxValue, ErrorMessage = "Por favor especifique solo números.")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "Debe especificar como mínimo 8 dígitos.")]
        public string telefono_user { get; set; }
        [Required]
        public DateTime fnac_user { get; set; }
    }
}
