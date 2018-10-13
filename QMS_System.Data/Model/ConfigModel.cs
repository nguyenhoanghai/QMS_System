using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class ConfigModel 
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        public bool IsActived { get; set; }
    }
}
