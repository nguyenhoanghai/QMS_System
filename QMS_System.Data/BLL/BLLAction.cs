using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLAction
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLAction _Instance;  //volatile =>  tranh dung thread
        public static BLLAction Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLAction();

                return _Instance;
            }
        }
        private BLLAction() { }
        #endregion
        public List<ActionModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Action.Where(x => !x.IsDeleted).Select(x => new ActionModel() { Id = x.Id, Code = x.Code, Index = x.Index, Function = x.Function, Note = x.Note }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Action.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
            }
        }

        public int Insert(Q_Action obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_Action.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(Q_Action model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Action.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Code = model.Code;
                        obj.Index = model.Index;
                        obj.Function = model.Function;
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
                var obj = db.Q_Action.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(Q_Action model)
        {
            Q_Action obj = null;
            if (!string.IsNullOrEmpty(model.Code))
                obj = db.Q_Action.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Code.Trim().ToUpper().Equals(model.Code.Trim().ToUpper()));
            return obj != null ? true : false;
        }
        public int GetLastIndex()
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Action.Where(x => !x.IsDeleted).OrderByDescending(x => x.Index).FirstOrDefault();
                return obj != null ? obj.Index : 0;
            }
        }
    }
}
