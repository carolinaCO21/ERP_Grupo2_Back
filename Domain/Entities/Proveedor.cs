using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    /// <summary>
    /// Representa una empresa proveedora a la que se le pueden realizar pedidos.
    /// Contiene los datos fiscales y de contacto necesarios para la gestión de compras.
    /// </summary>
    [Table("Proveedores")]
    public class Proveedor
    {
        /// <summary>Identificador único del proveedor.</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Nombre comercial de la empresa proveedora.</summary>
        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre de la empresa no puede superar los 200 caracteres.")]
        public string NombreEmpresa { get; set; } = string.Empty;

        /// <summary>CIF/NIF de la empresa. Debe ser único en el sistema.</summary>
        [Required(ErrorMessage = "El CIF es obligatorio.")]
        [StringLength(20, ErrorMessage = "El CIF no puede superar los 20 caracteres.")]
        public string Cif { get; set; } = string.Empty;

        /// <summary>Dirección fiscal de la empresa proveedora.</summary>
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(300, ErrorMessage = "La dirección no puede superar los 300 caracteres.")]
        public string Direccion { get; set; } = string.Empty;

        /// <summary>Ciudad donde se ubica la empresa.</summary>
        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100, ErrorMessage = "La ciudad no puede superar los 100 caracteres.")]
        public string Ciudad { get; set; } = string.Empty;

        /// <summary>Provincia de la empresa.</summary>
        [Required(ErrorMessage = "La provincia es obligatoria.")]
        [StringLength(100, ErrorMessage = "La provincia no puede superar los 100 caracteres.")]
        public string Provincia { get; set; } = string.Empty;

        /// <summary>Código postal de la dirección fiscal.</summary>
        [Required(ErrorMessage = "El código postal es obligatorio.")]
        [StringLength(10, ErrorMessage = "El código postal no puede superar los 10 caracteres.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "El código postal debe tener 5 dígitos.")]
        public string CodigoPostal { get; set; } = string.Empty;

        /// <summary>Teléfono de contacto de la empresa.</summary>
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        /// <summary>Email de contacto de la empresa proveedora.</summary>
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(256, ErrorMessage = "El email no puede superar los 256 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Indica si el proveedor está activo y disponible para recibir pedidos.</summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Proveedor"/> con todos sus campos obligatorios.
        /// </summary>
        public Proveedor(string nombreEmpresa, string cif, string direccion, string ciudad,
            string provincia, string codigoPostal, string telefono, string email)
        {
            NombreEmpresa = nombreEmpresa;
            Cif = cif;
            Direccion = direccion;
            Ciudad = ciudad;
            Provincia = provincia;
            CodigoPostal = codigoPostal;
            Telefono = telefono;
            Email = email;
            Activo = true;
        }

        /// <summary>Constructor sin parámetros requerido por Entity Framework.</summary>
        public Proveedor() { }
    }
}
