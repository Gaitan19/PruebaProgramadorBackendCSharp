using PruebaProgramadorBackendCSharp.Models;

namespace PruebaProgramadorBackendCSharp.Repositories
{
    public interface IMarcaAutoRepository
    {
        Task<IEnumerable<MarcaAuto>> GetAllAsync();
        Task<MarcaAuto?> GetByIdAsync(int id);
        Task AddAsync(MarcaAuto marca);
        void Update(MarcaAuto marca);
        void Delete(MarcaAuto marca);
        Task SaveChangesAsync();

    }
}
