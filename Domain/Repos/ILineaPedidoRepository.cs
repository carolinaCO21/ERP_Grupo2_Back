using API.Domain.Entities;

namespace API.Domain.Repos
{
    /// <summary>
    /// Contrato de acceso a datos para la tabla Lineas_Pedido.
    /// Define las operaciones de consulta, creación y eliminación de líneas de pedido.
    /// </summary>
    public interface ILineaPedidoRepository
    {
        /// <summary>Obtiene todas las líneas de un pedido específico.</summary>
        /// <param name="pedidoId">Identificador del pedido.</param>
        /// <returns>Lista de líneas del pedido.</returns>
        List<LineaPedido> GetByPedidoId(int pedidoId);

        /// <summary>Inserta una nueva línea de pedido en la base de datos.</summary>
        /// <param name="linea">Entidad línea de pedido a insertar.</param>
        /// <returns>La línea insertada con su Id generado.</returns>
        LineaPedido Create(LineaPedido linea);

        /// <summary>
        /// Elimina todas las líneas asociadas a un pedido.
        /// Se utiliza antes de reemplazar las líneas durante una actualización.
        /// </summary>
        /// <param name="pedidoId">Identificador del pedido cuyas líneas se eliminan.</param>
        /// <returns><c>true</c> si se ejecutó correctamente.</returns>
        bool DeleteByPedidoId(int pedidoId);
    }
}
