using API.Domain.Entities;
using API.Domain.Repos;
using Microsoft.Data.SqlClient;

namespace Data.DataBase.Repos
{
    public class ProductoProveedorRepository : IProductoProveedorRepository
    {
        public List<ProductoProveedor> GetByProveedorId(int proveedorId)
        {
            var relaciones = new List<ProductoProveedor>();

            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, id_proveedor, id_producto, precio_unitario, activo FROM Productos_Proveedor WHERE id_proveedor = @ProveedorId",
                connection);
            command.Parameters.AddWithValue("@ProveedorId", proveedorId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                relaciones.Add(MapearProductoProveedor(reader));
            }

            return relaciones;
        }

        public ProductoProveedor? GetByProveedorAndProducto(int proveedorId, int productoId)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, id_proveedor, id_producto, precio_unitario, activo FROM Productos_Proveedor WHERE id_proveedor = @ProveedorId AND id_producto = @ProductoId",
                connection);
            command.Parameters.AddWithValue("@ProveedorId", proveedorId);
            command.Parameters.AddWithValue("@ProductoId", productoId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearProductoProveedor(reader);
            }

            return null;
        }

        private ProductoProveedor MapearProductoProveedor(SqlDataReader reader)
        {
            return new ProductoProveedor(
                idProveedor: reader.GetInt32(1),
                idProducto: reader.GetInt32(2),
                precioUnitario: reader.GetDecimal(3)
            )
            {
                Id = reader.GetInt32(0),
                Activo = reader.GetBoolean(4)
            };
        }
    }
}
