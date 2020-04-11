using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class TicketInfo
    {
        public int RequireDetailId { get; set; }
        public int RequireId { get; set; }
        public int CounterId { get; set; }
        public int TicketNumber { get; set; }     
        public int QMSNumber { get; set; }
        public string ThirdNumber  { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan TimeServeAllow { get; set; } 
        public DateTime? PrintTime { get; set; }
        public int EquipCode { get; set; }
        public string CountDown { get; set; }
    }
}
