using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.BLL
{
    public class BLLWorkType
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLWorkType _Instance;  //volatile =>  tranh dung thread
        public static BLLWorkType Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLWorkType();

                return _Instance;
            }
        }
        private BLLWorkType() { }
        #endregion

        public List<WorkTypeModel> GetLookUp(string connectString )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                  return db.Q_WorkType.Where(x => !x.IsDeleted  ).OrderBy(x => x.Code).Select(x => new WorkTypeModel() { Id = x.Id, Name = x.Name ,Code = x.Code}).ToList();
            }
        }

        public List<WorkTypeModel> Gets(string connectString )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                  return db.Q_WorkType.Where(x => !x.IsDeleted  ).OrderBy(x => x.Code).Select(x => new WorkTypeModel() { Id = x.Id, Name = x.Name,   Code = x.Code }).ToList();
            }
        }

        public Q_WorkType Get(string connectString, int WorkTypeId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_WorkType.FirstOrDefault (x => !x.IsDeleted && x.Id == WorkTypeId) ;
            }
        }
         
        public ResponseBaseModel Insert(string connectString, Q_WorkType model)
        {
            var rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(model,true))
                {
                    if (!CheckExists(model, false))
                    {
                        if(model.Id == 0) 
                        db.Q_WorkType.Add(model);
                        else
                        {
                            var obj = db.Q_WorkType.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if(obj != null)
                            {
                                obj.Code = model.Code;
                                obj.Name = model.Name;
                            }
                            else
                            {
                                rs.IsSuccess = false;
                                rs.sms = "Loại công việc đã bị xóa hoặc không tồn tại trong hệ thống. Vui lòng kiểm tra lại!.";
                            }
                        }
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.sms = "Tên loại công việc đã tồn tại. Vui lòng nhập tên khác!.";
                    } 
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Mã loại công việc đã tồn tại. Vui lòng nhập mã khác!.";
                } 
            }
            return rs;
        }
         
        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_WorkType.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(Q_WorkType model, bool checkCode)
        {
            Q_WorkType obj = null;
            if (checkCode)
                obj = db.Q_WorkType.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Code.Trim().ToUpper().Equals(model.Code.Trim().ToUpper()));
            else
                obj = db.Q_WorkType.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
