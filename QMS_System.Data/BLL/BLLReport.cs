using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
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
                                    if(thunganObj.Start.Value > newObj.End.Value)
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
    }
}
