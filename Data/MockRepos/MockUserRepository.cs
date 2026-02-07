using API.Domain.Entities;
using API.Domain.Repos;

namespace Data.MockRepos
{
    /// <summary>
    /// Implementación mock del repositorio de usuarios.
    /// Proporciona datos en memoria para pruebas y desarrollo.
    /// </summary>
    public class MockUserRepository : IUserRepository
    {
        /// <summary>Colección en memoria de usuarios simulados.</summary>
        private readonly List<User> _users;

        /// <summary>
        /// Inicializa el repositorio con datos de prueba predefinidos.
        /// </summary>
        public MockUserRepository()
        {
            _users = new List<User>
            {
                new User("firebase_uid_001", "admin@erp.com", "Carlos", "García López", "Admin") { Id = 1 },
                new User("firebase_uid_002", "usuario@erp.com", "María", "Fernández Ruiz", "Usuario") { Id = 2 },
                new User("firebase_uid_003", "supervisor@erp.com", "Juan", "Martínez Sánchez", "Supervisor") { Id = 3 }
            };
        }

        /// <inheritdoc />
        public User? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc />
        public User? GetByFirebaseUid(string firebaseUid)
        {
            return _users.FirstOrDefault(u => u.FirebaseUid == firebaseUid);
        }

        /// <inheritdoc />
        public string? GetNombreCompletoById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user != null ? $"{user.Nombre} {user.Apellidos}" : null;
        }
    }
}
