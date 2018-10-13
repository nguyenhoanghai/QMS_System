using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{ 
    public class ServiceShiftModel  
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int ShiftId { get; set; }
        public string Note { get; set; }
        public int Index { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
