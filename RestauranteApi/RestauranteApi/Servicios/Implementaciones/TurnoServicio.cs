using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;
namespace RestauranteApi.Servicios.Implementaciones
{
    public class TurnoServicio : ITurnoServicio
    {
        private readonly RestauranteDbContext _context;
        public TurnoServicio(RestauranteDbContext context)
        {
            _context = context;
        }
        public async Task<List<Turno>> ObtenerTodos()
        {
            return await _context.Turnos.ToListAsync();

        }
        public async Task<Turno?> ObtenerPorId(int id)
        {
            return await _context.Turnos.FindAsync(id);
        }
        public async Task<Turno> Crear(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
            return turno;
        }
        public async Task<bool> Actualizar(int id, Turno turno)
        {
            var existente = await _context.Turnos.FindAsync(id);
            if (existente == null) return false;
            existente.HoraInicio = turno.HoraInicio;
            existente.HoraFin = turno.HoraFin;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Eliminar(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return false;
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
