using API.Domain.DTOs;

namespace API.Domain.Interfaces
{
    /// <summary>
    /// Contrato que define las operaciones de negocio para la consulta de proveedores.
    /// </summary>
    public interface IProveedorUseCase
    {
        /// <summary>Obtiene la lista de todos los proveedores activos.</summary>
        /// <returns>Lista de proveedores activos.</returns>
        List<ProveedorDTO> GetAllProveedores();

        /// <summary>Obtiene los datos de un proveedor específico.</summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Datos del proveedor.</returns>
        ProveedorDTO GetProveedorById(int proveedorId);

        /// <summary>Obtiene los productos que ofrece un proveedor con sus precios.</summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Lista de productos ofrecidos por el proveedor.</returns>
        List<ProductoProveedorDTO> GetProductosByProveedor(int proveedorId);
    }
}
