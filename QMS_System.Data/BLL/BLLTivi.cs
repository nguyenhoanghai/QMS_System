using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLTivi
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLTivi _Instance;  //volatile =>  tranh dung thread
        public static BLLTivi Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLTivi();

                return _Instance;
            }
        }
        private BLLTivi() { }
        #endregion

        public List<CounterDayInfoModel> Gets(string connectString, List<int> equipmentCodes)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var counters = (from x in db.Q_CounterDayInfo
                                    where equipmentCodes.Contains(x.EquipCode) && !x.Q_Counter.IsDeleted
                                    orderby x.Q_Counter.Index
                                    select new CounterDayInfoModel()
                                    {
                                        STT_3 = x.STT_3,
                                        STT = x.STT,
                                        STT_QMS = x.STT_QMS,
                                        STT_UT = x.STT_UT,
                                        CounterId = x.CounterId,
                                        TenQuay = x.Q_Counter.Name,
                                        TGBatDau = x.StartTime,
                                        TGChuan = x.ServeTime,
                                        TGInPhieu = x.PrintTime,
                                        TT_STT = x.StatusSTT,
                                        TT_STT_UT = x.StatusSTT_UT,
                                        EquipCode = x.EquipCode
                                    }).ToList();
                    if (counters.Count > 0)
                    {
                        string query = string.Empty;
                        for (int i = 0; i < counters.Count; i++)
                        {
                            string _trangthai = "..:..";
                            if (counters[i].TGInPhieu.HasValue &&
                                (int)eStatus.HOTAT != counters[i].TT_STT &&
                                (int)eStatus.PHATSINH != counters[i].TT_STT)
                            {
                                DateTime tgtc = counters[i].TGBatDau.Value.Add(counters[i].TGChuan.Value.TimeOfDay);
                                TimeSpan tgcl = tgtc.TimeOfDay.Subtract(DateTime.Now.TimeOfDay);
                                if (tgcl >= new TimeSpan(0, 0, 0))
                                {
                                    _trangthai = "Process";
                                    counters[i].TGConLai = tgcl;
                                    //  counters[i].TGXuLy = 
                                }
                                else
                                {
                                    counters[i].TT_STT = (int)eStatus.PHATSINH;
                                    _trangthai = "Over";
                                    query += "  Update [Q_CounterDayInfo] set [StatusSTT]=-1 where [CounterId]=" + counters[i].CounterId;
                                }
                            }
                            else if (counters[i].TT_STT != 0)
                            {
                                if ((int)eStatus.HOTAT == counters[i].TT_STT)
                                    _trangthai = "Complete";
                                else if ((int)eStatus.PHATSINH == counters[i].TT_STT)
                                    _trangthai = "Over";
                            }

                            counters[i].TrangThai = _trangthai;
                        }
                        if (!string.IsNullOrEmpty(query))
                            db.SaveChanges();
                    }
                    return counters;
                }
                catch (Exception ex) { }
                return new List<CounterDayInfoModel>();
            }
        }

    }
}
