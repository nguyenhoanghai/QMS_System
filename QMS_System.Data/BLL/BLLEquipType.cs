using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLEquipType
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLEquipType _Instance;  //volatile =>  tranh dung thread
        public static BLLEquipType Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLEquipType();

                return _Instance;
            }
        }
        private BLLEquipType() { }
        #endregion
        public List<EquipTypeModel> Gets() // load len gridview
        {
              using (db = new QMSSystemEntities()){
            return db.Q_EquipmentType.Where(x => !x.IsDeleted).Select(x => new EquipTypeModel() 
            {
                Id = x.Id, 
                Name = x.Name, 
                Note = x.Note
            }).ToList();
        }}

        public List<ModelSelectItem> GetLookUp()  // dua len combobox
        {
              using (db = new QMSSystemEntities()){
            return db.Q_EquipmentType.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
        }}
       
        public int Insert(Q_EquipmentType obj)  // them
        {
              using (db = new QMSSystemEntities()){
            if(!CheckExists(obj))
            {
                db.Q_EquipmentType.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }}

        public bool Update(Q_EquipmentType model)  // cap nhat
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_EquipmentType.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
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
        }}

        public bool Delete(int Id)  // xoa
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_EquipmentType.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
              }
        }
        private bool CheckExists(Q_EquipmentType model)
        { 
            Q_EquipmentType obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_EquipmentType.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
