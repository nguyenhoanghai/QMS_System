using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLUserCmdReadSound
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLUserCmdReadSound _Instance;  //volatile =>  tranh dung thread
        public static BLLUserCmdReadSound Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUserCmdReadSound();

                return _Instance;
            }
        }
        private BLLUserCmdReadSound() { }
        #endregion

        public List<UserCmdReadSoundModel> Gets( )
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserCommandReadSound.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_Command.IsDeleted && !x.Q_ReadTemplate.IsDeleted ).Select(x => new UserCmdReadSoundModel() { Id = x.Id, UserId = x.UserId, Index = x.Index, CommandId = x.CommandId, Note = x.Q_Command.CodeHEX.Trim().ToUpper(), ReadTemplateId = x.ReadTemplateId }).ToList();
            }
        }
        public List<UserCmdReadSoundModel> Gets(int userId, int cmdId)
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserCommandReadSound.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_Command.IsDeleted && !x.Q_ReadTemplate.IsDeleted && x.UserId == userId && x.CommandId == cmdId).Select(x => new UserCmdReadSoundModel() { Id = x.Id, UserId = x.UserId, Index = x.Index, CommandId = x.CommandId, Note = x.Note, ReadTemplateId = x.ReadTemplateId }).ToList();
            }
        }
        public List<int> GetReadTemplateIds(int userId, string cmdCodeHex)
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserCommandReadSound.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_Command.IsDeleted && !x.Q_ReadTemplate.IsDeleted && x.UserId == userId && x.Q_Command.CodeHEX.Trim().ToUpper().Equals(cmdCodeHex.ToUpper())).Select(x => x.ReadTemplateId).ToList();
            }
        }

        public int Insert(Q_UserCommandReadSound obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_UserCommandReadSound.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public int Update(Q_UserCommandReadSound model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_UserCommandReadSound.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.UserId = model.UserId;
                        obj.CommandId = model.CommandId;
                        obj.Index = model.Index;
                        obj.ReadTemplateId = model.ReadTemplateId;
                        obj.Note = model.Note;
                        db.SaveChanges();
                        return obj.Id;
                    }
                    else
                        return 0;
                }
                return 0;
            }
        }

        public bool Delete(int Id)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_UserCommandReadSound.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(Q_UserCommandReadSound model)
        {
            var obj = db.Q_UserCommandReadSound.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.UserId == model.UserId && x.CommandId == model.CommandId && x.ReadTemplateId == model.ReadTemplateId);
            return obj != null ? true : false;
        }
    }
}
