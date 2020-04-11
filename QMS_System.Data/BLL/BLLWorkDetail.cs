using QMS_System.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLWorkDetail
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLWorkDetail _Instance;  //volatile =>  tranh dung thread
        public static BLLWorkDetail Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLWorkDetail();

                return _Instance;
            }
        }
        private BLLWorkDetail() { }
        #endregion

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return (from x in db.Q_WorkDetail
                        where !x.IsDeleted && !x.Q_Works.IsDeleted && !x.Q_WorkType.IsDeleted
                        select
                        new ModelSelectItem()
                        {
                            Id = x.Id,
                            Name = x.Q_Works.Name,
                            Code = x.TimeProcess.TimeOfDay.ToString(),
                            Data = x.WorkTypeId,
                            Data1 = x.Q_WorkType.Code
                        }).ToList();
            }
        }

        public List<WorkDetailModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return (from x in db.Q_WorkDetail
                        where !x.IsDeleted && !x.Q_Works.IsDeleted && !x.Q_WorkType.IsDeleted
                        select
                        new WorkDetailModel()
                        {
                            Id = x.Id,
                            WorkName = x.Q_Works.Name,
                            TimeProcess = x.TimeProcess,
                            WorkTypeId = x.WorkTypeId,
                            WorkTypeCode = x.Q_WorkType.Code,
                            WorkTypeName = x.Q_WorkType.Name,
                            WorkId = x.WorkId
                        }).ToList();
            }
        }

        public List<WorkDetailModel> Gets(string connectString, int ticketNumber)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var workDetail = db.Q_DailyRequire_WorkDetail.FirstOrDefault(x => x.Q_DailyRequire.TicketNumber == ticketNumber);
                if (workDetail != null)
                    return (from x in db.Q_WorkDetail
                            where !x.IsDeleted && !x.Q_Works.IsDeleted && !x.Q_WorkType.IsDeleted && x.WorkTypeId == workDetail.Q_WorkDetail.WorkTypeId
                            select
                            new WorkDetailModel()
                            {
                                Id = x.Id,
                                WorkName = x.Q_Works.Name,
                                TimeProcess = x.TimeProcess,
                                WorkTypeId = x.WorkTypeId,
                                WorkTypeCode = x.Q_WorkType.Code,
                                WorkTypeName = x.Q_WorkType.Name,
                                WorkId = x.WorkId
                            }).ToList();
                return new List<WorkDetailModel>();
            }
        }

        public List<WorkDetailModel> GetWithWorkType(string connectString, int workTypeId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return (from x in db.Q_WorkDetail
                        where !x.IsDeleted && !x.Q_Works.IsDeleted && !x.Q_WorkType.IsDeleted && x.WorkTypeId == workTypeId
                        select
                        new WorkDetailModel()
                        {
                            Id = x.Id,
                            WorkName = x.Q_Works.Name,
                            TimeProcess = x.TimeProcess,
                            WorkTypeId = x.WorkTypeId,
                            WorkTypeCode = x.Q_WorkType.Code,
                            WorkTypeName = x.Q_WorkType.Name,
                            WorkId = x.WorkId
                        }).ToList();
            }
        }

        public Q_WorkDetail Get(string connectString, int WorkDetailId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_WorkDetail.FirstOrDefault(x => !x.IsDeleted && x.Id == WorkDetailId);
            }
        }

        public ResponseBaseModel Insert(string connectString, Q_WorkDetail model)
        {
            var rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(model))
                {
                    if (model.Id == 0)
                        db.Q_WorkDetail.Add(model);
                    else
                    {
                        var obj = db.Q_WorkDetail.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (obj != null)
                        {
                            obj.WorkId = model.WorkId;
                            obj.WorkTypeId = model.WorkTypeId;
                            obj.TimeProcess = model.TimeProcess;
                        }
                        else
                        {
                            rs.IsSuccess = false;
                            rs.sms = "Chi tiết công việc đã bị xóa hoặc không tồn tại trong hệ thống. Vui lòng kiểm tra lại!.";
                        }
                    }
                    db.SaveChanges();
                    rs.IsSuccess = true;
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Loại công việc đã tồn tại công việc này. Vui lòng chọn công việc khác!.";
                }
            }
            return rs;
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_WorkDetail.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(Q_WorkDetail model)
        {
            var obj = db.Q_WorkDetail.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.WorkId == model.WorkId && x.WorkTypeId == model.WorkTypeId);
            return obj != null ? true : false;
        }

        public List<WorkDetailModel> GetDailyRequire_WorkDetail(string connectString, int dailyRequireId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                  var objs = (from x in db.Q_DailyRequire_WorkDetail
                         where !x.IsDeleted && !x.Q_WorkDetail.Q_Works.IsDeleted && !x.Q_WorkDetail.Q_WorkType.IsDeleted && x.DailyRequireId  == dailyRequireId
                            select
                        new WorkDetailModel()
                        {
                            Id = x.Id,
                            WorkName = x.Q_WorkDetail.Q_Works.Name,
                            TimeProcess = x.Q_WorkDetail.TimeProcess,
                            WorkTypeId = x.Q_WorkDetail.WorkTypeId,
                            WorkTypeCode = x.Q_WorkDetail.Q_WorkType.Code,
                            WorkTypeName = x.Q_WorkDetail.Q_WorkType.Name,
                            WorkId = x.Q_WorkDetail.WorkId
                        });
                return objs.ToList();
            }
        }

        public ResponseBaseModel AddWorkToTicket(string connectString, int requireId, int workDetailId)
        {
            var rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                var checkExists = db.Q_DailyRequire_WorkDetail.FirstOrDefault(x => !x.IsDeleted && x.DailyRequireId == requireId && x.WorkDetailId == workDetailId);
                if (checkExists == null)
                {
                    var requireObj = db.Q_DailyRequire.FirstOrDefault(x => x.Id == requireId);
                    var workDetail = db.Q_WorkDetail.FirstOrDefault(x => x.Id == workDetailId);
                    if (requireObj != null && workDetail != null)
                    {
                        requireObj.ServeTimeAllow = requireObj.ServeTimeAllow.Add(workDetail.TimeProcess.TimeOfDay);
                        db.Q_DailyRequire_WorkDetail.Add(new Q_DailyRequire_WorkDetail()
                        {
                            DailyRequireId = requireId,
                            WorkDetailId = workDetailId
                        });
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.sms = "Chi tiết công việc đã bị xóa hoặc không tồn tại trong hệ thống. Vui lòng kiểm tra lại!.";
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Phiếu bạn đang chọn đã tồn tại công việc này. Vui lòng chọn công việc khác!.";
                }
            }
            return rs;
        }

        public ResponseBaseModel DeleteWorkFromTicket(string connectString, int requireWorkDetailId)
        {
            var rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                var checkExists = db.Q_DailyRequire_WorkDetail.FirstOrDefault(x => !x.IsDeleted && x.Id == requireWorkDetailId);
                if (checkExists != null)
                {
                    var requireObj = db.Q_DailyRequire.FirstOrDefault(x => x.Id == checkExists.DailyRequireId);
                    var workDetail = db.Q_WorkDetail.FirstOrDefault(x => x.Id == checkExists.WorkDetailId);
                    if (requireObj != null && workDetail != null)
                    {
                        requireObj.ServeTimeAllow = requireObj.ServeTimeAllow.Subtract(workDetail.TimeProcess.TimeOfDay);
                        db.Database.ExecuteSqlCommand("delete Q_DailyRequire_WorkDetail where id =" + checkExists.Id);
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.sms = "Chi tiết công việc đã bị xóa hoặc không tồn tại trong hệ thống. Vui lòng kiểm tra lại!.";
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Phiếu bạn đang chọn không tồn tại công việc này. Vui lòng làm mới lại lưới công việc.";
                }
            }
            return rs;
        }

    }
}
