using Microsoft.AspNetCore.Mvc;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;
namespace RestauranteApi.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesasController : ControllerBase
    {
        private readonly IMesaServicio _mesaServicio;

        public MesasController(IMesaServicio mesaServicio)
        {
            _mesaServicio = mesaServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var mesas = await _mesaServicio.ObtenerTodas();
            return Ok(mesas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var mesa = await _mesaServicio.ObtenerPorId(id);
            if (mesa == null) return NotFound("Mesa no encontrada");
            return Ok(mesa);
        }

        // GET api/mesas/disponibles?fechaHora=2025-05-10T19:00&personas=4
        [HttpGet("disponibles")]
        public async Task<IActionResult> ObtenerDisponibles(
            [FromQuery] DateTime fechaHora,
            [FromQuery] int personas,
            [FromQuery] int? zonaId = null)
        {
            var mesas = await _mesaServicio.ObtenerDisponibles(fechaHora, personas, zonaId);
            return Ok(mesas);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Mesa mesa)
        {
            var nueva = await _mesaServicio.Crear(mesa);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nueva.Id }, nueva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Mesa mesa)
        {
            var resultado = await _mesaServicio.Actualizar(id, mesa);
            if (!resultado) return NotFound("Mesa no encontrada");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _mesaServicio.Eliminar(id);
            if (!resultado) return NotFound("Mesa no encontrada");
            return NoContent();
        }
    }
}