using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class CommandModel  
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CodeHEX { get; set; }
        public string Note { get; set; }
    }
}
