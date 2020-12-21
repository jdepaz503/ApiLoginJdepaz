using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Interfaces;
using ApiLoginJdepaz.Core.Models.Generic;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.Core.UseCase
{
    public class ProductoUseCase : IProductoUseCase
    {
        private readonly IProductosRepository repository;

        public ProductoUseCase(IProductosRepository productosRepository)
        {
            repository = productosRepository ?? throw new ArgumentNullException(nameof(productosRepository));
        }

        public async Task<ProductoResponse> AddProduct(ProductoRequest request) => await repository.AddProduct(request);
        public async Task<List<ProductoResponse>> GetProducts() => await repository.GetProducts();
        public async Task<ProductoResponse> UpdateProducts(ProductoRequest request) => await repository.UpdateProducts(request);

    }
}
