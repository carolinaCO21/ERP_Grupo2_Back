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
                "SELECT id, username, email, password, nombre, apellidos, rol, activo FROM Users WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearUsuario(reader);
            }

            return null;
        }

        public User? GetByUsername(string username)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, username, email, password, nombre, apellidos, rol, activo FROM Users WHERE username = @Username",
                connection);
            command.Parameters.AddWithValue("@Username", username);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearUsuario(reader);
            }

            return null;
        }

        public User? GetByEmail(string email)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, username, email, password, nombre, apellidos, rol, activo FROM Users WHERE email = @Email",
                connection);
            command.Parameters.AddWithValue("@Email", email);

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
                var nombre = reader.IsDBNull(0) ? "" : reader.GetString(0);
                var apellidos = reader.IsDBNull(1) ? "" : reader.GetString(1);
                return $"{nombre} {apellidos}".Trim();
            }

            return null;
        }

        private User MapearUsuario(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.IsDBNull(3) ? null : reader.GetString(3),
                Nombre = reader.IsDBNull(4) ? null : reader.GetString(4),
                Apellidos = reader.IsDBNull(5) ? null : reader.GetString(5),
                Rol = reader.IsDBNull(6) ? null : reader.GetString(6),
                Activo = !reader.IsDBNull(7) && reader.GetBoolean(7)
            };
        }
    }
}
