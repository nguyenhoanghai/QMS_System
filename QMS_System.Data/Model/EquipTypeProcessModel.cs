using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class EquipTypeProcessModel  
    {
        public int Id { get; set; }
        public int EquipTypeId { get; set; }
        public int ProcessId { get; set; }
        public int Step { get; set; }
        public int Priority { get; set; }
        public int Count { get; set; }
    }
}
