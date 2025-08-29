using Microsoft.EntityFrameworkCore;
using PruebaProgramadorBackendCSharp.Models;

namespace PruebaProgramadorBackendCSharp.Data
{
    public class PruebaDbContext : DbContext
    {
        public PruebaDbContext(DbContextOptions<PruebaDbContext> options) : base(options) { }

        public DbSet<MarcaAuto> MarcasAutos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MarcaAuto>(b =>
            {
                b.HasKey(m => m.Id);
                b.Property(m => m.Id).ValueGeneratedOnAdd(); // identity
                b.Property(m => m.Nombre).IsRequired().HasMaxLength(100);
                b.Property(m => m.Descripcion).HasMaxLength(500);
                b.Property(m => m.FechaCreacion).IsRequired();
            });

            modelBuilder.Entity<MarcaAuto>().HasData(
                new MarcaAuto
                {
                    Id = 1,
                    Nombre = "Toyota",
                    Descripcion = "Marca japonesa",
                    FechaCreacion = DateTime.SpecifyKind(new DateTime(1937, 8, 28), DateTimeKind.Utc)
                },
                new MarcaAuto
                {
                    Id = 2,
                    Nombre = "Ford",
                    Descripcion = "Marca estadounidense",
                    FechaCreacion = DateTime.SpecifyKind(new DateTime(1903, 6, 16), DateTimeKind.Utc)
                },
                new MarcaAuto
                {
                    Id = 3,
                    Nombre = "BMW",
                    Descripcion = "Marca alemana",
                    FechaCreacion = DateTime.SpecifyKind(new DateTime(1916, 3, 7), DateTimeKind.Utc)
                }
            );
        }
    }
}
