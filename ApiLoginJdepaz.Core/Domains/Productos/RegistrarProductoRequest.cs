using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Productos
{
    public class RegistrarProductoRequest
    {
        public string SKU { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public float precio { get; set; }
        public string descripcion { get; set; }
        public string imagen { get; set; }
    }
}
