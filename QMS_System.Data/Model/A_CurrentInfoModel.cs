using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class A_CurrentInfoModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string img { get; set; }
        public string Position { get; set; }
        public int Ticket { get; set; }
        public DateTime Start { get; set; }
       public List<ModelSelectItem> Evaluate { get; set; }
       public int StatusId { get; set; }
       public int ServiceId { get; set; }
       public DateTime? EndProcessTime { get; set; }
       public TimeSpan BaseServeTime { get; set; }
        public A_CurrentInfoModel() {
           Evaluate = new List<ModelSelectItem>();
       }
    }
}
