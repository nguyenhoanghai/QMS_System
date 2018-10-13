using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLActionParameter
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLActionParameter _Instance;  //volatile =>  tranh dung thread
        public static BLLActionParameter Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLActionParameter();

                return _Instance;
            }
        }
        private BLLActionParameter() { }
        #endregion
        public List<ActionParamModel> Gets(int actionId)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_ActionParameter.Where(x => !x.IsDeleted && !x.Q_Action.IsDeleted && actionId == x.ActionId).Select(x => new ActionParamModel() { Id = x.Id, ActionId = x.ActionId, ParameterCode = x.ParameterCode, Note = x.Note }).ToList();
        }
        }

        public List<ModelSelectItem> GetLookUp( int actionId)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_ActionParameter.Where(x => !x.IsDeleted && !x.Q_Action.IsDeleted && x.ActionId == actionId).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.ParameterCode , Code = x.Note}).ToList();
        }
        }
       
        public int Insert(Q_ActionParameter obj)
        {
              using (db = new QMSSystemEntities()){
            db.Q_ActionParameter.Add(obj);
            db.SaveChanges();
            return obj.Id;
        }
        }

        public bool Update(Q_ActionParameter model)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_ActionParameter.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                obj.ParameterCode = model.ParameterCode;
                obj.ActionId = model.ActionId; 
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
                var obj = db.Q_ActionParameter.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
