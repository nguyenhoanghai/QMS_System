using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.BLL
{
    public class BLLServiceInfo
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLServiceInfo _Instance;
        public static BLLServiceInfo Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLServiceInfo();

                return _Instance;
            }
        }
        private BLLServiceInfo() { }
        #endregion

        public List<ServiceInfoModel> GetServiceInfo()
        {
            using (db = new QMSSystemEntities())
            { 
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var yc = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.PrintTime >= today).ToList();
                var result = db.Q_Service.Select(x => new ServiceInfoModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    TotalCarsWaiting = yc.Where(c => c.StatusId == (int)eStatus.CHOXL).Select(c=>c.DailyRequireId).Distinct().Count(),
                }).ToList();
                if (result.Count > 0)
                { 
                    foreach (var item in result)
                    {
                        var obj = yc.Where(x => x.StatusId == (int)eStatus.DAGXL && item.MADV == x.MADV).FirstOrDefault();
                        item.TicketNumberProcessing = (obj != null ? (int)obj.MAPHIEU : 0);
                    }
                }
                return result;`
            }
        }

        public ResponseBase InsertServiceRequire(YEUCAU model)
        {
            db = new QMSEntities();
            try
            {
                var result = new ResponseBase();
                var requireObjs = db.YEUCAUs.Where(x => x.MADV == model.MADV).OrderByDescending(x => x.GIOCAP).ToList();
                var serviceObj = db.DICHVUs.Where(x => x.MADV == model.MADV).FirstOrDefault();
                double minutes = 0;
                DateTime time = DateTime.Now;
                if (serviceObj != null)
                {
                    model.GIOCAP = time;
                    model.MATT = eStatusName.Wating;
                    if (requireObjs.Count > 0)
                    {
                        model.MAPHIEU = requireObjs[0].MAPHIEU + 1;
                        var processObj = requireObjs.Where(x => eStatusName.Processing == x.MATT).FirstOrDefault();
                        if (processObj != null)
                            minutes = TimeSpan.Parse(serviceObj.THOIGIANXULY).TotalMinutes * ((int)requireObjs[0].MAPHIEU - (int)processObj.MAPHIEU);
                        else
                            minutes = TimeSpan.Parse(serviceObj.THOIGIANXULY).TotalMinutes * requireObjs.Count;
                        model.TGPHUCVU_DK = time.AddMinutes(minutes);
                    }
                    else
                    {
                        model.MAPHIEU = serviceObj.SOPHIEU.Value + 1;
                        model.TGPHUCVU_DK = time;
                    }
                    db.YEUCAUs.Add(model);
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Data = "Khách hàng có Số điện thoại : " + model.PHONE + " <br/>đã chọn Dịch vụ : " + serviceObj.TENDV + "<br/>được cấp STT : " + model.MAPHIEU + "<br/>Thời gian phục vụ dự kiến :" + model.TGPHUCVU_DK.Value.ToString("HH'h : 'mm");
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "them", Message = "Không tìm thấy Dịch Vu theo yêu cầu.Vui lòng thử lại." });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ResponseBase Find(string phone, int serviceId)
        {
            db = new QMSEntities();
            try
            {
                var result = new ResponseBase();
                var requireObjs = db.YEUCAUs.Where(x => x.MADV == serviceId && x.PHONE.Trim().Equals(phone.Trim())).OrderByDescending(x => x.GIOCAP).FirstOrDefault();
                var serviceObj = db.DICHVUs.Where(x => x.MADV == serviceId).FirstOrDefault();

                if (requireObjs != null)
                {
                    result.IsSuccess = true;
                    result.Data = "Khách hàng có SĐT:<b><span class=\"red\"> " + requireObjs.PHONE + "</span></b> <br/>Đã chọn dịch vụ: <b><span class=\"red\">" + serviceObj.TENDV + "</span></b><br/>Được cấp STT: <b><span class=\"red\">" + requireObjs.MAPHIEU + "</span></b><br/>Thời gian phục vụ dự kiến:<b><span class=\"red\"> " + requireObjs.TGPHUCVU_DK.Value.ToString("HH'h : 'mm") + "</span></b>";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "them", Message = "Không tìm thấy thông tin yêu cầu.Vui lòng thử lại." });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ModelSelectItem> GetServiceSelect()
        {
            db = new QMSEntities();
            return db.DICHVUs.OrderBy(x => x.MADV).Select(x => new ModelSelectItem() { Value = x.MADV, Name = x.TENDV }).ToList(); //(Int32)x.MANV có thể gây lỗi
        }
    }
}
