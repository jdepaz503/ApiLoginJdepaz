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
using ApiLoginJdepaz.Core.Domains.Request;

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

        public async Task<List<ProductoResponse>> GetProducts(DataListRequest request)
        {
            List<ProductoResponse> response = new List<ProductoResponse>();
            try
            {
                //IList<TblProductos> usr = await db.Productos.FromSqlRaw("SP_ObtenerProductos").ToListAsync();
                IQueryable<TblProductos> query = db.Productos;
                if (request.Filtros != null)
                {
                    foreach (OpcionBusqueda opcionBusqueda in request.Filtros)
                    {
                        if (opcionBusqueda.Clave == "SKU")
                            query = query.Where(f => f.SKU == opcionBusqueda.Valor);
                        else if (opcionBusqueda.Clave == "Nombre")
                        {
                            switch (opcionBusqueda.PatronOperador)
                            {
                                case PatronBusqueda.Equals:
                                    query = query.Where(f => f.nombre == opcionBusqueda.Valor);
                                    break;
                                case PatronBusqueda.StartsWith:
                                    query = query.Where(f => f.nombre.StartsWith(opcionBusqueda.Valor));
                                    break;
                                case PatronBusqueda.EndsWith:
                                    query = query.Where(f => f.nombre.EndsWith(opcionBusqueda.Valor));
                                    break;
                                case PatronBusqueda.Contains:
                                    query = query.Where(f => f.nombre.Contains(opcionBusqueda.Valor));
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (opcionBusqueda.Clave == "Precio")
                        {
                            if (opcionBusqueda.Operador == "<=")
                                query = query.Where(f => f.precio <= Convert.ToDecimal(opcionBusqueda.Valor));
                            else if (opcionBusqueda.Operador == "<")
                                query = query.Where(f => f.precio < Convert.ToDecimal(opcionBusqueda.Valor));
                            if (opcionBusqueda.Operador == ">=")
                                query = query.Where(f => f.precio >= Convert.ToDecimal(opcionBusqueda.Valor));
                            else if (opcionBusqueda.Operador == ">")
                                query = query.Where(f => f.precio > Convert.ToDecimal(opcionBusqueda.Valor));
                        }
                    }
                }
                IList<TblProductos> usr = await query.Skip((request.Pagina.Value - 1) * request.ElementosPorPagina.Value).Take(request.ElementosPorPagina.Value).ToListAsync();
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

        public async Task<EliminarUsuarioResponse> DeleteProduct(string sku)
        {
            EliminarUsuarioResponse response = new EliminarUsuarioResponse();
            var paramSku = new SqlParameter("@SKU", sku);
            try
            {
                IList<TblProductos> usr = await db.Productos.FromSqlRaw(
                    "SP_EliminarProductos @SKU",paramSku).ToListAsync();
                if (usr != null && usr.Count != 0)
                {
                    response.result = usr.ToString();
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
