using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    /// <summary>
    /// Representa un usuario del sistema ERP.
    /// Almacena la información de autenticación y perfil del usuario
    /// que interactúa con el módulo de pedidos a proveedores.
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>Identificador único del usuario.</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>UID de Firebase para vincular la autenticación externa.</summary>
        [Required(ErrorMessage = "El UID de Firebase es obligatorio.")]
        [StringLength(128, ErrorMessage = "El UID de Firebase no puede superar los 128 caracteres.")]
        public string FirebaseUid { get; set; } = string.Empty;

        /// <summary>Correo electrónico del usuario. Debe ser único en el sistema.</summary>
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(256, ErrorMessage = "El email no puede superar los 256 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Nombre del usuario.</summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Apellidos del usuario.</summary>
        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(150, ErrorMessage = "Los apellidos no pueden superar los 150 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        /// <summary>Rol del usuario en el sistema (Admin, Usuario, Supervisor, etc.).</summary>
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(50, ErrorMessage = "El rol no puede superar los 50 caracteres.")]
        public string Rol { get; set; } = string.Empty;

        /// <summary>Indica si el usuario está activo en el sistema.</summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="User"/> con todos sus campos obligatorios.
        /// </summary>
        public User(string firebaseUid, string email, string nombre, string apellidos, string rol)
        {
            FirebaseUid = firebaseUid;
            Email = email;
            Nombre = nombre;
            Apellidos = apellidos;
            Rol = rol;
            Activo = true;
        }

        /// <summary>Constructor sin parámetros requerido por Entity Framework.</summary>
        public User() { }
    }
}
