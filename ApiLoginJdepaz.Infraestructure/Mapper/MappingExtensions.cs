using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ApiLoginJdepaz.Infraestructure.Mapper
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddAndConfigMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
