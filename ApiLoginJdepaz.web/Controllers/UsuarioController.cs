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
        [Route("~/api/v{version:ApiVersion}/Agregar")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddUser([FromBody] RegistroUsuarioRequest request)
        {
            GenericResponse<RegistroUsuarioResponse> response;
            try
            {

                var item = await useCase.AddUser(request);

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
        [Route("~/api/v{version:ApiVersion}/Modificar")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromBody] ModificarUsuarioRequest request)
        {
            GenericResponse<ModificarUsuarioRequest> response;
            try
            {

                var item = await useCase.UpdateUser(request);

                if (string.IsNullOrEmpty(item.username))
                {
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
        [Route("~/api/v{version:ApiVersion}/Desactivar")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DefuseUser([FromBody] DesactivarUsuarioRequest request)
        {
            GenericResponse<DesactivarUsuarioRequest> response;
            try
            {

                var item = await useCase.DefuseUser(request);

                if (string.IsNullOrEmpty(item.username))
                {
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
        [Route("~/api/v{version:ApiVersion}/cambiarContraseña/{token},{newPassword}")]
        public string changePassword(string token, string newPassword)
        {
            var response = useCase.changePassword(token, newPassword);
            return response;
        }
    }
}
