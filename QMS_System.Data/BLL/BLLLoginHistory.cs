using GPRO.Ultilities;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLLoginHistory
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLLoginHistory _Instance;  //volatile =>  tranh dung thread
        public static BLLLoginHistory Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLoginHistory();

                return _Instance;
            }
        }
        private BLLLoginHistory() { }
        #endregion
        public List<LoginHistoryModel> GetsForMain()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN).Select(x => new LoginHistoryModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    EquipCode = x.EquipCode,
                    Date = x.Date,
                    StatusId = x.StatusId,
                    LogoutTime = x.LogoutTime
                }).ToList();
            }
        }

        public List<LoginHistoryModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_LoginHistory.Select(x => new LoginHistoryModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    EquipCode = x.EquipCode,
                    Date = x.Date,
                    StatusId = x.StatusId,
                    LogoutTime = x.LogoutTime
                }).ToList();
            }
        }

        public List<LoginHistoryModel> Gets_()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_LoginHistory.Where(x => x.StatusId == (int)eStatus.LOGIN).Select(x => new LoginHistoryModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    EquipCode = x.EquipCode,
                    Date = x.Date,
                    StatusId = x.StatusId,
                    LogoutTime = x.LogoutTime
                }).ToList();
            }
        }

        public void ResetDailyLoginInfor(DateTime date, string IsCopyToNewDay)
        {
            using (db = new QMSSystemEntities())
            {
                var yes = db.Q_Login.ToList();
                if (yes.Count > 0)
                {
                    List<int> code = new List<int>();
                    Q_LoginHistory obj;
                    Q_Login login;
                    var now = DateTime.Now;
                    if (IsCopyToNewDay == "1")
                    {
                        for (int i = 0; i < yes.Count; i++)
                        {
                            obj = new Q_LoginHistory();
                            Parse.CopyObject(yes[i], ref obj);
                            db.Q_LoginHistory.Add(obj);

                            if (!code.Contains(yes[i].EquipCode))
                            {
                                login = new Q_Login();
                                login.Date = now;
                                login.EquipCode = yes[i].EquipCode;
                                login.StatusId = (int)eStatus.LOGIN;
                                login.UserId = yes[i].UserId;
                                db.Q_Login.Add(login);
                                code.Add(yes[i].EquipCode);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < yes.Count; i++)
                        {
                            obj = new Q_LoginHistory();
                            Parse.CopyObject(yes[i], ref obj);
                            db.Q_LoginHistory.Add(obj);
                        }
                    } 
                    db.Q_Login.RemoveRange(yes);
                    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Q_Login', RESEED, 0); ");
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Get infomation for home form
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<HomeModel> GetForHome(DateTime date)
        {
            using (db = new QMSSystemEntities())
            {
                var users = db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN).Select(x => new HomeModel()
                   {
                       UserId = x.UserId,
                       User = x.Q_User.UserName,
                       //  Counter = x.Q_Equipment.Q_Counter.Name,
                       LoginTime = x.Date,
                       EquipCode = x.EquipCode
                   }).ToList();
                if (users.Count > 0)
                {
                    var codes = users.Select(x => x.EquipCode).Distinct().ToArray();
                    var counters = db.Q_Equipment.Where(x => !x.IsDeleted && codes.Contains(x.Code) && x.EquipTypeId == (int)eEquipType.Counter).Select(x => new EquipmentModel() { Id = x.Id, Name = x.Q_Counter.Name, Code = x.Code }).ToList();

                    var daily = db.Q_DailyRequire_Detail.Where(x => x.StatusId != (int)eStatus.CHOXL);
                    var usermajors = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted).ToList();
                    EquipmentModel find;
                    foreach (var item in users)
                    {
                        var majorIds = usermajors.Where(x => x.UserId == item.UserId).Select(x => x.MajorId).ToList();
                        item.TotalDone = daily.Count(x => x.StatusId == (int)eStatus.HOTAT && x.UserId == item.UserId);
                        item.TotalWating = db.Q_DailyRequire_Detail.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId));
                        var current = daily.Where(x => x.StatusId == (int)eStatus.DAGXL && x.UserId == item.UserId).OrderBy(x => x.ProcessTime).FirstOrDefault();
                        if (current != null)
                        {
                            item.CurrentTicket = current.Q_DailyRequire.TicketNumber;
                            item.CommingTime = current.ProcessTime;
                        }
                        find = counters.FirstOrDefault(x => x.Code == item.EquipCode);
                        item.Counter = (find != null ? find.Name : "");
                    }
                }
                return users;

                //var users = db.Q_LoginHistory.Where(x => x.StatusId == (int)eStatus.LOGIN).Select(x => new HomeModel()
                //   {
                //       UserId = x.UserId,
                //       User = x.Q_User.UserName,
                //       CurrentTicket = x.Ticket,
                //       CommingTime = x.Start,
                //       TotalDone = x.Done,
                //       TotalWating = x.Waitting, 
                //       LoginTime = x.Date,
                //       EquipCode = x.EquipCode
                //   }).ToList();

                //if (users.Count > 0)
                //{
                //    var codes = users.Select(x => x.EquipCode).Distinct().ToArray();
                //    var counters = db.Q_Equipment.Where(x => !x.IsDeleted && codes.Contains(x.Code) && x.EquipTypeId == (int)eEquipType.Counter).Select(x => new EquipmentModel() { Id = x.Id, Name = x.Q_Counter.Name, Code = x.Code }).ToList();
                //    EquipmentModel find;
                //    foreach (var item in users)
                //    { 
                //        find = counters.FirstOrDefault(x => x.Code == item.EquipCode);
                //        item.Counter = (find != null ? find.Name : "");
                //    }
                //}
                //return users;
            }
        }

        public HomeModel GetForHome(int userId, int equipCode, DateTime date)
        {
            var model = new HomeModel();
            using (db = new QMSSystemEntities())
            {
                //var user = db.Q_LoginHistory.FirstOrDefault(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day && x.StatusId == (int)eStatus.LOGIN && x.UserId == userId && x.EquipCode == equipCode);
                //if (user != null)
                //{
                //    var rq = db.Q_DailyRequire.Where(x => x.PrintTime.Year == date.Year && x.PrintTime.Month == date.Month && x.PrintTime.Day == date.Day).ToList();
                //    var dailyDetails = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day);

                //    var majorIds = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).Select(x => x.MajorId).ToList();
                //    model.TotalDone = dailyDetails.Count(x => x.StatusId == (int)eStatus.HOTAT && x.UserId == userId);
                //    model.TotalWating = dailyDetails.Count(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day && x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId));
                //    var current = dailyDetails.Where(x => x.StatusId == (int)eStatus.DAGXL && x.UserId == userId).OrderBy(x => x.ProcessTime).FirstOrDefault();
                //    if (current != null)
                //    {
                //        model.CurrentTicket = current.Q_DailyRequire.TicketNumber;
                //        model.CommingTime = current.ProcessTime;
                //    }

                //    if (rq.Count() > 0)
                //    {
                //        foreach (var item in rq)
                //        {
                //            var dt = dailyDetails.Where(x => x.DailyRequireId == item.Id).ToList();
                //            if (dt != null && dt.Count() == 1)
                //            {
                //                var first = dt.FirstOrDefault();
                //                if (first.StatusId == (int)eStatus.CHOXL && majorIds.Contains(first.MajorId))
                //                    model.CounterWaitingTickets += item.TicketNumber + " ";
                //                else if (first.StatusId == (int)eStatus.CHOXL && !majorIds.Contains(first.MajorId))
                //                    model.AllWaitingTickets += item.TicketNumber + " ";
                //            }
                //            else if (dt != null && dt.Count() > 1)
                //                if (dt.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId)) > 0)
                //                    model.CounterWaitingTickets += item.TicketNumber + " ";
                //        }
                //    }
                //    model.Counter = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode).Q_Counter.Name;
                //}
                //return model;


                var user = db.Q_Login.FirstOrDefault(x => x.StatusId == (int)eStatus.LOGIN && x.UserId == userId && x.EquipCode == equipCode);
                if (user != null)
                {
                    var rq = db.Q_DailyRequire.ToList();
                    var dailyDetails = db.Q_DailyRequire_Detail;

                    var majorIds = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).Select(x => x.MajorId).ToList();
                    model.TotalDone = dailyDetails.Count(x => x.StatusId == (int)eStatus.HOTAT && x.UserId == userId);
                    model.TotalWating = dailyDetails.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId));
                    var current = dailyDetails.Where(x => x.StatusId == (int)eStatus.DAGXL && x.UserId == userId).OrderBy(x => x.ProcessTime).FirstOrDefault();
                    if (current != null)
                    {
                        model.CurrentTicket = current.Q_DailyRequire.TicketNumber;
                        model.CommingTime = current.ProcessTime;
                    }

                    if (rq.Count() > 0)
                    {
                        foreach (var item in rq)
                        {
                            var dt = dailyDetails.Where(x => x.DailyRequireId == item.Id).ToList();
                            if (dt != null && dt.Count() == 1)
                            {
                                var first = dt.FirstOrDefault();
                                if (first.StatusId == (int)eStatus.CHOXL && majorIds.Contains(first.MajorId))
                                    model.CounterWaitingTickets += item.TicketNumber + " ";
                                else if (first.StatusId == (int)eStatus.CHOXL && !majorIds.Contains(first.MajorId))
                                    model.AllWaitingTickets += item.TicketNumber + " ";
                            }
                            else if (dt != null && dt.Count() > 1)
                                if (dt.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId)) > 0)
                                    model.CounterWaitingTickets += item.TicketNumber + " ";
                        }
                    }
                    model.Counter = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode).Q_Counter.Name;
                }
                return model;
            }
        }

        //public void LogOutAllUser()
        //{
        //    try
        //    {
        //        using (db = new QMSSystemEntities())
        //        {
        //            var lg = db.Q_LoginHistory.Where(x => x.StatusId == (int)eStatus.LOGIN).ToList();
        //            if (lg.Count > 0)
        //            {
        //                var now = DateTime.Now;
        //                for (int i = 0; i < lg.Count; i++)
        //                {
        //                    lg[i].StatusId = (int)eStatus.LOGOUT;
        //                    lg[i].LogoutTime = now;
        //                }
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //}

        /// <summary>
        /// Reset user infor for new day
        /// </summary>
        //public void ResetUsers()
        //{
        //    try
        //    {
        //        using (db = new QMSSystemEntities())
        //        {
        //            var lg = db.Q_LoginHistory.ToList();
        //            if (lg.Count > 0)
        //            {
        //                var now = DateTime.Now;
        //                int[] userIds = lg.Select(x => x.UserId).Distinct().ToArray();
        //                Q_LoginHistory found;
        //                for (int i = 0; i < userIds.Length; i++)
        //                {
        //                    found = lg.Where(x => x.UserId == userIds[i]).OrderByDescending(x => x.Date).FirstOrDefault();
        //                    found.StatusId = (int)eStatus.LOGIN;
        //                    found.Date = now;
        //                    found.Ticket = 0;
        //                    found.Done = 0;
        //                    found.Waitting = 0;
        //                    found.Start = null;
        //                    found.LogoutTime = null;
        //                }

        //                //for (int i = 0; i < lg.Count; i++)
        //                //{
        //                //    lg[i].StatusId = (int)eStatus.LOGIN;
        //                //    lg[i].Date = now;
        //                //    lg[i].Ticket = 0;
        //                //    lg[i].Done = 0;
        //                //    lg[i].Waitting = 0;
        //                //    lg[i].Start = null;
        //                //    lg[i].LogoutTime = null;
        //                //}
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //}


        public bool Login(Q_LoginHistory obj)
        {
            using (db = new QMSSystemEntities())
            {
                db.Q_LoginHistory.Add(obj);
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Counter soft
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="equipCode"></param>
        /// <returns></returns>
        public ResponseBaseModel Login(string userName, string password, int equipCode)
        {
            ResponseBaseModel rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities())
            {
                var user = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()) && x.Password.Trim().ToUpper().Equals(password.Trim().ToUpper()));
                if (user != null)
                {
                    var equipObj = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode && x.EquipTypeId == (int)eEquipType.Counter);
                    if (equipObj != null)
                    {
                        Login login = null;
                        var now = DateTime.Now;
                        //var obj = db.Q_LoginHistory.FirstOrDefault(x => x.StatusId == (int)eStatus.LOGIN && (x.UserId == user.Id || x.EquipCode == equipCode));
                        //if (obj == null)
                        //{
                        //    // chua log
                        //    obj = new Q_LoginHistory() { UserId = user.Id, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = now };
                        //    db.Q_LoginHistory.Add(obj);
                        //    login = new Login() { UserName = user.Name, EquipCode = equipCode, LoginTime = now.ToString("dd/MM/yyyy HH:mm"), UserId = user.Id, CounterId = equipObj.CounterId, CounterName = equipObj.Q_Counter.Name };
                        //}
                        //else
                        //{
                        //    if ((obj.EquipCode != equipCode && obj.UserId == user.Id) || (obj.EquipCode == equipCode && obj.UserId != user.Id))
                        //    {
                        //        // login on other counter
                        //        obj.LogoutTime = now;
                        //        obj.StatusId = (int)eStatus.LOGOUT;

                        //        var newobj = new Q_LoginHistory() { UserId = user.Id, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = now };
                        //        db.Q_LoginHistory.Add(newobj);
                        //        login = new Login() { UserName = user.Name, EquipCode = equipCode, LoginTime = now.ToString("dd/MM/yyyy HH:mm"), UserId = user.Id, CounterId = equipObj.CounterId, CounterName = equipObj.Q_Counter.Name };
                        //    }
                        //    else if (obj.EquipCode == equipCode && obj.UserId == user.Id)
                        //        login = new Login() { UserName = user.Name, EquipCode = equipCode, LoginTime = obj.Date.ToString("dd/MM/yyyy HH:mm"), UserId = user.Id, CounterId = equipObj.CounterId, CounterName = equipObj.Q_Counter.Name };
                        //}



                        var obj = db.Q_Login.FirstOrDefault(x => x.UserId == user.Id && x.EquipCode == equipCode);
                        if (obj == null)
                        {
                            obj = new Q_Login() { UserId = user.Id, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = now };
                            db.Q_Login.Add(obj);
                            login = new Login() { UserName = user.Name, EquipCode = equipCode, LoginTime = now.ToString("dd/MM/yyyy HH:mm"), UserId = user.Id, CounterId = equipObj.CounterId, CounterName = equipObj.Q_Counter.Name };
                        }
                        else
                        {
                            obj.LogoutTime = null;
                            obj.StatusId = (int)eStatus.LOGIN;
                            login = new Login() { UserName = user.Name, EquipCode = equipCode, LoginTime = now.ToString("dd/MM/yyyy HH:mm"), UserId = user.Id, CounterId = equipObj.CounterId, CounterName = equipObj.Q_Counter.Name };
                        }
                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Data = login;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.sms = "Không tìm thấy thông tin thiết bị. Vui lòng kiểm tra cấu hình";
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Không tìm thấy thông tin tài khoản. Vui lòng kiểm tra lại";
                }
                return rs;
            }
        }

        public int GetUserId(int equipCode, DateTime date)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Login.FirstOrDefault(x =>x.EquipCode == equipCode && x.StatusId == (int)eStatus.LOGIN);
                return (obj != null ? obj.UserId : 0);
            }
        }

        /// <summary>
        /// process Login logout of counter
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool CounterLoginLogOut(int userId, int equipCode, DateTime date)
        {
            try
            {
                using (db = new QMSSystemEntities())
                {
                    //var obj = db.Q_LoginHistory.FirstOrDefault(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day && x.StatusId == (int)eStatus.LOGIN && (x.UserId == userId || x.EquipCode == equipCode));
                    //if (obj == null)
                    //{
                    //    // chua log
                    //    obj = new Q_LoginHistory() { UserId = userId, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = date };
                    //    db.Q_LoginHistory.Add(obj);
                    //}
                    //else
                    //{
                    //    if ((obj.EquipCode != equipCode && obj.UserId == userId) || (obj.EquipCode == equipCode && obj.UserId != userId))
                    //    {
                    //        // login on other counter
                    //        obj.LogoutTime = date;
                    //        obj.StatusId = (int)eStatus.LOGOUT;

                    //        var newobj = new Q_LoginHistory() { UserId = userId, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = date };
                    //        db.Q_LoginHistory.Add(newobj);
                    //    }
                    //    else if (obj.EquipCode == equipCode && obj.UserId == userId)
                    //    {
                    //        // allready login => logout
                    //        obj.LogoutTime = date;
                    //        obj.StatusId = (int)eStatus.LOGOUT;

                    //        var last = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    //        if (last != null && last.Count() > 0)
                    //        {
                    //            foreach (var item in last)
                    //            {
                    //                item.StatusId = (int)eStatus.HOTAT;
                    //                item.EndProcessTime = DateTime.Now;
                    //            }
                    //        }
                    //    }
                    //}
                    //db.SaveChanges();
                    //return true;


                    var obj = db.Q_Login.FirstOrDefault(x => x.StatusId == (int)eStatus.LOGIN && (x.UserId == userId || x.EquipCode == equipCode));
                    if (obj == null)
                    {
                        // chua log
                        obj = new Q_Login() { UserId = userId, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = date };
                        db.Q_Login.Add(obj);
                    }
                    else
                    {
                        if ((obj.EquipCode != equipCode && obj.UserId == userId) || (obj.EquipCode == equipCode && obj.UserId != userId))
                        {
                            // login on other counter
                            obj.LogoutTime = date;
                            obj.StatusId = (int)eStatus.LOGOUT;

                            var newobj = new Q_Login() { UserId = userId, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = date };
                            db.Q_Login.Add(newobj);
                        }
                        else if (obj.EquipCode == equipCode && obj.UserId == userId)
                        {
                            // allready login => logout
                            obj.LogoutTime = date;
                            obj.StatusId = (int)eStatus.LOGOUT;

                            var last = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.PrintTime.Year == date.Year && x.Q_DailyRequire.PrintTime.Month == date.Month && x.Q_DailyRequire.PrintTime.Day == date.Day && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                            if (last != null && last.Count() > 0)
                            {
                                foreach (var item in last)
                                {
                                    item.StatusId = (int)eStatus.HOTAT;
                                    item.EndProcessTime = DateTime.Now;
                                }
                            }
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
 
}
