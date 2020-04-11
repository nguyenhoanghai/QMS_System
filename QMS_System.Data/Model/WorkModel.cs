using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class WorkModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<WorkDetailModel> Details { get; set; }
        public WorkModel()
        {
            Details = new List<WorkDetailModel>();
        }

    }
    public class WorkDetailModel
    {
        public int Id { get; set; }
        public int WorkId { get; set; } 
        public string WorkName { get; set; }
        public DateTime TimeProcess { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeCode { get; set; }
        public string WorkTypeName { get; set; }

    }
}
