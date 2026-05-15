using RestauranteApi.Entidades;

public class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<Reserva> Reservas { get; set; } = new();

    public List<ListaEspera> ListasEspera { get; set; } = new();
}