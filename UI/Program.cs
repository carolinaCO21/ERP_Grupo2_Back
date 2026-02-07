using API.Domain.Interfaces;
using API.Domain.Repos;
using API.Domain.UseCases;
using Data.MockRepos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ── Inyección de Repositorios Mock ─────────────────────────────────────────
// Registramos las implementaciones mock como Singleton para mantener
// los datos en memoria durante toda la vida de la aplicación.

builder.Services.AddSingleton<IUserRepository, MockUserRepository>();
builder.Services.AddSingleton<IProveedorRepository, MockProveedorRepository>();
builder.Services.AddSingleton<IProductoRepository, MockProductoRepository>();
builder.Services.AddSingleton<IProductoProveedorRepository, MockProductoProveedorRepository>();
builder.Services.AddSingleton<IPedidoRepository, MockPedidoRepository>();
builder.Services.AddSingleton<ILineaPedidoRepository, MockLineaPedidoRepository>();

// ── Inyección de Casos de Uso
// Los casos de uso se registran como Scoped para crear una instancia por solicitud HTTP.

builder.Services.AddScoped<IProveedorUseCase, ProveedorUseCase>();
builder.Services.AddScoped<IProductoUseCase, ProductoUseCase>();
builder.Services.AddScoped<IPedidoUseCase, PedidoUseCase>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

