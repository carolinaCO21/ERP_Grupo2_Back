using API.Domain.Entities;

namespace API.Domain.Repos
{
    /// <summary>
    /// Contrato de acceso a datos para la tabla Pedidos.
    /// Define las operaciones CRUD y consultas específicas sobre pedidos.
    /// </summary>
    public interface IPedidoRepository
    {
        /// <summary>Obtiene todos los pedidos incluyendo sus líneas de detalle.</summary>
        /// <returns>Lista completa de pedidos.</returns>
        List<Pedido> GetAll();

        /// <summary>Obtiene los pedidos realizados a un proveedor específico.</summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Lista de pedidos filtrados por proveedor.</returns>
        List<Pedido> GetByProveedorId(int proveedorId);

        /// <summary>Obtiene los pedidos que se encuentran en un estado específico.</summary>
        /// <param name="estado">Estado por el que filtrar (Pendiente, Aprobado, etc.).</param>
        /// <returns>Lista de pedidos filtrados por estado.</returns>
        List<Pedido> GetByEstado(string estado);

        /// <summary>Obtiene un pedido por su identificador, incluyendo sus líneas.</summary>
        /// <param name="id">Identificador del pedido.</param>
        /// <returns>El pedido encontrado o null si no existe.</returns>
        Pedido? GetById(int id);

        /// <summary>Inserta un nuevo pedido en la base de datos.</summary>
        /// <param name="pedido">Entidad pedido a insertar.</param>
        /// <returns>El pedido insertado con su Id generado.</returns>
        Pedido Create(Pedido pedido);

        /// <summary>Actualiza un pedido existente en la base de datos.</summary>
        /// <param name="pedido">Entidad pedido con los datos actualizados.</param>
        /// <returns>El pedido actualizado.</returns>
        Pedido Update(Pedido pedido);

        /// <summary>Elimina un pedido de la base de datos.</summary>
        /// <param name="id">Identificador del pedido a eliminar.</param>
        /// <returns><c>true</c> si se eliminó correctamente; <c>false</c> si no se encontró.</returns>
        bool Delete(int id);

        /// <summary>
        /// Genera un número de pedido único con formato PED-{AÑO}-{SECUENCIAL}.
        /// Ejemplo: PED-2024-00001.
        /// </summary>
        /// <returns>Número de pedido generado.</returns>
        string GenerateNumeroPedido();
    }
}
