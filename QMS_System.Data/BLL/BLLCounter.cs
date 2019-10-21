using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLCounter
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLCounter _Instance;  //volatile =>  tranh dung thread
        public static BLLCounter Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCounter();

                return _Instance;
            }
        }
        private BLLCounter() { }
        #endregion

        public List<CounterModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Counter.Where(x => !x.IsDeleted).OrderBy(x => x.Index).Select(x => new CounterModel()
                {
                    Id = x.Id,
                    ShortName = x.ShortName,
                    Name = x.Name,
                    Position = x.Position,
                    Index = x.Index,
                    Acreage = x.Acreage,  // Diện tích
                }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    return db.Q_Counter.Where(x => !x.IsDeleted).OrderBy(x => x.Index).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
                };
            }
            catch (Exception ex)
            { 
                throw ex;
            }

        }

        public int Insert(string connectString, Q_Counter obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_Counter.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public int Update(string connectString, Q_Counter model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Counter.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.ShortName = model.ShortName;
                        obj.Name = model.Name;
                        obj.Index = model.Index;
                        obj.Position = model.Position;
                        obj.Acreage = model.Acreage;
                        db.SaveChanges();
                        return model.Id;
                    }
                    else
                        return 0;
                }
                return 0;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Counter.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(Q_Counter model)
        {
            Q_Counter obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Counter.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
