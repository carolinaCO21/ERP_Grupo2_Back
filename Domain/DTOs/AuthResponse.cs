namespace API.Domain.DTOs
{
    /// <summary>
    /// Respuesta de autenticación con información del usuario.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>ID del usuario en la base de datos SQL Server.</summary>
        public int UserId { get; set; }

        /// <summary>Username del usuario.</summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>Email del usuario.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Nombre del usuario.</summary>
        public string? Nombre { get; set; }

        /// <summary>Apellidos del usuario.</summary>
        public string? Apellidos { get; set; }

        /// <summary>Rol del usuario en el sistema.</summary>
        public string? Rol { get; set; }

        /// <summary>Indica si el token es válido.</summary>
        public bool IsValid { get; set; }
    }
}
