using PruebaProgramadorBackendCSharp.DTOs;
using PruebaProgramadorBackendCSharp.Models;

namespace PruebaProgramadorBackendCSharp.Services
{
    public interface IMarcaAutoService
    {
        Task<IEnumerable<MarcaAuto>> ObtenerTodasAsync();
        Task<MarcaAuto?> ObtenerPorIdAsync(int id);
        Task<MarcaAuto> CrearAsync(MarcaAutoCreateDto dto);
        Task<MarcaAuto> ActualizarAsync(MarcaAutoUpdateDto dto);
        Task EliminarAsync(int id);
    }
}
