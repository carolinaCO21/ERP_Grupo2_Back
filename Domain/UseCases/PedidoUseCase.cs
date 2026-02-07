using API.Domain.DTOs;
using API.Domain.Entities;
using API.Domain.Enums;
using API.Domain.Exceptions;
using API.Domain.Interfaces;
using API.Domain.Repos;

namespace API.Domain.UseCases
{
    /// <summary>
    /// Implementa toda la lógica de negocio relacionada con pedidos a proveedores.
    /// Coordina los repositorios, aplica validaciones de negocio, gestiona la máquina
    /// de estados y construye los DTOs de respuesta.
    /// </summary>
    public class PedidoUseCase : IPedidoUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILineaPedidoRepository _lineaPedidoRepository;
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IProductoProveedorRepository _productoProveedorRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Inicializa el caso de uso con las dependencias de repositorios inyectadas.
        /// </summary>
        public PedidoUseCase(
            IPedidoRepository pedidoRepository,
            ILineaPedidoRepository lineaPedidoRepository,
            IProveedorRepository proveedorRepository,
            IProductoRepository productoRepository,
            IProductoProveedorRepository productoProveedorRepository,
            IUserRepository userRepository)
        {
            _pedidoRepository = pedidoRepository;
            _lineaPedidoRepository = lineaPedidoRepository;
            _proveedorRepository = proveedorRepository;
            _productoRepository = productoRepository;
            _productoProveedorRepository = productoProveedorRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Obtiene todos los pedidos en formato lista resumida.
        /// Para cada pedido resuelve el nombre del proveedor.
        /// </summary>
        /// <returns>Lista de pedidos con información resumida.</returns>
        public List<PedidoListDTO> GetAllPedidos()
        {
            var pedidos = _pedidoRepository.GetAll();
            return ConvertirAPedidoListDTO(pedidos);
        }

        /// <summary>
        /// Obtiene los pedidos realizados a un proveedor específico.
        /// </summary>
        /// <param name="proveedorId">Identificador del proveedor.</param>
        /// <returns>Lista de pedidos filtrados por proveedor.</returns>
        /// <exception cref="EntityNotFoundException">Si el proveedor no existe.</exception>
        public List<PedidoListDTO> GetPedidosByProveedor(int proveedorId)
        {
            ValidarProveedorExiste(proveedorId);

            var pedidos = _pedidoRepository.GetByProveedorId(proveedorId);
            return ConvertirAPedidoListDTO(pedidos);
        }

        /// <summary>
        /// Obtiene los pedidos que se encuentran en un estado específico.
        /// </summary>
        /// <param name="estado">Estado por el que filtrar (debe ser un valor válido de EstadoPedido).</param>
        /// <returns>Lista de pedidos filtrados por estado.</returns>
        /// <exception cref="BusinessRuleException">Si el estado proporcionado no es válido.</exception>
        public List<PedidoListDTO> GetPedidosByEstado(string estado)
        {
            ValidarEstado(estado);

            var pedidos = _pedidoRepository.GetByEstado(estado);
            return ConvertirAPedidoListDTO(pedidos);
        }

        /// <summary>
        /// Obtiene el detalle completo de un pedido, incluyendo sus líneas,
        /// nombres de proveedor, usuario y productos.
        /// </summary>
        /// <param name="pedidoId">Identificador del pedido.</param>
        /// <returns>Detalle completo del pedido.</returns>
        /// <exception cref="EntityNotFoundException">Si el pedido no existe.</exception>
        public PedidoDetailDTO GetPedidoById(int pedidoId)
        {
            var pedido = _pedidoRepository.GetById(pedidoId)
                ?? throw new EntityNotFoundException("Pedido", pedidoId);

            return ConstruirPedidoDetailDTO(pedido);
        }

        /// <summary>
        /// Crea un nuevo pedido con estado Pendiente validando todas las reglas de negocio:
        /// El usuario se obtiene a partir de su FirebaseUID.
        /// proveedor activo, usuario activo, productos en catálogo del proveedor,
        /// al menos una línea, y cálculo automático de importes.
        /// </summary>
        /// <param name="pedidoDto">Datos del pedido a crear.</param>
        /// <param name="firebaseUid">UID de Firebase del usuario autenticado.</param>
        /// <returns>Detalle del pedido creado.</returns>
        /// <exception cref="EntityNotFoundException">Si el proveedor, usuario o algún producto no existe.</exception>
        /// <exception cref="BusinessRuleException">Si alguna regla de negocio se viola.</exception>
        public PedidoDetailDTO CreatePedido(PedidoCreateDTO pedidoDto, string firebaseUid)
        {
            var proveedor = ValidarProveedorActivo(pedidoDto.IdProveedor);
            var usuario = ValidarUsuarioActivoPorFirebaseUid(firebaseUid);
            ValidarLineasNoVacias(pedidoDto.LineasPedido);
            ValidarProductosEnCatalogoProveedor(pedidoDto.IdProveedor, pedidoDto.LineasPedido);

            var numeroPedido = _pedidoRepository.GenerateNumeroPedido();

            var pedido = new Pedido(numeroPedido, pedidoDto.IdProveedor, usuario.Id, pedidoDto.DireccionEntrega);
            var pedidoCreado = _pedidoRepository.Create(pedido);

            var lineasCreadas = CrearLineasPedido(pedidoCreado.Id, pedidoDto.LineasPedido);

            pedidoCreado.LineasPedido = lineasCreadas;
            pedidoCreado.RecalcularTotales();
            _pedidoRepository.Update(pedidoCreado);

            return ConstruirPedidoDetailDTO(pedidoCreado);
        }


        /// <summary>
        /// Valida que el usuario existe y está activo usando su FirebaseUID.
        /// </summary>
        /// <param name="firebaseUid">UID de Firebase del usuario.</param>
        /// <returns>La entidad User validada.</returns>
        private User ValidarUsuarioActivoPorFirebaseUid(string firebaseUid)
        {
            var usuario = _userRepository.GetByFirebaseUid(firebaseUid)
                ?? throw new EntityNotFoundException("Pedido",$"No se encontró un usuario con FirebaseUID: {firebaseUid}");

            if (!usuario.Activo)
            {
                throw new BusinessRuleException(
                    $"El usuario '{usuario.Nombre} {usuario.Apellidos}' no está activo en el sistema.");
            }

            return usuario;
        }

        /// <summary>
        /// Actualiza un pedido existente respetando la máquina de estados y las restricciones:
        /// las líneas solo se pueden modificar en estado Pendiente, y los cambios de estado
        /// deben seguir las transiciones permitidas.
        /// </summary>
        /// <param name="pedidoDto">Datos de actualización del pedido.</param>
        /// <returns>Detalle del pedido actualizado.</returns>
        /// <exception cref="EntityNotFoundException">Si el pedido no existe.</exception>
        /// <exception cref="InvalidStateTransitionException">Si la transición de estado no es válida.</exception>
        /// <exception cref="BusinessRuleException">Si se intentan editar líneas fuera de estado Pendiente.</exception>
        public PedidoDetailDTO UpdatePedido(PedidoUpdateDTO pedidoDto)
        {
            var pedido = _pedidoRepository.GetById(pedidoDto.Id)
                ?? throw new EntityNotFoundException("Pedido", pedidoDto.Id);

            ValidarEstado(pedidoDto.Estado);
            var nuevoEstado = Enum.Parse<EstadoPedido>(pedidoDto.Estado);

            if (pedido.GetEstadoActual() != nuevoEstado)
            {
                pedido.CambiarEstado(nuevoEstado);
            }

            if (!string.IsNullOrWhiteSpace(pedidoDto.DireccionEntrega))
            {
                pedido.DireccionEntrega = pedidoDto.DireccionEntrega;
            }

            if (pedidoDto.LineasPedido != null && pedidoDto.LineasPedido.Count > 0)
            {
                if (!pedido.PermiteEdicionLineas())
                {
                    throw new BusinessRuleException(
                        $"No se pueden modificar las líneas de un pedido en estado '{pedido.Estado}'. " +
                        "Solo los pedidos en estado 'Pendiente' permiten edición de líneas.");
                }

                ValidarProductosEnCatalogoProveedor(pedido.IdProveedor, pedidoDto.LineasPedido);

                _lineaPedidoRepository.DeleteByPedidoId(pedido.Id);
                var nuevasLineas = CrearLineasPedido(pedido.Id, pedidoDto.LineasPedido);

                pedido.LineasPedido = nuevasLineas;
                pedido.RecalcularTotales();
            }

            _pedidoRepository.Update(pedido);

            return ConstruirPedidoDetailDTO(pedido);
        }

        /// <summary>
        /// Elimina un pedido y sus líneas. Solo permitido para pedidos en estado Pendiente.
        /// </summary>
        /// <param name="pedidoId">Identificador del pedido a eliminar.</param>
        /// <returns><c>true</c> si se eliminó correctamente.</returns>
        /// <exception cref="EntityNotFoundException">Si el pedido no existe.</exception>
        /// <exception cref="BusinessRuleException">Si el pedido no está en estado Pendiente.</exception>
        public bool DeletePedido(int pedidoId)
        {
            var pedido = _pedidoRepository.GetById(pedidoId)
                ?? throw new EntityNotFoundException("Pedido", pedidoId);

            if (!pedido.PermiteEliminacion())
            {
                throw new BusinessRuleException(
                    $"No se puede eliminar un pedido en estado '{pedido.Estado}'. " +
                    "Solo los pedidos en estado 'Pendiente' pueden ser eliminados.");
            }

            return _pedidoRepository.Delete(pedidoId);
        }

        // ── Métodos privados de validación ─────────────────────────────

        /// <summary>
        /// Valida que el estado proporcionado sea un valor válido del enum EstadoPedido.
        /// </summary>
        private static void ValidarEstado(string estado)
        {
            if (!Enum.TryParse<EstadoPedido>(estado, ignoreCase: true, out var estadoParseado) ||

                !Enum.IsDefined(typeof(EstadoPedido), estadoParseado))
            {
                var estadosValidos = string.Join(", ", Enum.GetNames<EstadoPedido>());
                throw new BusinessRuleException(
                    $"El estado '{estado}' no es válido. Estados permitidos: {estadosValidos}.");
            }
        }

        /// <summary>
        /// Valida que el proveedor existe en la base de datos.
        /// </summary>
        private void ValidarProveedorExiste(int proveedorId)
        {
            _ = _proveedorRepository.GetById(proveedorId)
                ?? throw new EntityNotFoundException("Proveedor", proveedorId);
        }

        /// <summary>
        /// Valida que el proveedor existe y está activo para recibir pedidos.
        /// </summary>
        /// <returns>La entidad Proveedor validada.</returns>
        private Proveedor ValidarProveedorActivo(int proveedorId)
        {
            var proveedor = _proveedorRepository.GetById(proveedorId)
                ?? throw new EntityNotFoundException("Proveedor", proveedorId);

            if (!proveedor.Activo)
            {
                throw new BusinessRuleException(
                    $"El proveedor '{proveedor.NombreEmpresa}' no está activo y no puede recibir pedidos.");
            }

            return proveedor;
        }

        /// <summary>
        /// Valida que el usuario existe y está activo en el sistema.
        /// </summary>
        /// <returns>La entidad User validada.</returns>
        private User ValidarUsuarioActivo(int usuarioId)
        {
            var usuario = _userRepository.GetById(usuarioId)
                ?? throw new EntityNotFoundException("Usuario", usuarioId);

            if (!usuario.Activo)
            {
                throw new BusinessRuleException(
                    $"El usuario '{usuario.Nombre} {usuario.Apellidos}' no está activo en el sistema.");
            }

            return usuario;
        }

        /// <summary>
        /// Valida que la lista de líneas del pedido no esté vacía.
        /// </summary>
        private static void ValidarLineasNoVacias(List<LineaPedidoCreateDTO> lineas)
        {
            if (lineas == null || lineas.Count == 0)
            {
                throw new BusinessRuleException("El pedido debe contener al menos una línea.");
            }
        }

        /// <summary>
        /// Valida que todos los productos de las líneas estén en el catálogo del proveedor.
        /// Un producto debe tener una relación activa en ProductoProveedor para ser pedido.
        /// </summary>
        private void ValidarProductosEnCatalogoProveedor(int proveedorId, List<LineaPedidoCreateDTO> lineas)
        {
            foreach (var linea in lineas)
            {
                var producto = _productoRepository.GetById(linea.IdProducto)
                    ?? throw new EntityNotFoundException("Producto", linea.IdProducto);

                if (!producto.Activo)
                {
                    throw new BusinessRuleException(
                        $"El producto '{producto.Nombre}' (ID: {producto.Id}) no está activo.");
                }

                var relacion = _productoProveedorRepository.GetByProveedorAndProducto(proveedorId, linea.IdProducto);

                if (relacion == null || !relacion.Activo)
                {
                    var nombreProveedor = _proveedorRepository.GetNombreById(proveedorId) ?? "Desconocido";
                    throw new BusinessRuleException(
                        $"El producto '{producto.Nombre}' (ID: {producto.Id}) no está en el catálogo " +
                        $"del proveedor '{nombreProveedor}' (ID: {proveedorId}).");
                }
            }
        }

        // ── Métodos privados de construcción ───────────────────────────

        /// <summary>
        /// Crea las líneas de pedido en la base de datos a partir de los DTOs de creación.
        /// Calcula automáticamente los importes de cada línea.
        /// </summary>
        private List<LineaPedido> CrearLineasPedido(int pedidoId, List<LineaPedidoCreateDTO> lineasDto)
        {
            var lineasCreadas = new List<LineaPedido>();

            foreach (var lineaDto in lineasDto)
            {
                var linea = new LineaPedido(
                    pedidoId,
                    lineaDto.IdProducto,
                    lineaDto.Cantidad,
                    lineaDto.PrecioUnitario,
                    lineaDto.IvaPorcentaje);

                lineasCreadas.Add(_lineaPedidoRepository.Create(linea));
            }

            return lineasCreadas;
        }

        /// <summary>
        /// Convierte una lista de entidades Pedido a DTOs de lista resumida,
        /// resolviendo el nombre de cada proveedor.
        /// </summary>
        private List<PedidoListDTO> ConvertirAPedidoListDTO(List<Pedido> pedidos)
        {
            var resultado = new List<PedidoListDTO>();

            foreach (var pedido in pedidos)
            {
                var nombreProveedor = _proveedorRepository.GetNombreById(pedido.IdProveedor) ?? "Proveedor desconocido";
                resultado.Add(new PedidoListDTO(pedido, nombreProveedor));
            }

            return resultado;
        }

        /// <summary>
        /// Construye un PedidoDetailDTO completo a partir de una entidad Pedido,
        /// resolviendo nombres de proveedor, usuario y productos de cada línea.
        /// </summary>
        private PedidoDetailDTO ConstruirPedidoDetailDTO(Pedido pedido)
        {
            var nombreProveedor = _proveedorRepository.GetNombreById(pedido.IdProveedor) ?? "Proveedor desconocido";
            var nombreUsuario = _userRepository.GetNombreCompletoById(pedido.IdUsuario) ?? "Usuario desconocido";

            var lineas = pedido.LineasPedido.Count > 0
                ? pedido.LineasPedido
                : _lineaPedidoRepository.GetByPedidoId(pedido.Id);

            var lineasDto = new List<LineaPedidoDTO>();
            foreach (var linea in lineas)
            {
                var producto = _productoRepository.GetById(linea.IdProducto);
                var nombreProducto = producto?.Nombre ?? "Producto desconocido";
                var codigoProducto = producto?.CodigoProducto ?? "N/A";
                lineasDto.Add(new LineaPedidoDTO(linea, nombreProducto, codigoProducto));
            }

            return new PedidoDetailDTO(
                pedido.Id,
                pedido.NumeroPedido,
                pedido.IdProveedor,
                nombreProveedor,
                pedido.IdUsuario,
                nombreUsuario,
                pedido.FechaPedido,
                pedido.Estado,
                pedido.Subtotal,
                pedido.Impuestos,
                pedido.Total,
                pedido.DireccionEntrega,
                lineasDto);
        }
    }
}
