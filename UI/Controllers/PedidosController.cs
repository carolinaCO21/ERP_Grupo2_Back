using API.Domain.DTOs;
using API.Domain.Exceptions;
using API.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de pedidos a proveedores.
    /// Expone operaciones CRUD y consultas filtradas sobre pedidos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoUseCase _pedidoUseCase;
        private readonly ILogger<PedidosController> _logger;

        /// <summary>
        /// Inicializa el controlador con las dependencias inyectadas.
        /// </summary>
        public PedidosController(IPedidoUseCase pedidoUseCase, ILogger<PedidosController> logger)
        {
            _pedidoUseCase = pedidoUseCase;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los pedidos del sistema.
        /// </summary>
        /// <returns>Lista de pedidos en formato resumido.</returns>
        /// <response code="200">Lista de pedidos obtenida correctamente.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDTO<List<PedidoListDTO>>), StatusCodes.Status200OK)]
        public ActionResult<ApiResponseDTO<List<PedidoListDTO>>> GetAll()
        {
            var pedidos = _pedidoUseCase.GetAllPedidos();
            return Ok(ApiResponseDTO<List<PedidoListDTO>>.Ok(pedidos, "Pedidos obtenidos correctamente."));
        }

        /// <summary>
        /// Obtiene un pedido específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del pedido.</param>
        /// <returns>Detalle completo del pedido incluyendo sus líneas.</returns>
        /// <response code="200">Pedido encontrado.</response>
        /// <response code="404">Pedido no encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDTO<PedidoDetailDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponseDTO<PedidoDetailDTO>> GetById(int id)
        {
            try
            {
                var pedido = _pedidoUseCase.GetPedidoById(id);
                return Ok(ApiResponseDTO<PedidoDetailDTO>.Ok(pedido, "Pedido obtenido correctamente."));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Pedido no encontrado: {Id}", id);
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene los pedidos filtrados por proveedor.
        /// </summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Lista de pedidos del proveedor.</returns>
        /// <response code="200">Pedidos obtenidos correctamente.</response>
        /// <response code="404">Proveedor no encontrado.</response>
        [HttpGet("proveedor/{proveedorId}")]
        [ProducesResponseType(typeof(ApiResponseDTO<List<PedidoListDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponseDTO<List<PedidoListDTO>>> GetByProveedor(int proveedorId)
        {
            try
            {
                var pedidos = _pedidoUseCase.GetPedidosByProveedor(proveedorId);
                return Ok(ApiResponseDTO<List<PedidoListDTO>>.Ok(pedidos, "Pedidos del proveedor obtenidos correctamente."));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Proveedor no encontrado: {Id}", proveedorId);
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene los pedidos filtrados por estado.
        /// </summary>
        /// <param name="estado">Estado del pedido (Pendiente, Aprobado, EnProceso, Enviado, Recibido, Cancelado).</param>
        /// <returns>Lista de pedidos en el estado indicado.</returns>
        /// <response code="200">Pedidos obtenidos correctamente.</response>
        /// <response code="400">Estado no válido.</response>
        [HttpGet("estado/{estado}")]
        [ProducesResponseType(typeof(ApiResponseDTO<List<PedidoListDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        public ActionResult<ApiResponseDTO<List<PedidoListDTO>>> GetByEstado(string estado)
        {
            try
            {
                var pedidos = _pedidoUseCase.GetPedidosByEstado(estado);
                return Ok(ApiResponseDTO<List<PedidoListDTO>>.Ok(pedidos, $"Pedidos en estado '{estado}' obtenidos correctamente."));
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "Estado no válido: {Estado}", estado);
                return BadRequest(ApiResponseDTO<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// Crea un nuevo pedido a proveedor.
        /// </summary>
        /// <param name="pedidoDto">Datos del pedido a crear.</param>
        /// <returns>Detalle del pedido creado.</returns>
        /// <response code="201">Pedido creado correctamente.</response>
        /// <response code="400">Error de validación o regla de negocio.</response>
        /// <response code="404">Proveedor, usuario o producto no encontrado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDTO<PedidoDetailDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponseDTO<PedidoDetailDTO>> Create([FromBody] PedidoCreateDTO pedidoDto)
        {
            try
            {
                var pedido = _pedidoUseCase.CreatePedido(pedidoDto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = pedido.Id },
                    ApiResponseDTO<PedidoDetailDTO>.Ok(pedido, "Pedido creado correctamente."));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Entidad no encontrada al crear pedido");
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada al crear pedido");
                return BadRequest(ApiResponseDTO<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// Actualiza un pedido existente.
        /// </summary>
        /// <param name="pedidoDto">Datos de actualización del pedido.</param>
        /// <returns>Detalle del pedido actualizado.</returns>
        /// <response code="200">Pedido actualizado correctamente.</response>
        /// <response code="400">Error de validación, regla de negocio o transición de estado inválida.</response>
        /// <response code="404">Pedido no encontrado.</response>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseDTO<PedidoDetailDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponseDTO<PedidoDetailDTO>> Update([FromBody] PedidoUpdateDTO pedidoDto)
        {
            try
            {
                var pedido = _pedidoUseCase.UpdatePedido(pedidoDto);
                return Ok(ApiResponseDTO<PedidoDetailDTO>.Ok(pedido, "Pedido actualizado correctamente."));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Pedido no encontrado: {Id}", pedidoDto.Id);
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
            catch (InvalidStateTransitionException ex)
            {
                _logger.LogWarning(ex, "Transición de estado inválida para pedido: {Id}", pedidoDto.Id);
                return BadRequest(ApiResponseDTO<object>.Error(ex.Message));
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada al actualizar pedido: {Id}", pedidoDto.Id);
                return BadRequest(ApiResponseDTO<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// Elimina un pedido. Solo permitido en estado Pendiente.
        /// </summary>
        /// <param name="id">Identificador del pedido a eliminar.</param>
        /// <returns>Confirmación de eliminación.</returns>
        /// <response code="204">Pedido eliminado correctamente.</response>
        /// <response code="400">El pedido no puede ser eliminado por su estado actual.</response>
        /// <response code="404">Pedido no encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                _pedidoUseCase.DeletePedido(id);
                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Pedido no encontrado: {Id}", id);
                return NotFound(ApiResponseDTO<object>.Error(ex.Message));
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "No se puede eliminar el pedido: {Id}", id);
                return BadRequest(ApiResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
