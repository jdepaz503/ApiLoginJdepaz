using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Usuarios
{
    public class RegistroUsuarioRequest
    {
        public string nombre_user { get; set; }
        public string username { get; set; }
        public string email_user { get; set; }
        public string pass_user { get; set; }
        public string telefono_user { get; set; }
        public DateTime fnac_user { get; set; }
    }
}
