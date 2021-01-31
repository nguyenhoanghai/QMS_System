using QMS_System.Data.Enum;
using QMS_System.Data.Model.TienThu;
using System;
using System.Linq;

namespace QMS_System.Data.BLL.TienThu
{
    public class BLLKhachHangInfo
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLKhachHangInfo _Instance;
        public static BLLKhachHangInfo Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLKhachHangInfo();

                return _Instance;
            }
        }
        private BLLKhachHangInfo() { }
        #endregion

        public ShowThongTinKHModel showThongTinKH(string connectString, string videoTemplate, TimeSpan timeStopInfo)
        {
            var obj = new ShowThongTinKHModel();
            var info = BLLCounterSoftRequire.Instance.Gets(connectString, (int)eCounterSoftRequireType.ShowCustDetail_TT).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (info != null && info.CreatedDate.AddSeconds(timeStopInfo.TotalSeconds) >= DateTime.Now)
            {
                obj.IsShowVideo = false;
                obj.JsonKhachHang = info.Content;
                return obj;
            }

            obj.IsShowVideo = true;
            obj.Videos = BLLVideoTemplate.Instance.GetPlaylist(connectString, videoTemplate);
            return obj;
        }
    }
}
