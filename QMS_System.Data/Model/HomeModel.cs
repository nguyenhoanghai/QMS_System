using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class HomeModel  
    {
        public string Counter { get; set; }
        public int UserId { get; set; }
        public int CounterId { get; set; }
        public string CounterName { get; set; }
        public string User { get; set; }
        public DateTime LoginTime { get; set; }
        public string AVGTime { get; set; }
        public int CurrentTicket { get; set; }
        public DateTime? CommingTime { get; set; } 
        public int TotalDone { get; set; }
        public int TotalWating { get; set; }
        public string CounterWaitingTickets { get; set; }
        public string CounterWaitingTickets_qualuot { get; set; }
        public string AllWaitingTickets { get; set; }
        public int EquipCode { get; set; }
        public int CountWaitAtCounter { get; set; }
        public bool IsLogout { get; set; }
        public DateTime? TimeRegister { get; set; }

    }
}
