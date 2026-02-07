using API.Domain.Entities;

namespace API.Domain.Repos
{
    /// <summary>
    /// Contrato de acceso a datos para la tabla Productos.
    /// Define las operaciones de consulta sobre productos del catálogo.
    /// </summary>
    public interface IProductoRepository
    {
        /// <summary>Obtiene todos los productos registrados.</summary>
        /// <returns>Lista completa de productos.</returns>
        List<Producto> GetAll();

        /// <summary>Obtiene un producto por su identificador.</summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>El producto encontrado o null si no existe.</returns>
        Producto? GetById(int id);

        /// <summary>
        /// Obtiene únicamente el nombre de un producto por su identificador.
        /// Optimizado para construir DTOs sin cargar la entidad completa.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Nombre del producto o null si no existe.</returns>
        string? GetNombreById(int id);
    }
}
