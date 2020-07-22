using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace QMS_System.Data.BLL.VietThaiQuan
{
    public class BLLVietThaiQuan
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLVietThaiQuan _Instance;  //volatile =>  tranh dung thread
        public static BLLVietThaiQuan Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLVietThaiQuan();

                return _Instance;
            }
        }
        private BLLVietThaiQuan() { }
        #endregion

        public ResponseBase ThemPhieu(string connectString, int stt, int dichvuId, string maphieuDichVu, string maCongViec, string maLoaiCongViec, TimeSpan? serveTimeAllow,DateTime printTime)
        {
            var rs = new ResponseBase();
            Q_DailyRequire rq = null,
                     modelObj = new Q_DailyRequire();
            int _serviceNumber = 0;
            using (var db = new QMSSystemEntities(connectString))
            {
                if (!string.IsNullOrEmpty(maCongViec) && !string.IsNullOrEmpty(maLoaiCongViec))
                {
                    modelObj = LayGioPhucVuDuKien(db, maCongViec, maLoaiCongViec, modelObj);
                    if (serveTimeAllow.HasValue)
                        modelObj.ServeTimeAllow = modelObj.ServeTimeAllow.Add(serveTimeAllow.Value);
                    serveTimeAllow = modelObj.ServeTimeAllow;
                }

                var ser = db.Q_Service.FirstOrDefault(x => x.Id == dichvuId);
                if (ser != null)
                {
                    _serviceNumber = ser.StartNumber;
                    if (!serveTimeAllow.HasValue)
                        serveTimeAllow = ser.TimeProcess.TimeOfDay;
                }

                var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == dichvuId).OrderBy(x => x.Index).FirstOrDefault();
                if (nv == null)
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                }
                else
                {
                    modelObj.TicketNumber = stt;
                    modelObj.ServiceId = dichvuId;
                    modelObj.BusinessId = null; 
                    modelObj.PrintTime = printTime;
                    modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                    modelObj.CustomerName = "";// Name;
                    modelObj.CustomerDOB = 0;// DOB;
                    modelObj.CustomerAddress = "";// Address;
                    modelObj.MaBenhNhan = "";// MaBenhNhan;
                    modelObj.MaPhongKham = "";// MaPhongKham;
                    modelObj.STT_PhongKham = maphieuDichVu;
                    modelObj.CarNumber = "";// SoXe;
                    modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                    var foundService = db.Q_Service.FirstOrDefault(x => x.Id == dichvuId);

                    var detail = new Q_DailyRequire_Detail();
                    detail.Q_DailyRequire = modelObj;
                    detail.MajorId = nv.MajorId;
                    detail.StatusId = (int)eStatus.CHOXL;

                    if (foundService != null && foundService.AutoEnd)
                    {
                        detail.StatusId = (int)eStatus.HOTAT;
                        var now = DateTime.Now;
                        var timeend = foundService.TimeAutoEnd != null ? foundService.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                        detail.ProcessTime = now;
                        detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                    }
                    modelObj.Q_DailyRequire_Detail.Add(detail);
                    db.Q_DailyRequire.Add(modelObj);

                    var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                    var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                    var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();
                    var tq = (from x in db.Q_Equipment
                              where
                             (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                              !x.IsDeleted &&
                              !x.Q_Counter.IsDeleted
                              select x.Q_Counter.Name).ToArray();
                    var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                    db.SaveChanges();
                    rs.IsSuccess = true;
                    rs.Data = 0;
                    rs.Data_1 = detail.MajorId;
                    rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                    rs.Data_2 = tqs;
                    rs.Data_3 = modelObj.TicketNumber;
                }
            } 
            return rs;
        }

        private Q_DailyRequire LayGioPhucVuDuKien(QMSSystemEntities db, string maCongViec, string maLoaiCongViec, Q_DailyRequire model)
        {
            DateTime processTime = new DateTime(2020, 1, 1, 00, 00, 00);
            string[] codes = maCongViec.Split(',').Select(x => x.Trim().ToUpper()).ToArray();
            if (codes != null && codes.Length > 0)
            {
                var a = (from x in db.Q_WorkDetail
                         where !x.IsDeleted
                         select new ModelSelectItem() { Code = x.Q_Works.Code, Name = x.Q_WorkType.Code }).ToList();
                var congviecs = (from x in db.Q_WorkDetail
                                 where !x.IsDeleted &&
                                 codes.Contains(x.Q_Works.Code.Trim().ToUpper()) &&
                                 maLoaiCongViec.Trim().ToUpper().Equals(x.Q_WorkType.Code.Trim().ToUpper())
                                 select new { TimeProcess = x.TimeProcess, Id = x.Id }).ToArray();
                model.Q_DailyRequire_WorkDetail = new List<Q_DailyRequire_WorkDetail>();
                for (int i = 0; i < congviecs.Count(); i++)
                {
                    processTime = processTime.AddSeconds(congviecs[i].TimeProcess.TimeOfDay.TotalSeconds);
                    model.Q_DailyRequire_WorkDetail.Add(new Q_DailyRequire_WorkDetail()
                    {
                        Q_DailyRequire = model,
                        WorkDetailId = congviecs[i].Id
                    });
                }
            }
            model.ServeTimeAllow = processTime.TimeOfDay;
            return model;
        }

    }
}
