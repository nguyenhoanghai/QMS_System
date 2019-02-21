using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLService
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLService _Instance;  //volatile =>  tranh dung thread
        public static BLLService Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLService();

                return _Instance;
            }
        }
        private BLLService() { }
        #endregion
        public List<ServiceModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Service.Where(x => !x.IsDeleted).Select(x => new ServiceModel() { Id = x.Id, Name = x.Name, StartNumber = x.StartNumber, EndNumber = x.EndNumber, TimeProcess = x.TimeProcess, Note = x.Note, IsActived = x.IsActived,Code = x.Code }).ToList();
            }
        }

        public List<ServiceDayModel> GetsForMain()
        {
            using (db = new QMSSystemEntities())
            {
                var sers = db.Q_Service.Where(x => !x.IsDeleted&&x.IsActived).Select(x => new ServiceDayModel() { Id = x.Id, Name = x.Name, StartNumber = x.StartNumber, EndNumber = x.EndNumber, TimeProcess = x.TimeProcess }).ToList();
                var serShifts = db.Q_ServiceShift.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && !x.Q_Shift.IsDeleted).Select(x => new ServiceShiftModel() { Id = x.Id, ServiceId = x.ServiceId, ShiftId = x.ShiftId, Index = x.Index, Start = x.Q_Shift.Start, End = x.Q_Shift.End }).ToList();
                if (sers.Count > 0)
                    foreach (var item in sers)
                        item.Shifts.AddRange(serShifts.Where(x => x.ServiceId == item.Id));
                return sers;
            }
        }

        public Q_Service Get(int serviceId)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Service.Where(x => !x.IsDeleted && x.Id == serviceId).FirstOrDefault();
                return obj;
            }
        }
        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Service.Where(x => !x.IsDeleted&&x.IsActived).AsEnumerable().Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name, Code = x.TimeProcess.TimeOfDay.ToString() }).ToList();
            }
        }

        public int Insert(Q_Service obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_Service.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }
        public bool Update(Q_Service model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Service.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Name = model.Name;
                        obj.StartNumber = model.StartNumber;
                        obj.Note = model.Note;
                        obj.EndNumber = model.EndNumber;
                        obj.TimeProcess = model.TimeProcess;
                        obj.IsActived = model.IsActived;
                        obj.Code = model.Code;
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
                var obj = db.Q_Service.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Service model)
        {
            Q_Service obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Service.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
