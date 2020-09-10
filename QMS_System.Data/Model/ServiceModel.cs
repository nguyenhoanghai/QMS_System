using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.Model
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartNumber { get; set; }
        public int ServiceType { get; set; }
        public int EndNumber { get; set; }
        public bool IsActived { get; set; }
        public bool isKetLuan { get; set; }
        public DateTime TimeProcess { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
    }

    public class ServiceDayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
        public DateTime TimeProcess { get; set; } 
        public List<ServiceShiftModel> Shifts { get; set; }
        public ServiceDayModel()
        {
            Shifts = new List<ServiceShiftModel>();
        }
    }

    public class ServiceStepModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int ServiceId { get; set; }
        public int MajorId { get; set; }
    }
}
