using API.Domain.Entities;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO para mostrar pedidos en un listado con información resumida.
    /// Se utiliza en el endpoint GET /api/pedidos para evitar cargar todos los detalles.
    /// </summary>
    public class PedidoListDTO
    {
        /// <summary>Identificador del pedido.</summary>
        public int Id { get; }

        /// <summary>Número de pedido generado (ej: PED-2024-00001).</summary>
        public string NumeroPedido { get; }

        /// <summary>Nombre del proveedor asociado al pedido.</summary>
        public string NombreProveedor { get; }

        /// <summary>Fecha y hora de creación del pedido.</summary>
        public DateTime FechaPedido { get; }

        /// <summary>Estado actual del pedido.</summary>
        public string Estado { get; }

        /// <summary>Total del pedido con impuestos incluidos.</summary>
        public decimal Total { get; }

        /// <summary>
        /// Construye el DTO a partir de una entidad Pedido y el nombre del proveedor.
        /// </summary>
        /// <param name="pedido">Entidad pedido origen.</param>
        /// <param name="nombreProveedor">Nombre del proveedor resuelto desde la BD.</param>
        public PedidoListDTO(Pedido pedido, string nombreProveedor)
        {
            Id = pedido.Id;
            NumeroPedido = pedido.NumeroPedido;
            NombreProveedor = nombreProveedor;
            FechaPedido = pedido.FechaPedido;
            Estado = pedido.Estado;
            Total = pedido.Total;
        }
    }
}
