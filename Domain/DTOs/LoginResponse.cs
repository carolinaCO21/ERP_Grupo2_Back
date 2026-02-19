namespace API.Domain.DTOs
{
    /// <summary>
    /// Respuesta de login exitoso con token de Firebase y datos del usuario desde SQL Server.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>Token JWT de Firebase para usar en requests subsiguientes.</summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>Token de refresco de Firebase.</summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>Tiempo de expiración del token en segundos.</summary>
        public string ExpiresIn { get; set; } = string.Empty;

        /// <summary>Información del usuario desde SQL Server.</summary>
        public AuthResponse User { get; set; } = new();
    }
}
