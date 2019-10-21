using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLCounterSound
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLCounterSound _Instance;  //volatile =>  tranh dung thread
        public static BLLCounterSound Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCounterSound();

                return _Instance;
            }
        }
        private BLLCounterSound() { }
        #endregion

        public List<CounterSoundModel> Gets(string connectString )
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_CounterSound.Where(x => !x.IsDeleted && !x.Q_Counter.IsDeleted && !x.Q_Language.IsDeleted  ).Select(x => new CounterSoundModel()
                {
                    Id = x.Id,
                    SoundName = x.SoundName,
                    Note = x.Note,
                    CounterId = x.CounterId,
                    LanguageId = x.LanguageId
                }).ToList();
            }
        }
        public List<CounterSoundModel> Gets(string connectString,int counterId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                return db.Q_CounterSound.Where(x => !x.IsDeleted && !x.Q_Counter.IsDeleted && !x.Q_Language.IsDeleted && x.CounterId == counterId).Select(x => new CounterSoundModel()
                {
                    Id = x.Id,
                    SoundName = x.SoundName,
                    Note = x.Note,
                    CounterId = x.CounterId,
                    LanguageId = x.LanguageId
                }).ToList();
            }
        }

        public string GetSoundName(string connectString, int counterId, int languageId)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_CounterSound.FirstOrDefault(x => !x.IsDeleted && !x.Q_Counter.IsDeleted && !x.Q_Language.IsDeleted && x.CounterId == counterId && x.LanguageId == languageId);
                return (obj != null ? obj.SoundName : "");
            }
        }

        public int Insert(string connectString,Q_CounterSound obj)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                if (!CheckExists(obj))
                {
                    db.Q_CounterSound.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public int Update(string connectString,Q_CounterSound model)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_CounterSound.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.SoundName = model.SoundName;
                        obj.LanguageId = model.LanguageId;
                        obj.Note = model.Note;
                        obj.CounterId = model.CounterId;
                        db.SaveChanges();
                        return model.Id;
                    }
                    else
                        return 0;
                }
                return 0;
            }
        }

        public bool Delete(string connectString,int Id)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var obj = db.Q_CounterSound.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(Q_CounterSound model)
        {
            var obj = db.Q_CounterSound.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.CounterId == model.CounterId && x.LanguageId == model.LanguageId && x.SoundName.Trim().ToUpper().Equals(model.SoundName.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
