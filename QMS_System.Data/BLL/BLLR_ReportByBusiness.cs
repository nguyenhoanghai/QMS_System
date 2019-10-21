using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLR_ReportByBusiness
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLR_ReportByBusiness _Instance;  //volatile =>  tranh dung thread
        public static BLLR_ReportByBusiness Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLR_ReportByBusiness();

                return _Instance;
            }
        }
        private BLLR_ReportByBusiness() { }
        #endregion

        public List<R_ReportByBusinessModel> GetLongestWaitingBusiness(string connectString, int num) // Khách hàng có thời gian chờ lâu nhất
        {
            using (db = new QMSSystemEntities(connectString))
            {
                List<R_ReportByBusinessModel> list = null;
                var today = DateTime.Now.Date; // chi lay phan ngay thang nam ko lay gio phut giay
                try
                {
                    //Tao 1 list chua danh sach khach hang da giao dich xong hoac dang giao dich
                    //Tao 1 list khach hang van dang doi
                    // Them 2 list tren vao mot list thu 3
                    // Truy van dieu kien tren list thu 3 chon ra cac khach hang co thoi gian doi lau nhat
                    list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.ProcessTime.HasValue && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today).Select(x => new R_ReportByBusinessModel()
                    {
                        //Index = i,
                        UserId = x.UserId,
                        MajorId = x.MajorId,
                        ServiceId = x.Q_DailyRequire.ServiceId,
                        UserName = x.Q_User.Name,
                        TicketNumber = x.Q_DailyRequire.TicketNumber,
                        MajorName = x.Q_Major.Name,
                        //EquipmentName = x.Q_Equipment.Name,
                        PrintTime = x.Q_DailyRequire.PrintTime,
                        ProcessTime = x.ProcessTime,
                        EndProcessTime = x.EndProcessTime,
                        //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                    }).OrderByDescending(x => EntityFunctions.DiffSeconds(x.PrintTime.Value, x.ProcessTime.Value)).Take(num).ToList();


                    if (list.Count > 0)
                    {
                        int i = 1;
                        foreach (var item in list)
                        {
                            item.Index = i++;
                            item.TotalWaitingTime = (item.PrintTime.HasValue && item.ProcessTime.HasValue) ? item.ProcessTime.Value.Subtract(item.PrintTime.Value) : (TimeSpan?)null;
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public List<R_ReportByBusinessModel> GetLongestTransBusiness(string connectString, int num) // Lay n khach hang co thoi gian giao dich lau nhat
        {
            using (db = new QMSSystemEntities(connectString))
            {
                List<R_ReportByBusinessModel> list = null;
                var today = DateTime.Now.Date; // chi lay phan ngay thang nam ko lay gio phut giay
                //var today = DateTime.Now.AddDays(-1).Date; // Lay ngay truoc do 1 ngay
                try
                {

                    list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.ProcessTime.HasValue && x.EndProcessTime.HasValue && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today).Select(x => new R_ReportByBusinessModel()
                    {
                        //Index = i,
                        UserId = x.UserId,
                        MajorId = x.MajorId,
                        ServiceId = x.Q_DailyRequire.ServiceId,
                        UserName = x.Q_User.Name,
                        TicketNumber = x.Q_DailyRequire.TicketNumber,
                        MajorName = x.Q_Major.Name,
                      //  CounterName = x.Q_Equipment.Q_Counter.Name,
                        PrintTime = x.Q_DailyRequire.PrintTime,
                        ProcessTime = x.ProcessTime,
                        EndProcessTime = x.EndProcessTime,
                        //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                    }).OrderByDescending(x => EntityFunctions.DiffSeconds(x.ProcessTime.Value, x.EndProcessTime.Value)).Take(num).ToList();


                    if (list.Count > 0)
                    {
                        int i = 1;
                        foreach (var item in list)
                        {
                            item.Index = i++;
                            item.TotalTransTime = (item.ProcessTime.HasValue && item.EndProcessTime.HasValue) ? item.EndProcessTime.Value.Subtract(item.ProcessTime.Value) : (TimeSpan?)null;
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<R_ReportByBusinessModel> GetInProgressTrans(string connectString)  // Số khách hàng đang giao dịch
        {
            using (db = new QMSSystemEntities(connectString))
            {
                List<R_ReportByBusinessModel> list = null;
                var today = DateTime.Now.Date; // chi lay phan ngay thang nam ko lay gio phut giay
                //var today = DateTime.Now.AddDays(-1).Date; // Lay ngay truoc do 1 ngay
                try
                {

                    list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.StatusId != null && x.Q_Status.Id == (int)eStatus.DAGXL && EntityFunctions.TruncateTime(x.ProcessTime.Value) == today).GroupBy(x => x.Q_DailyRequire.ServiceId).Select(t => new R_ReportByBusinessModel()
                    {
                        ServiceId = t.Key,
                        ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                        TotalInProgressTrans = t.Count()
                    }).OrderBy(x => x.ServiceId).ToList();

                    if (list.Count > 0)
                    {
                        int i = 1;
                        foreach (var item in list)
                        {
                            item.Index = i++;
                            //item.TotalTransTime = (item.StartTransTime.HasValue && item.EndTransTime.HasValue) ? item.EndTransTime.Value.Subtract(item.StartTransTime.Value) : (TimeSpan?)null;
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<R_ReportByBusinessModel> GetWaitingTransInternal(string connectString)  // Số khách hàng đang chờ nội bộ
        {
            using (db = new QMSSystemEntities(connectString))
            {
                List<R_ReportByBusinessModel> list = null;
                var today = DateTime.Now.Date; // chi lay phan ngay thang nam ko lay gio phut giay
                //var today = DateTime.Now.AddDays(-1).Date; // Lay ngay truoc do 1 ngay
                try
                {

                    list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.StatusId != null && x.Q_DailyRequire.PrintTime != null && x.Q_Status.Id == (int)eStatus.CHOXL /*&& điều kiện truy vấn nội bộ là gì*/ && EntityFunctions.TruncateTime(x.Q_DailyRequire.PrintTime) == today).GroupBy(x => x.Q_DailyRequire.ServiceId).Select(t => new R_ReportByBusinessModel()
                    {
                        ServiceId = t.Key,
                        ServiceName = t.Select(x => x.Q_DailyRequire.Q_Service.Name).FirstOrDefault(),
                        TotalWaitingTransInternal = t.Count()
                    }).OrderBy(x => x.ServiceId).ToList();

                    if (list.Count > 0)
                    {
                        int i = 1;
                        foreach (var item in list)
                        {
                            item.Index = i++;
                            //item.TotalTransTime = (item.StartTransTime.HasValue && item.EndTransTime.HasValue) ? item.EndTransTime.Value.Subtract(item.StartTransTime.Value) : (TimeSpan?)null;
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
