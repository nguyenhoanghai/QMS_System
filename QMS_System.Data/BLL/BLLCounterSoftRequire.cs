using QMS_System.Data.Enum;
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

        public List<CounterSoftRequireModel> Gets(string connectString)
        {
            var list = new List<CounterSoftRequireModel>();
            using (db = new QMSSystemEntities(connectString))
            {
                var objs = db.Q_CounterSoftRequire.Where(x => x.TypeOfRequire != (int)eCounterSoftRequireType.SendSMS).ToList();
                if (objs.Count > 0)
                {
                    for (int i = 0; i < objs.Count; i++)
                    {
                        list.Add(new CounterSoftRequireModel() { Id = objs[i].Id, Content = objs[i].Content, Type = objs[i].TypeOfRequire });
                        if (objs[i].TypeOfRequire == (int)eCounterSoftRequireType.ReadSound ||
                            objs[i].TypeOfRequire == (int)eCounterSoftRequireType.SendNextToMainDisplay ||
                            objs[i].TypeOfRequire == (int)eCounterSoftRequireType.SendRecallToMainDisplay||
                            objs[i].TypeOfRequire == (int)eCounterSoftRequireType.CheckUserFree)
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
        public bool Insert(string connectString, string str, int requireType)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                db.Q_CounterSoftRequire.Add(new Q_CounterSoftRequire() { Content = str, TypeOfRequire = requireType });
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Insert(string connectString, string str, int requireType, int _counterId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                db.Q_CounterSoftRequire.Add(new Q_CounterSoftRequire() { Content = str, TypeOfRequire = requireType });
                if (requireType == (int)eCounterSoftRequireType.ReadSound)
                {
                    var config = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.TVReadSound);
                    if (config != null && config.Value == "1")
                        db.Q_TVReadSound.Add(new Q_TVReadSound()
                        {
                            CounterId = _counterId,
                            Sounds = str,
                            CreatedAt = DateTime.Now,
                            UsersReaded = "0,"
                        });
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public void DeleteAll(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
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

        public void Delete(int Id, string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_CounterSoftRequire.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    db.Q_CounterSoftRequire.Remove(obj);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// ktra co dang xu lý in phiếu hay goi next hay khong ?
        /// </summary>
        /// <returns></returns> 
        public Q_CounterSoftRequire HasProcessing(string connectString, int type)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_CounterSoftRequire.FirstOrDefault(x => x.TypeOfRequire == type);
            }
        }
    }
}
