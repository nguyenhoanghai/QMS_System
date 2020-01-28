using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QMS_System.Data.BLL
{

    public class BLLTVReadSound
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLTVReadSound _Instance;
        public static BLLTVReadSound Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLTVReadSound();

                return _Instance;
            }
        }
        private BLLTVReadSound() { }
        #endregion

        public List<string> Gets(string connectString, int[] counterIds, int userId)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    List<string> returnStr = new List<string>();
                    var _id = ("," + userId);
                    var now = DateTime.Now;
                    now = now.AddMinutes(-2);
                    var items = db.Q_TVReadSound.Where(x => counterIds.Contains(x.CounterId.Value) && !x.UsersReaded.Contains(_id) && x.CreatedAt > now).ToList();
                    if (items.Count > 0)
                    {
                        foreach (var x in items)
                        {
                            x.UsersReaded = (x.UsersReaded + "," + userId);
                            returnStr.Add(x.Sounds);
                        }
                        db.SaveChanges();
                    }
                    return returnStr;
                }
            }
            catch (Exception ex)
            {
                // throw ex.InnerException;
            }
            return new List<string>();
        }
        /*
        public List<string> Gets(SqlConnection sqlConnection, int[] counterIds, int userId)
        {
            try
            {
                var now = DateTime.Now;
                now = now.AddMinutes(-2);
                var query = "select * from Q_TVReadSound where UsersReaded not like '%,"+userId+",%' and UsersReaded not like '%,"+userId+"' and CounterId in ("+string.Join(",",counterIds)+") and CreatedAt >"+now;
                var da = new SqlDataAdapter(query, sqlConnection);
                var dataTable = new DataTable();



                using (db = new QMSSystemEntities(connectString))
                {
                    List<string> returnStr = new List<string>();
                    var _id = ("," + userId);
                    var now = DateTime.Now;
                    now = now.AddMinutes(-2);
                    var items = db.Q_TVReadSound.Where(x => counterIds.Contains(x.CounterId.Value) && !x.UsersReaded.Contains(_id) && x.CreatedAt > now).ToList();
                    if (items.Count > 0)
                    {
                        foreach (var x in items)
                        {
                            x.UsersReaded = (x.UsersReaded + "," + userId);
                            returnStr.Add(x.Sounds);
                        }
                        db.SaveChanges();
                    }
                    return returnStr;
                }
            }
            catch (Exception ex)
            {
                // throw ex.InnerException;
            }
            return new List<string>();
        }
    }
    */
    }
}