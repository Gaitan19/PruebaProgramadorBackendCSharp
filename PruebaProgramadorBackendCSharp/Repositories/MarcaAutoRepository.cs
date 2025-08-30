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

        public async Task AddAsync(MarcaAuto marca)
        {
            await _context.MarcasAutos.AddAsync(marca);
        }

        public void Update(MarcaAuto marca)
        {
            _context.MarcasAutos.Update(marca);
        }

        public void Delete(MarcaAuto marca)
        {
            _context.MarcasAutos.Remove(marca);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}
