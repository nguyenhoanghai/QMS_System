using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Data.Entity.Core.Objects; 

namespace QMS_System.Data.BLL
{
    public class BLLR_DetailByTimeRange
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLR_DetailByTimeRange _Instance;  //volatile =>  tranh dung thread
        public static BLLR_DetailByTimeRange Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLR_DetailByTimeRange();

                return _Instance;
            }
        }
        private BLLR_DetailByTimeRange() { }
        #endregion
        public List<R_DetailByTimeRangeModel> GetDetailByTimeRangeUser(int userId, DateTime? fromdate, DateTime? todate)
        {
            using (db = new QMSSystemEntities())
            {
                if (fromdate != null)
                    fromdate = fromdate.Value.Date;
                if (todate != null)
                    todate = todate.Value.Date;
                List<R_DetailByTimeRangeModel> list = null;
                try
                {
                    if (userId == 0)
                    {
                        if (fromdate == null && todate != null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                UserId = x.UserId,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                MajorName = x.Q_Major.Name,
                             //   EquipmentName = x.Q_Equipment.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.UserId).ToList();
                        }
                        else if (fromdate != null && todate == null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                UserId = x.UserId,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                MajorName = x.Q_Major.Name,
                               // EquipmentName = x.Q_Equipment.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.UserId).ToList();
                        }
                        else
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                UserId = x.UserId,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                MajorName = x.Q_Major.Name,
                              //  EquipmentName = x.Q_Equipment.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.UserId).ToList();
                        }
                    }
                    else // nếu UserId # 0
                    {

                        if (fromdate == null && todate != null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.UserId == userId && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                UserId = x.UserId,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                MajorName = x.Q_Major.Name,
                               // EquipmentName = x.Q_Equipment.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).ToList();
                        }
                        else if (fromdate != null && todate == null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.UserId == userId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                UserId = x.UserId,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                MajorName = x.Q_Major.Name,
                              //  EquipmentName = x.Q_Equipment.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).ToList();
                        }
                        else
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.UserId == userId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                UserId = x.UserId,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                MajorName = x.Q_Major.Name,
                             //   EquipmentName = x.Q_Equipment.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).ToList();
                        }
                    }
                    if (list.Count > 0)
                    {
                        int i = 1;
                        foreach (var item in list)
                        {
                            item.Index = i++;
                            item.TotalTransTime = (item.ProcessTime.HasValue && item.EndProcessTime.HasValue) ? item.EndProcessTime.Value.Subtract(item.ProcessTime.Value) : (TimeSpan?)null;
                        }
                    }
                    return list.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<R_DetailByTimeRangeModel> GetDetailByTimeRangeMajor(int majorId, DateTime? fromdate, DateTime? todate)
        {
            using (db = new QMSSystemEntities())
            {
                if (fromdate != null)
                    fromdate = fromdate.Value.Date;
                if (todate != null)
                    todate = todate.Value.Date;
                List<R_DetailByTimeRangeModel> list = null;
                try
                {
                    if (majorId == 0)
                    {
                        if (fromdate == null && todate != null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                MajorId = x.MajorId,
                                UserId = x.UserId,
                                MajorName = x.Q_Major.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                              //  CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.MajorId).ThenBy(x => x.UserId).ToList();
                        }
                        else if (fromdate != null && todate == null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                MajorId = x.MajorId,
                                UserId = x.UserId,
                                MajorName = x.Q_Major.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                               // CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.MajorId).ThenBy(x => x.UserId).ToList();
                        }
                        else // fromdate != null && todate != null
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                MajorId = x.MajorId,
                                UserId = x.UserId,
                                MajorName = x.Q_Major.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                              //  CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.MajorId).ThenBy(x => x.UserId).ToList();
                        }
                    }
                    else
                    {
                        if (fromdate == null && todate != null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.MajorId == majorId && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                MajorId = x.MajorId,
                                UserId = x.UserId,
                                MajorName = x.Q_Major.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                               // CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.UserId).ToList();
                        }
                        else if (fromdate != null && todate == null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.MajorId == majorId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                MajorId = x.MajorId,
                                UserId = x.UserId,
                                MajorName = x.Q_Major.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                               // CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.UserId).ToList();
                        }
                        else
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.MajorId == majorId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                MajorId = x.MajorId,
                                UserId = x.UserId,
                                MajorName = x.Q_Major.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                              //  CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.UserId).ToList();
                        }
                    }
                    if (list.Count > 0)
                    {
                        int i = 1;
                        foreach (var item in list)
                        {
                            item.Index = i++;
                            item.TotalTransTime = (item.ProcessTime.HasValue && item.EndProcessTime.HasValue) ? item.EndProcessTime.Value.Subtract(item.ProcessTime.Value) : (TimeSpan?)null;
                        }
                    }
                    return list.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<R_DetailByTimeRangeModel> GetDetailByTimeRangeService(int serviceId, DateTime? fromdate, DateTime? todate)
        {
            using (db = new QMSSystemEntities())
            {
                if (fromdate != null)
                    fromdate = fromdate.Value.Date;
                if (todate != null)
                    todate = todate.Value.Date;
                List<R_DetailByTimeRangeModel> list = null;
                try
                {
                    if (serviceId == 0)
                    {
                        if (fromdate == null && todate != null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                ServiceId = x.Q_DailyRequire.Q_Service.Id,
                                ServiceName = x.Q_DailyRequire.Q_Service.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                               // CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                StatusCode = x.Q_Status.Code
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.ServiceId).ToList();
                        }
                        else if (fromdate != null && todate == null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                ServiceId = x.Q_DailyRequire.Q_Service.Id,
                                ServiceName = x.Q_DailyRequire.Q_Service.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                               // CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                StatusCode = x.Q_Status.Code
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.ServiceId).ToList();
                        }
                        else
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                ServiceId = x.Q_DailyRequire.Q_Service.Id,
                                ServiceName = x.Q_DailyRequire.Q_Service.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                             //   CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                ProcessTime = x.ProcessTime,
                                EndProcessTime = x.EndProcessTime,
                                StatusCode = x.Q_Status.Code
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                            }).OrderBy(x => x.ServiceId).ToList();
                        }

                    }
                    else // serviceId !=0
                    {
                        if (fromdate == null && todate != null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.ServiceId == serviceId && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                ServiceId = x.Q_DailyRequire.Q_Service.Id,
                                ServiceName = x.Q_DailyRequire.Q_Service.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                                //CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                //StartTransTime = x.StartTransTime,
                                //EndTransTime = x.EndTransTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                                StatusCode = x.Q_Status.Code
                            }).ToList();
                        }
                        else if (fromdate != null && todate == null)
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.ServiceId == serviceId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                ServiceId = x.Q_DailyRequire.Q_Service.Id,
                                ServiceName = x.Q_DailyRequire.Q_Service.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                                //CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                //StartTransTime = x.StartTransTime,
                                //EndTransTime = x.EndTransTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                                StatusCode = x.Q_Status.Code
                            }).ToList();
                        }
                        else
                        {
                            list = db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.ServiceId == serviceId && EntityFunctions.TruncateTime(x.ProcessTime.Value) >= fromdate && EntityFunctions.TruncateTime(x.ProcessTime.Value) <= todate).Select(x => new R_DetailByTimeRangeModel()
                            {
                                //Index = i,
                                ServiceId = x.Q_DailyRequire.Q_Service.Id,
                                ServiceName = x.Q_DailyRequire.Q_Service.Name,
                                UserName = x.Q_User.Name,
                                TicketNumber = x.Q_DailyRequire.TicketNumber,
                                //EquipmentName = x.Q_Equipment.Name,
                                //CounterName = x.Q_Equipment.Q_Counter.Name,
                                PrintTime = x.Q_DailyRequire.PrintTime,
                                //StartTransTime = x.StartTransTime,
                                //EndTransTime = x.EndTransTime,
                                //TotalTransTime = x.EndTransTime.Value.Subtract(x.StartTransTime.Value),
                                StatusCode = x.Q_Status.Code
                            }).ToList();
                        }
                    }
                    if (list.Count > 0)
                    {
                        int i = 1;
                        foreach (var item in list)
                        {
                            item.Index = i++;
                            //item.TotalTransTime = item.EndTransTime.HasValue ? item.EndTransTime.Value.Subtract(item.StartTransTime.Value) : (TimeSpan?)null;
                        }
                    }
                    return list.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
