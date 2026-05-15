using RestauranteApi.Entidades;

namespace RestauranteApi.Servicios.Interfaces
{
    public interface IClienteServicio
    {
        Task<List<Cliente>> ObtenerTodos();
        Task<Cliente?> ObtenerPorId(int id);
        Task<Cliente> Crear(Cliente cliente);
        Task<bool> Actualizar(int id, Cliente cliente);
        Task<bool> Eliminar(int id);
    }
}