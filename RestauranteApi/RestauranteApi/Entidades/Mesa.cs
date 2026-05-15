using RestauranteApi.Entidades;

namespace RestauranteApi.Entidades
{
    public class Mesa
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int Capacidad { get; set; }      // ← solo esta, borrar "capacidad"
        public bool Activa { get; set; } = true;

        public int ZonaId { get; set; }
        public Zona Zona { get; set; } = null!;

        public List<Reserva> Reservas { get; set; } = new();
        public List<BloqueoMesa> Bloqueos { get; set; } = new();
    }
}