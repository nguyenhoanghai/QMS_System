using GPRO.Ultilities;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLCustomer
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLCustomer _Instance;  //volatile =>  tranh dung thread
        public static BLLCustomer Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCustomer();

                return _Instance;
            }
        }
        private BLLCustomer() { }
        #endregion

        private bool CheckExists(int Id, string keyword)
        {
            try
            {
                var nv = db.Q_Customer.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Code.Trim().ToUpper().Equals(keyword));
                if (nv == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase CreateOrUpdate(string connectString, Q_Customer model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var rs = new ResponseBase();
                    if (CheckExists(model.Id, model.Name.Trim().ToUpper()))
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Insert", Message = "Mã khách hàng này đã được sử dụng. Vui lòng nhập mã khác !." });
                    }
                    else
                    {
                        Q_Customer obj;
                        if (model.Id == 0)
                        {
                            obj = new Q_Customer();
                            Parse.CopyObject(model, ref obj);
                            db.Q_Customer.Add(model);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.Q_Customer.FirstOrDefault(m => m.Id == model.Id);
                            if (obj == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                            }
                            else
                            {
                                obj.Name = model.Name;
                                obj.Address = model.Address;
                                obj.Gender = model.Gender;
                                obj.Phone = model.Phone;
                                obj.Code = model.Code;
                                obj.YearOfBirth = model.YearOfBirth;
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

        public List<CustomerModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Customer.Where(x => !x.IsDeleted).Select(x => new CustomerModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Gender = x.Gender,
                    strGender = (x.Gender ? "Nam" : "Nữ"),
                    Address = x.Address,
                    Phone = x.Phone,
                    Code = x.Code,
                    YearOfBirth = x.YearOfBirth
                }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Customer.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Customer.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public CustomerModel Get(string connectString, int custId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                CustomerModel obj = null;
                var user = db.Q_Customer.FirstOrDefault(x => !x.IsDeleted && x.Id == custId);
                if (user != null)
                {
                    obj = new CustomerModel();
                    obj.Id = user.Id;
                    obj.Name = user.Name;
                    obj.Code = user.Code;
                    obj.Address = user.Address;
                    obj.Gender = user.Gender;
                    obj.Phone = user.Phone;
                    obj.YearOfBirth = user.YearOfBirth;
                }
                return obj;
            }
        }
        public CustomerModel Get(string connectString, string  custCode)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                CustomerModel obj = null;
                var user = db.Q_Customer.FirstOrDefault(x => !x.IsDeleted && x.Code.Trim().ToUpper() == custCode.Trim().ToUpper());
                if (user != null)
                {
                    obj = new CustomerModel();
                    obj.Id = user.Id;
                    obj.Name = user.Name;
                    obj.Code = user.Code;
                    obj.Address = user.Address;
                    obj.Gender = user.Gender;
                    obj.Phone = user.Phone;
                    obj.YearOfBirth = user.YearOfBirth;
                }
                return obj;
            }
        }

    }
}
