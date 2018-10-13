using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLServiceShift
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLServiceShift _Instance;  //volatile =>  tranh dung thread
        public static BLLServiceShift Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLServiceShift();

                return _Instance;
            }
        }
        private BLLServiceShift() { }
        #endregion
        public List<ServiceShiftModel> Gets(int serviceId)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_ServiceShift.Where(x => !x.IsDeleted && !x.Q_Shift.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == serviceId).Select(x => new ServiceShiftModel() { Id = x.Id, ServiceId = x.ServiceId, ShiftId = x.ShiftId, Index = x.Index }).ToList();
        }}

        public Q_ServiceShift Get(int serviceId,DateTime date)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_ServiceShift.Where(x=>!x.IsDeleted && !x.Q_Service.IsDeleted && !x.Q_Service.IsDeleted&&x.ServiceId == serviceId).ToList().FirstOrDefault(x => x.Q_Shift.Start.TimeOfDay <= date.TimeOfDay && x.Q_Shift.End.TimeOfDay >= date.TimeOfDay);
        }}

         
        public int Insert(Q_ServiceShift obj)
        {
              using (db = new QMSSystemEntities()){
            if (!CheckExists(obj))
            {
                db.Q_ServiceShift.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }}

        public bool Update(Q_ServiceShift model)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_ServiceShift.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                if (!CheckExists(model))
                {
                    obj.ServiceId = model.ServiceId;
                    obj.ShiftId = model.ShiftId;
                    obj.Index = model.Index;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }}

        public bool Delete(int Id)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_ServiceShift.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
              }
        }
        private bool CheckExists(Q_ServiceShift model)
        { 
            var obj = db.Q_ServiceShift.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.ServiceId == model.ServiceId && x.ShiftId == model.ShiftId);
            return obj != null ? true : false;
        }
    }
}
