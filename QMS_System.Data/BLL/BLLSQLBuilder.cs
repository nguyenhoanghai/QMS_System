using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.BLL
{
   public class BLLSQLBuilder
    {
       #region constructor 
        static object key = new object();
        private static volatile BLLSQLBuilder _Instance;  
        public static BLLSQLBuilder Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLSQLBuilder();

                return _Instance;
            }
        }
        private BLLSQLBuilder() { }
        #endregion

        public bool Excecute(string query)
        {
            var rs = true;
            try
            {
                using (var db = new QMSSystemEntities())
                {
                    db.Database.ExecuteSqlCommand(query);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                rs = false;
            }
            return rs;
        }
    }
}
