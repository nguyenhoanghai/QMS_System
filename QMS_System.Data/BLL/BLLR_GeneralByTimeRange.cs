using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 
using QMS_System.Data.Enum;
using System.Data.Entity.Core.Objects;

namespace QMS_System.Data.BLL
{
    public class BLLR_GeneralByTimeRange
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLR_GeneralByTimeRange _Instance;  //volatile =>  tranh dung thread
        public static BLLR_GeneralByTimeRange Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLR_GeneralByTimeRange();

                return _Instance;
            }
        }
        private BLLR_GeneralByTimeRange() { }
        #endregion

        public List<R_GeneralByTimeRangeModel> GetGeneralByTimeRangeUser(int userId, DateTime? fromdate, DateTime? todate)
        {
            using (db = new QMSSystemEntities())
            {
                if (fromdate != null)
                    fromdate = fromdate.Value.Date;
                if (todate != null)
                    todate = todate.Value.Date;
                List<R_GeneralByTimeRangeModel> Ilist = new List<R_GeneralByTimeRangeModel>();
                if (userId == 0)
                {

                    if (fromdate == null && todate != null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            UserId = t.Key,
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                            UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                        }).ToList();
                    }
                    else if (fromdate != null && todate == null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            UserId = t.Key,
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                            UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                        }).ToList();
                    }
                    else
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            UserId = t.Key,
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                            UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                        }).ToList();
                    }
                }
                else // nếu userId != 0
                {
                    if (fromdate == null && todate != null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.UserId == userId && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            UserId = t.Key,
                            UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else if (fromdate != null && todate == null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.UserId == userId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            UserId = t.Key,
                            UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.UserId == userId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.UserId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            UserId = t.Key,
                            UserName = t.Select(x => x.Q_User.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                }

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


        public List<R_GeneralByTimeRangeModel> GetGeneralByTimeRangeMajor(int majorId, DateTime? fromdate, DateTime? todate)
        {
            using (db = new QMSSystemEntities())
            {
                if (fromdate != null)
                    fromdate = fromdate.Value.Date;
                if (todate != null)
                    todate = todate.Value.Date;
                List<R_GeneralByTimeRangeModel> Ilist = new List<R_GeneralByTimeRangeModel>();
                if (majorId == 0)
                {

                    if (fromdate == null && todate != null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            MajorId = t.Key,
                            MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else if (fromdate != null && todate == null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            MajorId = t.Key,
                            MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            MajorId = t.Key,
                            MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                }
                else  // nếu majorId =! 0
                {
                    if (fromdate == null && todate != null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.MajorId == majorId && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            MajorId = t.Key,
                            MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else if (fromdate != null && todate == null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.MajorId == majorId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            MajorId = t.Key,
                            MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.MajorId == majorId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.MajorId).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            MajorId = t.Key,
                            MajorName = t.Select(x => x.Q_Major.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                }

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

        public List<R_GeneralByTimeRangeModel> GetGeneralByTimeRangeService(int serviceId, DateTime? fromdate, DateTime? todate)
        {
            using (db = new QMSSystemEntities())
            {
                if (fromdate != null)
                    fromdate = fromdate.Value.Date;
                if (todate != null)
                    todate = todate.Value.Date;
                List<R_GeneralByTimeRangeModel> Ilist = new List<R_GeneralByTimeRangeModel>();
                if (serviceId == 0)
                {

                    if (fromdate == null && todate != null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            ServiceId = t.Key,
                            ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else if (fromdate != null && todate == null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            ServiceId = t.Key,
                            ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            ServiceId = t.Key,
                            ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                }
                else
                {
                    if (fromdate == null && todate != null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.Q_Service.Id == serviceId && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            ServiceId = t.Key,
                            ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else if (fromdate != null && todate == null)
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.Q_Service.Id == serviceId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            ServiceId = t.Key,
                            ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                    else
                    {
                        Ilist = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.Q_Service.Id == serviceId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate && x.Q_Status.Id == (int)eStatus.HOTAT).GroupBy(x => x.Q_DailyRequire.Q_Service.Id).Select(t => new R_GeneralByTimeRangeModel()
                        {
                            ServiceId = t.Key,
                            ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                            TotalTransaction = t.Count(),
                            TotalTransTime = t.Sum(p => p.EndProcessTime.Value.Minute + (p.EndProcessTime.Value.Hour - p.ProcessTime.Value.Hour) * 60 - p.ProcessTime.Value.Minute),
                        }).ToList();
                    }
                }


                if (Ilist.Count > 0)
                {
                    int i = 1;
                    foreach (var item in Ilist)
                    {
                        item.Index = i++;
                        item.AverageTimePerTrans = (item.TotalTransTime != null && item.TotalTransaction != null) ? Math.Round((double)(item.TotalTransTime / (double)item.TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0;
                        //item.TotalTransTime = item.TotalTransTime != null ? item.TotalTransTime : 0;
                    }
                }
                return Ilist;
            }
        }
    }
}
