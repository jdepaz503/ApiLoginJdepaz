using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Models.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.Core.UseCase.Interfaces
{
    public interface IProductoUseCase
    {
        Task<ProductoResponse> AddProduct(ProductoRequest request);
        Task<List<ProductoResponse>> GetProducts();

        Task<ProductoResponse> UpdateProducts(ProductoRequest request);
    }
}
