using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL_Notification
{
    class SQLTask
    {

        public static string conString;
        public static string conStringCCL = "data source=imapcentral;Initial Catalog=COCKPIT;User ID=sa;Password=Test123;TrustServerCertificate=true; MultipleActiveResultSets=true;";

        public static string GetConnection()
        {
           
            conString = conStringCCL;

            return conString;
        }
    }
}
