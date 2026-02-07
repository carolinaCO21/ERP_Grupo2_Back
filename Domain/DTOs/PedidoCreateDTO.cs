using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO que recibe los datos necesarios para crear un nuevo pedido.
    /// El frontend envía este objeto en el POST /api/pedidos.
    /// Incluye validaciones de ModelState para rechazar peticiones malformadas.
    /// </summary>
    public class PedidoCreateDTO
    {
        /// <summary>Identificador del proveedor al que se realiza el pedido.</summary>
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del proveedor debe ser válido.")]
        public int IdProveedor { get; set; }

        /// <summary>Identificador del usuario que crea el pedido.</summary>
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del usuario debe ser válido.")]
        public int IdUsuario { get; set; }

        /// <summary>Dirección donde se debe entregar el pedido.</summary>
        [Required(ErrorMessage = "La dirección de entrega es obligatoria.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "La dirección de entrega debe tener entre 5 y 500 caracteres.")]
        public string DireccionEntrega { get; set; } = string.Empty;

        /// <summary>
        /// Lista de líneas del pedido. Debe contener al menos una línea.
        /// </summary>
        [Required(ErrorMessage = "El pedido debe contener al menos una línea.")]
        [MinLength(1, ErrorMessage = "El pedido debe contener al menos una línea.")]
        public List<LineaPedidoCreateDTO> LineasPedido { get; set; } = new();
    }
}
