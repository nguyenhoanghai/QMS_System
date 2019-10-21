using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLMajor
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLMajor _Instance;  //volatile =>  tranh dung thread
        public static BLLMajor Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLMajor();

                return _Instance;
            }
        }
        private BLLMajor() { }
        #endregion
        public List<MajorModel> Gets(string connectString )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    return db.Q_Major.Where(x => !x.IsDeleted).Select(x => new MajorModel() { Id = x.Id, Name = x.Name, Note = x.Note }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Major.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int Insert(string connectString,Q_Major obj )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_Major.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(string connectString,Q_Major model )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
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

        public bool Delete(string connectString,int Id )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Major model)
        {
            Q_Major obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
