using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLLanguage
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLLanguage _Instance;  //volatile =>  tranh dung thread
        public static BLLLanguage Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLanguage();

                return _Instance;
            }
        }
        private BLLLanguage() { }
        #endregion
        public List<LanguageModel> Gets(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_Language.Where(x => !x.IsDeleted).Select(x => new LanguageModel() { Id = x.Id, Name  = x.Name, Note = x.Note }).ToList();
        }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_Language.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
        }
        }
       
        public int Insert(string connectString,Q_Language obj)
        {
              using (db = new QMSSystemEntities(connectString)){
            if (!CheckExists(obj))
            {
                db.Q_Language.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }
        }

        public bool Update(string connectString,Q_Language model)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_Language.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                if (!CheckExists(model))
                {
                    obj.Name = model.Name;
                    obj.Note = model.Note;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        }

        public bool Delete(string connectString,int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Language.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Language model)
        { 
            Q_Language obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Language.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
