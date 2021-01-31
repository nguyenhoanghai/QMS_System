using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class CustomerModel
    {
        public int Id { get; set; }
        public int YearOfBirth { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string strGender { get; set; }
        public string Address { get; set; }      
        public string Phone { get; set; } 
    }
}
