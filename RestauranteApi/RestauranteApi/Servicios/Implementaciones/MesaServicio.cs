using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;
namespace RestauranteApi.Servicios.Implementaciones
{
    public class MesaServicio : IMesaServicio
    {
        private readonly RestauranteDbContext _context;

        public MesaServicio(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mesa>> ObtenerTodas()
        {
            return await _context.Mesas
                .Include(m => m.Zona)
                .ToListAsync();
        }

        public async Task<Mesa?> ObtenerPorId(int id)
        {
            return await _context.Mesas
                .Include(m => m.Zona)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Mesa> Crear(Mesa mesa)
        {
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();
            return mesa;
        }

        public async Task<bool> Actualizar(int id, Mesa mesa)
        {
            var existente = await _context.Mesas.FindAsync(id);
            if (existente == null) return false;

            existente.Numero = mesa.Numero;
            existente.Capacidad = mesa.Capacidad;
            existente.Activa = mesa.Activa;
            existente.ZonaId = mesa.ZonaId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null) return false;

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();
            return true;
        }

        //  MÉTODO DE NEGOCIO — busca mesas disponibles
        public async Task<List<Mesa>> ObtenerDisponibles(DateTime fechaHora, int personas, int? zonaId)
        {
            // 1. Verificar que el horario esté dentro de un turno válido
            var hora = fechaHora.TimeOfDay;
            var turnoValido = await _context.Turnos
                .AnyAsync(t => t.HoraInicio <= hora && t.HoraFin >= hora);

            if (!turnoValido)
                return new List<Mesa>(); // no hay turno en ese horario

            // 2. Buscar mesas que cumplan los criterios
            var mesas = await _context.Mesas
                .Include(m => m.Zona)
                .Include(m => m.Reservas)
                .Include(m => m.Bloqueos)
                .Where(m =>
                    m.Activa &&
                    m.Capacidad >= personas &&
                    (zonaId == null || m.ZonaId == zonaId)
                )
                .ToListAsync();

            // 3. Filtrar las que no tienen reserva ni bloqueo en ese horario
            var disponibles = mesas.Where(m =>
                // No tiene reserva activa en ese horario (asumimos 2 horas por reserva)
                !m.Reservas.Any(r =>
                    r.Estado == EstadoReserva.Activa &&
                    r.FechaHora < fechaHora.AddHours(2) &&
                    r.FechaHora.AddHours(2) > fechaHora
                ) &&
                // No tiene bloqueo en ese horario
                !m.Bloqueos.Any(b =>
                    b.Inicio < fechaHora.AddHours(2) &&
                    b.Fin > fechaHora
                )
            ).ToList();

            return disponibles;
        }
    }
}