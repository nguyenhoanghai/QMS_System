using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
   public class CounterDayInfoModel
    {
        public int CounterId { get; set; }
        public int EquipCode { get; set; }
        public string TenQuay { get; set; }
        public DateTime? TGInPhieu { get; set; }
        public DateTime? TGBatDau { get; set; }
        public DateTime? TGChuan { get; set; }
        public TimeSpan? TGXuLy { get; set; }
        public TimeSpan? TGConLai { get; set; }
        public string STT_3 { get; set; }
        public int STT_QMS { get; set; }
        public int STT { get; set; }
        public int STT_UT { get; set; }
        public int TT_STT { get; set; }
        public int TT_STT_UT { get; set; }
        public string TrangThai { get; set; }
    }
}
