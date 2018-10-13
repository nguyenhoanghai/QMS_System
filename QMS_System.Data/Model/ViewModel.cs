﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMS_System.Data.Model
{
    public class ViewModel
    {
        public string Date { get; set; }
        public string Time { get; set; }      
        public int TotalCar { get; set; }
        public int TotalCarServed { get; set; }
        public int TotalCarWaiting { get; set; }
        public int TotalCarProcessing { get; set; }
        public List<ViewDetailModel> Details { get; set; }

         public List<SubModel> Services { get; set; }
        
        public ViewModel()
        {
            Details = new List<ViewDetailModel>(); 
            Services = new List<SubModel>();
        }
    }

    public class ViewDetailModel
    {
        public int STT { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public string TableCode { get; set; }
        public int TicketNumber { get; set; }
        public string CarNumber { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Temp { get; set; }
        public string StartStr { get; set; }
        public string TimeProcess { get; set; }
        public string[] RuningText { get; set; }
        public TimeSpan ServeTimeAllow { get; set; }
        public DateTime TimeCL { get; set; }
        public string strServeTimeAllow { get; set; }
        public string strTimeCL { get; set; }
        public bool IsEndTime { get; set; }
    }

    public class SubModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int Ticket { get; set; }
    }

 
}
