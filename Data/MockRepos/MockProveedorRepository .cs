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
    /// Implementación mock del repositorio de proveedores.
    /// Proporciona datos en memoria para pruebas y desarrollo.
    /// </summary>
    public class MockProveedorRepository : IProveedorRepository
    {

        /// <summary>Colección en memoria de proveedores simulados.</summary>
        private readonly List<Proveedor> _proveedores;

        /// <summary>
        /// Inicializa el repositorio con datos de prueba predefinidos.
        /// </summary>
        public MockProveedorRepository()
        {
            _proveedores = new List<Proveedor>
            {
                new Proveedor("Suministros Industriales S.L.", "B12345678", "Calle Mayor 10", "Madrid", "Madrid", "28001", "912345678", "contacto@suministros.es") { Id = 1 },
                new Proveedor("Materiales Construcción S.A.", "A87654321", "Avda. Industria 25", "Barcelona", "Barcelona", "08001", "932345678", "ventas@materiales.es") { Id = 2 },
                new Proveedor("Herramientas del Norte S.L.", "B11223344", "Polígono Industrial 5", "Bilbao", "Vizcaya", "48001", "944567890", "info@herramientasnorte.es") { Id = 3 }
            };
        }

        /// <inheritdoc />
        public List<Proveedor> GetAll()
        {
            return _proveedores.ToList();
        }

        /// <inheritdoc />
        public Proveedor? GetById(int id)
        {
            return _proveedores.FirstOrDefault(p => p.Id == id);
        }

        /// <inheritdoc />
        public string? GetNombreById(int id)
        {
            return _proveedores.FirstOrDefault(p => p.Id == id)?.NombreEmpresa;
        }


    }
}
