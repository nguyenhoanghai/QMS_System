using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class ReadTemplateModel 
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

        public List<ReadTemplateDetailModel> Details { get; set; }
        public ReadTemplateModel()
        {
            Details = new List<ReadTemplateDetailModel>();
        }
    }
    public class ReadTemplateDetailModel
    {
        public int Id { get; set; }
        public int ReadTemplateId { get; set; }
        public int Index { get; set; }
        public int SoundId { get; set; }
    }
}
