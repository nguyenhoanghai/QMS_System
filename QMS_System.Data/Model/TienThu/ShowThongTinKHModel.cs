using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model.TienThu
{
   public class ShowThongTinKHModel
    {
        public bool IsShowVideo { get; set; }
        public string JsonKhachHang { get; set; }
        public List<VideoPlaylist> Videos { get; set; }
        public ShowThongTinKHModel()
        {
            Videos = new List<VideoPlaylist>();
        }
    }
}
