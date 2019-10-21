using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLSound
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLSound _Instance;  //volatile =>  tranh dung thread
        public static BLLSound Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLSound();

                return _Instance;
            }
        }
        private BLLSound() { }
        #endregion
        public List<SoundModel> Gets(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_Sound.Where(x => !x.IsDeleted).Select(x => new SoundModel() { Id = x.Id,Code = x.Code, Name = x.Name, LanguageId = x.LanguageId, Note = x.Note }).ToList();
        }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_Sound.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = (x.Name +" ("+ x.Q_Language.Name+")") }).ToList();
        }
       
        }
        public int Insert(string connectString,Q_Sound obj)
        {
              using (db = new QMSSystemEntities(connectString)){
            db.Q_Sound.Add(obj);
            db.SaveChanges();
            return obj.Id;
        }
        }

        public bool Update(string connectString,Q_Sound model)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_Sound.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                obj.Code = model.Code;
                obj.Name = model.Name;
                obj.LanguageId = model.LanguageId;
                obj.Note = model.Note; 
                db.SaveChanges();
                return true;
            }
            return false;
        }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Sound.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
