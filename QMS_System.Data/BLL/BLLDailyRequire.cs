using GPRO.Ultilities;
using Newtonsoft.Json;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLDailyRequire
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLDailyRequire _Instance;  //volatile =>  tranh dung thread
        public static BLLDailyRequire Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLDailyRequire();

                return _Instance;
            }
        }
        private BLLDailyRequire() { }
        #endregion

        public Q_DailyRequire GetTicket(string connectString, int ticket)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_DailyRequire.FirstOrDefault(x => x.TicketNumber == ticket);
            }
        }

        public int UpdateTicketInfo(string connectString, int ticket, TimeSpan serveTimeAllow, string customerName, int customerDOB, string Address)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_DailyRequire.FirstOrDefault(x => x.TicketNumber == ticket);
                if (obj != null)
                {
                    obj.ServeTimeAllow = serveTimeAllow;
                    obj.CustomerName = customerName;
                    obj.CustomerDOB = customerDOB;
                    obj.CustomerAddress = Address;
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return obj.Id;
                }
            }
            return 0;
        }

        public int CurentTicket(string connectString, int equipcode)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipcode);
                if (equip != null)
                    return db.Q_Counter.FirstOrDefault(x => x.Id == equip.CounterId).LastCall;
                else
                    return 0;
            }
        }

        public int GetTicket(string connectString, int majorId, int userId, int equipCode)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_DailyRequire_Detail.Where(x => x.MajorId == majorId).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
                if (obj != null)
                {
                    obj.UserId = userId;
                    obj.EquipCode = equipCode;
                    db.SaveChanges();
                    return obj.Q_DailyRequire.TicketNumber;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Counter
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int DoneTicket(string connectString, int userId, int equipCode, DateTime date)
        {
            int ticket = 0;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.Where(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    if (last != null && last.Count() > 0)
                    {
                        foreach (var item in last)
                        {
                            item.StatusId = (int)eStatus.HOTAT;
                            item.EndProcessTime = DateTime.Now;
                            ticket = item.Q_DailyRequire.TicketNumber;
                        }
                        db.SaveChanges();
                        return ticket;
                    }
                }
            }
            catch (Exception ex)
            { }
            return ticket;
        }

        /// <summary>
        /// Counter soft
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int Next(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern)
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
                            var newTicket = db.Q_DailyRequire_Detail.Where(x => x.MajorId == a && x.StatusId == (int)eStatus.CHOXL && !x.Q_DailyRequire.Q_Service.isKetLuan).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
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

        public int Next_KetLuan(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern)
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
                            var newTicket = db.Q_DailyRequire_Detail.Where(x => x.MajorId == a && x.StatusId == (int)eStatus.CHOXL && x.Q_DailyRequire.Q_Service.isKetLuan).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
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
                                    catch (Exception ex)
                                    {
                                        ticket = newTicket.Q_DailyRequire.TicketNumber;
                                    }
                                }
                                var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                                if (equip != null)
                                    db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCallKetLuan=" + ticket + ",CurrentNumber=" + ticket + "  , isrunning=0 where Id=" + equip.CounterId);

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

        public int CurrentTicket(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern)
        {
            int ticket = 0;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
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

        public ResponseBaseModel GetCurrentTicket(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern)
        {
            ResponseBaseModel res = new ResponseBaseModel();
            res.IsSuccess = false;
            int ticket = 0;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
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

                        var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                        res.IsSuccess = true;
                        res.Data = ticket;
                        res.Data_3 = new TicketInfo()
                        {
                            RequireDetailId = last.Id,
                            RequireId = last.DailyRequireId,
                            StartTime = last.ProcessTime.Value.TimeOfDay,
                            TimeServeAllow = last.Q_DailyRequire.ServeTimeAllow,
                            TicketNumber = ticket,
                            QMSNumber = last.Q_DailyRequire.TicketNumber,
                            ThirdNumber = last.Q_DailyRequire.STT_PhongKham,
                            CounterId = (equip != null ? equip.CounterId : 0),
                            PrintTime = last.Q_DailyRequire.PrintTime,
                            CountDown = getCountDownTime(last)
                        };
                    }
                }
            }
            catch (Exception)
            { }
            return res;
        }

        public TicketInfo GetCurrentTicketInfo(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL).ToList();
                    if (last.Count > 0)
                    {
                        int ticket = 0;
                        var first = last.FirstOrDefault();
                        if (useWithThirdPattern == 0)
                            ticket = first.Q_DailyRequire.TicketNumber;
                        else
                        {
                            try
                            {

                                ticket = int.Parse(first.Q_DailyRequire.STT_PhongKham);
                            }
                            catch (Exception)
                            {
                                ticket = first.Q_DailyRequire.TicketNumber;
                            }
                        }


                        return new TicketInfo()
                        {
                            RequireDetailId = first.Id,
                            RequireId = first.DailyRequireId,
                            StartTime = first.ProcessTime.Value.TimeOfDay,
                            TimeServeAllow = first.Q_DailyRequire.ServeTimeAllow,
                            TicketNumber = ticket
                        };
                    }
                }
            }
            catch (Exception)
            { }
            return null;
        }

        public bool StoreTicket(string connectString, int ticket, int userId, int equipCode, DateTime date)
        {

            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    Q_DailyRequire parent;
                    Q_DailyRequire_Detail child;
                    if (ticket != 0)
                    {
                        parent = db.Q_DailyRequire.FirstOrDefault(x => x.PrintTime.Year == date.Year && x.PrintTime.Month == date.Month && x.PrintTime.Day == date.Day && x.TicketNumber == ticket);
                        child = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.DailyRequireId == parent.Id && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    }
                    else
                    {
                        child = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                        parent = db.Q_DailyRequire.FirstOrDefault(x => x.Id == child.DailyRequireId);
                    }
                    if (parent != null && child != null)
                    {
                        parent.PrintTime = DateTime.Now;
                        child.UserId = null;
                        child.EquipCode = null;
                        child.StatusId = (int)eStatus.CHOXL;
                        child.ProcessTime = null;
                        child.EndProcessTime = null;
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception)
            { }
            return false;
        }

        /// <summary>
        /// counter
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int DeleteTicket(string connectString, int ticket, DateTime date)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var objs = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.TicketNumber == ticket);
                    if (objs != null && objs.Count() > 0)
                    {
                        int parentId = 0;
                        foreach (var item in objs)
                        {
                            parentId = item.DailyRequireId;
                            db.Q_DailyRequire_Detail.Remove(item);
                        }
                        if (parentId != 0)
                        {
                            var parentObj = db.Q_DailyRequire.FirstOrDefault(x => x.Id == parentId);
                            if (parentObj != null)
                            {
                                ticket = parentObj.TicketNumber;
                                db.Q_DailyRequire.Remove(parentObj);
                            }
                        }
                        db.SaveChanges();
                        return ticket;
                    }
                }
            }
            catch (Exception)
            { }
            return 0;
        }

        public bool DeleteAllTicketInDay(string connectString, DateTime date)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    string query = "DELETE FROM [dbo].[q_userevaluate]   ";
                    query += "DELETE FROM [dbo].[Q_DailyRequire_Detail]   ";
                    query += "DELETE FROM [dbo].[Q_DailyRequire]   ";
                    query += "DELETE FROM [dbo].[Q_RequestTicket]   ";
                    query += "update [dbo].[q_counter] set [LastCall]='0', [IsRunning]=1 , CurrentNumber=0, LastCallKetLuan= 0 ";
                    query += "update [dbo].[Q_ServiceLimit] set [CurrentDay]='" + date.ToString("dd/MM/yyyy") + "', [CurrentQuantity]=0 ";
                    db.Database.ExecuteSqlCommand(query);

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Counter
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="ticket"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseBaseModel CallAny(string connectString, int majorId, int userId, int equipCode, int ticket, DateTime date, int UseWithThirdPattern)
        {
            ResponseBaseModel res = new ResponseBaseModel();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    if (last == null)
                    {
                        string config = BLLConfig.Instance.GetConfigByCode(connectString, eConfigCode.CheckServiceLimit);
                        List<int> serviceOver = new List<int>();
                        bool hasCheckLimit = false;
                        if (!string.IsNullOrEmpty(config) && config == "1")
                        {
                            hasCheckLimit = true;
                        }
                        Start:
                        var check = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.TicketNumber == ticket && x.StatusId == (int)eStatus.CHOXL && !serviceOver.Contains(x.Q_DailyRequire.ServiceId));
                        if (check != null)
                        {
                            if (hasCheckLimit)
                            {
                                var userService = db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.UserId == userId && x.ServiceId == check.Q_DailyRequire.ServiceId);
                                if (userService != null && userService.Quantity == userService.CurrentQuantity)
                                {
                                    //ktra tiep xem co user nao còn gọi dc hay ko
                                    var otherUsers = db.Q_ServiceLimit.Where(x => !x.IsDeleted && x.Quantity != x.CurrentQuantity && x.ServiceId == check.Q_DailyRequire.ServiceId).ToList();
                                    if (otherUsers.Count > 0)
                                    {
                                        // neu co nguoi con goi dc thi ko dc lay phieu voi dich vu nay ma phai goi dich vu khac
                                        serviceOver.Add(check.Q_DailyRequire.ServiceId);
                                        goto Start;
                                    }
                                }
                            }

                            check.UserId = userId;
                            check.EquipCode = equipCode;
                            check.StatusId = (int)eStatus.DAGXL;
                            check.ProcessTime = date;
                            int num = 0;
                            if (UseWithThirdPattern == 0)
                                num = check.Q_DailyRequire.TicketNumber;
                            else
                            {
                                try
                                {
                                    num = int.Parse(check.Q_DailyRequire.STT_PhongKham);
                                }
                                catch (Exception)
                                {
                                    num = check.Q_DailyRequire.TicketNumber;
                                }
                            }

                            var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                            if (equip != null)
                                db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + num + ",CurrentNumber=" + num + "  where Id=" + equip.CounterId);

                            db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                            db.SaveChanges();
                            res.Data = check.Q_DailyRequire.ServeTimeAllow;
                            res.Data_1 = check.ProcessTime.Value.TimeOfDay;
                            res.Data_3 = new TicketInfo()
                            {
                                RequireDetailId = check.Id,
                                RequireId = check.DailyRequireId,
                                StartTime = check.ProcessTime.Value.TimeOfDay,
                                TimeServeAllow = check.Q_DailyRequire.ServeTimeAllow,
                                TicketNumber = num,
                                QMSNumber = check.Q_DailyRequire.TicketNumber,
                                ThirdNumber = check.Q_DailyRequire.STT_PhongKham,
                                CounterId = equip.CounterId,
                                PrintTime = check.Q_DailyRequire.PrintTime,
                                CountDown = getCountDownTime(check)
                            };
                            res.IsSuccess = true;
                        }
                        else
                        {
                            var rq = db.Q_DailyRequire.FirstOrDefault(x => x.PrintTime.Year == date.Year && x.PrintTime.Month == date.Month && x.PrintTime.Day == date.Day && x.TicketNumber == ticket);
                            if (rq != null)
                            {
                                var newobj = new Q_DailyRequire_Detail();
                                newobj.DailyRequireId = rq.Id;
                                newobj.MajorId = majorId;
                                newobj.UserId = userId;
                                newobj.EquipCode = equipCode;
                                newobj.StatusId = (int)eStatus.DAGXL;
                                newobj.ProcessTime = date;
                                int num = 0;
                                if (UseWithThirdPattern == 0)
                                    num = rq.TicketNumber;
                                else
                                {
                                    try
                                    {

                                        num = int.Parse(rq.STT_PhongKham);
                                    }
                                    catch (Exception)
                                    {
                                        num = rq.TicketNumber;
                                    }
                                }
                                var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                                if (equip != null)
                                    db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + ticket + ",CurrentNumber=" + ticket + "  where Id=" + equip.CounterId);

                                db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                                db.Q_DailyRequire_Detail.Add(newobj);
                                db.SaveChanges();
                                res.Data = newobj.Q_DailyRequire.ServeTimeAllow;
                                res.Data_1 = newobj.ProcessTime.Value.TimeOfDay;
                                res.Data_3 = new TicketInfo()
                                {
                                    RequireDetailId = newobj.Id,
                                    RequireId = newobj.DailyRequireId,
                                    StartTime = newobj.ProcessTime.Value.TimeOfDay,
                                    TimeServeAllow = newobj.Q_DailyRequire.ServeTimeAllow,
                                    TicketNumber = num,
                                    QMSNumber = newobj.Q_DailyRequire.TicketNumber,
                                    ThirdNumber = newobj.Q_DailyRequire.STT_PhongKham,
                                    CounterId = equip.CounterId,
                                    PrintTime = newobj.Q_DailyRequire.PrintTime,
                                    EquipCode = equipCode,
                                    CountDown = getCountDownTime(newobj)
                                };
                                res.IsSuccess = true;
                            }
                            else
                            {
                                res.IsSuccess = false;
                                res.sms = "Không tìm thấy số phiếu " + ticket;
                                res.Title = "Lỗi";
                            }
                        }
                    }
                    else
                        res.IsSuccess = false;
                }
            }
            catch (Exception)
            {
                res.IsSuccess = false;
                res.sms = "Lỗi thực thi dữ liệu";
                res.Title = "Lỗi";
            }
            return res;
        }

        /// <summary>
        /// call by major
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseBaseModel CallByMajor(string connectString, int majorId, int userId, int equipCode, DateTime date, int UseWithThirdPattern)
        {
            ResponseBaseModel res = new ResponseBaseModel();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    if (last == null)
                    {
                        string config = BLLConfig.Instance.GetConfigByCode(connectString, eConfigCode.CheckServiceLimit);
                        List<int> serviceOver = new List<int>();
                        bool hasCheckLimit = false;
                        if (!string.IsNullOrEmpty(config) && config == "1")
                        {
                            hasCheckLimit = true;
                        }
                        Start:
                        var check = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.MajorId == majorId && x.StatusId == (int)eStatus.CHOXL && !serviceOver.Contains(x.Q_DailyRequire.ServiceId));
                        if (check != null)
                        {
                            if (hasCheckLimit)
                            {
                                var userService = db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.UserId == userId && x.ServiceId == check.Q_DailyRequire.ServiceId);
                                if (userService != null && userService.Quantity == userService.CurrentQuantity)
                                {
                                    //ktra tiep xem co user nao còn gọi dc hay ko
                                    var otherUsers = db.Q_ServiceLimit.Where(x => !x.IsDeleted && x.Quantity != x.CurrentQuantity && x.ServiceId == check.Q_DailyRequire.ServiceId).ToList();
                                    if (otherUsers.Count > 0)
                                    {
                                        // neu co nguoi con goi dc thi ko dc lay phieu voi dich vu nay ma phai goi dich vu khac
                                        serviceOver.Add(check.Q_DailyRequire.ServiceId);
                                        goto Start;
                                    }
                                }
                            }

                            check.UserId = userId;
                            check.EquipCode = equipCode;
                            check.StatusId = (int)eStatus.DAGXL;
                            check.ProcessTime = date;
                            int num = 0;
                            if (UseWithThirdPattern == 0)
                                num = check.Q_DailyRequire.TicketNumber;
                            else
                            {
                                try
                                {
                                    num = int.Parse(check.Q_DailyRequire.STT_PhongKham);
                                }
                                catch (Exception)
                                {
                                    num = check.Q_DailyRequire.TicketNumber;
                                }
                            }
                            var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                            if (equip != null)
                            {
                                db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + num + ",CurrentNumber=" + num + " , isrunning=0 where Id=" + equip.CounterId);
                            }

                            db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                            db.SaveChanges();
                            res.IsSuccess = true;
                            res.Data = num;
                            res.Data_1 = check.Q_DailyRequire.ServeTimeAllow;
                            res.Data_2 = check.ProcessTime.Value.TimeOfDay;
                            res.Data_3 = new TicketInfo()
                            {
                                RequireDetailId = check.Id,
                                RequireId = check.DailyRequireId,
                                StartTime = check.ProcessTime.Value.TimeOfDay,
                                TimeServeAllow = check.Q_DailyRequire.ServeTimeAllow,
                                TicketNumber = num,
                                QMSNumber = check.Q_DailyRequire.TicketNumber,
                                ThirdNumber = check.Q_DailyRequire.STT_PhongKham,
                                CounterId = equip.CounterId,
                                PrintTime = check.Q_DailyRequire.PrintTime,
                                CountDown = getCountDownTime(check)
                            };
                        }
                    }
                    else
                    {
                        res.IsSuccess = false;

                        db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                        db.Q_RequestTicket.Add(new Q_RequestTicket() { UserId = userId, CreatedAt = DateTime.Now });
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                res.IsSuccess = false;
                res.sms = "Lỗi thực thi dữ liệu";
                res.Title = "Lỗi";
            }
            return res;
        }

        /// <summary>
        /// goi mới phan điều theo nghiệp vụ
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        /// <param name="UseWithThirdPattern"></param>
        /// <returns></returns>
        public bool ChekCanCallNext(string connectString, int majorId, int userId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var continnue = false;
                var currentUsersLogin = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN select x.UserId).Distinct().ToList();

                var callInfos = (from x in db.Q_DailyRequire_Detail where x.MajorId == majorId && x.StatusId != (int)eStatus.CHOXL select x).GroupBy(x => x.UserId).Select(x => new { userid = x.Key, count = x.Count() }).OrderByDescending(x => x.count).ToList();
                int userCount = 0, min = 0, max = 0;

                for (int i = 0; i < currentUsersLogin.Count; i++)
                {
                    var user = callInfos.FirstOrDefault(x => x.userid == userId);
                    if (user != null)
                    {
                        if (user.count > max)
                            max = user.count;
                        if (user.count < min)
                            min = user.count;
                        if (currentUsersLogin[i] == userId)
                            userCount = user.count;
                    }
                    else
                    {
                        min = 0;
                        if (currentUsersLogin[i] == userId)
                            userCount = 0;
                    }
                }

                if (max > userCount || min == max)
                    continnue = true;

                return continnue;
            }
        }

        /// <summary>
        ///  Counter Soft
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="ticket"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseBaseModel CallAny(string connectString, int userId, int equipCode, int ticket, DateTime date)
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
                    var check = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.TicketNumber == ticket && (x.StatusId == (int)eStatus.CHOXL || x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.QUALUOT));
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
                            var rq = db.Q_DailyRequire.FirstOrDefault(x => x.TicketNumber == ticket);
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

        /// <summary>
        /// end all ticket which not End in another equipment
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        public void EndAllCallOfUserWithoutThisEquipment(string connectString, int userId, int equipCode)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var founds = (from x in db.Q_DailyRequire_Detail where x.UserId == userId && x.EquipCode != equipCode && x.StatusId == (int)eStatus.DAGXL select x).ToList();
                    if (founds.Count > 0)
                    {
                        var now = DateTime.Now;
                        foreach (var item in founds)
                        {
                            item.EndProcessTime = now;
                            item.StatusId = (int)eStatus.HOTAT;
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private string getCountDownTime(Q_DailyRequire_Detail obj)
        {
            DateTime tgtc = obj.ProcessTime.Value.Add(obj.Q_DailyRequire.ServeTimeAllow);
            TimeSpan tgcl = tgtc.TimeOfDay.Subtract(DateTime.Now.TimeOfDay);
            string _countDown = "";
            if (tgcl <= new TimeSpan(0, 0, 0))
            {
                _countDown = "-1";
            }
            else
                _countDown = tgcl.ToString(@"hh\:mm");
            return _countDown;
        }
        /// <summary>
        /// Counter
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="IsDoneCurrentTicket"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public TicketInfo CallNewTicket(string connectString, int[] majorIds, int userId, int equipCode, bool IsDoneCurrentTicket, DateTime date, int UseWithThirdPattern)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                TicketInfo ticketInfo = null;
                var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                if (last == null)
                {
                    string config = BLLConfig.Instance.GetConfigByCode(connectString, eConfigCode.CheckServiceLimit);
                    List<int> serviceOver = new List<int>();
                    bool hasCheckLimit = false;
                    if (!string.IsNullOrEmpty(config) && config == "1")
                    {
                        hasCheckLimit = true;
                    }
                    Start:
                    var obj = db.Q_DailyRequire_Detail.Where(x => majorIds.Contains(x.MajorId) && x.StatusId == (int)eStatus.CHOXL && !serviceOver.Contains(x.Q_DailyRequire.ServiceId)).OrderBy(x => x.Q_DailyRequire.PrintTime).ToList().FirstOrDefault();
                    if (obj != null)
                    {
                        if (hasCheckLimit)
                        {
                            var userService = db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.UserId == userId && x.ServiceId == obj.Q_DailyRequire.ServiceId);
                            if (userService != null && userService.Quantity == userService.CurrentQuantity)
                            {
                                //ktra tiep xem co user nao còn gọi dc hay ko
                                var otherUsers = db.Q_ServiceLimit.Where(x => !x.IsDeleted && x.Quantity != x.CurrentQuantity && x.ServiceId == obj.Q_DailyRequire.ServiceId).ToList();
                                if (otherUsers.Count > 0)
                                {
                                    // neu co nguoi con goi dc thi ko dc lay phieu voi dich vu nay ma phai goi dich vu khac
                                    serviceOver.Add(obj.Q_DailyRequire.ServiceId);
                                    goto Start;
                                }
                            }
                        }

                        obj.UserId = userId;
                        obj.EquipCode = equipCode;
                        obj.StatusId = (int)eStatus.DAGXL;
                        obj.ProcessTime = date;
                        int num = 0;
                        if (UseWithThirdPattern == 0)
                            num = obj.Q_DailyRequire.TicketNumber;
                        else
                        {
                            try
                            {
                                num = int.Parse(obj.Q_DailyRequire.STT_PhongKham);
                            }
                            catch (Exception)
                            {
                                num = obj.Q_DailyRequire.TicketNumber;
                            }
                        }

                        var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                        if (equip != null)
                            db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + num + " ,CurrentNumber=" + num + ",  isrunning=0 where Id=" + equip.CounterId);

                        db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                        db.SaveChanges();

                        ticketInfo = new TicketInfo()
                        {
                            RequireDetailId = obj.Id,
                            RequireId = obj.DailyRequireId,
                            StartTime = obj.ProcessTime.Value.TimeOfDay,
                            TimeServeAllow = obj.Q_DailyRequire.ServeTimeAllow,
                            TicketNumber = num,
                            QMSNumber = obj.Q_DailyRequire.TicketNumber,
                            ThirdNumber = obj.Q_DailyRequire.STT_PhongKham,
                            CounterId = equip.CounterId,
                            PrintTime = obj.Q_DailyRequire.PrintTime,
                            EquipCode = equipCode,
                            CountDown = getCountDownTime(obj)
                        };
                        return ticketInfo;
                    }
                    else
                    {

                        db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                        db.Q_RequestTicket.Add(new Q_RequestTicket() { UserId = userId, CreatedAt = DateTime.Now });
                        db.SaveChanges();
                        return ticketInfo;
                    }
                }
                else
                {
                    int num = 0;
                    if (UseWithThirdPattern == 0)
                        num = last.Q_DailyRequire.TicketNumber;
                    else
                    {
                        try
                        {
                            num = int.Parse(last.Q_DailyRequire.STT_PhongKham);
                        }
                        catch (Exception)
                        {
                            num = last.Q_DailyRequire.TicketNumber;
                        }
                    }

                    var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                    if (equip != null)
                        db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + num + " ,CurrentNumber=" + num + " , isrunning=0 where Id=" + equip.CounterId);

                    db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                    db.SaveChanges();
                    ticketInfo = new TicketInfo()
                    {
                        RequireDetailId = last.Id,
                        RequireId = last.DailyRequireId,
                        StartTime = last.ProcessTime.Value.TimeOfDay,
                        TimeServeAllow = last.Q_DailyRequire.ServeTimeAllow,
                        TicketNumber = num,
                        QMSNumber = last.Q_DailyRequire.TicketNumber,
                        ThirdNumber = last.Q_DailyRequire.STT_PhongKham,
                        CounterId = equip.CounterId,
                        PrintTime = last.Q_DailyRequire.PrintTime,
                        EquipCode = equipCode,
                        CountDown = getCountDownTime(last)
                    };
                    return ticketInfo;
                }
            }
        }

        /// <summary>
        /// Counter
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="userId"></param>
        /// <param name="equipId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int CallNewTicket(string connectString, int majorId, int userId, int equipId, DateTime date)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipId && x.StatusId == (int)eStatus.DAGXL);
                if (last == null)
                {
                    Q_DailyRequire_Detail obj;
                    if (majorId != 0)
                        obj = db.Q_DailyRequire_Detail.Where(x => x.MajorId == majorId && x.StatusId == (int)eStatus.CHOXL).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
                    else
                        obj = db.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.CHOXL).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.UserId = userId;
                        obj.EquipCode = equipId;
                        obj.StatusId = (int)eStatus.DAGXL;
                        obj.ProcessTime = date;
                        var num = obj.Q_DailyRequire.TicketNumber;
                        var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipId);
                        if (equip != null)
                            db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + num + ",CurrentNumber=" + num + " , isrunning=0 where Id=" + equip.CounterId);
                        db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                        db.SaveChanges();
                        return obj.Q_DailyRequire.TicketNumber;
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                        db.Q_RequestTicket.Add(new Q_RequestTicket() { UserId = userId, CreatedAt = DateTime.Now });
                        db.SaveChanges();
                        return 0;
                    }
                }
                else
                    return last.Q_DailyRequire.TicketNumber;
            }
        }

        public TicketInfo CallNewTicket_GLP_NghiepVu(string connectString, int[] majorIds, int userId, int equipId, DateTime date, int UseWithThirdPattern)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipId);
                var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipId && x.StatusId == (int)eStatus.DAGXL);
                if (last == null)
                {
                    string config = BLLConfig.Instance.GetConfigByCode(connectString, eConfigCode.CheckServiceLimit);
                    List<int> serviceOver = new List<int>();
                    bool hasCheckLimit = false;
                    if (!string.IsNullOrEmpty(config) && config == "1")
                    {
                        hasCheckLimit = true;
                    }
                    Start:
                    var obj = db.Q_DailyRequire_Detail.Where(x => majorIds.Contains(x.MajorId) && x.StatusId == (int)eStatus.CHOXL && !serviceOver.Contains(x.Q_DailyRequire.ServiceId)).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
                    if (obj != null)
                    {
                        if (hasCheckLimit)
                        {
                            var userService = db.Q_ServiceLimit.FirstOrDefault(x => !x.IsDeleted && x.UserId == userId && x.ServiceId == obj.Q_DailyRequire.ServiceId);
                            if (userService != null && userService.Quantity == userService.CurrentQuantity)
                            {
                                //ktra tiep xem co user nao còn gọi dc hay ko
                                var otherUsers = db.Q_ServiceLimit.Where(x => !x.IsDeleted && x.Quantity != x.CurrentQuantity && x.ServiceId == obj.Q_DailyRequire.ServiceId).ToList();
                                if (otherUsers.Count > 0)
                                {
                                    // neu co nguoi con goi dc thi ko dc lay phieu voi dich vu nay ma phai goi dich vu khac
                                    serviceOver.Add(obj.Q_DailyRequire.ServiceId);
                                    goto Start;
                                }
                            }
                        }

                        obj.UserId = userId;
                        obj.EquipCode = equipId;
                        obj.StatusId = (int)eStatus.DAGXL;
                        obj.ProcessTime = date;
                        int num = 0;
                        if (UseWithThirdPattern == 0)
                            num = obj.Q_DailyRequire.TicketNumber;
                        else
                        {
                            try
                            {

                                num = int.Parse(obj.Q_DailyRequire.STT_PhongKham);
                            }
                            catch (Exception)
                            {
                                num = obj.Q_DailyRequire.TicketNumber;
                            }
                        }

                        if (equip != null)
                            db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + num + ",CurrentNumber=" + num + ", isrunning=0 where Id=" + equip.CounterId);
                        db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                        db.SaveChanges();
                        return new TicketInfo()
                        {
                            RequireDetailId = obj.Id,
                            RequireId = obj.DailyRequireId,
                            StartTime = obj.ProcessTime.Value.TimeOfDay,
                            TimeServeAllow = obj.Q_DailyRequire.ServeTimeAllow,
                            TicketNumber = num,
                            QMSNumber = obj.Q_DailyRequire.TicketNumber,
                            ThirdNumber = obj.Q_DailyRequire.STT_PhongKham,
                            CounterId = equip.CounterId,
                            PrintTime = obj.Q_DailyRequire.PrintTime,
                            EquipCode = equipId,
                            CountDown = getCountDownTime(obj)
                        };
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                        db.Q_RequestTicket.Add(new Q_RequestTicket() { UserId = userId, CreatedAt = DateTime.Now });
                        db.SaveChanges();
                        return null;
                    }
                }
                else
                {
                    int num = 0;
                    if (UseWithThirdPattern == 0)
                        num = last.Q_DailyRequire.TicketNumber;
                    else
                    {
                        try
                        {
                            num = int.Parse(last.Q_DailyRequire.STT_PhongKham);
                        }
                        catch (Exception)
                        {
                            num = last.Q_DailyRequire.TicketNumber;
                        }
                    }

                    return new TicketInfo()
                    {
                        RequireDetailId = last.Id,
                        RequireId = last.DailyRequireId,
                        StartTime = last.ProcessTime.Value.TimeOfDay,
                        TimeServeAllow = last.Q_DailyRequire.ServeTimeAllow,
                        TicketNumber = num,
                        QMSNumber = last.Q_DailyRequire.TicketNumber,
                        ThirdNumber = last.Q_DailyRequire.STT_PhongKham,
                        CounterId = equip.CounterId,
                        PrintTime = last.Q_DailyRequire.PrintTime,
                        EquipCode = equipId,
                        CountDown = getCountDownTime(last)
                    };
                }
            }
        }

        public bool TranferTicket(string connectString, int equipCode, int majorId, int ticket, DateTime date, bool isCountersoftCall)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    Q_DailyRequire_Detail obj = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day && x.MajorId == majorId && x.StatusId == (int)eStatus.CHOXL && x.Q_DailyRequire.TicketNumber == ticket).FirstOrDefault();
                    if (obj == null)
                    {
                        var dq = db.Q_DailyRequire.FirstOrDefault(x => x.PrintTime.Year == date.Year && x.PrintTime.Month == date.Month && x.PrintTime.Day == date.Day && x.TicketNumber == ticket);
                        if (dq != null)
                        {
                            if (isCountersoftCall)
                            {
                                //end processing
                                var curr = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.DailyRequireId == dq.Id && x.StatusId == (int)eStatus.DAGXL);
                                if (curr != null)
                                {
                                    curr.EndProcessTime = DateTime.Now;
                                    curr.StatusId = (int)eStatus.HOTAT;
                                }
                                //remove waiting
                                var waits = db.Q_DailyRequire_Detail.Where(x => x.DailyRequireId == dq.Id && x.StatusId == (int)eStatus.CHOXL).ToList();
                                if (waits.Count > 0)
                                    for (int i = 0; i < waits.Count; i++)
                                        db.Q_DailyRequire_Detail.Remove(waits[i]);

                            }
                            obj = new Q_DailyRequire_Detail() { DailyRequireId = dq.Id, MajorId = majorId, StatusId = (int)eStatus.CHOXL };
                            db.Q_DailyRequire_Detail.Add(obj);
                            // add main to check auto call
                            var require = new Q_CounterSoftRequire();
                            require.TypeOfRequire = (int)eCounterSoftRequireType.CheckUserFree;
                            require.Content = JsonConvert.SerializeObject(new ModelSelectItem() { Id = majorId, Data = dq.ServiceId, Code = dq.TicketNumber.ToString() });
                            require.CreatedDate = DateTime.Now;
                            db.Q_CounterSoftRequire.Add(require);
                            db.SaveChanges();
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Counter
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetTicket(string connectString, List<int> majorId, int userId, int equipCode, DateTime date)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                if (last == null)
                {
                    var obj = db.Q_DailyRequire_Detail.Where(x => majorId.Contains(x.MajorId) && x.StatusId == (int)eStatus.CHOXL).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.UserId = userId;
                        obj.EquipCode = equipCode;
                        obj.ProcessTime = DateTime.Now;
                        obj.StatusId = (int)eStatus.DAGXL;
                        db.SaveChanges();
                        return obj.Q_DailyRequire.TicketNumber;
                    }
                    else
                        return 0;
                }
                else
                    return last.Q_DailyRequire.TicketNumber;
            }
        }

        public int CountTicketDoneProcessed(string connectString, int equipCode)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.HOTAT && x.EquipCode == equipCode).Count();
            }
        }

        public int CountTicketWatingProcessed(string connectString, int equipCode)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.HOTAT && x.EquipCode == equipCode).Count();
            }
        }

        public int CountTicket(string connectString, int businessId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_DailyRequire.Where(x => x.BusinessId == businessId).Count();
            }
        }

        public ResponseBase Insert(string connectString, int ticketNumber, int serviceId, int businessId, DateTime printTime, string maCongViec, string maLoaiCongViec)
        {
            var rs = new ResponseBase();
            rs.IsSuccess = false;
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = new Q_DailyRequire();
                obj.TicketNumber = ticketNumber;
                obj.ServiceId = serviceId;
                obj.BusinessId = null;
                if (businessId != 0)
                    obj.BusinessId = businessId;
                obj.PrintTime = printTime;
                if (!string.IsNullOrEmpty(maCongViec) && !string.IsNullOrEmpty(maLoaiCongViec))
                {
                    obj = LayGioPhucVuDuKien(db, maCongViec, maLoaiCongViec, obj);
                }
                db.Q_DailyRequire.Add(obj);
                db.SaveChanges();

                var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == serviceId).OrderBy(x => x.Index).FirstOrDefault();
                var detail = new Q_DailyRequire_Detail();
                detail.Q_DailyRequire = obj;
                detail.MajorId = (nv != null ? nv.MajorId : 1);
                detail.StatusId = (int)eStatus.CHOXL;
                db.Q_DailyRequire_Detail.Add(detail);
                db.SaveChanges();
                rs.IsSuccess = true;
                rs.Data = obj.Id;
                rs.Data_1 = detail.MajorId;
                return rs;
            }
        }

        public int GetCurrentNumber(string connectString, int serviceId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                int num = 0;
                if (serviceId != 0)
                    num = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == serviceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.Q_DailyRequire.TicketNumber).Select(x => x.Q_DailyRequire.TicketNumber).FirstOrDefault();
                else
                    num = db.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.Q_DailyRequire.TicketNumber).Select(x => x.Q_DailyRequire.TicketNumber).FirstOrDefault();

                return (num != null ? num : 0);
            }
        }

        public int GetLastTicketNumber(string connectString, int serviceId, DateTime date)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                Q_DailyRequire obj;
                if (serviceId != 0)
                    obj = db.Q_DailyRequire.Where(x => x.PrintTime.Year == date.Year && x.PrintTime.Month == date.Month && x.PrintTime.Day == date.Day && x.ServiceId == serviceId).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                else
                    obj = db.Q_DailyRequire.Where(x => x.PrintTime.Year == date.Year && x.PrintTime.Month == date.Month && x.PrintTime.Day == date.Day).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                return obj != null ? obj.TicketNumber : 0;
            }
        }

        /// <summary>
        /// Web
        /// </summary>
        /// <param name="IsTienthu"></param>
        /// <param name="maNVThuNganTienThu"></param>
        /// <param name="counterIdsNotShow"></param>
        /// <returns></returns>
        public ViewModel GetDayInfo(string connectString, bool IsTienthu, int maNVThuNganTienThu, int[] counterIdsNotShow, int[] distanceTimeAllow)
        {
            using (var db_ = new QMSSystemEntities(connectString))
            {
                TimeSpan distanceWarning = new TimeSpan(0, 2, 0);
                var cf = db_.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.DistanceWarningTimeEnd);
                if (cf != null)
                {
                    var timearr = cf.Value.Split(':').Select(x => Convert.ToInt32(x)).ToArray();
                    distanceWarning = new TimeSpan(timearr[0], timearr[1], timearr[2]);
                }
                DateTime Now = DateTime.Now;
                var totalrequireds = db_.Q_DailyRequire_Detail.ToList();
                var dayInfo = new ViewModel();
                dayInfo.Date = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                dayInfo.Time = DateTime.Now.ToString("HH' : 'mm' : 'ss");
                dayInfo.TotalCar = totalrequireds.Select(x => x.DailyRequireId).Distinct().Count();
                dayInfo.TotalCarWaiting = totalrequireds.Where(x => x.StatusId == (int)eStatus.CHOXL).Select(x => x.DailyRequireId).Distinct().Count();
                if (IsTienthu)
                {
                    var numbers = db_.Q_DailyRequire_Detail.Where(x => x.UserId.Value == maNVThuNganTienThu && x.StatusId == (int)eStatus.CHOXL).Select(x => x.DailyRequireId).Distinct().ToList();
                    dayInfo.TotalCarServed = totalrequireds.Where(x => x.StatusId == (int)eStatus.HOTAT || numbers.Contains(x.DailyRequireId)).Select(x => x.DailyRequireId).Distinct().Count();
                    dayInfo.TotalCarProcessing = totalrequireds.Where(x => !numbers.Contains(x.DailyRequireId) && x.StatusId == (int)eStatus.DAGXL && x.UserId != maNVThuNganTienThu).Select(x => x.DailyRequireId).Distinct().Count();
                    dayInfo.TotalCarWaiting = (dayInfo.TotalCar - (dayInfo.TotalCarServed + dayInfo.TotalCarProcessing));    //totalrequireds.Where(x => x.StatusId == (int)eStatus.CHOXL).Select(x => x.DailyRequireId).Distinct().Count();
                    if (dayInfo.TotalCarWaiting < 0)
                        dayInfo.TotalCarWaiting = 0;
                }
                else
                {
                    dayInfo.TotalCarProcessing = totalrequireds.Where(x => x.StatusId == (int)eStatus.CHOXL).Select(x => x.DailyRequireId).Distinct().Count();
                    dayInfo.TotalCarServed = totalrequireds.Where(x => x.StatusId == (int)eStatus.HOTAT).Select(x => x.DailyRequireId).Distinct().Count();
                }
                try
                {
                    dayInfo.Details.AddRange(db_.Q_Counter.Where(x => !x.IsDeleted && !counterIdsNotShow.Contains(x.Id)).OrderBy(x => x.Index).Select(x => new ViewDetailModel()
                    {
                        CarNumber = "0",
                        TableId = x.Id,
                        TableName = x.Name,
                        TableCode = x.ShortName,
                        Start = null,
                        TicketNumber = 0,
                        TimeProcess = "0",
                        StartStr = "0",
                        strTimeCL = "0",
                        strServeTimeAllow = "0",
                        GioGiaoDK = "0",
                        TienDoTH = 0,
                        strTGDK_Khach = "0"
                    }).ToList());

                    var objs = db_.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL).Select(x => new ViewDetailModel()
                    {
                        STT = (int)x.Id,
                        TableId = x.EquipCode ?? 0,
                        // TableId = x.Q_Equipment.CounterId,
                        CarNumber = x.Q_DailyRequire.CarNumber,
                        //TableName = x.Q_Equipment.Q_Counter.Name,
                        Start = x.ProcessTime.Value,
                        TicketNumber = x.Q_DailyRequire.TicketNumber,
                        Temp = null,
                        ServeTimeAllow = x.Q_DailyRequire.ServeTimeAllow,
                        IsEndTime = false,
                        strTimeCL = "0",
                        strServeTimeAllow = "0",
                        TimeProcess = "0",
                        StartStr = "0",
                        GioGiaoDK = "0",
                        TienDoTH = 0,
                        ReadServeOverCounter = x.ServeOverCounter,
                        strTGDK_Khach = "0"
                    }).ToList();

                    if (dayInfo.Details.Count > 0)
                    {
                        var equips = db_.Q_Equipment.Where(x => !x.IsDeleted && x.EquipTypeId == (int)eEquipType.Counter);
                        foreach (var item in objs)
                            item.TableId = equips.FirstOrDefault(x => x.Code == item.TableId).CounterId;

                        var logins = (from x in db_.Q_Login where x.StatusId == (int)eStatus.LOGIN select x).ToList();

                        var registerTicket = (from x in db_.Q_RequestTicket where !x.IsDeleted select x).ToList();
                        foreach (var item in dayInfo.Details)
                        {
                            var find = objs.FirstOrDefault(x => x.TableId == item.TableId);
                            if (find != null)
                            {
                                if (find.Temp == null)
                                {
                                    //var fObj = db_.Q_DailyRequire_Detail.FirstOrDefault(x => x.Id == find.STT);
                                    //fObj.Temp = find.Start;
                                    //find.Temp = find.Start;
                                }
                                item.STT = find.STT;
                                item.ReadServeOverCounter = find.ReadServeOverCounter;
                                item.CarNumber = !string.IsNullOrEmpty(find.CarNumber) ? find.CarNumber : "0";
                                item.TicketNumber = find.TicketNumber;
                                item.Start = find.Start;
                                item.strServeTimeAllow = (find.ServeTimeAllow.ToString(@"hh\:mm")).Replace(":", "<label class='blue'>:</label>");

                                if (find.Start != null)
                                {
                                    TimeSpan time = DateTime.Now.Subtract(item.Start.Value);
                                    item.TimeProcess = time.ToString(@"hh\:mm").Replace(":", "<label class='blue'>:</label>");
                                    item.StartStr = item.Start.Value.ToString(@"hh\:mm").Replace(":", "<label class='blue'>:</label>");

                                    DateTime tgtc = item.Start.Value.Add(find.ServeTimeAllow);
                                    TimeSpan tgcl = tgtc.TimeOfDay.Subtract(DateTime.Now.TimeOfDay);
                                    if (tgcl <= distanceWarning || tgcl <= new TimeSpan(0, 0, 0))
                                    {
                                        item.IsEndTime = true;
                                        item.strTimeCL = tgcl.ToString(@"hh\:mm").Replace(":", "<label class='blue'>:</label>");
                                        if (tgcl <= new TimeSpan(0, 0, 0))
                                            item.strTimeCL = ("00:00").Replace(":", "<label class='blue'>:</label>");
                                    }
                                    else
                                        item.strTimeCL = tgcl.ToString(@"hh\:mm").Replace(":", "<label class='blue'>:</label>");

                                    item.GioGiaoDK = item.Start.Value.AddSeconds(find.ServeTimeAllow.TotalSeconds).ToString(@"hh\:mm").Replace(":", "<label class='blue'>:</label>");

                                    decimal tiendo = 100;
                                    if (find.ServeTimeAllow.TotalSeconds > 0)
                                        tiendo = Math.Ceiling((decimal)(time.TotalSeconds / find.ServeTimeAllow.TotalSeconds) * 100);
                                    item.TienDoTH = (int)(tiendo > 100 ? 100 : tiendo);
                                }
                            }

                            var foundLogin = logins.OrderBy(x => x.Date).FirstOrDefault(x => x.EquipCode == item.TableId);
                            if (foundLogin != null && registerTicket.Count > 0)
                            {
                                var foundDK = registerTicket.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.UserId == foundLogin.UserId);
                                if (foundDK != null)
                                {
                                    item.TGDK_Khach = foundDK.CreatedAt;
                                    item.strTGDK_Khach = (foundDK.CreatedAt.ToString(@"hh\:mm\:ss")).Replace(":", "<label class='blue'>:</label>");
                                }
                            }
                        }
                        db_.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                }
                return dayInfo;
            }
        }

        public ViewModel GetDayInfo(string connectString, int[] counterIds, int[] services)
        {
            using (var db_ = new QMSSystemEntities(connectString))
            {
                var returnModel = new ViewModel();
                try
                {
                    returnModel.Services.AddRange(db_.Q_Service.Where(x => !x.IsDeleted && x.IsActived && services.Contains(x.Id)).Select(x => new SubModel() { ServiceId = x.Id, ServiceName = x.Name, Ticket = 0 }));
                    if (returnModel.Services.Count > 0)
                    {
                        var callObjs = db_.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL).Select(x => new ServiceModel() { Id = x.Q_DailyRequire.ServiceId, TimeProcess = x.EndProcessTime ?? DateTime.Now, StartNumber = x.Q_DailyRequire.TicketNumber }).ToList();
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

                    returnModel.Details.AddRange(db_.Q_Counter.Where(x => !x.IsDeleted && counterIds.Contains(x.Id)).OrderBy(x => x.Index).Select(x => new ViewDetailModel()
                    {
                        STT = (!x.IsRunning ? 1 : 0),
                        CarNumber = "0",
                        TableId = x.Id,
                        TableName = x.Name,
                        TableCode = x.ShortName,
                        Start = null,
                        TicketNumber = x.CurrentNumber,
                        TimeProcess = "0",
                        StartStr = (!x.IsRunning ? ("Mời bệnh nhân " + x.CurrentNumber + " đến phòng : " + x.Name) : "")
                    }));
                    if (returnModel.Details.Count > 0)
                    {
                        returnModel.Details[0].RuningText = returnModel.Details.Where(x => x.STT == 1).Select(x => x.StartStr).ToArray();
                        db_.Database.ExecuteSqlCommand("update q_counter set isrunning =1");
                        db_.SaveChanges();
                    }
                }
                catch (Exception)
                { }
                return returnModel;
            }
        }

        public ViewModel GetDayInfo(string connectString, int[] counterIds, int[] services, int userId)
        {
            using (var db_ = new QMSSystemEntities(connectString))
            {
                var returnModel = new ViewModel();
                try
                {
                    returnModel.Services.AddRange(db_.Q_Service.Where(x => !x.IsDeleted && x.IsActived && services.Contains(x.Id)).Select(x => new SubModel() { ServiceId = x.Id, ServiceName = x.Name, Ticket = 0 }));
                    if (returnModel.Services.Count > 0)
                    {
                        var callObjs = db_.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL).Select(x => new ServiceModel() { Id = x.Q_DailyRequire.ServiceId, TimeProcess = x.EndProcessTime ?? DateTime.Now, StartNumber = x.Q_DailyRequire.TicketNumber }).ToList();
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
                            EquipCode = x.Code
                        }).ToList();

                    var dangXL = db_.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL).ToList();

                    foreach (var item in counters)
                    {
                        var current = dangXL.Where(x => x.StatusId == (int)eStatus.DAGXL && x.EquipCode == item.EquipCode).OrderByDescending(x => x.ProcessTime).FirstOrDefault();
                        if (current != null)
                        {
                            //  if (useWithThridPattern == 0)
                            item.TicketNumber = current.Q_DailyRequire.TicketNumber;
                            //  else
                            //   {
                            //      if (current.Q_DailyRequire.STT_PhongKham != null)
                            //            item.CurrentTicket = int.Parse(current.Q_DailyRequire.STT_PhongKham);
                            //   }

                        }
                        returnModel.Details.Add(item);
                    }
                    returnModel.Sounds = BLLTVReadSound.Instance.Gets(connectString, counterIds, userId);
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

        public ModelSelectItem GetCurrentProcess(string connectString, string userName)
        {
            var rs = new ModelSelectItem();
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_DailyRequire_Detail.Where(x => (x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.DANHGIA) && x.ProcessTime.Value.Day == DateTime.Now.Day && x.ProcessTime.Value.Month == DateTime.Now.Month && x.ProcessTime.Value.Year == DateTime.Now.Year && x.Q_User.UserName.Trim().ToUpper().Equals(userName)).FirstOrDefault();
                if (obj != null)
                {
                    rs.Id = obj.Q_DailyRequire.TicketNumber;
                    rs.Data = (obj.StatusId == (int)eStatus.DANHGIA ? 1 : 0);
                }
                else
                    rs.Id = 0;
            }
            return rs;
        }

        public ModelSelectItem GetSTT_UserName(string connectString, int maTB)
        {
            var rs = new ModelSelectItem();
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_DailyRequire_Detail.Where(x => (x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.DANHGIA) && x.ProcessTime.Value.Day == DateTime.Now.Day && x.ProcessTime.Value.Month == DateTime.Now.Month && x.ProcessTime.Value.Year == DateTime.Now.Year && x.EquipCode.HasValue && x.EquipCode.Value == maTB).FirstOrDefault();
                if (obj != null)
                {
                    rs.Id = obj.Q_DailyRequire.TicketNumber;
                    rs.Code = obj.Q_User.Name;
                    rs.Data = (obj.StatusId == (int)eStatus.DANHGIA ? 1 : 0);
                }
                else
                    rs.Id = 0;
            }
            return rs;
        }
        
        public void CopyHistory(string connectString, bool isver3)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                DateTime now = DateTime.Now,
                  today = new DateTime(now.Year, now.Month, now.Day);
                List<int> ids;
                var requires = db.Q_DailyRequire.Where(x => x.PrintTime < today).ToList();
                if (requires.Count > 0)
                {
                    ids = requires.Select(x => x.Id).ToList();
                    var reDetails = db.Q_DailyRequire_Detail.Where(x => ids.Contains(x.DailyRequireId)).ToList();
                    ids = reDetails.Select(x => x.Id).ToList();
                    var userEvas = db.Q_UserEvaluate.Where(x => !x.IsDeleted && ids.Contains(x.DailyRequireDeId ?? 0)).ToList();

                    List<ModelSelectItem> dailyWorkDetails = new List<ModelSelectItem>();
                    if (isver3)
                    {
                        dailyWorkDetails = db.Q_DailyRequire_WorkDetail.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.DailyRequireId, Data = x.WorkDetailId }).ToList();
                    }
                    Q_HisDailyRequire grandParentObj;
                    Q_HisDailyRequire_De ParentObj;
                    Q_HisUserEvaluate childObj;
                    foreach (var grand in requires)
                    {
                        grandParentObj = new Q_HisDailyRequire();
                        Parse.CopyObject(grand, ref grandParentObj);
                        grandParentObj.Id = 0;
                        grandParentObj.Q_HisDailyRequire_De = new Collection<Q_HisDailyRequire_De>();
                        foreach (var parent in reDetails.Where(x => x.DailyRequireId == grand.Id))
                        {
                            ParentObj = new Q_HisDailyRequire_De();
                            Parse.CopyObject(parent, ref ParentObj);
                            ParentObj.Id = 0;
                            ParentObj.Q_HisUserEvaluate = new Collection<Q_HisUserEvaluate>();
                            foreach (var child in userEvas.Where(x => x.DailyRequireDeId == parent.Id))
                            {
                                childObj = new Q_HisUserEvaluate();
                                Parse.CopyObject(child, ref childObj);
                                childObj.Id = 0;
                                childObj.HisDailyRequireDeId = 0;
                                childObj.Q_HisDailyRequire_De = ParentObj;
                                ParentObj.Q_HisUserEvaluate.Add(childObj);
                            }
                            ParentObj.HisDailyRequireId = 0;
                            ParentObj.Q_HisDailyRequire = grandParentObj;
                            grandParentObj.Q_HisDailyRequire_De.Add(ParentObj);
                        }

                        if (isver3)
                        {
                            grandParentObj.Q_HisDailyRequire_WorkDetail = new List<Q_HisDailyRequire_WorkDetail>();
                            foreach (var item in dailyWorkDetails.Where(x => x.Id == grand.Id))
                            {
                                grandParentObj.Q_HisDailyRequire_WorkDetail.Add(new Q_HisDailyRequire_WorkDetail()
                                {
                                    Q_HisDailyRequire = grandParentObj,
                                    WorkDetailId = item.Data
                                });
                            }
                            db.Database.ExecuteSqlCommand("delete Q_DailyRequire_WorkDetail  DBCC CHECKIDENT('Q_DailyRequire_WorkDetail', RESEED, 0) ");
                        }
                        db.Q_HisDailyRequire.Add(grandParentObj);
                    }
                    db.Q_UserEvaluate.RemoveRange(userEvas);
                    db.Q_DailyRequire_Detail.RemoveRange(reDetails);
                    db.Q_DailyRequire.RemoveRange(requires);

                    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Q_UserEvaluate', RESEED, 0);DBCC CHECKIDENT('Q_DailyRequire_Detail', RESEED, 0);DBCC CHECKIDENT('Q_DailyRequire', RESEED, 0); ");
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Print ticket
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseBase PrintNewTicket(string connectString,
            int serviceId,
            int serviceNumber,
            int businessId,
            DateTime printTime,
            int? printType,
            TimeSpan? serveTimeAllow,
            string Name,
            string Address,
            int? DOB,
            string MaBenhNhan,
            string MaPhongKham,
            string SttPhongKham,
            string SoXe,
            string maCongViec,
            string maLoaiCongViec
            )
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    Q_DailyRequire rq = null,
                       modelObj = new Q_DailyRequire();
                    int socu = 0;
                    int _serviceNumber = 0;

                    var _printType = 1;
                    var cf = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.PrintType);
                    if (cf != null)
                        int.TryParse(cf.Value, out _printType);
                    printType = _printType; 

                    if (!string.IsNullOrEmpty(maCongViec) && !string.IsNullOrEmpty(maLoaiCongViec))
                    {
                        modelObj = LayGioPhucVuDuKien(db, maCongViec, maLoaiCongViec, modelObj);
                        if (serveTimeAllow.HasValue)
                            modelObj.ServeTimeAllow = modelObj.ServeTimeAllow.Add(serveTimeAllow.Value);
                        serveTimeAllow = modelObj.ServeTimeAllow;
                    }
                    if (printType == (int)ePrintType.TheoTungDichVu)
                    {
                        rq = db.Q_DailyRequire.Where(x => x.ServiceId == serviceId).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                        var ser = db.Q_Service.FirstOrDefault(x => x.Id == serviceId);
                        if (ser != null)
                        {
                            _serviceNumber = ser.StartNumber;
                            if (!serveTimeAllow.HasValue)
                                serveTimeAllow = ser.TimeProcess.TimeOfDay;
                        }
                    }
                    else if (printType == (int)ePrintType.BatDauChung)
                    {
                        rq = db.Q_DailyRequire.OrderByDescending(x => x.TicketNumber).FirstOrDefault();

                        cf = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.StartNumber);
                        if (cf != null)
                            int.TryParse(cf.Value, out _serviceNumber); 
                    }
                    serviceNumber = _serviceNumber;
                    socu = ((rq == null) ? serviceNumber : rq.TicketNumber);

                    var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == serviceId).OrderBy(x => x.Index).FirstOrDefault();
                    if (nv == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                    }
                    else
                    {
                        modelObj.TicketNumber = (socu + 1);
                        modelObj.ServiceId = serviceId;
                        modelObj.BusinessId = null;
                        if (businessId != 0)
                            modelObj.BusinessId = businessId;
                        modelObj.PrintTime = printTime;
                        modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                        modelObj.CustomerName = Name;
                        modelObj.CustomerDOB = DOB;
                        modelObj.CustomerAddress = Address;
                        modelObj.MaBenhNhan = MaBenhNhan;
                        modelObj.MaPhongKham = MaPhongKham;
                        modelObj.STT_PhongKham = SttPhongKham;
                        modelObj.CarNumber = SoXe;
                        modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                        var foundService = db.Q_Service.FirstOrDefault(x => x.Id == serviceId);

                        var detail = new Q_DailyRequire_Detail();
                        detail.Q_DailyRequire = modelObj;
                        detail.MajorId = nv.MajorId;
                        detail.StatusId = (int)eStatus.CHOXL;

                        if (foundService != null && foundService.AutoEnd)
                        {
                            detail.StatusId = (int)eStatus.HOTAT;
                            var now = DateTime.Now;
                            var timeend = foundService.TimeAutoEnd != null ? foundService.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                            detail.ProcessTime = now;
                            detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                        }
                        modelObj.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(modelObj);

                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == serviceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

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
                        rs.Data = socu;
                        rs.Data_1 = detail.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = modelObj.TicketNumber;
                        rs.str1 = printType.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "Lỗi Exception", Message = "Lỗi thực thi. " + ex.Message });
            }
            return rs;
        }

        public ResponseBase PrintNewTicket(string connectString,
           int serviceId,
           int serviceNumber,
           int businessId,
           DateTime printTime,
           int? printType,
           TimeSpan? serveTimeAllow,
           string Name,
           string Address,
           int? DOB,
           string MaBenhNhan,
           string MaPhongKham,
           string SttPhongKham,
           string SoXe,
           string maCongViec,
           string maLoaiCongViec,
           int MajorId
           )
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    Q_DailyRequire rq = null,
                       modelObj = new Q_DailyRequire();
                    int socu = 0;
                    int _serviceNumber = 0;
                    if (!printType.HasValue)
                    {
                        var _printType = 1;
                        var cf = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.PrintType);
                        if (cf != null)
                            int.TryParse(cf.Value, out _printType);
                        printType = _printType;
                    }

                    if (!string.IsNullOrEmpty(maCongViec) && !string.IsNullOrEmpty(maLoaiCongViec))
                    {
                        modelObj = LayGioPhucVuDuKien(db, maCongViec, maLoaiCongViec, modelObj);
                        if (serveTimeAllow.HasValue)
                            modelObj.ServeTimeAllow = modelObj.ServeTimeAllow.Add(serveTimeAllow.Value);
                        serveTimeAllow = modelObj.ServeTimeAllow;
                    }
                    if (printType == (int)ePrintType.TheoTungDichVu)
                    {
                        rq = db.Q_DailyRequire.Where(x => x.ServiceId == serviceId).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                        var ser = db.Q_Service.FirstOrDefault(x => x.Id == serviceId);
                        if (ser != null)
                        {
                            _serviceNumber = ser.StartNumber;
                            if (!serveTimeAllow.HasValue)
                                serveTimeAllow = ser.TimeProcess.TimeOfDay;
                        }
                    }
                    else if (printType == (int)ePrintType.BatDauChung)
                    {
                        rq = db.Q_DailyRequire.OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                        var cf = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.StartNumber);
                        if (cf != null)
                            int.TryParse(cf.Value, out _serviceNumber);
                    }
                    socu = ((rq == null) ? serviceNumber : rq.TicketNumber);

                    var nv = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id == MajorId);
                    if (nv == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                    }
                    else
                    {
                        modelObj.TicketNumber = (socu + 1);
                        modelObj.ServiceId = serviceId;
                        modelObj.BusinessId = null;
                        if (businessId != 0)
                            modelObj.BusinessId = businessId;
                        modelObj.PrintTime = printTime;
                        modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                        modelObj.CustomerName = Name;
                        modelObj.CustomerDOB = DOB;
                        modelObj.CustomerAddress = Address;
                        modelObj.MaBenhNhan = MaBenhNhan;
                        modelObj.MaPhongKham = MaPhongKham;
                        modelObj.STT_PhongKham = SttPhongKham;
                        modelObj.CarNumber = SoXe;
                        modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                        var foundService = db.Q_Service.FirstOrDefault(x => x.Id == serviceId);

                        var detail = new Q_DailyRequire_Detail();
                        detail.Q_DailyRequire = modelObj;
                        detail.MajorId = nv.Id;
                        detail.StatusId = (int)eStatus.CHOXL;

                        if (foundService != null && foundService.AutoEnd)
                        {
                            detail.StatusId = (int)eStatus.HOTAT;
                            var now = DateTime.Now;
                            var timeend = foundService.TimeAutoEnd != null ? foundService.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                            detail.ProcessTime = now;
                            detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                        }
                        modelObj.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(modelObj);

                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == serviceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.Id && !x.IsDeleted select x.UserId).ToList();
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
                        rs.Data = socu;
                        rs.Data_1 = detail.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = modelObj.TicketNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "Lỗi Exception", Message = "Lỗi thực thi. " + ex.Message });
            }
            return rs;
        }

        private Q_DailyRequire LayGioPhucVuDuKien(QMSSystemEntities db, string maCongViec, string maLoaiCongViec, Q_DailyRequire model)
        {
            DateTime processTime = new DateTime(2020, 1, 1, 00, 00, 00);
            string[] codes = maCongViec.Split(',').Select(x => x.Trim().ToUpper()).ToArray();
            if (codes != null && codes.Length > 0)
            {
                var a = (from x in db.Q_WorkDetail
                         where !x.IsDeleted
                         select new ModelSelectItem() { Code = x.Q_Works.Code, Name = x.Q_WorkType.Code }).ToList();
                var congviecs = (from x in db.Q_WorkDetail
                                 where !x.IsDeleted &&
                                 codes.Contains(x.Q_Works.Code.Trim().ToUpper()) &&
                                 maLoaiCongViec.Trim().ToUpper().Equals(x.Q_WorkType.Code.Trim().ToUpper())
                                 select new { TimeProcess = x.TimeProcess, Id = x.Id }).ToArray();
                model.Q_DailyRequire_WorkDetail = new List<Q_DailyRequire_WorkDetail>();
                for (int i = 0; i < congviecs.Count(); i++)
                {
                    processTime = processTime.AddSeconds(congviecs[i].TimeProcess.TimeOfDay.TotalSeconds);
                    model.Q_DailyRequire_WorkDetail.Add(new Q_DailyRequire_WorkDetail()
                    {
                        Q_DailyRequire = model,
                        WorkDetailId = congviecs[i].Id
                    });
                }
            }
            model.ServeTimeAllow = processTime.TimeOfDay;
            return model;
        }

        /// <summary>
        /// android app
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="phone"></param>
        /// <param name="printTime"></param>
        /// <returns></returns>
        public ResponseBase RegisterByPhone(string connectString, int serviceId, string phone, DateTime printTime)
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    int printType = 1;
                    int.TryParse(db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.PrintType).Value, out printType);

                    Q_DailyRequire rq = null;
                    int sophieu = 0;
                    if (printType == (int)ePrintType.TheoTungDichVu)
                        rq = db.Q_DailyRequire.Where(x => x.ServiceId == serviceId).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                    else if (printType == (int)ePrintType.BatDauChung)
                        rq = db.Q_DailyRequire.OrderByDescending(x => x.TicketNumber).FirstOrDefault();

                    if (rq == null)
                    {
                        switch (printType)
                        {
                            case (int)ePrintType.TheoTungDichVu:
                                sophieu = db.Q_Service.FirstOrDefault(x => x.Id == serviceId).StartNumber;
                                break;
                            case (int)ePrintType.BatDauChung:
                                sophieu = int.Parse(db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.StartNumber).Value);
                                break;
                        }
                    }
                    else
                        sophieu = rq.TicketNumber + 1;

                    var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == serviceId).OrderBy(x => x.Index).FirstOrDefault();
                    if (nv == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                    }
                    else
                    {
                        var obj = new Q_DailyRequire();
                        obj.TicketNumber = sophieu;
                        obj.ServiceId = serviceId;
                        obj.BusinessId = null;
                        //   if (businessId != 0)
                        //      obj.BusinessId = businessId;
                        obj.PrintTime = printTime;
                        obj.PhoneNumber = phone;
                        obj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                        var detail = new Q_DailyRequire_Detail();
                        detail.Q_DailyRequire = obj;
                        detail.MajorId = (nv != null ? nv.MajorId : 1);
                        detail.StatusId = (int)eStatus.CHOXL;
                        obj.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(obj);

                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == serviceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Data = sophieu;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "Lỗi Exception", Message = "Lỗi thực thi. " + ex.Message });
            }
            return rs;
        }

        /// <summary>
        /// android app
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="phone"></param>
        /// <param name="printTime"></param>
        /// <returns></returns>
        public List<ModelSelectItem> CheckRegister(string connectString, string phone, DateTime date)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    return db.Q_DailyRequire.Where(x => x.PhoneNumber == phone).OrderByDescending(x => x.TicketNumber).Select(x => new ModelSelectItem() { Id = x.Id, Data = x.TicketNumber, Name = x.Q_Service.Name }).ToList();
                }
            }
            catch (Exception)
            { }
            return new List<ModelSelectItem>();
        }

        /// <summary>
        /// Android App
        /// </summary>
        /// <param name="requireId"></param>
        /// <returns></returns>
        public A_CurrentInfoModel CurrentInfo(string connectString, int requireId)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var obj = db.Q_DailyRequire_Detail.Where(x => x.DailyRequireId == requireId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).Select(x => new A_CurrentInfoModel()
                    {
                        Id = x.Id,
                        Ticket = x.Q_DailyRequire.TicketNumber,
                        UserName = x.Q_User.Name,
                        img = x.Q_User.Avatar,
                        Position = x.Q_User.Position,
                        Start = x.ProcessTime ?? DateTime.Now
                    }).FirstOrDefault();
                    // var evaluate

                }
            }
            catch (Exception)
            { }
            return new A_CurrentInfoModel();
        }

        public ResponseBase CheckServeInformation(string connectString, int ticket)
        {
            var rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var details = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.TicketNumber == ticket).Select(x => new A_CurrentInfoModel()
                    {
                        Id = x.Id,
                        Ticket = x.Q_DailyRequire.TicketNumber,
                        UserName = x.Q_User.Name,
                        img = x.Q_User.Avatar,
                        Position = x.Q_User.Position,
                        Start = x.ProcessTime ?? DateTime.Now,
                        StatusId = x.StatusId,
                        EndProcessTime = x.EndProcessTime,
                        ServiceId = x.Q_DailyRequire.ServiceId,
                        BaseServeTime = x.Q_DailyRequire.ServeTimeAllow
                    }).ToList();
                    if (details.Count > 0)
                    {
                        var checkObj = details.FirstOrDefault(x => x.StatusId == (int)eStatus.DAGXL);
                        if (checkObj == null)
                        {
                            checkObj = details.OrderByDescending(x => x.EndProcessTime).FirstOrDefault(x => x.StatusId == (int)eStatus.HOTAT);
                            if (checkObj == null)
                            {
                                var cf = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.PrintType);
                                A_CurrentInfoModel currentInfor = null;
                                long sum = 0;
                                List<Q_DailyRequire> dscho = null;
                                int startTicket = 0;
                                switch (Convert.ToInt32(cf.Value))
                                {
                                    case (int)ePrintType.BatDauChung:
                                        currentInfor = db.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).Select(x => new A_CurrentInfoModel()
                                        {
                                            Id = x.Id,
                                            Ticket = x.Q_DailyRequire.TicketNumber,
                                            UserName = x.Q_User.Name,
                                            img = x.Q_User.Avatar,
                                            Position = x.Q_User.Position,
                                            Start = x.ProcessTime ?? DateTime.Now
                                        }).FirstOrDefault();
                                        if (currentInfor != null)
                                            startTicket = currentInfor.Ticket;
                                        dscho = db.Q_DailyRequire.Where(x => x.TicketNumber > startTicket && x.TicketNumber < ticket).ToList();
                                        break;
                                    case (int)ePrintType.TheoTungDichVu:
                                        int serId = db.Q_DailyRequire.FirstOrDefault(x => x.TicketNumber == ticket).ServiceId;
                                        currentInfor = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == serId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).Select(x => new A_CurrentInfoModel()
                                        {
                                            Id = x.Id,
                                            Ticket = x.Q_DailyRequire.TicketNumber,
                                            UserName = x.Q_User.Name,
                                            img = x.Q_User.Avatar,
                                            Position = x.Q_User.Position,
                                            Start = x.ProcessTime ?? DateTime.Now
                                        }).FirstOrDefault();
                                        if (currentInfor != null)
                                            startTicket = currentInfor.Ticket;
                                        dscho = db.Q_DailyRequire.Where(x => x.ServiceId == serId && x.TicketNumber > startTicket && x.TicketNumber < ticket).ToList();
                                        break;
                                }
                                if (dscho != null && dscho.Count > 0)
                                    sum = (from r in dscho select r.ServeTimeAllow.Ticks).Sum();
                                rs.Data = "Wait";
                                rs.Records = "Số phiếu <span class='text-red bold'>" + ticket + "</span> của quý khách dự kiến sẽ được phục vụ lúc <span class='text-red bold'>" + ((sum > 0 ? DateTime.Now.TimeOfDay.Add(new TimeSpan(sum)) : DateTime.Now.TimeOfDay).ToString(@"hh\:mm")) + "</span>";
                                rs.Data_1 = "Số phiếu " + ticket + " của quý khách dự kiến sẽ được phục vụ lúc " + (DateTime.Now.TimeOfDay.Add(new TimeSpan(sum)).ToString(@"hh\:mm")) + " ";
                            }
                            else //da xu ly hoan tat
                            {
                                rs.Data = "End";
                                rs.Records = "Số phiếu  <span class='text-red bold'>" + ticket + "</span> của quý khách đã phục vụ xong lúc <span class='text-red bold'>" + checkObj.EndProcessTime.Value.ToShortTimeString() + "</span>";
                                rs.Data_1 = "Số phiếu " + ticket + " của quý khách đã phục vụ xong lúc " + checkObj.EndProcessTime.Value.ToShortTimeString() + " ";
                            }
                        }
                        else //dang xu ly
                        {
                            rs.Data = "Process";
                            rs.Records = "Số phiếu <span class='text-red bold'>" + ticket + "</span> của quý khách đang được phục vụ từ lúc " + checkObj.Start.ToString(@"hh\:mm") + " dự kiến hoàn thành lúc " + checkObj.Start.AddSeconds(checkObj.BaseServeTime.TotalSeconds).ToString(@"hh\:mm") + ".";
                            rs.Data_1 = "Số phiếu  " + ticket + " của quý khách đang được phục vụ từ lúc " + checkObj.Start.ToString(@"hh\:mm") + " dự kiến hoàn thành lúc " + checkObj.Start.AddSeconds(checkObj.BaseServeTime.TotalSeconds).ToString(@"hh\:mm") + ".";
                        }
                    }
                    else
                    {
                        rs.Records = "Không tìm thấy thông tin phiếu <span class='text-red bold'>" + ticket + "</span> trong hệ thống.";
                        rs.Data_1 = "Không tìm thấy thông tin phiếu " + ticket + " trong hệ thống.";
                    }
                }
            }
            catch (Exception ex)
            { }
            return rs;
        }

        public ResponseBase API_PrintNewTicket(string connectString,
            string Name,
            string Address,
            int? DOB,
            string MaBenhNhan,
            string MaPhongKham,
            string SttPhongKham
            )
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var DV = db.Q_Service.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(MaPhongKham.Trim().ToUpper()));
                    if (DV != null)
                    {
                        var CF = db.Q_Config.FirstOrDefault(x => x.Code.Equals(eConfigCode.PrintType));
                        if (CF != null)
                        {
                            Q_DailyRequire rq = null;
                            int socu = 0;
                            if (Convert.ToInt32(CF.Value) == (int)ePrintType.TheoTungDichVu)
                            {
                                rq = db.Q_DailyRequire.Where(x => x.ServiceId == DV.Id).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                                socu = ((rq == null) ? (DV.StartNumber - 1) : rq.TicketNumber);
                            }
                            else if (Convert.ToInt32(CF.Value) == (int)ePrintType.BatDauChung)
                            {
                                rq = db.Q_DailyRequire.OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                                socu = ((rq == null) ? (int.Parse(db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.StartNumber).Value) - 1) : rq.TicketNumber);
                            }

                            var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == DV.Id).OrderBy(x => x.Index).FirstOrDefault();
                            if (nv == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                            }
                            else
                            {
                                var obj = new Q_DailyRequire();
                                obj.TicketNumber = (socu + 1);
                                obj.ServiceId = DV.Id;
                                obj.BusinessId = null;
                                obj.PrintTime = DateTime.Now;
                                obj.ServeTimeAllow = DV.TimeProcess.TimeOfDay;
                                obj.CustomerName = Name;
                                obj.CustomerDOB = DOB;
                                obj.CustomerAddress = Address;
                                obj.MaBenhNhan = MaBenhNhan;
                                obj.MaPhongKham = MaPhongKham;
                                obj.STT_PhongKham = SttPhongKham;
                                obj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                                var detail = new Q_DailyRequire_Detail();
                                detail.Q_DailyRequire = obj;
                                detail.MajorId = (nv != null ? nv.MajorId : 1);
                                detail.StatusId = (int)eStatus.CHOXL;
                                obj.Q_DailyRequire_Detail.Add(detail);
                                db.Q_DailyRequire.Add(obj);

                                var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == DV.Id && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                                db.SaveChanges();
                                rs.IsSuccess = true;
                                rs.Data = socu;
                                rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                            }
                        }
                        else
                        {
                            rs.IsSuccess = false;
                            rs.Errors.Add(new Error() { MemberName = "Lỗi", Message = "Không tìm thấy cấu hình kiểu in phiếu trong hệ thống. Vui lòng kiểm tra lại." });
                        }
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi", Message = "Không tìm thấy mã phòng khám (" + MaPhongKham + ") trong hệ thống. Vui lòng kiểm tra lại." });
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "Lỗi Exception", Message = "Lỗi thực thi. " + ex.Message });
            }
            return rs;
        }

        public ResponseBase API_UpdateTicketInfo(string connectString,
            string Name,
            string MaBenhNhan,
            string SttPhongKham
            )
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var tket = db.Q_DailyRequire.FirstOrDefault(x => x.STT_PhongKham == SttPhongKham);
                    if (tket == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi", Message = "Lỗi: Không tìm thấy phiếu (" + SttPhongKham + ") trong hệ thống. Vui lòng kiểm tra lại." });
                    }
                    else
                    {
                        tket.CustomerName = Name;
                        tket.MaBenhNhan = MaBenhNhan;
                        db.Entry<Q_DailyRequire>(tket).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "Lỗi Exception", Message = "Lỗi thực thi. " + ex.Message });
            }
            return rs;
        }

        public ResponseBase CheckUserFree(string connectString, int majorId, int serviceId, int withoutTicketNumber, int checkFollowMajorOrder)
        {
            var rs = new ResponseBase();
            rs.Data = 0;
            using (var db = new QMSSystemEntities(connectString))
            {
                var currentLogins = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN select x.UserId).ToArray();
                //lay user co nghiep vu và dang login
                var userIds = (from x in db.Q_UserMajor
                               where
                               !x.IsDeleted && !x.Q_Major.IsDeleted &&
                               !x.Q_User.IsDeleted && x.MajorId == majorId &&
                               currentLogins.Contains(x.UserId)
                               select x.UserId).ToList();

                var requests = db.Q_RequestTicket.Where(x => !x.IsDeleted && userIds.Contains(x.UserId)).OrderBy(x => x.CreatedAt).ToList();
                if (requests.Count > 0)
                {
                    Q_RequestTicket foundRequest = null;
                    if (checkFollowMajorOrder == 1)
                    {
                        var userIdFilters = requests.Select(x => x.UserId).ToArray();
                        //ktra theo thứ tu nghiep vu
                        var userMajors = db.Q_UserMajor.Where(x => userIdFilters.Contains(x.UserId) && x.MajorId == majorId).OrderBy(x => x.Index).ToArray();
                        if (userMajors.Length > 0)
                        {
                            var orders = userMajors.Select(x => x.Index).Distinct().OrderBy(x => x).ToList();
                            for (int i = 0; i < orders.Count; i++)
                            {
                                var userWithOrder = userMajors.Where(x => x.Index == orders[i]).Select(x => x.UserId).Distinct().ToList();
                                var check = requests.Where(x => userWithOrder.Contains(x.UserId)).OrderBy(x => x.CreatedAt).FirstOrDefault();
                                if (check != null)
                                {
                                    rs.Data = check.UserId;
                                    foundRequest = check;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        rs.Data = requests[0].UserId;
                        foundRequest = requests[0];
                    }
                    foundRequest.IsDeleted = true;
                    db.Entry<Q_RequestTicket>(foundRequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return rs;
            }
        }

        public void UpdateCounterRepeatServeOver(string connectString, List<int> dailyDetailIds)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var details = from x in db.Q_DailyRequire_Detail where dailyDetailIds.Contains(x.Id) select x;
                if (details != null && details.Count() > 0)
                {
                    foreach (var item in details)
                    {
                        item.ServeOverCounter = item.ServeOverCounter + 1;
                        db.Entry<Q_DailyRequire_Detail>(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                }
            }
        }

        public TicketInfo UpdateServeTime(string connectString, int userId, int minutes, bool replaceByNew)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var ticketDetail = (from x in db.Q_DailyRequire_Detail where x.UserId.HasValue && x.UserId == userId && x.StatusId == (int)eStatus.DAGXL select x).FirstOrDefault();
                if (ticketDetail != null)
                {
                    var ticket = (from x in db.Q_DailyRequire where x.Id == ticketDetail.DailyRequireId select x).FirstOrDefault();
                    if (ticket != null)
                    {
                        if (replaceByNew)
                            ticket.ServeTimeAllow = new TimeSpan(0, minutes, 0);
                        else
                            ticket.ServeTimeAllow = ticket.ServeTimeAllow.Add(new TimeSpan(0, minutes, 0));
                        db.Entry(ticket).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return new TicketInfo()
                        {
                            RequireDetailId = ticket.Id,
                            TimeServeAllow = ticket.ServeTimeAllow,
                            TicketNumber = ticket.TicketNumber
                        };
                    }
                }
                return null;
            }
        }

        public ResponseBaseModel InsertAndCallEmptyTicket(string connectString, int equipCode)
        {
            var rs = new ResponseBaseModel();
            rs.IsSuccess = false;
            using (var db = new QMSSystemEntities(connectString))
            {
                var login = db.Q_Login.OrderByDescending(x => x.Date).FirstOrDefault(x => x.EquipCode == equipCode && x.StatusId == (int)eStatus.LOGIN);
                if (login != null)
                {
                    var userMajor = db.Q_UserMajor.Where(x => !x.IsDeleted && x.UserId == login.UserId).OrderBy(x => x.Index).FirstOrDefault();
                    if (userMajor != null)
                    {
                        var service = db.Q_ServiceStep.Where(x => !x.IsDeleted && x.MajorId == userMajor.MajorId).OrderBy(x => x.Index).FirstOrDefault();
                        var newTicket = new Q_DailyRequire() { TicketNumber = -1, ServiceId = service.ServiceId, PrintTime = DateTime.Now, ServeTimeAllow = service.Q_Service.TimeProcess.TimeOfDay };
                        var detail = new Q_DailyRequire_Detail() { Q_DailyRequire = newTicket, EquipCode = equipCode, MajorId = userMajor.MajorId, UserId = login.UserId, ProcessTime = newTicket.PrintTime, StatusId = (int)eStatus.DAGXL };
                        newTicket.Q_DailyRequire_Detail = new List<Q_DailyRequire_Detail>();
                        newTicket.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(newTicket);
                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Data = newTicket.TicketNumber;
                        rs.Data_1 = newTicket.ServeTimeAllow;
                        rs.Data_2 = newTicket.PrintTime.TimeOfDay;

                        var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                        if (equip != null)
                            db.Database.ExecuteSqlCommand(@"update Q_Counter set LastCall=" + newTicket.TicketNumber + ",CurrentNumber=" + newTicket.TicketNumber + " , isrunning=0 where Id=" + equip.CounterId);
                        rs.Data_3 = new TicketInfo()
                        {
                            RequireDetailId = detail.Id,
                            RequireId = detail.DailyRequireId,
                            StartTime = detail.ProcessTime.Value.TimeOfDay,
                            TimeServeAllow = detail.Q_DailyRequire.ServeTimeAllow,
                            TicketNumber = newTicket.TicketNumber,
                            QMSNumber = newTicket.TicketNumber,
                            ThirdNumber = "-1",
                            CounterId = equip.CounterId,
                            PrintTime = detail.Q_DailyRequire.PrintTime,
                            EquipCode = equipCode
                        };
                    }
                }
                return rs;
            }
        }

        public void RemoveRegisterAutocall(string connectString, int userId)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                db.Database.ExecuteSqlCommand("update Q_RequestTicket set isdeleted = 1 where userid = " + userId);
                db.SaveChanges();
            }
        }

        public void UpdateCounterDayInfo(string connectString, TicketInfo info)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var found = db.Q_CounterDayInfo.FirstOrDefault(x => x.CounterId == info.CounterId);
                DateTime startDate = new DateTime(2020, 1, 1),
                    serveDate = new DateTime(2020, 1, 1);
                if (found != null)
                {
                    found.STT = info.TicketNumber;
                    found.STT_QMS = info.QMSNumber;
                    found.STT_3 = info.ThirdNumber == null ? "" : info.ThirdNumber;
                    found.PrintTime = info.PrintTime;
                    found.StartTime = startDate.Add(info.StartTime);
                    found.ServeTime = serveDate.Add(info.TimeServeAllow);
                    found.StatusSTT = (int)eStatus.DAGXL;
                    found.EquipCode = info.EquipCode;
                    db.SaveChanges();
                }
            }
        }

        public List<ModelSelectItem> GetAllTicketNumbersInDay(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {

                var STTs = (from x in db.Q_DailyRequire select new ModelSelectItem() { Id = x.TicketNumber, Code = x.STT_PhongKham, Record = x.Id }).ToList();
                if (STTs != null && STTs.Count > 0)
                {
                    var wd = db.Q_DailyRequire_WorkDetail.Where(x => !x.IsDeleted).Select(x => new { DailyRequireId = x.DailyRequireId, WorkTypeId = x.Q_WorkDetail.WorkTypeId, WorkTypeName = x.Q_WorkDetail.Q_WorkType.Name }).ToList();
                    for (int i = 0; i < STTs.Count; i++)
                    {
                        var found = wd.FirstOrDefault(x => x.DailyRequireId == STTs[i].Record);
                        if (found != null)
                        {
                            STTs[i].Data = found.WorkTypeId;
                            STTs[i].Name = found.WorkTypeName;
                        }
                        else
                        {
                            STTs[i].Data = 1;
                            STTs[i].Name = "";
                        }
                    }
                }
                return STTs;
            }
        }

        public Q_DailyRequire Get(string connectString, string sttPhongKham)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                return db.Q_DailyRequire.FirstOrDefault(x => x.STT_PhongKham.Trim().ToUpper() == sttPhongKham.Trim().ToUpper());
            }
        }
    }
}
