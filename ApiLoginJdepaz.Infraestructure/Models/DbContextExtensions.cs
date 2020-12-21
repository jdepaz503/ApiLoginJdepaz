using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Infraestructure.Models
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddAndConfigDbContext<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
        {
            return services.AddDbContext<TContext>(opts => opts.UseSqlServer(connectionString));
        }
    }
}
