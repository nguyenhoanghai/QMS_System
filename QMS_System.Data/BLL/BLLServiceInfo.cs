using GPRO.Core.Mvc;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLServiceInfo
    {
        #region constructor 
        static object key = new object();
        private static volatile BLLServiceInfo _Instance;  //volatile =>  tranh dung thread
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

        public List<ServiceInfoModel> GetServiceInfo(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var result = db.Q_Service.Where(x => !x.IsDeleted && x.IsActived).Select(x => new ServiceInfoModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    // TotalCarsWaiting = x.Q_DailyRequire.Where(c => c.st == eStatusName.Wating).Count(),
                }).ToList();
                if (result.Count > 0)
                {
                    var dailyRequires = db.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.CHOXL).Select(x => new { StatusId = x.StatusId, ServiceId = x.Q_DailyRequire.ServiceId, TicketNumber = x.Q_DailyRequire.TicketNumber }).ToList();

                    foreach (var item in result)
                    {
                        var obj = dailyRequires.Where(x => x.StatusId == (int)eStatus.DAGXL && item.Id == x.ServiceId).FirstOrDefault();
                        item.TicketNumberProcessing = (obj != null ? (int)obj.TicketNumber : 0);

                        item.TotalCarsWaiting = dailyRequires.Where(x => x.ServiceId == item.Id && x.StatusId == (int)eStatus.CHOXL).Count();
                    }
                }
                return result;
            }
        }

        public ResponseBase InsertServiceRequire(Q_DailyRequire model, string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var result = new ResponseBase();
                    var checkExists = db.Q_DailyRequire.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber && x.ServiceId == model.ServiceId);
                    if (checkExists != null)
                    {
                        result.IsSuccess = true;
                        result.Data = "Khách hàng có Số điện thoại : " + checkExists.PhoneNumber + " <br/>đã đăng ký dịch vụ : " + checkExists.Q_Service.Name + "<br/>được cấp STT : " + checkExists.TicketNumber + "<br/>Thời gian phục vụ dự kiến :" + (checkExists.TGDKien.HasValue ? checkExists.TGDKien.Value.ToString("HH'h : 'mm") : "");

                    }
                    else
                    {
                        var dichvu = db.Q_Service.Where(x => x.Id == model.ServiceId).FirstOrDefault();
                        var serviceMajor = db.Q_ServiceStep.Where(x => !x.IsDeleted).OrderBy(x => x.Index).FirstOrDefault();
                        double minutes = 0;
                        DateTime time = DateTime.Now;
                        if (dichvu != null && serviceMajor != null)
                        {
                            var phieuDV = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == model.ServiceId).OrderByDescending(x => x.Q_DailyRequire.PrintTime).Select(x => new { PrintTime = x.Q_DailyRequire.PrintTime, TicketNumber = x.Q_DailyRequire.TicketNumber, StatusId = x.StatusId }).ToList();

                            model.PrintTime = time;
                            // model.sta = eStatusName.Wating;
                            if (phieuDV.Count > 0)
                            {
                                model.TicketNumber = phieuDV[0].TicketNumber + 1;
                                var phieuDangXL = phieuDV.Where(x => (int)eStatus.DAGXL == x.StatusId).FirstOrDefault();
                                if (phieuDangXL != null)
                                    minutes = (dichvu.TimeProcess.TimeOfDay).TotalMinutes * ((int)phieuDV[0].TicketNumber - (int)phieuDangXL.TicketNumber);
                                else
                                    minutes = (dichvu.TimeProcess.TimeOfDay).TotalMinutes * phieuDV.Count;
                                model.TGDKien = time.AddMinutes(minutes);
                            }
                            else
                            {
                                model.TicketNumber = dichvu.StartNumber;
                                model.TGDKien = time;
                            }

                            Q_DailyRequire_Detail modelDetail = new Q_DailyRequire_Detail();
                            modelDetail.Q_DailyRequire = model;
                            modelDetail.MajorId = serviceMajor.MajorId;
                            modelDetail.StatusId = (int)eStatus.CHOXL;

                            model.Q_DailyRequire_Detail = new List<Q_DailyRequire_Detail>();
                            model.Q_DailyRequire_Detail.Add(modelDetail);

                            db.Q_DailyRequire.Add(model);
                            db.SaveChanges();
                            result.IsSuccess = true;
                            result.Data = "Khách hàng có Số điện thoại : " + model.PhoneNumber + " <br/>đã chọn Dịch vụ : " + dichvu.Name + "<br/>được cấp STT : " + model.TicketNumber + "<br/>Thời gian phục vụ dự kiến :" + model.TGDKien.Value.ToString("HH'h : 'mm");
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "them", Message = "Không tìm thấy Dịch Vu theo yêu cầu.Vui lòng thử lại." });
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ResponseBase Find(string connectString, string phone, int serviceId)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var result = new ResponseBase();
                    var requireObjs = db.Q_DailyRequire.Where(x => x.ServiceId == serviceId && x.PhoneNumber.Trim().Equals(phone.Trim())).OrderByDescending(x => x.PrintTime).FirstOrDefault();
                    var serviceObj = db.Q_Service.Where(x => !x.IsDeleted && x.Id == serviceId).FirstOrDefault();

                    if (requireObjs != null)
                    {
                        result.IsSuccess = true;
                        result.Data = "Khách hàng có SĐT:<b><span class=\"red\"> " + requireObjs.PhoneNumber + "</span></b> <br/>Đã chọn dịch vụ: <b><span class=\"red\">" + serviceObj.Name + "</span></b><br/>Được cấp STT: <b><span class=\"red\">" + requireObjs.TicketNumber + "</span></b><br/>Thời gian phục vụ dự kiến:<b><span class=\"red\"> " + requireObjs.TGDKien.Value.ToString("HH'h : 'mm") + "</span></b>";
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
        }

    }
}
