using Microsoft.AspNetCore.Mvc;
using RestauranteApi.Servicios.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly ITurnoServicio _turnoServicio;

    public TurnosController(ITurnoServicio turnoServicio)
    {
        _turnoServicio = turnoServicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var turnos = await _turnoServicio.ObtenerTodos();
        return Ok(turnos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var turno = await _turnoServicio.ObtenerPorId(id);
        if (turno == null) return NotFound("Turno no encontrado");
        return Ok(turno);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Turno turno)
    {
        var nuevo = await _turnoServicio.Crear(turno);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevo.Id }, nuevo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] Turno turno)
    {
        var resultado = await _turnoServicio.Actualizar(id, turno);
        if (!resultado) return NotFound("Turno no encontrado");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _turnoServicio.Eliminar(id);
        if (!resultado) return NotFound("Turno no encontrado");
        return NoContent();
    }
}