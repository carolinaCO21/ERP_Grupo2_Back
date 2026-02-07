using API.Domain.DTOs;
using API.Domain.Exceptions;
using API.Domain.Interfaces;
using API.Domain.Repos;

namespace API.Domain.UseCases
{
    /// <summary>
    /// Implementa la lógica de negocio para la consulta de productos del catálogo.
    /// </summary>
    public class ProductoUseCase : IProductoUseCase
    {
        private readonly IProductoRepository _productoRepository;

        /// <summary>
        /// Inicializa el caso de uso con la dependencia del repositorio de productos.
        /// </summary>
        public ProductoUseCase(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        /// <summary>
        /// Obtiene la lista de todos los productos activos del catálogo.
        /// </summary>
        /// <returns>Lista de productos activos transformados a DTO.</returns>
        public List<ProductoDTO> GetAllProductos()
        {
            var productos = _productoRepository.GetAll();

            return productos
                .Where(p => p.Activo)
                .Select(p => new ProductoDTO(p))
                .ToList();
        }

        /// <summary>
        /// Obtiene los datos de un producto específico por su identificador.
        /// </summary>
        /// <param name="productoId">Identificador del producto.</param>
        /// <returns>Datos del producto transformados a DTO.</returns>
        /// <exception cref="EntityNotFoundException">Si el producto no existe.</exception>
        public ProductoDTO GetProductoById(int productoId)
        {
            var producto = _productoRepository.GetById(productoId)
                ?? throw new EntityNotFoundException("Producto", productoId);

            return new ProductoDTO(producto);
        }
    }
}
