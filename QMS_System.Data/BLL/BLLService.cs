using QMS_System.Data.Model;
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
        public List<ServiceModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Service.Where(x => !x.IsDeleted).Select(x => new ServiceModel() { Id = x.Id, Name = x.Name, StartNumber = x.StartNumber, EndNumber = x.EndNumber, TimeProcess = x.TimeProcess, Note = x.Note, IsActived = x.IsActived, Code = x.Code }).ToList();
            }
        }

        public List<ServiceModel> Gets_BenhVien(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Service.Where(x => !x.IsDeleted && x.showBenhVien).Select(x => new ServiceModel() { Id = x.Id, Name = x.Name, StartNumber = x.StartNumber, EndNumber = x.EndNumber, TimeProcess = x.TimeProcess, Note = x.Note, IsActived = x.IsActived, Code = x.Code }).ToList();
            }
        }

        public List<ServiceDayModel> GetsForMain(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var sers = db.Q_Service.Where(x => !x.IsDeleted && x.IsActived).Select(x => new ServiceDayModel() { Id = x.Id, Name = x.Name, StartNumber = x.StartNumber, EndNumber = x.EndNumber, TimeProcess = x.TimeProcess }).ToList();
                var serShifts = db.Q_ServiceShift.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && !x.Q_Shift.IsDeleted).Select(x => new ServiceShiftModel() { Id = x.Id, ServiceId = x.ServiceId, ShiftId = x.ShiftId, Index = x.Index, Start = x.Q_Shift.Start, End = x.Q_Shift.End }).ToList();
                if (sers.Count > 0)
                    foreach (var item in sers)
                        item.Shifts.AddRange(serShifts.Where(x => x.ServiceId == item.Id));
                return sers;
            }
        }

        public Q_Service Get(string connectString, int serviceId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Service.Where(x => !x.IsDeleted && x.Id == serviceId).FirstOrDefault();
                return obj;
            }
        }
        public List<ModelSelectItem> GetLookUp(string connectString, bool countWaiting)
        {
            using (db = new QMSSystemEntities(connectString))
            {

                var services = db.Q_Service.Where(x => !x.IsDeleted && x.IsActived).AsEnumerable().Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name, Code = x.TimeProcess.TimeOfDay.ToString(), Data = 0 }).ToList();
                if (countWaiting)
                {
                    var details = (from x in db.Q_DailyRequire_Detail where !x.UserId.HasValue select x).ToList();
                    foreach (var item in services)
                    {
                        var found = details.Where(x => x.Q_DailyRequire.ServiceId == item.Id);
                        if (found != null && found.Count() > 0)
                            item.Data = found.Count();
                    }
                }
                return services;

            }
        }

        public int Insert(string connectString, Q_Service obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_Service.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }
        public bool Update(string connectString, Q_Service model)
        {
            using (db = new QMSSystemEntities(connectString))
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
        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
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
