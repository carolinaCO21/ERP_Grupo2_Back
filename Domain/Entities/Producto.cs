using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    /// <summary>
    /// Representa un producto que puede ser solicitado a los proveedores.
    /// Contiene la información de catálogo, precio base y stock disponible.
    /// </summary>
    [Table("Productos")]
    public class Producto
    {
        /// <summary>Identificador único del producto.</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Código interno del producto (SKU). Debe ser único en el sistema.</summary>
        [Required(ErrorMessage = "El código de producto es obligatorio.")]
        [StringLength(50, ErrorMessage = "El código de producto no puede superar los 50 caracteres.")]
        public string CodigoProducto { get; set; } = string.Empty;

        /// <summary>Nombre descriptivo del producto.</summary>
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre del producto no puede superar los 200 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Descripción detallada del producto.</summary>
        [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>Categoría a la que pertenece el producto.</summary>
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(100, ErrorMessage = "La categoría no puede superar los 100 caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        /// <summary>Unidad de medida del producto (unidades, kg, litros, etc.).</summary>
        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        [StringLength(30, ErrorMessage = "La unidad de medida no puede superar los 30 caracteres.")]
        public string UnidadMedida { get; set; } = string.Empty;

        /// <summary>Precio base del producto. Debe ser mayor o igual a cero.</summary>
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 999999999.99, ErrorMessage = "El precio debe estar entre 0 y 999.999.999,99.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        /// <summary>Cantidad actual disponible en inventario.</summary>
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int StockActual { get; set; }

        /// <summary>Indica si el producto está activo y disponible para pedidos.</summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Producto"/> con todos sus campos obligatorios.
        /// </summary>
        public Producto(string codigoProducto, string nombre, string descripcion,
            string categoria, string unidadMedida, decimal precio, int stockActual)
        {
            CodigoProducto = codigoProducto;
            Nombre = nombre;
            Descripcion = descripcion;
            Categoria = categoria;
            UnidadMedida = unidadMedida;
            Precio = precio;
            StockActual = stockActual;
            Activo = true;
        }

        /// <summary>Constructor sin parámetros requerido por Entity Framework.</summary>
        public Producto() { }
    }
}
