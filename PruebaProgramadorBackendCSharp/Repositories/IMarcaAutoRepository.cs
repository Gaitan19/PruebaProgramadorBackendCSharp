using PruebaProgramadorBackendCSharp.Models;

namespace PruebaProgramadorBackendCSharp.Repositories
{
    public interface IMarcaAutoRepository
    {
        Task<IEnumerable<MarcaAuto>> GetAllAsync();
        Task<MarcaAuto?> GetByIdAsync(int id);
      
    }
}
