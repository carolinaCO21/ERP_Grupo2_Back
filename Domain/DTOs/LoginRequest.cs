namespace API.Domain.DTOs
{
    /// <summary>
    /// Modelo para recibir credenciales de login.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>Email del usuario.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Contraseña del usuario.</summary>
        public string Password { get; set; } = string.Empty;
    }
}
