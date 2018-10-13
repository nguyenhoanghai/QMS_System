using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLCounterSoftRequire
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLCounterSoftRequire _Instance;  //volatile =>  tranh dung thread
        public static BLLCounterSoftRequire Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCounterSoftRequire();

                return _Instance;
            }
        }
        private BLLCounterSoftRequire() { }
        #endregion
        public List<CounterSoftRequireModel> Gets()
        {
           var  list = new List<CounterSoftRequireModel>();
            using (db = new QMSSystemEntities())
            {
                var objs = db.Q_CounterSoftRequire.ToList(); 
                if (objs.Count > 0)
                {
                    for (int i = 0; i < objs.Count; i++)
                    {
                      //  list.AddRange(objs[i].Content.Split('|').ToList());
                        list.Add(new CounterSoftRequireModel() { Content = objs[i].Content, Type = objs[i].TypeOfRequire });
                        db.Q_CounterSoftRequire.Remove(objs[i]);
                    }
                    db.SaveChanges();
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="requireType">0:read sound , 1 print Ticket</param>
        /// <returns></returns>
        public bool Insert(string str, int requireType)
        {
            using (db = new QMSSystemEntities())
            {
                db.Q_CounterSoftRequire.Add(new Q_CounterSoftRequire() { Content = str, TypeOfRequire = requireType });
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public void DeleteAll()
        {
            using (db = new QMSSystemEntities())
            {
                var objs = db.Q_CounterSoftRequire.ToList();
                if (objs.Count > 0)
                {
                    for (int i = 0; i < objs.Count; i++)
                        db.Q_CounterSoftRequire.Remove(objs[i]);
                    db.SaveChanges();
                }
            }
        }
    }
}
