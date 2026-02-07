using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO para actualizar un pedido existente.
    /// Permite cambiar el estado, la dirección de entrega y las líneas del pedido.
    /// Las líneas solo se pueden modificar si el pedido está en estado Pendiente.
    /// </summary>
    public class PedidoUpdateDTO
    {
        /// <summary>Identificador del pedido a actualizar.</summary>
        [Required(ErrorMessage = "El identificador del pedido es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del pedido debe ser válido.")]
        public int Id { get; set; }

        /// <summary>
        /// Nuevo estado del pedido. Debe ser un estado válido del enum EstadoPedido.
        /// </summary>
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        /// <summary>Nueva dirección de entrega (opcional, se mantiene la anterior si es null).</summary>
        [StringLength(500, ErrorMessage = "La dirección de entrega no puede superar los 500 caracteres.")]
        public string? DireccionEntrega { get; set; }

        /// <summary>
        /// Nuevas líneas del pedido. Solo se aplican si el pedido está en estado Pendiente.
        /// Si se proporcionan, reemplazan completamente las líneas anteriores.
        /// </summary>
        public List<LineaPedidoCreateDTO>? LineasPedido { get; set; }
    }
}
