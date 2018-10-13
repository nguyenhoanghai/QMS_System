using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMS_System.Data.Model
{
   public class UserCmdReadSoundModel
    {
        public int Id    { get; set; }
        public int Index { get; set; }
        public int UserId { get; set; }
        public int CommandId { get; set; }
        public int ReadTemplateId { get; set; }
        public string Note { get; set; } 
    }
}
