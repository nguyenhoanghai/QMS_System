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
        public int TicketNumber { get; set; }     
        public TimeSpan StartTime { get; set; }
        public TimeSpan TimeServeAllow { get; set; } 
    }
}
