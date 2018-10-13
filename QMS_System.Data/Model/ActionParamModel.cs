using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class ActionParamModel  
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string ParameterCode { get; set; }
        public string Note { get; set; } 
    }
}
