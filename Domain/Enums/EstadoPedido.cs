namespace API.Domain.Enums
{
    /// <summary>
    /// Enumeración que define los estados posibles de un pedido a proveedor.
    /// Controla la máquina de estados y las transiciones válidas del ciclo de vida del pedido.
    /// </summary>
    public enum EstadoPedido
    {
        /// <summary>Estado inicial al crear el pedido. Permite edición y cancelación.</summary>
        Pendiente,

        /// <summary>Pedido revisado y aprobado por un supervisor. Ya no se pueden modificar las líneas.</summary>
        Aprobado,

        /// <summary>El proveedor está preparando el pedido.</summary>
        EnProceso,

        /// <summary>El pedido ha sido enviado por el proveedor.</summary>
        Enviado,

        /// <summary>El pedido ha llegado al almacén. Estado final.</summary>
        Recibido,

        /// <summary>El pedido fue cancelado. Estado final.</summary>
        Cancelado
    }
}
