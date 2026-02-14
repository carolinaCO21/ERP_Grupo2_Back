using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataBase
{
    public class Conection
    {
        public static string GetConnectionString()
        {
            return "server=montano.database.windows.net;database=PersonaDB;uid=jamontano;pwd=Os1532008;trustServerCertificate = true;";
        }
    }
}