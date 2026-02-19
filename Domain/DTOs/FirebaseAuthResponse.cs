namespace API.Domain.DTOs
{
    /// <summary>
    /// Respuesta de Firebase Auth REST API.
    /// </summary>
    public class FirebaseAuthResponse
    {
        public string idToken { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
        public string expiresIn { get; set; } = string.Empty;
        public string localId { get; set; } = string.Empty;
    }
}
