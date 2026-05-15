using RestauranteApi.Entidades;

namespace RestauranteApi.Servicios.Interfaces
{
    public interface IBloqueoServicio
    {
        Task<List<BloqueoMesa>> ObtenerTodos();
        Task<BloqueoMesa?> ObtenerPorId(int id);
        Task<(bool exito, string mensaje, BloqueoMesa? bloqueo)> Crear(BloqueoMesa bloqueo);
        Task<bool> Eliminar(int id);
        Task<List<BloqueoMesa>> ObtenerPorMesa(int mesaId);
    }
}