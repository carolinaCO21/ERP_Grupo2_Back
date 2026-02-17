using API.Domain.Entities;
using API.Domain.Repos;
using Microsoft.Data.SqlClient;

namespace Data.DataBase.Repos
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ILineaPedidoRepository _lineaPedidoRepository;

        public PedidoRepository(ILineaPedidoRepository lineaPedidoRepository)
        {
            _lineaPedidoRepository = lineaPedidoRepository;
        }

        public List<Pedido> GetAll()
        {
            var pedidos = new List<Pedido>();

            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, numero_pedido, id_proveedor, id_usuario, fecha_pedido, estado, subtotal, impuestos, total, direccion_entrega FROM Pedidos",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pedidos.Add(MapearPedido(reader));
            }

            foreach (var pedido in pedidos)
            {
                pedido.LineasPedido = _lineaPedidoRepository.GetByPedidoId(pedido.Id);
            }

            return pedidos;
        }

        public List<Pedido> GetByProveedorId(int proveedorId)
        {
            var pedidos = new List<Pedido>();

            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, numero_pedido, id_proveedor, id_usuario, fecha_pedido, estado, subtotal, impuestos, total, direccion_entrega FROM Pedidos WHERE id_proveedor = @ProveedorId",
                connection);
            command.Parameters.AddWithValue("@ProveedorId", proveedorId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pedidos.Add(MapearPedido(reader));
            }

            foreach (var pedido in pedidos)
            {
                pedido.LineasPedido = _lineaPedidoRepository.GetByPedidoId(pedido.Id);
            }

            return pedidos;
        }

        public List<Pedido> GetByEstado(string estado)
        {
            var pedidos = new List<Pedido>();

            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, numero_pedido, id_proveedor, id_usuario, fecha_pedido, estado, subtotal, impuestos, total, direccion_entrega FROM Pedidos WHERE estado = @Estado",
                connection);
            command.Parameters.AddWithValue("@Estado", estado);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pedidos.Add(MapearPedido(reader));
            }

            foreach (var pedido in pedidos)
            {
                pedido.LineasPedido = _lineaPedidoRepository.GetByPedidoId(pedido.Id);
            }

            return pedidos;
        }

        public Pedido? GetById(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "SELECT id, numero_pedido, id_proveedor, id_usuario, fecha_pedido, estado, subtotal, impuestos, total, direccion_entrega FROM Pedidos WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var pedido = MapearPedido(reader);
                reader.Close();
                pedido.LineasPedido = _lineaPedidoRepository.GetByPedidoId(pedido.Id);
                return pedido;
            }

            return null;
        }

        public Pedido Create(Pedido pedido)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                @"INSERT INTO Pedidos (numero_pedido, id_proveedor, id_usuario, fecha_pedido, estado, subtotal, impuestos, total, direccion_entrega)
                  VALUES (@NumeroPedido, @IdProveedor, @IdUsuario, @FechaPedido, @Estado, @Subtotal, @Impuestos, @Total, @DireccionEntrega);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                connection);

            command.Parameters.AddWithValue("@NumeroPedido", pedido.NumeroPedido);
            command.Parameters.AddWithValue("@IdProveedor", pedido.IdProveedor);
            command.Parameters.AddWithValue("@IdUsuario", pedido.IdUsuario);
            command.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
            command.Parameters.AddWithValue("@Estado", pedido.Estado);
            command.Parameters.AddWithValue("@Subtotal", pedido.Subtotal);
            command.Parameters.AddWithValue("@Impuestos", pedido.Impuestos);
            command.Parameters.AddWithValue("@Total", pedido.Total);
            command.Parameters.AddWithValue("@DireccionEntrega", pedido.DireccionEntrega);

            pedido.Id = (int)command.ExecuteScalar();

            return pedido;
        }

        public Pedido Update(Pedido pedido)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                @"UPDATE Pedidos 
                  SET numero_pedido = @NumeroPedido,
                      id_proveedor = @IdProveedor,
                      id_usuario = @IdUsuario,
                      fecha_pedido = @FechaPedido,
                      estado = @Estado,
                      subtotal = @Subtotal,
                      impuestos = @Impuestos,
                      total = @Total,
                      direccion_entrega = @DireccionEntrega
                  WHERE id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", pedido.Id);
            command.Parameters.AddWithValue("@NumeroPedido", pedido.NumeroPedido);
            command.Parameters.AddWithValue("@IdProveedor", pedido.IdProveedor);
            command.Parameters.AddWithValue("@IdUsuario", pedido.IdUsuario);
            command.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
            command.Parameters.AddWithValue("@Estado", pedido.Estado);
            command.Parameters.AddWithValue("@Subtotal", pedido.Subtotal);
            command.Parameters.AddWithValue("@Impuestos", pedido.Impuestos);
            command.Parameters.AddWithValue("@Total", pedido.Total);
            command.Parameters.AddWithValue("@DireccionEntrega", pedido.DireccionEntrega);

            command.ExecuteNonQuery();

            return pedido;
        }

        public bool Delete(int id)
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM Pedidos WHERE id = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        public string GenerateNumeroPedido()
        {
            using var connection = new SqlConnection(Conection.GetConnectionString());
            connection.Open();

            var year = DateTime.UtcNow.Year;
            
            // Usar transacción para garantizar atomicidad
            using var transaction = connection.BeginTransaction();
            
            try
            {
                var command = new SqlCommand(
                    @"SELECT TOP 1 numero_pedido 
                      FROM Pedidos WITH (UPDLOCK, HOLDLOCK)
                      WHERE numero_pedido LIKE @Pattern 
                      ORDER BY numero_pedido DESC",
                    connection, transaction);
                
                command.Parameters.AddWithValue("@Pattern", $"PED-{year}-%");

                var ultimoNumero = command.ExecuteScalar() as string;
                int secuencial = 1;

                if (!string.IsNullOrEmpty(ultimoNumero))
                {
                    // Extraer el número secuencial del formato PED-2026-00005
                    var partes = ultimoNumero.Split('-');
                    if (partes.Length == 3 && int.TryParse(partes[2], out var ultimoSecuencial))
                    {
                        secuencial = ultimoSecuencial + 1;
                    }
                }

                transaction.Commit();
                return $"PED-{year}-{secuencial:D5}";
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private Pedido MapearPedido(SqlDataReader reader)
        {
            var pedido = new Pedido
            {
                Id = reader.GetInt32(0),
                NumeroPedido = reader.GetString(1),
                IdProveedor = reader.GetInt32(2),
                IdUsuario = reader.GetInt32(3),
                FechaPedido = reader.GetDateTime(4),
                Estado = reader.GetString(5),
                Subtotal = reader.GetDecimal(6),
                Impuestos = reader.GetDecimal(7),
                Total = reader.GetDecimal(8),
                DireccionEntrega = reader.GetString(9)
            };

            return pedido;
        }
    }
}
