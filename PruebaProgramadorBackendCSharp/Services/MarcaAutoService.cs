using PruebaProgramadorBackendCSharp.DTOs;
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
            var MarcaAutoEncontrada = await _MarcaAutoRepository.GetByIdAsync(id);
            if (MarcaAutoEncontrada == null)
                throw new KeyNotFoundException("Marca no encontrada.");

            return MarcaAutoEncontrada;
        }

        public async Task<MarcaAuto> CrearAsync(MarcaAutoCreateDto dto)
        {
            

            var nuevaMarca = new MarcaAuto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                FechaCreacion = DateTime.UtcNow
            };

            await _MarcaAutoRepository.AddAsync(nuevaMarca);
            await _MarcaAutoRepository.SaveChangesAsync();
            return nuevaMarca;
        }

        public async Task<MarcaAuto> ActualizarAsync(MarcaAutoUpdateDto dto)
        {
            if (dto.Id <= 0) throw new ArgumentException("El ID debe ser válido.");

            var existente = await _MarcaAutoRepository.GetByIdAsync(dto.Id);
            if (existente == null)
                throw new KeyNotFoundException("Marca no encontrada.");

            existente.Nombre = dto.Nombre;
            existente.Descripcion = dto.Descripcion;

            _MarcaAutoRepository.Update(existente);
            await _MarcaAutoRepository.SaveChangesAsync();
            return existente;
        }

        public async Task EliminarAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID debe ser válido.");

            var existente = await _MarcaAutoRepository.GetByIdAsync(id);
            if (existente == null)
                throw new KeyNotFoundException("Marca no encontrada.");

            _MarcaAutoRepository.Delete(existente);
            await _MarcaAutoRepository.SaveChangesAsync();
        }
    }
}
