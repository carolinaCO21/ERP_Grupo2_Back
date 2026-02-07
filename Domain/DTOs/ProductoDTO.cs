using API.Domain.Entities;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO para mostrar información de productos en la API.
    /// Expone los datos de catálogo y stock disponible.
    /// </summary>
    public class ProductoDTO
    {
        /// <summary>Identificador del producto.</summary>
        public int Id { get; }

        /// <summary>Código interno / SKU del producto.</summary>
        public string CodigoProducto { get; }

        /// <summary>Nombre del producto.</summary>
        public string Nombre { get; }

        /// <summary>Categoría a la que pertenece.</summary>
        public string Categoria { get; }

        /// <summary>Unidad de medida (unidades, kg, litros, etc.).</summary>
        public string UnidadMedida { get; }

        /// <summary>Precio base del producto.</summary>
        public decimal Precio { get; }

        /// <summary>Cantidad disponible en inventario.</summary>
        public int StockActual { get; }

        /// <summary>
        /// Construye el DTO a partir de una entidad Producto.
        /// </summary>
        /// <param name="producto">Entidad producto origen.</param>
        public ProductoDTO(Producto producto)
        {
            Id = producto.Id;
            CodigoProducto = producto.CodigoProducto;
            Nombre = producto.Nombre;
            Categoria = producto.Categoria;
            UnidadMedida = producto.UnidadMedida;
            Precio = producto.Precio;
            StockActual = producto.StockActual;
        }
    }
}
