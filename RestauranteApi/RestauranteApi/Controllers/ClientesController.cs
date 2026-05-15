using Microsoft.AspNetCore.Mvc;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteServicio _clienteServicio;

        public ClientesController(IClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var clientes = await _clienteServicio.ObtenerTodos();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var cliente = await _clienteServicio.ObtenerPorId(id);
            if (cliente == null) return NotFound("Cliente no encontrado");
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Cliente cliente)
        {
            var nuevo = await _clienteServicio.Crear(cliente);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevo.Id }, nuevo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cliente cliente)
        {
            var resultado = await _clienteServicio.Actualizar(id, cliente);
            if (!resultado) return NotFound("Cliente no encontrado");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _clienteServicio.Eliminar(id);
            if (!resultado) return NotFound("Cliente no encontrado");
            return NoContent();
        }
    }
}