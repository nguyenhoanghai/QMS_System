using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLServiceLimit
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLServiceLimit _Instance;  //volatile =>  tranh dung thread
        public static BLLServiceLimit Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLServiceLimit();

                return _Instance;
            }
        }
        private BLLServiceLimit() { }
        #endregion

        public List<ServiceLimitModel> Gets(string connectString,int? userId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (userId.HasValue)
                    return db.Q_ServiceLimit.Where(x => !x.IsDeleted && x.UserId == userId.Value).Select(x => new ServiceLimitModel() { Id = x.Id, ServiceId = x.ServiceId, Quantity = x.Quantity, CurrentDay = x.CurrentDay, CurrentQuantity = x.CurrentQuantity, UserId = x.UserId }).ToList();

                return db.Q_ServiceLimit.Where(x => !x.IsDeleted).Select(x => new ServiceLimitModel() { Id = x.Id, ServiceId = x.ServiceId, Quantity = x.Quantity, CurrentDay = x.CurrentDay, CurrentQuantity = x.CurrentQuantity, UserId = x.UserId }).ToList();
            }
        }

        public bool InsertOrUpdate(string connectString,Q_ServiceLimit model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (CheckExists(model) == null)
                {
                    if (model.Id == 0)
                    {
                        model.CurrentDay = DateTime.Now.ToString("dd/MM/yyyy");
                        db.Q_ServiceLimit.Add(model);
                    }
                    else
                    {
                        var obj = db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (obj != null)
                        {
                            obj.Quantity = model.Quantity;
                            obj.ServiceId = model.ServiceId;
                            db.SaveChanges();
                        }
                        else
                            return false;
                    }
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool UpdateDayInfo(string connectString, int userId, int serviceId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.ServiceId == serviceId && x.UserId == userId);
                if (obj != null)
                {
                    obj.CurrentDay = DateTime.Now.ToString("dd/MM/yyyy");
                    obj.CurrentQuantity = obj.CurrentQuantity + 1;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Delete(string connectString,int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private Q_ServiceLimit CheckExists(Q_ServiceLimit model)
        {
            return db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.UserId == model.UserId && x.ServiceId == model.ServiceId);
        }
    }
}
