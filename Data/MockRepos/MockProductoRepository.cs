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
    /// Implementación mock del repositorio de productos.
    /// Proporciona datos en memoria para pruebas y desarrollo.
    /// </summary>
    public class MockProductoRepository : IProductoRepository
    {

        /// <summary>Colección en memoria de productos simulados.</summary>
        private readonly List<Producto> _productos;

        /// <summary>
        /// Inicializa el repositorio con datos de prueba predefinidos.
        /// </summary>
        public MockProductoRepository()
        {
            _productos = new List<Producto>
            {
                new Producto("SKU-001", "Tornillo M8x50", "Tornillo métrico de acero inoxidable", "Ferretería", "Unidades", 0.15m, 5000) { Id = 1 },
                new Producto("SKU-002", "Tuerca M8", "Tuerca hexagonal de acero", "Ferretería", "Unidades", 0.08m, 8000) { Id = 2 },
                new Producto("SKU-003", "Cable eléctrico 2.5mm", "Cable unipolar flexible", "Electricidad", "Metros", 1.20m, 2000) { Id = 3 },
                new Producto("SKU-004", "Pintura blanca 15L", "Pintura plástica interior/exterior", "Pinturas", "Litros", 45.00m, 150) { Id = 4 },
                new Producto("SKU-005", "Cemento gris 25kg", "Cemento Portland para construcción", "Construcción", "Kg", 5.50m, 500) { Id = 5 }
            };
        }

        /// <inheritdoc />
        public List<Producto> GetAll()
        {
            return _productos.ToList();
        }

        /// <inheritdoc />
        public Producto? GetById(int id)
        {
            return _productos.FirstOrDefault(p => p.Id == id);
        }

        /// <inheritdoc />
        public string? GetNombreById(int id)
        {
            return _productos.FirstOrDefault(p => p.Id == id)?.Nombre;
        }




    }
}
