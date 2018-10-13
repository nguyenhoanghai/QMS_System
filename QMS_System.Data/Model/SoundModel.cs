using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class SoundModel 
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
}
