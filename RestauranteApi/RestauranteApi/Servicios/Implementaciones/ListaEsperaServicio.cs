using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Servicios.Implementaciones
{
    public class ListaEsperaServicio : IListaEsperaServicio
    {
        private readonly RestauranteDbContext _context;

        public ListaEsperaServicio(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListaEspera>> ObtenerTodas()
        {
            return await _context.ListaEspera
                .Include(l => l.Cliente)
                .Include(l => l.Zona)
                .ToListAsync();
        }

        public async Task<ListaEspera?> ObtenerPorId(int id)
        {
            return await _context.ListaEspera
                .Include(l => l.Cliente)
                .Include(l => l.Zona)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<ListaEspera>> ObtenerPendientes()
        {
            return await _context.ListaEspera
                .Include(l => l.Cliente)
                .Include(l => l.Zona)
                .Where(l => !l.Atendido)
                .ToListAsync();
        }

        public async Task<ListaEspera> Agregar(ListaEspera solicitud)
        {
            solicitud.FechaSolicitud = DateTime.Now;
            solicitud.Atendido = false;
            _context.ListaEspera.Add(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        // ⭐ MÉTODO DE NEGOCIO — promueve la primera solicitud pendiente a reserva
        public async Task<(bool exito, string mensaje)> PromoverAReserva(int listaEsperaId)
        {
            // 1. Buscar la solicitud
            var solicitud = await _context.ListaEspera
                .FirstOrDefaultAsync(l => l.Id == listaEsperaId && !l.Atendido);

            if (solicitud == null)
                return (false, "Solicitud no encontrada o ya fue atendida");

            // 2. Buscar un turno válido para la fecha deseada
            var hora = solicitud.FechaDeseada.TimeOfDay;
            var turno = await _context.Turnos
                .FirstOrDefaultAsync(t => t.HoraInicio <= hora && t.HoraFin >= hora);

            if (turno == null)
                return (false, "No hay turno válido para la hora deseada");

            // 3. Buscar una mesa disponible
            var mesas = await _context.Mesas
                .Include(m => m.Reservas)
                .Include(m => m.Bloqueos)
                .Where(m =>
                    m.Activa &&
                    m.Capacidad >= solicitud.NumeroPersonas &&
                    (solicitud.ZonaId == null || m.ZonaId == solicitud.ZonaId)
                )
                .ToListAsync();

            var mesaDisponible = mesas.FirstOrDefault(m =>
                !m.Reservas.Any(r =>
                    r.Estado == EstadoReserva.Activa &&
                    r.FechaHora < solicitud.FechaDeseada.AddHours(2) &&
                    r.FechaHora.AddHours(2) > solicitud.FechaDeseada
                ) &&
                !m.Bloqueos.Any(b =>
                    b.Inicio < solicitud.FechaDeseada.AddHours(2) &&
                    b.Fin > solicitud.FechaDeseada
                )
            );

            if (mesaDisponible == null)
                return (false, "No hay mesas disponibles para esa fecha y hora");

            // 4. Crear la reserva
            var reserva = new Reserva
            {
                ClienteId = solicitud.ClienteId,
                MesaId = mesaDisponible.Id,
                TurnoId = turno.Id,
                FechaHora = solicitud.FechaDeseada,
                NumeroPersonas = solicitud.NumeroPersonas,
                Estado = EstadoReserva.Activa,
                Notas = "Promovida desde lista de espera"
            };

            _context.Reservas.Add(reserva);

            // 5. Marcar la solicitud como atendida
            solicitud.Atendido = true;
            await _context.SaveChangesAsync();

            return (true, $"Reserva creada en mesa {mesaDisponible.Numero}");
        }
    }
}