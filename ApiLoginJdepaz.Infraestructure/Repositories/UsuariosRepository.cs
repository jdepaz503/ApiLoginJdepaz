﻿using AutoMapper;
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
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ApiLoginJdepaz.Infraestructure.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly DataContext db;
        private readonly IMapper map;
        private readonly IConfiguration config;
        public UsuariosRepository(DataContext _db, IMapper mapper, IConfiguration configuration)
        {
            db = _db ?? throw new ArgumentNullException(nameof(_db));
            map = mapper ?? throw new ArgumentNullException(nameof(mapper));
            config = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
            var paramPatron = new SqlParameter("@Patron", config["AppSettings:PatronConfig"]);
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
            var paramPatron = new SqlParameter("@Patron", config["AppSettings:PatronConfig"]);
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
            var paramPatron = new SqlParameter("@Patron", config["AppSettings:PatronConfig"]);
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

        

        public async Task<EmailPasswordResetResponse> passwordReset(ResetPasswordRequest request)
        {
            EmailPasswordResetResponse response = new EmailPasswordResetResponse();
            var paramUserName = new SqlParameter("@username", request.user);
            var paramCorreo= new SqlParameter("@email_user", request.correo);
            try
            {
                IList<TblUsuarios> usr = await db.Usuarios.FromSqlRaw(
                    "SP_PasswordReset @username,@email_user ", paramUserName, paramCorreo).ToListAsync();
                if (usr != null && usr.Count != 0)
                {
                    
                    var envio = sendMail(request.user, usr.FirstOrDefault().email_user);
                    response.email_user = envio;
                }
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                throw;
            }

        }
        
        public string sendMail(string user,string correo)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(config["EnvioCorreo:CorreoFrom"]));
                email.To.Add(MailboxAddress.Parse(correo));

                //generar JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(config["JWT:key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.Email, correo),
                            new Claim(ClaimTypes.UserData, user)
                    }),
                    Audience = correo,
                    IssuedAt = DateTime.UtcNow,
                    Issuer = config["JWT:Issuer"],
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Sid, correo));
                if (!string.IsNullOrEmpty(correo))
                    tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, correo));

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                string Token = tokenHandler.WriteToken(securityToken);



                email.Subject = "ResetPassword ApiLoginJdepaz";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = 
                    "<h4>ApiLoginJdepaz</h4>" +
                    "<p>" + user + " Gracias por utilizar el servicio de Johnny De Paz, podrá cambiar su contraseña mediante el siguiente " +
                    "<a href='"+ config["AppSettings:LinkFrontforEmail"] + "' target='_blank'>enlance</a>, recuerde que solo tienes 15 minutos.</p>"
                };

                // send email
                var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(config["EnvioCorreo:CorreoFrom"], config["EnvioCorreo:PassCorreoFrom"]);
                smtp.Send(email);
                smtp.Disconnect(true);
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                return Token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                return "Error" + ex.Message;
            }
        }

        public async Task<string> changePassword(CambiarClaveRequest request)
        {
            string result = "";
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(request.token) as JwtSecurityToken;
            var patron = config["AppSettings:PatronConfig"];
            var emailInToken = securityToken.Claims.Where(c => c.Type == "email").Select(x => x.Value).FirstOrDefault();
            var UserInToken = securityToken.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata").Select(x => x.Value).FirstOrDefault();
            var paramUser_name = new SqlParameter("@username", UserInToken);
            var paramEmail_user = new SqlParameter("@email_user", emailInToken);
            var ParamPass_user = new SqlParameter("@pass_user", request.newPassword);
            var paramPatron = new SqlParameter("@Patron", patron);
            try
            {
                IList<TblUsuarios> usr = await db.Usuarios.FromSqlRaw("SP_cambiarClaveUsuario @username,@email_user,@pass_user,@Patron", paramUser_name, paramEmail_user, ParamPass_user, paramPatron).ToListAsync();
                if (usr != null && usr.Count != 0)
                {
                    result ="Se cambió la clave para el usuario "+ UserInToken + " con correo " +usr.FirstOrDefault().email_user;
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                result = ex.Message;
                throw;
            }

            return result;
        }

    }
}
