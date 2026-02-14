using API.Domain.Entities;
using API.Domain.Repos;
using Microsoft.Data.SqlClient;

namespace Data.DataBase.Repos
{
    public class ProveedorRepository : IProveedorRepository
    {
        public List<Proveedor> GetAll()
        {
            var proveedores = new List<Proveedor>();

            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, nombre_empresa, cif, direccion, ciudad, provincia, codigo_postal, telefono, email, activo FROM Proveedores",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                proveedores.Add(MapearProveedor(reader));
            }

            return proveedores;
        }

        public Proveedor? GetById(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, nombre_empresa, cif, direccion, ciudad, provincia, codigo_postal, telefono, email, activo FROM Proveedores WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearProveedor(reader);
            }

            return null;
        }

        public string? GetNombreById(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT nombre_empresa FROM Proveedores WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            var result = command.ExecuteScalar();
            return result?.ToString();
        }

        private Proveedor MapearProveedor(SqlDataReader reader)
        {
            return new Proveedor(
                nombreEmpresa: reader.GetString(1),
                cif: reader.GetString(2),
                direccion: reader.IsDBNull(3) ? "" : reader.GetString(3),
                ciudad: reader.IsDBNull(4) ? "" : reader.GetString(4),
                provincia: reader.IsDBNull(5) ? "" : reader.GetString(5),
                codigoPostal: reader.IsDBNull(6) ? "" : reader.GetString(6),
                telefono: reader.IsDBNull(7) ? "" : reader.GetString(7),
                email: reader.IsDBNull(8) ? "" : reader.GetString(8)
            )
            {
                Id = reader.GetInt32(0),
                Activo = reader.GetBoolean(9)
            };
        }
    }
}
