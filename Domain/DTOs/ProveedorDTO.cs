using API.Domain.Entities;

namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO para mostrar información de proveedores en la API.
    /// Expone únicamente los datos necesarios para el frontend.
    /// </summary>
    public class ProveedorDTO
    {
        /// <summary>Identificador del proveedor.</summary>
        public int Id { get; }

        /// <summary>Nombre comercial de la empresa.</summary>
        public string NombreEmpresa { get; }

        /// <summary>CIF/NIF de la empresa.</summary>
        public string Cif { get; }

        /// <summary>Teléfono de contacto.</summary>
        public string Telefono { get; }

        /// <summary>Email de contacto.</summary>
        public string Email { get; }

        /// <summary>Indica si el proveedor está activo.</summary>
        public bool Activo { get; }

        /// <summary>
        /// Construye el DTO a partir de una entidad Proveedor.
        /// </summary>
        /// <param name="proveedor">Entidad proveedor origen.</param>
        public ProveedorDTO(Proveedor proveedor)
        {
            Id = proveedor.Id;
            NombreEmpresa = proveedor.NombreEmpresa;
            Cif = proveedor.Cif;
            Telefono = proveedor.Telefono;
            Email = proveedor.Email;
            Activo = proveedor.Activo;
        }
    }
}
