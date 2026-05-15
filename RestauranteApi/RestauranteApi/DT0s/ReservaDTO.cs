namespace RestauranteApi.DTOs
{
    public class ReservaDTO
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int NumeroPersonas { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Notas { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string NumeroMesa { get; set; } = string.Empty;
        public string NombreTurno { get; set; } = string.Empty;
    }

    public class CrearReservaDTO
    {
        public int ClienteId { get; set; }
        public int MesaId { get; set; }
        public int TurnoId { get; set; }
        public DateTime FechaHora { get; set; }
        public int NumeroPersonas { get; set; }
        public string? Notas { get; set; }
    }
}