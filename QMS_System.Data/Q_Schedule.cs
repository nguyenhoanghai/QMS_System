//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QMS_System.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Q_Schedule
    {
        public Q_Schedule()
        {
            this.Q_Schedule_Detail = new HashSet<Q_Schedule_Detail>();
        }
    
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<Q_Schedule_Detail> Q_Schedule_Detail { get; set; }
    }
}
