using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLStatusType
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLStatusType _Instance;  //volatile =>  tranh dung thread
        public static BLLStatusType Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLStatusType();

                return _Instance;
            }
        }
        private BLLStatusType() { }
        #endregion
        public List<StatusTypeModel> Gets()
        {
              using (db = new QMSSystemEntities()){
            return db.Q_StatusType.Select(x => new StatusTypeModel() { Id = x.Id, Name = x.Name, Note = x.Note  }).ToList();
        }
        }

        public List<ModelSelectItem> GetLookUp()
        {
              using (db = new QMSSystemEntities()){
            return db.Q_StatusType.Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
        }}
       
        public int Insert(Q_StatusType obj)
        {
              using (db = new QMSSystemEntities()){
            if (!CheckExists(obj))
            {
                db.Q_StatusType.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }
        }

        public bool Update(Q_StatusType model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_StatusType.FirstOrDefault(x => x.Id == model.Id);
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

        //public bool Delete(int Id)
        //{
        //      using (db = new QMSSystemEntities()){
        //    var obj = db.Q_StatusType.FirstOrDefault(x => x.Id == Id);
        //    if (obj != null)
        //    {
        //        obj.IsDeleted = true;
        //        db.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
        private bool CheckExists(Q_StatusType model)
        { 
            Q_StatusType obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_StatusType.FirstOrDefault(x =>   x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
