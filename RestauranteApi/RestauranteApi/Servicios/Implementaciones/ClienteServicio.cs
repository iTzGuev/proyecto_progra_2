using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Servicios.Implementaciones
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly RestauranteDbContext _context;

        public ClienteServicio(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> ObtenerTodos()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorId(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente> Crear(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> Actualizar(int id, Cliente cliente)
        {
            var existente = await _context.Clientes.FindAsync(id);
            if (existente == null) return false;

            existente.Nombre = cliente.Nombre;
            existente.Telefono = cliente.Telefono;
            existente.Email = cliente.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}