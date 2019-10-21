using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLMaindisplayDirection
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLMaindisplayDirection _Instance;  //volatile =>  tranh dung thread
        public static BLLMaindisplayDirection Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLMaindisplayDirection();

                return _Instance;
            }
        }
        private BLLMaindisplayDirection() { }
        #endregion
        public List<MaindisplayDirectionModel> Gets(string connectString,int counterId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var list = db.Q_MaindisplayDirection.Where(x => !x.IsDeleted && !x.Q_Equipment.IsDeleted && !x.Q_Counter.IsDeleted && x.CounterId == counterId).Select(x => new MaindisplayDirectionModel()
                {
                    Id = x.Id,
                    CounterId = x.CounterId,
                    EquipmentId = x.EquipmentId,
                    EquipmentCode = x.Q_Equipment.Code,
                    Direction = x.Direction,
                    Note = x.Note
                }).ToList();
                if (list.Count > 0)
                {
                    int i = 1;
                    foreach (var item in list)
                    {
                        item.Index = i++;
                    }
                }
                return list;
            }
        }


        public int Insert(string connectString,Q_MaindisplayDirection obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_MaindisplayDirection.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public bool Update(string connectString,Q_MaindisplayDirection model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(model))
                {
                    var obj = db.Q_MaindisplayDirection.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                    if (obj != null)
                    {
                        obj.CounterId = model.CounterId;
                        obj.EquipmentId = model.EquipmentId;
                        obj.Direction = model.Direction;
                        obj.Note = model.Note;
                        db.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool Delete(string connectString,int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_MaindisplayDirection.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_MaindisplayDirection model)
        {
            var obj = db.Q_MaindisplayDirection.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.CounterId == model.CounterId && x.EquipmentId == model.EquipmentId);
            return obj != null ? true : false;
        }
    }
}
