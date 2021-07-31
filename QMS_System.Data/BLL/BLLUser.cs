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
    public class BLLUser
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLUser _Instance;  //volatile =>  tranh dung thread
        public static BLLUser Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUser();

                return _Instance;
            }
        }
        private BLLUser() { }
        #endregion

        private bool CheckExists(int Id, string keyword)
        {
            try
            {
                var nv = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Name.Trim().ToUpper().Equals(keyword));
                if (nv == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase CreateOrUpdate(string connectString, UserModel model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var rs = new ResponseBase();
                    if (CheckExists(model.Id, model.Name.Trim().ToUpper()))
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Insert", Message = "Tên Nhân Viên này đã được sử dụng. Vui lòng nhập Tên khác !." });
                    }
                    else
                    {
                        Q_User obj;
                        if (model.Id == 0)
                        {
                            obj = new Q_User();
                            Parse.CopyObject(model, ref obj);
                            if (!string.IsNullOrEmpty(model.Image))
                                obj.Avatar = model.Image;
                            db.Q_User.Add(obj);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.Q_User.FirstOrDefault(m => m.Id == model.Id);
                            if (obj == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                            }
                            else
                            {
                                obj.Name = model.Name;
                                obj.Address = model.Address;
                                obj.Sex = model.Sex;
                                obj.Help = model.Help;
                                obj.UserName = model.UserName;
                                if (model.Password != obj.Password)
                                    obj.Password = model.Password;
                                if (!string.IsNullOrEmpty(model.Image))
                                    obj.Avatar = model.Image;
                                obj.Position = model.Position;
                                obj.WorkingHistory = model.WorkingHistory;
                                obj.Professional = model.Professional;
                                obj.Counters = model.Counters;
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

        public List<UserModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_User.Where(x => !x.IsDeleted).Select(x => new UserModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sex = x.Sex,
                    Address = x.Address,
                    UserName = x.UserName,
                    Password = x.Password,
                    Help = x.Help,
                    Avatar = x.Avatar,
                    Professional = x.Professional,
                    Position = x.Position,
                    WorkingHistory = x.WorkingHistory,
                    Counters = x.Counters
                }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_User.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int Insert(string connectString, Q_User obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                db.Q_User.Add(obj);
                db.SaveChanges();
                return obj.Id;
            }
        }

        public bool Update(string connectString, Q_User model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    obj.Name = model.Name;
                    obj.Sex = model.Sex;
                    obj.Address = model.Address;
                    obj.UserName = model.UserName;
                    obj.Password = model.Password;
                    obj.Help = model.Help;
                    obj.Avatar = model.Avatar;
                    obj.Professional = model.Professional;
                    obj.Position = model.Position;
                    obj.WorkingHistory = model.WorkingHistory;
                    obj.Counters = model.Counters;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public Login FindUser(string connectString, string sUsername, string sPassword)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                Login login = null;
                var obj = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.UserName.Equals(sUsername) && x.Password.Equals(sPassword));
                if (obj != null)
                {
                    login = new Login();
                    //login.UserName = sUsername;
                    //  login.strPassword = sPassword;
                    login.UserId = obj.Id;
                    login.UserName = obj.Name.ToString();
                }
                return login;
            }
        }

        public UserModel Get(string connectString, int userId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                UserModel obj = null;
                var user = db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.Id == userId);
                if (user != null)
                {
                    obj = new UserModel();
                    obj.Id = user.Id;
                    obj.Name = user.Name;
                    obj.Sex = user.Sex;
                    obj.Address = user.Address;
                    obj.Avatar = user.Avatar;
                    obj.Professional = user.Professional;
                    obj.Position = user.Position;
                    obj.WorkingHistory = user.WorkingHistory;
                    obj.UserName = user.UserName;
                    obj.Password = user.Password;
                    obj.Counters = user.Counters;
                }
                return obj;
            }
        }

        public UserModel GetByUserName(string connectString, string userName)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return (from x in db.Q_User
                        where !x.IsDeleted && x.UserName.Trim().Equals(userName)
                        select
                            new UserModel()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Sex = x.Sex,
                                Address = x.Address,
                                Avatar = x.Avatar,
                                Professional = x.Professional,
                                Position = x.Position,
                                WorkingHistory = x.WorkingHistory,
                                UserName = x.UserName,
                                Password = x.Password,
                                Counters = x.Counters
                            }).FirstOrDefault();
            }
        }

        public Q_User Get(string connectString, string userName, string password)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_User.Where(x => !x.IsDeleted && x.UserName.Trim().ToUpper().Equals(userName) && x.Password.Trim().ToUpper().Equals(password)).FirstOrDefault();
            }
        }

        public Q_User Get(string connectString, string username)
        {
            using (db = new QMSSystemEntities(connectString)) { return db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.UserName.Trim().Equals(username)); }
        }

        public PagedList<UserModel> GetList(string connectString, string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                try
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    IQueryable<Q_User> objs = null;
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    if (!string.IsNullOrEmpty(keyWord))
                        objs = db.Q_User.Where(x => !x.IsDeleted &&
                        (x.Name.Trim().ToUpper().Contains(keyWord.Trim().ToUpper()) ||
                        x.UserName.Trim().ToUpper().Contains(keyWord.Trim().ToUpper())));
                    else
                        objs = db.Q_User.Where(x => !x.IsDeleted);

                    return new PagedList<UserModel>(objs
                        .OrderBy(sorting)
                        .Select(x => new UserModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Address = x.Address,
                            Sex = x.Sex,
                            Help = x.Help,
                            UserName = x.UserName,
                            Password = x.Password,
                            Avatar = x.Avatar,
                            Professional = x.Professional,
                            Position = x.Position,
                            WorkingHistory = x.WorkingHistory,
                            Counters = x.Counters
                        }).ToList(), pageNumber, pageSize);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string GetUserAvatar(string connectString, string userName)
        {
            using (var _db = new QMSSystemEntities(connectString))
            {
                var user = _db.Q_User.FirstOrDefault(x => !x.IsDeleted && x.UserName.Trim().ToUpper().Equals(userName.ToUpper().Trim()));
                if (user != null && !string.IsNullOrEmpty(user.Avatar))
                    return user.Avatar;
                return string.Empty;
            }
        }
    }
}
