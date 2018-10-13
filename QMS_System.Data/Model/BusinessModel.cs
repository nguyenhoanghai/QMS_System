using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class BusinessModel : Q_Business
    {
        public int Id { get; set; }
        public int BusinessTypeId { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int TotalTicket { get; set; }
    }
}
