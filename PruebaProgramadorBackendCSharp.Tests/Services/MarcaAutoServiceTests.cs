using FluentAssertions;
using Moq;
using PruebaProgramadorBackendCSharp.DTOs;
using PruebaProgramadorBackendCSharp.Models;
using PruebaProgramadorBackendCSharp.Repositories;
using PruebaProgramadorBackendCSharp.Services;

namespace PruebaProgramadorBackendCSharp.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para la clase MarcaAutoService
    /// Verifica la lógica de negocio y validaciones del servicio
    /// </summary>
    public class MarcaAutoServiceTests
    {
        private readonly Mock<IMarcaAutoRepository> _mockRepository;
        private readonly MarcaAutoService _service;

        public MarcaAutoServiceTests()
        {
            _mockRepository = new Mock<IMarcaAutoRepository>();
            _service = new MarcaAutoService(_mockRepository.Object);
        }

        /// <summary>
        /// Prueba que ObtenerTodasAsync devuelve todas las marcas del repositorio
        /// </summary>
        [Fact]
        public async Task ObtenerTodasAsync_DebeRetornarTodasLasMarcasDelRepositorio()
        {
            // Arrange
            var marcasEsperadas = new List<MarcaAuto>
            {
                new MarcaAuto { Id = 1, Nombre = "Toyota", Descripcion = "Marca japonesa", FechaCreacion = DateTime.UtcNow },
                new MarcaAuto { Id = 2, Nombre = "Ford", Descripcion = "Marca estadounidense", FechaCreacion = DateTime.UtcNow }
            };

            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(marcasEsperadas);

            // Act
            var resultado = await _service.ObtenerTodasAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.Should().BeEquivalentTo(marcasEsperadas);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        /// <summary>
        /// Prueba que ObtenerPorIdAsync devuelve la marca correcta cuando existe
        /// </summary>
        [Fact]
        public async Task ObtenerPorIdAsync_CuandoExisteLaMarca_DebeRetornarLaMarca()
        {
            // Arrange
            var marcaEsperada = new MarcaAuto 
            { 
                Id = 1, 
                Nombre = "BMW", 
                Descripcion = "Marca alemana", 
                FechaCreacion = DateTime.UtcNow 
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(marcaEsperada);

            // Act
            var resultado = await _service.ObtenerPorIdAsync(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(marcaEsperada);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        /// <summary>
        /// Prueba que ObtenerPorIdAsync lanza excepción cuando la marca no existe
        /// </summary>
        [Fact]
        public async Task ObtenerPorIdAsync_CuandoNoExisteLaMarca_DebeLanzarKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((MarcaAuto?)null);

            // Act & Assert
            var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ObtenerPorIdAsync(999));
            excepcion.Message.Should().Be("Marca no encontrada.");
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
        }

        /// <summary>
        /// Prueba que CrearAsync crea una nueva marca correctamente
        /// </summary>
        [Fact]
        public async Task CrearAsync_DebeCrearNuevaMarcaCorrectamente()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "Mercedes-Benz",
                Descripcion = "Marca alemana de lujo"
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<MarcaAuto>()))
                .Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _service.CrearAsync(dto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Nombre.Should().Be("Mercedes-Benz");
            resultado.Descripcion.Should().Be("Marca alemana de lujo");
            resultado.FechaCreacion.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

            _mockRepository.Verify(r => r.AddAsync(It.Is<MarcaAuto>(m => 
                m.Nombre == "Mercedes-Benz" && 
                m.Descripcion == "Marca alemana de lujo")), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Prueba que CrearAsync asigna correctamente la fecha de creación en UTC
        /// </summary>
        [Fact]
        public async Task CrearAsync_DebeAsignarFechaCreacionEnUtc()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "Porsche",
                Descripcion = "Marca alemana deportiva"
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<MarcaAuto>()))
                .Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var fechaAntes = DateTime.UtcNow;

            // Act
            var resultado = await _service.CrearAsync(dto);

            // Assert
            var fechaDespues = DateTime.UtcNow;
            resultado.FechaCreacion.Should().BeAfter(fechaAntes.AddSeconds(-1));
            resultado.FechaCreacion.Should().BeBefore(fechaDespues.AddSeconds(1));
            resultado.FechaCreacion.Kind.Should().Be(DateTimeKind.Utc);
        }

        /// <summary>
        /// Prueba que ActualizarAsync actualiza una marca existente correctamente
        /// </summary>
        [Fact]
        public async Task ActualizarAsync_CuandoExisteLaMarca_DebeActualizarCorrectamente()
        {
            // Arrange
            var marcaExistente = new MarcaAuto
            {
                Id = 1,
                Nombre = "Volkswagen",
                Descripcion = "Marca alemana",
                FechaCreacion = DateTime.UtcNow.AddYears(-1)
            };

            var dto = new MarcaAutoUpdateDto
            {
                Id = 1,
                Nombre = "Volkswagen",
                Descripcion = "Marca alemana popular"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(marcaExistente);
            _mockRepository.Setup(r => r.Update(It.IsAny<MarcaAuto>()));
            _mockRepository.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _service.ActualizarAsync(dto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Nombre.Should().Be("Volkswagen");
            resultado.Descripcion.Should().Be("Marca alemana popular");

            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            _mockRepository.Verify(r => r.Update(marcaExistente), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Prueba que ActualizarAsync lanza excepción cuando el ID es inválido
        /// </summary>
        [Fact]
        public async Task ActualizarAsync_CuandoIdEsInvalido_DebeLanzarArgumentException()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 0,
                Nombre = "Test",
                Descripcion = "Test"
            };

            // Act & Assert
            var excepcion = await Assert.ThrowsAsync<ArgumentException>(() => _service.ActualizarAsync(dto));
            excepcion.Message.Should().Be("El ID debe ser válido.");
            _mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Prueba que ActualizarAsync lanza excepción cuando la marca no existe
        /// </summary>
        [Fact]
        public async Task ActualizarAsync_CuandoNoExisteLaMarca_DebeLanzarKeyNotFoundException()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 999,
                Nombre = "Test",
                Descripcion = "Test"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((MarcaAuto?)null);

            // Act & Assert
            var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ActualizarAsync(dto));
            excepcion.Message.Should().Be("Marca no encontrada.");
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
        }

        /// <summary>
        /// Prueba que EliminarAsync elimina una marca existente correctamente
        /// </summary>
        [Fact]
        public async Task EliminarAsync_CuandoExisteLaMarca_DebeEliminarCorrectamente()
        {
            // Arrange
            var marcaExistente = new MarcaAuto
            {
                Id = 1,
                Nombre = "Audi",
                Descripcion = "Marca alemana premium",
                FechaCreacion = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(marcaExistente);
            _mockRepository.Setup(r => r.Delete(It.IsAny<MarcaAuto>()));
            _mockRepository.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.EliminarAsync(1);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            _mockRepository.Verify(r => r.Delete(marcaExistente), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Prueba que EliminarAsync lanza excepción cuando el ID es inválido
        /// </summary>
        [Fact]
        public async Task EliminarAsync_CuandoIdEsInvalido_DebeLanzarArgumentException()
        {
            // Act & Assert
            var excepcion = await Assert.ThrowsAsync<ArgumentException>(() => _service.EliminarAsync(0));
            excepcion.Message.Should().Be("El ID debe ser válido.");
            _mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Prueba que EliminarAsync lanza excepción cuando la marca no existe
        /// </summary>
        [Fact]
        public async Task EliminarAsync_CuandoNoExisteLaMarca_DebeLanzarKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((MarcaAuto?)null);

            // Act & Assert
            var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.EliminarAsync(999));
            excepcion.Message.Should().Be("Marca no encontrada.");
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
        }
    }
}