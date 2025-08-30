using System.ComponentModel.DataAnnotations;

namespace PruebaProgramadorBackendCSharp.DTOs
{
    public class MarcaAutoUpdateDto
    {
        [Required(ErrorMessage = "El ID es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no debe superar los 500 caracteres")]
        public string Descripcion { get; set; } = null!;
    }
}
