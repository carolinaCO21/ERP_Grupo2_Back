using API.Domain.Entities;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO para mostrar qué productos ofrece un proveedor con sus precios específicos.
    /// Se utiliza en el endpoint GET /api/proveedores/{id}/productos.
    /// </summary>
    public class ProductoProveedorDTO
    {
        /// <summary>Identificador del producto.</summary>
        public int IdProducto { get; }

        /// <summary>Código SKU del producto.</summary>
        public string CodigoProducto { get; }

        /// <summary>Nombre del producto.</summary>
        public string NombreProducto { get; }

        /// <summary>Precio que cobra este proveedor por este producto.</summary>
        public decimal PrecioUnitario { get; }

        /// <summary>
        /// Construye el DTO a partir de una relación ProductoProveedor y los datos del producto.
        /// </summary>
        /// <param name="pp">Entidad ProductoProveedor origen.</param>
        /// <param name="codigoProducto">Código SKU del producto.</param>
        /// <param name="nombreProducto">Nombre del producto.</param>
        public ProductoProveedorDTO(ProductoProveedor pp, string codigoProducto, string nombreProducto)
        {
            IdProducto = pp.IdProducto;
            CodigoProducto = codigoProducto;
            NombreProducto = nombreProducto;
            PrecioUnitario = pp.PrecioUnitario;
        }
    }
}
