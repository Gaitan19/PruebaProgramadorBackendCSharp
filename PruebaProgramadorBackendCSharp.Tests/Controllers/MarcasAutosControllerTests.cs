using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PruebaProgramadorBackendCSharp.Controllers;
using PruebaProgramadorBackendCSharp.DTOs;
using PruebaProgramadorBackendCSharp.Models;
using PruebaProgramadorBackendCSharp.Services;

namespace PruebaProgramadorBackendCSharp.Tests.Controllers
{
    /// <summary>
    /// Pruebas unitarias para la clase MarcasAutosController
    /// Verifica el comportamiento de los endpoints del controlador
    /// </summary>
    public class MarcasAutosControllerTests
    {
        private readonly Mock<IMarcaAutoService> _mockService;
        private readonly MarcasAutosController _controller;

        public MarcasAutosControllerTests()
        {
            _mockService = new Mock<IMarcaAutoService>();
            _controller = new MarcasAutosController(_mockService.Object);
        }

        #region Pruebas para GET (obtener todas las marcas)

        /// <summary>
        /// Prueba que GET devuelve OK con todas las marcas cuando hay datos
        /// </summary>
        [Fact]
        public async Task Get_CuandoHayMarcas_DebeRetornarOkConTodasLasMarcas()
        {
            // Arrange
            var marcas = new List<MarcaAuto>
            {
                new MarcaAuto { Id = 1, Nombre = "Toyota", Descripcion = "Marca japonesa", FechaCreacion = DateTime.UtcNow },
                new MarcaAuto { Id = 2, Nombre = "Ford", Descripcion = "Marca estadounidense", FechaCreacion = DateTime.UtcNow }
            };

            _mockService.Setup(s => s.ObtenerTodasAsync())
                .ReturnsAsync(marcas);

            // Act
            var resultado = await _controller.Get();

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var marcasDevueltas = okResult.Value.Should().BeAssignableTo<IEnumerable<MarcaAuto>>().Subject;
            marcasDevueltas.Should().HaveCount(2);
            _mockService.Verify(s => s.ObtenerTodasAsync(), Times.Once);
        }

        /// <summary>
        /// Prueba que GET devuelve OK con lista vacía cuando no hay marcas
        /// </summary>
        [Fact]
        public async Task Get_CuandoNoHayMarcas_DebeRetornarOkConListaVacia()
        {
            // Arrange
            var marcasVacias = new List<MarcaAuto>();

            _mockService.Setup(s => s.ObtenerTodasAsync())
                .ReturnsAsync(marcasVacias);

            // Act
            var resultado = await _controller.Get();

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var marcasDevueltas = okResult.Value.Should().BeAssignableTo<IEnumerable<MarcaAuto>>().Subject;
            marcasDevueltas.Should().BeEmpty();
            _mockService.Verify(s => s.ObtenerTodasAsync(), Times.Once);
        }

        #endregion

        #region Pruebas para GET por ID

        /// <summary>
        /// Prueba que GET por ID devuelve OK con la marca cuando existe
        /// </summary>
        [Fact]
        public async Task GetById_CuandoExisteLaMarca_DebeRetornarOkConLaMarca()
        {
            // Arrange
            var marca = new MarcaAuto 
            { 
                Id = 1, 
                Nombre = "BMW", 
                Descripcion = "Marca alemana", 
                FechaCreacion = DateTime.UtcNow 
            };

            _mockService.Setup(s => s.ObtenerPorIdAsync(1))
                .ReturnsAsync(marca);

            // Act
            var resultado = await _controller.Get(1);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var marcaDevuelta = okResult.Value.Should().BeOfType<MarcaAuto>().Subject;
            marcaDevuelta.Id.Should().Be(1);
            marcaDevuelta.Nombre.Should().Be("BMW");
            _mockService.Verify(s => s.ObtenerPorIdAsync(1), Times.Once);
        }

        /// <summary>
        /// Prueba que GET por ID devuelve BadRequest cuando la marca no existe
        /// </summary>
        [Fact]
        public async Task GetById_CuandoNoExisteLaMarca_DebeRetornarBadRequest()
        {
            // Arrange
            _mockService.Setup(s => s.ObtenerPorIdAsync(999))
                .ThrowsAsync(new KeyNotFoundException("Marca no encontrada."));

            // Act
            var resultado = await _controller.Get(999);

            // Assert
            var badRequestResult = resultado.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorObject = badRequestResult.Value.Should().BeEquivalentTo(new { error = "Marca no encontrada." });
            _mockService.Verify(s => s.ObtenerPorIdAsync(999), Times.Once);
        }

        #endregion

        #region Pruebas para POST (crear marca)

        /// <summary>
        /// Prueba que POST crea una nueva marca correctamente
        /// </summary>
        [Fact]
        public async Task Post_ConDatosValidos_DebeRetornarCreatedAtAction()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "Mercedes-Benz",
                Descripcion = "Marca alemana de lujo"
            };

            var nuevaMarca = new MarcaAuto
            {
                Id = 1,
                Nombre = "Mercedes-Benz",
                Descripcion = "Marca alemana de lujo",
                FechaCreacion = DateTime.UtcNow
            };

            _mockService.Setup(s => s.CrearAsync(dto))
                .ReturnsAsync(nuevaMarca);

            // Act
            var resultado = await _controller.Post(dto);

            // Assert
            var createdResult = resultado.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(_controller.Get));
            createdResult.RouteValues!["id"].Should().Be(1);
            
            var marcaCreada = createdResult.Value.Should().BeOfType<MarcaAuto>().Subject;
            marcaCreada.Nombre.Should().Be("Mercedes-Benz");
            _mockService.Verify(s => s.CrearAsync(dto), Times.Once);
        }

        /// <summary>
        /// Prueba que POST devuelve BadRequest cuando el modelo es inválido
        /// </summary>
        [Fact]
        public async Task Post_ConModeloInvalido_DebeRetornarBadRequest()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto { Nombre = "", Descripcion = "" };
            _controller.ModelState.AddModelError("Nombre", "El nombre es obligatorio");

            // Act
            var resultado = await _controller.Post(dto);

            // Assert
            resultado.Should().BeOfType<BadRequestObjectResult>();
            _mockService.Verify(s => s.CrearAsync(It.IsAny<MarcaAutoCreateDto>()), Times.Never);
        }

        /// <summary>
        /// Prueba que POST devuelve BadRequest cuando el servicio lanza excepción
        /// </summary>
        [Fact]
        public async Task Post_CuandoServicioLanzaExcepcion_DebeRetornarBadRequest()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "Test",
                Descripcion = "Test"
            };

            _mockService.Setup(s => s.CrearAsync(dto))
                .ThrowsAsync(new Exception("Error en el servicio"));

            // Act
            var resultado = await _controller.Post(dto);

            // Assert
            var badRequestResult = resultado.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorObject = badRequestResult.Value.Should().BeEquivalentTo(new { error = "Error en el servicio" });
            _mockService.Verify(s => s.CrearAsync(dto), Times.Once);
        }

        #endregion

        #region Pruebas para PUT (actualizar marca)

        /// <summary>
        /// Prueba que PUT actualiza una marca correctamente
        /// </summary>
        [Fact]
        public async Task Put_ConDatosValidos_DebeRetornarOkConMarcaActualizada()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 1,
                Nombre = "Volkswagen",
                Descripcion = "Marca alemana popular"
            };

            var marcaActualizada = new MarcaAuto
            {
                Id = 1,
                Nombre = "Volkswagen",
                Descripcion = "Marca alemana popular",
                FechaCreacion = DateTime.UtcNow
            };

            _mockService.Setup(s => s.ActualizarAsync(dto))
                .ReturnsAsync(marcaActualizada);

            // Act
            var resultado = await _controller.Put(1, dto);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var marcaDevuelta = okResult.Value.Should().BeOfType<MarcaAuto>().Subject;
            marcaDevuelta.Id.Should().Be(1);
            marcaDevuelta.Descripcion.Should().Be("Marca alemana popular");
            _mockService.Verify(s => s.ActualizarAsync(dto), Times.Once);
        }

        /// <summary>
        /// Prueba que PUT devuelve BadRequest cuando el ID de la URL no coincide con el del cuerpo
        /// </summary>
        [Fact]
        public async Task Put_CuandoIdNoCoincide_DebeRetornarBadRequest()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 2,
                Nombre = "Test",
                Descripcion = "Test"
            };

            // Act
            var resultado = await _controller.Put(1, dto);

            // Assert
            var badRequestResult = resultado.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorObject = badRequestResult.Value.Should().BeEquivalentTo(new { error = "El ID de la URL no coincide con el del cuerpo." });
            _mockService.Verify(s => s.ActualizarAsync(It.IsAny<MarcaAutoUpdateDto>()), Times.Never);
        }

        /// <summary>
        /// Prueba que PUT devuelve NotFound cuando la marca no existe
        /// </summary>
        [Fact]
        public async Task Put_CuandoNoExisteLaMarca_DebeRetornarNotFound()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 999,
                Nombre = "Test",
                Descripcion = "Test"
            };

            _mockService.Setup(s => s.ActualizarAsync(dto))
                .ThrowsAsync(new KeyNotFoundException("Marca no encontrada."));

            // Act
            var resultado = await _controller.Put(999, dto);

            // Assert
            resultado.Should().BeOfType<NotFoundResult>();
            _mockService.Verify(s => s.ActualizarAsync(dto), Times.Once);
        }

        #endregion

        #region Pruebas para DELETE

        /// <summary>
        /// Prueba que DELETE elimina una marca correctamente
        /// </summary>
        [Fact]
        public async Task Delete_CuandoExisteLaMarca_DebeRetornarNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.EliminarAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.Delete(1);

            // Assert
            resultado.Should().BeOfType<NoContentResult>();
            _mockService.Verify(s => s.EliminarAsync(1), Times.Once);
        }

        /// <summary>
        /// Prueba que DELETE devuelve NotFound cuando la marca no existe
        /// </summary>
        [Fact]
        public async Task Delete_CuandoNoExisteLaMarca_DebeRetornarNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.EliminarAsync(999))
                .ThrowsAsync(new KeyNotFoundException("Marca no encontrada."));

            // Act
            var resultado = await _controller.Delete(999);

            // Assert
            resultado.Should().BeOfType<NotFoundResult>();
            _mockService.Verify(s => s.EliminarAsync(999), Times.Once);
        }

        /// <summary>
        /// Prueba que DELETE devuelve BadRequest cuando el servicio lanza excepción
        /// </summary>
        [Fact]
        public async Task Delete_CuandoServicioLanzaExcepcion_DebeRetornarBadRequest()
        {
            // Arrange
            _mockService.Setup(s => s.EliminarAsync(1))
                .ThrowsAsync(new Exception("Error en el servicio"));

            // Act
            var resultado = await _controller.Delete(1);

            // Assert
            var badRequestResult = resultado.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorObject = badRequestResult.Value.Should().BeEquivalentTo(new { error = "Error en el servicio" });
            _mockService.Verify(s => s.EliminarAsync(1), Times.Once);
        }

        #endregion
    }
}