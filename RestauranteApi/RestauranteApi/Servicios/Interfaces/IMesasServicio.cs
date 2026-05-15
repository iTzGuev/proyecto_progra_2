using RestauranteApi.Entidades;

namespace RestauranteApi.Servicios.Interfaces
{
    public interface IMesaServicio
    {
        Task<List<Mesa>> ObtenerTodas();
        Task<Mesa?> ObtenerPorId(int id);
        Task<Mesa> Crear(Mesa mesa);
        Task<bool> Actualizar(int id, Mesa mesa);
        Task<bool> Eliminar(int id);
        Task<List<Mesa>> ObtenerDisponibles(DateTime fechaHora, int personas, int? zonaId);
    }
}