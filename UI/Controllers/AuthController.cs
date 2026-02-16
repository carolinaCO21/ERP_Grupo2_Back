using API.Domain.Entities;
using API.Domain.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace UI.Controllers
{
    /// <summary>
    /// Controlador API para autenticación de usuarios con Firebase.
    /// Autentica con Firebase y obtiene datos adicionales de SQL Server.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Inicializa el controlador con las dependencias inyectadas.
        /// </summary>
        public AuthController(
            IUserRepository userRepository, 
            ILogger<AuthController> logger,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Inicia sesión con email y contraseña usando Firebase Authentication.
        /// Autentica al usuario con Firebase y retorna datos adicionales desde SQL Server.
        /// </summary>
        /// <param name="request">Credenciales de login (email y password).</param>
        /// <returns>Token JWT de Firebase e información del usuario desde la BD.</returns>
        /// <response code="200">Login exitoso, retorna token e información del usuario.</response>
        /// <response code="400">Request inválido.</response>
        /// <response code="401">Credenciales incorrectas o usuario inactivo.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email y contraseña son requeridos." });
            }

            try
            {
                // ═══ PASO 1: Autenticar con Firebase ═══
                var firebaseApiKey = _configuration["Firebase:ApiKey"];
                if (string.IsNullOrEmpty(firebaseApiKey))
                {
                    _logger.LogError("Firebase ApiKey no configurada en appsettings.json");
                    return StatusCode(500, new { message = "Error de configuración del servidor." });
                }

                var firebaseAuthUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={firebaseApiKey}";
                
                var firebaseRequest = new
                {
                    email = request.Email,
                    password = request.Password,
                    returnSecureToken = true
                };

                var firebaseResponse = await _httpClient.PostAsJsonAsync(firebaseAuthUrl, firebaseRequest);
                var responseContent = await firebaseResponse.Content.ReadAsStringAsync();

                if (!firebaseResponse.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Login fallido en Firebase para {Email}: {Response}", request.Email, responseContent);
                    return Unauthorized(new { message = "Email o contraseña incorrectos." });
                }

                var authResponse = JsonSerializer.Deserialize<FirebaseAuthResponse>(responseContent);
                if (authResponse == null || string.IsNullOrEmpty(authResponse.localId))
                {
                    return Unauthorized(new { message = "Error al autenticar con Firebase." });
                }

                // ═══ PASO 2: Buscar datos adicionales en SQL Server ═══
                // Buscar por email (ya que el usuario se autenticó con Firebase)
                var user = _userRepository.GetByEmail(request.Email);

                if (user == null)
                {
                    _logger.LogWarning("Usuario {Email} autenticado en Firebase pero no existe en SQL Server.", request.Email);
                    return Unauthorized(new 
                    { 
                        message = "Usuario no registrado en el sistema. Contacte al administrador.",
                        email = request.Email
                    });
                }

                if (!user.Activo)
                {
                    _logger.LogWarning("Intento de login de usuario inactivo: {Email}", request.Email);
                    return Unauthorized(new { message = "Usuario inactivo. Contacte al administrador." });
                }

                // ═══ PASO 3: Retornar token de Firebase + datos de SQL Server ═══
                var loginResponse = new LoginResponse
                {
                    Token = authResponse.idToken,
                    RefreshToken = authResponse.refreshToken,
                    ExpiresIn = authResponse.expiresIn,
                    User = new AuthResponse
                    {
                        UserId = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Nombre = user.Nombre,
                        Apellidos = user.Apellidos,
                        Rol = user.Rol,
                        IsValid = true
                    }
                };

                _logger.LogInformation("Usuario {Email} autenticado correctamente (Firebase + SQL Server).", request.Email);
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al autenticar usuario {Email}.", request.Email);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        /// <summary>
        /// Obtiene información del usuario autenticado actual.
        /// Requiere un token JWT válido de Firebase en el header Authorization.
        /// </summary>
        /// <returns>Información del usuario desde la BD.</returns>
        /// <response code="200">Información del usuario obtenida correctamente.</response>
        /// <response code="401">No autorizado o token inválido.</response>
        /// <response code="404">Usuario no encontrado en la base de datos.</response>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCurrentUser()
        {
            try
            {
                // Extraer email del token JWT de Firebase
                var email = User.FindFirst(ClaimTypes.Email)?.Value
                    ?? User.FindFirst("email")?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("No se pudo extraer el email del token.");
                    return Unauthorized(new { message = "Token inválido: no se pudo obtener el email." });
                }

                // Buscar usuario en SQL Server
                var user = _userRepository.GetByEmail(email);

                if (user == null)
                {
                    _logger.LogWarning("Usuario con email {Email} no encontrado en SQL Server.", email);
                    return NotFound(new { message = "Usuario no encontrado en el sistema." });
                }

                var response = new AuthResponse
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Nombre = user.Nombre,
                    Apellidos = user.Apellidos,
                    Rol = user.Rol,
                    IsValid = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener información del usuario autenticado.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }

    #region DTOs

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

    /// <summary>
    /// Respuesta de Firebase Auth REST API.
    /// </summary>
    internal class FirebaseAuthResponse
    {
        public string idToken { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
        public string expiresIn { get; set; } = string.Empty;
        public string localId { get; set; } = string.Empty;
    }

    #endregion
}
