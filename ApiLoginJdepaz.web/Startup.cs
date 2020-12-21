using ApiLoginJdepaz.Core.Interfaces;
using ApiLoginJdepaz.Core.UseCase;
using ApiLoginJdepaz.Core.UseCase.Interfaces;
using ApiLoginJdepaz.Infraestructure.Mapper;
using ApiLoginJdepaz.Infraestructure.Models;
using ApiLoginJdepaz.Infraestructure.Models.DataContext;
using ApiLoginJdepaz.Infraestructure.Repositories;
using ApiLoginJdepaz.web.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
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
            OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
            {
                Description = "Ingrese token de seguridad en campo pendiente",
                Name = "Authorization",
                In = ParameterLocation.Header,
                //BearerFormat = "JWT",
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

            services.AddTransient<IUsuariosRepository, UsuariosRepository>();
            services.AddTransient<IUsuarioUseCase, UsuarioUseCase>();
            //services.AddTransient<IUsuariosRepository, UsuariosRepository>();

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
                c.AddSecurityRequirement(securityRequirement);
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            });

            AuthValidations Validations = new AuthValidations();
            
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:SecretKey"]));
            //d30df44c-a3ab-41d0-af6f-7370e2e6db86
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                //ValidIssuer = ServerAddress,
                ValidateIssuerSigningKey = false,
                AudienceValidator = Validations.AudienceValidation,
                //IssuerSigningKeyValidator = Validations.SignInKeyValidation,
                //IssuerValidator = Validations.IssuerValidation,
                IssuerSigningKey = key,

                RequireSignedTokens = true,
                RoleClaimTypeRetriever = Validations.RoleClaimTypeRetreiverAssign,
                RoleClaimType = "string"
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
            services.AddAuthorization();

            services.AddAndConfigDbContext<DataContext>(Configuration.GetConnectionString("DefaultConnection"));
            services.AddAndConfigMapper();


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Localhost"))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Johnny's API");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
