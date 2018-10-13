using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class Login
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int EquipCode { get; set; }
        public int CounterId { get; set; }
        public string LoginTime { get; set; }
        public string CounterName { get; set; }
    }
}
