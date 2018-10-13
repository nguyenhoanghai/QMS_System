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
        public List<UserMajorModel> Gets( )
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted ).Select(x => new UserMajorModel() { Id = x.Id, UserId = x.UserId, MajorId = x.MajorId, Index = x.Index }).OrderBy(x=>x.Index).ToList();
            }
        }
        public List<UserMajorModel> Gets(int userId)
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).Select(x => new UserMajorModel() { Id = x.Id, UserId = x.UserId, MajorId = x.MajorId, Index = x.Index }).OrderBy(x => x.Index).ToList();
            }
        }
        public int GetUserFirstMajor(int userId)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).OrderBy(x => x.Index).FirstOrDefault();
                return (obj != null ? obj.MajorId : 0);
            }
        }

        public int Insert(Q_UserMajor obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_UserMajor.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(Q_UserMajor model)
        {
            using (db = new QMSSystemEntities())
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

        public bool Delete(int Id)
        {
            using (db = new QMSSystemEntities())
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
    }
}
