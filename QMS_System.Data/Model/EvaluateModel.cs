using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMS_System.Data.BLL
{
   public class EvaluateModel  
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public List<EvaluateDetailModel> Childs { get; set; }
        public EvaluateModel()
        {
            Childs = new List<EvaluateDetailModel>();
        }
    }

   public class EvaluateDetailModel  
   {
       public int Id { get; set; }
       public int EvaluateId { get; set; }
       public int Index { get; set; }
       public string Name { get; set; }
       public string Note { get; set; }
       public bool IsDefault { get; set; }
       public bool IsDeleted { get; set; }
       public string Icon { get; set; }
       public string Image { get; set; }
        public string SmsContent { get; set; }
       public bool IsSendSMS { get; set; }
   }
}
