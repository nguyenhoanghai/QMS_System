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
    
    public partial class Q_ReadTemp_Detail
    {
        public int Id { get; set; }
        public int ReadTemplateId { get; set; }
        public int Index { get; set; }
        public int SoundId { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Q_ReadTemplate Q_ReadTemplate { get; set; }
    }
}
