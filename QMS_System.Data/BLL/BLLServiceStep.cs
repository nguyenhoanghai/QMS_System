using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLServiceStep
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLServiceStep _Instance;  //volatile =>  tranh dung thread
        public static BLLServiceStep Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLServiceStep();

                return _Instance;
            }
        }
        private BLLServiceStep() { }
        #endregion
        public List<ServiceStepModel> Gets(int serviceId)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == serviceId).Select(x => new ServiceStepModel() { Id = x.Id, MajorId = x.MajorId, ServiceId = x.ServiceId, Index = x.Index }).OrderBy(x => x.Index).ToList();
        }
        }

        //public List<ModelSelectItem> GetLookUp()
        //{
        //      using (db = new QMSSystemEntities()){
        //    return db.Q_ServiceStep.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
        //}

        public int Insert(Q_ServiceStep obj)
        {
              using (db = new QMSSystemEntities()){
            if (!CheckExists(obj))
            {
                db.Q_ServiceStep.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }
        }
        public bool Update(Q_ServiceStep model)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_ServiceStep.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                if (!CheckExists(model))
                {
                    obj.ServiceId = model.ServiceId;
                    obj.MajorId = model.MajorId;
                    obj.Index = model.Index;
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
                var obj = db.Q_ServiceStep.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_ServiceStep model)
        {
            var obj = db.Q_ServiceStep.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.ServiceId == model.ServiceId && x.MajorId == model.MajorId);
            return obj != null ? true : false;
        }
    }
}
