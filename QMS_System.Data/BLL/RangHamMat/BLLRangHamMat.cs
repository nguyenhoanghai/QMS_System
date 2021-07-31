using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using QMS_System.ThirdApp.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GPRO.Core.Mvc;

namespace QMS_System.Data.BLL.RangHamMat
{
    public class BLLRangHamMat
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLRangHamMat _Instance;
        public static BLLRangHamMat Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLRangHamMat();

                return _Instance;
            }
        }
        private BLLRangHamMat() { }
        #endregion

        //app counter soft
        public List<ModelSelectItem> getDSDangNhap(string connectString)
        {
            var users = new List<ModelSelectItem>();
            using (var db = new QMSSystemEntities(connectString))
            {
                users = db.Q_Equipment
                    .Where(x => !x.IsDeleted && x.EquipTypeId == (int)eEquipType.Counter && x.StatusId == (int)eStatus.HOTAT)
                    .Select(x => new ModelSelectItem()
                    {
                        Code = x.Q_Counter.Name,
                        Id = x.Code,
                        Record = x.CounterId
                    }).OrderBy(x => x.Id).ToList();

                var logins = db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN).Select(x => new ModelSelectItem() { Id = x.EquipCode, Name = x.Q_User.Name, Record = x.UserId }).ToList();

                for (int i = 0; i < users.Count; i++)
                {
                    var found = logins.FirstOrDefault(x => x.Id == users[i].Id);
                    if (found != null)
                    {
                        //mượn rồi tra lai
                        users[i].Data1 = users[i].Record;
                        users[i].Name = found.Name;
                        users[i].Record = found.Record;
                    }
                }
            }
            return users;
        }

        public ResponseBase changeLogin(string connectString, int userId, int eCode)
        {
            var result = new ResponseBase();
            result.IsSuccess = false;
            using (var db = new QMSSystemEntities(connectString))
            {
                var last = db.Q_DailyRequire_Detail.Where(x => x.UserId == userId && x.EquipCode == eCode && x.StatusId == (int)eStatus.DAGXL);
                if (last != null && last.Count() > 0)
                {
                    foreach (var item in last)
                    {
                        item.StatusId = (int)eStatus.HOTAT;
                        item.EndProcessTime = DateTime.Now; 
                    }
                    db.SaveChanges(); 
                } 

                var newLogin = new Q_Login() { EquipCode = eCode, UserId = userId, Date = DateTime.Now, StatusId = (int)eStatus.LOGIN };

                db.Q_Login.Add(newLogin);
                db.SaveChanges();

                var logins = db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN && (x.UserId == userId || x.EquipCode == eCode) && x.Id != newLogin.Id).ToList();
                if (logins != null && logins.Count() > 0)
                {
                    for (int i = 0; i < logins.Count(); i++)
                    {
                        logins[i].StatusId = (int)eStatus.LOGOUT;
                        logins[i].LogoutTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
                result.IsSuccess = true;
            }
            return result;
        }

        public ScheduleModel findCustomerSchedule(string connectString, string custCode, int month, int year, int day)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                return db.Q_Schedule_Detail
                     .Where(x => x.Q_Customer.Code.Trim().ToUpper() == custCode.Trim().ToUpper() && x.Q_Schedule.Month == month && x.Q_Schedule.Year == year && x.ScheduleDate.Day == day)
                 .Select(x => new ScheduleModel()
                 {
                     Id = x.Id,
                     Code = x.Q_Customer.Code,
                     Name = x.Q_Customer.Name,
                     ScheduleDate = x.ScheduleDate,
                     Phone = x.Q_Customer.Phone,
                     Address = x.Q_Customer.Address,
                     YearOfBirth = x.Q_Customer.YearOfBirth,
                     KhungGio = x.Q_KhungGio.Name,
                     KhungGioId = x.KhungGioId,
                     ServiceId = x.ServiceId,
                     ServiceName = x.Q_Service.Name
                 }).FirstOrDefault();

            }
        }

        public ResponseBase CapSoPhongKhamCoHen(string connectString, int dichvuId, string TenBN, string Address, int DOB, string MaBenhNhan, DateTime printTime, int khungGioId)
        {
            var rs = new ResponseBase();
            Q_DailyRequire rq = null,
                     modelObj = new Q_DailyRequire();
            int _serviceStartNumber = 0;
            TimeSpan? serveTimeAllow = new TimeSpan(0, 0, 0);
            TimeSpan? tgGoiDK = new TimeSpan(0, 0, 0);
            using (var db = new QMSSystemEntities(connectString))
            {
                var dichvu = db.Q_Service.FirstOrDefault(x => x.Id == dichvuId);
                if (dichvu != null)
                {
                    _serviceStartNumber = dichvu.StartNumber;
                    serveTimeAllow = dichvu.TimeProcess.TimeOfDay;
                }

                var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == dichvuId).OrderBy(x => x.Index).FirstOrDefault();
                if (nv == null)
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                }
                else
                {
                    var _found = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.ServiceId == dichvuId && x.Q_DailyRequire.MaBenhNhan == MaBenhNhan);
                    if (_found != null && khungGioId != 0)
                    {
                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                        var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();

                        var tq = (from x in db.Q_Equipment
                                  where
                                 (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                  !x.IsDeleted &&
                                  !x.Q_Counter.IsDeleted
                                  select x.Q_Counter.Name).ToArray();
                        var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                        rs.IsSuccess = true;
                        rs.Data = _found.Q_DailyRequire.TicketNumber - 1;
                        rs.Data_1 = _found.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = _found.Q_DailyRequire.TicketNumber;
                    }
                    else
                    {
                        int stt = 0;
                        var kGioObj = db.Q_KhungGio.FirstOrDefault(x => x.Id == khungGioId);
                        if (kGioObj != null)
                        {
                            if (printTime.TimeOfDay <= kGioObj.EndTime)
                            {
                                // dung gio
                                var lastTicket = db.Q_DailyRequire.Where(x => x.ServiceId == dichvuId && x.STT_PhongKham == kGioObj.Name && x.TicketNumber < 20).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                                if (lastTicket != null)
                                {
                                    stt = lastTicket.TicketNumber++;
                                }
                                else
                                {
                                    stt = (_serviceStartNumber + (kGioObj.Index * 3) + 1);
                                }
                            }
                            else
                            {
                                // di trễ
                                var lastTicket = db.Q_DailyRequire.Where(x => x.ServiceId == dichvuId && x.TicketNumber >= (_serviceStartNumber + 20)).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                                if (lastTicket != null)
                                {
                                    stt = lastTicket.TicketNumber++;
                                }
                                else
                                {
                                    stt = (_serviceStartNumber + 20);
                                }
                            }
                        }
                        else
                        {
                            //ko hẹn
                            var lastTicket = db.Q_DailyRequire.Where(x => x.ServiceId == dichvuId && x.TicketNumber >= (_serviceStartNumber + 20)).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                            if (lastTicket != null)
                            {
                                stt = lastTicket.TicketNumber++;
                            }
                            else
                            {
                                stt = (_serviceStartNumber + 20);
                            }
                        }
                        modelObj.TicketNumber = stt;
                        modelObj.ServiceId = dichvuId;
                        modelObj.PrintTime = printTime;
                        modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                        modelObj.CustomerName = TenBN;
                        modelObj.CustomerDOB = DOB;
                        modelObj.CustomerAddress = Address;
                        modelObj.MaBenhNhan = MaBenhNhan;
                        modelObj.STT_PhongKham = (kGioObj != null ? kGioObj.Name : "--");
                        modelObj.Type = (int)eDailyRequireType.KhamBenh;
                        modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                        var detail = new Q_DailyRequire_Detail();
                        detail.Q_DailyRequire = modelObj;
                        detail.MajorId = nv.MajorId;
                        detail.StatusId = (int)eStatus.CHOXL;

                        if (dichvu != null && dichvu.AutoEnd)
                        {
                            detail.StatusId = (int)eStatus.HOTAT;
                            var now = DateTime.Now;
                            var timeend = dichvu.TimeAutoEnd != null ? dichvu.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                            detail.ProcessTime = now;
                            detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                        }
                        modelObj.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(modelObj);

                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                        var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();
                        var tq = (from x in db.Q_Equipment
                                  where
                                 (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                  !x.IsDeleted &&
                                  !x.Q_Counter.IsDeleted
                                  select x.Q_Counter.Name).ToArray();
                        var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Data = (stt - 1);
                        rs.Data_1 = detail.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = stt;
                    }
                }
            }
            return rs;
        }

        public ResponseBase CapSoPhongKhamKhongHen(string connectString, int dichvuId, DateTime printTime)
        {
            var rs = new ResponseBase();
            var modelObj = new Q_DailyRequire();
            int _serviceStartNumber = 0;
            TimeSpan? serveTimeAllow = new TimeSpan(0, 0, 0),
              tgGoiDK = new TimeSpan(0, 0, 0);
            using (var db = new QMSSystemEntities(connectString))
            {
                var dichvu = db.Q_Service.FirstOrDefault(x => x.Id == dichvuId);
                if (dichvu != null)
                {
                    _serviceStartNumber = dichvu.StartNumber;
                    serveTimeAllow = dichvu.TimeProcess.TimeOfDay;
                }

                var nv = db.Q_ServiceStep
                    .Where(x => !x.IsDeleted &&
                    !x.Q_Service.IsDeleted &&
                    x.ServiceId == dichvuId)
                    .OrderBy(x => x.Index)
                    .FirstOrDefault();
                if (nv == null)
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                }
                else
                {
                    int stt = 0;
                    var lastTicket = db.Q_DailyRequire
                        .Where(x => x.ServiceId == dichvuId &&
                        x.Type == (int)eDailyRequireType.KhamBenh)
                        .OrderByDescending(x => x.TicketNumber)
                        .FirstOrDefault();

                    if (lastTicket != null)
                    {
                        stt = lastTicket.TicketNumber + 1;
                    }
                    else
                    {
                        stt = _serviceStartNumber;
                    }

                    modelObj.TicketNumber = stt;
                    modelObj.ServiceId = dichvuId;
                    modelObj.BusinessId = null;
                    modelObj.PrintTime = printTime;
                    modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                    modelObj.Type = (int)eDailyRequireType.KhamBenh;
                    modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                    var detail = new Q_DailyRequire_Detail();
                    detail.Q_DailyRequire = modelObj;
                    detail.MajorId = nv.MajorId;
                    detail.StatusId = (int)eStatus.CHOXL;

                    if (dichvu != null && dichvu.AutoEnd)
                    {
                        detail.StatusId = (int)eStatus.HOTAT;
                        var now = DateTime.Now;
                        var timeend = dichvu.TimeAutoEnd != null ? dichvu.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                        detail.ProcessTime = now;
                        detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                    }
                    modelObj.Q_DailyRequire_Detail.Add(detail);
                    db.Q_DailyRequire.Add(modelObj);

                    var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                    var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                    var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();
                    var tq = (from x in db.Q_Equipment
                              where
                             (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                              !x.IsDeleted &&
                              !x.Q_Counter.IsDeleted
                              select x.Q_Counter.Name).ToArray();
                    var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                    db.SaveChanges();
                    rs.IsSuccess = true;
                    rs.Data = (stt - 1);
                    rs.Data_1 = detail.MajorId;
                    rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                    rs.Data_2 = tqs;
                    rs.Data_3 = stt;
                }
            }
            return rs;
        }

        public List<ModelSelectItem> GetKhungGioLookUp(string connectString)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    return db.Q_KhungGio.Where(x => !x.IsDeleted).OrderBy(x => x.StartTime).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public HomeModel GetForHome(string connectString, int userId, int equipCode, DateTime date, int useWithThridPattern)
        {
            var model = new HomeModel();
            using (db = new QMSSystemEntities(connectString))
            {
                var user = db.Q_Login.FirstOrDefault(x => x.StatusId == (int)eStatus.LOGIN && x.UserId == userId && x.EquipCode == equipCode);
                if (user != null)
                {
                    var rq = db.Q_DailyRequire.OrderBy(x => x.TicketNumber).ToList();
                    var dailyDetails = db.Q_DailyRequire_Detail;

                    var majorIds = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).Select(x => x.MajorId).ToList();
                    model.TotalDone = dailyDetails.Count(x => x.StatusId == (int)eStatus.HOTAT && x.UserId == userId);
                    model.TotalWating = dailyDetails.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId));
                    var current = dailyDetails.Where(x => x.StatusId == (int)eStatus.DAGXL && x.UserId == userId).OrderBy(x => x.ProcessTime).FirstOrDefault();
                    if (current != null)
                    {
                        if (useWithThridPattern == 0)
                            model.CurrentTicket = current.Q_DailyRequire.TicketNumber;
                        else
                            model.CurrentTicket = int.Parse(current.Q_DailyRequire.STT_PhongKham);
                        model.CommingTime = current.ProcessTime;
                    }
                    model.CountWaitAtCounter = 0;
                    if (rq.Count() > 0)
                    {
                        int diemDoiTaiQuay = 0;
                        foreach (var item in rq)
                        {
                            var dt = dailyDetails.Where(x => x.DailyRequireId == item.Id).ToList();
                            if (dt != null && dt.Count() == 1)
                            {
                                var first = dt.FirstOrDefault();
                                if (first.StatusId == (int)eStatus.CHOXL && majorIds.Contains(first.MajorId) && first.Q_DailyRequire.Q_Service.ServiceType == (int)eServiceType.CLS)
                                {
                                    model.CounterWaitingTickets += ((useWithThridPattern == 0 ? item.TicketNumber.ToString() : (item.STT_PhongKham == null ? " " : item.STT_PhongKham)) + " ");
                                    diemDoiTaiQuay++;
                                }
                                else if (first.StatusId == (int)eStatus.CHOXL && majorIds.Contains(first.MajorId) && first.Q_DailyRequire.Q_Service.ServiceType == (int)eServiceType.PhongKham)
                                    model.AllWaitingTickets += item.TicketNumber + " ";
                            }
                            else if (dt != null && dt.Count() > 1)
                                if (dt.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId)) > 0)
                                {
                                    model.CounterWaitingTickets += ((useWithThridPattern == 0 ? item.TicketNumber.ToString() : (item.STT_PhongKham == null ? " " : item.STT_PhongKham)) + " ");
                                    diemDoiTaiQuay++;
                                }
                        }
                        model.CountWaitAtCounter = diemDoiTaiQuay;
                    }

                    model.Counter = "";
                    var found = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                    if (found != null && found.Q_Counter != null)
                        model.Counter = found.Q_Counter.Name;
                }
                else
                    model.IsLogout = true;
                return model;
            }
        }

        public int Next(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern, int serviceType)
        {
            int ticket = 0;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var oldTickets = db.Q_DailyRequire_Detail.Where(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    if (oldTickets != null && oldTickets.Count() > 0)
                    {
                        foreach (var item in oldTickets)
                        {
                            item.StatusId = (int)eStatus.HOTAT;
                            item.EndProcessTime = DateTime.Now;
                        }
                        db.SaveChanges();
                    }

                    var majorIds = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).OrderBy(x => x.Index).Select(x => x.MajorId).ToList();
                    if (majorIds.Count > 0)
                    {
                        for (int i = 0; i < majorIds.Count; i++)
                        {
                            int a = majorIds[i];
                            var newTicket = db.Q_DailyRequire_Detail.Where(x => x.MajorId == a && x.StatusId == (int)eStatus.CHOXL && x.Q_DailyRequire.Q_Service.ServiceType == serviceType).OrderBy(x => x.Q_DailyRequire.TicketNumber).FirstOrDefault();
                            if (newTicket != null)
                            {
                                newTicket.UserId = userId;
                                newTicket.EquipCode = equipCode;
                                newTicket.ProcessTime = DateTime.Now;
                                newTicket.StatusId = (int)eStatus.DAGXL;

                                if (useWithThirdPattern == 0)
                                    ticket = newTicket.Q_DailyRequire.TicketNumber;
                                else
                                {
                                    try
                                    {
                                        ticket = int.Parse(newTicket.Q_DailyRequire.STT_PhongKham);
                                    }
                                    catch (Exception)
                                    {
                                        ticket = newTicket.Q_DailyRequire.TicketNumber;
                                    }
                                }

                                var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                                if (equip != null)
                                    db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + ticket + ",CurrentNumber=" + ticket + ", isrunning=0 where Id=" + equip.CounterId);

                                db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                                db.SaveChanges();
                                break;
                            }
                        }
                    }
                    return ticket;
                }
            }
            catch (Exception)
            { }
            return ticket;
        }

        public int CurrentTicket(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern, int serviceType)
        {
            int ticket = 0;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL && x.Q_DailyRequire.Q_Service.ServiceType == serviceType);
                    if (last != null)
                    {
                        if (useWithThirdPattern == 0)
                            ticket = last.Q_DailyRequire.TicketNumber;
                        else
                        {
                            try
                            {
                                ticket = int.Parse(last.Q_DailyRequire.STT_PhongKham);
                            }
                            catch (Exception)
                            {
                                ticket = last.Q_DailyRequire.TicketNumber;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return ticket;
        }

        public ResponseBaseModel CallAny(string connectString, int userId, int equipCode, int ticket, DateTime date, int serviceType)
        {
            ResponseBaseModel res = new ResponseBaseModel();
            bool hasChange = false;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var oldTickets = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.TicketNumber != ticket && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    if (oldTickets != null && oldTickets.Count() > 0)
                    {
                        foreach (var item in oldTickets)
                        {
                            item.StatusId = (int)eStatus.HOTAT;
                            item.EndProcessTime = DateTime.Now;
                        }
                        hasChange = true;
                    }
                    var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                    var check = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.TicketNumber == ticket && (x.StatusId == (int)eStatus.CHOXL || x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.QUALUOT) && x.Q_DailyRequire.Q_Service.ServiceType == serviceType);
                    if (check != null)
                    {
                        if (check.StatusId != (int)eStatus.DAGXL)
                        {
                            check.UserId = userId;
                            check.EquipCode = equipCode;
                            check.StatusId = (int)eStatus.DAGXL;
                            check.ProcessTime = DateTime.Now;
                        }
                        hasChange = true;
                        res.Data_3 = new TicketInfo()
                        {
                            RequireDetailId = check.Id,
                            RequireId = check.DailyRequireId,
                            StartTime = check.ProcessTime.Value.TimeOfDay,
                            TimeServeAllow = check.Q_DailyRequire.ServeTimeAllow,
                            TicketNumber = check.Q_DailyRequire.TicketNumber,
                            CounterId = equip.CounterId,
                            PrintTime = check.Q_DailyRequire.PrintTime,
                            EquipCode = equipCode
                        };
                        db.Database.ExecuteSqlCommand("update Q_Counter set LastCall =" + check.Q_DailyRequire.TicketNumber + ", CurrentNumber=" + check.Q_DailyRequire.TicketNumber + " where Id =" + equip.CounterId);
                    }
                    else
                    {
                        var mj = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && x.UserId == userId).OrderBy(x => x.Index).FirstOrDefault();
                        if (mj != null)
                        {
                            var rq = db.Q_DailyRequire.FirstOrDefault(x => x.TicketNumber == ticket && x.Q_Service.ServiceType == serviceType);
                            if (rq != null)
                            {
                                var newobj = new Q_DailyRequire_Detail();
                                newobj.DailyRequireId = rq.Id;
                                newobj.MajorId = mj.MajorId;
                                newobj.UserId = userId;
                                newobj.EquipCode = equipCode;
                                newobj.StatusId = (int)eStatus.DAGXL;
                                newobj.ProcessTime = DateTime.Now;
                                db.Q_DailyRequire_Detail.Add(newobj);
                                hasChange = true;

                                res.Data_3 = new TicketInfo()
                                {
                                    RequireDetailId = newobj.Id,
                                    RequireId = newobj.DailyRequireId,
                                    StartTime = newobj.ProcessTime.Value.TimeOfDay,
                                    TimeServeAllow = rq.ServeTimeAllow,
                                    TicketNumber = rq.TicketNumber,
                                    CounterId = equip.CounterId,
                                    PrintTime = rq.PrintTime,
                                    EquipCode = equipCode
                                };

                                db.Database.ExecuteSqlCommand("update Q_Counter set LastCall =" + rq.TicketNumber + ", CurrentNumber=" + rq.TicketNumber + " where Id =" + equip.CounterId);
                            }
                            else
                            {
                                res.IsSuccess = false;
                                res.sms = "Không tìm thấy số phiếu " + ticket;
                                res.Title = "Lỗi";
                            }
                        }
                        else
                        {
                            res.IsSuccess = false;
                            res.sms = "Nhân viên chưa được phân nghiệp vụ";
                            res.Title = "Lỗi";
                        }
                    }
                    if (hasChange)
                    {
                        db.SaveChanges();
                        res.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.sms = "Lỗi thực thi dữ liệu";
                res.Title = "Lỗi";
            }
            return res;
        }

        //web
        public ViewModel GetDayInfo(string connectString, int[] counterIds, int[] services, int serviceType)
        {
            using (var db_ = new QMSSystemEntities(connectString))
            {
                var returnModel = new ViewModel();
                try
                {
                    returnModel.Services.AddRange(db_.Q_Service.Where(x => !x.IsDeleted && x.IsActived && services.Contains(x.Id)).Select(x => new SubModel() { ServiceId = x.Id, ServiceName = x.Name, Ticket = 0 }));
                    if (returnModel.Services.Count > 0)
                    {
                        var callObjs = db_.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL && x.Q_DailyRequire.Q_Service.ServiceType == serviceType).Select(x => new ServiceModel() { Id = x.Q_DailyRequire.ServiceId, TimeProcess = x.EndProcessTime ?? DateTime.Now, StartNumber = x.Q_DailyRequire.TicketNumber }).ToList();
                        if (callObjs.Count > 0)
                        {
                            for (int i = 0; i < returnModel.Services.Count; i++)
                            {
                                var found = callObjs.Where(x => x.Id == returnModel.Services[i].ServiceId).OrderByDescending(x => x.TimeProcess).FirstOrDefault();
                                if (found != null)
                                    returnModel.Services[i].Ticket = found.StartNumber;
                            }
                        }
                    }

                    var counters = db_.Q_Equipment.Where(x => !x.IsDeleted && !x.Q_Counter.IsDeleted && counterIds.Contains(x.CounterId) && x.EquipTypeId == (int)eEquipType.Counter)
                        .Select(x => new ViewDetailModel()
                        {
                            STT = (!x.Q_Counter.IsRunning ? 1 : 0),
                            CarNumber = "0",
                            TableId = x.Q_Counter.Id,
                            TableName = x.Q_Counter.Name,
                            TableCode = x.Q_Counter.ShortName,
                            Start = null,
                            TicketNumber = x.Q_Counter.CurrentNumber,
                            TimeProcess = "0",
                            StartStr = (!x.Q_Counter.IsRunning ? ("Mời bệnh nhân " + x.Q_Counter.CurrentNumber + " đến phòng : " + x.Q_Counter.Name) : ""),
                            EquipCode = x.Code,
                        }).ToList();

                    var dangXL = db_.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL && x.Q_DailyRequire.Q_Service.ServiceType == serviceType).ToList();
                    var logins = db_.Q_Login
                        .Where(x => x.StatusId == (int)eStatus.LOGIN)
                        .Select(x => new ModelSelectItem()
                        {
                            Id = x.UserId,
                            Name = x.Q_User.Name,
                            Record = x.EquipCode
                        }).ToList();

                    var uMajors = db_.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted).OrderBy(x => x.Index).Select(x => new { UserId = x.UserId, MajorId = x.MajorId, Index = x.Index }).ToList();
                    foreach (var item in counters)
                    {
                        item.Tong = 0;
                        var found = logins.FirstOrDefault(x => x.Record == item.EquipCode);
                        if (found != null)
                        {
                            item.UserName = found.Name;
                            var fMajors = uMajors.Where(x => x.UserId == found.Id).Select(x => x.MajorId).ToList();
                            item.Tong = db_.Q_DailyRequire_Detail.Where(x => fMajors.Contains(x.MajorId) && x.Q_DailyRequire.Q_Service.ServiceType == serviceType).Select(x => x.Id).ToList().Count;
                        }

                        var current = dangXL.Where(x => x.StatusId == (int)eStatus.DAGXL && x.EquipCode == item.EquipCode).OrderByDescending(x => x.ProcessTime).FirstOrDefault();
                        if (current != null)
                        {
                            item.TicketNumber = current.Q_DailyRequire.TicketNumber;
                            item.TenBN = current.Q_DailyRequire.CustomerName;
                            item.NamSinh = (current.Q_DailyRequire.CustomerDOB.HasValue ? current.Q_DailyRequire.CustomerDOB.Value.ToString() : "---");
                            item.KhungGio = current.Q_DailyRequire.STT_PhongKham;
                            item.GioLaySo = current.Q_DailyRequire.PrintTime.ToString("HH:mm");
                            item.Note = "";
                        }
                        else
                        {
                            item.TicketNumber = 0;
                            item.TenBN = "---";
                            item.NamSinh = "---";
                            item.KhungGio = "---";
                            item.GioLaySo = "---";
                            item.Note = "---";
                        }
                        returnModel.Details.Add(item);
                    }
                    //returnModel.Sounds = BLLTVReadSound.Instance.Gets(connectString, counterIds, userId);
                    if (returnModel.Details.Count > 0)
                    {
                        returnModel.Details[0].RuningText = returnModel.Details.Where(x => x.STT == 1).Select(x => x.StartStr).ToArray();
                        db_.Database.ExecuteSqlCommand("update q_counter set isrunning =1");
                        db_.SaveChanges();
                    }
                }
                catch (Exception ex)
                { }
                return returnModel;
            }
        }


    }

    public class ViewRHM
    {
        public string DangGoi { get; set; }
    }
}
