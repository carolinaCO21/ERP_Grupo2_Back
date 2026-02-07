using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Domain.Enums;
using API.Domain.Exceptions;

namespace API.Domain.Entities
{
    /// <summary>
    /// Representa la cabecera de un pedido realizado a un proveedor.
    /// Contiene la información general del pedido (quién, cuándo, totales)
    /// y gestiona la máquina de estados del ciclo de vida del pedido.
    /// </summary>
    [Table("Pedidos")]
    public class Pedido
    {
        /// <summary>
        /// Mapa de transiciones válidas entre estados del pedido.
        /// Define desde qué estado se puede pasar a qué otros estados.
        /// </summary>
        private static readonly Dictionary<EstadoPedido, EstadoPedido[]> TransicionesValidas = new()
        {
            { EstadoPedido.Pendiente, new[] { EstadoPedido.Aprobado, EstadoPedido.Cancelado } },
            { EstadoPedido.Aprobado, new[] { EstadoPedido.EnProceso, EstadoPedido.Cancelado } },
            { EstadoPedido.EnProceso, new[] { EstadoPedido.Enviado } },
            { EstadoPedido.Enviado, new[] { EstadoPedido.Recibido } },
            { EstadoPedido.Recibido, Array.Empty<EstadoPedido>() },
            { EstadoPedido.Cancelado, Array.Empty<EstadoPedido>() }
        };

        /// <summary>Identificador único del pedido.</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Número de pedido generado automáticamente (ej: PED-2024-00001).</summary>
        [Required(ErrorMessage = "El número de pedido es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número de pedido no puede superar los 20 caracteres.")]
        public string NumeroPedido { get; set; } = string.Empty;

        /// <summary>Identificador del proveedor al que se realiza el pedido.</summary>
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public int IdProveedor { get; set; }

        /// <summary>Identificador del usuario que creó el pedido.</summary>
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int IdUsuario { get; set; }

        /// <summary>Fecha y hora de creación del pedido.</summary>
        [Required]
        public DateTime FechaPedido { get; set; }

        /// <summary>Estado actual del pedido dentro de su ciclo de vida.</summary>
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres.")]
        public string Estado { get; set; } = EstadoPedido.Pendiente.ToString();

        /// <summary>Suma de todas las líneas sin impuestos.</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>Total de impuestos (IVA) del pedido.</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Impuestos { get; set; }

        /// <summary>Importe total del pedido (Subtotal + Impuestos).</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        /// <summary>Dirección donde se debe entregar el pedido.</summary>
        [Required(ErrorMessage = "La dirección de entrega es obligatoria.")]
        [StringLength(500, ErrorMessage = "La dirección de entrega no puede superar los 500 caracteres.")]
        public string DireccionEntrega { get; set; } = string.Empty;

        // ── Propiedades de navegación ──────────────────────────────────

        /// <summary>Proveedor asociado al pedido.</summary>
        [ForeignKey(nameof(IdProveedor))]
        public Proveedor? Proveedor { get; set; }

        /// <summary>Usuario que creó el pedido.</summary>
        [ForeignKey(nameof(IdUsuario))]
        public User? Usuario { get; set; }

        /// <summary>Colección de líneas de detalle del pedido.</summary>
        public List<LineaPedido> LineasPedido { get; set; } = new();

        // ── Constructores ──────────────────────────────────────────────

        /// <summary>
        /// Inicializa un nuevo pedido con estado Pendiente y fecha actual.
        /// </summary>
        /// <param name="numeroPedido">Número único generado para el pedido.</param>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <param name="idUsuario">Identificador del usuario creador.</param>
        /// <param name="direccionEntrega">Dirección de entrega del pedido.</param>
        public Pedido(string numeroPedido, int idProveedor, int idUsuario, string direccionEntrega)
        {
            NumeroPedido = numeroPedido;
            IdProveedor = idProveedor;
            IdUsuario = idUsuario;
            DireccionEntrega = direccionEntrega;
            FechaPedido = DateTime.UtcNow;
            Estado = EstadoPedido.Pendiente.ToString();
        }

        /// <summary>Constructor sin parámetros requerido por Entity Framework.</summary>
        public Pedido() { }

        // ── Métodos de dominio ─────────────────────────────────────────

        /// <summary>
        /// Obtiene el estado actual del pedido como valor del enum <see cref="EstadoPedido"/>.
        /// </summary>
        /// <returns>El estado actual tipado como <see cref="EstadoPedido"/>.</returns>
        /// <exception cref="BusinessRuleException">Si el estado almacenado no es un valor válido del enum.</exception>
        public EstadoPedido GetEstadoActual()
        {
            if (!Enum.TryParse<EstadoPedido>(Estado, out var estadoActual))
                throw new BusinessRuleException($"El estado '{Estado}' no es un estado de pedido válido.");

            return estadoActual;
        }

        /// <summary>
        /// Cambia el estado del pedido validando que la transición sea permitida
        /// según la máquina de estados definida.
        /// </summary>
        /// <param name="nuevoEstado">Estado al que se quiere transicionar.</param>
        /// <exception cref="InvalidStateTransitionException">Si la transición no está permitida.</exception>
        public void CambiarEstado(EstadoPedido nuevoEstado)
        {
            var estadoActual = GetEstadoActual();

            if (!TransicionesValidas.TryGetValue(estadoActual, out var estadosPermitidos) ||
                !estadosPermitidos.Contains(nuevoEstado))
            {
                throw new InvalidStateTransitionException(estadoActual.ToString(), nuevoEstado.ToString());
            }

            Estado = nuevoEstado.ToString();
        }

        /// <summary>
        /// Indica si el pedido se encuentra en un estado que permite modificar sus líneas.
        /// Solo los pedidos en estado Pendiente pueden ser editados.
        /// </summary>
        /// <returns><c>true</c> si el pedido permite edición; <c>false</c> en caso contrario.</returns>
        public bool PermiteEdicionLineas()
        {
            return GetEstadoActual() == EstadoPedido.Pendiente;
        }

        /// <summary>
        /// Indica si el pedido se encuentra en un estado que permite su eliminación.
        /// Solo los pedidos en estado Pendiente pueden ser eliminados.
        /// </summary>
        /// <returns><c>true</c> si el pedido puede eliminarse; <c>false</c> en caso contrario.</returns>
        public bool PermiteEliminacion()
        {
            return GetEstadoActual() == EstadoPedido.Pendiente;
        }

        /// <summary>
        /// Recalcula los totales del pedido a partir de sus líneas de detalle.
        /// Actualiza Subtotal, Impuestos y Total.
        /// </summary>
        public void RecalcularTotales()
        {
            Subtotal = LineasPedido.Sum(l => l.Subtotal);
            Impuestos = LineasPedido.Sum(l => l.TotalLinea - l.Subtotal);
            Total = Subtotal + Impuestos;
        }
    }
}
