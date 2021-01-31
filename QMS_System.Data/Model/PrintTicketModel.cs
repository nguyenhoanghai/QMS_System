using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class PrintTicketModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PrintTemplate { get; set; }
        public int PrintIndex { get; set; }
        public int PrintPages { get; set; }
        public bool IsActive { get; set; }
    }
}
