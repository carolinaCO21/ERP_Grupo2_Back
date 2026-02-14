using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Data.DataBase
{
    public class Conection
    {
        private static IConfiguration? _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConnectionString()
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException(
                    "La configuración no ha sido inicializada. Llama a Conection.Initialize() desde Program.cs");
            }

            return _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión 'DefaultConnection' en appsettings.json");
        }
    }
}