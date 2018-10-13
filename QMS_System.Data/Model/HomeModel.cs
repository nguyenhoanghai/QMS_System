﻿using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class HomeModel  
    {
        public string Counter { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public DateTime LoginTime { get; set; }
        public string AVGTime { get; set; }
        public int CurrentTicket { get; set; }
        public DateTime? CommingTime { get; set; } 
        public int TotalDone { get; set; }
        public int TotalWating { get; set; }
        public string CounterWaitingTickets { get; set; }
        public string AllWaitingTickets { get; set; }
        public int EquipCode { get; set; }
    }
}
