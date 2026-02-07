using API.Domain.DTOs;

namespace API.Domain.Interfaces
{
    /// <summary>
    /// Contrato que define las operaciones de negocio para la consulta de productos.
    /// </summary>
    public interface IProductoUseCase
    {
        /// <summary>Obtiene la lista de todos los productos activos.</summary>
        /// <returns>Lista de productos activos.</returns>
        List<ProductoDTO> GetAllProductos();

        /// <summary>Obtiene los datos de un producto específico.</summary>
        /// <param name="productoId">Identificador del producto.</param>
        /// <returns>Datos del producto.</returns>
        ProductoDTO GetProductoById(int productoId);
    }
}
