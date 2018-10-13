using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLPolicy
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLPolicy _Instance;  //volatile =>  tranh dung thread
        public static BLLPolicy Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPolicy();

                return _Instance;
            }
        }
        private BLLPolicy() { }
        #endregion
        public List<PolicyModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Policy.Where(x => !x.IsDeleted).Select(x => new PolicyModel() { Id = x.Id, Name = x.Name, Note = x.Note, IsActived = x.IsActived }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Policy.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int Insert(Q_Policy obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_Policy.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(Q_Policy model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Policy.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Name = model.Name;
                        obj.IsActived = model.IsActived;
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
                var obj = db.Q_Policy.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Policy model)
        {
            Q_Policy obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Policy.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
