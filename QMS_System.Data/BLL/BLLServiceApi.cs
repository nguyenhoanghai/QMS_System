using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using QMS_System.ThirdApp.Enum;
using System;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLServiceApi
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLServiceApi _Instance;
        public static BLLServiceApi Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLServiceApi();

                return _Instance;
            }
        }
        private BLLServiceApi() { }
        #endregion

        public ResponseBaseModel Next(string connectString, int userId, int equipCode, DateTime date, int useWithThirdPattern )
        {
            var res = new ResponseBaseModel(); 
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var oldTickets = db.Q_DailyRequire_Detail.Where(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL  );
                    if (oldTickets != null && oldTickets.Count() > 0)
                    {
                        foreach (var item in oldTickets)
                        {
                            item.StatusId = (int)eStatus.HOTAT;
                            item.EndProcessTime = DateTime.Now;
                        }
                        db.SaveChanges();
                    }

                    var majorIds = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).OrderBy(x => x.Index).Select(x => x.MajorId).ToList();
                    if (majorIds.Count > 0)
                    {
                        for (int i = 0; i < majorIds.Count; i++)
                        {
                            int a = majorIds[i];
                            int ticket = 0;
                            var newTicket = db.Q_DailyRequire_Detail.Where(x => x.MajorId == a && (x.StatusId == (int)eStatus.CHOXL || x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.QUALUOT)   && !x.Q_DailyRequire.Q_Service.isKetLuan).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
                            if (newTicket != null)
                            {
                                newTicket.UserId = userId;
                                newTicket.EquipCode = equipCode;
                                newTicket.ProcessTime = DateTime.Now;
                                newTicket.StatusId = (int)eStatus.DAGXL;

                                if (useWithThirdPattern == 0)
                                    ticket = newTicket.Q_DailyRequire.TicketNumber;
                                else
                                {
                                    try
                                    {
                                        ticket = int.Parse(newTicket.Q_DailyRequire.STT_PhongKham);
                                    }
                                    catch (Exception)
                                    {
                                        ticket = newTicket.Q_DailyRequire.TicketNumber;
                                    }
                                }
                                 
                                var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                                res.IsSuccess = true;
                                res.Data_3 = new TicketInfo()
                                {
                                    RequireDetailId = newTicket.Id,
                                    RequireId = newTicket.DailyRequireId,
                                    StartTime = newTicket.ProcessTime.Value.TimeOfDay,
                                    TimeServeAllow = newTicket.Q_DailyRequire.ServeTimeAllow,
                                    TicketNumber = ticket,
                                    CounterId = equip.CounterId,
                                    PrintTime = newTicket.Q_DailyRequire.PrintTime,
                                    EquipCode = equipCode,
                                    Note = (newTicket.Q_DailyRequire.MaPhongKham + "," + newTicket.Q_DailyRequire.MaBenhNhan + "," + newTicket.Q_DailyRequire.CustomerName + "," + newTicket.Q_DailyRequire.CustomerDOB)
                                };

                                if (equip != null)
                                    //switch (dailyRequireType)
                                    //{
                                    //    case (int)eDailyRequireType.KhamBenh:
                                            db.Database.ExecuteSqlCommand("update Q_Counter set LastCall =" + ticket + ", CurrentNumber=" + ticket + " where Id =" + equip.CounterId);
                                    //        break;
                                    //    case (int)eDailyRequireType.KetLuan:
                                    //        db.Database.ExecuteSqlCommand("update Q_Counter set LastCallKetLuan =" + ticket + "  where Id =" + equip.CounterId);
                                    //        break;
                                    //}

                                db.Database.ExecuteSqlCommand(@"update  Q_RequestTicket  set isdeleted= 1 where userId=" + userId);
                                db.SaveChanges();
                                break;
                            }
                        }
                    }
                    return res;
                }
            }
            catch (Exception)
            { }
            return res;
        }
         
    }
}
