using API.Domain.DTOs;

namespace API.Domain.Interfaces
{
    /// <summary>
    /// Contrato que define las operaciones de negocio disponibles para la gestión de pedidos.
    /// Orquesta repositorios y aplica las reglas de negocio del módulo de pedidos a proveedores.
    /// </summary>
    public interface IPedidoUseCase
    {
        /// <summary>Obtiene todos los pedidos en formato lista resumida.</summary>
        /// <returns>Lista de pedidos con información resumida.</returns>
        List<PedidoListDTO> GetAllPedidos();

        /// <summary>Obtiene los pedidos realizados a un proveedor específico.</summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Lista de pedidos filtrados por proveedor.</returns>
        List<PedidoListDTO> GetPedidosByProveedor(int proveedorId);

        /// <summary>Obtiene los pedidos que se encuentran en un estado específico.</summary>
        /// <param name="estado">Estado por el que filtrar.</param>
        /// <returns>Lista de pedidos filtrados por estado.</returns>
        List<PedidoListDTO> GetPedidosByEstado(string estado);

        /// <summary>Obtiene el detalle completo de un pedido, incluyendo sus líneas.</summary>
        /// <param name="pedidoId">Identificador del pedido.</param>
        /// <returns>Detalle completo del pedido.</returns>
        PedidoDetailDTO GetPedidoById(int pedidoId);

        /// <summary>
        /// Crea un nuevo pedido con estado Pendiente, validando todas las reglas de negocio.
        /// El usuario se identifica mediante su FirebaseUID obtenido del token de autenticación.
        /// </summary>
        /// <param name="pedidoDto">Datos del pedido a crear.</param>
        /// <param name="firebaseUid">UID de Firebase del usuario autenticado.</param>
        /// <returns>Detalle del pedido creado.</returns>
        PedidoDetailDTO CreatePedido(PedidoCreateDTO pedidoDto, string firebaseUid);

        /// <summary>
        /// Actualiza un pedido existente respetando la máquina de estados
        /// y las restricciones de edición de líneas.
        /// </summary>
        /// <param name="pedidoDto">Datos de actualización del pedido.</param>
        /// <returns>Detalle del pedido actualizado.</returns>
        PedidoDetailDTO UpdatePedido(PedidoUpdateDTO pedidoDto);

        /// <summary>
        /// Elimina un pedido y sus líneas. Solo permitido en estado Pendiente.
        /// </summary>
        /// <param name="pedidoId">Identificador del pedido a eliminar.</param>
        /// <returns><c>true</c> si se eliminó correctamente.</returns>
        bool DeletePedido(int pedidoId);
    }
}
