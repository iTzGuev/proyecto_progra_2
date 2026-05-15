using RestauranteApi.Entidades;

public class Turno
{

    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public List<Reserva> Reservas { get; set; } = new();


}
