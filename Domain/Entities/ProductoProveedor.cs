using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    /// <summary>
    /// Tabla intermedia que relaciona productos con proveedores.
    /// Indica qué productos ofrece cada proveedor y a qué precio específico,
    /// que puede diferir del precio base del producto.
    /// </summary>
    [Table("Productos_Proveedor")]
    public class ProductoProveedor
    {
        /// <summary>Identificador único de la relación producto-proveedor.</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Identificador del proveedor.</summary>
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public int IdProveedor { get; set; }

        /// <summary>Identificador del producto.</summary>
        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int IdProducto { get; set; }

        /// <summary>Precio que cobra este proveedor por este producto.</summary>
        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        /// <summary>Indica si esta relación producto-proveedor está activa.</summary>
        public bool Activo { get; set; } = true;

        // ── Propiedades de navegación ──────────────────────────────────

        /// <summary>Proveedor asociado.</summary>
        [ForeignKey(nameof(IdProveedor))]
        public Proveedor? Proveedor { get; set; }

        /// <summary>Producto asociado.</summary>
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }

        // ── Constructores ──────────────────────────────────────────────

        /// <summary>
        /// Inicializa una nueva relación producto-proveedor.
        /// </summary>
        public ProductoProveedor(int idProveedor, int idProducto, decimal precioUnitario)
        {
            IdProveedor = idProveedor;
            IdProducto = idProducto;
            PrecioUnitario = precioUnitario;
            Activo = true;
        }

        /// <summary>Constructor sin parámetros requerido por Entity Framework.</summary>
        public ProductoProveedor() { }
    }
}
