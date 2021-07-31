using GPRO.Core.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace QMS_System.Data.Model
{
    public class ResponseBaseModel
    {
        [DefaultValue(false)]
        public bool IsSuccess { get; set; }
        public string sms { get; set; }
        public string Title { get; set; }
        public dynamic Data { get; set; }
        public dynamic Data_1 { get; set; }
        public dynamic Data_2 { get; set; }
        public dynamic Data_3 { get; set; }
        public dynamic Data_4 { get; set; }
    }

    public class ResponseBase
    {
        [DefaultValue(false)]
        public bool IsSuccess { get; set; }
        public List<Error> Errors { get; set; }
        public dynamic Data { get; set; }
        public dynamic Records { get; set; }
        public dynamic Data_1 { get; set; }
        public dynamic Data_2 { get; set; }
        public dynamic Data_3 { get; set; }
        public dynamic Data_4 { get; set; }
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; } 
        public ResponseBase()
        {
            Errors = new List<Error>();
        }
    }
     
}
