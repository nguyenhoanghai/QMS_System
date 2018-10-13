using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class R_GeneralByTimeRangeModel
    {
        public int Index { get; set; }
        public int? UserId { get; set; }
        public int? MajorId { get; set; }
        public string UserName { get; set; }
        public string MajorName { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? TotalTransaction { get; set; }
        public double? TotalTransTime { get; set; }
        public double? AverageTimePerTrans { get; set; }

        //public string EquipmentName { get; set; }
        //public long TicketNo { get; set; }
        //public string MajorName { get; set; }
        //public DateTime RequestTicketTime { get; set; }
        //public DateTime StartTransTime { get; set; }
        //public DateTime EndTransTime { get; set; }
        //public TimeSpan TotalTransTime { get; set; }
    }
}
