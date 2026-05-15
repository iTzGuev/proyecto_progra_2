using RestauranteApi.Entidades;

namespace RestauranteApi.Servicios.Interfaces
{
    public interface IZonaServicio
    {
        Task<List<Zona>> ObtenerTodas();
        Task<Zona?> ObtenerPorId(int id);
        Task<Zona> Crear(Zona zona);
        Task<bool> Actualizar(int id, Zona zona);
        Task<bool> Eliminar(int id);
    }
}