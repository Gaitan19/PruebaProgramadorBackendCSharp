using Microsoft.AspNetCore.Mvc;
using PruebaProgramadorBackendCSharp.DTOs;
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
            try
            {
                var marca = await _service.ObtenerPorIdAsync(id);
                
                return Ok(marca);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/MarcasAutos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MarcaAutoCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var nueva = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = nueva.Id }, nueva);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/MarcasAutos/5

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] MarcaAutoUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest(new { error = "El ID de la URL no coincide con el del cuerpo." });

            try
            {
                var actualizado = await _service.ActualizarAsync(dto);
                return Ok(actualizado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/MarcasAutos/5

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.EliminarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
