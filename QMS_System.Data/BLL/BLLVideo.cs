using QMS_System.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLVideo
    {
        #region constructor 
        static object key = new object();
        private static volatile BLLVideo _Instance;
        public static BLLVideo Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLVideo();

                return _Instance;
            }
        }
        private BLLVideo() { }
        #endregion

        public List<VideoModel> Gets(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                return db.Q_Video.Where(x => !x.IsDeleted).Select(x => new VideoModel()
                {
                    Id = x.Id,
                    FileName = x.FileName,
                }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                return db.Q_Video.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.FileName }).ToList();
            }
        }

        public int AddFile(string connectString, Q_Video model)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                db.Q_Video.Add(model);
                db.SaveChanges();
                return model.Id;
            }
        }

        public bool Delete(string connectString,int Id)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                try
                {
                    var file = db.Q_Video.Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == Id);
                    file.IsDeleted = true;
                    db.Entry<Q_Video>(file).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (System.Exception e)
                {
                }
                return false;
            }
        }

    }
}
