using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class TimeScheduleModel
    {
        public int Id { get; set; }
        public int CustId { get; set; }
        public int KhungGioId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string CustName { get; set; }
        public string CustCode { get; set; }
        public string KhungGio { get; set; }
        public DateTime Time { get; set; }
        public string Note { get; set; } 
    }
}
