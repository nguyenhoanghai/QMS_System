using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public List<LoginHistoryModel> GetsForMain(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var logins = db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN && !x.Q_User.IsDeleted).Select(x => new LoginHistoryModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserName = x.Q_User.UserName,
                    EquipCode = x.EquipCode,
                    Date = x.Date,
                    StatusId = x.StatusId,
                    LogoutTime = x.LogoutTime
                }).ToList();
                var equips = (from x in db.Q_Equipment where !x.IsDeleted select x).ToList();
                foreach (var item in logins)
                {
                    var found = equips.FirstOrDefault(x => x.Code == item.EquipCode);
                    try
                    {
                        if (found != null && found.Q_Counter != null)
                        {
                            item.CounterName = found.Q_Counter.Name;
                            item.CounterCode = found.CounterId;
                        }
                    }
                    catch (Exception ex)
                    {
                        item.CounterName = "";
                        item.CounterCode = found.CounterId;
                    }
                }
                return logins;
            }
        }

        public List<LoginHistoryModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
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

        public List<LoginHistoryModel> Gets_(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
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

        public void ResetDailyLoginInfor(string connectString, DateTime date, string IsCopyToNewDay)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var yes = db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN).OrderBy(x => x.UserId).ToList();
                    if (yes.Count > 0)
                    {
                        db.Database.ExecuteSqlCommand("delete Q_Login ");
                        db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Q_Login', RESEED, 0); ");
                        db.SaveChanges();
                        List<int> code = new List<int>();
                        //Q_LoginHistory obj;
                        Q_Login login;
                        var now = DateTime.Now;
                        if (IsCopyToNewDay == "1")
                        {
                            for (int i = 0; i < yes.Count; i++)
                            {
                                //obj = new Q_LoginHistory();
                                //Parse.CopyObject(yes[i], ref obj);
                                //db.Q_LoginHistory.Add(obj);

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
                            //for (int i = 0; i < yes.Count; i++)
                            //{
                            //    obj = new Q_LoginHistory();
                            //    Parse.CopyObject(yes[i], ref obj);
                            //    db.Q_LoginHistory.Add(obj);
                            //}
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception ex) { }
            }
        }

        /// <summary>
        /// Get infomation for home form
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<HomeModel> GetForHome(string connectString, DateTime date, int useWithThridPattern)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var users = db.Q_Login.Where(x => !x.Q_User.IsDeleted && x.StatusId == (int)eStatus.LOGIN).Select(x => new HomeModel()
                {
                    UserId = x.UserId,
                    User = x.Q_User.UserName,
                    //  Counter = x.Q_Equipment.Q_Counter.Name,
                    LoginTime = x.Date,
                    EquipCode = x.EquipCode
                }).OrderBy(x => x.UserId).ToList();
                if (users.Count > 0)
                {
                    var codes = users.Select(x => x.EquipCode).Distinct().ToArray();
                    var counters = db.Q_Equipment.Where(x => !x.IsDeleted && codes.Contains(x.Code) && x.EquipTypeId == (int)eEquipType.Counter).Select(x => new EquipmentModel() { Id = x.Id, Name = x.Q_Counter.Name, Code = x.Code }).ToList();

                    var daily = db.Q_DailyRequire_Detail.Where(x => x.StatusId != (int)eStatus.CHOXL);
                    var usermajors = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted).ToList();
                    EquipmentModel find;
                    var registers = db.Q_RequestTicket.Where(x => !x.IsDeleted);
                    foreach (var item in users)
                    {
                        var majorIds = usermajors.Where(x => x.UserId == item.UserId).Select(x => x.MajorId).ToList();
                        item.TotalDone = daily.Count(x => x.StatusId == (int)eStatus.HOTAT && x.UserId == item.UserId);
                        item.TotalWating = db.Q_DailyRequire_Detail.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId));
                        var current = daily.Where(x => x.StatusId == (int)eStatus.DAGXL && x.UserId == item.UserId).OrderByDescending(x => x.ProcessTime).FirstOrDefault();
                        if (current != null)
                        {
                            if (useWithThridPattern == 0)
                            {
                                item.CurrentTicket = current.Q_DailyRequire != null ? current.Q_DailyRequire.TicketNumber : 0;
                            }
                            else
                            {
                                if (current.Q_DailyRequire.STT_PhongKham != null)
                                    item.CurrentTicket = int.Parse(current.Q_DailyRequire.STT_PhongKham);
                            }

                            item.CommingTime = current.ProcessTime;
                        }
                        find = counters.FirstOrDefault(x => x.Code == item.EquipCode);
                        item.Counter = (find != null ? find.Name : "");
                        var found = registers.Where(x => x.UserId == item.UserId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        if (found != null)
                            item.TimeRegister = found.CreatedAt;
                    }
                }
                return users.OrderBy(x => x.UserId).ToList();
            }
        }

        /// <summary>
        /// Get infomation for home form
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<HomeModel> GetForHome_ver3(string connectString, DateTime date, int useWithThridPattern)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var users = (from x in db.Q_Login
                             where !x.Q_User.IsDeleted && x.StatusId == (int)eStatus.LOGIN
                             orderby x.UserId
                             select new HomeModel()
                             {
                                 UserId = x.UserId,
                                 User = x.Q_User.UserName,
                                 LoginTime = x.Date,
                                 EquipCode = x.EquipCode
                             }).ToList();
                if (users.Count > 0)
                {
                    var daily = (from x in db.Q_CounterDayInfo
                                 select new
                                 {
                                     CounterId = x.CounterId,
                                     CounterName = x.Q_Counter.Name,
                                     STT = x.STT,
                                     Starttime = x.StartTime,
                                     EquipCode = x.EquipCode
                                 }).ToList();

                    var usermajors = (from x in db.Q_UserMajor
                                      where !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted
                                      select new
                                      {
                                          UserId = x.UserId,
                                          MajorId = x.MajorId
                                      }).ToList();
                    var dayDetails = (from x in db.Q_DailyRequire_Detail
                                      select new
                                      {
                                          StatusId = x.StatusId,
                                          UserId = x.UserId,
                                          MajorId = x.MajorId,
                                          ProcessTime = x.ProcessTime
                                      }).ToList();
                    var registers = (from x in db.Q_RequestTicket
                                     where !x.IsDeleted
                                     select new
                                     {
                                         UserId = x.UserId,
                                         CreatedAt = x.CreatedAt
                                     }).ToList();
                    foreach (var item in users)
                    {
                        var majorIds = usermajors.Where(x => x.UserId == item.UserId).Select(x => x.MajorId).ToList();
                        item.TotalDone = dayDetails.Count(x => x.StatusId == (int)eStatus.HOTAT && x.UserId == item.UserId);
                        item.TotalWating = dayDetails.Count(x => x.StatusId == (int)eStatus.CHOXL && majorIds.Contains(x.MajorId));

                        var findCounter = daily.FirstOrDefault(x => x.EquipCode == item.EquipCode);
                        if (findCounter != null)
                        {
                            item.Counter = findCounter.CounterName;
                            item.CurrentTicket = findCounter.STT;
                            item.CommingTime = findCounter.Starttime;
                        }
                        var found = registers.Where(x => x.UserId == item.UserId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        if (found != null)
                            item.TimeRegister = found.CreatedAt;
                    }
                }
                return users.OrderBy(x => x.UserId).ToList();
            }
        }


        /// <summary>
        /// counter soft
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        /// <param name="useWithThridPattern"></param>
        /// <returns></returns>
        public HomeModel GetForHome(string connectString, int userId, int equipCode, DateTime date, int useWithThridPattern)
        {
            var model = new HomeModel();
            using (db = new QMSSystemEntities(connectString))
            {
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
                                if (first.StatusId == (int)eStatus.CHOXL && majorIds.Contains(first.MajorId))
                                {
                                    model.CounterWaitingTickets += ((useWithThridPattern == 0 ? item.TicketNumber.ToString() : (item.STT_PhongKham == null ? " " : item.STT_PhongKham)) + " ");
                                    diemDoiTaiQuay++;
                                }
                                else if (first.StatusId == (int)eStatus.CHOXL && !majorIds.Contains(first.MajorId))
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

        public HomeModel GetForHome(SqlConnection sqlConnection, int userId, int equipCode, DateTime date, int useWithThridPattern)
        {
            var model = new HomeModel();
            string query = "Select * from Q_Login where StatusId=" + (int)eStatus.LOGIN + " and UserId=" + userId + " and EquipCode=" + equipCode;
            var da = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                int intType = 0;
                dataTable.Clear();
                List<int> majorIds = new List<int>();
                query = @"select um.MajorId from Q_UserMajor um, Q_User u, Q_Major m where u.IsDeleted = 0 and um.IsDeleted = 0 and m.IsDeleted = 0 and um.MajorId = m.Id and um.UserId = u.Id and UM.UserId =" + userId;
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        intType = 0;
                        int.TryParse(row["MajorId"].ToString(), out intType);
                        majorIds.Add(intType);
                    }
                }

                dataTable.Clear();
                model.TotalDone = 0;
                query = "select COUNT(Id) as sl from Q_DailyRequire_Detail where StatusId=" + (int)eStatus.HOTAT + " and UserId=" + userId;
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                    model.TotalDone = Convert.ToInt32(dataTable.Rows[0]["sl"].ToString());

                dataTable.Clear();
                model.TotalWating = 0;
                model.CounterWaitingTickets = "";
                query = @"select r.TicketNumber as stt, r.STT_PhongKham as stt_pk, StatusId from Q_DailyRequire_Detail d, Q_DailyRequire r where d.DailyRequireId = r.Id and (StatusId =" + (int)eStatus.CHOXL + " or StatusId =" + (int)eStatus.QUALUOT + ") and d.MajorId in (" + string.Join(",", majorIds) + ")";
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string _stt = "";
                        if (useWithThridPattern == 0)
                            _stt = row["stt"].ToString() + " ";
                        else if (!string.IsNullOrEmpty(row["stt_pk"].ToString()))
                            _stt = row["stt_pk"].ToString() + " ";

                        if ((int)row["StatusId"] == (int)eStatus.CHOXL)
                            model.CounterWaitingTickets += row["stt"].ToString() + " ";
                        else if ((int)row["StatusId"] == (int)eStatus.QUALUOT)
                            model.CounterWaitingTickets_qualuot += row["stt"].ToString() + " ";
                    }
                    model.TotalWating = dataTable.Rows.Count;
                }

                dataTable.Clear();
                query = "select top 1 r.TicketNumber,r.STT_PhongKham,d.ProcessTime  from Q_DailyRequire_Detail d , Q_DailyRequire r where r.Id = d.DailyRequireId and d.UserId = " + userId + " and d.StatusId = " + (int)eStatus.DAGXL + " order by d.ProcessTime";
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    intType = 0;
                    if (useWithThridPattern == 0)
                        int.TryParse(dataTable.Rows[0]["TicketNumber"].ToString(), out intType);
                    else if (!string.IsNullOrEmpty(dataTable.Rows[0]["STT_PhongKham"].ToString()))
                        int.TryParse(dataTable.Rows[0]["STT_PhongKham"].ToString(), out intType);
                    model.CurrentTicket = intType;
                    if (!string.IsNullOrEmpty(dataTable.Rows[0]["ProcessTime"].ToString()))
                        model.CommingTime = DateTime.Parse(dataTable.Rows[0]["ProcessTime"].ToString());
                }

                dataTable.Clear();
                model.Counter = "";
                query = @"select c.Name from Q_Equipment e, Q_Counter c where c.Id = e.CounterId and e.IsDeleted = 0 and c.IsDeleted = 0 and Code =" + equipCode;
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                    model.Counter = dataTable.Rows[0][0].ToString();
            }
            else
                model.IsLogout = true;
            return model;
        }

        public HomeModel GetForHome(SqlConnection sqlConnection, string userName, int equipCode, DateTime date, int useWithThridPattern )
        {
            var model = new HomeModel();
            int userId = 0;
            string query = @"Select * from Q_Login lg, Q_User u where lg.StatusId=" + (int)eStatus.LOGIN + " and lg.EquipCode=" + equipCode + " and u.Id = lg.UserId and u.UserName ='" + userName + "'";
            var da = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0]["UserId"].ToString(), out userId);
                model.UserId = userId;

                int intType = 0;
                dataTable.Clear();
                List<int> majorIds = new List<int>();
                query = @"select um.MajorId from Q_UserMajor um, Q_User u, Q_Major m where u.IsDeleted = 0 and um.IsDeleted = 0 and m.IsDeleted = 0 and um.MajorId = m.Id and um.UserId = u.Id and UM.UserId =" + userId;
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        intType = 0;
                        int.TryParse(row["MajorId"].ToString(), out intType);
                        majorIds.Add(intType);
                    }
                }

                dataTable.Clear();
                model.TotalDone = 0;
                query = "select COUNT(Id) as sl from Q_DailyRequire_Detail where StatusId=" + (int)eStatus.HOTAT + " and UserId=" + userId;
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                    model.TotalDone = Convert.ToInt32(dataTable.Rows[0]["sl"].ToString());

                dataTable.Clear();
                model.TotalWating = 0;
                model.CounterWaitingTickets = "";
                query = @"select r.TicketNumber as stt, r.STT_PhongKham as stt_pk, StatusId from Q_DailyRequire_Detail d, Q_DailyRequire r where d.DailyRequireId = r.Id and (StatusId =" + (int)eStatus.CHOXL + " or StatusId =" + (int)eStatus.QUALUOT + ") and d.MajorId in (" + string.Join(",", majorIds) + ")";
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string _stt = "";
                        if (useWithThridPattern == 0)
                            _stt = row["stt"].ToString() + " ";
                        else if (!string.IsNullOrEmpty(row["stt_pk"].ToString()))
                            _stt = row["stt_pk"].ToString() + " ";

                        if ((int)row["StatusId"] == (int)eStatus.CHOXL)
                            model.CounterWaitingTickets += row["stt"].ToString() + " ";
                        else if ((int)row["StatusId"] == (int)eStatus.QUALUOT)
                            model.CounterWaitingTickets_qualuot += row["stt"].ToString() + " ";
                    }
                    model.TotalWating = dataTable.Rows.Count;
                }

                dataTable.Clear();
                model.CurrentTicket = 0;
                query = "select top 1 d.Id, r.TicketNumber,r.STT_PhongKham,d.ProcessTime,d.StatusId  from Q_DailyRequire_Detail d , Q_DailyRequire r where r.Id = d.DailyRequireId and d.UserId = " + userId + " and (d.StatusId = " + (int)eStatus.DAGXL + " or d.StatusId = " + (int)eStatus.DANHGIA + ") order by d.ProcessTime";
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    intType = 0;
                    if (useWithThridPattern == 0)
                        int.TryParse(dataTable.Rows[0]["TicketNumber"].ToString(), out intType);
                    else if (!string.IsNullOrEmpty(dataTable.Rows[0]["STT_PhongKham"].ToString()))
                        int.TryParse(dataTable.Rows[0]["STT_PhongKham"].ToString(), out intType);
                    model.CurrentTicket = intType;
                    if (!string.IsNullOrEmpty(dataTable.Rows[0]["ProcessTime"].ToString()))
                        model.CommingTime = DateTime.Parse(dataTable.Rows[0]["ProcessTime"].ToString());

                    int.TryParse(dataTable.Rows[0]["StatusId"].ToString(), out intType);
                    model.Status = (intType == (int)eStatus.DANHGIA ? 1 : 0);

                    int.TryParse(dataTable.Rows[0]["Id"].ToString(), out intType);
                    dataTable.Clear();
                    query = "Select top 1 Id from Q_UserEvaluate where DailyRequireDeId = " + intType;
                    da = new SqlDataAdapter(query, sqlConnection);
                    da.Fill(dataTable);
                    model.HasEvaluate = (dataTable != null && dataTable.Rows.Count > 0); 
                }

                dataTable.Clear();
                model.Counter = "";
                query = @"select c.Name from Q_Equipment e, Q_Counter c where c.Id = e.CounterId and e.IsDeleted = 0 and c.IsDeleted = 0 and Code =" + equipCode;
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                    model.Counter = dataTable.Rows[0][0].ToString();

                dataTable.Clear();
                query = @"Select [Content] from Q_CounterSoftRequire where TypeOfRequire ="+(int)eCounterSoftRequireType.SendSMS;
                da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    { 
                        model.SMS.Add(row["Content"].ToString()); 
                    } 
                }
            }
            else
                model.IsLogout = true;
            return model;
        }


        public bool Login(string connectString, Q_LoginHistory obj)
        {
            using (db = new QMSSystemEntities(connectString))
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
        public ResponseBaseModel Login(string connectString, string userName, string password, int equipCode)
        {
            ResponseBaseModel rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                var user = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()) && x.Password.Trim().ToUpper().Equals(password.Trim().ToUpper()));
                if (user != null)
                {
                    var equipObj = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode && x.EquipTypeId == (int)eEquipType.Counter);
                    if (equipObj != null)
                    {
                        Login login = null;
                        var now = DateTime.Now;
                        var objs = db.Q_Login.Where(x => x.UserId == user.Id && x.EquipCode != equipCode && x.StatusId == (int)eStatus.LOGIN).ToList();
                        if (objs.Count > 0)
                        {
                            rs.IsSuccess = false;
                            rs.sms = "Tài khoản " + userName + " đã được đăng nhập ở quầy '" + objs[0].EquipCode + "' .Bạn cần phải đăng xuất khỏi quầy '" + objs[0].EquipCode + "' trước sau đó mới có thể đăng nhập vào quầy này được.";
                        }
                        else
                        {
                            objs = db.Q_Login.Where(x => x.UserId != user.Id && x.EquipCode == equipCode && x.StatusId == (int)eStatus.LOGIN).ToList();
                            if (objs.Count > 0)
                            {
                                rs.IsSuccess = false;
                                rs.sms = "Quầy '" + objs[0].EquipCode + "' đã được đăng nhập bằng tài khoản " + objs[0].Q_User.UserName + " .Bạn cần phải đăng xuất tài khoản " + objs[0].Q_User.UserName + "  khỏi quầy '" + objs[0].EquipCode + "' trước sau đó mới có thể đăng nhập vào quầy này được.";
                            }
                            else
                            {
                                var obj = db.Q_Login.FirstOrDefault(x => x.UserId == user.Id && x.EquipCode == equipCode && x.StatusId == (int)eStatus.LOGIN);
                                if (obj != null)
                                {
                                    login = new Login()
                                    {
                                        UserName = userName,
                                        EquipCode = equipCode,
                                        LoginTime = obj.Date.ToString("dd/MM/yyyy HH:mm"),
                                        UserId = obj.Q_User.Id,
                                        CounterId = equipObj.CounterId,
                                        CounterName = equipObj.Q_Counter.Name,
                                        CounterCode = equipObj.Q_Counter.ShortName
                                    };
                                }
                                else
                                {
                                    login = new Login()
                                    {
                                        UserName = user.Name,
                                        EquipCode = equipCode,
                                        LoginTime = now.ToString("dd/MM/yyyy HH:mm"),
                                        UserId = user.Id,
                                        CounterId = equipObj.CounterId,
                                        CounterName = equipObj.Q_Counter.Name,
                                        CounterCode = equipObj.Q_Counter.ShortName
                                    };
                                    db.Q_Login.Add(new Q_Login()
                                    {
                                        UserId = user.Id,
                                        EquipCode = equipCode,
                                        StatusId = (int)eStatus.LOGIN,
                                        Date = now
                                    });
                                    db.SaveChanges();
                                }
                                rs.IsSuccess = true;
                                rs.Data = login;
                            }
                        }
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.sms = "Không tìm thấy thông tin quầy. Vui lòng kiểm tra cấu hình";
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

        /// <summary>
        /// Counter soft dùng 1 tk login vào nhiều quầy
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResponseBaseModel Login(string connectString, string userName, string password)
        {
            ResponseBaseModel rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                var user = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()) && x.Password.Trim().ToUpper().Equals(password.Trim().ToUpper()));
                if (user != null)
                {
                    List<LoginHistoryModel> logins = null;
                    if (!string.IsNullOrEmpty(user.Counters))
                    {
                        var ids = user.Counters.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        var equipCodes = (from x in db.Q_Equipment where !x.IsDeleted && !x.Q_Counter.IsDeleted && ids.Contains(x.CounterId) select x.Code).ToList();
                        logins = db.Q_Login
                                                    .Where(x => x.StatusId == (int)eStatus.LOGIN && !x.Q_User.IsDeleted && equipCodes.Contains(x.EquipCode))
                                                    .Select(x => new LoginHistoryModel()
                                                    {
                                                        Id = x.Id,
                                                        UserId = x.UserId,
                                                        UserName = x.Q_User.UserName,
                                                        EquipCode = x.EquipCode,
                                                        Date = x.Date,
                                                        StatusId = x.StatusId,
                                                        LogoutTime = x.LogoutTime
                                                    }).ToList();
                        var equips = from x in db.Q_Equipment where !x.IsDeleted select x;
                        foreach (var item in logins)
                        {
                            var found = equips.FirstOrDefault(x => x.Code == item.EquipCode);
                            if (found != null)
                            {
                                item.CounterName = found.Q_Counter.Name;
                                item.CounterCode = found.CounterId;
                            }
                        }
                    }

                    var login = new Login()
                    {
                        UserName = user.Name,
                        EquipCode = 0,
                        LoginTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                        UserId = user.Id,
                        CounterId = 0,
                        CounterName = "",
                        Counters = logins
                    };
                    rs.IsSuccess = true;
                    rs.Data = login;
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Không tìm thấy thông tin tài khoản. Vui lòng kiểm tra lại";
                }
                return rs;
            }
        }

        public void AutoLogin(string connectString, string listAuto)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var listUser = listAuto.Split(',').ToArray();
                if (listUser != null && listUser.Length > 0)
                {
                    DateTime now = DateTime.Now;
                    var logins = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN select x).ToList();
                    for (int i = 0; i < listUser.Length; i++)
                    {
                        var info = listUser[i].Split('-').Select(x => Convert.ToInt32(x)).ToArray();
                        if (info.Length == 2)
                        {
                            var found = logins.FirstOrDefault(x => x.UserId == info[0]);
                            if (found == null)
                                db.Q_Login.Add(new Q_Login()
                                {
                                    UserId = info[0],
                                    EquipCode = info[1],
                                    StatusId = (int)eStatus.LOGIN,
                                    Date = now
                                });
                        }
                    }
                    db.SaveChanges();
                }

            }
        }

        /// <summary>
        /// counter soft
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="date"></param>
        public void Logout(string connectString, int userId, int equipCode, DateTime date)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var objs = db.Q_Login.Where(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.LOGIN).ToList();
                if (objs.Count > 0)
                {
                    foreach (var item in objs)
                    {
                        item.LogoutTime = date;
                        item.StatusId = (int)eStatus.LOGOUT;
                        db.Entry<Q_Login>(item).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }
        }


        public int GetUserId(string connectString, int equipCode, DateTime date)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Login.FirstOrDefault(x => x.EquipCode == equipCode && x.StatusId == (int)eStatus.LOGIN);
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
        public int CounterLoginLogOut(string connectString, int userId, int equipCode, DateTime date)
        {
            int num = 8888;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var obj = db.Q_Login.FirstOrDefault(x => x.StatusId == (int)eStatus.LOGIN && (x.UserId == userId || x.EquipCode == equipCode));
                    if (obj == null)
                    {
                        // chua log
                        obj = new Q_Login() { UserId = userId, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = date };
                        db.Q_Login.Add(obj);
                        num = 9999;
                    }
                    else
                    {
                        if (obj.EquipCode == equipCode && obj.UserId == userId)
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
                            num = 8888;
                        }
                        else
                        {
                            // login on other counter
                            // obj.LogoutTime = date;
                            //   obj.StatusId = (int)eStatus.LOGOUT;

                            //  var newobj = new Q_Login() { UserId = userId, EquipCode = equipCode, StatusId = (int)eStatus.LOGIN, Date = date };
                            //  db.Q_Login.Add(newobj);
                            num = 7777;
                        }

                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public ResponseBase findLogin(string connectString, string maphongkham)
        {
            var rs = new ResponseBase();
            rs.IsSuccess = false;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var tb = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && !x.Q_Counter.IsDeleted && x.Q_Counter.ShortName.Trim().ToUpper().Equals(maphongkham.Trim().ToUpper()));
                    if (tb != null)
                    {
                        var login = db.Q_Login.OrderByDescending(x => x.Date).FirstOrDefault(x => !x.Q_User.IsDeleted && x.StatusId == (int)eStatus.LOGIN && x.EquipCode == tb.Code);
                        if (login != null)
                        {
                            rs.IsSuccess = true;
                            rs.Records = login.EquipCode;
                            rs.Data = login.UserId;
                            rs.Data_1 = tb.CounterId;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return rs;
        }

    }
}
