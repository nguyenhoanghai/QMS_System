using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{ 
    public class MaindisplayDirectionModel  
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int CounterId { get; set; }
        public int EquipmentId { get; set; }
        public int EquipmentCode { get; set; }
        public bool Direction { get; set; }
        public string Note { get; set; }
    }
}
