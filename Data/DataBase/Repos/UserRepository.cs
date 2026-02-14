using API.Domain.Entities;
using API.Domain.Repos;
using Microsoft.Data.SqlClient;

namespace Data.DataBase.Repos
{
    public class UserRepository : IUserRepository
    {
        public User? GetById(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, username, email, nombre, apellidos, rol, activo FROM Users WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearUsuario(reader);
            }

            return null;
        }

        public User? GetByFirebaseUid(string firebaseUid)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, username, email, nombre, apellidos, rol, activo FROM Users WHERE username = @FirebaseUid",
                connection);
            command.Parameters.AddWithValue("@FirebaseUid", firebaseUid);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearUsuario(reader);
            }

            return null;
        }

        public string? GetNombreCompletoById(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT nombre, apellidos FROM Users WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var nombre = reader.GetString(0);
                var apellidos = reader.GetString(1);
                return $"{nombre} {apellidos}";
            }

            return null;
        }

        private User MapearUsuario(SqlDataReader reader)
        {
            return new User(
                firebaseUid: reader.GetString(1),
                email: reader.GetString(2),
                nombre: reader.IsDBNull(3) ? "" : reader.GetString(3),
                apellidos: reader.IsDBNull(4) ? "" : reader.GetString(4),
                rol: reader.IsDBNull(5) ? "" : reader.GetString(5)
            )
            {
                Id = reader.GetInt32(0),
                Activo = reader.GetBoolean(6)
            };
        }
    }
}
