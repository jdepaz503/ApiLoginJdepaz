using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Request
{
    public class DataListRequest
    {
        public IList<OpcionBusqueda> Filtros { get; set; }
        public int? Pagina { get; set; }
        public int? ElementosPorPagina { get; set; }
    }
}
