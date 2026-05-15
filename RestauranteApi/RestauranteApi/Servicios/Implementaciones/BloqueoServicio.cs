using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Servicios.Implementaciones
{
    public class BloqueoServicio : IBloqueoServicio
    {
        private readonly RestauranteDbContext _context;

        public BloqueoServicio(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<List<BloqueoMesa>> ObtenerTodos()
        {
            return await _context.BloqueosMesa
                .Include(b => b.Mesa)
                .ToListAsync();
        }

        public async Task<BloqueoMesa?> ObtenerPorId(int id)
        {
            return await _context.BloqueosMesa
                .Include(b => b.Mesa)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<BloqueoMesa>> ObtenerPorMesa(int mesaId)
        {
            return await _context.BloqueosMesa
                .Include(b => b.Mesa)
                .Where(b => b.MesaId == mesaId)
                .ToListAsync();
        }

        public async Task<(bool exito, string mensaje, BloqueoMesa? bloqueo)> Crear(BloqueoMesa bloqueo)
        {
            // 1. Verificar que la mesa existe
            var mesa = await _context.Mesas.FindAsync(bloqueo.MesaId);
            if (mesa == null)
                return (false, "La mesa no existe", null);

            // 2. Verificar que no hay reservas activas en ese horario
            var hayReserva = await _context.Reservas
                .AnyAsync(r =>
                    r.MesaId == bloqueo.MesaId &&
                    r.Estado == EstadoReserva.Activa &&
                    r.FechaHora < bloqueo.Fin &&
                    r.FechaHora.AddHours(2) > bloqueo.Inicio
                );
            if (hayReserva)
                return (false, "La mesa tiene reservas activas en ese horario", null);

            // 3. Verificar que no hay otro bloqueo en ese horario
            var hayBloqueo = await _context.BloqueosMesa
                .AnyAsync(b =>
                    b.MesaId == bloqueo.MesaId &&
                    b.Inicio < bloqueo.Fin &&
                    b.Fin > bloqueo.Inicio
                );
            if (hayBloqueo)
                return (false, "La mesa ya tiene un bloqueo en ese horario", null);

            _context.BloqueosMesa.Add(bloqueo);
            await _context.SaveChangesAsync();
            return (true, "Bloqueo creado exitosamente", bloqueo);
        }

        public async Task<bool> Eliminar(int id)
        {
            var bloqueo = await _context.BloqueosMesa.FindAsync(id);
            if (bloqueo == null) return false;

            _context.BloqueosMesa.Remove(bloqueo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}