using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLReport
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLReport _Instance;
        public static BLLReport Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLReport();

                return _Instance;
            }
        }
        private BLLReport() { }
        #endregion

        public List<ReportModel> DetailReport(string connectString, int objId, int typeOfSearch, DateTime from, DateTime to)
        {
            List<ReportModel> list = new List<ReportModel>();
            using (db = new QMSSystemEntities(connectString))
            {

                try
                {
                    if (typeOfSearch == 3)
                    {
                        #region  theo dich vu
                        //yesterday
                        list.AddRange(db.Q_HisDailyRequire.Where(x => !x.Q_Service.IsDeleted && x.PrintTime >= from && x.PrintTime <= to).OrderBy(x => x.TicketNumber).Select(x => new ReportModel()
                        {
                            Number = x.TicketNumber,
                            ServiceName = x.Q_Service.Name,
                            PrintTime = x.PrintTime,
                            Id = x.Id,
                            ServiceId = x.ServiceId
                        }).ToList());

                        var detais = db.Q_HisDailyRequire_De.Where(x => !x.Q_Major.IsDeleted && !x.Q_HisDailyRequire.Q_Service.IsDeleted && x.Q_HisDailyRequire.PrintTime >= from && x.Q_HisDailyRequire.PrintTime <= to).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            var obj = detais.FirstOrDefault(x => x.HisDailyRequireId == list[i].Id && x.StatusId == (int)eStatus.CHOXL);
                            list[i].StatusName = (obj != null ? "Đang xử lý" : "Hoàn tất");
                        }

                        //today

                        var todays = db.Q_DailyRequire.Where(x => !x.Q_Service.IsDeleted && x.PrintTime >= from && x.PrintTime <= to).OrderBy(x => x.TicketNumber).Select(x => new ReportModel()
                        {
                            Number = x.TicketNumber,
                            ServiceName = x.Q_Service.Name,
                            PrintTime = x.PrintTime,
                            Id = x.Id
                        }).ToList();
                        if (todays.Count > 0)
                        {
                            var tddetais = db.Q_DailyRequire_Detail.Where(x => !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.PrintTime >= from && x.Q_DailyRequire.PrintTime <= to).ToList();
                            for (int i = 0; i < todays.Count; i++)
                            {
                                todays[i].stt = (i + 1);
                                var obj = tddetais.FirstOrDefault(x => x.DailyRequireId == todays[i].Id && x.StatusId == (int)eStatus.CHOXL);
                                todays[i].StatusName = (obj != null ? "Đang xử lý" : "Hoàn tất");
                            }
                            list.AddRange(todays);
                        }

                        if (list.Count > 0)
                        {
                            if (objId > 0)
                                list = list.Where(x => x.ServiceId == objId).ToList();
                            list = list.OrderBy(x => x.PrintTime).ThenBy(x => x.Number).ToList();
                            for (int i = 0; i < list.Count; i++)
                                list[i].stt = i + 1;
                        }

                        #endregion
                    }
                    else
                    {
                        #region
                        //yesterday
                        list = db.Q_HisDailyRequire_De.Where(x => !x.Q_Major.IsDeleted && !x.Q_HisDailyRequire.Q_Service.IsDeleted && x.Q_HisDailyRequire.PrintTime >= from && x.Q_HisDailyRequire.PrintTime <= to).OrderBy(x => x.Q_HisDailyRequire.TicketNumber).Select(x => new ReportModel()
                        {
                            UserName = x.Q_User.Name,
                            Number = x.Q_HisDailyRequire.TicketNumber,
                            MajorName = x.Q_Major.Name,
                            ServiceName = x.Q_HisDailyRequire.Q_Service.Name,
                            PrintTime = x.Q_HisDailyRequire.PrintTime,
                            Start = x.ProcessTime,
                            End = x.EndProcessTime,
                            StatusName = x.Q_Status.Note,
                            UserId = x.UserId,
                            MajorId = x.MajorId,
                            ServiceId = x.Q_HisDailyRequire.ServiceId,
                        }).ToList();

                        list.AddRange(db.Q_DailyRequire_Detail.Where(x => !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.PrintTime >= from && x.Q_DailyRequire.PrintTime <= to).OrderBy(x => x.Q_DailyRequire.TicketNumber).Select(x => new ReportModel()
                        {
                            UserName = x.Q_User.Name,
                            Number = x.Q_DailyRequire.TicketNumber,
                            MajorName = x.Q_Major.Name,
                            ServiceName = x.Q_DailyRequire.Q_Service.Name,
                            PrintTime = x.Q_DailyRequire.PrintTime,
                            Start = x.ProcessTime,
                            End = x.EndProcessTime,
                            StatusName = x.Q_Status.Note,
                            UserId = x.UserId,
                            MajorId = x.MajorId,
                            ServiceId = x.Q_DailyRequire.ServiceId,
                        }).ToList());

                        if (objId > 0)
                            switch (typeOfSearch)
                            {
                                case 1: list = list.Where(x => x.UserId == objId).ToList(); break;
                                case 2: list = list.Where(x => x.MajorId == objId).ToList(); break;
                            }

                        if (list.Count > 0)
                        {
                            list = list.OrderBy(x => x.PrintTime).ThenBy(x => x.Number).ToList();
                            for (int i = 0; i < list.Count; i++)
                            {
                                list[i].stt = (i + 1);
                                if (list[i].Start.HasValue && list[i].End.HasValue)
                                    list[i].ProcessTime = list[i].End.Value.Subtract(list[i].Start.Value).ToString("hh\\:mm");
                                if (list[i].Start.HasValue)
                                    list[i].WaitingTime = list[i].Start.Value.Subtract(list[i].PrintTime).ToString("hh\\:mm");
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                { }
                return list;
            }
        }

        public List<ReportModel> DetailReport(SqlConnection sqlConnection, int objId, int typeOfSearch, DateTime from, DateTime to)
        {
            List<ReportModel> list = new List<ReportModel>();
            string query = "";
            if (typeOfSearch == 3)
            {
                //yesterday 
                query = "select dr.TicketNumber, s.Name as ServiceName , s.Id as ServiceId, dr.Id, dr.PrintTime from Q_HisDailyRequire dr,Q_Service s where s.IsDeleted=0 and dr.PrintTime >='" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' order by dr.TicketNumber";
                var adap = new SqlDataAdapter(query, sqlConnection);
                var dt = new DataTable();
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new ReportModel()
                        {
                            Id = getIntValue(row["Id"]),
                            Number = getIntValue(row["TicketNumber"]),
                            ServiceName = getStringValue(row["ServiceName"]),
                            PrintTime = getDateValue(row["PrintTime"]).Value,
                            ServiceId = getIntValue(row["ServiceId"])
                        });
                    }

                    var detais = new List<ModelSelectItem>();
                    query = "select dd.HisDailyRequireId, dd.StatusId from Q_HisDailyRequire dr, Q_HisDailyRequire_De dd where dr.Id = dd.HisDailyRequireId and dr.PrintTime >= '" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    adap = new SqlDataAdapter(query, sqlConnection);
                    dt.Clear();
                    adap.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            detais.Add(new ModelSelectItem()
                            {
                                Id = getIntValue(row["HisDailyRequireId"]),
                                Data = getIntValue(row["StatusId"])
                            });
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        var obj = detais.FirstOrDefault(x => x.Id == list[i].Id && x.Data == (int)eStatus.CHOXL);
                        list[i].StatusName = (obj != null ? "Đang xử lý" : "Hoàn tất");
                    }

                }

                //today
                if (to >= DateTime.Now)
                {
                    var todays = new List<ReportModel>();
                    query = "select dr.TicketNumber, s.Name as ServiceName , s.Id as ServiceId, dr.Id, dr.PrintTime from Q_DailyRequire dr,Q_Service s where s.IsDeleted=0 and dr.PrintTime >='" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' order by dr.TicketNumber";
                    adap = new SqlDataAdapter(query, sqlConnection);
                    dt.Clear();
                    adap.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            todays.Add(new ReportModel()
                            {
                                Id = getIntValue(row["Id"]),
                                Number = getIntValue(row["TicketNumber"]),
                                ServiceName = getStringValue(row["ServiceName"]),
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                ServiceId = getIntValue(row["ServiceId"])
                            });
                        }

                        var tddetais = new List<ModelSelectItem>();
                        query = "select dd.DailyRequireId, dd.StatusId from Q_DailyRequire dr, Q_DailyRequire_Detail dd where dr.Id = dd.DailyRequireId and dr.PrintTime >= '" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        adap = new SqlDataAdapter(query, sqlConnection);
                        dt.Clear();
                        adap.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                tddetais.Add(new ModelSelectItem()
                                {
                                    Id = getIntValue(row["HisDailyRequireId"]),
                                    Data = getIntValue(row["StatusId"])
                                });
                            }
                        }
                        for (int i = 0; i < todays.Count; i++)
                        {
                            todays[i].stt = (i + 1);
                            var obj = tddetais.FirstOrDefault(x => x.Id == todays[i].Id && x.Data == (int)eStatus.CHOXL);
                            todays[i].StatusName = (obj != null ? "Đang xử lý" : "Hoàn tất");
                        }
                        list.AddRange(todays);
                    }
                }
                if (list.Count > 0)
                {
                    if (objId > 0)
                        list = list.Where(x => x.ServiceId == objId).ToList();
                    list = list.OrderBy(x => x.PrintTime).ThenBy(x => x.Number).ToList();
                    for (int i = 0; i < list.Count; i++)
                        list[i].stt = i + 1;
                }
            }
            else
            {
                //yesterday
                query = "select u.Name as UserName, u.Id as UserId, m.Id as MajorId,  m.Name as MajorName ,dr.TicketNumber, s.Name as ServiceName , s.Id as ServiceId, dr.Id, dr.PrintTime, dd.ProcessTime,dd.EndProcessTime, sta.Note as StatusName from Q_HisDailyRequire dr, Q_HisDailyRequire_De dd, Q_Service s, Q_Major m, Q_User u,Q_Status sta where s.IsDeleted = 0 and dd.HisDailyRequireId = dr.Id and m.Id = dd.MajorId and u.Id = dd.UserId and sta.Id = dd.StatusId and dr.PrintTime >= '" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <= '" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' order by dr.TicketNumber";
                var adap = new SqlDataAdapter(query, sqlConnection);
                var dt = new DataTable();
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new ReportModel()
                        {
                            UserId = getIntValue(row["UserId"]),
                            UserName = getStringValue(row["UserName"]),
                            Number = getIntValue(row["TicketNumber"]),
                            MajorId = getIntValue(row["MajorId"]),
                            MajorName = getStringValue(row["MajorName"]),
                            ServiceId = getIntValue(row["ServiceId"]),
                            ServiceName = getStringValue(row["ServiceName"]),
                            PrintTime = getDateValue(row["PrintTime"]).Value,
                            Start = getDateValue(row["ProcessTime"]),
                            End = getDateValue(row["EndProcessTime"]),
                            StatusName = getStringValue(row["StatusName"])
                        });
                    }
                }
                //today
                query = "select u.Name as UserName, u.Id as UserId, m.Id as MajorId,  m.Name as MajorName ,dr.TicketNumber, s.Name as ServiceName , s.Id as ServiceId, dr.Id, dr.PrintTime, dd.ProcessTime,dd.EndProcessTime, sta.Note as StatusName from Q_DailyRequire dr, Q_DailyRequire_Detail dd, Q_Service s, Q_Major m, Q_User u,Q_Status sta where s.IsDeleted = 0 and dd.DailyRequireId = dr.Id and m.Id = dd.MajorId and u.Id = dd.UserId and sta.Id = dd.StatusId and dr.PrintTime >= '" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime <= '" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' order by dr.TicketNumber";
                adap = new SqlDataAdapter(query, sqlConnection);
                dt.Clear();
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new ReportModel()
                        {
                            UserId = getIntValue(row["UserId"]),
                            UserName = getStringValue(row["UserName"]),
                            Number = getIntValue(row["TicketNumber"]),
                            MajorId = getIntValue(row["MajorId"]),
                            MajorName = getStringValue(row["MajorName"]),
                            ServiceId = getIntValue(row["ServiceId"]),
                            ServiceName = getStringValue(row["ServiceName"]),
                            PrintTime = getDateValue(row["PrintTime"]).Value,
                            Start = getDateValue(row["ProcessTime"]),
                            End = getDateValue(row["EndProcessTime"]),
                            StatusName = getStringValue(row["StatusName"])
                        });
                    }
                }
                if (objId > 0)
                    switch (typeOfSearch)
                    {
                        case 1: list = list.Where(x => x.UserId == objId).ToList(); break;
                        case 2: list = list.Where(x => x.MajorId == objId).ToList(); break;
                    }

                if (list.Count > 0)
                {
                    list = list.OrderBy(x => x.PrintTime).ThenBy(x => x.Number).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].stt = (i + 1);
                        if (list[i].Start.HasValue && list[i].End.HasValue)
                            list[i].ProcessTime = list[i].End.Value.Subtract(list[i].Start.Value).ToString("hh\\:mm");
                        if (list[i].Start.HasValue)
                            list[i].WaitingTime = list[i].Start.Value.Subtract(list[i].PrintTime).ToString("hh\\:mm");
                    }
                }

            }
            return list;
        }

        public List<R_GeneralInDayModel> GeneralReport(string connectString, int objId, int typeOfSearch, DateTime from, DateTime to)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                List<R_GeneralInDayModel> objs = null;

                try
                {
                    switch (typeOfSearch)
                    {
                        case 1:
                            #region NhanVien
                            objs = db.Q_HisDailyRequire_De.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_HisDailyRequire.Q_Service.IsDeleted && x.Q_HisDailyRequire.PrintTime >= from && x.Q_HisDailyRequire.PrintTime <= to && x.Q_Status.Id == (int)eStatus.HOTAT).Select(t => new R_GeneralInDayModel()
                            {
                                ObjectId = t.UserId ?? 0,
                                Start = t.ProcessTime,
                                End = t.EndProcessTime,
                                Name = t.Q_User.Name,
                            }).ToList();

                            objs.AddRange(db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.PrintTime >= from && x.Q_DailyRequire.PrintTime <= to && x.Q_Status.Id == (int)eStatus.HOTAT).Select(t => new R_GeneralInDayModel()
                            {
                                ObjectId = t.UserId ?? 0,
                                Start = t.ProcessTime,
                                End = t.EndProcessTime,
                                Name = t.Q_User.Name,
                            }).ToList());
                            #endregion
                            break;
                        case 2:
                            #region nghiep vu
                            objs = db.Q_HisDailyRequire_De.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_HisDailyRequire.Q_Service.IsDeleted && x.Q_HisDailyRequire.PrintTime >= from && x.Q_HisDailyRequire.PrintTime <= to && x.Q_Status.Id == (int)eStatus.HOTAT).Select(t => new R_GeneralInDayModel()
                            {
                                ObjectId = t.MajorId,
                                Start = t.ProcessTime,
                                End = t.EndProcessTime,
                                Name = t.Q_Major.Name,
                            }).ToList();

                            objs.AddRange(db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.PrintTime >= from && x.Q_DailyRequire.PrintTime <= to && x.Q_Status.Id == (int)eStatus.HOTAT).Select(t => new R_GeneralInDayModel()
                            {
                                ObjectId = t.MajorId,
                                Start = t.ProcessTime,
                                End = t.EndProcessTime,
                                Name = t.Q_Major.Name,
                            }).ToList());
                            #endregion
                            break;
                        case 3:
                            #region dich vu
                            objs = db.Q_HisDailyRequire_De.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_HisDailyRequire.Q_Service.IsDeleted && x.Q_HisDailyRequire.PrintTime >= from && x.Q_HisDailyRequire.PrintTime <= to && x.Q_Status.Id == (int)eStatus.HOTAT).Select(t => new R_GeneralInDayModel()
                            {
                                ObjectId = t.Q_HisDailyRequire.ServiceId,
                                Start = t.ProcessTime,
                                End = t.EndProcessTime,
                                Name = t.Q_HisDailyRequire.Q_Service.Name,
                            }).ToList();

                            objs.AddRange(db.Q_DailyRequire_Detail.Where(x => !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted && x.Q_DailyRequire.PrintTime >= from && x.Q_DailyRequire.PrintTime <= to && x.Q_Status.Id == (int)eStatus.HOTAT).Select(t => new R_GeneralInDayModel()
                            {
                                ObjectId = t.Q_DailyRequire.ServiceId,
                                Start = t.ProcessTime,
                                End = t.EndProcessTime,
                                Name = t.Q_DailyRequire.Q_Service.Name,
                            }).ToList());
                            #endregion
                            break;
                    }
                    if (objs.Count > 0)
                    {
                        if (objId > 0)
                            objs = objs.Where(x => x.ObjectId == objId).ToList();
                        objs = objs.GroupBy(x => x.ObjectId).Select(t => new R_GeneralInDayModel()
                        {
                            ObjectId = t.Key,
                            TotalTransaction = t.Count(),
                            // TotalTransTime = t.Sum(p =>  p.End.Value.Minute + (p.End.Value.Hour - p.Start.Value.Hour) * 60 - p.Start.Value.Minute),
                            TotalTransTime = t.Sum(p => p.End.Value.Subtract(p.Start.Value).TotalMinutes),
                            Name = t.FirstOrDefault().Name
                        }).ToList();

                        for (int i = 0; i < objs.Count; i++)
                        {
                            objs[i].Index = i + 1;
                            objs[i].AverageTimePerTrans = (objs[i].TotalTransTime != null && objs[i].TotalTransaction != null) ? Math.Round((double)(objs[i].TotalTransTime / objs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return objs;
            }
        }

        /// <summary>
        /// Thong ke tong hop tien thu
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="thuNganId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<R_GeneralInDayModel> GeneralReport_DichVuTienThu(string connectString, int objId, int thuNganId, DateTime from, DateTime to)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                List<R_GeneralInDayModel> objs = null;
                List<R_GeneralInDayModel> ThuNganObjs = null;
                List<R_GeneralInDayModel> temps = new List<R_GeneralInDayModel>();
                try
                {

                    #region history
                    objs = db.Q_HisDailyRequire_De.Where(x => !x.Q_User.IsDeleted &&
                        !x.Q_Major.IsDeleted &&
                        !x.Q_HisDailyRequire.Q_Service.IsDeleted &&
                        x.Q_HisDailyRequire.PrintTime >= from &&
                        x.Q_HisDailyRequire.PrintTime <= to &&
                        x.Q_Status.Id == (int)eStatus.HOTAT)
                        .Select(t => new R_GeneralInDayModel()
                        {
                            Id = t.HisDailyRequireId,
                            ObjectId = t.Q_HisDailyRequire.ServiceId,
                            Start = t.ProcessTime,
                            End = t.EndProcessTime,
                            Name = t.Q_HisDailyRequire.Q_Service.Name,
                            TotalTransaction = 0,
                            TotalTransTime = 0,
                            AverageTimePerTrans = 0,
                            AverageTimeWaitingAfterPerTrans = 0,
                            AverageTimeWaitingBeforePerTrans = 0,
                            UserId = t.UserId,
                            PrintTime = t.Q_HisDailyRequire.PrintTime,
                            Index = t.Q_HisDailyRequire.TicketNumber
                        }).ToList();

                    ThuNganObjs = db.Q_HisDailyRequire_De.Where(x =>
                    !x.Q_User.IsDeleted &&
                    x.UserId == thuNganId &&
                    !x.Q_Major.IsDeleted &&
                    !x.Q_HisDailyRequire.Q_Service.IsDeleted &&
                    x.Q_HisDailyRequire.PrintTime >= from &&
                    x.Q_HisDailyRequire.PrintTime <= to &&
                    x.Q_Status.Id == (int)eStatus.HOTAT).Select(t => new R_GeneralInDayModel()
                    {
                        Id = t.HisDailyRequireId,
                        ObjectId = t.Q_HisDailyRequire.ServiceId,
                        Start = t.ProcessTime,
                        End = t.EndProcessTime,
                        Name = t.Q_HisDailyRequire.Q_Service.Name,
                        TotalTransaction = 0,
                        TotalTransTime = 0,
                        AverageTimePerTrans = 0,
                        AverageTimeWaitingAfterPerTrans = 0,
                        AverageTimeWaitingBeforePerTrans = 0,
                        UserId = t.UserId,
                        PrintTime = t.Q_HisDailyRequire.PrintTime,
                        Index = t.Q_HisDailyRequire.TicketNumber
                    }).ToList();

                    if (objs.Count > 0)
                    {
                        if (objId > 0)
                            objs = objs.Where(x => x.ObjectId == objId).ToList();
                        temps.AddRange(objs);

                        objs = objs.GroupBy(x => x.ObjectId).Select(t => new R_GeneralInDayModel()
                        {
                            ObjectId = t.Key,
                            //check lai
                            TotalTransaction = t.Select(x => x.Id).Distinct().Count(),
                            TotalTransTime = t.Sum(p => p.End.Value.Subtract(p.Start.Value).TotalMinutes),
                            Name = t.FirstOrDefault().Name
                        }).ToList();

                        for (int i = 0; i < objs.Count; i++)
                        {
                            objs[i].Index = i + 1;

                            //luot giao dich
                            var phieus = temps.Where(x => x.ObjectId == objs[i].ObjectId && x.UserId != null && x.UserId != thuNganId && x.End.HasValue).ToList();
                            var ids = phieus.Select(x => x.Id).Distinct().ToArray();
                            objs[i].TotalTransaction = ids.Length;
                            objs[i].TotalTransTime = Math.Round(phieus.Sum(p => (p.End.Value.Subtract(p.Start.Value)).TotalMinutes), 0, MidpointRounding.AwayFromZero);

                            objs[i].AverageTimePerTrans = ((objs[i].TotalTransTime > 0 && objs[i].TotalTransaction > 0) ? Math.Round((double)(objs[i].TotalTransTime / objs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0);

                            // thoi gian cho truoc xu ly
                            int TNGoi = 0;
                            for (int z = 0; z < ids.Length; z++)
                            {
                                var newObj = phieus.Where(x => x.Id == ids[z]).OrderBy(x => x.Start).FirstOrDefault();
                                var choTruocSC = newObj.Start.Value.Subtract(newObj.PrintTime);

                                objs[i].AverageTimeWaitingBeforePerTrans += choTruocSC.TotalMinutes;

                                newObj = phieus.Where(x => x.Id == ids[z]).OrderByDescending(x => x.End).FirstOrDefault();
                                var thunganObj = ThuNganObjs.Where(x => x.Id == ids[z]).OrderByDescending(x => x.Start).FirstOrDefault();
                                var thunganObjss = ThuNganObjs.Where(x => x.Id == ids[z]).OrderByDescending(x => x.Start).ToList();
                                if (thunganObj != null && thunganObj.End.HasValue)
                                {
                                    TNGoi++;
                                    double choSauSC = 0;
                                    if (thunganObj.Start.Value > newObj.End.Value)
                                        choSauSC = thunganObj.Start.Value.Subtract(newObj.End.Value).TotalMinutes;
                                    else
                                        choSauSC = newObj.End.Value.Subtract(thunganObj.Start.Value).TotalMinutes;

                                    if (choSauSC > 0)
                                        objs[i].AverageTimeWaitingAfterPerTrans += choSauSC;
                                }

                            }
                            if (objs[i].AverageTimeWaitingBeforePerTrans > 0)
                                //  objs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(objs[i].AverageTimeWaitingBeforePerTrans / objs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero);
                                objs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(objs[i].AverageTimeWaitingBeforePerTrans / ids.Length), 2, MidpointRounding.AwayFromZero);
                            if (objs[i].AverageTimeWaitingAfterPerTrans > 0)
                                objs[i].AverageTimeWaitingAfterPerTrans = Math.Round((double)(objs[i].AverageTimeWaitingAfterPerTrans / TNGoi), 2, MidpointRounding.AwayFromZero);

                            if (objs[i].AverageTimeWaitingBeforePerTrans < 0)
                                objs[i].AverageTimeWaitingBeforePerTrans = 0;
                            if (objs[i].AverageTimeWaitingAfterPerTrans < 0)
                                objs[i].AverageTimeWaitingAfterPerTrans = 0;

                        }
                    }
                    #endregion
                    temps.Clear();
                    #region today
                    var todayObjs = (db.Q_DailyRequire_Detail.Where(x =>
                                    !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_DailyRequire.Q_Service.IsDeleted &&
                                    x.Q_DailyRequire.PrintTime >= from && x.Q_DailyRequire.PrintTime <= to &&
                                    x.Q_Status.Id == (int)eStatus.HOTAT)
                        .Select(t => new R_GeneralInDayModel()
                        {
                            Id = t.DailyRequireId,
                            ObjectId = t.Q_DailyRequire.ServiceId,
                            Start = t.ProcessTime,
                            End = t.EndProcessTime,
                            Name = t.Q_DailyRequire.Q_Service.Name,
                            TotalTransaction = 0,
                            TotalTransTime = 0,
                            AverageTimePerTrans = 0,
                            AverageTimeWaitingAfterPerTrans = 0,
                            AverageTimeWaitingBeforePerTrans = 0,
                            UserId = t.UserId,
                            PrintTime = t.Q_DailyRequire.PrintTime
                        }).ToList());

                    ThuNganObjs = (db.Q_DailyRequire_Detail.Where(x =>
                    !x.Q_User.IsDeleted &&
                    !x.Q_Major.IsDeleted &&
                    !x.Q_DailyRequire.Q_Service.IsDeleted &&
                    x.Q_DailyRequire.PrintTime >= from &&
                    x.Q_DailyRequire.PrintTime <= to &&
                    x.Q_Status.Id == (int)eStatus.HOTAT &&
                    x.UserId == thuNganId).Select(t => new R_GeneralInDayModel()
                    {
                        Id = t.DailyRequireId,
                        ObjectId = t.Q_DailyRequire.ServiceId,
                        Start = t.ProcessTime,
                        End = t.EndProcessTime,
                        Name = t.Q_DailyRequire.Q_Service.Name,
                        TotalTransaction = 0,
                        TotalTransTime = 0,
                        AverageTimePerTrans = 0,
                        AverageTimeWaitingAfterPerTrans = 0,
                        AverageTimeWaitingBeforePerTrans = 0,
                        UserId = t.UserId,
                        PrintTime = t.Q_DailyRequire.PrintTime
                    }).ToList());

                    if (todayObjs.Count > 0)
                    {
                        if (objId > 0)
                            todayObjs = todayObjs.Where(x => x.ObjectId == objId).ToList();

                        temps.AddRange(todayObjs);
                        todayObjs = todayObjs.GroupBy(x => x.ObjectId).Select(t => new R_GeneralInDayModel()
                        {
                            ObjectId = t.Key,
                            //check lai
                            TotalTransaction = t.Select(x => x.Id).Distinct().Count(),
                            // TotalTransTime = t.Sum(p => p.End.Value.Minute + (p.End.Value.Hour - p.Start.Value.Hour) * 60 - p.Start.Value.Minute),
                            TotalTransTime = t.Sum(p => p.End.Value.Subtract(p.Start.Value).TotalMinutes),
                            Name = t.FirstOrDefault().Name
                        }).ToList();

                        for (int i = 0; i < todayObjs.Count; i++)
                        {
                            todayObjs[i].Index = i + 1;

                            //luot giao dich
                            //  var phieus = temps.Where(x => x.ObjectId == todayObjs[i].ObjectId && x.UserId != null && x.UserId != thuNganId).ToList();
                            var phieus = temps.Where(x => x.ObjectId == todayObjs[i].ObjectId && x.UserId != thuNganId).ToList();
                            var ids = phieus.Select(x => x.Id).Distinct().ToArray();
                            todayObjs[i].TotalTransaction = ids.Length;
                            //todayObjs[i].TotalTransTime = phieus.Sum(p => p.End.Value.Minute + (p.End.Value.Hour - p.Start.Value.Hour) * 60 - p.Start.Value.Minute);
                            todayObjs[i].TotalTransTime = phieus.Sum(p => p.End.Value.Subtract(p.Start.Value).TotalMinutes);
                            todayObjs[i].AverageTimePerTrans = ((todayObjs[i].TotalTransTime > 0 && todayObjs[i].TotalTransaction > 0) ? Math.Round((double)(todayObjs[i].TotalTransTime / todayObjs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0);

                            // thoi gian cho truoc xu ly
                            int TNGoi = 0;

                            for (int z = 0; z < ids.Length; z++)
                            {
                                var newObj = phieus.Where(x => x.Id == ids[z]).OrderBy(x => x.Start).FirstOrDefault();
                                // todayObjs[i].AverageTimeWaitingBeforePerTrans += (newObj.Start.Value.Minute + (newObj.Start.Value.Hour - newObj.PrintTime.Hour) * 60 - newObj.PrintTime.Minute);
                                double choTruocSC = newObj.Start.Value.Subtract(newObj.PrintTime).TotalMinutes;
                                todayObjs[i].AverageTimeWaitingBeforePerTrans += choTruocSC;

                                newObj = phieus.Where(x => x.Id == ids[z]).OrderByDescending(x => x.End).FirstOrDefault();
                                var thunganObj = ThuNganObjs.Where(x => x.Id == ids[z]).OrderByDescending(x => x.End).FirstOrDefault();
                                if (thunganObj != null)
                                {
                                    TNGoi++;
                                    double choSauSC = (thunganObj.Start.Value.Subtract(newObj.End.Value).TotalMinutes);
                                    if (choSauSC > 0)
                                        todayObjs[i].AverageTimeWaitingAfterPerTrans += choSauSC;
                                }
                            }
                            if (todayObjs[i].AverageTimeWaitingBeforePerTrans > 0)
                                // todayObjs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(todayObjs[i].AverageTimeWaitingBeforePerTrans / todayObjs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero);
                                todayObjs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(todayObjs[i].AverageTimeWaitingBeforePerTrans / ids.Length), 2, MidpointRounding.AwayFromZero);
                            if (todayObjs[i].AverageTimeWaitingAfterPerTrans > 0)
                                todayObjs[i].AverageTimeWaitingAfterPerTrans = Math.Round((double)(todayObjs[i].AverageTimeWaitingAfterPerTrans / TNGoi), 2, MidpointRounding.AwayFromZero);

                            if (todayObjs[i].AverageTimeWaitingBeforePerTrans < 0)
                                todayObjs[i].AverageTimeWaitingBeforePerTrans = 0;
                            if (todayObjs[i].AverageTimeWaitingAfterPerTrans < 0)
                                todayObjs[i].AverageTimeWaitingAfterPerTrans = 0;
                        }
                        objs.AddRange(todayObjs);
                    }
                    #endregion

                    objs = objs.GroupBy(x => x.ObjectId).Select(t => new R_GeneralInDayModel()
                    {
                        ObjectId = t.Key,
                        TotalTransaction = t.Sum(p => p.TotalTransaction),
                        TotalTransTime = t.Sum(p => p.TotalTransTime),
                        Name = t.FirstOrDefault().Name,
                        AverageTimePerTrans = (t.Sum(p => p.AverageTimePerTrans) > 0 ? t.Sum(p => p.AverageTimePerTrans) / t.Count() : 0),
                        AverageTimeWaitingAfterPerTrans = t.Sum(p => p.AverageTimeWaitingAfterPerTrans),
                        AverageTimeWaitingBeforePerTrans = (t.Sum(p => p.AverageTimeWaitingBeforePerTrans) > 0 ? t.Sum(p => p.AverageTimeWaitingBeforePerTrans) / t.Count() : 0),

                    }).OrderBy(x => x.ObjectId).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
                return objs;
            }
        }

        public List<R_GeneralInDayModel> GeneralReport_DichVuTienThu(SqlConnection sqlConnection, int objId, int thuNganId, DateTime from, DateTime to)
        {
            List<R_GeneralInDayModel>
                objs = new List<R_GeneralInDayModel>(),
            ThuNganObjs = new List<R_GeneralInDayModel>(),
            todayObjs = new List<R_GeneralInDayModel>(),
            temps = new List<R_GeneralInDayModel>();
            TimeSpan time = new TimeSpan(0, 0, 0);
            string query = string.Empty;
            SqlDataAdapter adap = null;
            DataTable dt = null;
            try
            {
                #region Histories
                query = "select dr.Id as DailyRequireId,dr.ServiceId,dd.ProcessTime as Start,dd.EndProcessTime as [End],s.Name as ServiceName,dd.UserId,dr.PrintTime,dr.TicketNumber from Q_HisDailyRequire dr,Q_HisDailyRequire_De dd,Q_Major m,Q_Service s where m.IsDeleted = 0 and dd.MajorId = m.Id and s.IsDeleted = 0 and dr.ServiceId = s.Id and dr.Id = dd.HisDailyRequireId  and dd.StatusId=" + (int)eStatus.HOTAT + " and dr.PrintTime >= '" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime<='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                adap = new SqlDataAdapter(query, sqlConnection);
                dt = new DataTable();
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    R_GeneralInDayModel newItem;
                    foreach (DataRow row in dt.Rows)
                    {
                        newItem = new R_GeneralInDayModel()
                        {
                            Id = getIntValue(row["DailyRequireId"]),
                            ObjectId = getIntValue(row["ServiceId"]),
                            Start = getDateValue(row["Start"]),
                            End = getDateValue(row["End"]),
                            Name = getStringValue(row["ServiceName"]),
                            TotalTransaction = 0,
                            TotalTransTime = 0,
                            AverageTimePerTrans = 0,
                            AverageTimeWaitingAfterPerTrans = 0,
                            AverageTimeWaitingBeforePerTrans = 0,
                            PrintTime = getDateValue(row["PrintTime"]).Value,
                            Index = getIntValue(row["TicketNumber"]),
                            UserId = getIntValue(row["UserId"]),
                        };
                        if (newItem.UserId.HasValue && newItem.UserId.Value == thuNganId)
                            ThuNganObjs.Add(newItem);
                        else
                            objs.Add(newItem);
                    }

                    if (objId > 0)
                        objs = objs.Where(x => x.ObjectId == objId).ToList();
                    temps.AddRange(objs);

                    objs = objs.GroupBy(x => x.ObjectId).Select(t => new R_GeneralInDayModel()
                    {
                        ObjectId = t.Key,
                        //check lai
                        TotalTransaction = t.Select(x => x.Id).Distinct().Count(),
                        TotalTransTime = t.Sum(p => p.End.Value.Subtract(p.Start.Value).TotalMinutes),
                        Name = t.FirstOrDefault().Name
                    }).ToList();

                    for (int i = 0; i < objs.Count; i++)
                    {
                        objs[i].Index = i + 1;

                        //luot giao dich
                        var phieus = temps.Where(x => x.ObjectId == objs[i].ObjectId && x.UserId != null && x.UserId != thuNganId && x.End.HasValue).ToList();
                        var ids = phieus.Select(x => x.Id).Distinct().ToArray();
                        objs[i].TotalTransaction = ids.Length;
                        objs[i].TotalTransTime = Math.Round(phieus.Sum(p => (p.End.Value.Subtract(p.Start.Value)).TotalMinutes), 0, MidpointRounding.AwayFromZero);

                        objs[i].AverageTimePerTrans = ((objs[i].TotalTransTime > 0 && objs[i].TotalTransaction > 0) ? Math.Round((double)(objs[i].TotalTransTime / objs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0);

                        // thoi gian cho truoc xu ly
                        int TNGoi = 0;
                        for (int z = 0; z < ids.Length; z++)
                        {
                            var newObj = phieus.Where(x => x.Id == ids[z]).OrderBy(x => x.Start).FirstOrDefault();
                            var choTruocSC = newObj.Start.Value.Subtract(newObj.PrintTime);

                            objs[i].AverageTimeWaitingBeforePerTrans += choTruocSC.TotalMinutes;

                            newObj = phieus.Where(x => x.Id == ids[z]).OrderByDescending(x => x.End).FirstOrDefault();
                            var thunganObj = ThuNganObjs.Where(x => x.Id == ids[z]).OrderByDescending(x => x.Start).FirstOrDefault();
                            var thunganObjss = ThuNganObjs.Where(x => x.Id == ids[z]).OrderByDescending(x => x.Start).ToList();
                            if (thunganObj != null && thunganObj.End.HasValue)
                            {
                                TNGoi++;
                                double choSauSC = 0;
                                if (thunganObj.Start.Value > newObj.End.Value)
                                    choSauSC = thunganObj.Start.Value.Subtract(newObj.End.Value).TotalMinutes;
                                else
                                    choSauSC = newObj.End.Value.Subtract(thunganObj.Start.Value).TotalMinutes;

                                if (choSauSC > 0)
                                    objs[i].AverageTimeWaitingAfterPerTrans += choSauSC;
                            }

                        }
                        if (objs[i].AverageTimeWaitingBeforePerTrans > 0)
                            //  objs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(objs[i].AverageTimeWaitingBeforePerTrans / objs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero);
                            objs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(objs[i].AverageTimeWaitingBeforePerTrans / ids.Length), 2, MidpointRounding.AwayFromZero);
                        if (objs[i].AverageTimeWaitingAfterPerTrans > 0)
                            objs[i].AverageTimeWaitingAfterPerTrans = Math.Round((double)(objs[i].AverageTimeWaitingAfterPerTrans / TNGoi), 2, MidpointRounding.AwayFromZero);

                        if (objs[i].AverageTimeWaitingBeforePerTrans < 0)
                            objs[i].AverageTimeWaitingBeforePerTrans = 0;
                        if (objs[i].AverageTimeWaitingAfterPerTrans < 0)
                            objs[i].AverageTimeWaitingAfterPerTrans = 0;

                    }
                }
                #endregion

                temps.Clear();
                if (to >= DateTime.Now)
                {
                    #region today
                    query = "select dr.Id as DailyRequireId,dr.ServiceId,dd.ProcessTime as Start,dd.EndProcessTime as [End],s.Name as ServiceName,dd.UserId,dr.PrintTime,dr.TicketNumber from Q_DailyRequire dr,Q_DailyRequire_Detail dd,Q_Major m,Q_Service s where m.IsDeleted = 0 and dd.MajorId = m.Id and s.IsDeleted = 0 and dr.ServiceId = s.Id and dr.Id = dd.DailyRequireId and dd.StatusId=" + (int)eStatus.HOTAT;
                    adap = new SqlDataAdapter(query, sqlConnection);
                    dt = new DataTable();
                    adap.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        R_GeneralInDayModel newItem;
                        foreach (DataRow row in dt.Rows)
                        {
                            newItem = new R_GeneralInDayModel()
                            {
                                Id = getIntValue(row["DailyRequireId"]),
                                ObjectId = getIntValue(row["ServiceId"]),
                                Start = getDateValue(row["Start"]),
                                End = getDateValue(row["End"]),
                                Name = getStringValue(row["ServiceName"]),
                                TotalTransaction = 0,
                                TotalTransTime = 0,
                                AverageTimePerTrans = 0,
                                AverageTimeWaitingAfterPerTrans = 0,
                                AverageTimeWaitingBeforePerTrans = 0,
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                Index = getIntValue(row["TicketNumber"]),
                                UserId = getIntValue(row["UserId"]),
                            };
                            if (newItem.UserId.HasValue && newItem.UserId.Value == thuNganId)
                                ThuNganObjs.Add(newItem);
                            else
                                todayObjs.Add(newItem);
                        }
                        if (objId > 0)
                            todayObjs = todayObjs.Where(x => x.ObjectId == objId).ToList();

                        temps.AddRange(todayObjs);
                        todayObjs = todayObjs.GroupBy(x => x.ObjectId).Select(t => new R_GeneralInDayModel()
                        {
                            ObjectId = t.Key,
                            //check lai
                            TotalTransaction = t.Select(x => x.Id).Distinct().Count(),
                            // TotalTransTime = t.Sum(p => p.End.Value.Minute + (p.End.Value.Hour - p.Start.Value.Hour) * 60 - p.Start.Value.Minute),
                            TotalTransTime = t.Sum(p => p.End.Value.Subtract(p.Start.Value).TotalMinutes),
                            Name = t.FirstOrDefault().Name
                        }).ToList();

                        for (int i = 0; i < todayObjs.Count; i++)
                        {
                            todayObjs[i].Index = i + 1;

                            //luot giao dich
                            //  var phieus = temps.Where(x => x.ObjectId == todayObjs[i].ObjectId && x.UserId != null && x.UserId != thuNganId).ToList();
                            var phieus = temps.Where(x => x.ObjectId == todayObjs[i].ObjectId && x.UserId != thuNganId).ToList();
                            var ids = phieus.Select(x => x.Id).Distinct().ToArray();
                            todayObjs[i].TotalTransaction = ids.Length;
                            //todayObjs[i].TotalTransTime = phieus.Sum(p => p.End.Value.Minute + (p.End.Value.Hour - p.Start.Value.Hour) * 60 - p.Start.Value.Minute);
                            todayObjs[i].TotalTransTime = phieus.Sum(p => p.End.Value.Subtract(p.Start.Value).TotalMinutes);
                            todayObjs[i].AverageTimePerTrans = ((todayObjs[i].TotalTransTime > 0 && todayObjs[i].TotalTransaction > 0) ? Math.Round((double)(todayObjs[i].TotalTransTime / todayObjs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero) : 0);

                            // thoi gian cho truoc xu ly
                            int TNGoi = 0;

                            for (int z = 0; z < ids.Length; z++)
                            {
                                var newObj = phieus.Where(x => x.Id == ids[z]).OrderBy(x => x.Start).FirstOrDefault();
                                // todayObjs[i].AverageTimeWaitingBeforePerTrans += (newObj.Start.Value.Minute + (newObj.Start.Value.Hour - newObj.PrintTime.Hour) * 60 - newObj.PrintTime.Minute);
                                double choTruocSC = newObj.Start.Value.Subtract(newObj.PrintTime).TotalMinutes;
                                todayObjs[i].AverageTimeWaitingBeforePerTrans += choTruocSC;

                                newObj = phieus.Where(x => x.Id == ids[z]).OrderByDescending(x => x.End).FirstOrDefault();
                                var thunganObj = ThuNganObjs.Where(x => x.Id == ids[z]).OrderByDescending(x => x.End).FirstOrDefault();
                                if (thunganObj != null)
                                {
                                    TNGoi++;
                                    double choSauSC = (thunganObj.Start.Value.Subtract(newObj.End.Value).TotalMinutes);
                                    if (choSauSC > 0)
                                        todayObjs[i].AverageTimeWaitingAfterPerTrans += choSauSC;
                                }
                            }
                            if (todayObjs[i].AverageTimeWaitingBeforePerTrans > 0)
                                // todayObjs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(todayObjs[i].AverageTimeWaitingBeforePerTrans / todayObjs[i].TotalTransaction), 2, MidpointRounding.AwayFromZero);
                                todayObjs[i].AverageTimeWaitingBeforePerTrans = Math.Round((double)(todayObjs[i].AverageTimeWaitingBeforePerTrans / ids.Length), 2, MidpointRounding.AwayFromZero);
                            if (todayObjs[i].AverageTimeWaitingAfterPerTrans > 0)
                                todayObjs[i].AverageTimeWaitingAfterPerTrans = Math.Round((double)(todayObjs[i].AverageTimeWaitingAfterPerTrans / TNGoi), 2, MidpointRounding.AwayFromZero);

                            if (todayObjs[i].AverageTimeWaitingBeforePerTrans < 0)
                                todayObjs[i].AverageTimeWaitingBeforePerTrans = 0;
                            if (todayObjs[i].AverageTimeWaitingAfterPerTrans < 0)
                                todayObjs[i].AverageTimeWaitingAfterPerTrans = 0;
                        }
                        objs.AddRange(todayObjs);
                    }

                    #endregion
                }
                objs = objs.GroupBy(x => x.ObjectId).Select(t => new R_GeneralInDayModel()
                {
                    ObjectId = t.Key,
                    TotalTransaction = t.Sum(p => p.TotalTransaction),
                    TotalTransTime = t.Sum(p => p.TotalTransTime),
                    Name = t.FirstOrDefault().Name,
                    AverageTimePerTrans = (t.Sum(p => p.AverageTimePerTrans) > 0 ? t.Sum(p => p.AverageTimePerTrans) / t.Count() : 0),
                    AverageTimeWaitingAfterPerTrans = t.Sum(p => p.AverageTimeWaitingAfterPerTrans),
                    AverageTimeWaitingBeforePerTrans = (t.Sum(p => p.AverageTimeWaitingBeforePerTrans) > 0 ? t.Sum(p => p.AverageTimeWaitingBeforePerTrans) / t.Count() : 0),

                }).OrderBy(x => x.ObjectId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return objs;

        }

        /// <summary>
        /// Lấy báo cáo chi tiết ngày theo mẫu TienThu 
        /// </summary>
        /// <param name="objId">Đối tượng search</param>
        /// <param name="ThuNganId">Thu ngan Id</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<ReportModel> DetailReport_DichVuTienThu(string connectString, int objId, int ThuNganId, DateTime from, DateTime to)
        {
            List<ReportModel> yesList = new List<ReportModel>(),
                            todayList = new List<ReportModel>(),
                            returnList = new List<ReportModel>();
            TimeSpan time = new TimeSpan(0, 0, 0);
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    #region  theo dich vu
                    #region Yesterday
                    yesList.AddRange(db.Q_HisDailyRequire_De.Where(x => !x.Q_Major.IsDeleted &&
                        !x.Q_HisDailyRequire.Q_Service.IsDeleted &&
                        x.Q_HisDailyRequire.PrintTime >= from &&
                        x.Q_HisDailyRequire.PrintTime <= to &&
                        x.ProcessTime.HasValue &&
                        x.UserId.HasValue &&
                         x.Q_Status.Id == (int)eStatus.HOTAT &&
                        x.UserId != ThuNganId).Select(x => new ReportModel()
                        {
                            Id = x.Q_HisDailyRequire.Id,
                            Number = x.Q_HisDailyRequire.TicketNumber,
                            ServiceName = x.Q_HisDailyRequire.Q_Service.Name,
                            PrintTime = x.Q_HisDailyRequire.PrintTime,
                            ServiceId = x.Q_HisDailyRequire.ServiceId,
                            Start = x.ProcessTime,
                            End = x.EndProcessTime,
                            StatusName = (x.StatusId == (int)eStatus.CHOXL ? "Đang xử lý" : "Hoàn tất")
                        }).ToList());
                    if (yesList.Count > 0 && objId != 0)
                        yesList = yesList.Where(x => x.ServiceId == objId).ToList();

                    var hisThuNgans = db.Q_HisDailyRequire_De.Where(x => !x.Q_Major.IsDeleted &&
                     !x.Q_HisDailyRequire.Q_Service.IsDeleted &&
                     x.Q_HisDailyRequire.PrintTime >= from &&
                     x.Q_HisDailyRequire.PrintTime <= to &&
                     x.UserId.HasValue && x.UserId == ThuNganId &&
                      x.ProcessTime.HasValue).Select(x => new ReportModel()
                      {
                          Id = x.Q_HisDailyRequire.Id,
                          Number = x.Q_HisDailyRequire.TicketNumber,
                          ServiceName = x.Q_HisDailyRequire.Q_Service.Name,
                          PrintTime = x.Q_HisDailyRequire.PrintTime,
                          ServiceId = x.Q_HisDailyRequire.ServiceId,
                          Start = x.ProcessTime,
                          End = x.EndProcessTime,
                          StatusName = (x.StatusId == (int)eStatus.CHOXL ? "Đang xử lý" : "Hoàn tất")
                      }).ToList();

                    if (yesList.Count > 0)
                    {
                        var ids = yesList.Select(x => x.Id).Distinct().ToArray();
                        for (int i = 0; i < ids.Length; i++)
                        {
                            var phieus = yesList.Where(x => x.Id == ids[i]).OrderBy(x => x.Start).ToList();
                            var fakePrintTime = phieus[0].PrintTime;
                            if (phieus.Count > 0)
                            {
                                for (int ii = 0; ii < phieus.Count; ii++)
                                {
                                    if (phieus[ii].Start.HasValue)
                                        phieus[ii].str_Start = phieus[ii].Start.Value.ToString("dd/MM/yyyy HH:mm");
                                    if (phieus[ii].End.HasValue)
                                        phieus[ii].str_End = phieus[ii].End.Value.ToString("dd/MM/yyyy HH:mm");
                                    //thoi gian giao dich tai ban nang
                                    if (phieus[ii].Start.HasValue && phieus[ii].End.HasValue)
                                    {
                                        phieus[ii].ProcessTime = phieus[ii].End.Value.Subtract(phieus[ii].Start.Value).ToString("hh\\:mm\\:ss");

                                    }

                                    //thoi gian cho truoc khi len ban nang sua chua
                                    // tinh cho lan dau tien nhung lan sau ko tinh
                                    if (phieus[ii].Start.HasValue && ii == 0)
                                        phieus[ii].WaitingTime = phieus[ii].Start.Value.Subtract(phieus[ii].PrintTime).ToString("hh\\:mm\\:ss");

                                    if (ii == phieus.Count - 1)
                                    {
                                        //thoi gian thu ngan bat dau goi
                                        var hisTN = hisThuNgans.Where(x => x.Id == phieus[ii].Id && x.Start.HasValue && x.End.HasValue).OrderByDescending(x => x.Start).FirstOrDefault();
                                        if (hisTN != null)
                                        {
                                            phieus[ii].StartTN = hisTN.Start;
                                            phieus[ii].str_StartTN = hisTN.Start.Value.ToString("dd/MM/yyyy HH:mm");

                                            //khoang thoi gian chờ tu luc ket thuc sua chua cho toi khi thu ngan goi
                                            if (phieus[ii].End.HasValue && phieus[ii].StartTN.HasValue)
                                                phieus[ii].WaitingTimeTN = phieus[ii].StartTN.Value.Subtract(phieus[ii].End.Value).ToString("hh\\:mm\\:ss");
                                        }
                                    }
                                    returnList.Add(phieus[ii]);
                                    if (phieus[ii].End.HasValue)
                                        fakePrintTime = phieus[ii].End.Value;
                                }
                            }
                        }
                    }
                    #endregion

                    #region Today
                    todayList.AddRange(db.Q_DailyRequire_Detail.Where(x => !x.Q_Major.IsDeleted &&
                        !x.Q_DailyRequire.Q_Service.IsDeleted &&
                        x.Q_DailyRequire.PrintTime >= from &&
                        x.Q_DailyRequire.PrintTime <= to &&
                        x.ProcessTime.HasValue &&
                        x.UserId.HasValue &&
                         x.Q_Status.Id == (int)eStatus.HOTAT &&
                        x.UserId != ThuNganId).Select(x => new ReportModel()
                        {
                            Id = x.Q_DailyRequire.Id,
                            Number = x.Q_DailyRequire.TicketNumber,
                            ServiceName = x.Q_DailyRequire.Q_Service.Name,
                            PrintTime = x.Q_DailyRequire.PrintTime,
                            ServiceId = x.Q_DailyRequire.ServiceId,
                            Start = x.ProcessTime,
                            End = x.EndProcessTime,
                            StatusName = (x.StatusId == (int)eStatus.CHOXL ? "Đang xử lý" : "Hoàn tất")
                        }).ToList());

                    var todayThuNgans = db.Q_DailyRequire_Detail.Where(x => !x.Q_Major.IsDeleted &&
                     !x.Q_DailyRequire.Q_Service.IsDeleted &&
                     x.Q_DailyRequire.PrintTime >= from &&
                     x.Q_DailyRequire.PrintTime <= to &&
                     x.UserId.HasValue &&
                      x.ProcessTime.HasValue &&
                     x.UserId == ThuNganId).Select(x => new ReportModel()
                     {
                         Id = x.Q_DailyRequire.Id,
                         Number = x.Q_DailyRequire.TicketNumber,
                         ServiceName = x.Q_DailyRequire.Q_Service.Name,
                         PrintTime = x.Q_DailyRequire.PrintTime,
                         ServiceId = x.Q_DailyRequire.ServiceId,
                         Start = x.ProcessTime,
                         End = x.EndProcessTime,
                         StatusName = (x.StatusId == (int)eStatus.CHOXL ? "Đang xử lý" : "Hoàn tất")
                     }).ToList();

                    if (todayList.Count > 0)
                    {
                        var ids = todayList.Select(x => x.Id).Distinct().ToArray();
                        for (int i = 0; i < ids.Length; i++)
                        {
                            var phieus = todayList.Where(x => x.Id == ids[i]).OrderBy(x => x.Start).ToList();
                            if (phieus.Count > 0)
                            {
                                var fakePrintTime = phieus[0].PrintTime;
                                for (int ii = 0; ii < phieus.Count; ii++)
                                {
                                    if (phieus[ii].Start.HasValue)
                                        phieus[ii].str_Start = phieus[ii].Start.Value.ToString("dd/MM/yyyy HH:mm");
                                    if (phieus[ii].End.HasValue)
                                        phieus[ii].str_End = phieus[ii].End.Value.ToString("dd/MM/yyyy HH:mm");
                                    //thoi gian giao dich tai ban nang
                                    if (phieus[ii].Start.HasValue && phieus[ii].End.HasValue)
                                        phieus[ii].ProcessTime = phieus[ii].End.Value.Subtract(phieus[ii].Start.Value).ToString("hh\\:mm\\:ss");
                                    //thoi gian cho truoc khi len ban nang sua chua
                                    if (phieus[ii].Start.HasValue && fakePrintTime != null)
                                        phieus[ii].WaitingTime = phieus[ii].Start.Value.Subtract(fakePrintTime).ToString("hh\\:mm\\:ss");

                                    if (ii == phieus.Count - 1)
                                    {
                                        //thoi gian thu ngan bat dau goi
                                        var nowTN = todayThuNgans.Where(x => x.Id == phieus[ii].Id && x.Start.HasValue).OrderByDescending(x => x.Start).FirstOrDefault();
                                        if (nowTN != null)
                                        {
                                            phieus[ii].StartTN = nowTN.Start;
                                            phieus[ii].str_StartTN = nowTN.Start.Value.ToString("dd/MM/yyyy HH:mm");

                                            //khoang thoi gian chờ tu luc ket thuc sua chua cho toi khi thu ngan goi
                                            if (phieus[ii].End.HasValue && phieus[ii].StartTN.HasValue)
                                                phieus[ii].WaitingTimeTN = phieus[ii].StartTN.Value.Subtract(phieus[ii].End.Value).ToString("hh\\:mm\\:ss");
                                        }
                                    }
                                    returnList.Add(phieus[ii]);
                                    if (phieus[ii].End.HasValue)
                                        fakePrintTime = phieus[ii].End.Value;
                                }
                            }


                        }
                    }
                    #endregion

                    if (returnList.Count > 0)
                    {
                        if (objId > 0)
                            returnList = returnList.Where(x => x.ServiceId == objId).ToList();
                        returnList = returnList.OrderBy(x => x.PrintTime).ThenBy(x => x.Number).ToList();
                        for (int i = 0; i < returnList.Count; i++)
                            returnList[i].stt = i + 1;
                    }

                    #endregion
                }
                catch (Exception)
                { }
                return returnList;
            }
        }

        public List<ReportModel> DetailReport_DichVuTienThu(SqlConnection sqlConnection, int objId, int ThuNganId, DateTime from, DateTime to)
        {
            List<ReportModel>
                yesList = new List<ReportModel>(),
                hisThuNgans = new List<ReportModel>(),
                            todayList = new List<ReportModel>(),
                            todayThuNgans = new List<ReportModel>(),
                            returnList = new List<ReportModel>();
            ReportModel newItem;
            #region Yesterday
            TimeSpan time = new TimeSpan(0, 0, 0);
            string query = "select dr.Id, dr.TicketNumber,dr.PrintTime,s.Name as ServiceName, s.Id as ServiceId, dd.ProcessTime, dd.EndProcessTime, dd.StatusId, dd.UserId from Q_HisDailyRequire dr, Q_HisDailyRequire_De dd,  Q_Service s where dr.Id = dd.HisDailyRequireId  and dr.ServiceId = s.Id  and s.IsDeleted = 0 and s.IsActived = 1 and dd.ProcessTime is not null  and dd.StatusId=" + (int)eStatus.HOTAT + " and dr.PrintTime >= '" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime<='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            var adap = new SqlDataAdapter(query, sqlConnection);
            var dt = new DataTable();
            adap.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    newItem = new ReportModel()
                    {
                        Id = getIntValue(row["Id"]),
                        Number = getIntValue(row["TicketNumber"]),
                        ServiceName = getStringValue(row["ServiceName"]),
                        PrintTime = getDateValue(row["PrintTime"]).Value,
                        ServiceId = getIntValue(row["ServiceId"]),
                        Start = getDateValue(row["ProcessTime"]),
                        End = getDateValue(row["EndProcessTime"]),
                        StatusName = (getIntValue(row["StatusId"]) == (int)eStatus.CHOXL ? "Đang xử lý" : "Hoàn tất"),
                        UserId = getIntValue(row["UserId"])
                    };
                    if (newItem.UserId.HasValue && newItem.UserId.Value == ThuNganId)
                        hisThuNgans.Add(newItem);
                    else
                        yesList.Add(newItem);
                }
            }
            if (yesList.Count > 0 && objId != 0)
                yesList = yesList.Where(x => x.ServiceId == objId).ToList();


            if (yesList.Count > 0)
            {
                var ids = yesList.Select(x => x.Id).Distinct().ToArray();
                for (int i = 0; i < ids.Length; i++)
                {
                    var phieus = yesList.Where(x => x.Id == ids[i]).OrderBy(x => x.Start).ToList();
                    var fakePrintTime = phieus[0].PrintTime;
                    if (phieus.Count > 0)
                    {
                        for (int ii = 0; ii < phieus.Count; ii++)
                        {
                            if (phieus[ii].Start.HasValue)
                                phieus[ii].str_Start = phieus[ii].Start.Value.ToString("dd/MM/yyyy HH:mm");
                            if (phieus[ii].End.HasValue)
                                phieus[ii].str_End = phieus[ii].End.Value.ToString("dd/MM/yyyy HH:mm");
                            //thoi gian giao dich tai ban nang
                            if (phieus[ii].Start.HasValue && phieus[ii].End.HasValue)
                            {
                                phieus[ii].ProcessTime = phieus[ii].End.Value.Subtract(phieus[ii].Start.Value).ToString("hh\\:mm\\:ss");

                            }

                            //thoi gian cho truoc khi len ban nang sua chua
                            // tinh cho lan dau tien nhung lan sau ko tinh
                            if (phieus[ii].Start.HasValue && ii == 0)
                                phieus[ii].WaitingTime = phieus[ii].Start.Value.Subtract(phieus[ii].PrintTime).ToString("hh\\:mm\\:ss");

                            if (ii == phieus.Count - 1)
                            {
                                //thoi gian thu ngan bat dau goi
                                var hisTN = hisThuNgans.Where(x => x.Id == phieus[ii].Id && x.Start.HasValue && x.End.HasValue).OrderByDescending(x => x.Start).FirstOrDefault();
                                if (hisTN != null)
                                {
                                    phieus[ii].StartTN = hisTN.Start;
                                    phieus[ii].str_StartTN = hisTN.Start.Value.ToString("dd/MM/yyyy HH:mm");

                                    //khoang thoi gian chờ tu luc ket thuc sua chua cho toi khi thu ngan goi
                                    if (phieus[ii].End.HasValue && phieus[ii].StartTN.HasValue)
                                        phieus[ii].WaitingTimeTN = phieus[ii].StartTN.Value.Subtract(phieus[ii].End.Value).ToString("hh\\:mm\\:ss");
                                }
                            }
                            returnList.Add(phieus[ii]);
                            if (phieus[ii].End.HasValue)
                                fakePrintTime = phieus[ii].End.Value;
                        }
                    }
                }
            }
            #endregion

            #region Today
            if (to >= DateTime.Now)
            {
                query = "select dr.Id, dr.TicketNumber,dr.PrintTime,s.Name as ServiceName, s.Id as ServiceId, dd.ProcessTime, dd.EndProcessTime, dd.StatusId, dd.UserId from Q_DailyRequire dr, Q_DailyRequire_Detail dd,   Q_Service s where dr.Id = dd.DailyRequireId  and dr.ServiceId = s.Id   and s.IsDeleted = 0 and s.IsActived = 1 and dd.ProcessTime is not null and dd.StatusId=" + (int)eStatus.HOTAT + " and dr.PrintTime >= '" + from.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime<='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                adap = new SqlDataAdapter(query, sqlConnection);
                dt.Clear();
                adap.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        newItem = new ReportModel()
                        {
                            Id = getIntValue(row["Id"]),
                            Number = getIntValue(row["TicketNumber"]),
                            ServiceName = getStringValue(row["ServiceName"]),
                            PrintTime = getDateValue(row["PrintTime"]).Value,
                            ServiceId = getIntValue(row["ServiceId"]),
                            UserId = getIntValue(row["UserId"]),
                            Start = getDateValue(row["ProcessTime"]),
                            End = getDateValue(row["EndProcessTime"]),
                            StatusName = (getIntValue(row["StatusId"]) == (int)eStatus.CHOXL ? "Đang xử lý" : "Hoàn tất")
                        };

                        if (newItem.UserId.HasValue && newItem.UserId.Value == ThuNganId)
                            todayThuNgans.Add(newItem);
                        else
                            todayList.Add(newItem);
                    }
                }

                if (todayList.Count > 0)
                {
                    var ids = todayList.Select(x => x.Id).Distinct().ToArray();
                    for (int i = 0; i < ids.Length; i++)
                    {
                        var phieus = todayList.Where(x => x.Id == ids[i]).OrderBy(x => x.Start).ToList();
                        if (phieus.Count > 0)
                        {
                            var fakePrintTime = phieus[0].PrintTime;
                            for (int ii = 0; ii < phieus.Count; ii++)
                            {
                                if (phieus[ii].Start.HasValue)
                                    phieus[ii].str_Start = phieus[ii].Start.Value.ToString("dd/MM/yyyy HH:mm");
                                if (phieus[ii].End.HasValue)
                                    phieus[ii].str_End = phieus[ii].End.Value.ToString("dd/MM/yyyy HH:mm");
                                //thoi gian giao dich tai ban nang
                                if (phieus[ii].Start.HasValue && phieus[ii].End.HasValue)
                                    phieus[ii].ProcessTime = phieus[ii].End.Value.Subtract(phieus[ii].Start.Value).ToString("hh\\:mm\\:ss");
                                //thoi gian cho truoc khi len ban nang sua chua
                                if (phieus[ii].Start.HasValue && fakePrintTime != null)
                                    phieus[ii].WaitingTime = phieus[ii].Start.Value.Subtract(fakePrintTime).ToString("hh\\:mm\\:ss");

                                if (ii == phieus.Count - 1)
                                {
                                    //thoi gian thu ngan bat dau goi
                                    var nowTN = todayThuNgans.Where(x => x.Id == phieus[ii].Id && x.Start.HasValue).OrderByDescending(x => x.Start).FirstOrDefault();
                                    if (nowTN != null)
                                    {
                                        phieus[ii].StartTN = nowTN.Start;
                                        phieus[ii].str_StartTN = nowTN.Start.Value.ToString("dd/MM/yyyy HH:mm");

                                        //khoang thoi gian chờ tu luc ket thuc sua chua cho toi khi thu ngan goi
                                        if (phieus[ii].End.HasValue && phieus[ii].StartTN.HasValue)
                                            phieus[ii].WaitingTimeTN = phieus[ii].StartTN.Value.Subtract(phieus[ii].End.Value).ToString("hh\\:mm\\:ss");
                                    }
                                }
                                returnList.Add(phieus[ii]);
                                if (phieus[ii].End.HasValue)
                                    fakePrintTime = phieus[ii].End.Value;
                            }
                        }


                    }
                }
            }
            #endregion

            if (returnList.Count > 0)
            {
                if (objId > 0)
                    returnList = returnList.Where(x => x.ServiceId == objId).ToList();
                returnList = returnList.OrderBy(x => x.PrintTime).ThenBy(x => x.Number).ToList();
                for (int i = 0; i < returnList.Count; i++)
                    returnList[i].stt = i + 1;
            }
            return returnList;
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

        #region version 3
        public List<ReportModel> BaoCaoNangSuatDichVuVaThoiGianTrungBinh_TheoNV(SqlConnection sqlConnection, DateTime startDate, DateTime endDate, int thuNganId)
        {
            List<ReportModel> report = new List<ReportModel>(),
                queryResult = new List<ReportModel>();
            try
            {
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var emptyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                TimeSpan time = new TimeSpan(0, 0, 0);
                string query = "";
                SqlDataAdapter sqlDataAdapter;
                DataTable dataTable;

                query = "select Id, Name from Q_User where IsDeleted = 0 and id !=" + thuNganId;
                sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        report.Add(new ReportModel()
                        {
                            UserId = getIntValue(row["Id"]),
                            UserName = getStringValue(row["Name"]),
                            Number = 0,//sl cuoc goi
                            TongTGCho = emptyDate,//tong tg cho trung binh
                            TGChoTruocSC = emptyDate,//tg cho truoc sua chua
                            TGChoSauSC = emptyDate, //tg cho sau sua chua,
                            TGXuLyTT = emptyDate
                        });
                    }
                }

                if (startDate < today)
                {
                    query = "select UserId, u.Name, ProcessTime, EndProcessTime ,dr.PrintTime, TicketNumber from Q_HisDailyRequire_De dd,Q_HisDailyRequire dr, Q_User u where dd.HisDailyRequireId = dr.Id and u.Id = dd.UserId and dd.StatusId=" + (int)eStatus.HOTAT + " and dr.PrintTime >= '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime<='" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            queryResult.Add(new ReportModel()
                            {
                                UserId = getIntValue(row["UserId"]),
                                UserName = getStringValue(row["Name"]),
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                Start = getDateValue(row["ProcessTime"]),
                                End = getDateValue(row["EndProcessTime"]),
                                Number = getIntValue(row["TicketNumber"])
                            });
                        }
                    }
                }

                if (endDate >= today)
                {
                    query = "select UserId, u.Name, ProcessTime, EndProcessTime ,dr.PrintTime,TicketNumber from Q_DailyRequire_Detail dd,Q_DailyRequire dr, Q_User u where dd.DailyRequireId = dr.Id and u.Id = dd.UserId and dd.StatusId=" + (int)eStatus.HOTAT;
                    sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            queryResult.Add(new ReportModel()
                            {
                                UserId = getIntValue(row["UserId"]),
                                UserName = getStringValue(row["Name"]),
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                Start = getDateValue(row["ProcessTime"]),
                                End = getDateValue(row["EndProcessTime"]),
                                Number = getIntValue(row["TicketNumber"])
                            });
                        }
                    }
                }

                if (queryResult.Count > 0 && report.Count > 0)
                {

                    for (int i = 0; i < report.Count; i++)
                    {
                        report[i].Number = 0;//sl cuoc goi
                        report[i].TongTGCho = emptyDate;//tong tg cho trung binh
                        report[i].TGChoTruocSC = emptyDate;//tg cho truoc sua chua
                        report[i].TGChoSauSC = emptyDate;//tg cho sau sua chua
                        report[i].TGXuLyTT = emptyDate;//tg xu ly

                        var founds = queryResult.Where(x => x.UserId == report[i].UserId).ToList();
                        if (founds.Count > 0)
                        {
                            report[i].Number = founds.Count; // so luong cuoc goi
                            var aa = founds.Select(x => x.Number).ToList();
                            foreach (var item in founds)
                            {
                                if (item.Start.HasValue)
                                {
                                    //TG chờ truoc sửa chữa
                                    report[i].TGChoTruocSC = report[i].TGChoTruocSC.Value.Add(item.Start.Value.Subtract(item.PrintTime));

                                    //TG cho sau sủa chữa
                                    if (item.End.HasValue)
                                    {
                                        //TG xu lý
                                        report[i].TGXuLyTT = report[i].TGXuLyTT.Add(item.End.Value.Subtract(item.Start.Value));

                                        var thunganGoi = queryResult.FirstOrDefault(x => x.Number == item.Number && x.PrintTime == item.PrintTime && x.UserId == thuNganId);
                                        if (thunganGoi != null)
                                            report[i].TGChoSauSC = report[i].TGChoSauSC.Value.Add(thunganGoi.Start.Value.Subtract(item.End.Value));
                                    }
                                }
                            }

                            double seconds = report[i].TGChoTruocSC.Value.Subtract(emptyDate).TotalSeconds;
                            seconds += report[i].TGChoSauSC.Value.Subtract(emptyDate).TotalSeconds;
                            seconds += report[i].TGXuLyTT.Subtract(emptyDate).TotalSeconds;
                            var ss = emptyDate.AddSeconds(seconds);  
                            report[i].TongTGCho = report[i].TongTGCho.Value.Add(ss.TimeOfDay);

                            report[i].dTongTGCho = Math.Round(ss.Subtract(emptyDate).TotalMinutes);
                            report[i].dTGChoTruocSC = Math.Round(report[i].TGChoTruocSC.Value.Subtract(emptyDate).TotalMinutes);
                            report[i].dTGChoSauSC = Math.Round(report[i].TGChoSauSC.Value.Subtract(emptyDate).TotalMinutes);
                            report[i].dTGXuLyTT = Math.Round(report[i].TGXuLyTT.Subtract(emptyDate).TotalMinutes);
                        }
                        else
                            report[i].Number = 0;
                    }
                }
            }
            catch (Exception ex) { }
            return report;
        }

        public List<ReportModel> BaoCaoNangSuatDichVuVaThoiGianTrungBinh_TheoDV(SqlConnection sqlConnection, DateTime startDate, DateTime endDate, int thuNganId)
        {
            List<ReportModel> report = new List<ReportModel>(),
                queryResult = new List<ReportModel>();
            try
            {
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var emptyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                TimeSpan time = new TimeSpan(0, 0, 0);
                string query = "";
                SqlDataAdapter sqlDataAdapter;
                DataTable dataTable;

                query = "select Id, Name from Q_Service where IsDeleted = 0 and IsActived =1";
                sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        report.Add(new ReportModel()
                        {
                            ServiceId = getIntValue(row["Id"]),
                            ServiceName = getStringValue(row["Name"]),
                            Number = 0,//sl cuoc goi
                            TongTGCho = emptyDate,//tong tg cho trung binh
                            TGChoTruocSC = emptyDate,//tg cho truoc sua chua
                            TGChoSauSC = emptyDate, //tg cho sau sua chua
                            TGXuLyTT = emptyDate
                        });
                    }
                }

                if (startDate < today)
                {
                    query = "select UserId, u.Name, ProcessTime, EndProcessTime ,dr.PrintTime, TicketNumber,ServiceId from Q_HisDailyRequire_De dd,Q_HisDailyRequire dr, Q_User u where dd.HisDailyRequireId = dr.Id and u.Id = dd.UserId and dd.StatusId=" + (int)eStatus.HOTAT + " and dr.PrintTime >= '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime<='" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            queryResult.Add(new ReportModel()
                            {
                                UserId = getIntValue(row["UserId"]),
                                UserName = getStringValue(row["Name"]),
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                Start = getDateValue(row["ProcessTime"]),
                                End = getDateValue(row["EndProcessTime"]),
                                Number = getIntValue(row["TicketNumber"]),
                                ServiceId = getIntValue(row["ServiceId"])
                            });
                        }
                    }
                }

                if (endDate >= today)
                {
                    query = "select UserId, u.Name, ProcessTime, EndProcessTime ,dr.PrintTime,TicketNumber,ServiceId from Q_DailyRequire_Detail dd,Q_DailyRequire dr, Q_User u where dd.DailyRequireId = dr.Id and u.Id = dd.UserId and dd.StatusId=" + (int)eStatus.HOTAT;
                    sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            queryResult.Add(new ReportModel()
                            {
                                UserId = getIntValue(row["UserId"]),
                                UserName = getStringValue(row["Name"]),
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                Start = getDateValue(row["ProcessTime"]),
                                End = getDateValue(row["EndProcessTime"]),
                                Number = getIntValue(row["TicketNumber"]),
                                ServiceId = getIntValue(row["ServiceId"])
                            });
                        }
                    }
                }

                if (queryResult.Count > 0 && report.Count > 0)
                {

                    for (int i = 0; i < report.Count; i++)
                    {
                        report[i].Number = 0;//sl cuoc goi
                        report[i].TongTGCho = emptyDate;//tong tg cho trung binh
                        report[i].TGChoTruocSC = emptyDate;//tg cho truoc sua chua
                        report[i].TGChoSauSC = emptyDate;//tg cho sau sua chua
                        report[i].TGXuLyTT = emptyDate;//tg xu lý

                        var founds = queryResult.Where(x => x.ServiceId == report[i].ServiceId && x.UserId != thuNganId).ToList();
                        if (founds.Count > 0)
                        {
                            var aa = founds.Select(x => x.Number).ToList();
                            report[i].Number = founds.Count; // so luong cuoc goi
                            foreach (var item in founds)
                            {
                                if (item.Start.HasValue)
                                {
                                    //TG chờ truoc sửa chữa
                                    report[i].TGChoTruocSC = report[i].TGChoTruocSC.Value.Add(item.Start.Value.Subtract(item.PrintTime));

                                    //TG cho sau sủa chữa
                                    if (item.End.HasValue)
                                    {
                                        //TG chờ truoc sửa chữa                                    
                                        report[i].TGXuLyTT = report[i].TGXuLyTT.Add(item.End.Value.Subtract(item.Start.Value));

                                        var thunganGoi = queryResult.FirstOrDefault(x => x.Number == item.Number && x.PrintTime == item.PrintTime && x.UserId == thuNganId);
                                        if (thunganGoi != null)
                                            report[i].TGChoSauSC = report[i].TGChoSauSC.Value.Add(thunganGoi.Start.Value.Subtract(item.End.Value));
                                    }
                                }
                            }

                            double seconds = report[i].TGChoTruocSC.Value.Subtract(emptyDate).TotalSeconds;
                            seconds += report[i].TGChoSauSC.Value.Subtract(emptyDate).TotalSeconds;
                            seconds += report[i].TGXuLyTT.Subtract(emptyDate).TotalSeconds;
                            var ss = emptyDate.AddSeconds(seconds);
                            report[i].TongTGCho = report[i].TongTGCho.Value.Add(ss.TimeOfDay);

                            report[i].dTongTGCho = Math.Round( ss.Subtract(emptyDate).TotalMinutes);
                            report[i].dTGChoTruocSC = Math.Round(report[i].TGChoTruocSC.Value.Subtract(emptyDate).TotalMinutes);
                            report[i].dTGChoSauSC = Math.Round(report[i].TGChoSauSC.Value.Subtract(emptyDate).TotalMinutes);
                            report[i].dTGXuLyTT = Math.Round(report[i].TGXuLyTT.Subtract(emptyDate).TotalMinutes);
                        }
                        else
                            report[i].Number = 0;
                    }
                }
            }
            catch (Exception ex) { }
            return report;
        }

        public List<ReportModel> BaoCaoNangSuatDichVuChiTietTheoPhieu(SqlConnection sqlConnection, DateTime startDate, DateTime endDate, int thuNganId)
        {
            List<ReportModel> report = new List<ReportModel>(),
                queryResult = new List<ReportModel>();
            try
            {
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan time = new TimeSpan(0, 0, 0);
                string query = "";
                SqlDataAdapter sqlDataAdapter;
                DataTable dataTable;

                if (startDate < today)
                {
                    query = "select dr.STT_PhongKham, TicketNumber, ServiceId , s.Name as ServiceName , UserId, u.Name as UserName, ProcessTime, EndProcessTime ,dr.PrintTime,dr.ServeTimeAllow from Q_HisDailyRequire_De dd,Q_HisDailyRequire dr, Q_User u , Q_Service s where dd.HisDailyRequireId = dr.Id and u.Id = dd.UserId and s.Id = dr.ServiceId and dd.StatusId=" + (int)eStatus.HOTAT + " and dr.PrintTime >= '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr.PrintTime<='" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            queryResult.Add(new ReportModel()
                            {
                                STT_PhongKham = getStringValue(row["STT_PhongKham"]),
                                Number = getIntValue(row["TicketNumber"]),
                                ServiceId = getIntValue(row["ServiceId"]),
                                ServiceName = getStringValue(row["ServiceName"]),
                                UserId = getIntValue(row["UserId"]),
                                UserName = getStringValue(row["UserName"]),
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                Start = getDateValue(row["ProcessTime"]),
                                End = getDateValue(row["EndProcessTime"]),
                                ServeTime = getDateValue(row["ServeTimeAllow"]) ?? DateTime.Now
                            });
                        }
                    }
                }

                if (endDate >= today)
                {
                    query = "select dr.STT_PhongKham, TicketNumber, ServiceId , s.Name as ServiceName , UserId, u.Name as UserName, ProcessTime, EndProcessTime ,dr.PrintTime,dr.ServeTimeAllow from Q_DailyRequire_Detail dd,Q_DailyRequire dr, Q_User u ,Q_Service s where dd.DailyRequireId = dr.Id and u.Id = dd.UserId and s.Id = dr.ServiceId and dd.StatusId=" + (int)eStatus.HOTAT;
                    sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            queryResult.Add(new ReportModel()
                            {
                                STT_PhongKham = getStringValue(row["STT_PhongKham"]),
                                Number = getIntValue(row["TicketNumber"]),
                                ServiceId = getIntValue(row["ServiceId"]),
                                ServiceName = getStringValue(row["ServiceName"]),
                                UserId = getIntValue(row["UserId"]),
                                UserName = getStringValue(row["UserName"]),
                                PrintTime = getDateValue(row["PrintTime"]).Value,
                                Start = getDateValue(row["ProcessTime"]),
                                End = getDateValue(row["EndProcessTime"]),
                                ServeTime = getDateValue(row["ServeTimeAllow"]) ?? DateTime.Now
                            });
                        }
                    }
                }

                if (queryResult.Count > 0)
                {
                    ReportModel newObj = null;
                    var emptyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    var distincePrinTimes = queryResult.Select(x => x.PrintTime).Distinct().ToList();
                    for (int i = 0; i < distincePrinTimes.Count; i++)
                    {
                        var founds = queryResult.Where(x => x.PrintTime == distincePrinTimes[i]).OrderBy(x => x.Start.Value).ToList();
                        if (founds.Count > 0)
                        {
                            newObj = new ReportModel();
                            newObj.STT_PhongKham = founds[0].STT_PhongKham;
                            newObj.Number = founds[0].Number;
                            newObj.ServiceName = founds[0].ServiceName;
                            newObj.UserName = founds[0].UserName;
                            newObj.PrintTime = founds[0].PrintTime;
                            newObj.ServeTime = founds[0].ServeTime;
                            newObj.Start = founds[0].Start;
                            newObj.TGChoTruocSC = emptyDate.Add(founds[0].Start.Value.Subtract(founds[0].PrintTime));
                            newObj.TGXuLyTT = emptyDate.Add(founds[0].End.Value.Subtract(founds[0].Start.Value));
                            newObj.PhatSinh = (newObj.ServeTime.TimeOfDay.TotalSeconds >= newObj.TGXuLyTT.TimeOfDay.TotalSeconds ? false : true);

                            //TG cho sau sủa chữa
                            var thunganGoi = queryResult.FirstOrDefault(x => x.Number == founds[0].Number && x.PrintTime == founds[0].PrintTime && x.UserId == thuNganId);
                            if (thunganGoi != null)
                                newObj.TGChoSauSC = emptyDate.Add(thunganGoi.Start.Value.Subtract(founds[0].End.Value));
                            else
                                newObj.TGChoSauSC = emptyDate;
                            //tong tg chờ
                            newObj.TongTGCho = newObj.TGChoTruocSC.Value.Add(newObj.TGChoSauSC.Value.TimeOfDay);
                            report.Add(newObj);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return report;
        }

        #endregion
    }
}
