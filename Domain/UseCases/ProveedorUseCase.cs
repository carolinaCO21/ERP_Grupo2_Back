using API.Domain.DTOs;
using API.Domain.Exceptions;
using API.Domain.Interfaces;
using API.Domain.Repos;

namespace API.Domain.UseCases
{
    /// <summary>
    /// Implementa la lógica de negocio para la consulta de proveedores.
    /// Coordina los repositorios necesarios y construye los DTOs de respuesta.
    /// </summary>
    public class ProveedorUseCase : IProveedorUseCase
    {
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IProductoProveedorRepository _productoProveedorRepository;
        private readonly IProductoRepository _productoRepository;

        /// <summary>
        /// Inicializa el caso de uso con las dependencias de repositorios inyectadas.
        /// </summary>
        public ProveedorUseCase(
            IProveedorRepository proveedorRepository,
            IProductoProveedorRepository productoProveedorRepository,
            IProductoRepository productoRepository)
        {
            _proveedorRepository = proveedorRepository;
            _productoProveedorRepository = productoProveedorRepository;
            _productoRepository = productoRepository;
        }

        /// <summary>
        /// Obtiene la lista de todos los proveedores activos del sistema.
        /// </summary>
        /// <returns>Lista de proveedores activos transformados a DTO.</returns>
        public List<ProveedorDTO> GetAllProveedores()
        {
            var proveedores = _proveedorRepository.GetAll();

            return proveedores
                .Where(p => p.Activo)
                .Select(p => new ProveedorDTO(p))
                .ToList();
        }

        /// <summary>
        /// Obtiene los datos de un proveedor específico por su identificador.
        /// </summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Datos del proveedor transformados a DTO.</returns>
        /// <exception cref="EntityNotFoundException">Si el proveedor no existe.</exception>
        public ProveedorDTO GetProveedorById(int proveedorId)
        {
            var proveedor = _proveedorRepository.GetById(proveedorId)
                ?? throw new EntityNotFoundException("Proveedor", proveedorId);

            return new ProveedorDTO(proveedor);
        }

        /// <summary>
        /// Obtiene los productos que ofrece un proveedor con sus precios específicos.
        /// Solo incluye relaciones activas.
        /// </summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Lista de productos ofrecidos por el proveedor.</returns>
        /// <exception cref="EntityNotFoundException">Si el proveedor no existe.</exception>
        public List<ProductoProveedorDTO> GetProductosByProveedor(int proveedorId)
        {
            _ = _proveedorRepository.GetById(proveedorId)
                ?? throw new EntityNotFoundException("Proveedor", proveedorId);

            var relaciones = _productoProveedorRepository.GetByProveedorId(proveedorId);
            var resultado = new List<ProductoProveedorDTO>();

            foreach (var relacion in relaciones.Where(r => r.Activo))
            {
                var producto = _productoRepository.GetById(relacion.IdProducto);

                if (producto != null && producto.Activo)
                {
                    resultado.Add(new ProductoProveedorDTO(relacion, producto.CodigoProducto, producto.Nombre));
                }
            }

            return resultado;
        }
    }
}
