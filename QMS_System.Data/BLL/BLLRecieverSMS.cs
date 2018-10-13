using GPRO.Ultilities;
using PagedList;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLRecieverSMS
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLRecieverSMS _Instance;
        public static BLLRecieverSMS Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLRecieverSMS();

                return _Instance;
            }
        }
        private BLLRecieverSMS() { }
        #endregion

        private bool CheckExists(int Id, string keyword)
        {
            try
            {
                var nv = db.Q_RecieverSMS.FirstOrDefault(x => x.Id != Id && x.PhoneNumber.Trim().Equals(keyword));
                if (nv == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(Q_RecieverSMS model)
        {
            using (db = new QMSSystemEntities())
            {
                try
                {
                    var rs = new ResponseBase();
                    if (CheckExists(model.Id, model.PhoneNumber.Trim().ToUpper()))
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Insert", Message = "Số điện thoại này đã được sử dụng. Vui lòng nhập Số điện thoại khác !." });
                    }
                    else
                    {
                        Q_RecieverSMS obj;
                        if (model.Id == 0)
                        {
                            obj = new Q_RecieverSMS();
                            Parse.CopyObject(model, ref obj);
                            db.Q_RecieverSMS.Add(model);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.Q_RecieverSMS.FirstOrDefault(m => m.Id == model.Id);
                            if (obj == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                            }
                            else
                            {
                                obj.PhoneNumber = model.PhoneNumber;
                                obj.Note = model.Note;
                                obj.IsActive = model.IsActive;
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

        public List<RecieverSMSModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_RecieverSMS.Select(x => new RecieverSMSModel()
                {
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    IsActive = x.IsActive,
                    Note = x.Note
                }).ToList();
            }
        }

        public List<string> GetPhones()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_RecieverSMS.Where(x=>x.IsActive).Select(x => x.PhoneNumber  ).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_RecieverSMS.Where(x => !x.IsActive).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.PhoneNumber }).ToList();
            }
        }

        public ResponseBase Delete(int Id)
        {
            var rs = new ResponseBase();
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_RecieverSMS.FirstOrDefault(x => !x.IsActive && x.Id == Id);
                if (obj != null)
                {
                    db.Q_RecieverSMS.Remove(obj);
                    db.SaveChanges();
                    rs.IsSuccess = true;
                    rs.Errors.Add(new Error() { MemberName = "Thông báo", Message = "Xóa thành công." });
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Thông báo", Message = "Số điện thoại này đang được sử dụng hoặc không còn tồn tại trong hệ thống. Vui lòng kiểm tra lại!." });
                }
            }
            return rs;
        }

        public PagedList<RecieverSMSModel> Gets(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            using (db = new QMSSystemEntities())
            {
                try
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    IQueryable<Q_RecieverSMS> objs = null;
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    if (!string.IsNullOrEmpty(keyWord))
                        objs = db.Q_RecieverSMS.Where(x => x.PhoneNumber.Trim().Contains(keyWord.Trim())).OrderByDescending(x => x.Id);
                    else
                        objs = db.Q_RecieverSMS.OrderByDescending(x => x.Id);

                    if (objs != null && objs.Count() > 0)
                    {
                        var NhanVien = objs.Select(x => new RecieverSMSModel()
                        {
                            Id = x.Id,
                            PhoneNumber = x.PhoneNumber,
                            IsActive = x.IsActive,
                            Note = x.Note,
                        });
                        return new PagedList<RecieverSMSModel>(NhanVien.ToList(), pageNumber, pageSize);
                    }
                    else
                        return new PagedList<RecieverSMSModel>(new List<RecieverSMSModel>(), pageNumber, pageSize);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
