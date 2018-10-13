using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLAlert
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLAlert _Instance;  //volatile =>  tranh dung thread
        public static BLLAlert Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLAlert();

                return _Instance;
            }
        }
        private BLLAlert() { }
        #endregion
        public List<AlertModel> Gets()
        {
              using (db = new QMSSystemEntities()){
            return db.Q_Alert.Where(x => !x.IsDeleted).Select(x => new AlertModel() { Id = x.Id, Note = x.Note, SoundId = x.SoundId, Start = x.Start, End = x.End }).ToList();
        }}

        //public List<ModelSelectItem> GetLookUp()
        //{
        //      using (db = new QMSSystemEntities()){
        //    return db.Q_Alert.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
        //}
       
        public int Insert(Q_Alert obj)
        {
              using (db = new QMSSystemEntities()){
            if (!CheckExist(obj))
            {
                db.Q_Alert.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }}

        public bool Update(Q_Alert model)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_Alert.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                if(!CheckExist(model))
                {
                    obj.SoundId = model.SoundId;
                    obj.Note = model.Note;
                    obj.Start = model.Start;
                    obj.End = model.End;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }}

        public bool Delete(int Id)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_Alert.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
              }
        }

        private bool CheckExist(Q_Alert model)
        { 
            var obj = db.Q_Alert.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.SoundId == model.SoundId);
            return obj != null ? true : false;
        }
    }
}
