using GPRO.Core.Mvc;
using GPRO.Ultilities;
using PagedList;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLEvaluate
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLEvaluate _Instance;  //volatile =>  tranh dung thread
        public static BLLEvaluate Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLEvaluate();

                return _Instance;
            }
        }
        private BLLEvaluate() { }
        #endregion
        public List<EvaluateModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Evaluate.Where(x => !x.IsDeleted).OrderBy(x => x.Index).Select(x => new EvaluateModel() { Id = x.Id, Name = x.Name, Note = x.Note }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Evaluate.Where(x => !x.IsDeleted).OrderBy(x => x.Index).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int Insert(string connectString, Q_Evaluate obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_Evaluate.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public int Update(string connectString, Q_Evaluate model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Evaluate.FirstOrDefault(x => x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Index = model.Index;
                        obj.Name = model.Name;
                        obj.Note = model.Note;
                        db.SaveChanges();
                        return obj.Id;
                    }
                    else
                        return 0;
                }
                return 0;
            }
        }
        public ResponseBase InsertOrUpdate(string connectString, Q_Evaluate obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var rs = new ResponseBase();
                try
                {
                    if (obj.Id == 0)
                    {
                        db.Q_Evaluate.Add(obj);
                        rs.IsSuccess = false;
                    }
                    else
                    {
                        var oldObj = db.Q_Evaluate.FirstOrDefault(x => x.Id == obj.Id); ;
                        if (oldObj != null)
                        {
                            oldObj.Index = obj.Index;
                            oldObj.Name = obj.Name;
                            oldObj.Note = obj.Note;
                        }
                    }
                    db.SaveChanges();
                    rs.IsSuccess = true;
                }
                catch { }
                return rs;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Evaluate.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Evaluate model)
        {
            Q_Evaluate obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Evaluate.FirstOrDefault(x => x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }

        public List<EvaluateModel> GetWithChild(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var parents = db.Q_Evaluate.Where(x => !x.IsDeleted).OrderBy(x => x.Index).Select(x => new EvaluateModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Index = x.Index
                }).ToList();
                if (parents.Count > 0)
                {
                    var childs = db.Q_EvaluateDetail.Where(x => !x.IsDeleted).Select(x => new EvaluateDetailModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Index = x.Index,
                        EvaluateId = x.EvaluateId,
                        IsDefault = x.IsDefault,
                        Icon = x.Icon
                    }).ToList();
                    if (childs.Count > 0)
                    {
                        foreach (var item in parents)
                        {
                            item.Childs.AddRange(childs.Where(x => x.EvaluateId == item.Id).OrderBy(x => x.Index).ToList());
                        }
                        return parents;
                    }
                }
                return null;
            }

        }

        public PagedList<EvaluateModel> GetList(string connectString, string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    IEnumerable<Q_Evaluate> objs = null;
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Index DESC";
                    if (!string.IsNullOrEmpty(keyWord))
                        objs = db.Q_Evaluate.Where(x => !x.IsDeleted && x.Name.Trim().ToUpper().Contains(keyWord.Trim().ToUpper())).OrderBy(x => x.Index);
                    else
                        objs = db.Q_Evaluate.Where(x => !x.IsDeleted).OrderBy(x => x.Index);

                    if (objs != null && objs.Count() > 0)
                        return new PagedList<EvaluateModel>(objs.Select(x => new EvaluateModel() { Id = x.Id, Name = x.Name, Index = x.Index, Note = x.Note }).ToList(), pageNumber, pageSize);
                    else
                        return new PagedList<EvaluateModel>(new List<EvaluateModel>(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class BLLEvaluateDetail
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLEvaluateDetail _Instance;  //volatile =>  tranh dung thread
        public static BLLEvaluateDetail Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLEvaluateDetail();

                return _Instance;
            }
        }
        private BLLEvaluateDetail() { }
        #endregion
        public List<EvaluateDetailModel> Gets(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_EvaluateDetail.Where(x => !x.IsDeleted && !x.Q_Evaluate.IsDeleted && x.EvaluateId == Id).OrderBy(x => x.Index).Select(x => new EvaluateDetailModel() { Id = x.Id, Name = x.Name, Note = x.Note }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_EvaluateDetail.Where(x => !x.IsDeleted).OrderBy(x => x.Index).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public ResponseBase InsertOrUpdate(string connectString, EvaluateDetailModel model)
        {
            var rs = new ResponseBase();
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(model))
                {
                    if (model.Id == 0)
                    {
                        Q_EvaluateDetail obj = new Q_EvaluateDetail();
                        Parse.CopyObject(model, ref obj);
                        if (!string.IsNullOrEmpty(model.Image))
                            obj.Icon = model.Image;
                        db.Q_EvaluateDetail.Add(obj);
                        rs.IsSuccess = false;
                    }
                    else
                    {
                        var oldObj = db.Q_EvaluateDetail.FirstOrDefault(x => !x.IsDeleted && !x.Q_Evaluate.IsDeleted && x.Id == model.Id);
                        if (oldObj == null)
                        {
                            rs.IsSuccess = false;
                            rs.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                        }
                        else
                        {
                            oldObj.EvaluateId = model.EvaluateId;
                            oldObj.Index = model.Index;
                            oldObj.Name = model.Name;
                            oldObj.IsDefault = model.IsDefault;
                            oldObj.Note = model.Note;
                            if (!string.IsNullOrEmpty(model.Image))
                                oldObj.Icon = model.Image;
                            oldObj.SmsContent = null;
                            oldObj.IsSendSMS = model.IsSendSMS;
                            if (model.IsSendSMS)
                                oldObj.SmsContent = model.SmsContent;
                        }
                    }
                    db.SaveChanges();
                    rs.IsSuccess = true;
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Insert", Message = "Tên thang đánh giá này đã được sử dụng. Vui lòng nhập Tên khác !." });
                }
                return rs;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_EvaluateDetail.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists( EvaluateDetailModel model)
        {
            Q_EvaluateDetail obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_EvaluateDetail.FirstOrDefault(x => x.Id != model.Id && x.EvaluateId == model.EvaluateId && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
        public PagedList<EvaluateDetailModel> Gets(string connectString, int type, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Index DESC";
                    var objs = db.Q_EvaluateDetail.Where(x => !x.IsDeleted && x.EvaluateId == type).OrderBy(x => x.Index).Select(x => new EvaluateDetailModel()
                    {
                        Id = x.Id,
                        EvaluateId = x.EvaluateId,
                        Index = x.Index,
                        Name = x.Name,
                        Note = x.Note,
                        IsDefault = x.IsDefault,
                        Icon = x.Icon,
                        IsSendSMS = x.IsSendSMS,
                        SmsContent = x.SmsContent
                    }).ToList();
                    return new PagedList<EvaluateDetailModel>(objs, pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
