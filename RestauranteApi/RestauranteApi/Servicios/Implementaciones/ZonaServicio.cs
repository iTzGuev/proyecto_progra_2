using Microsoft.EntityFrameworkCore;
using RestauranteApi.Entidades;
using RestauranteApi.Servicios.Interfaces;

namespace RestauranteApi.Servicios.Implementaciones
{
    public class ZonaServicio : IZonaServicio
    {
        private readonly RestauranteDbContext _context;

        public ZonaServicio(RestauranteDbContext context)
        {
            _context = context;
        }

        // Obtener todas las zonas
        public async Task<List<Zona>> ObtenerTodas()
        {
            return await _context.Zonas
                .Include(z => z.Mesas)  // incluye las mesas de cada zona
                .ToListAsync();
        }

        // Obtener una zona por su Id
        public async Task<Zona?> ObtenerPorId(int id)
        {
            return await _context.Zonas
                .Include(z => z.Mesas)
                .FirstOrDefaultAsync(z => z.Id == id);
        }

        // Crear una zona nueva
        public async Task<Zona> Crear(Zona zona)
        {
            _context.Zonas.Add(zona);
            await _context.SaveChangesAsync();
            return zona;
        }

        // Actualizar una zona existente
        public async Task<bool> Actualizar(int id, Zona zona)
        {
            var existente = await _context.Zonas.FindAsync(id);
            if (existente == null) return false;

            existente.Nombre = zona.Nombre;
            existente.Descripcion = zona.Descripcion;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar una zona
        public async Task<bool> Eliminar(int id)
        {
            var zona = await _context.Zonas.FindAsync(id);
            if (zona == null) return false;

            _context.Zonas.Remove(zona);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}