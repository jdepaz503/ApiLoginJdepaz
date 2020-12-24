using ApiLoginJdepaz.Core.Domains.Usuarios;
using ApiLoginJdepaz.Core.Models.Generic;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.web.Controllers
{
    /*
        Controller Usuario: Encargado de la administración de la tabla Usuarios. 
        
        1-AddUser: Es el encargado de registrar nuevos usuarios para poder acceder a este endpoint es necesario hacerlo mediante un token el cual se obtienen al hacer un login valido. Se puede usar el usuario jdepaz con contraseña 12345 (siguiendo el orden de los migrations)
        2-UpdateUser: Es el encargado de modificar información de usuario, únicamente de los campos nombre, telefono y fecha de nacimiento. 
        3-DefuseUser: Se utiliza para hacer un borrado lógico de un usuario, cambiando a 0 su estado mediante el username. 
        4-GetUsers: Es el encargado de listar todos los usuarios registrados, no requiere un JWT debido a que solo muestra la información de clientes exceptuando la contraseña. 
        5-passwordReset: Es el encargado de iniciar proceso de reestablecer contraseña, únicamente solicita correo electrónico, él valida que el correo electrónico exista en la base de datos, en caso de existir, manda un correo electrónico con el nombre de usuario, un párrafo indicando el proceso a seguir (dar clic a un enlace) y que sólo dispone de 15 minutos. En este caso para retomar el flujo es necesario especificar un enlace de un front encargado de recibir el token que el endpoint recibe y pedirle al usuario la nueva contraseña, para llamar al siguiente endoint a continuación. 
        6-changePassword:  Es el encargado de recibir el token y nueva contraseña para poder cambiar la clave de usuario que viene inmerso en los claims del token. 

    */
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioUseCase useCase;

        public UsuarioController(IUsuarioUseCase usuarioUseCase)
        {
            useCase = usuarioUseCase ?? throw new ArgumentNullException(nameof(usuarioUseCase));
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/AgregarUsuario")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddUser([FromBody] RegistroUsuarioRequest request)
        {
            GenericResponse<RegistroUsuarioResponse> response;
            try
            {
                //Llamada al usecase
                var item = await useCase.AddUser(request);

                //Validar que no exista el usuario, si existe en la base de datos, el SP en la base devuelve 0
                if (item.username == "0")
                {
                    response = new GenericResponse<RegistroUsuarioResponse>()
                    {
                        Item = null,
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Description = "NOMBRE DE USUARIO YA EXISTE"
                        }
                    };
                    return Ok(response);
                }

                //Validar que no se haya ocupado el correo antes, si existe en la base de datos, el SP en la base devuelve -1
                if (item.username == "-1")
                {
                    response = new GenericResponse<RegistroUsuarioResponse>()
                    {
                        Item = null,
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Description = "CORREO DE USUARIO YA ESTÁ EN USO"
                        }
                    };
                    return Ok(response);
                }

                //Si no existe el username ni el correo en la base de datos, el usuario se dio de alta. 
                if (item.username != "-1" && item.username != "0")
                {
                    response = new GenericResponse<RegistroUsuarioResponse>()
                    {
                        Item = null,
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Description = "USUARIO DADO DE ALTA"
                        }
                    };
                    return Ok(response);
                }
                else
                    response = new GenericResponse<RegistroUsuarioResponse>()
                    {
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Description = $"No se ha podido registrar"
                        }
                    };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                response = new GenericResponse<RegistroUsuarioResponse>()
                {
                    status = new HttpCodeStatus()
                    {
                        Code = System.Net.HttpStatusCode.InternalServerError,
                        Description = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/ModificarUsuario")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromBody] ModificarUsuarioRequest request)
        {
            GenericResponse<ModificarUsuarioRequest> response;
            try
            {
                //Llamada al useCase
                var item = await useCase.UpdateUser(request);

                
                if (string.IsNullOrEmpty(item.username))
                {
                    //si se especifico un idUser o username que no existe en la base, indicará que el usuario no existe
                    response = new GenericResponse<ModificarUsuarioRequest>()
                    {
                        Item = null,
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Description = $"Id de Usuario no existe"
                        }
                    };
                    return Ok(response);
                }

                else
                    response = new GenericResponse<ModificarUsuarioRequest>()
                    {
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Description = $"USUARIO ACTUALIZADO"
                        }
                    };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                response = new GenericResponse<ModificarUsuarioRequest>()
                {
                    status = new HttpCodeStatus()
                    {
                        Code = System.Net.HttpStatusCode.InternalServerError,
                        Description = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/DesactivarUsuario")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DefuseUser([FromBody] DesactivarUsuarioRequest request)
        {
            GenericResponse<DesactivarUsuarioRequest> response;
            try
            {

                var item = await useCase.DefuseUser(request);

                if (string.IsNullOrEmpty(item.username))
                {
                    //si se especifico un username que no existe en la base, indicará que el usuario no existe
                    response = new GenericResponse<DesactivarUsuarioRequest>()
                    {
                        Item = null,
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Description = $"Id de Usuario no existe"
                        }
                    };
                    return Ok(response);
                }

                else
                    response = new GenericResponse<DesactivarUsuarioRequest>()
                    {
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Description = $"USUARIO DADO DE BAJA"
                        }
                    };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                response = new GenericResponse<DesactivarUsuarioRequest>()
                {
                    status = new HttpCodeStatus()
                    {
                        Code = System.Net.HttpStatusCode.InternalServerError,
                        Description = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/ObtenerUsuarios")]
        public async Task<List<UsuarioResponse>> GetUsers()
        {
            var item = await useCase.GetUsers();
            return item;
        }

        
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/PasswordReset")]
        public Task<EmailPasswordResetResponse> passwordReset(ResetPasswordRequest request)
        {
            var response = useCase.passwordReset(request);
            return response;
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/cambiarContraseña/")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public Task<string> changePassword(CambiarClaveRequest request)
        {
            var response = useCase.changePassword(request);
            return response;
        }
    }
}
