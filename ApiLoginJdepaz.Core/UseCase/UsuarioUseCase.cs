using ApiLoginJdepaz.Core.Domains.Login;
using ApiLoginJdepaz.Core.Domains.Usuarios;
using ApiLoginJdepaz.Core.Interfaces;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.Core.UseCase
{
    public class UsuarioUseCase : IUsuarioUseCase
    {
        private readonly IUsuariosRepository repository;
        public UsuarioUseCase(IUsuariosRepository usuariosRepository)
        {
            repository = usuariosRepository ?? throw new ArgumentNullException(nameof(usuariosRepository));
        }

        public async Task<UsuarioResponse> LoginUsuario(LoginWithPatron request) => await repository.LoginUser(request);
        public async Task<List<UsuarioResponse>> GetUsers() => await repository.GetUsers();
        public async Task<UsuarioResponse> AddUser(RegistroUsuarioRequest request) => await repository.AddUser(request);
        public async Task<UsuarioResponse> UpdateUser(ModificarUsuarioRequest request) => await repository.UpdateUser(request);
        public async Task<UsuarioResponse> DefuseUser(DesactivarUsuarioRequest request) => await repository.DefuseUser(request);
        public void sendMail(string correo) => repository.sendMail(correo);
    }
}
