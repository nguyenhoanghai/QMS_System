using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class CommandParamModel  
    {
        public int Id { get; set; }
        public int CommandId { get; set; }
        public string Parameter { get; set; }
        public string Note { get; set; }
    }
}
