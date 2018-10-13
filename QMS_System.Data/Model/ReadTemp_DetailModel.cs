using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class ReadTemp_DetailModel
    {
        public int Id { get; set; }
        public int ReadTemplateId { get; set; }
        public int Index { get; set; } 
        public int SoundId { get; set; } 
    }
}
