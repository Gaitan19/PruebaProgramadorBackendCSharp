using Microsoft.EntityFrameworkCore;
using PruebaProgramadorBackendCSharp.Data;
using PruebaProgramadorBackendCSharp.Models;

namespace PruebaProgramadorBackendCSharp.Repositories
{
    public class MarcaAutoRepository : IMarcaAutoRepository
    {
        private readonly PruebaDbContext _context;
        public MarcaAutoRepository(PruebaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MarcaAuto>> GetAllAsync()
        {
            return await _context.MarcasAutos.AsNoTracking().ToListAsync();
        }

        public async Task<MarcaAuto?> GetByIdAsync(int id)
        {
            return await _context.MarcasAutos.FindAsync(id);
        }

       
    }
}
