using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;

namespace RestauranteApi
{
    public class RestauranteDbContext : DbContext
    {
        public RestauranteDbContext(DbContextOptions<RestauranteDbContext> options)
            : base(options) { }

        // ═══════════════════════════════════
        // TABLAS (una por cada entidad)
        // ═══════════════════════════════════
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<BloqueoMesa> BloqueosMesa { get; set; }
        public DbSet<ListaEspera> ListaEspera { get; set; }

        // ═══════════════════════════════════
        // DATOS DE PRUEBA (Seeds)
        // ═══════════════════════════════════
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -- Zonas --
            modelBuilder.Entity<Zona>().HasData(
                new Zona { Id = 1, Nombre = "Terraza", Descripcion = "Zona exterior con vista al jardín" },
                new Zona { Id = 2, Nombre = "Interior", Descripcion = "Zona interior climatizada" },
                new Zona { Id = 3, Nombre = "VIP", Descripcion = "Zona privada premium" }
            );

            // -- Turnos --
            modelBuilder.Entity<Turno>().HasData(
                new Turno { Id = 1, Nombre = "Almuerzo", HoraInicio = new TimeSpan(11, 0, 0), HoraFin = new TimeSpan(15, 0, 0) },
                new Turno { Id = 2, Nombre = "Cena", HoraInicio = new TimeSpan(18, 0, 0), HoraFin = new TimeSpan(23, 0, 0) }
            );

            // -- Mesas --
            modelBuilder.Entity<Mesa>().HasData(
                new Mesa { Id = 1, Numero = "M-01", Capacidad = 4, ZonaId = 1 },
                new Mesa { Id = 2, Numero = "M-02", Capacidad = 2, ZonaId = 1 },
                new Mesa { Id = 3, Numero = "M-03", Capacidad = 6, ZonaId = 2 },
                new Mesa { Id = 4, Numero = "M-04", Capacidad = 8, ZonaId = 2 },
                new Mesa { Id = 5, Numero = "M-05", Capacidad = 4, ZonaId = 3 }
            );

            // -- Clientes --
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, Nombre = "Juan Pérez", Telefono = "8888-1111", Email = "juan@email.com" },
                new Cliente { Id = 2, Nombre = "María López", Telefono = "8888-2222", Email = "maria@email.com" },
                new Cliente { Id = 3, Nombre = "Carlos Mora", Telefono = "8888-3333", Email = "carlos@email.com" }
            );
        }
    }
}