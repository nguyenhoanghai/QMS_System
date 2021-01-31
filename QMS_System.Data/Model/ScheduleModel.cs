using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class ScheduleModel
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int YearOfBirth { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string KhungGio { get; set; }
        public int KhungGioId { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public DateTime ScheduleDate { get; set; }  
    }
}
