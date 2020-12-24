using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Usuarios
{
    public class CambiarClaveRequest
    {
        public string token { get; set; }
        public string newPassword { get; set; }
    }
}
