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
    /*
        Controller Producto: Es el encargado gestionar todos los datos e interacción con la tabla Productos 
        Para poder tener acceso al consumo de los métodos de este Endpoint es necesario proporcionar el token obtenido de un login valido 
        para poder hacer Authorize, de lo contrario no se podrán consumir. 

        Endpoints:
        o	AddProduct: Es el encargado de registrar los productos en la base de datos.
        o	GetProducts: Es el encargado de mostrar todos los productos reflejados, el listado que retorna tiene la opción de poder ser filtrado y paginado. 
        o	UpdateProducts: Es el encargado de actualizar los campos de la tabla producto mediante el SKU del producto
        o	DeleteProduct: Es el encargado de realizar un borrado físico mediante el SKU del producto

        

    */
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
