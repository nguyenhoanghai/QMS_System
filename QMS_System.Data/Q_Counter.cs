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
    
    public partial class Q_Counter
    {
        public Q_Counter()
        {
            this.Q_CounterSound = new HashSet<Q_CounterSound>();
            this.Q_Equipment = new HashSet<Q_Equipment>();
            this.Q_MaindisplayDirection = new HashSet<Q_MaindisplayDirection>();
        }
    
        public int Id { get; set; }
        public string ShortName { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Acreage { get; set; }
        public bool IsDeleted { get; set; }
        public int LastCall { get; set; }
        public bool IsRunning { get; set; }
    
        public virtual ICollection<Q_CounterSound> Q_CounterSound { get; set; }
        public virtual ICollection<Q_Equipment> Q_Equipment { get; set; }
        public virtual ICollection<Q_MaindisplayDirection> Q_MaindisplayDirection { get; set; }
    }
}
