using AutoMapper;
using ApiLoginJdepaz.Core.Domains.Login;
using ApiLoginJdepaz.Core.Domains.Usuarios;
using ApiLoginJdepaz.Core.Interfaces;
using ApiLoginJdepaz.Infraestructure.Models.DataContext;
using ApiLoginJdepaz.Infraestructure.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.Infraestructure.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly DataContext db;
        private readonly IMapper map;
        public UsuariosRepository(DataContext _db, IMapper mapper)
        {
            db = _db ?? throw new ArgumentNullException(nameof(_db));
            map = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UsuarioResponse> LoginUser(LoginWithPatron request)
        {
            UsuarioResponse response = new UsuarioResponse();
            var paramUserName = new SqlParameter("@username", request.username);
            var paramPassword = new SqlParameter("@pass_user", request.pass_user);
            var paramPatron = new SqlParameter("@Patron", request.Patron);
            try
            {
                IList<TblUsuarios> usr = await db.Usuarios.FromSqlRaw("SP_ValidarUsuario @username, @pass_user, @Patron", paramUserName, paramPassword, paramPatron).ToListAsync();
                if (usr != null && usr.Count == 1)
                    response = map.Map<UsuarioResponse>(usr.FirstOrDefault());
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }
           
        }

        public async Task<List<UsuarioResponse>> GetUsers()
        {
            List<UsuarioResponse> response = new List<UsuarioResponse>();
            try
            {
                IList<TblUsuarios> usr = await db.Usuarios.FromSqlRaw("SP_ObtenerUsuarios").ToListAsync();
                if (usr != null && usr.Count != 0)
                    response = map.Map<List<UsuarioResponse>>(usr.ToList());
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<UsuarioResponse> AddUser(RegistroUsuarioRequest request)
        {
            UsuarioResponse response = new UsuarioResponse();
            var paramNombreUser = new SqlParameter("@nombre_user", request.nombre_user);
            var paramUserName = new SqlParameter("@username", request.username);
            var paramEmailUser = new SqlParameter("@email_user", request.email_user);
            var paramPassUser = new SqlParameter("@pass_user", request.pass_user);
            var paramTelUser = new SqlParameter("@telefono_user", request.telefono_user);
            var paramFnacUser= new SqlParameter("@fnac_user", request.fnac_user);
            var paramPatron = new SqlParameter("@Patron", "V4Kc10ne$");
            try
            {
                IList<TblUsuarios> usr = await db.Usuarios.FromSqlRaw(
                    "SP_AgregarUsuario @nombre_user, @username, @email_user,@pass_user,@telefono_user,@fnac_user,@Patron ",
                    paramNombreUser, paramUserName, paramEmailUser, paramPassUser, paramTelUser, paramFnacUser, paramPatron).ToListAsync();
                if (usr != null && usr.Count == 1)
                {
                    response = map.Map<UsuarioResponse>(usr.FirstOrDefault());
                }
                    
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }

        }

        public async Task<UsuarioResponse> UpdateUser(ModificarUsuarioRequest request)
        {
            UsuarioResponse response = new UsuarioResponse();
            var paramUserId= new SqlParameter("@UserId", request.UserId);
            var paramNombreUser = new SqlParameter("@nombre_user", request.nombre_user);
            var paramTelUser = new SqlParameter("@telefono_user", request.telefono_user);
            var paramFnacUser = new SqlParameter("@fnac_user", request.fnac_user);
            var paramPatron = new SqlParameter("@Patron", "V4Kc10ne$");
            try
            {
                IList<TblUsuarios> usr = await db.Usuarios.FromSqlRaw(
                    "SP_ModificarUsuario @UserId,@nombre_user,@telefono_user,@fnac_user,@Patron ",
                    paramUserId,paramNombreUser, paramTelUser, paramFnacUser, paramPatron).ToListAsync();
                if (usr != null && usr.Count == 1)
                {
                    response = map.Map<UsuarioResponse>(usr.FirstOrDefault());
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }

        }

        public async Task<UsuarioResponse> DefuseUser(DesactivarUsuarioRequest request)
        {
            UsuarioResponse response = new UsuarioResponse();
            var paramUserName = new SqlParameter("@username", request.username);
            var paramPatron = new SqlParameter("@Patron", "V4Kc10ne$");
            try
            {
                IList<TblUsuarios> usr = await db.Usuarios.FromSqlRaw(
                    "SP_desactivarUsuario @username,@Patron ", paramUserName,paramPatron).ToListAsync();
                if (usr != null && usr.Count == 1)
                {
                    response = map.Map<UsuarioResponse>(usr.FirstOrDefault());
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
