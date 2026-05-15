using Microsoft.AspNetCore.Mvc;
using RestauranteApi.DTOs;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaServicio _reservaServicio;

        public ReservasController(IReservaServicio reservaServicio)
        {
            _reservaServicio = reservaServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var reservas = await _reservaServicio.ObtenerTodas();

            // Convertir entidades a DTOs
            var dto = reservas.Select(r => new ReservaDTO
            {
                Id = r.Id,
                FechaHora = r.FechaHora,
                NumeroPersonas = r.NumeroPersonas,
                Estado = r.Estado.ToString(),
                Notas = r.Notas,
                NombreCliente = r.Cliente?.Nombre ?? "Sin cliente",
                NumeroMesa = r.Mesa?.Numero ?? "Sin mesa",
                NombreTurno = r.Turno?.Nombre ?? "Sin turno"
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var r = await _reservaServicio.ObtenerPorId(id);
            if (r == null) return NotFound("Reserva no encontrada");

            var dto = new ReservaDTO
            {
                Id = r.Id,
                FechaHora = r.FechaHora,
                NumeroPersonas = r.NumeroPersonas,
                Estado = r.Estado.ToString(),
                Notas = r.Notas,
                NombreCliente = r.Cliente?.Nombre ?? "Sin cliente",
                NumeroMesa = r.Mesa?.Numero ?? "Sin mesa",
                NombreTurno = r.Turno?.Nombre ?? "Sin turno"
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearReservaDTO dto)
        {
            // Convertir DTO a entidad
            var reserva = new Reserva
            {
                ClienteId = dto.ClienteId,
                MesaId = dto.MesaId,
                TurnoId = dto.TurnoId,
                FechaHora = dto.FechaHora,
                NumeroPersonas = dto.NumeroPersonas,
                Notas = dto.Notas
            };

            var (exito, mensaje, nueva) = await _reservaServicio.Crear(reserva);
            if (!exito) return BadRequest(mensaje);

            return Ok(new { mensaje, id = nueva!.Id });
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var resultado = await _reservaServicio.Cancelar(id);
            if (!resultado) return BadRequest("No se puede cancelar la reserva");
            return NoContent();
        }

        [HttpPut("{id}/atendida")]
        public async Task<IActionResult> MarcarAtendida(int id)
        {
            var resultado = await _reservaServicio.MarcarAtendida(id);
            if (!resultado) return BadRequest("No se puede marcar como atendida");
            return NoContent();
        }
    }
}