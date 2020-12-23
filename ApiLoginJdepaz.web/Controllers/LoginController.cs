using ApiLoginJdepaz.Core.Domains.Login;
using ApiLoginJdepaz.Core.Models.Generic;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace ApiLoginJdepaz.web.Controllers
{
    /*
        Controller Login: Es el encargado de validar si el usuario existe en la base de datos mediante el username y passuser. 
        Endpoint: 
        1-Login: Pide un loginRequest como parametros, el cual contiene username y pass_user, endpoint consulta a useCase.LoginUsuario
    */

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] 
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioUseCase useCase;
        private readonly IConfiguration config;
        public LoginController(IUsuarioUseCase usuarioUseCase, IConfiguration configuration)
        {
            useCase = usuarioUseCase ?? throw new ArgumentNullException(nameof(usuarioUseCase));
            config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/Auth")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            
            GenericResponse<LoginResponse> response;
            try
            {
                string patron = config["AppSettings:PatronConfig"];
                LoginWithPatron requestPatron = new LoginWithPatron()
                {
                    pass_user = request.pass_user,
                    username = request.username,
                    Patron = patron
                };
                
                //Consulta al useCase LoginUsuario, encargado de devolvernos el registro de usuario si llegace a existir en la base. 
                var item = await useCase.LoginUsuario(requestPatron);


                //Validar que el username no sea null
                if (item != null && !string.IsNullOrEmpty(item.username))
                {
                    //validar que usuario no esté inactivo
                    if (item.Estado == 0)
                    {
                        LoginResponse loginResponseUnauthorized = new LoginResponse()
                        {
                            Jwt = "0",
                            ExpirationDate = DateTime.Today.AddDays(-1)
                            //Se devuelve el LoginResponse sin token
                        };
                        response = new GenericResponse<LoginResponse>()
                        {
                            Item = loginResponseUnauthorized,
                            status = new HttpCodeStatus()
                            {
                                Code = System.Net.HttpStatusCode.Unauthorized,
                                Description = "USUARIO INHABILITADO"
                            }
                        };
                        //Se manda el Gererin response indicando que usuario está inhabilitado
                    }
                    else
                    {
                        //Si sse especifica 
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(config["JWT:key"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, $"{item.nombre_user}"),
                                new Claim(ClaimTypes.Email, $"{item.email_user}")
                            }),
                            Audience = request.username,
                            IssuedAt = DateTime.UtcNow,
                            Issuer = config["JWT:Issuer"],
                            Expires = DateTime.UtcNow.AddMinutes(300),//Caducidad de 5 horas
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Sid, item.email_user));
                        if (!string.IsNullOrEmpty(item.email_user))
                            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, item.email_user));
                        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                        string Token = tokenHandler.WriteToken(securityToken);

                        LoginResponse loginResponse = new LoginResponse()
                        {
                            Jwt = Token,
                            ExpirationDate = tokenDescriptor.Expires.Value
                        };
                        response = new GenericResponse<LoginResponse>()
                        {
                            Item = loginResponse,
                            status = new HttpCodeStatus()
                            {
                                Code = System.Net.HttpStatusCode.OK,
                                Description = "OK"
                            }
                        };
                    }
                }
                else
                    response = new GenericResponse<LoginResponse>()
                    {
                        status = new HttpCodeStatus()
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Description = $"No se ha encontrado el usuario con nombre {request.username}"
                        }
                    };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                response = new GenericResponse<LoginResponse>()
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
    }
}
