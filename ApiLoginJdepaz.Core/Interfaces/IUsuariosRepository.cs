﻿using ApiLoginJdepaz.Core.Domains.Login;
using ApiLoginJdepaz.Core.Domains.Usuarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.Core.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<UsuarioResponse> LoginUser(LoginWithPatron request);
        Task<List<UsuarioResponse>> GetUsers(); 
        Task<UsuarioResponse> AddUser(RegistroUsuarioRequest request);
        Task<UsuarioResponse> UpdateUser(ModificarUsuarioRequest request);
        Task<UsuarioResponse> DefuseUser(DesactivarUsuarioRequest request);
        string sendMail(string user, string correo);
        Task<EmailPasswordResetResponse> passwordReset(ResetPasswordRequest request);
        Task<string> changePassword(CambiarClaveRequest request);
    }
}
