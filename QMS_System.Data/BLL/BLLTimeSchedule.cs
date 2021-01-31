using GPRO.Ultilities;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLTimeSchedule
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLTimeSchedule _Instance;
        public static BLLTimeSchedule Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLTimeSchedule();

                return _Instance;
            }
        }
        private BLLTimeSchedule() { }
        #endregion

        public ResponseBase CreateOrUpdate(string connectString, Q_Schedule_Detail model, Q_Customer custModel)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var rs = new ResponseBase();
                    var cust = db.Q_Customer.FirstOrDefault(x => !x.IsDeleted && x.Code.Trim().ToUpper() == custModel.Code.Trim().ToUpper());
                    if (cust == null)
                    {
                        cust = new Q_Customer();
                        Parse.CopyObject(custModel, ref cust);
                        db.Q_Customer.Add(cust);
                        db.SaveChanges();
                    }
                    model.CustomerId = cust.Id;

                    var foundItem = CheckExists(model, db);
                    if (foundItem != null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Insert", Message = "khách hàng này đã có lịch hẹn lúc " + foundItem.ScheduleDate.ToString("dd/MM/yyyy HH:mm") + "." });
                    }
                    else
                    {
                        Q_Schedule parent = db.Q_Schedule.FirstOrDefault(x => x.Month == model.ScheduleDate.Month && x.Year == model.ScheduleDate.Year);
                        if (model.Id == 0)
                        {
                            if (parent == null)
                            {
                                parent = new Q_Schedule();
                                parent.Month = model.ScheduleDate.Month;
                                parent.Year = model.ScheduleDate.Year;
                                parent.Q_Schedule_Detail = new List<Q_Schedule_Detail>();
                                model.Q_Schedule = parent;
                                parent.Q_Schedule_Detail.Add(model);
                                db.Q_Schedule.Add(parent);
                                rs.IsSuccess = true;
                            }
                            else
                            {
                                model.ScheduleId = parent.Id;
                                db.Q_Schedule_Detail.Add(model);
                                rs.IsSuccess = true;
                            }
                        }
                        else
                        {
                            var obj = db.Q_Schedule_Detail.FirstOrDefault(m => m.Id == model.Id);
                            if (obj == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                            }
                            else
                            {
                                obj.CustomerId = model.CustomerId;
                                obj.ScheduleDate = model.ScheduleDate;
                                obj.KhungGioId = model.KhungGioId;
                                obj.ServiceId = model.ServiceId;
                                obj.Note = model.Note;
                                rs.IsSuccess = true;
                            }
                        }
                        if (rs.IsSuccess)
                        {
                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                    }
                    return rs;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private Q_Schedule_Detail CheckExists(Q_Schedule_Detail model, QMSSystemEntities db)
        {
            return db.Q_Schedule_Detail.FirstOrDefault(x => x.Id == model.Id && x.CustomerId == model.CustomerId && x.ServiceId == model.ServiceId);
        }

        public List<TimeScheduleModel> Gets(string connectString, int day, int month, int year, int khungId, int serviceId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Schedule_Detail.Where(x => !x.IsDeleted && x.ScheduleDate.Day == day && x.Q_Schedule.Month == month && x.Q_Schedule.Year == year && x.KhungGioId == khungId && x.ServiceId == serviceId).Select(x => new TimeScheduleModel()
                {
                    Id = x.Id,
                    CustId = x.CustomerId,
                    Time = x.ScheduleDate,
                    Note = x.Note,
                    CustName = x.Q_Customer.Name,
                    CustCode = x.Q_Customer.Code,
                    KhungGio = x.Q_KhungGio.Name,
                    KhungGioId = x.KhungGioId,
                    ServiceId = x.ServiceId,
                    ServiceName = x.Q_Service.Name
                }).ToList();
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Schedule_Detail.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
