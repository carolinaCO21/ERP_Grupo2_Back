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
    /// Implementación mock del repositorio de líneas de pedido.
    /// Proporciona datos en memoria para pruebas y desarrollo.
    /// </summary>
    public class MockLineaPedidoRepository : ILineaPedidoRepository
    {
        /// <summary>Colección en memoria de líneas de pedido simuladas.</summary>
        private readonly List<LineaPedido> _lineasPedido;

        /// <summary>Contador para generar IDs únicos.</summary>
        private int _nextId = 5;

        /// <summary>
        /// Inicializa el repositorio con datos de prueba predefinidos.
        /// </summary>
        public MockLineaPedidoRepository()
        {
            _lineasPedido = new List<LineaPedido>
            {
                // Líneas del pedido 1
                new LineaPedido(1, 1, 500, 0.12m, 21.00m) { Id = 1 },
                new LineaPedido(1, 2, 500, 0.06m, 21.00m) { Id = 2 },
                // Líneas del pedido 2
                new LineaPedido(2, 4, 10, 42.00m, 21.00m) { Id = 3 },
                new LineaPedido(2, 5, 20, 5.00m, 21.00m) { Id = 4 }
            };
        }

        /// <inheritdoc />
        public List<LineaPedido> GetByPedidoId(int pedidoId)
        {
            return _lineasPedido.Where(l => l.IdPedido == pedidoId).ToList();
        }

        /// <inheritdoc />
        public LineaPedido Create(LineaPedido linea)
        {
            linea.Id = _nextId++;
            _lineasPedido.Add(linea);
            return linea;
        }

        /// <inheritdoc />
        public bool DeleteByPedidoId(int pedidoId)
        {
            var lineas = _lineasPedido.Where(l => l.IdPedido == pedidoId).ToList();
            foreach (var linea in lineas)
            {
                _lineasPedido.Remove(linea);
            }
            return true;
        }
    }
}
