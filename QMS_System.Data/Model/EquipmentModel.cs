using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.Model
{
    public class EquipmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public int EquipTypeId { get; set; }
        public int CounterId { get; set; }
        public int StatusId { get; set; }
        public string Position { get; set; }
        public string Note { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
