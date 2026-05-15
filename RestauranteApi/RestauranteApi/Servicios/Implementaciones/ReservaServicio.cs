using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Servicios.Implementaciones
{
    public class ReservaServicio : IReservaServicio
    {
        private readonly RestauranteDbContext _context;

        public ReservaServicio(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reserva>> ObtenerTodas()
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .Include(r => r.Turno)
                .ToListAsync();
        }

        public async Task<Reserva?> ObtenerPorId(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .Include(r => r.Turno)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<(bool exito, string mensaje, Reserva? reserva)> Crear(Reserva reserva)
        {
            // 1. Verificar que el cliente existe
            var cliente = await _context.Clientes.FindAsync(reserva.ClienteId);
            if (cliente == null)
                return (false, "El cliente no existe", null);

            // 2. Verificar que la mesa existe y está activa
            var mesa = await _context.Mesas.FindAsync(reserva.MesaId);
            if (mesa == null || !mesa.Activa)
                return (false, "La mesa no existe o no está activa", null);

            // 3. Verificar que el turno existe
            var turno = await _context.Turnos.FindAsync(reserva.TurnoId);
            if (turno == null)
                return (false, "El turno no existe", null);

            // 4. Verificar que la hora está dentro del turno
            var hora = reserva.FechaHora.TimeOfDay;
            if (hora < turno.HoraInicio || hora > turno.HoraFin)
                return (false, $"La hora no está dentro del turno {turno.Nombre} ({turno.HoraInicio}-{turno.HoraFin})", null);

            // 5. Verificar que la mesa tiene capacidad suficiente
            if (mesa.Capacidad < reserva.NumeroPersonas)
                return (false, $"La mesa solo tiene capacidad para {mesa.Capacidad} personas", null);

            // 6. Verificar que no hay otra reserva activa en la misma mesa y horario
            var hayConflicto = await _context.Reservas
                .AnyAsync(r =>
                    r.MesaId == reserva.MesaId &&
                    r.Estado == EstadoReserva.Activa &&
                    r.FechaHora < reserva.FechaHora.AddHours(2) &&
                    r.FechaHora.AddHours(2) > reserva.FechaHora
                );
            if (hayConflicto)
                return (false, "La mesa ya tiene una reserva activa en ese horario", null);

            // 7. Verificar que no hay bloqueo en ese horario
            var hayBloqueo = await _context.BloqueosMesa
                .AnyAsync(b =>
                    b.MesaId == reserva.MesaId &&
                    b.Inicio < reserva.FechaHora.AddHours(2) &&
                    b.Fin > reserva.FechaHora
                );
            if (hayBloqueo)
                return (false, "La mesa está bloqueada en ese horario", null);

            // 8. Todo OK, crear la reserva
            reserva.Estado = EstadoReserva.Activa;
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return (true, "Reserva creada exitosamente", reserva);
        }

        public async Task<bool> Cancelar(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null || reserva.Estado != EstadoReserva.Activa)
                return false;

            reserva.Estado = EstadoReserva.Cancelada;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarcarAtendida(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null || reserva.Estado != EstadoReserva.Activa)
                return false;

            reserva.Estado = EstadoReserva.Atendida;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}