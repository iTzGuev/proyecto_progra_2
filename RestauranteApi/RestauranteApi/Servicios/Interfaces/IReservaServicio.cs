using RestauranteApi.Entidades;

namespace RestauranteApi.Servicios.Interfaces
{
    public interface IReservaServicio
    {
        Task<List<Reserva>> ObtenerTodas();
        Task<Reserva?> ObtenerPorId(int id);
        Task<(bool exito, string mensaje, Reserva? reserva)> Crear(Reserva reserva);
        Task<bool> Cancelar(int id);
        Task<bool> MarcarAtendida(int id);
    }
}