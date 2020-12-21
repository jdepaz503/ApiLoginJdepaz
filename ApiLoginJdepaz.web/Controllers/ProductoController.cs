using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Models.Generic;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoUseCase useCase;

        public ProductoController(IProductoUseCase productoUseCase)
        {
            useCase = productoUseCase ?? throw new ArgumentNullException(nameof(productoUseCase));
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/AgregarProducto")]
        public async Task<ProductoResponse> AddProduct([FromBody] RegistrarProductoRequest request)
        {
            return await useCase.AddProduct(request);
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/ObtenerProductos")]
        public async Task<List<ProductoResponse>> GetProducts()
        {
            return await useCase.GetProducts();
        }

    }
}
