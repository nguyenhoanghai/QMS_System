using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMS_System.Data.Model
{
   public class UserEvaluateModel
    {
        public int Id { get; set; }
        public string TENNV { get; set; }
        public int MAPHIEU { get; set; }
        public string DANHGIA { get; set; }
        public string YKIEN { get; set; }
        public DateTime  GLAYPHIEU { get; set; }
        public DateTime?  GDENQUAY { get; set; }
        public DateTime? GGIAODICH { get; set; }
        public DateTime?  GKETTHUC { get; set; }
    }
}
