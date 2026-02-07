namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO con toda la información de un pedido, incluyendo sus líneas de detalle.
    /// Se utiliza cuando se consulta el detalle completo de un pedido específico.
    /// </summary>
    public class PedidoDetailDTO
    {
        /// <summary>Identificador del pedido.</summary>
        public int Id { get; }

        /// <summary>Número de pedido generado.</summary>
        public string NumeroPedido { get; }

        /// <summary>Identificador del proveedor.</summary>
        public int IdProveedor { get; }

        /// <summary>Nombre del proveedor.</summary>
        public string NombreProveedor { get; }

        /// <summary>Identificador del usuario que creó el pedido.</summary>
        public int IdUsuario { get; }

        /// <summary>Nombre completo del usuario creador.</summary>
        public string NombreUsuario { get; }

        /// <summary>Fecha y hora de creación del pedido.</summary>
        public DateTime FechaPedido { get; }

        /// <summary>Estado actual del pedido.</summary>
        public string Estado { get; }

        /// <summary>Subtotal sin impuestos.</summary>
        public decimal Subtotal { get; }

        /// <summary>Total de impuestos (IVA).</summary>
        public decimal Impuestos { get; }

        /// <summary>Total final del pedido.</summary>
        public decimal Total { get; }

        /// <summary>Dirección de entrega del pedido.</summary>
        public string DireccionEntrega { get; }

        /// <summary>Lista de líneas de detalle del pedido.</summary>
        public List<LineaPedidoDTO> LineasPedido { get; }

        /// <summary>
        /// Construye el DTO completo con toda la información del pedido.
        /// </summary>
        public PedidoDetailDTO(int id, string numeroPedido, int idProveedor, string nombreProveedor,
            int idUsuario, string nombreUsuario, DateTime fechaPedido, string estado,
            decimal subtotal, decimal impuestos, decimal total, string direccionEntrega,
            List<LineaPedidoDTO> lineasPedido)
        {
            Id = id;
            NumeroPedido = numeroPedido;
            IdProveedor = idProveedor;
            NombreProveedor = nombreProveedor;
            IdUsuario = idUsuario;
            NombreUsuario = nombreUsuario;
            FechaPedido = fechaPedido;
            Estado = estado;
            Subtotal = subtotal;
            Impuestos = impuestos;
            Total = total;
            DireccionEntrega = direccionEntrega;
            LineasPedido = lineasPedido;
        }
    }
}
