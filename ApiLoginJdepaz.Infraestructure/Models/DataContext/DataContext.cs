using ApiLoginJdepaz.Infraestructure.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiLoginJdepaz.Infraestructure.Models.DataContext
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TblUsuarios> Usuarios { get; set; }
        public DbSet<TblProductos> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TblUsuarios>().ToTable("Usuarios");
            //modelBuilder.Entity<TblProductos>().ToTable("Productos");
        }
    }
}
