using GPRO.Core.Mvc;
using GPRO.Ultilities;
using Hugate.Framework;
using PagedList;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLUserMajor
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLUserMajor _Instance;  //volatile =>  tranh dung thread
        public static BLLUserMajor Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUserMajor();

                return _Instance;
            }
        }
        private BLLUserMajor() { }
        #endregion
        public List<UserMajorModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted).Select(x => new UserMajorModel() { Id = x.Id, UserId = x.UserId, MajorId = x.MajorId, Index = x.Index }).OrderBy(x => x.Index).ToList();
            }
        }
        public List<UserMajorModel> Gets(string connectString, int userId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).Select(x => new UserMajorModel() { Id = x.Id, UserId = x.UserId, MajorId = x.MajorId, Index = x.Index }).OrderBy(x => x.Index).ToList();
            }
        }
        public int GetUserFirstMajor(string connectString, int userId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).OrderBy(x => x.Index).FirstOrDefault();
                return (obj != null ? obj.MajorId : 0);
            }
        }

        public int Insert(string connectString, Q_UserMajor obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_UserMajor.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(string connectString, Q_UserMajor model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_UserMajor.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.UserId = model.UserId;
                        obj.MajorId = model.MajorId;
                        obj.Index = model.Index;
                        db.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_UserMajor.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_UserMajor model)
        {
            var obj = db.Q_UserMajor.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.UserId == model.UserId && x.MajorId == model.MajorId);
            return obj != null ? true : false;
        }

        public PagedList<UserMajorModel> GetList(string connectString, int userId, int startIndexRecord, int pageSize, string sorting)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Index ASC";

                    IQueryable<Q_UserMajor> objs = null;
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    objs = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && x.UserId == userId);

                    return new PagedList<UserMajorModel>(objs
                        .OrderBy(sorting)
                        .Select(x => new UserMajorModel()
                        {
                            Id = x.Id,
                            Index = x.Index,
                            MajorId = x.MajorId,
                            UserId = x.UserId,
                            UserName = x.Q_User.UserName,
                            MajorName = x.Q_Major.Name
                        }).ToList(), pageNumber, pageSize);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool CheckExists(UserMajorModel model)
        {
            try
            {
                var nv = db.Q_UserMajor.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && model.MajorId == x.MajorId && x.UserId == model.UserId);
                if (nv == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(string connectString, UserMajorModel model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var rs = new ResponseBase();
                    if (CheckExists(model))
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Insert", Message = "Nhân viên này đã được phân công nghiệp vụ này. Vui lòng chọn nghiệp vụ khác !." });
                    }
                    else
                    {
                        Q_UserMajor obj;
                        if (model.Id == 0)
                        {
                            obj = new Q_UserMajor();
                            Parse.CopyObject(model, ref obj);
                            db.Q_UserMajor.Add(obj);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.Q_UserMajor.FirstOrDefault(m => m.Id == model.Id);
                            if (obj == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                            }
                            else
                            {
                                obj.Index = model.Index;
                                obj.MajorId = model.MajorId;
                                obj.UserId = model.UserId;
                                rs.IsSuccess = true;
                            }
                        }
                        if (rs.IsSuccess)
                        {
                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                    }
                    return rs;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
