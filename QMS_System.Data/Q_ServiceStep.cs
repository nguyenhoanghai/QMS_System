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
    
    public partial class Q_ServiceStep
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int MajorId { get; set; }
        public int Index { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Q_Major Q_Major { get; set; }
        public virtual Q_Service Q_Service { get; set; }
    }
}
