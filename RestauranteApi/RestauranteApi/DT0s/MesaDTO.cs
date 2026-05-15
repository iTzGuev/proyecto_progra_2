namespace RestauranteApi.DTOs
{
    public class MesaDTO
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public bool Activa { get; set; }
        public string NombreZona { get; set; } = string.Empty;
    }

    public class CrearMesaDTO
    {
        public string Numero { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public int ZonaId { get; set; }
    }
}