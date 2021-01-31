using QMS_System.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLPrintTemplate
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLPrintTemplate _Instance;
        public static BLLPrintTemplate Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPrintTemplate();

                return _Instance;
            }
        }
        private BLLPrintTemplate() { }
        #endregion

        public List<PrintTicketModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_PrintTicket.OrderBy(x => x.PrintIndex).Select(x => new PrintTicketModel() { Id = x.Id, Name = x.Name, PrintTemplate = x.PrintTemplate, IsActive = x.IsActive, PrintIndex = x.PrintIndex, PrintPages = x.PrintPages }).ToList();
            }
        }

        public Q_PrintTicket Get(string connectString, int objId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_PrintTicket.FirstOrDefault(x => x.Id == objId);
            }
        }

        public ResponseBaseModel InsertOrUpdate(string connectString, Q_PrintTicket model)
        {
            var rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(model))
                {
                    if (model.Id == 0)
                        db.Q_PrintTicket.Add(model);
                    else
                    {
                        var obj = db.Q_PrintTicket.FirstOrDefault(x => x.Id == model.Id);
                        if (obj != null)
                        {
                            obj.PrintTemplate = model.PrintTemplate;
                            obj.Name = model.Name;
                            obj.PrintIndex = model.PrintIndex;
                            obj.PrintPages = model.PrintPages;
                            obj.IsActive = model.IsActive;
                        }
                        else
                        {
                            rs.IsSuccess = false;
                            rs.sms = "Mẫu in đã bị xóa hoặc không tồn tại trong hệ thống. Vui lòng kiểm tra lại!.";
                        }
                    }
                    db.SaveChanges();
                    rs.IsSuccess = true;
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.sms = "Tên Mẫu in đã tồn tại. Vui lòng nhập tên khác!.";
                }
            }
            return rs;
        }

        private bool CheckExists(Q_PrintTicket model)
        {
            Q_PrintTicket obj = null;
            obj = db.Q_PrintTicket.FirstOrDefault(x => x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
