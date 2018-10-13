using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class ResponseBaseModel
    {
        public bool IsSuccess { get; set; }
        public string sms { get; set; }
        public string Title { get; set; }
        public dynamic Data { get; set; }
    }

   public class ResponseBase
   {
       public bool IsSuccess { get; set; }
       public List<Error> Errors { get; set; }
       public dynamic Data { get; set; }
       public dynamic Records { get; set; }
       public ResponseBase()
       {
           Errors = new List<Error>();
       }
   }

   public class Error
   { 
       public string MemberName { get; set; }
       public string Message { get; set; }
   }
}
