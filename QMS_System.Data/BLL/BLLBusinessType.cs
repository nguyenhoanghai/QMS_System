using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLBusinessType
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLBusinessType _Instance;  //volatile =>  tranh dung thread
        public static BLLBusinessType Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLBusinessType();

                return _Instance;
            }
        }
        private BLLBusinessType() { }
        #endregion
        public List<BusinessTypeModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_BusinessType.Where(x => !x.IsDeleted).Select(x => new BusinessTypeModel() { Id = x.Id, Name = x.Name, Note = x.Note }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_BusinessType.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int Insert(Q_BusinessType obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_BusinessType.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(Q_BusinessType model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_BusinessType.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Name = model.Name;
                        obj.Note = model.Note;
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
        }

        public bool Delete(int Id)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_BusinessType.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_BusinessType model)
        {
            Q_BusinessType obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_BusinessType.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
