using API.Domain.Entities;
using API.Domain.Repos;
using Microsoft.Data.SqlClient;

namespace Data.DataBase.Repos
{
    public class LineaPedidoRepository : ILineaPedidoRepository
    {
        public List<LineaPedido> GetByPedidoId(int pedidoId)
        {
            var lineas = new List<LineaPedido>();

            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, id_pedido, id_producto, cantidad, precio_unitario, subtotal, iva_porcentaje, total_linea FROM Lineas_Pedido WHERE id_pedido = @PedidoId",
                connection);
            command.Parameters.AddWithValue("@PedidoId", pedidoId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lineas.Add(MapearLineaPedido(reader));
            }

            return lineas;
        }

        public LineaPedido Create(LineaPedido linea)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                @"INSERT INTO Lineas_Pedido (id_pedido, id_producto, cantidad, precio_unitario, subtotal, iva_porcentaje, total_linea)
                  VALUES (@IdPedido, @IdProducto, @Cantidad, @PrecioUnitario, @Subtotal, @IvaPorcentaje, @TotalLinea);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                connection);

            command.Parameters.AddWithValue("@IdPedido", linea.IdPedido);
            command.Parameters.AddWithValue("@IdProducto", linea.IdProducto);
            command.Parameters.AddWithValue("@Cantidad", linea.Cantidad);
            command.Parameters.AddWithValue("@PrecioUnitario", linea.PrecioUnitario);
            command.Parameters.AddWithValue("@Subtotal", linea.Subtotal);
            command.Parameters.AddWithValue("@IvaPorcentaje", linea.IvaPorcentaje);
            command.Parameters.AddWithValue("@TotalLinea", linea.TotalLinea);

            linea.Id = (int)command.ExecuteScalar();

            return linea;
        }

        public bool DeleteByPedidoId(int pedidoId)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM Lineas_Pedido WHERE id_pedido = @PedidoId",
                connection);
            command.Parameters.AddWithValue("@PedidoId", pedidoId);

            command.ExecuteNonQuery();
            return true;
        }

        private LineaPedido MapearLineaPedido(SqlDataReader reader)
        {
            var linea = new LineaPedido
            {
                Id = reader.GetInt32(0),
                IdPedido = reader.GetInt32(1),
                IdProducto = reader.GetInt32(2),
                Cantidad = reader.GetInt32(3),
                PrecioUnitario = reader.GetDecimal(4),
                Subtotal = reader.GetDecimal(5),
                IvaPorcentaje = reader.GetDecimal(6),
                TotalLinea = reader.GetDecimal(7)
            };

            return linea;
        }
    }
}
