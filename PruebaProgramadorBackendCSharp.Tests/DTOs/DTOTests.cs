using FluentAssertions;
using PruebaProgramadorBackendCSharp.DTOs;
using System.ComponentModel.DataAnnotations;

namespace PruebaProgramadorBackendCSharp.Tests.DTOs
{
    /// <summary>
    /// Pruebas unitarias para los DTOs (Data Transfer Objects)
    /// Verifica las validaciones y propiedades de los DTOs
    /// </summary>
    public class DTOTests
    {
        /// <summary>
        /// Prueba que MarcaAutoCreateDto con datos válidos no genera errores de validación
        /// </summary>
        [Fact]
        public void MarcaAutoCreateDto_ConDatosValidos_NoDebeGenerarErroresValidacion()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "Tesla",
                Descripcion = "Marca de vehículos eléctricos"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        /// <summary>
        /// Prueba que MarcaAutoCreateDto con nombre vacío genera error de validación
        /// </summary>
        [Fact]
        public void MarcaAutoCreateDto_ConNombreVacio_DebeGenerarErrorValidacion()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "",
                Descripcion = "Descripción válida"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Nombre"));
        }

        /// <summary>
        /// Prueba que MarcaAutoCreateDto con nombre null genera error de validación
        /// </summary>
        [Fact]
        public void MarcaAutoCreateDto_ConNombreNull_DebeGenerarErrorValidacion()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = null!,
                Descripcion = "Descripción válida"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Nombre"));
        }

        /// <summary>
        /// Prueba que MarcaAutoCreateDto con nombre muy largo genera error de validación
        /// </summary>
        [Fact]
        public void MarcaAutoCreateDto_ConNombreMuyLargo_DebeGenerarErrorValidacion()
        {
            // Arrange - Nombre de más de 100 caracteres
            var nombreLargo = new string('A', 101);
            var dto = new MarcaAutoCreateDto
            {
                Nombre = nombreLargo,
                Descripcion = "Descripción válida"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Nombre"));
        }

        /// <summary>
        /// Prueba que MarcaAutoCreateDto con descripción vacía genera error de validación
        /// </summary>
        [Fact]
        public void MarcaAutoCreateDto_ConDescripcionVacia_DebeGenerarErrorValidacion()
        {
            // Arrange
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "Tesla",
                Descripcion = ""
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Descripcion"));
        }

        /// <summary>
        /// Prueba que MarcaAutoCreateDto con descripción muy larga genera error de validación
        /// </summary>
        [Fact]
        public void MarcaAutoCreateDto_ConDescripcionMuyLarga_DebeGenerarErrorValidacion()
        {
            // Arrange - Descripción de más de 500 caracteres
            var descripcionLarga = new string('B', 501);
            var dto = new MarcaAutoCreateDto
            {
                Nombre = "Tesla",
                Descripcion = descripcionLarga
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Descripcion"));
        }

        /// <summary>
        /// Prueba que MarcaAutoUpdateDto con datos válidos no genera errores de validación
        /// </summary>
        [Fact]
        public void MarcaAutoUpdateDto_ConDatosValidos_NoDebeGenerarErroresValidacion()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 1,
                Nombre = "Tesla",
                Descripcion = "Marca de vehículos eléctricos"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        /// <summary>
        /// Prueba que MarcaAutoUpdateDto con ID cero no genera error de validación
        /// (ya que Required no valida cero para enteros por defecto)
        /// </summary>
        [Fact]
        public void MarcaAutoUpdateDto_ConIdCero_NoDebeGenerarErrorValidacion()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 0,
                Nombre = "Tesla",
                Descripcion = "Descripción válida"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        /// <summary>
        /// Prueba que MarcaAutoUpdateDto mantiene las mismas validaciones que Create para nombre
        /// </summary>
        [Fact]
        public void MarcaAutoUpdateDto_ConNombreInvalido_DebeGenerarErrorValidacion()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 1,
                Nombre = "",
                Descripcion = "Descripción válida"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Nombre"));
        }

        /// <summary>
        /// Prueba que MarcaAutoUpdateDto mantiene las mismas validaciones que Create para descripción
        /// </summary>
        [Fact]
        public void MarcaAutoUpdateDto_ConDescripcionInvalida_DebeGenerarErrorValidacion()
        {
            // Arrange
            var dto = new MarcaAutoUpdateDto
            {
                Id = 1,
                Nombre = "Tesla",
                Descripcion = ""
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Descripcion"));
        }

        /// <summary>
        /// Método helper para validar modelos usando DataAnnotations
        /// </summary>
        /// <param name="model">El modelo a validar</param>
        /// <returns>Lista de resultados de validación</returns>
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}