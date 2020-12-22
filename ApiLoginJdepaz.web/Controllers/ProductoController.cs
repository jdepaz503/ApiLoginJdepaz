using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Domains.Request;
using ApiLoginJdepaz.Core.Models.Generic;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ProductoResponse> AddProduct([FromBody] ProductoRequest request)
        {
            return await useCase.AddProduct(request);
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/ObtenerProductos")]        
        public async Task<List<ProductoResponse>> GetProducts([FromBody] DataListRequest request)
        {
            return await useCase.GetProducts(request);
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/ActualizarProducto")]
        public async Task<ProductoResponse> UpdateProducts([FromBody] ProductoRequest request)
        {
            return await useCase.UpdateProducts(request);
        }

        [HttpDelete]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/EliminarProducto/{sku}")]
        public async Task<EliminarUsuarioResponse> DeleteProduct ([FromRoute] string sku)
        {
            return await useCase.DeleteProduct(sku);
        }
    }
}
