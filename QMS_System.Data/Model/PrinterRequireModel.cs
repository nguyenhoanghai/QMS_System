using System;

namespace QMS_System.Data.Model
{
   public class PrinterRequireModel
    {
        public int PrinterId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ServeTime { get; set; }
        public string Name { get; set; }
        public int? DOB { get; set; }
        public string Address { get; set; }
        public string MaBenhNhan { get; set; }
        public string MaPhongKham { get; set; }
        public string SttPhongKham { get; set; }
        public string SoXe { get; set; }
    }
}
