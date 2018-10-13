using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class StatusModel
    {
        public int Id { get; set; }
        public int StatusTypeId { get; set; }
        public string Code { get; set; } 
        public string Note { get; set; } 
    }
}
