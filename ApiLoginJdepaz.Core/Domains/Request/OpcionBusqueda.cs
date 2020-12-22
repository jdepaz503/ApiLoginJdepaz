using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Request
{
    public class OpcionBusqueda
    {
        public string Clave { get; set; }
        public string Operador { get; set; }
        public PatronBusqueda PatronOperador { get; set; }
        public string Valor { get; set; }
    }

    public enum PatronBusqueda
    {
        Equals,
        StartsWith,
        EndsWith,
        Contains
    }
}
