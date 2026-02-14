using API.Domain.Entities;
using API.Domain.Repos;
using Microsoft.Data.SqlClient;

namespace Data.DataBase.Repos
{
    public class ProductoRepository : IProductoRepository
    {
        public List<Producto> GetAll()
        {
            var productos = new List<Producto>();

            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, codigo_producto, nombre, descripcion, categoria, unidad_medida, precio, stock_actual, activo FROM Productos",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                productos.Add(MapearProducto(reader));
            }

            return productos;
        }

        public Producto? GetById(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, codigo_producto, nombre, descripcion, categoria, unidad_medida, precio, stock_actual, activo FROM Productos WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearProducto(reader);
            }

            return null;
        }

        public string? GetNombreById(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT nombre FROM Productos WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            var result = command.ExecuteScalar();
            return result?.ToString();
        }

        private Producto MapearProducto(SqlDataReader reader)
        {
            return new Producto(
                codigoProducto: reader.GetString(1),
                nombre: reader.GetString(2),
                descripcion: reader.IsDBNull(3) ? "" : reader.GetString(3),
                categoria: reader.IsDBNull(4) ? "" : reader.GetString(4),
                unidadMedida: reader.IsDBNull(5) ? "" : reader.GetString(5),
                precio: reader.GetDecimal(6),
                stockActual: reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
            )
            {
                Id = reader.GetInt32(0),
                Activo = reader.GetBoolean(8)
            };
        }
    }
}
