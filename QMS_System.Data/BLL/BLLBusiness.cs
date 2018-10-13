using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLBusiness
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLBusiness _Instance;  //volatile =>  tranh dung thread
        public static BLLBusiness Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLBusiness();

                return _Instance;
            }
        }
        private BLLBusiness() { }
        #endregion
        public List<BusinessModel> Gets(int businessTypeId)
        {
              using (db = new QMSSystemEntities()){
            return db.Q_Business.Where(x => !x.IsDeleted && !x.Q_BusinessType.IsDeleted && x.BusinessTypeId == businessTypeId).Select(x => new BusinessModel()
            {
                Id = x.Id,
                Name = x.Name,
                BusinessTypeId = x.BusinessTypeId,
                Address = x.Address,
                TotalTicket = x.TotalTicket,
                Note = x.Note
            }).ToList();
        }}

        public int GetTicketAllow(int businessId)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_Business.Where(x => !x.IsDeleted && !x.Q_BusinessType.IsDeleted && x.Id == businessId).FirstOrDefault();
            return (obj != null ? obj.TotalTicket : 0);
        }}

        public List<ModelSelectItem> GetLookUp()
        {
              using (db = new QMSSystemEntities()){
            return db.Q_Business.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
        }}

        public int Insert(Q_Business obj)
        {
              using (db = new QMSSystemEntities()){
            if (!CheckExists(obj))
            {
                db.Q_Business.Add(obj);
                db.SaveChanges();
            }
            return obj.Id;
        }}

        public bool Update(Q_Business model)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_Business.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                if (!CheckExists(model))
                {
                    obj.Name = model.Name;
                    obj.BusinessTypeId = model.BusinessTypeId;
                    obj.Address = model.Address;
                    obj.TotalTicket = model.TotalTicket;
                    obj.Note = model.Note;
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            return false;
        }}

        public bool Delete(int Id)
        {
              using (db = new QMSSystemEntities()){
            var obj = db.Q_Business.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
              }
        }
        private bool CheckExists(Q_Business model)
        { 
            Q_Business obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Business.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }

    }
}
