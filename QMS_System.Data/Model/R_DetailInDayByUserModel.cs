using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class R_DetailInDayByUserModel
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string EquipmentName { get; set; }
        public int Index { get; set; }
        public decimal TicketNumber { get; set; }
        public string MajorName { get; set; }
        public DateTime? PrintTime { get; set; }
        public DateTime? ProcessTime { get; set; }
        public DateTime? EndProcessTime { get; set; }
        public TimeSpan? TotalTransTime { get; set; }
        public int? TotalTransaction { get; set; }
    }
}
