using ApiLoginJdepaz.Core.Domains.Login;
using ApiLoginJdepaz.Core.Models.Generic;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using AutoMapper.Configuration;
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
//
using Microsoft.Extensions.Configuration;

namespace ApiLoginJdepaz.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioUseCase useCase;
        //private readonly IConfiguration _config;
        public LoginController(IUsuarioUseCase usuarioUseCase)
        {
            useCase = usuarioUseCase ?? throw new ArgumentNullException(nameof(usuarioUseCase));
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("~/api/v{version:ApiVersion}/Auth")]
        public async Task<IActionResult> Index([FromBody] LoginRequest request)
        {
            GenericResponse<LoginResponse> response;
            try
            {
                LoginWithPatron requestPatron = new LoginWithPatron()
                {
                    pass_user = request.pass_user,
                    username = request.username,
                    Patron = "V4Kc10ne$"//conf["AppSettings:PatronConfig"]
                };
                
                var item = await useCase.LoginUsuario(requestPatron);


                if (item != null && !string.IsNullOrEmpty(item.username))
                {
                    if (item.Estado == 0)
                    {
                        LoginResponse loginResponseUnauthorized = new LoginResponse()
                        {
                            Jwt = "0",
                            ExpirationDate = DateTime.Today.AddDays(-1)
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
                    }
                    else
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes("123456-ABC-WXYZ-654321-V4Kc10ne$");//conf["AppSettings:Auth_SignInKey"]);_Configuration.GetValue<string>("JwtToken:Auth_SignInKey")
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.Name, $"{item.nombre_user}"),
                                //new Claim(ClaimTypes.Role, usr.IdRol.ToString()),
                                //new Claim(ClaimTypes.Gender, usr.Genero == "M" ? "Masculino" : "Femenino")
                            }),
                            Audience = request.username,
                            IssuedAt = DateTime.UtcNow,
                            Issuer = "http://localhost:5000",
                            Expires = DateTime.UtcNow.AddMinutes(5),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Sid, item.email_user));
                        if (!string.IsNullOrEmpty(item.email_user))
                            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, item.email_user));
                        if (!string.IsNullOrEmpty(item.telefono_user))
                            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.HomePhone, item.telefono_user.ToString()));

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
