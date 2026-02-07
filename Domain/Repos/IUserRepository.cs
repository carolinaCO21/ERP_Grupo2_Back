using API.Domain.Entities;

namespace API.Domain.Repos
{
    /// <summary>
    /// Contrato de acceso a datos para la tabla Users.
    /// Define las operaciones de consulta sobre usuarios del sistema.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>Obtiene un usuario por su identificador.</summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>El usuario encontrado o null si no existe.</returns>
        User? GetById(int id);

        /// <summary>
        /// Obtiene un usuario por su UID de Firebase.
        /// </summary>
        /// <param name="firebaseUid">UID de Firebase del usuario.</param>
        /// <returns>El usuario encontrado o null si no existe.</returns>
        User? GetByFirebaseUid(string firebaseUid);



        /// <summary>
        /// Obtiene el nombre completo (Nombre + Apellidos) de un usuario.
        /// Optimizado para construir DTOs sin cargar la entidad completa.
        /// </summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>Nombre completo del usuario o null si no existe.</returns>
        string? GetNombreCompletoById(int id);
    }
}
