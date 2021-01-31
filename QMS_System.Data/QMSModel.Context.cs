﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QMSSystemEntities : DbContext
    {
        public QMSSystemEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Q_Action> Q_Action { get; set; }
        public DbSet<Q_ActionParameter> Q_ActionParameter { get; set; }
        public DbSet<Q_Alert> Q_Alert { get; set; }
        public DbSet<Q_Business> Q_Business { get; set; }
        public DbSet<Q_BusinessType> Q_BusinessType { get; set; }
        public DbSet<Q_Command> Q_Command { get; set; }
        public DbSet<Q_CommandParameter> Q_CommandParameter { get; set; }
        public DbSet<Q_CounterSound> Q_CounterSound { get; set; }
        public DbSet<Q_Equipment> Q_Equipment { get; set; }
        public DbSet<Q_EquipmentType> Q_EquipmentType { get; set; }
        public DbSet<Q_EquipTypeProcess> Q_EquipTypeProcess { get; set; }
        public DbSet<Q_Evaluate> Q_Evaluate { get; set; }
        public DbSet<Q_EvaluateDetail> Q_EvaluateDetail { get; set; }
        public DbSet<Q_Language> Q_Language { get; set; }
        public DbSet<Q_Login> Q_Login { get; set; }
        public DbSet<Q_LoginHistory> Q_LoginHistory { get; set; }
        public DbSet<Q_MaindisplayDirection> Q_MaindisplayDirection { get; set; }
        public DbSet<Q_Major> Q_Major { get; set; }
        public DbSet<Q_Policy> Q_Policy { get; set; }
        public DbSet<Q_Process> Q_Process { get; set; }
        public DbSet<Q_ReadTemp_Detail> Q_ReadTemp_Detail { get; set; }
        public DbSet<Q_ReadTemplate> Q_ReadTemplate { get; set; }
        public DbSet<Q_RegisterRecieveTicket> Q_RegisterRecieveTicket { get; set; }
        public DbSet<Q_ServiceShift> Q_ServiceShift { get; set; }
        public DbSet<Q_ServiceStep> Q_ServiceStep { get; set; }
        public DbSet<Q_Shift> Q_Shift { get; set; }
        public DbSet<Q_Sound> Q_Sound { get; set; }
        public DbSet<Q_Status> Q_Status { get; set; }
        public DbSet<Q_StatusType> Q_StatusType { get; set; }
        public DbSet<Q_User> Q_User { get; set; }
        public DbSet<Q_UserCmdRegister> Q_UserCmdRegister { get; set; }
        public DbSet<Q_UserCommandReadSound> Q_UserCommandReadSound { get; set; }
        public DbSet<Q_UserMajor> Q_UserMajor { get; set; }
        public DbSet<Q_VideoTemplate> Q_VideoTemplate { get; set; }
        public DbSet<Q_VideoTemplate_De> Q_VideoTemplate_De { get; set; }
        public DbSet<Q_DailyRequire_Detail> Q_DailyRequire_Detail { get; set; }
        public DbSet<Q_HisDailyRequire_De> Q_HisDailyRequire_De { get; set; }
        public DbSet<Q_RequestTicket> Q_RequestTicket { get; set; }
        public DbSet<Q_RecieverSMS> Q_RecieverSMS { get; set; }
        public DbSet<Q_ServiceLimit> Q_ServiceLimit { get; set; }
        public DbSet<Q_HisUserEvaluate> Q_HisUserEvaluate { get; set; }
        public DbSet<Q_UserEvaluate> Q_UserEvaluate { get; set; }
        public DbSet<Q_Config> Q_Config { get; set; }
        public DbSet<Q_TVReadSound> Q_TVReadSound { get; set; }
        public DbSet<Q_Counter> Q_Counter { get; set; }
        public DbSet<Q_Video> Q_Video { get; set; }
        public DbSet<Q_CounterDayInfo> Q_CounterDayInfo { get; set; }
        public DbSet<Q_WorkDetail> Q_WorkDetail { get; set; }
        public DbSet<Q_Works> Q_Works { get; set; }
        public DbSet<Q_WorkType> Q_WorkType { get; set; }
        public DbSet<Q_DailyRequire_WorkDetail> Q_DailyRequire_WorkDetail { get; set; }
        public DbSet<Q_HisDailyRequire_WorkDetail> Q_HisDailyRequire_WorkDetail { get; set; }
        public DbSet<Q_Service> Q_Service { get; set; }
        public DbSet<Q_DailyRequire> Q_DailyRequire { get; set; }
        public DbSet<Q_HisDailyRequire> Q_HisDailyRequire { get; set; }
        public DbSet<Q_PrintTicket> Q_PrintTicket { get; set; }
        public DbSet<Q_Schedule> Q_Schedule { get; set; }
        public DbSet<Q_CounterSoftRequire> Q_CounterSoftRequire { get; set; }
        public DbSet<Q_Customer> Q_Customer { get; set; }
        public DbSet<Q_KhungGio> Q_KhungGio { get; set; }
        public DbSet<Q_Schedule_Detail> Q_Schedule_Detail { get; set; }
    }
}
