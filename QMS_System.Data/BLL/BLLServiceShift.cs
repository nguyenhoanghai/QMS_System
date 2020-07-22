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
        public List<ServiceShiftModel> Gets(string connectString,int serviceId)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_ServiceShift.Where(x => !x.IsDeleted && !x.Q_Shift.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == serviceId).Select(x => new ServiceShiftModel() { Id = x.Id, ServiceId = x.ServiceId, ShiftId = x.ShiftId, Index = x.Index }).ToList();
        }}

        public Q_ServiceShift Get(string connectString,int serviceId,DateTime date)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_ServiceShift.Where(x=>!x.IsDeleted && !x.Q_Service.IsDeleted && !x.Q_Service.IsDeleted&&x.ServiceId == serviceId).ToList().FirstOrDefault(x => x.Q_Shift.Start.TimeOfDay <= date.TimeOfDay && x.Q_Shift.End.TimeOfDay >= date.TimeOfDay);
        }}

         
        public int Insert(string connectString,Q_ServiceShift obj)
        {
              using (db = new QMSSystemEntities(connectString)){
            if (!CheckExists(obj))
            {
                db.Q_ServiceShift.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }}

        public bool Update(string connectString,Q_ServiceShift model)
        {
              using (db = new QMSSystemEntities(connectString)){
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

        public bool Delete(string connectString, int Id)
        {
              using (db = new QMSSystemEntities(connectString)){
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

        public bool CheckTime(string connectString, int serviceId,DateTime now)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var founds= (from x in db.Q_ServiceShift
                            where !x.IsDeleted && !x.Q_Service.IsDeleted && !x.Q_Shift.IsDeleted && x.ServiceId == serviceId   
                            select new { start = x.Q_Shift.Start, end = x.Q_Shift.End}).ToList();
                if(founds.Count > 0)
                {
                    founds = founds.Where(x=> now.TimeOfDay >= x.start.TimeOfDay && now.TimeOfDay <= x.end.TimeOfDay).ToList();
                }
                return (founds == null || founds.Count == 0);
            }
        }
    }
}
