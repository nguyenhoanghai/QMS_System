using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLReadTemplate
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLReadTemplate _Instance;  //volatile =>  tranh dung thread
        public static BLLReadTemplate Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLReadTemplate();

                return _Instance;
            }
        }
        private BLLReadTemplate() { }
        #endregion

        public List<ReadTemplateModel> GetsForMain(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var objs = db.Q_ReadTemplate.Where(x => !x.IsDeleted ).Select(x => new ReadTemplateModel() { Id = x.Id, Name = x.Name, LanguageId = x.LanguageId }).ToList();
                if (objs.Count > 0)
                {
                    int[] Ids = objs.Select(x => x.Id).ToArray();
                    var details = db.Q_ReadTemp_Detail.Where(x => !x.IsDeleted && !x.Q_ReadTemplate.IsDeleted && Ids.Contains(x.ReadTemplateId)).Select(x => new ReadTemplateDetailModel() { Id = x.Id, Index = x.Index, ReadTemplateId = x.ReadTemplateId, SoundId = x.SoundId }).OrderBy(x => x.Index).ToList();
                    if (details.Count > 0)
                    {
                        for (int i = 0; i < objs.Count; i++)
                        {
                            objs[i].Details.AddRange(details.Where(x => x.ReadTemplateId == objs[i].Id).OrderBy(x => x.Index));
                        }
                    }
                    return objs;
                }
                return new List<ReadTemplateModel>();
            }
        }

        public List<ReadTemplateModel> Gets(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_ReadTemplate.Where(x => !x.IsDeleted).Select(x => new ReadTemplateModel() { Id = x.Id, Name = x.Name, LanguageId = x.LanguageId, Note = x.Note }).ToList();
            }
        }

        public  Q_ReadTemplate   Get(string connectString,string name)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_ReadTemplate.FirstOrDefault(x => !x.IsDeleted && x.Name.Trim().ToUpper().Equals(name.Trim().ToUpper()));
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_ReadTemplate.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = (x.Name + " (" + x.Q_Language.Name + ")") }).ToList();
            }
        }

        public int Insert(string connectString,Q_ReadTemplate obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                db.Q_ReadTemplate.Add(obj);
                db.SaveChanges();
                return obj.Id;
            }
        }

        public bool Update(string connectString,Q_ReadTemplate model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_ReadTemplate.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    obj.Name = model.Name;
                    obj.LanguageId = model.LanguageId;
                    obj.Note = model.Note;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Delete(string connectString,int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_ReadTemplate.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
