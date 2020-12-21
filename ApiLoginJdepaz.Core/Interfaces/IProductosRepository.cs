using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Models.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.Core.Interfaces
{
    public interface IProductosRepository
    {
        Task<ProductoResponse> AddProduct(RegistrarProductoRequest request);
        Task<List<ProductoResponse>> GetProducts();
    }
}
