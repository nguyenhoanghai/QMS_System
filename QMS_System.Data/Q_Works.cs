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
    
    public partial class Q_Works
    {
        public Q_Works()
        {
            this.Q_WorkDetail = new HashSet<Q_WorkDetail>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<Q_WorkDetail> Q_WorkDetail { get; set; }
    }
}
