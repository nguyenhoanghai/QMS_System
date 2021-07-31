using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Hugate.Framework;
using PagedList;
using GPRO.Ultilities;
using GPRO.Core.Mvc;

namespace QMS_System.Data.BLL
{
    public class BLLMajor
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLMajor _Instance;  //volatile =>  tranh dung thread
        public static BLLMajor Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLMajor();

                return _Instance;
            }
        }
        private BLLMajor() { }
        #endregion
        public List<MajorModel> Gets(string connectString )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    return db.Q_Major.Where(x => !x.IsDeleted).Select(x => new MajorModel() { Id = x.Id, Name = x.Name, Note = x.Note }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Major.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int Insert(string connectString,Q_Major obj )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_Major.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(string connectString,Q_Major model )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Name = model.Name;
                        obj.Note = model.Note;
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
        }

        public bool Delete(string connectString,int Id )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Major model)
        {
            Q_Major obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }

        public PagedList<MajorModel> GetList(string connectString, string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Name ASC";

                    IQueryable<Q_Major> objs = null;
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    if (!string.IsNullOrEmpty(keyWord))
                        objs = db.Q_Major.Where(x => !x.IsDeleted && x.Name.Trim().ToUpper().Contains(keyWord.Trim().ToUpper()));
                    else
                        objs = db.Q_Major.Where(x => !x.IsDeleted);

                    return new PagedList<MajorModel>(objs
                        .OrderBy(sorting)
                        .Select(x => new MajorModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            IsShow = x.IsShow,
                            Note = x.Note,
                        }).ToList(), pageNumber, pageSize);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool CheckExists(int Id, string keyword)
        {
            try
            {
                var nv = db.Q_Major.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Name.Trim().ToUpper().Equals(keyword));
                if (nv == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(string connectString, MajorModel model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var rs = new ResponseBase();
                    if (CheckExists(model.Id, model.Name.Trim().ToUpper()))
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Insert", Message = "Tên nghiệp vụ này đã được sử dụng. Vui lòng nhập Tên khác !." });
                    }
                    else
                    {
                        Q_Major obj;
                        if (model.Id == 0)
                        {
                            obj = new Q_Major();
                            Parse.CopyObject(model, ref obj);
                            db.Q_Major.Add(obj);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.Q_Major.FirstOrDefault(m => m.Id == model.Id);
                            if (obj == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                            }
                            else
                            {
                                obj.Name = model.Name;
                                obj.IsShow = model.IsShow; 
                                obj.Note = model.Note;
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
