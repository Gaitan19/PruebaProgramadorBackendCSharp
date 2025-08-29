using PruebaProgramadorBackendCSharp.Models;

namespace PruebaProgramadorBackendCSharp.Services
{
    public interface IMarcaAutoService
    {
        Task<IEnumerable<MarcaAuto>> ObtenerTodasAsync();
        Task<MarcaAuto?> ObtenerPorIdAsync(int id);
    }
}
