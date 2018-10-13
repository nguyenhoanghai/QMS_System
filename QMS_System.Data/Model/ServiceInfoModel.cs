using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMS_System.Data.Model
{
   public class ServiceInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
        public DateTime TimeProcess { get; set; }
        public string Note { get; set; }  
        public int TicketNumberProcessing { get; set; }
        public int TotalCarsWaiting { get; set; }
    }
}
