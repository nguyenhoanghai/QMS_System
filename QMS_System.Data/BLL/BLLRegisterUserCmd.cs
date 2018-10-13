using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.BLL
{
    public class BLLRegisterUserCmd
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLRegisterUserCmd _Instance;  //volatile =>  tranh dung thread
        public static BLLRegisterUserCmd Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLRegisterUserCmd();

                return _Instance;
            }
        }
        private BLLRegisterUserCmd() { }
        #endregion
        public List<RegisterUserCmdModel> Gets(int userId, int cmdId, int cmdParamId)
        {
            using (db = new QMSSystemEntities())
            {
                var objs = db.Q_UserCmdRegister.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_CommandParameter.Q_Command.IsDeleted && !x.Q_CommandParameter.IsDeleted && !x.Q_ActionParameter.Q_Action.IsDeleted && !x.Q_ActionParameter.IsDeleted && x.UserId == userId);
                if (objs != null)
                {
                    if (cmdId > 0)
                       objs = objs.Where(x => x.Q_CommandParameter.CommandId == cmdId);
                    if (cmdParamId > 0)
                        objs = objs.Where(x => x.CmdParamId == cmdParamId);
                    return objs.Select(x => new RegisterUserCmdModel()
                                    {
                                        Id = x.Id,
                                        UserId = x.UserId,
                                        Index = x.Index,
                                        CMDName = x.Q_CommandParameter.Q_Command.CodeHEX,
                                        CMDParamName = x.Q_CommandParameter.Parameter,
                                        Note = x.Note,
                                        ActionName = x.Q_ActionParameter.Q_Action.Code,
                                        ActionParamName = x.Q_ActionParameter.ParameterCode,
                                        Param = x.Param
                                    }).OrderBy(x => x.CMDName).ThenBy(x => x.Index).ToList();
                }
                return new List<RegisterUserCmdModel>();
            }
        }

        public List<RegisterUserCmdModel> Gets()
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserCmdRegister.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_CommandParameter.Q_Command.IsDeleted && !x.Q_CommandParameter.IsDeleted && !x.Q_ActionParameter.Q_Action.IsDeleted && !x.Q_ActionParameter.IsDeleted).Select(x => new RegisterUserCmdModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Index = x.Index,
                    CMDName = x.Q_CommandParameter.Q_Command.CodeHEX,
                    CMDParamName = x.Q_CommandParameter.Parameter,
                    Note = x.Note,
                    ActionName = x.Q_ActionParameter.Q_Action.Code,
                    ActionParamName = x.Q_ActionParameter.ParameterCode,
                    Param = x.Param
                }).ToList();
            }
        }

        public List<RegisterUserCmdModel> Gets(int userId)
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserCmdRegister.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_CommandParameter.Q_Command.IsDeleted && !x.Q_CommandParameter.IsDeleted && !x.Q_ActionParameter.Q_Action.IsDeleted && !x.Q_ActionParameter.IsDeleted && x.UserId == userId).Select(x => new RegisterUserCmdModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Index = x.Index,
                    CMDName = x.Q_CommandParameter.Q_Command.CodeHEX,
                    CMDParamName = x.Q_CommandParameter.Parameter,
                    Note = x.Note,
                    ActionName = x.Q_ActionParameter.Q_Action.Code,
                    ActionParamName = x.Q_ActionParameter.ParameterCode,
                    Param = x.Param
                }).ToList();
            }
        }

        public List<RegisterUserCmdModel> GetUserRight(int userId, string cmd, string cmdParam)
        {
            using (db = new QMSSystemEntities())
            {
                return db.Q_UserCmdRegister.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_CommandParameter.Q_Command.IsDeleted && !x.Q_CommandParameter.IsDeleted && !x.Q_ActionParameter.Q_Action.IsDeleted && !x.Q_ActionParameter.IsDeleted && x.UserId == userId && x.Q_CommandParameter.Q_Command.CodeHEX == cmd && x.Q_CommandParameter.Parameter == cmdParam).OrderBy(x => x.Index).Select(x => new RegisterUserCmdModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Index = x.Index,
                    CMDName = x.Q_CommandParameter.Q_Command.CodeHEX,
                    CMDParamName = x.Q_CommandParameter.Parameter,
                    Note = x.Note,
                    ActionName = x.Q_ActionParameter.Q_Action.Code,
                    ActionParamName = x.Q_ActionParameter.ParameterCode,
                    Param = x.Param
                }).ToList();
            }
        }


        //public List<ModelSelectItem> GetLookUp()
        //{
        //      using (db = new QMSSystemEntities()){
        //    return db.Q_UserCmdRegister.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
        //}

        public int Insert(Q_UserCmdRegister obj)
        {
            using (db = new QMSSystemEntities())
            {
                if (!CheckExists(obj))
                {
                    db.Q_UserCmdRegister.Add(obj);
                    db.SaveChanges();
                }
                return obj.Id;
            }
        }

        public int Update(Q_UserCmdRegister model)
        {
            using (db = new QMSSystemEntities())
            {
                var obj = db.Q_UserCmdRegister.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                if (obj != null)
                {
                    if (!CheckExists(model))
                    {
                        obj.ActionParamId = model.ActionParamId;
                        obj.Index = model.Index;
                        obj.Param = model.Param;
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
                var obj = db.Q_UserCmdRegister.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(Q_UserCmdRegister model)
        {
            var obj = db.Q_UserCmdRegister.FirstOrDefault(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_CommandParameter.Q_Command.IsDeleted && !x.Q_CommandParameter.IsDeleted && !x.Q_ActionParameter.Q_Action.IsDeleted && !x.Q_ActionParameter.IsDeleted && x.UserId == model.UserId && x.CmdParamId == model.CmdParamId && x.ActionParamId == model.ActionParamId && x.Id != model.Id);
            return obj != null ? true : false;
        }
        public bool Copy(List<int> Ids, int userId)
        {
            try
            {
                using (db = new QMSSystemEntities())
                {
                    var src = db.Q_UserCmdRegister.Where(x => !x.IsDeleted && Ids.Contains(x.Id)).ToList();
                    if (src.Count > 0)
                    {
                        Q_UserCmdRegister obj;
                        for (int i = 0; i < src.Count; i++)
                        {
                            obj = new Q_UserCmdRegister();
                            obj.UserId = userId;
                            obj.CmdParamId = src[i].CmdParamId;
                            obj.ActionParamId = src[i].ActionParamId;
                            obj.Param = src[i].Param;
                            obj.Index = src[i].Index;
                            obj.Note = src[i].Note;
                            db.Q_UserCmdRegister.Add(obj);
                        }
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception)
            { }
            return false;
        }
    }
}
