using RestauranteApi.Entidades;

namespace RestauranteApi.Servicios.Interfaces
{
    public interface ITurnoServicio
    {
        Task<List<Turno>> ObtenerTodos();
        Task<Turno?> ObtenerPorId(int id);
        Task<Turno> Crear(Turno turno);
        Task<bool> Actualizar(int id, Turno turno);
        Task<bool> Eliminar(int id);
    }
}