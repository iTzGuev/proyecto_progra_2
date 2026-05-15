using Microsoft.AspNetCore.Mvc;
using RestauranteApi.DTOs;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZonasController : ControllerBase
    {
        private readonly IZonaServicio _zonaServicio;

        public ZonasController(IZonaServicio zonaServicio)
        {
            _zonaServicio = zonaServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var zonas = await _zonaServicio.ObtenerTodas();

            var dto = zonas.Select(z => new ZonaDTO
            {
                Id = z.Id,
                Nombre = z.Nombre,
                Descripcion = z.Descripcion,
                CantidadMesas = z.Mesas.Count
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var z = await _zonaServicio.ObtenerPorId(id);
            if (z == null) return NotFound("Zona no encontrada");

            var dto = new ZonaDTO
            {
                Id = z.Id,
                Nombre = z.Nombre,
                Descripcion = z.Descripcion,
                CantidadMesas = z.Mesas.Count
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearZonaDTO dto)
        {
            var zona = new Zona
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            var nueva = await _zonaServicio.Crear(zona);
            return Ok(new { mensaje = "Zona creada", id = nueva.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CrearZonaDTO dto)
        {
            var zona = new Zona
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            var resultado = await _zonaServicio.Actualizar(id, zona);
            if (!resultado) return NotFound("Zona no encontrada");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _zonaServicio.Eliminar(id);
            if (!resultado) return NotFound("Zona no encontrada");
            return NoContent();
        }
    }
}