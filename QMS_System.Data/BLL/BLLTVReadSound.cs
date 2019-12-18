using System;
using System.Collections.Generic;
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
                    var _id = ( "," +userId);
                    var now = DateTime.Now;
                    now = now.AddMinutes(-2);
                    var items = db.Q_TVReadSound.Where(x => counterIds.Contains(x.CounterId.Value) && !x.UsersReaded.Contains(_id)&& x.CreatedAt > now).ToList();
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

}