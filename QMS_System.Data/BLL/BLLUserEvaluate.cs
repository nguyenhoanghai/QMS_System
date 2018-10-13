using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
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

        public List<UserEvaluateModel> Gets(int userId, DateTime from, DateTime to)
        {
            using (var db = new QMSSystemEntities())
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
                    }).ToList());
                }

                if (objs != null && objs.Count > 0)
                {
                    var evaluateObjs = db.Q_EvaluateDetail.ToList();
                    foreach (var item in objs)
                    {
                        if (item.DANHGIA == "0")
                            item.DANHGIA = "không đánh giá";
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
         
        public int Insert(Q_UserEvaluate obj)
        {
            using (var db = new QMSSystemEntities())
            {
                db.Q_UserEvaluate.Add(obj);
                db.SaveChanges();
                return obj.Id;
            }
        }

        public ResponseBaseModel Evaluate(string username, string value, int num, string isUseQMS)
        {
            var rs = new ResponseBaseModel();
            try
            {
                using (var db = new QMSSystemEntities())
                {
                    var user = db.Q_User.FirstOrDefault(x => x.UserName.Trim().ToUpper().Equals(username.Trim().ToUpper()));
                    int tieuchi = 0;
                    int.TryParse(value.Substring((value.IndexOf('_') + 1)), out tieuchi);
                    bool sendSMS = db.Q_EvaluateDetail.FirstOrDefault(x => x.Id == tieuchi).IsSendSMS;

                    var now = DateTime.Now;
                    if (isUseQMS == "1")
                    {
                        #region
                        var ticketInfos = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.TicketNumber == num && x.Q_User.UserName.Trim().ToUpper().Equals(username.Trim().ToUpper()) && (x.StatusId == (int)eStatus.DANHGIA || x.StatusId == (int)eStatus.DAGXL)).ToList();
                        if (ticketInfos.Count > 0 && user != null)
                        {
                            var firstObj = ticketInfos.FirstOrDefault();
                            var obj = db.Q_UserEvaluate.FirstOrDefault(x => x.UserId == user.Id && x.Q_DailyRequire_Detail.Q_DailyRequire.TicketNumber == num && x.Q_DailyRequire_Detail.Q_DailyRequire.PrintTime == firstObj.Q_DailyRequire.PrintTime);
                            if (obj == null)
                            {
                                db.Q_UserEvaluate.Add(new Q_UserEvaluate()
                                {
                                    UserId = user.Id,
                                    DailyRequireDeId = firstObj.Id,
                                    Score = value,
                                    SendSMS = sendSMS,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            foreach (var item in ticketInfos)
                            {
                                item.StatusId = (int)eStatus.HOTAT;
                                item.EndProcessTime = now;
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
                            CreatedDate = DateTime.Now
                        });
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                }
            }
            catch { }
            return rs;
        }

        public List<ReportEvaluateModel> GetDailyReport()
        {
            using (var db = new QMSSystemEntities())
            {
                var now = DateTime.Now;
                List<ReportEvaluateModel> report = new List<ReportEvaluateModel>();
                report.AddRange(db.Q_Major.Select(x => new ReportEvaluateModel() { ServiceId = x.Id, ServiceName = x.Name, tc1 = 0, tc2 = 0, tc3 = 0 }));
                if (report.Count > 0)
                {
                    var danhgia = db.Q_UserEvaluate.Where(x => (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day >= now.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == now.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == now.Year) && (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day <= now.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == now.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == now.Year)).ToList();
                    var nvDG = db.Q_UserMajor.ToList();
                    List<int> userIds;
                    foreach (var r in report)
                    {
                        userIds = nvDG.Where(x => x.MajorId == r.ServiceId && x.Index == 1).Select(x => x.UserId).Distinct().ToList();
                        if (userIds.Count > 0)
                        {
                            r.tc1 = danhgia.Where(x => x.Score == "1_1" && userIds.Contains(x.UserId)).Count();
                            r.tc2 = danhgia.Where(x => x.Score == "1_2" && userIds.Contains(x.UserId)).Count();
                            r.tc3 = danhgia.Where(x => x.Score == "1_3" && userIds.Contains(x.UserId)).Count();
                        }
                    }
                }
                return report;
            }
        }

        public List<ReportEvaluateModel> GetDailyReport_NotUseQMS()
        {
            using (var db = new QMSSystemEntities())
            {
                var now = DateTime.Now;
                List<ReportEvaluateModel> report = new List<ReportEvaluateModel>();
                report.AddRange(db.Q_User.Select(x => new ReportEvaluateModel() { ServiceId = x.Id, ServiceName = x.Name, tc1 = 0, tc2 = 0, tc3 = 0 }));
                if (report.Count > 0)
                {
                    var danhgia = db.Q_UserEvaluate.Where(x => x.CreatedDate.Day == now.Day && x.CreatedDate.Month == now.Month && x.CreatedDate.Year == now.Year).ToList();
                    foreach (var r in report)
                    {
                        r.tc1 = danhgia.Where(x => x.Score == "1_1" && x.UserId == r.ServiceId).Count();
                        r.tc2 = danhgia.Where(x => x.Score == "1_2" && x.UserId == r.ServiceId).Count();
                        r.tc3 = danhgia.Where(x => x.Score == "1_3" && x.UserId == r.ServiceId).Count();
                    }
                }
                return report;
            }
        }

        public List<ReportEvaluateModel> GetReport(int userId, DateTime from, DateTime to)
        {
            using (var db = new QMSSystemEntities())
            {
                List<ReportEvaluateModel> report = new List<ReportEvaluateModel>();
                ReportEvaluateModel obj;
                if (userId != 0)
                {
                    obj = new ReportEvaluateModel();
                    obj.Name = db.Q_User.FirstOrDefault(x => x.Id == userId).Name;
                    obj.tc1 = 0;
                    obj.tc2 = 0;
                    obj.tc3 = 0;
                    var danhgia = db.Q_UserEvaluate.Where(x => (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day >= from.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == from.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == from.Year) && (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day <= to.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == to.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == to.Year) && x.UserId == userId).ToList();
                    if (danhgia.Count > 0)
                    {
                        obj.tc1 = danhgia.Where(x => x.Score == "1_1").Count();
                        obj.tc2 = danhgia.Where(x => x.Score == "1_2").Count();
                        obj.tc3 = danhgia.Where(x => x.Score == "1_3").Count();
                    }
                    report.Add(obj);
                }
                else
                {
                    var danhgia = db.Q_UserEvaluate.Where(x => (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day >= from.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == from.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == from.Year) && (x.Q_DailyRequire_Detail.EndProcessTime.Value.Day <= to.Day && x.Q_DailyRequire_Detail.EndProcessTime.Value.Month == to.Month && x.Q_DailyRequire_Detail.EndProcessTime.Value.Year == to.Year)).ToList();
                    report.AddRange(db.Q_User.Select(x => new ReportEvaluateModel() { UserId = x.Id, Name = x.Name, tc1 = 0, tc2 = 0, tc3 = 0 }));

                    if (danhgia.Count > 0)
                    {
                        foreach (var item in report)
                        {
                            var objs = danhgia.Where(x => x.UserId == item.UserId).ToList();
                            if (objs.Count > 0)
                            {
                                item.tc1 = objs.Where(x => x.Score == "1_1").Count();
                                item.tc2 = objs.Where(x => x.Score == "1_2").Count();
                                item.tc3 = objs.Where(x => x.Score == "1_3").Count();
                            }
                        }
                    }
                }
                return report;
            }
        }

        public List<ReportEvaluateModel> GetReport_NotUseQMS(int userId, DateTime from, DateTime to)
        {
            using (var db = new QMSSystemEntities())
            {
                List<ReportEvaluateModel> report = new List<ReportEvaluateModel>();
                ReportEvaluateModel obj;
                if (userId != 0)
                {
                    obj = new ReportEvaluateModel();
                    obj.Name = db.Q_User.FirstOrDefault(x => x.Id == userId).Name;
                    obj.tc1 = 0;
                    obj.tc2 = 0;
                    obj.tc3 = 0;
                    var danhgia = db.Q_UserEvaluate.Where(x => (x.CreatedDate.Day >= from.Day && x.CreatedDate.Month == from.Month && x.CreatedDate.Year == from.Year) && (x.CreatedDate.Day <= to.Day && x.CreatedDate.Month == to.Month && x.CreatedDate.Year == to.Year) && x.UserId == userId).ToList();
                    if (danhgia.Count > 0)
                    {
                        obj.tc1 = danhgia.Where(x => x.Score == "1_1").Count();
                        obj.tc2 = danhgia.Where(x => x.Score == "1_2").Count();
                        obj.tc3 = danhgia.Where(x => x.Score == "1_3").Count();
                    }
                    report.Add(obj);
                }
                else
                {
                    var danhgia = db.Q_UserEvaluate.Where(x => (x.CreatedDate.Day >= from.Day && x.CreatedDate.Month == from.Month && x.CreatedDate.Year == from.Year) && (x.CreatedDate.Day <= to.Day && x.CreatedDate.Month == to.Month && x.CreatedDate.Year == to.Year)).ToList();
                    report.AddRange(db.Q_User.Select(x => new ReportEvaluateModel() { UserId = x.Id, Name = x.Name, tc1 = 0, tc2 = 0, tc3 = 0 }));

                    if (danhgia.Count > 0)
                    {
                        foreach (var item in report)
                        {
                            var objs = danhgia.Where(x => x.UserId == item.UserId).ToList();
                            if (objs.Count > 0)
                            {
                                item.tc1 = objs.Where(x => x.Score == "1_1").Count();
                                item.tc2 = objs.Where(x => x.Score == "1_2").Count();
                                item.tc3 = objs.Where(x => x.Score == "1_3").Count();
                            }
                        }
                    }
                }
                return report;
            }
        }

        public SendSMSModel GetRequireSendSMS()
        {
            using (var db = new QMSSystemEntities())
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
    }

    public class ReportEvaluateModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public int tc1 { get; set; }
        public int tc2 { get; set; }
        public int tc3 { get; set; }
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
}
