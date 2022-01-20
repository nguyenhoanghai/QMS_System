using QMS_System.Data.Model;
using System;
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
                var objs = db.Q_PrintTicket.OrderBy(x => x.PrintIndex).Select(x => new PrintTicketModel() { Id = x.Id, Name = x.Name, PrintTemplate = x.PrintTemplate, IsActive = x.IsActive, PrintIndex = x.PrintIndex, PrintPages = x.PrintPages }).ToList();
                if (objs != null && objs.Count() > 0)
                    foreach (var item in objs)
                    {
                        item.ServiceIds = string.Join(",", db.Q_ServicePrintTemplate.Where(x => x.PrintTemplateId == item.Id).Select(x => x.ServiceId).ToArray());
                        item._ServiceIds = db.Q_ServicePrintTemplate.Where(x => x.PrintTemplateId == item.Id).Select(x => x.ServiceId).ToList();
                        item.ServiceNames = string.Join(",", db.Q_ServicePrintTemplate.Where(x => x.PrintTemplateId == item.Id).Select(x => x.Q_Service.Name).ToArray());
                    }

                return objs;
            }
        }

        public Q_PrintTicket Get(string connectString, int objId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_PrintTicket.FirstOrDefault(x => x.Id == objId);
            }
        }

        public ResponseBaseModel InsertOrUpdate(string connectString, Q_PrintTicket model, string serviceIds)
        {
            var rs = new ResponseBaseModel();
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(model))
                {
                    int[] serIds = serviceIds.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                    Q_ServicePrintTemplate servicePrintTemplate;
                    if (model.Id == 0)
                    {
                        model.Q_ServicePrintTemplate = new List<Q_ServicePrintTemplate>();
                        for (int i = 0; i < serIds.Length; i++)
                        {
                            servicePrintTemplate = new Q_ServicePrintTemplate() { ServiceId = serIds[i], Q_PrintTicket = model };
                            model.Q_ServicePrintTemplate.Add(servicePrintTemplate);
                        }
                        db.Q_PrintTicket.Add(model);
                    }
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

                            var olds = db.Q_ServicePrintTemplate.Where(x => x.PrintTemplateId == obj.Id).ToList();
                            if (olds.Count > 0)
                            {
                                for (int i = 0; i < olds.Count; i++)
                                {
                                    db.Q_ServicePrintTemplate.Remove(olds[i]);
                                }
                            }

                            model.Q_ServicePrintTemplate = new List<Q_ServicePrintTemplate>();
                            for (int i = 0; i < serIds.Length; i++)
                            {
                                servicePrintTemplate = new Q_ServicePrintTemplate() { ServiceId = serIds[i], PrintTemplateId = obj.Id };
                                db.Q_ServicePrintTemplate.Add(servicePrintTemplate);
                            }
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
