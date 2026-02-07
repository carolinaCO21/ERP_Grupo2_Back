using API.Domain.Entities;
using API.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MockRepos
{
    /// <summary>
    /// Implementación mock del repositorio de pedidos.
    /// Proporciona datos en memoria para pruebas y desarrollo.
    /// </summary>
    public class MockPedidoRepository : IPedidoRepository
    {
        /// <summary>Colección en memoria de pedidos simulados.</summary>
        private readonly List<Pedido> _pedidos;

        /// <summary>Contador para generar IDs únicos.</summary>
        private int _nextId = 3;

        /// <summary>Contador para generar números de pedido secuenciales.</summary>
        private int _secuencial = 3;

        /// <summary>
        /// Inicializa el repositorio con datos de prueba predefinidos.
        /// </summary>
        public MockPedidoRepository()
        {
            _pedidos = new List<Pedido>
            {
                new Pedido("PED-2024-00001", 1, 1, "Almacén Central, Calle Industria 50, Madrid")
                {
                    Id = 1,
                    FechaPedido = DateTime.UtcNow.AddDays(-5),
                    Estado = "Pendiente",
                    Subtotal = 100.00m,
                    Impuestos = 21.00m,
                    Total = 121.00m
                },
                new Pedido("PED-2024-00002", 2, 2, "Obra Norte, Avda. Construcción 10, Barcelona")
                {
                    Id = 2,
                    FechaPedido = DateTime.UtcNow.AddDays(-2),
                    Estado = "Aprobado",
                    Subtotal = 500.00m,
                    Impuestos = 105.00m,
                    Total = 605.00m
                }
            };
        }

        /// <inheritdoc />
        public List<Pedido> GetAll()
        {
            return _pedidos.ToList();
        }

        /// <inheritdoc />
        public List<Pedido> GetByProveedorId(int proveedorId)
        {
            return _pedidos.Where(p => p.IdProveedor == proveedorId).ToList();
        }

        /// <inheritdoc />
        public List<Pedido> GetByEstado(string estado)
        {
            return _pedidos.Where(p => p.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <inheritdoc />
        public Pedido? GetById(int id)
        {
            return _pedidos.FirstOrDefault(p => p.Id == id);
        }

        /// <inheritdoc />
        public Pedido Create(Pedido pedido)
        {
            pedido.Id = _nextId++;
            _pedidos.Add(pedido);
            return pedido;
        }

        /// <inheritdoc />
        public Pedido Update(Pedido pedido)
        {
            var existente = _pedidos.FirstOrDefault(p => p.Id == pedido.Id);
            if (existente != null)
            {
                var index = _pedidos.IndexOf(existente);
                _pedidos[index] = pedido;
            }
            return pedido;
        }

        /// <inheritdoc />
        public bool Delete(int id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido != null)
            {
                _pedidos.Remove(pedido);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public string GenerateNumeroPedido()
        {
            var year = DateTime.UtcNow.Year;
            return $"PED-{year}-{_secuencial++:D5}";
        }
    }
}
