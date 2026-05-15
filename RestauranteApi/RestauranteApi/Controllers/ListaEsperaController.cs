using Microsoft.AspNetCore.Mvc;
using RestauranteApi.DTOs;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListaEsperaController : ControllerBase
    {
        private readonly IListaEsperaServicio _listaEsperaServicio;

        public ListaEsperaController(IListaEsperaServicio listaEsperaServicio)
        {
            _listaEsperaServicio = listaEsperaServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var lista = await _listaEsperaServicio.ObtenerTodas();

            var dto = lista.Select(l => new ListaEsperaDTO
            {
                Id = l.Id,
                FechaSolicitud = l.FechaSolicitud,
                FechaDeseada = l.FechaDeseada,
                NumeroPersonas = l.NumeroPersonas,
                Atendido = l.Atendido,
                NombreCliente = l.Cliente?.Nombre ?? "Sin cliente",
                NombreZona = l.Zona?.Nombre
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var l = await _listaEsperaServicio.ObtenerPorId(id);
            if (l == null) return NotFound("Solicitud no encontrada");

            var dto = new ListaEsperaDTO
            {
                Id = l.Id,
                FechaSolicitud = l.FechaSolicitud,
                FechaDeseada = l.FechaDeseada,
                NumeroPersonas = l.NumeroPersonas,
                Atendido = l.Atendido,
                NombreCliente = l.Cliente?.Nombre ?? "Sin cliente",
                NombreZona = l.Zona?.Nombre
            };

            return Ok(dto);
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPendientes()
        {
            var pendientes = await _listaEsperaServicio.ObtenerPendientes();

            var dto = pendientes.Select(l => new ListaEsperaDTO
            {
                Id = l.Id,
                FechaSolicitud = l.FechaSolicitud,
                FechaDeseada = l.FechaDeseada,
                NumeroPersonas = l.NumeroPersonas,
                Atendido = l.Atendido,
                NombreCliente = l.Cliente?.Nombre ?? "Sin cliente",
                NombreZona = l.Zona?.Nombre
            }).ToList();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] CrearListaEsperaDTO dto)
        {
            var solicitud = new ListaEspera
            {
                ClienteId = dto.ClienteId,
                FechaDeseada = dto.FechaDeseada,
                NumeroPersonas = dto.NumeroPersonas,
                ZonaId = dto.ZonaId
            };

            var nueva = await _listaEsperaServicio.Agregar(solicitud);
            return Ok(new { mensaje = "Agregado a lista de espera", id = nueva.Id });
        }

        [HttpPut("{id}/promover")]
        public async Task<IActionResult> PromoverAReserva(int id)
        {
            var (exito, mensaje) = await _listaEsperaServicio.PromoverAReserva(id);
            if (!exito) return BadRequest(mensaje);
            return Ok(mensaje);
        }
    }
}