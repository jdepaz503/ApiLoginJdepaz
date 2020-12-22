using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Usuarios
{
    public class ResetPasswordRequest
    {
        public string user { get; set; }
        public string correo { get; set; }
    }
}
