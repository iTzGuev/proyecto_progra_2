using RestauranteApi.Entidades;

namespace RestauranteApi.Servicios.Interfaces
{
    public interface IListaEsperaServicio
    {
        Task<List<ListaEspera>> ObtenerTodas();
        Task<ListaEspera?> ObtenerPorId(int id);
        Task<ListaEspera> Agregar(ListaEspera solicitud);
        Task<(bool exito, string mensaje)> PromoverAReserva(int listaEsperaId);
        Task<List<ListaEspera>> ObtenerPendientes();
    }
}