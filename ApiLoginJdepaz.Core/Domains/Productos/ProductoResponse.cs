using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Productos
{
    public class ProductoResponse
    {
        public string SKU { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public string descripcion { get; set; }
        public string imagen { get; set; }
    }
}
