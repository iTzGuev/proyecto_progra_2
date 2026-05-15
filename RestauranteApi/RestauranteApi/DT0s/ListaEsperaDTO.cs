namespace RestauranteApi.DTOs
{
    public class ListaEsperaDTO
    {
        public int Id { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaDeseada { get; set; }
        public int NumeroPersonas { get; set; }
        public bool Atendido { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string? NombreZona { get; set; }
    }

    public class CrearListaEsperaDTO
    {
        public int ClienteId { get; set; }
        public DateTime FechaDeseada { get; set; }
        public int NumeroPersonas { get; set; }
        public int? ZonaId { get; set; }
    }
}