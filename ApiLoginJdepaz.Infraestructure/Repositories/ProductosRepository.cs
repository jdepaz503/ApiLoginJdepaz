using AutoMapper;
using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Models.Generic;
using ApiLoginJdepaz.Infraestructure.Models.DataContext;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiLoginJdepaz.Core.Interfaces;
using ApiLoginJdepaz.Infraestructure.Models.Entities;
using Microsoft.Data.SqlClient;

namespace ApiLoginJdepaz.Infraestructure.Repositories
{
    public class ProductosRepository : IProductosRepository
    {
        private readonly DataContext db;
        private readonly IMapper map;
        public ProductosRepository(DataContext _db, IMapper mapper)
        {
            db = _db ?? throw new ArgumentNullException(nameof(_db));
            map = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductoResponse> AddProduct(ProductoRequest request)
        {
            ProductoResponse response = new ProductoResponse();
            var paramSku            = new SqlParameter("@SKU",          request.SKU);
            var paramNombre         = new SqlParameter("@nombre",       request.nombre);
            var paramCantidad       = new SqlParameter("@cantidad",     request.cantidad);
            var paramPrecio         = new SqlParameter("@precio",       request.precio);
            var paramDescripcion    = new SqlParameter("@descripcion",  request.descripcion);
            var paramimagen         = new SqlParameter("@imagen",       request.imagen);
            try
            {
                IList<TblProductos> usr = await db.Productos.FromSqlRaw(
                    "SP_InsertarProductos @SKU,@nombre,@cantidad,@precio,@descripcion,@imagen",
                    paramSku, paramNombre, paramCantidad, paramPrecio, paramDescripcion, paramimagen).ToListAsync();
                if (usr != null && usr.Count != 0)
                { 
                    response = map.Map<ProductoResponse>(usr.FirstOrDefault());
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<List<ProductoResponse>> GetProducts()
        {
            List<ProductoResponse> response = new List<ProductoResponse>();
            try
            {
                IList<TblProductos> usr = await db.Productos.FromSqlRaw("SP_ObtenerProductos").ToListAsync();
                if (usr != null && usr.Count != 0)
                    response = map.Map<List<ProductoResponse>>(usr.ToList());
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<ProductoResponse> UpdateProducts(ProductoRequest request)
        {
            ProductoResponse response = new ProductoResponse();
            var paramSku = new SqlParameter("@SKU", request.SKU);
            var paramNombre = new SqlParameter("@nombre", request.nombre);
            var paramCantidad = new SqlParameter("@cantidad", request.cantidad);
            var paramPrecio = new SqlParameter("@precio", request.precio);
            var paramDescripcion = new SqlParameter("@descripcion", request.descripcion);
            var paramimagen = new SqlParameter("@imagen", request.imagen);
            try
            {
                IList<TblProductos> usr = await db.Productos.FromSqlRaw(
                    "SP_ActulizarProductos @SKU,@nombre,@cantidad,@precio,@descripcion,@imagen",
                    paramSku, paramNombre, paramCantidad, paramPrecio, paramDescripcion, paramimagen).ToListAsync();
                if (usr != null && usr.Count != 0)
                {
                    response = map.Map<ProductoResponse>(usr.FirstOrDefault());
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
