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

        /// <summary>Nombre de usuario único (campo de tu tabla).</summary>
        [Required(ErrorMessage = "El username es obligatorio.")]
        [StringLength(50, ErrorMessage = "El username no puede superar los 50 caracteres.")]
        public string Username { get; set; } = string.Empty;

        /// <summary>Correo electrónico del usuario. Debe ser único en el sistema.</summary>
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Contraseña (almacenada localmente, pero Firebase maneja la autenticación).</summary>
        [StringLength(255, ErrorMessage = "La contraseña no puede superar los 255 caracteres.")]
        public string? Password { get; set; }

        /// <summary>Nombre del usuario.</summary>
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Nombre { get; set; }

        /// <summary>Apellidos del usuario.</summary>
        [StringLength(50, ErrorMessage = "Los apellidos no pueden superar los 50 caracteres.")]
        public string? Apellidos { get; set; }

        /// <summary>Rol del usuario en el sistema (Admin, Usuario, Supervisor, etc.).</summary>
        [StringLength(50, ErrorMessage = "El rol no puede superar los 50 caracteres.")]
        public string? Rol { get; set; }

        /// <summary>Indica si el usuario está activo en el sistema.</summary>
        public bool Activo { get; set; } = true;

        /// <summary>Constructor sin parámetros requerido por Entity Framework.</summary>
        public User() { }
    }
}
