using Microsoft.AspNetCore.Mvc;
using PruebaProgramadorBackendCSharp.Services;

namespace PruebaProgramadorBackendCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasAutosController : ControllerBase
    {
        private readonly IMarcaAutoService _service;
        public MarcasAutosController(IMarcaAutoService service)
        {
            _service = service;
        }

        // GET: api/MarcasAutos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var marcas = await _service.ObtenerTodasAsync();
            return Ok(marcas);
        }

        // GET: api/MarcasAutos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var marca = await _service.ObtenerPorIdAsync(id);
            if (marca == null) return NotFound();
            return Ok(marca);
        }
    }
}
