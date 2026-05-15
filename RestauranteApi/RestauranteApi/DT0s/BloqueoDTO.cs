namespace RestauranteApi.DTOs
{
    public class BloqueoDTO
    {
        public int Id { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string NumeroMesa { get; set; } = string.Empty;
    }

    public class CrearBloqueoDTO
    {
        public int MesaId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}