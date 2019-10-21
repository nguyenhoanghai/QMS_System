using QMS_System.Data.Model;
using QMS_System.Data;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLReadTemp_Detail
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLReadTemp_Detail _Instance;  //volatile =>  tranh dung thread
        public static BLLReadTemp_Detail Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLReadTemp_Detail();

                return _Instance;
            }
        }
        private BLLReadTemp_Detail() { }
        #endregion
        public List<ReadTemp_DetailModel> GetListByTemplateId(string connectString, int templateId)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_ReadTemp_Detail.Where(x => !x.IsDeleted && !x.Q_ReadTemplate.IsDeleted && x.ReadTemplateId == templateId).Select(x => new ReadTemp_DetailModel() { Id = x.Id, ReadTemplateId = x.ReadTemplateId, Index = x.Index, SoundId = x.SoundId }).ToList();
        }}

        //public List<ModelSelectItem> GetLookUp(string connectString)
        //{
        //      using (db = new QMSSystemEntities(connectString)){
        //    return db.Q_ReadTemp_Detail.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
        //}
       
        public int Insert(string connectString,Q_ReadTemp_Detail obj)
        {
              using (db = new QMSSystemEntities(connectString)){
            db.Q_ReadTemp_Detail.Add(obj);
            db.SaveChanges();
            return obj.Id;
        }}

        public bool Update(string connectString,Q_ReadTemp_Detail model)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_ReadTemp_Detail.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
            if (obj != null)
            {
                obj.ReadTemplateId = model.ReadTemplateId;
                obj.Index = model.Index;
                obj.SoundId = model.SoundId;
                db.SaveChanges();
                return true;
            }
            return false;
        }}

        public bool Delete(string connectString,int Id)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_ReadTemp_Detail.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
        }}
        public bool DeleteByTemplateId(string connectString, int templateId)
        {
              using (db = new QMSSystemEntities(connectString)){
            var list = db.Q_ReadTemp_Detail.Where(x => !x.IsDeleted && x.ReadTemplateId == templateId).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.IsDeleted = true;
                }
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }}
        public int GetLastIndex(string connectString, int templateId)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_ReadTemp_Detail.Where(x => !x.IsDeleted && x.ReadTemplateId == templateId).OrderByDescending(x => x.Index).FirstOrDefault();
            return obj != null ? obj.Index : 0;
              }
        }
    }
}
