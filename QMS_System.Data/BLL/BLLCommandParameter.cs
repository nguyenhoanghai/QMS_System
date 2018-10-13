using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLCommandParameter
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLCommandParameter _Instance;  //volatile =>  tranh dung thread
        public static BLLCommandParameter Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCommandParameter();

                return _Instance;
            }
        }
        private BLLCommandParameter() { }
        #endregion
        public List<CommandParamModel> Gets(int commandId)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_CommandParameter.Where(x => !x.IsDeleted && !x.Q_Command.IsDeleted && x.CommandId == commandId).Select(x => new CommandParamModel() { Id = x.Id, CommandId = x.CommandId, Parameter = x.Parameter, Note = x.Note }).ToList();
        }
        }

        public List<ModelSelectItem> GetLookUp(int cmdId)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_CommandParameter.Where(x => !x.IsDeleted && !x.Q_Command.IsDeleted && x.CommandId == cmdId).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Parameter,  Code = x.Note }).ToList();
        }
        }
       
        public int Insert(Q_CommandParameter obj)
        {
              using (db = new QMSSystemEntities()){
            db.Q_CommandParameter.Add(obj);
            db.SaveChanges();
            return obj.Id;
        }
        }

        public bool Update(Q_CommandParameter model)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_CommandParameter.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                obj.CommandId = model.CommandId; 
                obj.Parameter = model.Parameter;
                obj.Note = model.Note; 
                db.SaveChanges();
                return true;
            }
            return false;
        }
        }

        public bool Delete(int Id)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_CommandParameter.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
