using API.Domain.Entities;

namespace API.Domain.Repos
{
    /// <summary>
    /// Contrato de acceso a datos para la tabla intermedia Productos_Proveedor.
    /// Permite consultar qué productos ofrece cada proveedor y a qué precio.
    /// </summary>
    public interface IProductoProveedorRepository
    {
        /// <summary>Obtiene todos los productos que ofrece un proveedor.</summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Lista de relaciones producto-proveedor activas.</returns>
        List<ProductoProveedor> GetByProveedorId(int proveedorId);

        /// <summary>
        /// Obtiene la relación entre un proveedor y un producto específico.
        /// Permite verificar si un proveedor ofrece un producto concreto.
        /// </summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <param name="productoId">Identificador del producto.</param>
        /// <returns>La relación encontrada o null si no existe.</returns>
        ProductoProveedor? GetByProveedorAndProducto(int proveedorId, int productoId);
    }
}
