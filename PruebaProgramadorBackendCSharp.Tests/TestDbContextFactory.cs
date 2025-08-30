using Microsoft.EntityFrameworkCore;
using PruebaProgramadorBackendCSharp.Data;

namespace PruebaProgramadorBackendCSharp.Tests
{
    /// <summary>
    /// Factory para crear contextos de base de datos en memoria para las pruebas unitarias
    /// </summary>
    public static class TestDbContextFactory
    {
        /// <summary>
        /// Crea un contexto de base de datos en memoria para las pruebas
        /// </summary>
        /// <param name="databaseName">Nombre único para la base de datos en memoria</param>
        /// <returns>Instancia del contexto de base de datos configurada para pruebas</returns>
        public static PruebaDbContext CreateInMemoryDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<PruebaDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            var context = new PruebaDbContext(options);
            
            // Asegurar que la base de datos se crea limpia para cada prueba
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            // Limpiar cualquier dato de semilla para evitar conflictos en las pruebas
            context.MarcasAutos.RemoveRange(context.MarcasAutos);
            context.SaveChanges();
            
            return context;
        }

        /// <summary>
        /// Crea un contexto de base de datos en memoria con datos de semilla para las pruebas
        /// </summary>
        /// <param name="databaseName">Nombre único para la base de datos en memoria</param>
        /// <returns>Instancia del contexto con datos de prueba precargados</returns>
        public static PruebaDbContext CreateInMemoryDbContextWithSeedData(string databaseName)
        {
            var context = CreateInMemoryDbContext(databaseName);
            
            // Los datos de semilla se cargan automáticamente por la configuración del modelo
            // No necesitamos agregar datos adicionales aquí ya que el contexto tiene HasData configurado
            
            return context;
        }
    }
}