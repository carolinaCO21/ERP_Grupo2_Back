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
    /// Implementación mock del repositorio de relaciones producto-proveedor.
    /// Proporciona datos en memoria para pruebas y desarrollo.
    /// </summary>
    public class MockProductoProveedorRepository : IProductoProveedorRepository
    {
        /// <summary>Colección en memoria de relaciones producto-proveedor simuladas.</summary>
        private readonly List<ProductoProveedor> _productosProveedores;

        /// <summary>
        /// Inicializa el repositorio con datos de prueba predefinidos.
        /// </summary>
        public MockProductoProveedorRepository()
        {
            _productosProveedores = new List<ProductoProveedor>
            {
                // Proveedor 1 ofrece tornillos y tuercas
                new ProductoProveedor(1, 1, 0.12m) { Id = 1 },
                new ProductoProveedor(1, 2, 0.06m) { Id = 2 },
                // Proveedor 2 ofrece cemento y pintura
                new ProductoProveedor(2, 4, 42.00m) { Id = 3 },
                new ProductoProveedor(2, 5, 5.00m) { Id = 4 },
                // Proveedor 3 ofrece cable eléctrico y tornillos
                new ProductoProveedor(3, 3, 1.10m) { Id = 5 },
                new ProductoProveedor(3, 1, 0.14m) { Id = 6 }
            };
        }

        /// <inheritdoc />
        public List<ProductoProveedor> GetByProveedorId(int proveedorId)
        {
            return _productosProveedores.Where(pp => pp.IdProveedor == proveedorId).ToList();
        }

        /// <inheritdoc />
        public ProductoProveedor? GetByProveedorAndProducto(int proveedorId, int productoId)
        {
            return _productosProveedores.FirstOrDefault(pp => pp.IdProveedor == proveedorId && pp.IdProducto == productoId);
        }
    }
}
