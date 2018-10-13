using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLCommand
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLCommand _Instance;  //volatile =>  tranh dung thread
        public static BLLCommand Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCommand();

                return _Instance;
            }
        }
        private BLLCommand() { }
        #endregion
        public List<CommandModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Command.Where(x => !x.IsDeleted).Select(x => new CommandModel() { Id = x.Id, Code = x.Code, CodeHEX = x.CodeHEX, Note = x.Note }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_Command.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code + " ( " + x.CodeHEX + " )", Code = x.Note }).ToList();
            }
        }

        public int Insert(Q_Command obj)
        {
            using (db = new QMSSystemEntities())
            {
                db.Q_Command.Add(obj);
                db.SaveChanges();
                return obj.Id;
            }
        }

        public bool Update(Q_Command model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Command.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    obj.Code = model.Code;
                    obj.CodeHEX = model.CodeHEX;
                    obj.Note = model.Note;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public bool Delete(int Id)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_Command.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
