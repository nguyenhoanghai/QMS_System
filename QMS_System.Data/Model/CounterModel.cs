using System;
using System.Collections.Generic;
using System.Linq; 

namespace QMS_System.Data.Model
{
    public class CounterModel 
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Acreage { get; set; }
    }
}
