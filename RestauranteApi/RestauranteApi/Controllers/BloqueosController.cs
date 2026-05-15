using Microsoft.AspNetCore.Mvc;
using RestauranteApi.DTOs;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloqueosController : ControllerBase
    {
        private readonly IBloqueoServicio _bloqueoServicio;

        public BloqueosController(IBloqueoServicio bloqueoServicio)
        {
            _bloqueoServicio = bloqueoServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var bloqueos = await _bloqueoServicio.ObtenerTodos();

            var dto = bloqueos.Select(b => new BloqueoDTO
            {
                Id = b.Id,
                Inicio = b.Inicio,
                Fin = b.Fin,
                Motivo = b.Motivo,
                NumeroMesa = b.Mesa?.Numero ?? "Sin mesa"
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var b = await _bloqueoServicio.ObtenerPorId(id);
            if (b == null) return NotFound("Bloqueo no encontrado");

            var dto = new BloqueoDTO
            {
                Id = b.Id,
                Inicio = b.Inicio,
                Fin = b.Fin,
                Motivo = b.Motivo,
                NumeroMesa = b.Mesa?.Numero ?? "Sin mesa"
            };

            return Ok(dto);
        }

        [HttpGet("mesa/{mesaId}")]
        public async Task<IActionResult> ObtenerPorMesa(int mesaId)
        {
            var bloqueos = await _bloqueoServicio.ObtenerPorMesa(mesaId);

            var dto = bloqueos.Select(b => new BloqueoDTO
            {
                Id = b.Id,
                Inicio = b.Inicio,
                Fin = b.Fin,
                Motivo = b.Motivo,
                NumeroMesa = b.Mesa?.Numero ?? "Sin mesa"
            }).ToList();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearBloqueoDTO dto)
        {
            var bloqueo = new BloqueoMesa
            {
                MesaId = dto.MesaId,
                Inicio = dto.Inicio,
                Fin = dto.Fin,
                Motivo = dto.Motivo
            };

            var (exito, mensaje, nuevo) = await _bloqueoServicio.Crear(bloqueo);
            if (!exito) return BadRequest(mensaje);
            return Ok(new { mensaje, id = nuevo!.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _bloqueoServicio.Eliminar(id);
            if (!resultado) return NotFound("Bloqueo no encontrado");
            return NoContent();
        }
    }
}