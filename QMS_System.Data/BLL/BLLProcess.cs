using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLProcess
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLProcess _Instance;  //volatile =>  tranh dung thread
        public static BLLProcess Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLProcess();

                return _Instance;
            }
        }
        private BLLProcess() { }
        #endregion
        public List<ProcessModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Process.Where(x => !x.IsDeleted).Select(x => new ProcessModel() { Id = x.Id, Name = x.Name, Index = x.Index, Note = x.Note }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Process.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int Insert(Q_Process obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_Process.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }
        public bool Update(Q_Process model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Process.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Name = model.Name;
                        obj.Index = model.Index;
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
                var obj = db.Q_Process.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Process model)
        {
            Q_Process obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Process.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
        public int GetLastIndex()
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Process.Where(x => !x.IsDeleted).OrderByDescending(x => x.Index).FirstOrDefault();
                return obj != null ? obj.Index : 0;
            }
        }
    }
}
