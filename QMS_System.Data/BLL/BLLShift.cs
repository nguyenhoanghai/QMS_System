using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLShift
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLShift _Instance;  //volatile =>  tranh dung thread
        public static BLLShift Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLShift();

                return _Instance;
            }
        }
        private BLLShift() { }
        #endregion
        public List<ShiftModel> Gets(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_Shift.Where(x => !x.IsDeleted).Select(x => new ShiftModel() { Id = x.Id, Name = x.Name, Note = x.Note, Start = x.Start, End = x.End }).ToList();
        }}

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_Shift.Where(x => !x.IsDeleted).ToList().Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name, Code = (x.Start.ToString("HH:mm")+" - "+x.End.ToString("HH:mm")) }).ToList();
        }}
       
        public int Insert(string connectString,Q_Shift obj)
        {
              using (db = new QMSSystemEntities(connectString)){
            if (!CheckExists(obj))
            {
                db.Q_Shift.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }}

        public bool Update(string connectString,Q_Shift model)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_Shift.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                if (!CheckExists(model))
                {
                    obj.Name = model.Name;
                    obj.Start = model.Start;
                    obj.End = model.End;
                    obj.Note = model.Note;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }}

        public bool Delete(string connectString, int Id)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_Shift.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
              }
        }

        private bool CheckExists(Q_Shift model)
        { 
            var obj = db.Q_Shift.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
