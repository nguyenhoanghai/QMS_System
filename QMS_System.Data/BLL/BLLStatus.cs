﻿using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.BLL
{
  public  class BLLStatus
    {
      #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLStatus _Instance;  //volatile =>  tranh dung thread
        public static BLLStatus Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLStatus();

                return _Instance;
            }
        }
        private BLLStatus() { }
        #endregion
        public List<StatusModel> Gets(string connectString,int typeId)
        {
              using (db = new QMSSystemEntities(connectString)){
            if(typeId != 0)
            return db.Q_Status.Where(x =>  typeId == x.StatusTypeId).Select(x => new StatusModel() { Id = x.Id, Code  = x.Code, Note = x.Note, StatusTypeId = x.StatusTypeId  }).ToList();
            return new List<StatusModel>();
        }}

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
              using (db = new QMSSystemEntities(connectString)){
            return db.Q_Status.Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
        }}
       
        public int Insert(string connectString,Q_Status obj)
        {
              using (db = new QMSSystemEntities(connectString)){
            db.Q_Status.Add(obj);
            db.SaveChanges();
            return obj.Id;
        }}

        public bool Update(string connectString,Q_Status model)
        {
              using (db = new QMSSystemEntities(connectString)){
            var obj = db.Q_Status.FirstOrDefault(x =>  x.Id == model.Id);
            if (obj != null)
            {
                obj.Code = model.Code;
                obj.StatusTypeId = model.StatusTypeId; 
                obj.Note = model.Note; 
                db.SaveChanges();
                return true;
            }
            return false;
              }
        }

        //public bool Delete(string connectString,int Id)
        //{
        //      using (db = new QMSSystemEntities(connectString)){
        //    var obj = db.Q_Status.FirstOrDefault(x =>  x.Id == Id);
        //    if (obj != null)
        //    {
        //        obj.IsDeleted = true;
        //        db.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
    }
}
