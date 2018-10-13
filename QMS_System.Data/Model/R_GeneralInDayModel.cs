using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class R_GeneralInDayModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int  ObjectId { get; set; }
        public string Name { get; set; } 
        public int TotalTransaction { get; set; }
        public double TotalTransTime { get; set; }
        public double AverageTimePerTrans { get; set; }
        public DateTime  PrintTime { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public double AverageTimeWaitingBeforePerTrans { get; set; }
        public double AverageTimeWaitingAfterPerTrans { get; set; }
        public int? UserId { get; set; }
    } 
}
