using API.Domain.Entities;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO para mostrar el detalle de cada línea de un pedido.
    /// Incluye información del producto resuelta para visualización en pantalla.
    /// </summary>
    public class LineaPedidoDTO
    {
        /// <summary>Identificador de la línea.</summary>
        public int Id { get; }

        /// <summary>Identificador del producto.</summary>
        public int IdProducto { get; }

        /// <summary>Nombre del producto.</summary>
        public string NombreProducto { get; }

        /// <summary>Código/SKU del producto.</summary>
        public string CodigoProducto { get; }

        /// <summary>Cantidad de unidades pedidas.</summary>
        public int Cantidad { get; }

        /// <summary>Precio por unidad en el momento del pedido.</summary>
        public decimal PrecioUnitario { get; }

        /// <summary>Subtotal de la línea sin impuestos.</summary>
        public decimal Subtotal { get; }

        /// <summary>Porcentaje de IVA aplicado.</summary>
        public decimal IvaPorcentaje { get; }

        /// <summary>Total de la línea con IVA incluido.</summary>
        public decimal TotalLinea { get; }

        /// <summary>
        /// Construye el DTO a partir de una línea de pedido y los datos del producto.
        /// </summary>
        /// <param name="linea">Entidad línea de pedido origen.</param>
        /// <param name="nombreProducto">Nombre del producto resuelto.</param>
        /// <param name="codigoProducto">Código SKU del producto resuelto.</param>
        public LineaPedidoDTO(LineaPedido linea, string nombreProducto, string codigoProducto)
        {
            Id = linea.Id;
            IdProducto = linea.IdProducto;
            NombreProducto = nombreProducto;
            CodigoProducto = codigoProducto;
            Cantidad = linea.Cantidad;
            PrecioUnitario = linea.PrecioUnitario;
            Subtotal = linea.Subtotal;
            IvaPorcentaje = linea.IvaPorcentaje;
            TotalLinea = linea.TotalLinea;
        }
    }
}
