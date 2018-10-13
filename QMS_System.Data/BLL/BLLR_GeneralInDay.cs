using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 
using QMS_System.Data.Enum;
using System.Data.Entity.Core.Objects;

namespace QMS_System.Data.BLL
{
    public class BLLR_GeneralInDay
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLR_GeneralInDay _Instance;  //volatile =>  tranh dung thread
        public static BLLR_GeneralInDay Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLR_GeneralInDay();

                return _Instance;
            }
        }
        private BLLR_GeneralInDay() { }
        #endregion

        public List<R_GeneralInDayModel> GetGeneralInDayByUser(int userId)
        {
            using (db = new QMSSystemEntities())
            {
                List<R_GeneralInDayModel> Ilist = new List<R_GeneralInDayModel>();
                var today = DateTime.Now.Date;
                //if (userId == 0)
                //{

                //    Ilist = db.Q_DailyRequire_Detail.Where(x =>!x.Q_User.IsDeleted && !x.Q_Major.IsDeleted   && !x.Q_DailyRequire.Q_Service.IsDeleted&& EntityFunctions.TruncateTime(x.ProcessTime.Value) == today && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralInDayModel()
                //     {
                //         UserId = t.Key,
                //         TotalTransaction = t.Count(),
                //         TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                //         UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                //     }).ToList();
                //}
                //else
                //{
                //    Ilist = db.Q_DailyRequire_Detail.Where(x =>!x.Q_User.IsDeleted && !x.Q_Major.IsDeleted  && !x.Q_DailyRequire.Q_Service.IsDeleted&& x.UserId == userId && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralInDayModel()
                //    {
                //        UserId = t.Key,
                //        UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                //        TotalTransaction = t.Count(),
                //        TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                //    }).ToList();
                //}

                if (Ilist.Count > 0)
                {
                    int i = 1;
                    foreach (var item in Ilist)
                    {
                        item.Index = i++;
                        //item.AverageTimePerTrans = Math.Round((double)((item.TotalTransTime.HasValue && item.TotalTransaction != null) ? item.TotalTransTime / item.TotalTransaction : null), 2, MidpointRounding.AwayFromZero);
                        item.AverageTimePerTrans = (item.TotalTransTime.HasValue && item.TotalTransaction != null) ? Math.Round((double)(item.TotalTransTime / item.TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0;
                    }
                }
                return Ilist;
            }
        }

        public List<R_GeneralInDayModel> GetGeneralInDayByMajor(int majorId)
        {
            using (db = new QMSSystemEntities())
            {
                List<R_GeneralInDayModel> Ilist = new List<R_GeneralInDayModel>();
                var today = DateTime.Now.Date;
                //if (majorId == 0)
                //{

                //    Ilist = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted   && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralInDayModel()
                //     {
                //         MajorId = t.Key,
                //         MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                //         TotalTransaction = t.Count(),
                //         TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                //     }).ToList();
                //}
                //else
                //{
                //    Ilist = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted  && !x.Q_DailyRequire.Q_Service.IsDeleted && x.MajorId == majorId && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralInDayModel()
                //    {
                //        MajorId = t.Key,
                //        MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                //        TotalTransaction = t.Count(),
                //        TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                //    }).ToList();
                //}

                if (Ilist.Count > 0)
                {
                    int i = 1;
                    foreach (var item in Ilist)
                    {
                        item.Index = i++;
                        item.AverageTimePerTrans = (item.TotalTransTime.HasValue && item.TotalTransaction != null) ? Math.Round((double)(item.TotalTransTime / item.TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0;
                    }
                }
                return Ilist;
            }
        }

        public List<R_GeneralInDayModel> GetGeneralInDayByService(int serviceId)
        {
            using (db = new QMSSystemEntities())
            {
                List<R_GeneralInDayModel> Ilist = new List<R_GeneralInDayModel>();
                var today = DateTime.Now.Date;
                //if (serviceId == 0)
                //{

                //    Ilist = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralInDayModel()
                //    {
                //        ServiceId = t.Key,
                //        ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                //        TotalTransaction = t.Count(),
                //        TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                //    }).ToList();
                //}
                //else
                //{
                //    Ilist = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.Q_Service.Id == serviceId && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralInDayModel()
                //    {
                //        ServiceId = t.Key,
                //        ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                //        TotalTransaction = t.Count(),
                //        TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                //    }).ToList();
                //}

                if (Ilist.Count > 0)
                {
                    int i = 1;
                    foreach (var item in Ilist)
                    {
                        item.Index = i++;
                        item.AverageTimePerTrans = (item.TotalTransTime.HasValue && item.TotalTransaction != null) ? Math.Round((double)(item.TotalTransTime / (double)item.TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0;
                    }
                }
                return Ilist;
            }
        }
    }
}
