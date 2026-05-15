using Microsoft.EntityFrameworkCore;
using RestauranteApi;
using RestauranteApi.Servicios.Implementaciones;
using RestauranteApi.Servicios.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Base de datos en memoria
builder.Services.AddDbContext<RestauranteDbContext>(options =>
    options.UseInMemoryDatabase("RestauranteDB"));

builder.Services.AddScoped<IClienteServicio, ClienteServicio>();
builder.Services.AddScoped<IZonaServicio, ZonaServicio>();
builder.Services.AddScoped<ITurnoServicio, TurnoServicio>();
builder.Services.AddScoped<IMesaServicio, MesaServicio>();
builder.Services.AddScoped<IReservaServicio, ReservaServicio>();
builder.Services.AddScoped<IBloqueoServicio, BloqueoServicio>();
builder.Services.AddScoped<IListaEsperaServicio, ListaEsperaServicio>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
var app = builder.Build();

// Cargar los seeds al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RestauranteDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.Run();