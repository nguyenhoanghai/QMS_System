using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLEquipment
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLEquipment _Instance;  //volatile =>  tranh dung thread
        public static BLLEquipment Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLEquipment();

                return _Instance;
            }
        }
        private BLLEquipment() { }
        #endregion
        public List<ModelSelectItem> GetMaindisplay()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Equipment.Where(x => !x.IsDeleted && !x.Q_EquipmentType.IsDeleted && x.EquipTypeId == (int)eEquipType.Maindisplay).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }
        public List<EquipmentModel> Gets(int equipTypeId)
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Equipment.Where(x => !x.IsDeleted && !x.Q_EquipmentType.IsDeleted && x.EquipTypeId == equipTypeId).Select(x => new EquipmentModel()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Position = x.Position,
                    EquipTypeId = x.EquipTypeId,
                    CounterId = x.CounterId,
                    StatusId = x.StatusId,
                    EndTime = x.EndTime,
                    Note = x.Note
                }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Equipment.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        public int GetCounterId(int equipCode)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                return (obj != null ? obj.CounterId : 0);
            }
        }
        public int GetEquipTypeId(int equipId)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipId);
                return (obj != null ? obj.EquipTypeId : 0);
            }
        }

        public int Insert(Q_Equipment obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_Equipment.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(Q_Equipment model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.Code = model.Code;
                        obj.Name = model.Name;
                        obj.Position = model.Position;
                        obj.EquipTypeId = model.EquipTypeId;
                        obj.CounterId = model.CounterId;
                        obj.StatusId = model.StatusId;
                        obj.EndTime = model.EndTime;
                        obj.Note = model.Note;
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
        }

        public bool Delete(int Id)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_Equipment model)
        {
            //  using (db = new QMSSystemEntities()){
            Q_Equipment obj = null;
            if (!string.IsNullOrEmpty(model.Name))
                obj = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Name.Trim().ToUpper().Equals(model.Name.Trim().ToUpper()));
            return obj != null ? true : false;
        }

        public List<string> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                List<string> codeArr = new List<string>();
                var codes = db.Q_Equipment.Where(x => !x.IsDeleted && (x.EquipTypeId == (int)eEquipType.Counter || x.EquipTypeId == (int)eEquipType.Printer)&&x.StatusId ==(int)eStatus.HOTAT).Select(x => x.Code).ToList();
                if (codes.Count > 0)
                    for (int i = 0; i < codes.Count; i++)
                        codeArr.Add(codes[i] < 10 ? ("0" + codes[i]) : codes[i].ToString());
                return codeArr;
            }
        }
    }
}
