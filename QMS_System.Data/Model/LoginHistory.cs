using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.Model
{
    public class LoginHistoryModel
    {
        public int Id { get; set; }
        public int EquipCode { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public int StatusId { get; set; }
        public DateTime? LogoutTime { get; set; }
        public int? TTPNEXT { get; set; }
    }
}
