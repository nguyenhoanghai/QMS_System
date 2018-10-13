using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class AlertModel
    {
        public int Id { get; set; }
        public int SoundId { get; set; }
        public string Note { get; set; } 
        public DateTime Start { get; set; }
        public DateTime End { get; set; } 
    }
}
