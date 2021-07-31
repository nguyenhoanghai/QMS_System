using System;
using System.Collections.Generic;
using System.Linq;

namespace QMS_System.Data.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Help { get; set; }
        public string Professional { get; set; }
        public string Position { get; set; }
        public string WorkingHistory { get; set; }
        public string Counters { get; set; }
    }
}
