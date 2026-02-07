using API.Domain.Entities;

namespace API.Domain.Repos
{
    /// <summary>
    /// Contrato de acceso a datos para la tabla Proveedores.
    /// Define las operaciones de consulta sobre proveedores.
    /// </summary>
    public interface IProveedorRepository
    {
        /// <summary>Obtiene todos los proveedores registrados.</summary>
        /// <returns>Lista completa de proveedores.</returns>
        List<Proveedor> GetAll();

        /// <summary>Obtiene un proveedor por su identificador.</summary>
        /// <param name="id">Identificador del proveedor.</param>
        /// <returns>El proveedor encontrado o null si no existe.</returns>
        Proveedor? GetById(int id);

        /// <summary>
        /// Obtiene únicamente el nombre de un proveedor por su identificador.
        /// Optimizado para construir DTOs sin cargar la entidad completa.
        /// </summary>
        /// <param name="id">Identificador del proveedor.</param>
        /// <returns>Nombre del proveedor o null si no existe.</returns>
        string? GetNombreById(int id);
    }
}
