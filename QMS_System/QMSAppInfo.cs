using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System
{
   public static class QMSAppInfo
    {
        public static int Version { get; set; }
        public static string ConnectString { get; set; }
        public static SqlConnection sqlConnection { get; set; }
    }
}
