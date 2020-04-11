using GPRO.Ultilities;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLUserEvaluate
    {
        #region constructor
        static object key = new object();
        private static volatile BLLUserEvaluate _Instance;
        public static BLLUserEvaluate Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUserEvaluate();

                return _Instance;
            }
        }
        private BLLUserEvaluate() { }
        #endregion

        public List<UserEvaluateModel> Gets(string connectString, int userId, DateTime from, DateTime to)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                List<UserEvaluateModel> objs;
                if (userId == 0)
                {
                    objs = db.Q_HisUserEvaluate.Where(x => x.Q_HisDailyRequire_De.ProcessTime >= from && x.Q_HisDailyRequire_De.ProcessTime <= to).Select(x => new UserEvaluateModel()
                    {
                        Id = x.Id,
                        MAPHIEU = x.Q_HisDailyRequire_De.Q_HisDailyRequire.TicketNumber,
                        TENNV = x.Q_User.Name,
                        DANHGIA = x.Score,
                        GLAYPHIEU = x.Q_HisDailyRequire_De.Q_HisDailyRequire.PrintTime,
                        GDENQUAY = x.Q_HisDailyRequire_De.ProcessTime,
                        GGIAODICH = x.Q_HisDailyRequire_De.ProcessTime,
                        GKETTHUC = x.Q_HisDailyRequire_De.EndProcessTime,
                        YKIEN = x.Comment
                    }).ToList();

                    objs.AddRange(db.Q_UserEvaluate.Where(x => x.Q_DailyRequire_Detail.ProcessTime >= from && x.Q_DailyRequire_Detail.ProcessTime <= to).Select(x => new UserEvaluateModel()
                    {
                        Id = x.Id,
                        MAPHIEU = x.Q_DailyRequire_Detail.Q_DailyRequire.TicketNumber,
                        TENNV = x.Q_User.Name,
                        DANHGIA = x.Score,
                        GLAYPHIEU = x.Q_DailyRequire_Detail.Q_DailyRequire.PrintTime,
                        GDENQUAY = x.Q_DailyRequire_Detail.ProcessTime,
                        GGIAODICH = x.Q_DailyRequire_Detail.ProcessTime,
                        GKETTHUC = x.Q_DailyRequire_Detail.EndProcessTime,
                        YKIEN = x.Comment
                    }).ToList());
                }
                else
                {
                    objs = db.Q_HisUserEvaluate.Where(x => x.UserId == userId && x.Q_HisDailyRequire_De.ProcessTime >= from && x.Q_HisDailyRequire_De.ProcessTime <= to).Select(x => new UserEvaluateModel()
                    {
                        Id = x.Id,
                        MAPHIEU = x.Q_HisDailyRequire_De.Q_HisDailyRequire.TicketNumber,
                        TENNV = x.Q_User.Name,
                        DANHGIA = x.Score,
                        GLAYPHIEU = x.Q_HisDailyRequire_De.Q_HisDailyRequire.PrintTime,
                        GDENQUAY = x.Q_HisDailyRequire_De.ProcessTime,
                        GGIAODICH = x.Q_HisDailyRequire_De.ProcessTime,
                        GKETTHUC = x.Q_HisDailyRequire_De.EndProcessTime,
                        YKIEN = x.Comment
                    }).ToList();

                    objs.AddRange(db.Q_UserEvaluate.Where(x => x.UserId == userId && x.Q_DailyRequire_Detail.ProcessTime >= from && x.Q_DailyRequire_Detail.ProcessTime <= to).Select(x => new UserEvaluateModel()
                    {
                        Id = x.Id,
                        MAPHIEU = x.Q_DailyRequire_Detail.Q_DailyRequire.TicketNumber,
                        TENNV = x.Q_User.Name,
                        DANHGIA = x.Score,
                        GLAYPHIEU = x.Q_DailyRequire_Detail.Q_DailyRequire.PrintTime,
                        GDENQUAY = x.Q_DailyRequire_Detail.ProcessTime,
                        GGIAODICH = x.Q_DailyRequire_Detail.ProcessTime,
                        GKETTHUC = x.Q_DailyRequire_Detail.EndProcessTime,
                        YKIEN = x.Comment
                    }).ToList());
                }

                if (objs != null && objs.Count > 0)
                {
                    var evaluateObjs = db.Q_EvaluateDetail.ToList();
                    foreach (var item in objs)
                    {
                        if (item.DANHGIA == "0")
                            item.DANHGIA = "không đánh giá";
                        else if (item.DANHGIA == "1000")
                            item.DANHGIA = "Ý kiến khác";
                        else
                        {
                            var arr = item.DANHGIA.Split('_').Select(x => Convert.ToInt32(x)).ToList();
                            var dt = evaluateObjs.FirstOrDefault(x => x.EvaluateId == arr[0] && x.Id == arr[1]);
                            item.DANHGIA = dt.Name;
                        }
                    }
                }
                return objs;
            }
        }

        public int Insert(string connectString, Q_UserEvaluate obj)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                db.Q_UserEvaluate.Add(obj);
                db.SaveChanges();
                return obj.Id;
            }
        }

        public ResponseBaseModel Evaluate(string connectString, string username, string value, int num, string isUseQMS, string comment)
        {
            var rs = new ResponseBaseModel();
            using (var db = new QMSSystemEntities(connectString))
            {
                var user = db.Q_User.FirstOrDefault(x => x.UserName.Trim().ToUpper().Equals(username.Trim().ToUpper()));
                if (user != null)
                {
                    int tieuchi = 0;
                    int.TryParse(value.Substring((value.IndexOf('_') + 1)), out tieuchi);
                    var detail = db.Q_EvaluateDetail.FirstOrDefault(x => x.Id == tieuchi);
                    bool sendSMS = detail != null ? detail.IsSendSMS : false;
                    var now = DateTime.Now;
                    var system = db.Q_Config.FirstOrDefault(x => x.IsActived && x.Code == eConfigCode.System).Value;

                    if (isUseQMS == "1")
                    {
                        #region
                        var ticketInfos = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.TicketNumber == num && x.Q_User.UserName.Trim().ToUpper().Equals(username.Trim().ToUpper()) && (x.StatusId == (int)eStatus.DANHGIA || x.StatusId == (int)eStatus.DAGXL)).ToList();
                        if (ticketInfos.Count > 0)
                        {
                            var firstObj = ticketInfos.FirstOrDefault();
                            IQueryable<Q_DailyRequire_Detail> details;
                            details = (from x in db.Q_DailyRequire_Detail
                                       where x.DailyRequireId == firstObj.DailyRequireId &&
                                       //hthong xe may ?
                                       // YES => lấy Status =HOTAT 
                                       // NO =>  lấy ststus = DANHGIA | DAGXL
                                       (system == "1" ? x.StatusId == (int)eStatus.HOTAT : (x.StatusId == (int)eStatus.DANHGIA || x.StatusId == (int)eStatus.DAGXL)) &&
                                       //hthong xe may ?
                                       // YES => danh gia cho thợ sửa cuối cùng ko phai thu ngân
                                       // NO =>  danh gia cho user gui yc
                                       (system == "1" ? x.UserId != user.Id : x.UserId == user.Id)
                                       select x);
                            var a = details.ToList();


                            var config = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.DoneTicketAfterEvaluate);
                            string doneTicketAfterEvaluate = "0";
                            if (config != null)
                                doneTicketAfterEvaluate = config.Value;

                            foreach (var item in details)
                            {
                                //ktra so phieu co danh gia chua ?
                                var obj = (from
                                          ue in db.Q_UserEvaluate
                                           where
                                           item.UserId == ue.UserId &&
                                           item.DailyRequireId == ue.Q_DailyRequire_Detail.DailyRequireId
                                           select ue).FirstOrDefault();
                                if (obj == null)
                                {
                                    db.Q_UserEvaluate.Add(new Q_UserEvaluate()
                                    {
                                        UserId = item.UserId ?? 0,
                                        DailyRequireDeId = item.Id,
                                        Score = value,
                                        SendSMS = sendSMS,
                                        CreatedDate = now,
                                        Comment = comment
                                    });
                                }

                                if (item.StatusId != (int)eStatus.HOTAT && doneTicketAfterEvaluate == "1")
                                {
                                    item.StatusId = (int)eStatus.HOTAT;
                                    item.EndProcessTime = now;
                                    db.Entry<Q_DailyRequire_Detail>(item).State = System.Data.Entity.EntityState.Modified;
                                }
                            }

                            //neu he thong xe may & end phieu sau khi danh gia thi di tim dòng của thu ngân sau đó end cho thu ngân
                            if (system == "1" && doneTicketAfterEvaluate == "1")
                            {
                                details = (from x in db.Q_DailyRequire_Detail
                                           where
                                           x.DailyRequireId == firstObj.DailyRequireId &&
                                           (x.StatusId == (int)eStatus.DANHGIA || x.StatusId == (int)eStatus.DAGXL)
                                            && x.UserId == user.Id
                                           select x);
                                foreach (var item in details)
                                {
                                    item.StatusId = (int)eStatus.HOTAT;
                                    item.EndProcessTime = now;
                                    db.Entry<Q_DailyRequire_Detail>(item).State = System.Data.Entity.EntityState.Modified;
                                }
                            }

                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                        #endregion
                    }
                    else
                    {
                        db.Q_UserEvaluate.Add(new Q_UserEvaluate()
                        {
                            UserId = user.Id,
                            DailyRequireDeId = null,
                            Score = value,
                            SendSMS = sendSMS,
                            CreatedDate = DateTime.Now,
                            Comment = comment
                        });
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }

                    if (sendSMS)
                    {
                        Q_CounterSoftRequire require;
                        string maNV = user.Id.ToString();
                        var phones = db.Q_RecieverSMS.Where(x => x.IsActive && x.UserIds.Contains(maNV)).ToList();
                        if (phones.Count > 0)
                        {
                            foreach (var item in phones)
                            {
                                if (item.UserIds.Split(',').ToArray().Contains(maNV) && item.PhoneNumber.Length > 0)
                                {
                                    require = new Q_CounterSoftRequire();
                                    require.Content = item.PhoneNumber + ":" + user.Name + "(" + user.UserName + ")" + " " + detail.SmsContent;
                                    require.TypeOfRequire = (int)eCounterSoftRequireType.SendSMS;
                                    db.Q_CounterSoftRequire.Add(require);
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return rs;
        }

        /// <summary>
        /// Đánh giá chất lượng phục vụ bằng counter
        /// </summary>
        /// <param name="equipCode">ma thiet bi</param>
        /// <param name="value">value đánh giá</param>
        /// <param name="system">hệ thống 1=> xe máy , 0 => khác </param> 
        /// <returns></returns>

        public ResponseBaseModel DanhGia_BP(string connectString, int equipCode, string value, int system)
        {
            var rs = new ResponseBaseModel();
            try
            {
                using (var db = new QMSSystemEntities(connectString))
                {
                    var login = db.Q_Login.FirstOrDefault(x => x.EquipCode == equipCode && x.StatusId == (int)eStatus.LOGIN);
                    if (login != null)
                    {
                        int tieuchi = 0;
                        int.TryParse(value.Substring((value.IndexOf('_') + 1)), out tieuchi);
                        bool sendSMS = db.Q_EvaluateDetail.FirstOrDefault(x => x.Id == tieuchi).IsSendSMS;

                        var now = DateTime.Now;
                        #region
                        var ticketInfos = db.Q_DailyRequire_Detail.Where(x => x.UserId == login.UserId && (x.StatusId == (int)eStatus.DANHGIA || x.StatusId == (int)eStatus.DAGXL)).ToList();
                        if (ticketInfos.Count > 0)
                        {
                            var firstObj = ticketInfos.FirstOrDefault();
                            IQueryable<Q_DailyRequire_Detail> details;
                            details = (from x in db.Q_DailyRequire_Detail
                                       where x.DailyRequireId == firstObj.DailyRequireId &&
                                       (x.StatusId == (int)eStatus.DANHGIA || x.StatusId == (int)eStatus.DAGXL) &&
                                       (system == 1 ? true : x.UserId == login.UserId)
                                       select x);

                            foreach (var item in details)
                            {
                                var obj = (from
                                          ue in db.Q_UserEvaluate
                                           where
                                           item.UserId == ue.UserId &&
                                           item.DailyRequireId == ue.Q_DailyRequire_Detail.DailyRequireId
                                           select ue).FirstOrDefault();
                                if (obj == null)
                                {
                                    db.Q_UserEvaluate.Add(new Q_UserEvaluate()
                                    {
                                        UserId = item.UserId ?? 0,
                                        DailyRequireDeId = item.Id,
                                        Score = value,
                                        SendSMS = sendSMS,
                                        CreatedDate = now
                                    });
                                }
                                var config = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.DoneTicketAfterEvaluate);
                                string doneTicketAfterEvaluate = "0";
                                if (config != null)
                                    doneTicketAfterEvaluate = config.Value;
                                if (item.StatusId != (int)eStatus.HOTAT && doneTicketAfterEvaluate == "1")
                                {
                                    item.StatusId = (int)eStatus.HOTAT;
                                    item.EndProcessTime = now;
                                    db.Entry<Q_DailyRequire_Detail>(item).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                        #endregion
                    }
                }
            }
            catch { }
            return rs;
        }

        public List<ReportEvaluateModel> GetDailyReport(SqlConnection sqlConnection, bool reportForUser, DateTime fromDate, DateTime toDate)
        {
            fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 0);
            var now = DateTime.Now;
            var report = new List<ReportEvaluateModel>();
            string query = "";
            if (reportForUser)
                query = "select Id as ServiceId, name as ServiceName from Q_User where IsDeleted=0";
            else
                query = "select Id as ServiceId, name as ServiceName from Q_Service where IsDeleted=0 and IsActived=1";
            var adap = new SqlDataAdapter(query, sqlConnection);
            var dt = new DataTable();
            adap.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    report.Add(new ReportEvaluateModel()
                    {
                        ServiceId = Convert.ToInt32(row["ServiceId"].ToString()),
                        ServiceName = row["ServiceName"].ToString()
                    });
                }

                #region ycDanhGia 
                var ycDanhGia = new List<ModelSelectItem>();
                query = "select d.EvaluateId, d.Id,d.Name  from Q_EvaluateDetail d, Q_Evaluate e where e.Id = d.EvaluateId and e.IsDeleted=0 and d.IsDeleted=0 order by d.[Index]";
                dt.Clear();
                adap = new SqlDataAdapter(query, sqlConnection);
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ycDanhGia.Add(new ModelSelectItem()
                        {
                            Code = (row["EvaluateId"].ToString() + "_" + row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            Id = 0,
                            Data = 0
                        });
                    }
                }
                ycDanhGia.Add(new ModelSelectItem() { Code = "1000", Name = "Ý kiến khác", Id = 0, Data = 0 });
                #endregion

                var histories = new List<ReportEvaluateDetailModel>();
                query = "select ue.UserId,u.Name as UserName, dr.ServiceId, s.Name as ServiceName,dr.PrintTime,ue.CreatedDate,dd.EndProcessTime,ue.Score, ue.Comment,dr.TicketNumber  from Q_HisUserEvaluate ue, Q_HisDailyRequire dr, Q_HisDailyRequire_De dd, Q_User u, Q_Service s where ue.IsDeleted = 0 and ue.HisDailyRequireDeId = dd.Id and dd.HisDailyRequireId = dr.Id and u.IsDeleted = 0 and ue.UserId = u.Id and s.IsDeleted = 0 and s.IsActived =1 and s.Id = dr.ServiceId and dr.PrintTime >= '" + fromDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                dt.Clear();
                adap = new SqlDataAdapter(query, sqlConnection);
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        histories.Add(new ReportEvaluateDetailModel()
                        {
                            UserId = getIntValue(row["UserId"]),
                            UserName = getStringValue(row["UserName"]),
                            ServiceId = getIntValue(row["ServiceId"]),
                            ServiceName = getStringValue(row["ServiceName"]),
                            PrintTime = getDateValue(row["PrintTime"]),
                            EvaluateTime = getDateValue(row["CreatedDate"]),
                            EndProcessTime = getDateValue(row["EndProcessTime"]),
                            Score = getStringValue(row["Score"]),
                            Comment = getStringValue(row["Comment"]),
                            Number = getIntValue(row["TicketNumber"])
                        });
                    }
                }
                //  var ss = histories.Where(x => !x.PrintTime.HasValue || !x.EvaluateTime.HasValue).ToList();
                if (DateTime.Now < toDate)
                {
                    query = "select ue.UserId,u.Name as UserName, dr.ServiceId, s.Name as ServiceName,dr.PrintTime,ue.CreatedDate,dd.EndProcessTime,ue.Score, ue.Comment,dr.TicketNumber  from Q_UserEvaluate ue, Q_DailyRequire dr, Q_DailyRequire_Detail dd, Q_User u, Q_Service s where ue.IsDeleted = 0 and ue.DailyRequireDeId = dd.Id and dd.DailyRequireId = dr.Id and u.IsDeleted = 0 and ue.UserId = u.Id and s.IsDeleted = 0 and s.IsActived =1 and s.Id = dr.ServiceId";
                    dt.Clear();
                    adap = new SqlDataAdapter(query, sqlConnection);
                    adap.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            histories.Add(new ReportEvaluateDetailModel()
                            {
                                UserId = getIntValue(row["UserId"]),
                                UserName = getStringValue(row["UserName"]),
                                ServiceId = getIntValue(row["ServiceId"]),
                                ServiceName = getStringValue(row["ServiceName"]),
                                PrintTime = getDateValue(row["PrintTime"]),
                                EvaluateTime = getDateValue(row["CreatedDate"]),
                                EndProcessTime = getDateValue(row["EndProcessTime"]),
                                Score = getStringValue(row["Score"]),
                                Comment = getStringValue(row["Comment"]),
                                Number = getIntValue(row["TicketNumber"])
                            });
                        }
                    }
                }

                foreach (var r in report)
                {
                    foreach (var yc in ycDanhGia)
                    {
                        var a = new ModelSelectItem();
                        Parse.CopyObject(yc, ref a);
                        if (reportForUser)
                            a.Id = histories.Where(x => x.Score == yc.Code && x.UserId == r.ServiceId).Count();
                        else
                            a.Id = histories.Where(x => x.Score == yc.Code && x.ServiceId == r.ServiceId).Count();
                        r.Details.Add(a);
                    }
                }
            }
            return report.ToList();
        }
        public List<ReportEvaluateModel> GetDailyReport_NotUseQMS(SqlConnection sqlConnection, bool reportForUser, DateTime fromDate, DateTime toDate)
        {
            fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 0);
            var now = DateTime.Now;
            var report = new List<ReportEvaluateModel>();
            string query = "select Id as ServiceId, name as ServiceName from Q_User where IsDeleted=0";
            var adap = new SqlDataAdapter(query, sqlConnection);
            var dt = new DataTable();
            adap.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    report.Add(new ReportEvaluateModel()
                    {
                        ServiceId = Convert.ToInt32(row["ServiceId"].ToString()),
                        ServiceName = row["ServiceName"].ToString()
                    });
                }

                #region ycDanhGia 
                var ycDanhGia = new List<ModelSelectItem>();
                query = "select d.EvaluateId, d.Id,d.Name  from Q_EvaluateDetail d, Q_Evaluate e where e.Id = d.EvaluateId and e.IsDeleted=0 and d.IsDeleted=0 order by d.[Index]";
                dt.Clear();
                adap = new SqlDataAdapter(query, sqlConnection);
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ycDanhGia.Add(new ModelSelectItem()
                        {
                            Code = (row["EvaluateId"].ToString() + "_" + row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            Id = 0,
                            Data = 0
                        });
                    }
                }
                ycDanhGia.Add(new ModelSelectItem() { Code = "1000", Name = "Ý kiến khác", Id = 0, Data = 0 });
                #endregion

                var histories = new List<ReportEvaluateDetailModel>();
                query = "select ue.UserId,u.Name as UserName, ue.CreatedDate, ue.Score, ue.Comment from Q_HisUserEvaluate ue, Q_User u, Q_Service s where ue.IsDeleted = 0  and u.IsDeleted = 0 and ue.UserId = u.Id and s.IsDeleted = 0 and s.IsActived = 1   and ue.CreatedDate >= '" + fromDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and ue.CreatedDate <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                dt.Clear();
                adap = new SqlDataAdapter(query, sqlConnection);
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        histories.Add(new ReportEvaluateDetailModel()
                        {
                            UserId = getIntValue(row["UserId"]),
                            UserName = getStringValue(row["UserName"]),
                            EvaluateTime = getDateValue(row["CreatedDate"]),
                            Score = getStringValue(row["Score"]),
                            Comment = getStringValue(row["Comment"]),
                        });
                    }

                    if (DateTime.Now < toDate)
                    {
                        query = "select ue.UserId,u.Name as UserName, ue.CreatedDate ,ue.Score, ue.Comment from Q_UserEvaluate ue, Q_User u where ue.IsDeleted = 0 and u.IsDeleted = 0 and ue.UserId = u.Id";
                        dt.Clear();
                        adap = new SqlDataAdapter(query, sqlConnection);
                        adap.Fill(dt);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                histories.Add(new ReportEvaluateDetailModel()
                                {
                                    UserId = getIntValue(row["UserId"]),
                                    UserName = getStringValue(row["UserName"]),
                                    EvaluateTime = getDateValue(row["CreatedDate"]),
                                    Score = getStringValue(row["Score"]),
                                    Comment = getStringValue(row["Comment"])
                                });
                            }
                        }
                    }
                }

                foreach (var r in report)
                {
                    foreach (var yc in ycDanhGia)
                    {
                        var a = new ModelSelectItem();
                        Parse.CopyObject(yc, ref a);
                        if (reportForUser)
                            a.Id = histories.Where(x => x.Score == yc.Code && x.UserId == r.ServiceId).Count();
                        r.Details.Add(a);
                    }
                }

            }
            return report.ToList();
        }

        private int getIntValue(object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return Convert.ToInt32(value);
            return 0;
        }
        private string getStringValue(object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return value.ToString();
            return "";
        }
        private DateTime? getDateValue(object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return DateTime.Parse(value.ToString());
            return null;
        }

        public List<ReportEvaluateModel> GetReport(string connectString, int userId, DateTime from, DateTime to)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                List<ReportEvaluateModel> report = new List<ReportEvaluateModel>();
                ReportEvaluateModel obj;
                var ycDanhGia = db.Q_EvaluateDetail.
                                    Where(x => !x.IsDeleted && !x.Q_Evaluate.IsDeleted).
                                    OrderBy(x => x.Index).
                                    Select(x => new ModelSelectItem() { Code = x.EvaluateId + "_" + x.Id, Name = x.Name, Id = 0, Data = 0 }).
                                    ToList();
                ycDanhGia.Add(new ModelSelectItem() { Code = "1000", Name = "Ý kiến khác", Id = 0, Data = 0 });
                if (userId != 0)
                {
                    obj = new ReportEvaluateModel();
                    obj.Name = db.Q_User.FirstOrDefault(x => x.Id == userId).Name;
                    var danhgia = db.Q_UserEvaluate.Where(x => (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day >= from.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == from.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == from.Year) && (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day <= to.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == to.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == to.Year) && x.UserId == userId).ToList();
                    if (danhgia.Count > 0)
                    {
                        obj.Details.AddRange(ycDanhGia);
                        foreach (var yc in obj.Details)
                            yc.Id = danhgia.Where(x => x.Score == yc.Code).Count();

                    }
                    report.Add(obj);
                }
                else
                {
                    var danhgia = db.Q_UserEvaluate.Where(x => (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day >= from.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == from.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == from.Year) && (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day <= to.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == to.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == to.Year)).ToList();
                    report.AddRange(db.Q_User.Select(x => new ReportEvaluateModel() { UserId = x.Id, Name = x.Name }));

                    if (danhgia.Count > 0)
                    {
                        foreach (var item in report)
                        {
                            var objs = danhgia.Where(x => x.UserId == item.UserId).ToList();
                            if (objs.Count > 0)
                            {
                                foreach (var yc in item.Details)
                                {
                                    var a = new ModelSelectItem();
                                    Parse.CopyObject(yc, ref a);
                                    a.Id = danhgia.Where(x => x.Score == yc.Code).Count();
                                    item.Details.Add(a);
                                }
                            }
                        }
                    }
                }
                return report;
            }
        }

        public List<ReportEvaluateModel> GetReport_NotUseQMS(string connectString, int userId, DateTime from, DateTime to)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                List<ReportEvaluateModel> report = new List<ReportEvaluateModel>();
                var ycDanhGia = db.Q_EvaluateDetail.
                                 Where(x => !x.IsDeleted && !x.Q_Evaluate.IsDeleted).
                                 OrderBy(x => x.Index).
                                 Select(x => new ModelSelectItem() { Code = x.EvaluateId + "_" + x.Id, Name = x.Name, Id = 0, Data = 0 }).
                                 ToList();
                ycDanhGia.Add(new ModelSelectItem() { Code = "1000", Name = "Ý kiến khác", Id = 0, Data = 0 });
                ReportEvaluateModel obj;
                if (userId != 0)
                {
                    obj = new ReportEvaluateModel();
                    obj.Name = db.Q_User.FirstOrDefault(x => x.Id == userId).Name;
                    obj.Details.AddRange(ycDanhGia);
                    var danhgia_HN = db.Q_UserEvaluate.Where(x => x.CreatedDate >= from && x.CreatedDate <= to && x.UserId == userId).ToList();
                    //  var danhgia_LS = db.Q_HisUserEvaluate.Where(x => (x.Q_HisDailyRequire_De.ProcessTime.Value.Day >= from.Day && x.Q_HisDailyRequire_De.ProcessTime.Value.Month == from.Month && x.Q_HisDailyRequire_De.ProcessTime.Value.Year == from.Year) && (x.Q_HisDailyRequire_De.ProcessTime.Value.Day <= to.Day && x.Q_HisDailyRequire_De.ProcessTime.Value.Month == to.Month && x.Q_HisDailyRequire_De.ProcessTime.Value.Year == to.Year) && x.UserId == userId).ToList();
                    var danhgia_LS = db.Q_HisUserEvaluate.Where(x =>
                                                x.Q_HisDailyRequire_De.ProcessTime.HasValue &&
                                                x.Q_HisDailyRequire_De.ProcessTime.Value >= from &&
                                                x.Q_HisDailyRequire_De.ProcessTime.Value <= to && x.UserId == userId)
                                                .ToList();
                    foreach (var yc in ycDanhGia)
                    {
                        var child = new ModelSelectItem();
                        Parse.CopyObject(yc, ref child);
                        child.Id += danhgia_HN.Where(x => x.Score == yc.Code).Count();
                        child.Id += danhgia_LS.Where(x => x.Score == yc.Code).Count();
                        obj.Details.Add(child);
                    }
                    report.Add(obj);
                }
                else
                {
                    var danhgiaHomnay = db.Q_UserEvaluate.Where(x => x.CreatedDate >= from && x.CreatedDate <= to).ToList();
                    var danhgia_LS = db.Q_HisUserEvaluate.Where(x => x.Q_HisDailyRequire_De.ProcessTime.HasValue &&
                    x.Q_HisDailyRequire_De.ProcessTime.Value >= from &&
                    x.Q_HisDailyRequire_De.ProcessTime <= to).ToList();

                    report.AddRange(db.Q_User.Select(x => new ReportEvaluateModel() { UserId = x.Id, Name = x.Name }));
                    foreach (var item in report)
                    {
                        //  item.Details.AddRange(ycDanhGia);
                        var objs_HN = danhgiaHomnay.Where(x => x.UserId == item.UserId).ToList();
                        var objs_LS = danhgia_LS.Where(x => x.UserId == item.UserId).ToList();

                        foreach (var yc in ycDanhGia)
                        {
                            var child = new ModelSelectItem();
                            Parse.CopyObject(yc, ref child);
                            child.Id += objs_HN.Where(x => x.Score == yc.Code).Count();
                            child.Id += objs_LS.Where(x => x.Score == yc.Code).Count();
                            item.Details.Add(child);
                        }
                    }
                }
                return report;
            }
        }

        public List<ReportEvaluateDetailModel> GetDailyReport_Detail(SqlConnection sqlConnection, DateTime fromDate, DateTime toDate)
        {
            fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 0);
            var histories = new List<ReportEvaluateDetailModel>();
            var query = "select ue.UserId,u.Name as UserName, dr.ServiceId, s.Name as ServiceName,dr.PrintTime,ue.CreatedDate,dd.EndProcessTime,ue.Score, ue.Comment,dr.TicketNumber  from Q_HisUserEvaluate ue, Q_HisDailyRequire dr, Q_HisDailyRequire_De dd, Q_User u, Q_Service s where ue.IsDeleted = 0 and ue.HisDailyRequireDeId = dd.Id and dd.HisDailyRequireId = dr.Id and u.IsDeleted = 0 and ue.UserId = u.Id and s.IsDeleted = 0 and s.IsActived =1 and s.Id = dr.ServiceId and dr.PrintTime >= '" + fromDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            var dt = new DataTable();
            var adap = new SqlDataAdapter(query, sqlConnection);
            adap.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    histories.Add(new ReportEvaluateDetailModel()
                    {
                        UserId = getIntValue(row["UserId"]),
                        UserName = getStringValue(row["UserName"]),
                        ServiceId = getIntValue(row["ServiceId"]),
                        ServiceName = getStringValue(row["ServiceName"]),
                        PrintTime = getDateValue(row["PrintTime"]),
                        EvaluateTime = getDateValue(row["CreatedDate"]),
                        EndProcessTime = getDateValue(row["EndProcessTime"]),
                        Score = getStringValue(row["Score"]),
                        Comment = getStringValue(row["Comment"]),
                        Number = getIntValue(row["TicketNumber"])
                    });
                }
            }
            if (DateTime.Now < toDate)
            {
                query = "select ue.UserId,u.Name as UserName, dr.ServiceId, s.Name as ServiceName,dr.PrintTime,ue.CreatedDate,dd.EndProcessTime,ue.Score, ue.Comment,dr.TicketNumber  from Q_UserEvaluate ue, Q_DailyRequire dr, Q_DailyRequire_Detail dd, Q_User u, Q_Service s where ue.IsDeleted = 0 and ue.DailyRequireDeId = dd.Id and dd.DailyRequireId = dr.Id and u.IsDeleted = 0 and ue.UserId = u.Id and s.IsDeleted = 0 and s.IsActived =1 and s.Id = dr.ServiceId";
                dt.Clear();
                adap = new SqlDataAdapter(query, sqlConnection);
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        histories.Add(new ReportEvaluateDetailModel()
                        {
                            UserId = getIntValue(row["UserId"]),
                            UserName = getStringValue(row["UserName"]),
                            ServiceId = getIntValue(row["ServiceId"]),
                            ServiceName = getStringValue(row["ServiceName"]),
                            PrintTime = getDateValue(row["PrintTime"]),
                            EvaluateTime = getDateValue(row["CreatedDate"]),
                            EndProcessTime = getDateValue(row["EndProcessTime"]),
                            Score = getStringValue(row["Score"]),
                            Comment = getStringValue(row["Comment"]),
                            Number = getIntValue(row["TicketNumber"])
                        });
                    }
                }
            }

            return histories.OrderBy(x => x.PrintTime).ThenBy(x => x.Number).ToList();
        }

        public SendSMSModel GetRequireSendSMS(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var report = new SendSMSModel();
                report.Phones.AddRange(db.Q_RecieverSMS.Where(x => x.IsActive).Select(x => x.PhoneNumber));

                //report.AddRange(db.Q_UserEvaluate.Where(x => !x.IsDeleted && x.SendSMS && !x.Q_User.IsDeleted).Select(x => new SMSModel() { UserName = x.Q_User.Name, ServiceName = x.Q_DailyRequire_Detail.Q_DailyRequire.Q_Service.Name, MajorName = x.Q_DailyRequire_Detail.Q_Major.Name, sms = x.Score }));
                report.SMS.AddRange(db.Q_UserEvaluate.Where(x => !x.IsDeleted && x.SendSMS && !x.Q_User.IsDeleted).Select(x => new SMSModel() { UserName = x.Q_User.Name, sms = x.Score, MajorName = x.Q_DailyRequire_Detail.Q_Major.Name }));
                if (report.SMS.Count > 0)
                {
                    var ThangDGs = db.Q_EvaluateDetail.Where(x => !x.IsDeleted && !x.Q_Evaluate.IsDeleted).ToList();
                    Q_EvaluateDetail found;
                    int[] arr;
                    for (int i = 0; i < report.SMS.Count; i++)
                    {
                        arr = report.SMS[i].sms.Split('_').Select(x => Convert.ToInt32(x)).ToArray();
                        if (arr.Length > 1)
                        {
                            found = ThangDGs.FirstOrDefault(x => x.Id == arr[1]);
                            report.SMS[i].sms = (found != null ? found.SmsContent : "");
                        }
                    }
                    db.Database.ExecuteSqlCommand("update  q_userevaluate set sendsms = 0");
                    db.SaveChanges();
                }
                return report;
            }
        }

        public List<string> GetRequireSendSMSForAndroid(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var requires = db.Q_CounterSoftRequire.Where(x => x.TypeOfRequire == (int)eCounterSoftRequireType.SendSMS).Select(x => x.Content).ToList();

                db.Database.ExecuteSqlCommand("delete  Q_CounterSoftRequire where TypeOfRequire = 3");
                db.SaveChanges();
                return requires;
            }
        }

        public AndroidModel GetInfoForAndroid(string connectString, string userName, int getSTT, int getSMS, int getUserInfo)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                AndroidModel androidModel = new AndroidModel();
                if (getSTT == 1)
                {
                    var obj = db.Q_DailyRequire_Detail.Where(x => (x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.DANHGIA) && x.ProcessTime.Value.Day == DateTime.Now.Day && x.ProcessTime.Value.Month == DateTime.Now.Month && x.ProcessTime.Value.Year == DateTime.Now.Year && x.Q_User.UserName.Trim().ToUpper().Equals(userName)).FirstOrDefault();
                    if (obj != null)
                    {
                        var userEval = db.Q_UserEvaluate.FirstOrDefault(x => x.DailyRequireDeId == obj.Id);
                        androidModel.HasEvaluate = (userEval != null ? true : false);
                        androidModel.TicketNumber = obj.Q_DailyRequire.TicketNumber;
                        androidModel.Status = (obj.StatusId == (int)eStatus.DANHGIA ? 1 : 0);
                    }
                    else
                        androidModel.TicketNumber = 0;
                }

                if (getUserInfo == 1)
                    androidModel.UserInfo = BLLUser.Instance.GetByUserName(connectString, userName);

                if (getSMS == 1)
                {
                    androidModel.SMS = db.Q_CounterSoftRequire.Where(x => x.TypeOfRequire == (int)eCounterSoftRequireType.SendSMS).Select(x => x.Content).ToList();
                    db.Database.ExecuteSqlCommand("delete  Q_CounterSoftRequire where TypeOfRequire = 3");
                    db.SaveChanges();
                }
                return androidModel;
            }
        }

    }
}

public class ReportEvaluateModel
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string ServiceName { get; set; }
    public int ServiceId { get; set; }
    public List<ModelSelectItem> Details { get; set; }
    public ReportEvaluateModel()
    {
        Details = new List<ModelSelectItem>();
    }
}


public class ReportEvaluateDetailModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int ServiceId { get; set; }
    public string ServiceName { get; set; }
    public DateTime? PrintTime { get; set; }
    public DateTime? EvaluateTime { get; set; }
    public DateTime? EndProcessTime { get; set; }
    public string Score { get; set; }
    public int Number { get; set; }
    public string Comment { get; set; }
}


public class SMSModel
{
    public string UserName { get; set; }
    public string ServiceName { get; set; }
    public string MajorName { get; set; }
    public string sms { get; set; }
}

public class SendSMSModel
{
    public List<SMSModel> SMS { get; set; }
    public List<string> Phones { get; set; }
    public SendSMSModel()
    {
        SMS = new List<SMSModel>();
        Phones = new List<string>();
    }
}

public class AndroidModel
{
    public List<string> SMS { get; set; }
    public int TicketNumber { get; set; }
    public int Status { get; set; }
    public UserModel UserInfo { get; set; }
    public bool HasEvaluate { get; set; }
    public AndroidModel()
    {
        SMS = new List<string>();
    }
}

