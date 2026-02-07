using API.Domain.DTOs;
using API.Domain.Exceptions;
using API.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    /// <summary>
    /// Controlador API para la consulta de proveedores y sus productos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorUseCase _proveedorUseCase;
        private readonly ILogger<ProveedoresController> _logger;

        /// <summary>
        /// Inicializa el controlador con las dependencias inyectadas.
        /// </summary>
        public ProveedoresController(IProveedorUseCase proveedorUseCase, ILogger<ProveedoresController> logger)
        {
            _proveedorUseCase = proveedorUseCase;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los proveedores activos del sistema.
        /// </summary>
        /// <returns>Lista de proveedores activos.</returns>
        /// <response code="200">Lista de proveedores obtenida correctamente.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDTO<List<ProveedorDTO>>), StatusCodes.Status200OK)]
        public ActionResult<ApiResponseDTO<List<ProveedorDTO>>> GetAll()
        {
            var proveedores = _proveedorUseCase.GetAllProveedores();
            return Ok(ApiResponseDTO<List<ProveedorDTO>>.Ok(proveedores, "Proveedores obtenidos correctamente."));
        }

        /// <summary>
        /// Obtiene un proveedor específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor.</param>
        /// <returns>Datos del proveedor.</returns>
        /// <response code="200">Proveedor encontrado.</response>
        /// <response code="404">Proveedor no encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDTO<ProveedorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponseDTO<ProveedorDTO>> GetById(int id)
        {
            try
            {
                var proveedor = _proveedorUseCase.GetProveedorById(id);
                return Ok(ApiResponseDTO<ProveedorDTO>.Ok(proveedor, "Proveedor obtenido correctamente."));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Proveedor no encontrado: {Id}", id);
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene los productos que ofrece un proveedor con sus precios específicos.
        /// </summary>
        /// <param name="id">Identificador del proveedor.</param>
        /// <returns>Lista de productos ofrecidos por el proveedor.</returns>
        /// <response code="200">Productos del proveedor obtenidos correctamente.</response>
        /// <response code="404">Proveedor no encontrado.</response>
        [HttpGet("{id}/productos")]
        [ProducesResponseType(typeof(ApiResponseDTO<List<ProductoProveedorDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponseDTO<List<ProductoProveedorDTO>>> GetProductosByProveedor(int id)
        {
            try
            {
                var productos = _proveedorUseCase.GetProductosByProveedor(id);
                return Ok(ApiResponseDTO<List<ProductoProveedorDTO>>.Ok(productos, "Productos del proveedor obtenidos correctamente."));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Proveedor no encontrado: {Id}", id);
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
