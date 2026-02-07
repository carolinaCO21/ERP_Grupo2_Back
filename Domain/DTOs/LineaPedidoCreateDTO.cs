using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO con los datos necesarios para crear una línea dentro de un pedido.
    /// Incluye validaciones para asegurar la integridad de los datos recibidos del frontend.
    /// </summary>
    public class LineaPedidoCreateDTO
    {
        /// <summary>Identificador del producto a pedir.</summary>
        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del producto debe ser válido.")]
        public int IdProducto { get; set; }

        /// <summary>Cantidad de unidades a pedir. Debe ser mayor que cero.</summary>
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Cantidad { get; set; }

        /// <summary>Precio por unidad del producto.</summary>
        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        public decimal PrecioUnitario { get; set; }

        /// <summary>Porcentaje de IVA a aplicar (ej: 21.00 para el 21%).</summary>
        [Required(ErrorMessage = "El porcentaje de IVA es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El porcentaje de IVA debe estar entre 0 y 100.")]
        public decimal IvaPorcentaje { get; set; }
    }
}
