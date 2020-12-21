using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Models.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.Core.Interfaces
{
    public interface IProductosRepository
    {
        Task<ProductoResponse> AddProduct(ProductoRequest request);
        Task<List<ProductoResponse>> GetProducts();
        Task<ProductoResponse> UpdateProducts(ProductoRequest request);
    }
}
