using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class ReportModel
    {
        public int Id { get; set; }
        public int stt { get; set; }
        public int Number { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int MajorId { get; set; }
        public string MajorName { get; set; }
        public string CounterName { get; set; }
        public DateTime PrintTime { get; set; }
        public DateTime? Start { get; set; }
        public string str_Start { get; set; }
        public DateTime? End { get; set; }
        public string str_End { get; set; }
        public string ProcessTime { get; set; }
        public string WaitingTime { get; set; }
        public string StatusName { get; set; }

        public DateTime? StartTN { get; set; }
        public string str_StartTN { get; set; }
        public string WaitingTimeTN { get; set; }
       // public DateTime? EndTN { get; set; }
    }
}
