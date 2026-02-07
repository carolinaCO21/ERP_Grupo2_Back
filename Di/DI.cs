using API.Domain.Interfaces;
using API.Domain.Repos;
using API.Domain.UseCases;
using Data.MockRepos;
using Microsoft.Extensions.DependencyInjection;

/// Centraliza el registro de todos los servicios de la aplicación.
/// </summary>
public static class DI
{
    /// <summary>
    /// Registra todos los servicios de la aplicación en el contenedor de DI.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <returns>La colección de servicios para encadenamiento.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // ── Repositorios Mock ─────────────────────────────────────────
        // Singleton para mantener datos en memoria durante toda la vida de la app.
        services.AddSingleton<IUserRepository, MockUserRepository>();
        services.AddSingleton<IProveedorRepository, MockProveedorRepository>();
        services.AddSingleton<IProductoRepository, MockProductoRepository>();
        services.AddSingleton<IProductoProveedorRepository, MockProductoProveedorRepository>();
        services.AddSingleton<IPedidoRepository, MockPedidoRepository>();
        services.AddSingleton<ILineaPedidoRepository, MockLineaPedidoRepository>();

        // ── Casos de Uso ──────────────────────────────────────────────
        // Scoped para crear una instancia por solicitud HTTP.
        services.AddScoped<IProveedorUseCase, ProveedorUseCase>();
        services.AddScoped<IProductoUseCase, ProductoUseCase>();
        services.AddScoped<IPedidoUseCase, PedidoUseCase>();

        return services;
    }
}
}