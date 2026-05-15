namespace RestauranteApi.Entidades
{
    public enum EstadoReserva
    {
        Activa,
        Cancelada,
        Atendida,
        NoShow
    }

    public class Reserva
    {
        public int Id { set; get; }
        public DateTime FechaHora { get; set; }

        public int NumeroPersonas { get; set; }
        
        public EstadoReserva Estado { get; set; } = EstadoReserva.Activa;

        public string? Notas { get; set; }

        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; } = null!;

        public int MesaId { get; set; }
        public Mesa? Mesa { get; set; } = null!;

        public int TurnoId { get; set; }
        public Turno? Turno { get; set; } = null!;



    }

}
