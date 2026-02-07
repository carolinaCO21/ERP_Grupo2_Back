using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    /// <summary>
    /// Representa una línea de detalle de un pedido a proveedor.
    /// Cada línea corresponde a un producto específico con su cantidad, precio e IVA.
    /// Un pedido puede contener múltiples líneas.
    /// </summary>
    [Table("Lineas_Pedido")]
    public class LineaPedido
    {
        /// <summary>Identificador único de la línea de pedido.</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Identificador del pedido al que pertenece esta línea.</summary>
        [Required(ErrorMessage = "El pedido es obligatorio.")]
        public int IdPedido { get; set; }

        /// <summary>Identificador del producto solicitado en esta línea.</summary>
        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int IdProducto { get; set; }

        /// <summary>Cantidad de unidades solicitadas. Debe ser mayor que cero.</summary>
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Cantidad { get; set; }

        /// <summary>Precio por unidad en el momento de realizar el pedido.</summary>
        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        /// <summary>Subtotal de la línea (Cantidad × PrecioUnitario), sin impuestos.</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>Porcentaje de IVA aplicado a esta línea (ej: 21.00 para el 21%).</summary>
        [Required(ErrorMessage = "El porcentaje de IVA es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El porcentaje de IVA debe estar entre 0 y 100.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal IvaPorcentaje { get; set; }

        /// <summary>Total de la línea con IVA incluido (Subtotal + importe de IVA).</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }

        // ── Propiedades de navegación ──────────────────────────────────

        /// <summary>Producto asociado a esta línea de pedido.</summary>
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }

        /// <summary>Pedido al que pertenece esta línea.</summary>
        [ForeignKey(nameof(IdPedido))]
        public Pedido? Pedido { get; set; }

        // ── Constructores ──────────────────────────────────────────────

        /// <summary>
        /// Inicializa una nueva línea de pedido calculando automáticamente
        /// el subtotal y el total con IVA.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido padre.</param>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <param name="cantidad">Cantidad de unidades a pedir.</param>
        /// <param name="precioUnitario">Precio por unidad.</param>
        /// <param name="ivaPorcentaje">Porcentaje de IVA a aplicar.</param>
        public LineaPedido(int idPedido, int idProducto, int cantidad,
            decimal precioUnitario, decimal ivaPorcentaje)
        {
            IdPedido = idPedido;
            IdProducto = idProducto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            IvaPorcentaje = ivaPorcentaje;
            CalcularImportes();
        }

        /// <summary>Constructor sin parámetros requerido por Entity Framework.</summary>
        public LineaPedido() { }

        // ── Métodos de dominio ─────────────────────────────────────────

        /// <summary>
        /// Calcula el subtotal y el total con IVA de esta línea de pedido.
        /// Subtotal = Cantidad × PrecioUnitario.
        /// TotalLinea = Subtotal + (Subtotal × IvaPorcentaje / 100).
        /// </summary>
        public void CalcularImportes()
        {
            Subtotal = Cantidad * PrecioUnitario;
            TotalLinea = Subtotal + (Subtotal * IvaPorcentaje / 100m);
        }
    }
}
