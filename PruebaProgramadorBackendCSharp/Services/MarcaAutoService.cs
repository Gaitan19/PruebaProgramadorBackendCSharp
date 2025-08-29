using PruebaProgramadorBackendCSharp.Models;
using PruebaProgramadorBackendCSharp.Repositories;

namespace PruebaProgramadorBackendCSharp.Services
{
    public class MarcaAutoService : IMarcaAutoService
    {
        private readonly IMarcaAutoRepository _MarcaAutoRepository;
        public MarcaAutoService(IMarcaAutoRepository repo)
        {
            _MarcaAutoRepository = repo;
        }

        public async Task<IEnumerable<MarcaAuto>> ObtenerTodasAsync()
        {
            return await _MarcaAutoRepository.GetAllAsync();
        }

        public async Task<MarcaAuto?> ObtenerPorIdAsync(int id)
        {
            return await _MarcaAutoRepository.GetByIdAsync(id);
        }
    }
}
