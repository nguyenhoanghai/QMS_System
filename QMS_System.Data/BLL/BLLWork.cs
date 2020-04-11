using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace QMS_System.Data.BLL
{
    public class BLLWork
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLWork _Instance;  //volatile =>  tranh dung thread
        public static BLLWork Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLWork();

                return _Instance;
            }
        }
        private BLLWork() { }
        #endregion

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Works.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public List<WorkModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Works.Where(x => !x.IsDeleted).OrderBy(x => x.Code).Select(x => new WorkModel() { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
            }
        }

        public Q_Works Get(string connectString, int WorkId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_Works.FirstOrDefault(x => !x.IsDeleted && x.Id == WorkId);
            }
        }

        public List<ModelSelectItem> GetLookUp(SqlConnection sqlConnection, bool countWaiting)
        {
            var model = new List<ModelSelectItem>();
            string query = "select Id, Name,TimeProcess from Q_Works where IsDeleted = 0 ";
            var da = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ModelSelectItem modelSelectItem;
                foreach (DataRow row in dataTable.Rows)
                {
                    modelSelectItem = new ModelSelectItem()
                    {
                        Id = Convert.ToInt32(row["Id"].ToString()),
                        Name = row["Name"].ToString(),
                        Data = 0
                    };
                    if (!string.IsNullOrEmpty(row["TimeProcess"].ToString()))
                    {
                        var date = DateTime.Parse(row["TimeProcess"].ToString());
                        modelSelectItem.Code = date.TimeOfDay.ToString();
                    }

                    query = "select COUNT(d.Id) as number from Q_DailyRequire_Detail d , Q_DailyRequire r where r.Id = d.DailyRequireId and UserId is NULL and r.WorkId=" + modelSelectItem.Id;
                    da = new SqlDataAdapter(query, sqlConnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        modelSelectItem.Data = Convert.ToInt32(dt.Rows[0]["number"].ToString());
                    }
                    model.Add(modelSelectItem);
                }
            }
            return model;
        }

        public ResponseBaseModel Insert(string connectString, Q_Works model)
        {
            var rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(model, true))
                {
                    if (!CheckExists(model, false))
                    {
                        if (model.Id == 0)
                            db.Q_Works.Add(model);
                        else
                        {
                            var obj = db.Q_Works.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj != null)
                            {
                                obj.Code = model.Code;
                                obj.Name = model.Name;
                            }
                            else
                            {
                                rs.IsSuccess = false;
                                rs.sms = "Công việc đã bị xóa hoặc không tồn tại trong hệ thống. Vui lòng kiểm tra lại!.";
                            }
                        }
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.sms = "Tên công việc đã tồn tại. Vui lòng nhập tên khác!.";
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Mã công việc đã tồn tại. Vui lòng nhập mã khác!.";
                }
            }
            return rs;
        }

        public bool Delete(string connectString, int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_Works.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(Q_Works model, bool checkCode)
        {
            Q_Works obj = null;
            if (checkCode)
                obj = db.Q_Works.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Code.Trim().ToUpper().Equals(model.Code.Trim().ToUpper()));
            else
                obj = db.Q_Works.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }

        public void taoCongViec(string path, string connectionString)
        {
            ExcelApp.Application excelApp = new ExcelApp.Application();
            ExcelApp.Workbook excelBook = excelApp.Workbooks.Open(path);
            ExcelApp._Worksheet excelSheet = excelBook.Sheets[1];
            ExcelApp.Range excelRange = excelSheet.UsedRange;

            using (var db = new QMSSystemEntities(connectionString))
            {
                Q_Works work;
                Q_WorkDetail workDetail_So;
                Q_WorkDetail workDetail_Ga;
                for (int ii = 5; ii < 36; ii++)
                {
                    string code = excelRange.Cells[ii, 1].Value2.ToString();
                    string name = excelRange.Cells[ii, 2].Value2.ToString();
                    string phutSo = excelRange.Cells[ii, 3].Value2.ToString();
                    string phutGa = excelRange.Cells[ii, 4].Value2.ToString();

                    work = new Q_Works() { Code = code, Name = name };
                    work.Q_WorkDetail = new List<Q_WorkDetail>();
                    workDetail_So = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 1, TimeProcess = layGio(Convert.ToInt32(phutSo)) }; //so
                    work.Q_WorkDetail.Add(workDetail_So);
                    workDetail_Ga = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 2, TimeProcess = layGio(Convert.ToInt32(phutGa)) }; //ga
                    work.Q_WorkDetail.Add(workDetail_Ga);
                    db.Q_Works.Add(work);
                }

                for (int ii = 38; ii < 68; ii++)
                {
                    string code = excelRange.Cells[ii, 1].Value2.ToString();
                    string name = excelRange.Cells[ii, 2].Value2.ToString();
                    string phutSo = excelRange.Cells[ii, 3].Value2.ToString();
                    string phutGa = excelRange.Cells[ii, 4].Value2.ToString();

                    work = new Q_Works() { Code = code, Name = name };
                    work.Q_WorkDetail = new List<Q_WorkDetail>();
                    workDetail_So = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 1, TimeProcess = layGio(Convert.ToInt32(phutSo)) }; //so
                    work.Q_WorkDetail.Add(workDetail_So);
                    workDetail_Ga = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 2, TimeProcess = layGio(Convert.ToInt32(phutGa)) }; //ga
                    work.Q_WorkDetail.Add(workDetail_Ga);
                    db.Q_Works.Add(work);
                }

                for (int ii = 5; ii < 56; ii++)
                {
                    string code = excelRange.Cells[ii, 8].Value2.ToString();
                    string name = excelRange.Cells[ii, 9].Value2.ToString();
                    string phutSo = excelRange.Cells[ii, 10].Value2.ToString();
                    string phutGa = excelRange.Cells[ii, 11].Value2.ToString();

                    work = new Q_Works() { Code = code, Name = name };
                    work.Q_WorkDetail = new List<Q_WorkDetail>();
                    workDetail_So = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 1, TimeProcess = layGio(Convert.ToInt32(phutSo)) }; //so
                    work.Q_WorkDetail.Add(workDetail_So);
                    workDetail_Ga = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 2, TimeProcess = layGio(Convert.ToInt32(phutGa)) }; //ga
                    work.Q_WorkDetail.Add(workDetail_Ga);
                    db.Q_Works.Add(work);
                }

                for (int ii = 57; ii < 69; ii++)
                {
                    string code = excelRange.Cells[ii, 8].Value2.ToString();
                    string name = excelRange.Cells[ii, 9].Value2.ToString();
                    string phutSo = excelRange.Cells[ii, 10].Value2.ToString();
                    string phutGa = excelRange.Cells[ii, 11].Value2.ToString();

                    work = new Q_Works() { Code = code, Name = name };
                    work.Q_WorkDetail = new List<Q_WorkDetail>();
                    workDetail_So = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 1, TimeProcess = layGio(Convert.ToInt32(phutSo)) }; //so
                    work.Q_WorkDetail.Add(workDetail_So);
                    workDetail_Ga = new Q_WorkDetail() { Q_Works = work, WorkTypeId = 2, TimeProcess = layGio(Convert.ToInt32(phutGa)) }; //ga
                    work.Q_WorkDetail.Add(workDetail_Ga);
                    db.Q_Works.Add(work);
                }
                db.SaveChanges();
            }
        }

        private DateTime layGio(int soPhut)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddMinutes(soPhut);
        }
    }
}
