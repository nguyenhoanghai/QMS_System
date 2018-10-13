using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMS_System.Data.Model
{
    public class RegisterUserCmdModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Index { get; set; }
        public string CMDName { get; set; }
        public string CMDParamName { get; set; }
        public string Note { get; set; }
        public string ActionParamName { get; set; }
        public string ActionName { get; set; }
        public string Param { get; set; }
    }
}
