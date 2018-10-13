using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class R_DetailInDayByServiceModel
    {
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int Index { get; set; }
        public decimal TicketNumber { get; set; }
        public string UserName { get; set; }
        public string CounterName { get; set; }
        public DateTime? PrintTime { get; set; }
        public DateTime? ProcessTime { get; set; }
        public DateTime? EndProcessTime { get; set; }
        public TimeSpan? TotalTransTime { get; set; }
        public int? TotalTransaction { get; set; }
        public string StatusCode { get; set; }
    }
}
