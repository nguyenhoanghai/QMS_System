using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLReadTempDetail
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLReadTempDetail _Instance;  //volatile =>  tranh dung thread
        public static BLLReadTempDetail Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLReadTempDetail();

                return _Instance;
            }
        }
        private BLLReadTempDetail() { }
        #endregion
        /// <summary>
        /// Get list of template detail by templateId
        /// </summary>
        /// <param name="Id">TemplateId</param>
        /// <returns></returns>
        public List<ReadTemplateDetailModel> Gets(int Id)
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_ReadTemp_Detail.Where(x => !x.IsDeleted && !x.Q_ReadTemplate.IsDeleted && x.ReadTemplateId == Id).Select(x => new ReadTemplateDetailModel() { Id = x.Id, Index = x.Index, ReadTemplateId = x.ReadTemplateId, SoundId = x.SoundId }).OrderBy(x => x.Index).ToList();

            }
        }

        public List<ReadTemplateModel> Gets(List<int> Ids)
        {
            using (db = new QMSSystemEntities())
            {
                var objs = db.Q_ReadTemplate.Where(x => !x.IsDeleted && Ids.Contains(x.Id)).Select(x => new ReadTemplateModel() { Id = x.Id, Name = x.Name, LanguageId = x.LanguageId }).ToList();
                if (objs.Count > 0)
                {
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

        public int Insert(Q_ReadTemp_Detail obj)
        {
            using (db = new QMSSystemEntities())
            {
                db.Q_ReadTemp_Detail.Add(obj);
                db.SaveChanges();
                return obj.Id;
            }
        }

        public bool Update(Q_ReadTemp_Detail model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_ReadTemp_Detail.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    obj.Index = model.Index;
                    obj.SoundId = model.SoundId;
                    obj.ReadTemplateId = model.ReadTemplateId;
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
                var obj = db.Q_ReadTemp_Detail.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
