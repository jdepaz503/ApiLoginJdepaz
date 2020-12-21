using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Usuarios
{
    public class ModificarUsuarioRequest
    {
        public int UserId { get; set; }
        public string nombre_user { get; set; }
        public string telefono_user { get; set; }
        public DateTime fnac_user { get; set; }
    }
}
