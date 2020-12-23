using ApiLoginJdepaz.Core.Interfaces;
using ApiLoginJdepaz.Core.UseCase;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using ApiLoginJdepaz.Infraestructure.Mapper;
using ApiLoginJdepaz.Infraestructure.Models;
using ApiLoginJdepaz.Infraestructure.Models.DataContext;
using ApiLoginJdepaz.Infraestructure.Repositories;
using ApiLoginJdepaz.web.Auth;
using ApiLoginJdepaz.web.Helpers;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ApiLoginJdepaz.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddApiVersioning();
            services.AddElmah();
            OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
            {
                Description = "Ingrese token de seguridad en campo pendiente",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Reference = new OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            OpenApiSecurityRequirement securityRequirement = new OpenApiSecurityRequirement();
            securityRequirement.Add(securityScheme, new List<string>());

            //Inyección de dependencias
            services.AddTransient<IUsuariosRepository, UsuariosRepository>();
            services.AddTransient<IUsuarioUseCase, UsuarioUseCase>();
            services.AddTransient<IProductosRepository, ProductosRepository>();
            services.AddTransient<IProductoUseCase, ProductoUseCase>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Johnny's API",
                    Version = "v1",
                    Contact = new OpenApiContact() { Name = "Johnny De Paz", Email = "jdepaz2012@gmail.com" }
                });
                c.SchemaFilter<EnumSchemaFilter>();
                c.AddSecurityRequirement(securityRequirement);
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            });

            AuthValidations Validations = new AuthValidations();

            string signInKey = "d307a600-a844-1008-c7be-aa7f98c0a71d";// Configuration["AppSettings:SecretKey"];//123456-ABC-WXYZ-654321-V4Kc10ne$
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signInKey));
            //d30df44c-a3ab-41d0-af6f-7370e2e6db86
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = false,
                AudienceValidator = Validations.AudienceValidation,
                //IssuerSigningKeyValidator = Validations.SignInKeyValidation,
                IssuerValidator = Validations.IssuerValidation,
                IssuerSigningKey = key,
                ValidIssuer = "http://localhost:5000",
                RequireSignedTokens = true//,
                //RoleClaimTypeRetriever = Validations.RoleClaimTypeRetreiverAssign,
                //RoleClaimType = "string"
            };
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = validationParameters;
                opts.RequireHttpsMetadata = false;
                opts.SaveToken = true;
                opts.Validate();
            });
            services.AddAuthorization(config =>
            {
                config.AddPolicy(JwtBearerDefaults.AuthenticationScheme, p => p.RequireClaim(ClaimTypes.Sid));
            });

            services.AddAndConfigDbContext<DataContext>(Configuration.GetConnectionString("DefaultConnection"));
            services.AddAndConfigMapper();

            //services.use
            services.AddMvc(r => r.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Johnny's API");
                });
            app.UseElmah();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseMvc();
        }
    }
}
