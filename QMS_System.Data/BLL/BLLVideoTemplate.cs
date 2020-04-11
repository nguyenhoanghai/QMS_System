using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLVideoTemplate
    {
        #region constructor 
        static object key = new object();
        private static volatile BLLVideoTemplate _Instance;
        public static BLLVideoTemplate Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLVideoTemplate();

                return _Instance;
            }
        }
        private BLLVideoTemplate() { }
        #endregion

        public List<VideoTemplateModel> Gets(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                return db.Q_VideoTemplate.Where(x => !x.IsDeleted).Select(x => new VideoTemplateModel()
                {
                    Id = x.Id,
                    TemplateName = x.TemplateName,
                    IsActive = x.IsActive,
                    Note = x.Note
                }).ToList();
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                return db.Q_VideoTemplate.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.TemplateName }).ToList();
            }
        }

        public ResponseBase InsertOrUpdate(string connectString, Q_VideoTemplate model)
        {
            var rs = new ResponseBase();
            using (var db = new QMSSystemEntities(connectString))
            {
                try
                {
                    if (CheckName(model, db) == null)
                    {
                        if (model.Id == 0)
                        {
                            db.Q_VideoTemplate.Add(model);
                        }
                        else
                        {
                            var obj = db.Q_VideoTemplate.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj != null)
                            {
                                obj.TemplateName = model.TemplateName;
                                obj.Note = model.Note;
                                obj.IsActive = model.IsActive;
                            }
                        }
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi nhập liệu", Message = "Tên mẫu video này đã tồn tại. Vui lòng nhập tên khác." });
                    }
                }
                catch (Exception)
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Lỗi nhập liệu", Message = "Lỗi CSDL." });
                }
                return rs;
            }
        }

        private Q_VideoTemplate CheckName(Q_VideoTemplate model, QMSSystemEntities db)
        {
            return db.Q_VideoTemplate.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.TemplateName.Equals(model.TemplateName));
        }

        public bool Delete(string connectString, int Id)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_VideoTemplate.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<VideoPlaylist> GetPlaylist(string connectString)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var objs = db.Q_VideoTemplate_De.Where(x => !x.IsDeleted &&
                        !x.Q_VideoTemplate.IsDeleted &&
                        !x.Q_Video.IsDeleted &&
                        x.Q_VideoTemplate.IsActive)
                .Select(x => new VideoPlaylist()
                {
                    Index = x.Index,
                    Path = x.Q_Video.FakeName,
                    Duration = x.Q_Video.Duration
                }).OrderBy(x => x.Index).ToList();
                for (int i = 0; i < objs.Count; i++)
                    objs[i]._Duration = objs[i].Duration.TotalMilliseconds;

                return objs;
            }
        }

    }

    public class BLLVideoTemplate_De
    {
        #region constructor 
        static object key = new object();
        private static volatile BLLVideoTemplate_De _Instance;
        public static BLLVideoTemplate_De Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLVideoTemplate_De();

                return _Instance;
            }
        }
        private BLLVideoTemplate_De() { }
        #endregion

        public List<VideoTemplate_DeModel> Gets(string connectString, int templateId)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                return db.Q_VideoTemplate_De.Where(x => !x.IsDeleted && !x.Q_VideoTemplate.IsDeleted && !x.Q_Video.IsDeleted && x.TemplateId == templateId).Select(x => new VideoTemplate_DeModel()
                {
                    Id = x.Id,
                    TemplateId = x.TemplateId,
                    Index = x.Index,
                    VideoId = x.VideoId
                }).ToList();
            }
        }

        public ResponseBase InsertOrUpdate(string connectString, Q_VideoTemplate_De model)
        {
            var rs = new ResponseBase();
            using (var db = new QMSSystemEntities(connectString))
            {
                try
                {
                    if (model.Id == 0)
                    {
                        db.Q_VideoTemplate_De.Add(model);
                    }
                    else
                    {
                        var obj = db.Q_VideoTemplate_De.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (obj != null)
                        {
                            obj.TemplateId = model.TemplateId;
                            obj.VideoId = model.VideoId;
                            obj.Index = model.Index;
                        }
                    }
                    db.SaveChanges();
                    rs.IsSuccess = true;
                }
                catch (Exception)
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Lỗi nhập liệu", Message = "Lỗi CSDL." });
                }
                return rs;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (var db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_VideoTemplate_De.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
