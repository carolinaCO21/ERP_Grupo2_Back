using API.Domain.DTOs;
using API.Domain.Exceptions;
using API.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    /// <summary>
    /// Controlador API para la consulta de productos del catálogo.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoUseCase _productoUseCase;
        private readonly ILogger<ProductosController> _logger;

        /// <summary>
        /// Inicializa el controlador con las dependencias inyectadas.
        /// </summary>
        public ProductosController(IProductoUseCase productoUseCase, ILogger<ProductosController> logger)
        {
            _productoUseCase = productoUseCase;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los productos activos del catálogo.
        /// </summary>
        /// <returns>Lista de productos activos.</returns>
        /// <response code="200">Lista de productos obtenida correctamente.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDTO<List<ProductoDTO>>), StatusCodes.Status200OK)]
        public ActionResult<ApiResponseDTO<List<ProductoDTO>>> GetAll()
        {
            var productos = _productoUseCase.GetAllProductos();
            return Ok(ApiResponseDTO<List<ProductoDTO>>.Ok(productos, "Productos obtenidos correctamente."));
        }

        /// <summary>
        /// Obtiene un producto específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Datos del producto.</returns>
        /// <response code="200">Producto encontrado.</response>
        /// <response code="404">Producto no encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDTO<ProductoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponseDTO<ProductoDTO>> GetById(int id)
        {
            try
            {
                var producto = _productoUseCase.GetProductoById(id);
                return Ok(ApiResponseDTO<ProductoDTO>.Ok(producto, "Producto obtenido correctamente."));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Producto no encontrado: {Id}", id);
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
