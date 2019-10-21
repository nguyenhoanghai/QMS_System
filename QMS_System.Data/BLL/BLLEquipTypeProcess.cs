using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLEquipTypeProcess
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLEquipTypeProcess _Instance;  //volatile =>  tranh dung thread
        public static BLLEquipTypeProcess Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLEquipTypeProcess();

                return _Instance;
            }
        }
        private BLLEquipTypeProcess() { }
        #endregion
        public List<EquipTypeProcessModel> Gets(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_EquipTypeProcess.Where(x => !x.IsDeleted).Select(x => new EquipTypeProcessModel()
            {
                Id = x.Id,
                EquipTypeId = x.EquipTypeId,
                ProcessId = x.ProcessId,
                Step = x.Step,
                Priority = x.Priority,
                Count = x.Count
            }).ToList();
        }}

        //public List<ModelSelectItem> GetLookUp(string connectString)
        //{
        //      using (db = new QMSSystemEntities(connectString)){
        //    return db.Q_EquipTypeProcess.Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
        //}
       
        public int Insert(string connectString,Q_EquipTypeProcess obj)
        {
              using (db = new QMSSystemEntities(connectString)){
            db.Q_EquipTypeProcess.Add(obj);
            db.SaveChanges();
            return obj.Id;
        }}

        public bool Update(string connectString,Q_EquipTypeProcess model)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_EquipTypeProcess.FirstOrDefault(x => x.Id == model.Id);
            if (obj != null)
            {
                obj.EquipTypeId = model.EquipTypeId;
                obj.ProcessId = model.ProcessId; 
                obj.Step = model.Step;
                obj.Priority = model.Priority;
                obj.Count = model.Count;
                db.SaveChanges();
                return true;
            }
            return false;
        }}

        public bool Delete(string connectString,int Id)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_EquipTypeProcess.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
