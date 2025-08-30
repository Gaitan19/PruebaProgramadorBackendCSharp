using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PruebaProgramadorBackendCSharp.Data;
using PruebaProgramadorBackendCSharp.Models;
using PruebaProgramadorBackendCSharp.Repositories;

namespace PruebaProgramadorBackendCSharp.Tests.Repositories
{
    /// <summary>
    /// Pruebas unitarias para la clase MarcaAutoRepository
    /// Verifica el correcto funcionamiento de las operaciones CRUD del repositorio
    /// </summary>
    public class MarcaAutoRepositoryTests : IDisposable
    {
        private readonly PruebaDbContext _context;
        private readonly MarcaAutoRepository _repository;
        private readonly string _databaseName;

        public MarcaAutoRepositoryTests()
        {
            // Crear un nombre único para la base de datos en memoria para cada test
            _databaseName = Guid.NewGuid().ToString();
            _context = TestDbContextFactory.CreateInMemoryDbContext(_databaseName);
            _repository = new MarcaAutoRepository(_context);
        }

        /// <summary>
        /// Prueba que GetAllAsync devuelve todas las marcas de la base de datos
        /// </summary>
        [Fact]
        public async Task GetAllAsync_DebeRetornarTodasLasMarcas()
        {
            // Arrange - Configurar datos de prueba
            var marca1 = new MarcaAuto { Nombre = "Toyota", Descripcion = "Marca japonesa", FechaCreacion = DateTime.UtcNow };
            var marca2 = new MarcaAuto { Nombre = "Ford", Descripcion = "Marca estadounidense", FechaCreacion = DateTime.UtcNow };

            await _context.MarcasAutos.AddRangeAsync(marca1, marca2);
            await _context.SaveChangesAsync();

            // Act - Ejecutar la acción a probar
            var resultado = await _repository.GetAllAsync();

            // Assert - Verificar los resultados
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.Should().Contain(m => m.Nombre == "Toyota");
            resultado.Should().Contain(m => m.Nombre == "Ford");
        }

        /// <summary>
        /// Prueba que GetAllAsync devuelve una colección vacía cuando no hay marcas
        /// </summary>
        [Fact]
        public async Task GetAllAsync_CuandoNoHayMarcas_DebeRetornarColeccionVacia()
        {
            // Act
            var resultado = await _repository.GetAllAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        /// <summary>
        /// Prueba que GetByIdAsync devuelve la marca correcta cuando existe
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_CuandoExisteLaMarca_DebeRetornarLaMarca()
        {
            // Arrange
            var marca = new MarcaAuto 
            { 
                Nombre = "BMW", 
                Descripcion = "Marca alemana", 
                FechaCreacion = DateTime.UtcNow 
            };

            await _context.MarcasAutos.AddAsync(marca);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _repository.GetByIdAsync(marca.Id);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Id.Should().Be(marca.Id);
            resultado.Nombre.Should().Be("BMW");
            resultado.Descripcion.Should().Be("Marca alemana");
        }

        /// <summary>
        /// Prueba que GetByIdAsync devuelve null cuando la marca no existe
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_CuandoNoExisteLaMarca_DebeRetornarNull()
        {
            // Act
            var resultado = await _repository.GetByIdAsync(999);

            // Assert
            resultado.Should().BeNull();
        }

        /// <summary>
        /// Prueba que AddAsync agrega correctamente una nueva marca
        /// </summary>
        [Fact]
        public async Task AddAsync_DebeAgregarNuevaMarcaCorrectamente()
        {
            // Arrange
            var nuevaMarca = new MarcaAuto
            {
                Nombre = "Mercedes-Benz",
                Descripcion = "Marca alemana de lujo",
                FechaCreacion = DateTime.UtcNow
            };

            // Act
            await _repository.AddAsync(nuevaMarca);
            await _repository.SaveChangesAsync();

            // Assert
            var marcaEnBd = await _context.MarcasAutos.FirstOrDefaultAsync(m => m.Nombre == "Mercedes-Benz");
            marcaEnBd.Should().NotBeNull();
            marcaEnBd!.Descripcion.Should().Be("Marca alemana de lujo");
            marcaEnBd.Id.Should().BeGreaterThan(0); // Debe tener un ID asignado
        }

        /// <summary>
        /// Prueba que Update actualiza correctamente una marca existente
        /// </summary>
        [Fact]
        public async Task Update_DebeActualizarMarcaExistente()
        {
            // Arrange
            var marca = new MarcaAuto
            {
                Nombre = "Volkswagen",
                Descripcion = "Marca alemana",
                FechaCreacion = DateTime.UtcNow
            };

            await _context.MarcasAutos.AddAsync(marca);
            await _context.SaveChangesAsync();

            // Modificar la marca
            marca.Descripcion = "Marca alemana popular";

            // Act
            _repository.Update(marca);
            await _repository.SaveChangesAsync();

            // Assert
            var marcaActualizada = await _context.MarcasAutos.FindAsync(marca.Id);
            marcaActualizada.Should().NotBeNull();
            marcaActualizada!.Descripcion.Should().Be("Marca alemana popular");
        }

        /// <summary>
        /// Prueba que Delete elimina correctamente una marca
        /// </summary>
        [Fact]
        public async Task Delete_DebeEliminarMarcaCorrectamente()
        {
            // Arrange
            var marca = new MarcaAuto
            {
                Nombre = "Audi",
                Descripcion = "Marca alemana premium",
                FechaCreacion = DateTime.UtcNow
            };

            await _context.MarcasAutos.AddAsync(marca);
            await _context.SaveChangesAsync();
            var marcaId = marca.Id;

            // Act
            _repository.Delete(marca);
            await _repository.SaveChangesAsync();

            // Assert
            var marcaEliminada = await _context.MarcasAutos.FindAsync(marcaId);
            marcaEliminada.Should().BeNull();
        }

        /// <summary>
        /// Prueba que SaveChangesAsync persiste los cambios en la base de datos
        /// </summary>
        [Fact]
        public async Task SaveChangesAsync_DebePersistirCambiosEnBaseDeDatos()
        {
            // Arrange
            var marca = new MarcaAuto
            {
                Nombre = "Nissan",
                Descripcion = "Marca japonesa",
                FechaCreacion = DateTime.UtcNow
            };

            await _repository.AddAsync(marca);

            // Act
            await _repository.SaveChangesAsync();

            // Assert
            var marcasEnBd = await _context.MarcasAutos.CountAsync();
            marcasEnBd.Should().Be(1);
        }

        /// <summary>
        /// Limpia los recursos después de cada prueba
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}