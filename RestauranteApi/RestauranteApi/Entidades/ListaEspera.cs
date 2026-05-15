using RestauranteApi.Entidades;

public class ListaEspera
{
    private Cliente? cliente;

    public int Id { get; set; }

    public DateTime FechaSolicitud { get; set; } = DateTime.Now;
   
    public DateTime FechaDeseada { get; set; }
    

    public int NumeroPersonas { get; set; }

    public bool Atendido { get; set; } = false;

    public int ClienteId { get; set; }
    public Cliente? Cliente { get => cliente; set => cliente = value; }

    public int? ZonaId { get; set; }

    public Zona? Zona { get; set; }
}