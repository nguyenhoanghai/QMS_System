using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class VideoTemplateModel
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }
    }

    public class VideoTemplate_DeModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int VideoId { get; set; }
        public int TemplateId { get; set; }
        public string FileName { get; set; }
        public string TemplateName { get; set; }
    }
}
