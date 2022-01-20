using GPRO.Core.Mvc;
using Newtonsoft.Json;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using QMS_System.Data.Model.HuuNghi;
using QMS_System.ThirdApp;
using QMS_System.ThirdApp.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QMS_System.Data.BLL.HuuNghi
{
    public class BLLHuuNghi
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLHuuNghi _Instance;  //volatile =>  tranh dung thread
        public static BLLHuuNghi Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLHuuNghi();

                return _Instance;
            }
        }
        private BLLHuuNghi() { }
        #endregion

        public ResponseBaseModel chuyenJson(string connectString, string json, Boolean clearOldData)
        {
            var rs = new ResponseBaseModel();
            rs.IsSuccess = true;
            List<KhoaModel> khoaModels = new List<KhoaModel>();
            //string json = " [{'Khoa_Id':432,'MaKhoa':'KA','TenKhoa':'Khoa khám bệnh A','dsPhongKham':[{'PhongKham_Id':564,'MaPhongKham':'KB_KA_NOI1','TenPhongKham':'AN1 - Khám Nội 1 ','dsDichVu':[{'DichVu_Id':20544,'MaDichVu':'239137','TenDichVu':'Khám CK Nội Tổng hợp A (BS, ThS, BSCKI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':18485,'MaDichVu':'017215 ','TenDichVu':'Khám Trưởng Khoa Nội A'},{'DichVu_Id':20546,'MaDichVu':'239139','TenDichVu':'Khám TS, BSCKII [Nội Tổng hợp A]'}]},{'PhongKham_Id':539,'MaPhongKham':'KB_KA_NOI2','TenPhongKham':'AN2 - Khám Nội 2 ','dsDichVu':[{'DichVu_Id':20544,'MaDichVu':'239137','TenDichVu':'Khám CK Nội Tổng hợp A (BS, ThS, BSCKI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':20546,'MaDichVu':'239139','TenDichVu':'Khám TS, BSCKII [Nội Tổng hợp A]'}]},{'PhongKham_Id':563,'MaPhongKham':'KB_KA_NOI3','TenPhongKham':'AN3 - Khám Nội 3','dsDichVu':[{'DichVu_Id':20544,'MaDichVu':'239137','TenDichVu':'Khám CK Nội Tổng hợp A (BS, ThS, BSCKI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':18485,'MaDichVu':'017215 ','TenDichVu':'Khám Trưởng Khoa Nội A'},{'DichVu_Id':20546,'MaDichVu':'239139','TenDichVu':'Khám TS, BSCKII [Nội Tổng hợp A]'}]},{'PhongKham_Id':875,'MaPhongKham':'KB_KA_NOI4','TenPhongKham':'AN4 - Ra Viện','dsDichVu':[{'DichVu_Id':20544,'MaDichVu':'239137','TenDichVu':'Khám CK Nội Tổng hợp A (BS, ThS, BSCKI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':18485,'MaDichVu':'017215 ','TenDichVu':'Khám Trưởng Khoa Nội A'},{'DichVu_Id':20546,'MaDichVu':'239139','TenDichVu':'Khám TS, BSCKII [Nội Tổng hợp A]'}]},{'PhongKham_Id':871,'MaPhongKham':'KB_DVNA','TenPhongKham':'Khám Dịch Vụ Nội A','dsDichVu':[{'DichVu_Id':20544,'MaDichVu':'239137','TenDichVu':'Khám CK Nội Tổng hợp A (BS, ThS, BSCKI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':18485,'MaDichVu':'017215 ','TenDichVu':'Khám Trưởng Khoa Nội A'},{'DichVu_Id':20546,'MaDichVu':'239139','TenDichVu':'Khám TS, BSCKII [Nội Tổng hợp A]'}]},{'PhongKham_Id':870,'MaPhongKham':'KB_DL1','TenPhongKham':'Nội Đại Lải 1','dsDichVu':[{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':718,'MaPhongKham':'KB_KA_TD','TenPhongKham':'Phòng tiếp đón (Khám A)','dsDichVu':[]}]},{'Khoa_Id':433,'MaKhoa':'KB','TenKhoa':'Khoa Khám bệnh B','dsPhongKham':[{'PhongKham_Id':540,'MaPhongKham':'KB_KB_NOI18','TenPhongKham':'18B - Khám Nội','dsDichVu':[{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':549,'MaPhongKham':'KB_KB_NOI7','TenPhongKham':'201 - Khám Nội BS. Loan','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':578,'MaPhongKham':'KB_KB_NOI5','TenPhongKham':'202 - Khám Nội BS. Thành','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':879,'MaPhongKham':'KB_KB_NOI17','TenPhongKham':'203 - Khám Nội BS. Ngọc','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':20547,'MaDichVu':'239140','TenDichVu':'Khám TS, BSCKII [Khám Bệnh B]'}]},{'PhongKham_Id':576,'MaPhongKham':'KB_KB_NOI8','TenPhongKham':'204 - Khám Nội BS. Hồng','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':551,'MaPhongKham':'KB_KB_NOI6','TenPhongKham':'207 - Khám Da Liễu BS. Hương','dsDichVu':[{'DichVu_Id':20506,'MaDichVu':'239100','TenDichVu':'Khám Chuyên khoa (BS, ThS, BSCKI) [Da Liễu]'},{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1316,'MaDichVu':'01316','TenDichVu':'Khám Da liễu'},{'DichVu_Id':4910,'MaDichVu':'04910','TenDichVu':'Khám Da liễu [lần 2]'},{'DichVu_Id':4911,'MaDichVu':'04911','TenDichVu':'Khám Da liễu [lần 3]'},{'DichVu_Id':4912,'MaDichVu':'04912','TenDichVu':'Khám Da liễu [lần 4]'},{'DichVu_Id':4913,'MaDichVu':'04913','TenDichVu':'Khám Da liễu [lần 5]'},{'DichVu_Id':4915,'MaDichVu':'04915','TenDichVu':'Khám Da liễu [lần 6 trở lên]'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':20528,'MaDichVu':'239122','TenDichVu':'Khám TS, BSCKII [Da liễu]'}]},{'PhongKham_Id':550,'MaPhongKham':'KB_KB_NOI10','TenPhongKham':'209 - Khám Nội BS.Hằng','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':20547,'MaDichVu':'239140','TenDichVu':'Khám TS, BSCKII [Khám Bệnh B]'}]},{'PhongKham_Id':575,'MaPhongKham':'KB_KB_NOI4','TenPhongKham':'210 - Khám nội BS. Hằng','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':20547,'MaDichVu':'239140','TenDichVu':'Khám TS, BSCKII [Khám Bệnh B]'}]},{'PhongKham_Id':580,'MaPhongKham':'KB_KB_NOI12','TenPhongKham':'211 - Khám Nội BS. Chung','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':746,'MaPhongKham':'KB_KB_NOI212','TenPhongKham':'212 - Khám Nội','dsDichVu':[{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':908,'MaPhongKham':'KB_KB_NOI_KD','TenPhongKham':'213 - Khám Da Liễu BS.Trang','dsDichVu':[{'DichVu_Id':20506,'MaDichVu':'239100','TenDichVu':'Khám Chuyên khoa (BS, ThS, BSCKI) [Da Liễu]'},{'DichVu_Id':1316,'MaDichVu':'01316','TenDichVu':'Khám Da liễu'},{'DichVu_Id':4910,'MaDichVu':'04910','TenDichVu':'Khám Da liễu [lần 2]'},{'DichVu_Id':4911,'MaDichVu':'04911','TenDichVu':'Khám Da liễu [lần 3]'},{'DichVu_Id':4912,'MaDichVu':'04912','TenDichVu':'Khám Da liễu [lần 4]'},{'DichVu_Id':4913,'MaDichVu':'04913','TenDichVu':'Khám Da liễu [lần 5]'},{'DichVu_Id':4915,'MaDichVu':'04915','TenDichVu':'Khám Da liễu [lần 6 trở lên]'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':20528,'MaDichVu':'239122','TenDichVu':'Khám TS, BSCKII [Da liễu]'}]},{'PhongKham_Id':799,'MaPhongKham':'KB_KB_NOI213','TenPhongKham':'213 - Khám Nội Tổ','dsDichVu':[{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':595,'MaPhongKham':'KB_KB_CKTT','TenPhongKham':'213 - Khám Tâm thần','dsDichVu':[{'DichVu_Id':20545,'MaDichVu':'239138','TenDichVu':'Khám CK Khám  bệnh B(BS, ThS, BSCI)'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':2365,'MaDichVu':'02365','TenDichVu':'Khám tâm thần'},{'DichVu_Id':5005,'MaDichVu':'05005','TenDichVu':'Khám tâm thần [lần 2]'},{'DichVu_Id':5007,'MaDichVu':'05007','TenDichVu':'Khám tâm thần [lần 3]'},{'DichVu_Id':5008,'MaDichVu':'05008','TenDichVu':'Khám tâm thần [lần 4]'},{'DichVu_Id':5009,'MaDichVu':'05009','TenDichVu':'Khám tâm thần [lần 5]'},{'DichVu_Id':5010,'MaDichVu':'05010','TenDichVu':'Khám tâm thần [lần 6 trở lên]'}]},{'PhongKham_Id':567,'MaPhongKham':'KB_KB_PK3','TenPhongKham':'215 - Khám Phụ khoa','dsDichVu':[{'DichVu_Id':20507,'MaDichVu':'239101','TenDichVu':'Khám Chuyên khoa  (BS, ThS, BSCKI) [Phụ Khoa]'},{'DichVu_Id':1315,'MaDichVu':'01315','TenDichVu':'Khám Phụ sản'},{'DichVu_Id':4995,'MaDichVu':'04995','TenDichVu':'Khám Phụ sản [lần 2]'},{'DichVu_Id':4996,'MaDichVu':'04996','TenDichVu':'Khám Phụ sản [lần 3]'},{'DichVu_Id':4997,'MaDichVu':'04997','TenDichVu':'Khám Phụ sản [lần 4]'},{'DichVu_Id':4998,'MaDichVu':'04998','TenDichVu':'Khám Phụ sản [lần 5]'},{'DichVu_Id':4999,'MaDichVu':'04999','TenDichVu':'Khám Phụ sản [lần 6 trở lên]'},{'DichVu_Id':20529,'MaDichVu':'239123','TenDichVu':'Khám TS, BSCKII [Phụ khoa]'}]},{'PhongKham_Id':894,'MaPhongKham':'KB_KB_NOI9','TenPhongKham':'9B - Khám Nội','dsDichVu':[{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4901,'MaDichVu':'04901','TenDichVu':'Khám Nội [lần 3]'},{'DichVu_Id':4902,'MaDichVu':'04902','TenDichVu':'Khám Nội [lần 4]'},{'DichVu_Id':4903,'MaDichVu':'04903','TenDichVu':'Khám Nội [lần 5]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':674,'MaPhongKham':'KB_KB_TD','TenPhongKham':'Phòng Tiếp Đón(Khám B)','dsDichVu':[]},{'PhongKham_Id':737,'MaPhongKham':'LAS','TenPhongKham':'Phòng Laser - Khoa khám bệnh B','dsDichVu':[]}]},{'Khoa_Id':425,'MaKhoa':'VLTL-PHCN','TenKhoa':'Khoa Phục hồi chức năng','dsPhongKham':[{'PhongKham_Id':683,'MaPhongKham':'KB_VLTL_PTK11','TenPhongKham':'11VLTL BS Đạt','dsDichVu':[{'DichVu_Id':20517,'MaDichVu':'239111','TenDichVu':'Khám CK Phục hồi chức năng (BS, ThS, BSCKI)'},{'DichVu_Id':1309,'MaDichVu':'01309','TenDichVu':'Khám Phục hồi chức năng'},{'DichVu_Id':4970,'MaDichVu':'04970','TenDichVu':'Khám Phục hồi chức năng [lần 2]'},{'DichVu_Id':4971,'MaDichVu':'04971','TenDichVu':'Khám Phục hồi chức năng [lần 3]'},{'DichVu_Id':4974,'MaDichVu':'04974','TenDichVu':'Khám Phục hồi chức năng [lần 4]'},{'DichVu_Id':4976,'MaDichVu':'04976','TenDichVu':'Khám Phục hồi chức năng [lần 5]'},{'DichVu_Id':4977,'MaDichVu':'04977','TenDichVu':'Khám Phục hồi chức năng [lần 6 trở lên]'},{'DichVu_Id':20541,'MaDichVu':'239134','TenDichVu':'Khám TS, BSCKII [Phục hồi chức năng]'}]},{'PhongKham_Id':680,'MaPhongKham':'KB_VLTL_PK2','TenPhongKham':'2VLTL BS. Dần','dsDichVu':[{'DichVu_Id':1309,'MaDichVu':'01309','TenDichVu':'Khám Phục hồi chức năng'},{'DichVu_Id':4970,'MaDichVu':'04970','TenDichVu':'Khám Phục hồi chức năng [lần 2]'},{'DichVu_Id':4971,'MaDichVu':'04971','TenDichVu':'Khám Phục hồi chức năng [lần 3]'},{'DichVu_Id':4974,'MaDichVu':'04974','TenDichVu':'Khám Phục hồi chức năng [lần 4]'},{'DichVu_Id':4976,'MaDichVu':'04976','TenDichVu':'Khám Phục hồi chức năng [lần 5]'},{'DichVu_Id':4977,'MaDichVu':'04977','TenDichVu':'Khám Phục hồi chức năng [lần 6 trở lên]'},{'DichVu_Id':19169,'MaDichVu':'017638 ','TenDichVu':'Khám Trưởng khoa Phục hồi chức năng'}]},{'PhongKham_Id':679,'MaPhongKham':'KB_VLTL_DTCA3','TenPhongKham':'3VLTL BS Hiền','dsDichVu':[{'DichVu_Id':20517,'MaDichVu':'239111','TenDichVu':'Khám CK Phục hồi chức năng (BS, ThS, BSCKI)'},{'DichVu_Id':1309,'MaDichVu':'01309','TenDichVu':'Khám Phục hồi chức năng'},{'DichVu_Id':4970,'MaDichVu':'04970','TenDichVu':'Khám Phục hồi chức năng [lần 2]'},{'DichVu_Id':4971,'MaDichVu':'04971','TenDichVu':'Khám Phục hồi chức năng [lần 3]'},{'DichVu_Id':4974,'MaDichVu':'04974','TenDichVu':'Khám Phục hồi chức năng [lần 4]'},{'DichVu_Id':4976,'MaDichVu':'04976','TenDichVu':'Khám Phục hồi chức năng [lần 5]'},{'DichVu_Id':4977,'MaDichVu':'04977','TenDichVu':'Khám Phục hồi chức năng [lần 6 trở lên]'},{'DichVu_Id':20541,'MaDichVu':'239134','TenDichVu':'Khám TS, BSCKII [Phục hồi chức năng]'}]},{'PhongKham_Id':685,'MaPhongKham':'KB_VLTL_PK6','TenPhongKham':'6VLTL BS Linh','dsDichVu':[{'DichVu_Id':20517,'MaDichVu':'239111','TenDichVu':'Khám CK Phục hồi chức năng (BS, ThS, BSCKI)'},{'DichVu_Id':1309,'MaDichVu':'01309','TenDichVu':'Khám Phục hồi chức năng'},{'DichVu_Id':4970,'MaDichVu':'04970','TenDichVu':'Khám Phục hồi chức năng [lần 2]'},{'DichVu_Id':4971,'MaDichVu':'04971','TenDichVu':'Khám Phục hồi chức năng [lần 3]'},{'DichVu_Id':4974,'MaDichVu':'04974','TenDichVu':'Khám Phục hồi chức năng [lần 4]'},{'DichVu_Id':4976,'MaDichVu':'04976','TenDichVu':'Khám Phục hồi chức năng [lần 5]'},{'DichVu_Id':4977,'MaDichVu':'04977','TenDichVu':'Khám Phục hồi chức năng [lần 6 trở lên]'},{'DichVu_Id':20541,'MaDichVu':'239134','TenDichVu':'Khám TS, BSCKII [Phục hồi chức năng]'}]},{'PhongKham_Id':682,'MaPhongKham':'KB_VLTL_TD8','TenPhongKham':'8VLTL','dsDichVu':[{'DichVu_Id':20517,'MaDichVu':'239111','TenDichVu':'Khám CK Phục hồi chức năng (BS, ThS, BSCKI)'},{'DichVu_Id':1309,'MaDichVu':'01309','TenDichVu':'Khám Phục hồi chức năng'},{'DichVu_Id':4970,'MaDichVu':'04970','TenDichVu':'Khám Phục hồi chức năng [lần 2]'},{'DichVu_Id':4971,'MaDichVu':'04971','TenDichVu':'Khám Phục hồi chức năng [lần 3]'},{'DichVu_Id':4974,'MaDichVu':'04974','TenDichVu':'Khám Phục hồi chức năng [lần 4]'},{'DichVu_Id':4976,'MaDichVu':'04976','TenDichVu':'Khám Phục hồi chức năng [lần 5]'},{'DichVu_Id':4977,'MaDichVu':'04977','TenDichVu':'Khám Phục hồi chức năng [lần 6 trở lên]'},{'DichVu_Id':20541,'MaDichVu':'239134','TenDichVu':'Khám TS, BSCKII [Phục hồi chức năng]'}]},{'PhongKham_Id':662,'MaPhongKham':'VLTL_PK01','TenPhongKham':'Phòng Tiếp Đón(Khoa VLTL)','dsDichVu':[{'DichVu_Id':20543,'MaDichVu':'239136','TenDichVu':'Khám CK Hồi sức tích cực chống độc (BS, ThS, BSCKI)'},{'DichVu_Id':20517,'MaDichVu':'239111','TenDichVu':'Khám CK Phục hồi chức năng (BS, ThS, BSCKI)'},{'DichVu_Id':1309,'MaDichVu':'01309','TenDichVu':'Khám Phục hồi chức năng'},{'DichVu_Id':4970,'MaDichVu':'04970','TenDichVu':'Khám Phục hồi chức năng [lần 2]'},{'DichVu_Id':4971,'MaDichVu':'04971','TenDichVu':'Khám Phục hồi chức năng [lần 3]'},{'DichVu_Id':4974,'MaDichVu':'04974','TenDichVu':'Khám Phục hồi chức năng [lần 4]'},{'DichVu_Id':4976,'MaDichVu':'04976','TenDichVu':'Khám Phục hồi chức năng [lần 5]'},{'DichVu_Id':4977,'MaDichVu':'04977','TenDichVu':'Khám Phục hồi chức năng [lần 6 trở lên]'}]}]},{'Khoa_Id':227,'MaKhoa':'CDHA','TenKhoa':'Khoa Cdha','dsPhongKham':[{'PhongKham_Id':880,'MaPhongKham':'CHT','TenPhongKham':'Phòng Chụp Cộng hưởng từ (Tầng 2 - Nhà số 10)','dsDichVu':[]},{'PhongKham_Id':792,'MaPhongKham':'X_Q','TenPhongKham':'Phòng chụp X Quang (Tầng 2 - Nhà số 10)','dsDichVu':[]},{'PhongKham_Id':791,'MaPhongKham':'SANGT','TenPhongKham':'Phòng Siêu âm ngoại trú (Tầng 2 - Nhà số 10) ','dsDichVu':[]},{'PhongKham_Id':790,'MaPhongKham':'SANT','TenPhongKham':'Phòng Siêu âm Nội trú (Tầng 2 - Nhà số 10)','dsDichVu':[]}]},{'Khoa_Id':656,'MaKhoa':'GPBL','TenKhoa':'Khoa Giải phẫu bệnh','dsPhongKham':[]},{'Khoa_Id':419,'MaKhoa':'SH','TenKhoa':'Khoa Hóa sinh','dsPhongKham':[{'PhongKham_Id':881,'MaPhongKham':'PLM','TenPhongKham':'Phòng lấy máu (Tầng 1 - Nhà số 7)','dsDichVu':[]}]},{'Khoa_Id':228,'MaKhoa':'CNK','TenKhoa':'Khoa Kiểm soát nhiễm khuẩn','dsPhongKham':[]},{'Khoa_Id':422,'MaKhoa':'VS','TenKhoa':'Khoa Vi sinh','dsPhongKham':[]},{'Khoa_Id':233,'MaKhoa':'TN','TenKhoa':'Khoa Bệnh nhiệt đới','dsPhongKham':[{'PhongKham_Id':777,'MaPhongKham':'KB_KB_CKB2','TenPhongKham':'Khám Nhiệt đới T1','dsDichVu':[{'DichVu_Id':20508,'MaDichVu':'239102','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Bệnh Nhiệt Đớ]'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':5011,'MaDichVu':'05011','TenDichVu':'Khám Nội [Bệnh nhiệt đới - lần 2]'},{'DichVu_Id':5012,'MaDichVu':'05012','TenDichVu':'Khám Nội [Bệnh Nhiệt đới - lần 3]'},{'DichVu_Id':5013,'MaDichVu':'05013','TenDichVu':'Khám Nội [Bệnh nhiệt đới - lần 4]'},{'DichVu_Id':5014,'MaDichVu':'05014','TenDichVu':'Khám Nội [Bệnh nhiệt đới - lần 5]'},{'DichVu_Id':5015,'MaDichVu':'05015','TenDichVu':'Khám Nội [Bệnh nhiệt đới - lần 6 trở lên]'},{'DichVu_Id':2369,'MaDichVu':'02369','TenDichVu':'Khám Nội [Bệnh nhiệt đới]'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':20530,'MaDichVu':'239124','TenDichVu':'Khám TS, BSCKII [Bệnh nhiệt đới]'}]}]},{'Khoa_Id':408,'MaKhoa':'DU','TenKhoa':'Khoa Hô hấp và dị ứng','dsPhongKham':[{'PhongKham_Id':782,'MaPhongKham':'T4 - DU','TenPhongKham':'9B - Hô Hấp Dị Ứng','dsDichVu':[{'DichVu_Id':20516,'MaDichVu':'239110','TenDichVu':'Khám CK Hô hấp dị ứng (BS, ThS, BSCKI)'},{'DichVu_Id':4917,'MaDichVu':'04917','TenDichVu':'Khám Nội [Dị ứng - lần 2]'},{'DichVu_Id':4919,'MaDichVu':'04919','TenDichVu':'Khám Nội [Dị ứng - lần 3]'},{'DichVu_Id':4921,'MaDichVu':'04921','TenDichVu':'Khám Nội [Dị ứng - lần 4]'},{'DichVu_Id':4923,'MaDichVu':'04923','TenDichVu':'Khám Nội [Dị ứng - lần 5]'},{'DichVu_Id':4924,'MaDichVu':'04924','TenDichVu':'Khám Nội [Dị ứng - lần 6 trở lên]'},{'DichVu_Id':2370,'MaDichVu':'02370','TenDichVu':'Khám Nội [Dị Ứng]'},{'DichVu_Id':20540,'MaDichVu':'239133','TenDichVu':'Khám TS, BSCKII [Hô hấp dị ứng]'}]},{'PhongKham_Id':912,'MaPhongKham':'PSDU','TenPhongKham':'Phòng nội soi hô hấp dị ứng','dsDichVu':[]}]},{'Khoa_Id':420,'MaKhoa':'HH','TenKhoa':'Khoa Huyết học - truyền máu','dsPhongKham':[{'PhongKham_Id':778,'MaPhongKham':'KB_KB_CKHH','TenPhongKham':'18 Huyết Học','dsDichVu':[{'DichVu_Id':20514,'MaDichVu':'239108','TenDichVu':'Khám CK Huyết học truyền máu (BS, ThS, BSCKI)'},{'DichVu_Id':4945,'MaDichVu':'04945','TenDichVu':'Khám Nội [Huyết Học - lần 2]'},{'DichVu_Id':4949,'MaDichVu':'04949','TenDichVu':'Khám Nội [Huyết Học - lần 3]'},{'DichVu_Id':4955,'MaDichVu':'04955','TenDichVu':'Khám Nội [Huyết Học - lần 4]'},{'DichVu_Id':4956,'MaDichVu':'04956','TenDichVu':'Khám Nội [Huyết Học - lần 5]'},{'DichVu_Id':4960,'MaDichVu':'04960','TenDichVu':'Khám Nội [Huyết Học - lần 6 trở lên]'},{'DichVu_Id':2368,'MaDichVu':'02368','TenDichVu':'Khám Nội [Huyết Học]'},{'DichVu_Id':20538,'MaDichVu':'239131','TenDichVu':'Khám TS, BSCKII [Huyết học truyền máu]'}]}]},{'Khoa_Id':416,'MaKhoa':'DTTYC','TenKhoa':'Khoa Khám chữa bệnh theo yêu cầu','dsPhongKham':[{'PhongKham_Id':785,'MaPhongKham':'KBTYC_NGOAI4','TenPhongKham':'Khoa KBYC - Khám Ngoại 4','dsDichVu':[{'DichVu_Id':20498,'MaDichVu':'239093','TenDichVu':'Khám Chuyên Khoai (BS, ThS, BSCKI) [Ngoại]'},{'DichVu_Id':1308,'MaDichVu':'01308','TenDichVu':'Khám Ngoại'},{'DichVu_Id':4985,'MaDichVu':'04985','TenDichVu':'Khám Ngoại [lần 2]'},{'DichVu_Id':4986,'MaDichVu':'04986','TenDichVu':'Khám Ngoại [lần 3]'},{'DichVu_Id':4987,'MaDichVu':'04987','TenDichVu':'Khám Ngoại [lần 4]'},{'DichVu_Id':4988,'MaDichVu':'04988','TenDichVu':'Khám Ngoại [lần 5]'},{'DichVu_Id':4989,'MaDichVu':'04989','TenDichVu':'Khám Ngoại [lần 6 trở lên]'},{'DichVu_Id':19145,'MaDichVu':'0232.014 ','TenDichVu':'Khám nội TYC [Lần 6]'},{'DichVu_Id':4472,'MaDichVu':'04472','TenDichVu':'khám sức khoẻ cho người nước ngoài'},{'DichVu_Id':4168,'MaDichVu':'04168','TenDichVu':'Khám sức khoẻ toàn diện lao động, lái xe,khám sức khoẻ định kỳ (không kể xét nghiệm,X-quang);'},{'DichVu_Id':20522,'MaDichVu':'239116','TenDichVu':'Khám TS, BSCKII Ngoại'}]},{'PhongKham_Id':658,'MaPhongKham':'KBTYC_NOI1','TenPhongKham':'Khoa KBYC - Khám Nội 1 - BS. Cứu','dsDichVu':[{'DichVu_Id':1635,'MaDichVu':'02325.004','TenDichVu':'Khám Lại Dịch Vụ'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':1595,'MaDichVu':'02325.003','TenDichVu':'Khám Nội Dịch Vụ'},{'DichVu_Id':19145,'MaDichVu':'0232.014 ','TenDichVu':'Khám nội TYC [Lần 6]'},{'DichVu_Id':4472,'MaDichVu':'04472','TenDichVu':'khám sức khoẻ cho người nước ngoài'},{'DichVu_Id':4168,'MaDichVu':'04168','TenDichVu':'Khám sức khoẻ toàn diện lao động, lái xe,khám sức khoẻ định kỳ (không kể xét nghiệm,X-quang);'}]},{'PhongKham_Id':659,'MaPhongKham':'KBTYC_NOI2','TenPhongKham':'Khoa KBYC - Khám Nội 2 ','dsDichVu':[{'DichVu_Id':1635,'MaDichVu':'02325.004','TenDichVu':'Khám Lại Dịch Vụ'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':19145,'MaDichVu':'0232.014 ','TenDichVu':'Khám nội TYC [Lần 6]'},{'DichVu_Id':4472,'MaDichVu':'04472','TenDichVu':'khám sức khoẻ cho người nước ngoài'},{'DichVu_Id':4168,'MaDichVu':'04168','TenDichVu':'Khám sức khoẻ toàn diện lao động, lái xe,khám sức khoẻ định kỳ (không kể xét nghiệm,X-quang);'}]},{'PhongKham_Id':660,'MaPhongKham':'KBTYC_NOI3','TenPhongKham':'Khoa KBYC - Khám Nội 3 ','dsDichVu':[{'DichVu_Id':1635,'MaDichVu':'02325.004','TenDichVu':'Khám Lại Dịch Vụ'},{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'},{'DichVu_Id':19145,'MaDichVu':'0232.014 ','TenDichVu':'Khám nội TYC [Lần 6]'},{'DichVu_Id':4472,'MaDichVu':'04472','TenDichVu':'khám sức khoẻ cho người nước ngoài'},{'DichVu_Id':4168,'MaDichVu':'04168','TenDichVu':'Khám sức khoẻ toàn diện lao động, lái xe,khám sức khoẻ định kỳ (không kể xét nghiệm,X-quang);'}]},{'PhongKham_Id':661,'MaPhongKham':'KBTYC_RHM','TenPhongKham':'Khoa KBYC - Khám Răng Hàm Mặt','dsDichVu':[{'DichVu_Id':1596,'MaDichVu':'01596','TenDichVu':'Khám Trưởng Khoa'}]},{'PhongKham_Id':531,'MaPhongKham':'KBTYC_TD','TenPhongKham':'Khoa KBYC - Phòng tiếp đón','dsDichVu':[]},{'PhongKham_Id':257,'MaPhongKham':'YC1','TenPhongKham':'Phòng khám bệnh theo yêu cầu - Ngoại Trú','dsDichVu':[{'DichVu_Id':2325,'MaDichVu':'02325','TenDichVu':'Khám bệnh theo yêu cầu'},{'DichVu_Id':2164,'MaDichVu':'02325.008','TenDichVu':'Khám Da Liễu Dịch Vụ'},{'DichVu_Id':1635,'MaDichVu':'02325.004','TenDichVu':'Khám Lại Dịch Vụ'},{'DichVu_Id':2163,'MaDichVu':'02325.007','TenDichVu':'Khám Mắt Dịch Vụ'},{'DichVu_Id':1492,'MaDichVu':'02325.001','TenDichVu':'Khám Ngoại [Dịch Vụ]'},{'DichVu_Id':1595,'MaDichVu':'02325.003','TenDichVu':'Khám Nội Dịch Vụ'},{'DichVu_Id':2212,'MaDichVu':'02325.009','TenDichVu':'Khám Nội Soi TMH Dịch Vụ'},{'DichVu_Id':2213,'MaDichVu':'02325.010','TenDichVu':'Khám Phụ Khoa Dịch Vụ'},{'DichVu_Id':2161,'MaDichVu':'02325.005','TenDichVu':'Khám RHM Dịch Vụ'},{'DichVu_Id':4862,'MaDichVu':'04862','TenDichVu':'Khám Sức Khoẻ CBNV (đối tượng C)'},{'DichVu_Id':2162,'MaDichVu':'02325.006','TenDichVu':'Khám TMH Dịch Vụ'},{'DichVu_Id':1493,'MaDichVu':'02325.002','TenDichVu':'Khám Vật Lý Trị Liệu Dịch Vụ'}]},{'PhongKham_Id':913,'MaPhongKham':'KSKCB_KBTYC','TenPhongKham':'Phòng khám sức khỏe cán bộ CNVC ','dsDichVu':[{'DichVu_Id':1307,'MaDichVu':'01307','TenDichVu':'Khám Nội'},{'DichVu_Id':4900,'MaDichVu':'04900','TenDichVu':'Khám Nội [lần 2]'},{'DichVu_Id':4904,'MaDichVu':'04904','TenDichVu':'Khám Nội [lần 6 trở lên]'}]},{'PhongKham_Id':795,'MaPhongKham':'SATYC','TenPhongKham':'Siêu âm (KCBTYC)','dsDichVu':[]}]},{'Khoa_Id':415,'MaKhoa':'MAT','TenKhoa':'Khoa Mắt','dsPhongKham':[{'PhongKham_Id':714,'MaPhongKham':'KB_KM_PK9T','TenPhongKham':'308 - BS Thảo','dsDichVu':[{'DichVu_Id':20496,'MaDichVu':'239091','TenDichVu':'Khám Chuyên Khoa  (BS, ThS, BSCKI) [Mắt]'},{'DichVu_Id':1313,'MaDichVu':'01313','TenDichVu':'Khám Mắt'},{'DichVu_Id':4980,'MaDichVu':'04980','TenDichVu':'Khám Mắt [lần 2]'},{'DichVu_Id':4981,'MaDichVu':'04981','TenDichVu':'Khám Mắt [lần 3]'},{'DichVu_Id':4982,'MaDichVu':'04982','TenDichVu':'Khám Mắt [lần 4]'},{'DichVu_Id':4983,'MaDichVu':'04983','TenDichVu':'Khám Mắt [lần 5]'},{'DichVu_Id':4984,'MaDichVu':'04984','TenDichVu':'Khám Mắt [lần 6 trở lên]'},{'DichVu_Id':20520,'MaDichVu':'239114','TenDichVu':'Khám TS, BSCKII Mắt'}]},{'PhongKham_Id':689,'MaPhongKham':'KB_KM_TD2','TenPhongKham':'309 - Tiếp đón mắt','dsDichVu':[{'DichVu_Id':20496,'MaDichVu':'239091','TenDichVu':'Khám Chuyên Khoa  (BS, ThS, BSCKI) [Mắt]'},{'DichVu_Id':1313,'MaDichVu':'01313','TenDichVu':'Khám Mắt'},{'DichVu_Id':4980,'MaDichVu':'04980','TenDichVu':'Khám Mắt [lần 2]'},{'DichVu_Id':4981,'MaDichVu':'04981','TenDichVu':'Khám Mắt [lần 3]'},{'DichVu_Id':4982,'MaDichVu':'04982','TenDichVu':'Khám Mắt [lần 4]'},{'DichVu_Id':4983,'MaDichVu':'04983','TenDichVu':'Khám Mắt [lần 5]'},{'DichVu_Id':4984,'MaDichVu':'04984','TenDichVu':'Khám Mắt [lần 6 trở lên]'},{'DichVu_Id':20520,'MaDichVu':'239114','TenDichVu':'Khám TS, BSCKII Mắt'}]},{'PhongKham_Id':692,'MaPhongKham':'KB_KM_TK5','TenPhongKham':'310 - BS Hà','dsDichVu':[{'DichVu_Id':20496,'MaDichVu':'239091','TenDichVu':'Khám Chuyên Khoa  (BS, ThS, BSCKI) [Mắt]'},{'DichVu_Id':2754,'MaDichVu':'02754','TenDichVu':'Khám Giáo sư, Phó giáo sư '},{'DichVu_Id':1313,'MaDichVu':'01313','TenDichVu':'Khám Mắt'},{'DichVu_Id':4980,'MaDichVu':'04980','TenDichVu':'Khám Mắt [lần 2]'},{'DichVu_Id':4981,'MaDichVu':'04981','TenDichVu':'Khám Mắt [lần 3]'},{'DichVu_Id':4982,'MaDichVu':'04982','TenDichVu':'Khám Mắt [lần 4]'},{'DichVu_Id':4983,'MaDichVu':'04983','TenDichVu':'Khám Mắt [lần 5]'},{'DichVu_Id':4984,'MaDichVu':'04984','TenDichVu':'Khám Mắt [lần 6 trở lên]'},{'DichVu_Id':20520,'MaDichVu':'239114','TenDichVu':'Khám TS, BSCKII Mắt'}]},{'PhongKham_Id':695,'MaPhongKham':'KB_KM_PK7L','TenPhongKham':'311 - BS Lan','dsDichVu':[{'DichVu_Id':20496,'MaDichVu':'239091','TenDichVu':'Khám Chuyên Khoa  (BS, ThS, BSCKI) [Mắt]'},{'DichVu_Id':1313,'MaDichVu':'01313','TenDichVu':'Khám Mắt'},{'DichVu_Id':4980,'MaDichVu':'04980','TenDichVu':'Khám Mắt [lần 2]'},{'DichVu_Id':4981,'MaDichVu':'04981','TenDichVu':'Khám Mắt [lần 3]'},{'DichVu_Id':4982,'MaDichVu':'04982','TenDichVu':'Khám Mắt [lần 4]'},{'DichVu_Id':4983,'MaDichVu':'04983','TenDichVu':'Khám Mắt [lần 5]'},{'DichVu_Id':4984,'MaDichVu':'04984','TenDichVu':'Khám Mắt [lần 6 trở lên]'},{'DichVu_Id':20520,'MaDichVu':'239114','TenDichVu':'Khám TS, BSCKII Mắt'}]},{'PhongKham_Id':713,'MaPhongKham':'KB_KM_PK7H','TenPhongKham':'314 - BS Cường','dsDichVu':[{'DichVu_Id':20496,'MaDichVu':'239091','TenDichVu':'Khám Chuyên Khoa  (BS, ThS, BSCKI) [Mắt]'},{'DichVu_Id':1313,'MaDichVu':'01313','TenDichVu':'Khám Mắt'},{'DichVu_Id':4980,'MaDichVu':'04980','TenDichVu':'Khám Mắt [lần 2]'},{'DichVu_Id':4981,'MaDichVu':'04981','TenDichVu':'Khám Mắt [lần 3]'},{'DichVu_Id':4982,'MaDichVu':'04982','TenDichVu':'Khám Mắt [lần 4]'},{'DichVu_Id':4983,'MaDichVu':'04983','TenDichVu':'Khám Mắt [lần 5]'},{'DichVu_Id':4984,'MaDichVu':'04984','TenDichVu':'Khám Mắt [lần 6 trở lên]'},{'DichVu_Id':20520,'MaDichVu':'239114','TenDichVu':'Khám TS, BSCKII Mắt'}]},{'PhongKham_Id':702,'MaPhongKham':'KB_KM_9PKN','TenPhongKham':'315 - BS Nguyệt','dsDichVu':[{'DichVu_Id':20496,'MaDichVu':'239091','TenDichVu':'Khám Chuyên Khoa  (BS, ThS, BSCKI) [Mắt]'},{'DichVu_Id':1313,'MaDichVu':'01313','TenDichVu':'Khám Mắt'},{'DichVu_Id':4980,'MaDichVu':'04980','TenDichVu':'Khám Mắt [lần 2]'},{'DichVu_Id':4981,'MaDichVu':'04981','TenDichVu':'Khám Mắt [lần 3]'},{'DichVu_Id':4982,'MaDichVu':'04982','TenDichVu':'Khám Mắt [lần 4]'},{'DichVu_Id':4983,'MaDichVu':'04983','TenDichVu':'Khám Mắt [lần 5]'},{'DichVu_Id':4984,'MaDichVu':'04984','TenDichVu':'Khám Mắt [lần 6 trở lên]'},{'DichVu_Id':20520,'MaDichVu':'239114','TenDichVu':'Khám TS, BSCKII Mắt'}]},{'PhongKham_Id':715,'MaPhongKham':'SAM','TenPhongKham':'Sieu am mat','dsDichVu':[]}]},{'Khoa_Id':417,'MaKhoa':'NTH','TenKhoa':'Khoa Ngoại tổng hợp','dsPhongKham':[{'PhongKham_Id':775,'MaPhongKham':'KB_KB_CKN','TenPhongKham':'208 - Khám Ngoại','dsDichVu':[{'DichVu_Id':20498,'MaDichVu':'239093','TenDichVu':'Khám Chuyên Khoai (BS, ThS, BSCKI) [Ngoại]'},{'DichVu_Id':1308,'MaDichVu':'01308','TenDichVu':'Khám Ngoại'},{'DichVu_Id':4985,'MaDichVu':'04985','TenDichVu':'Khám Ngoại [lần 2]'},{'DichVu_Id':4986,'MaDichVu':'04986','TenDichVu':'Khám Ngoại [lần 3]'},{'DichVu_Id':4987,'MaDichVu':'04987','TenDichVu':'Khám Ngoại [lần 4]'},{'DichVu_Id':4988,'MaDichVu':'04988','TenDichVu':'Khám Ngoại [lần 5]'},{'DichVu_Id':4989,'MaDichVu':'04989','TenDichVu':'Khám Ngoại [lần 6 trở lên]'},{'DichVu_Id':20522,'MaDichVu':'239116','TenDichVu':'Khám TS, BSCKII Ngoại'}]},{'PhongKham_Id':725,'MaPhongKham':'PB','TenPhongKham':'PB - Phòng Băng - Khoa Ngoại','dsDichVu':[]},{'PhongKham_Id':724,'MaPhongKham':'PX','TenPhongKham':'PX - Phòng xương - Khoa Ngoại','dsDichVu':[]}]},{'Khoa_Id':410,'MaKhoa':'CXK','TenKhoa':'Khoa Nội cơ xương khớp','dsPhongKham':[{'PhongKham_Id':781,'MaPhongKham':'T4 - CXK','TenPhongKham':'PK Cơ Xương Khớp','dsDichVu':[{'DichVu_Id':20495,'MaDichVu':'239090','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Cơ Xương Khớp]'},{'DichVu_Id':2366,'MaDichVu':'02366','TenDichVu':'Khám Nội [Cơ - xương - khớp]'},{'DichVu_Id':4909,'MaDichVu':'04909','TenDichVu':'Khám Nội [Cơ xương khớp - lần  6 trở lên]'},{'DichVu_Id':4905,'MaDichVu':'04905','TenDichVu':'Khám Nội [Cơ xương khớp - lần 2]'},{'DichVu_Id':4906,'MaDichVu':'04906','TenDichVu':'Khám Nội [Cơ xương khớp - lần 3]'},{'DichVu_Id':4907,'MaDichVu':'04907','TenDichVu':'Khám Nội [Cơ xương khớp - lần 4]'},{'DichVu_Id':4908,'MaDichVu':'04908','TenDichVu':'Khám Nội [Cơ xương khớp - lần 5]'},{'DichVu_Id':18241,'MaDichVu':'017152','TenDichVu':'Khám Trưởng khoa Cơ Xương Khớp'},{'DichVu_Id':20519,'MaDichVu':'239113','TenDichVu':'Khám TS, BSCKII Cơ Xương Khớp'}]},{'PhongKham_Id':717,'MaPhongKham':'DLX','TenPhongKham':'Phòng đo độ loãng xương (Tầng 2 - Nhà 10)','dsDichVu':[]}]},{'Khoa_Id':411,'MaKhoa':'TK','TenKhoa':'Khoa Nội thần kinh','dsPhongKham':[{'PhongKham_Id':574,'MaPhongKham':'KB_KB_CKTK13','TenPhongKham':'13B - CK Thần kinh ','dsDichVu':[{'DichVu_Id':20509,'MaDichVu':'239103','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Thần Kinh]'},{'DichVu_Id':2754,'MaDichVu':'02754','TenDichVu':'Khám Giáo sư, Phó giáo sư '},{'DichVu_Id':4914,'MaDichVu':'04914','TenDichVu':'Khám Nội [Thần Kinh - lần 2]'},{'DichVu_Id':4916,'MaDichVu':'04916','TenDichVu':'Khám Nội [Thần Kinh - lần 3]'},{'DichVu_Id':4918,'MaDichVu':'04918','TenDichVu':'Khám Nội [Thần Kinh - lần 4]'},{'DichVu_Id':4920,'MaDichVu':'04920','TenDichVu':'Khám Nội [Thần Kinh - lần 5]'},{'DichVu_Id':4922,'MaDichVu':'04922','TenDichVu':'Khám Nội [Thần kinh - lần 6 trở lên]'},{'DichVu_Id':1312,'MaDichVu':'01312','TenDichVu':'Khám Nội [Thần Kinh]'},{'DichVu_Id':20532,'MaDichVu':'239126','TenDichVu':'Khám TS, BSCKII [Thần kinh]'}]}]},{'Khoa_Id':774,'MaKhoa':'KNT','TenKhoa':'Khoa Nội tiết - đái tháo đường','dsPhongKham':[{'PhongKham_Id':786,'MaPhongKham':'KB_NTDTD1','TenPhongKham':'ĐTĐ - Đái Tháo Đường 1','dsDichVu':[{'DichVu_Id':20511,'MaDichVu':'239105','TenDichVu':'Khám CK Nội tiết đái tháo đường (BS, ThS, BSCKI)'},{'DichVu_Id':4161,'MaDichVu':'04161','TenDichVu':'Khám Nội tiết'},{'DichVu_Id':4990,'MaDichVu':'04990','TenDichVu':'Khám Nội Tiết [lần 2]'},{'DichVu_Id':4991,'MaDichVu':'04991','TenDichVu':'Khám Nội Tiết [lần 3]'},{'DichVu_Id':4992,'MaDichVu':'04992','TenDichVu':'Khám Nội Tiết [lần 4]'},{'DichVu_Id':4993,'MaDichVu':'04993','TenDichVu':'Khám Nội Tiết [lần 5]'},{'DichVu_Id':4994,'MaDichVu':'04994','TenDichVu':'Khám Nội tiết [lần 6 trở lên]'},{'DichVu_Id':20533,'MaDichVu':'239127','TenDichVu':'Khám TS, BSCKII [Nội tiết đái tháo đường]'}]},{'PhongKham_Id':891,'MaPhongKham':'KB_NTDTD2','TenPhongKham':'ĐTĐ2 Đái Tháo Đường 2','dsDichVu':[{'DichVu_Id':20511,'MaDichVu':'239105','TenDichVu':'Khám CK Nội tiết đái tháo đường (BS, ThS, BSCKI)'},{'DichVu_Id':4161,'MaDichVu':'04161','TenDichVu':'Khám Nội tiết'},{'DichVu_Id':4990,'MaDichVu':'04990','TenDichVu':'Khám Nội Tiết [lần 2]'},{'DichVu_Id':4991,'MaDichVu':'04991','TenDichVu':'Khám Nội Tiết [lần 3]'},{'DichVu_Id':4992,'MaDichVu':'04992','TenDichVu':'Khám Nội Tiết [lần 4]'},{'DichVu_Id':4993,'MaDichVu':'04993','TenDichVu':'Khám Nội Tiết [lần 5]'},{'DichVu_Id':4994,'MaDichVu':'04994','TenDichVu':'Khám Nội tiết [lần 6 trở lên]'},{'DichVu_Id':20533,'MaDichVu':'239127','TenDichVu':'Khám TS, BSCKII [Nội tiết đái tháo đường]'}]}]},{'Khoa_Id':412,'MaKhoa':'TH','TenKhoa':'Khoa Nội tiêu hóa','dsPhongKham':[{'PhongKham_Id':594,'MaPhongKham':'KB_KB_CKTH15','TenPhongKham':'CK Tiêu Hóa ','dsDichVu':[{'DichVu_Id':20513,'MaDichVu':'239107','TenDichVu':'Khám CK Tiêu Hóa (BS, ThS, BSCKI)'},{'DichVu_Id':4935,'MaDichVu':'04935','TenDichVu':'Khám Nội [Tiêu Hóa - lần 2]'},{'DichVu_Id':4936,'MaDichVu':'04936','TenDichVu':'Khám Nội [Tiêu Hóa - lần 3]'},{'DichVu_Id':4937,'MaDichVu':'04937','TenDichVu':'Khám Nội [Tiêu Hóa - lần 4]'},{'DichVu_Id':4938,'MaDichVu':'04938','TenDichVu':'Khám Nội [Tiêu Hóa - lần 5]'},{'DichVu_Id':4939,'MaDichVu':'04939','TenDichVu':'Khám Nội [Tiêu Hóa - lần 6 trở lên]'},{'DichVu_Id':2382,'MaDichVu':'02382','TenDichVu':'Khám Nội [Tiêu Hoá]'},{'DichVu_Id':18548,'MaDichVu':'017227 ','TenDichVu':'Khám trưởng khoa Tiêu Hóa'},{'DichVu_Id':20535,'MaDichVu':'239129','TenDichVu':'Khám TS, BSCKII [Tiêu hóa]'}]},{'PhongKham_Id':727,'MaPhongKham':'PS','TenPhongKham':'Phòng nội soi tiêu hóa','dsDichVu':[{'DichVu_Id':18548,'MaDichVu':'017227 ','TenDichVu':'Khám trưởng khoa Tiêu Hóa'}]}]},{'Khoa_Id':409,'MaKhoa':'TM','TenKhoa':'Khoa Nội tim mạch','dsPhongKham':[{'PhongKham_Id':577,'MaPhongKham':'KB_KB_CKTM11','TenPhongKham':'11B - CK Tim Mạch ','dsDichVu':[{'DichVu_Id':20512,'MaDichVu':'239106','TenDichVu':'Khám CK Tim Mạch (BS, ThS, BSCKI)'},{'DichVu_Id':4940,'MaDichVu':'04940','TenDichVu':'Khám Nội [Tim Mạch - lần 2]'},{'DichVu_Id':4941,'MaDichVu':'04941','TenDichVu':'Khám Nội [Tim Mạch - lần 3]'},{'DichVu_Id':4942,'MaDichVu':'04942','TenDichVu':'Khám Nội [Tim Mạch - lần 4]'},{'DichVu_Id':4943,'MaDichVu':'04943','TenDichVu':'Khám Nội [Tim Mạch - lần 5]'},{'DichVu_Id':4944,'MaDichVu':'04944','TenDichVu':'Khám Nội [Tim Mạch - lần 6 trở lên]'},{'DichVu_Id':2383,'MaDichVu':'02383','TenDichVu':'Khám Nội [Tim Mạch]'},{'DichVu_Id':20534,'MaDichVu':'239128','TenDichVu':'Khám TS, BSCKII [Tim mạch]'}]}]},{'Khoa_Id':431,'MaKhoa':'NTHA','TenKhoa':'Khoa Nội tổng hợp A','dsPhongKham':[{'PhongKham_Id':885,'MaPhongKham':'PSA','TenPhongKham':'Phòng nội soi Nội A','dsDichVu':[]},{'PhongKham_Id':898,'MaPhongKham':'SANA','TenPhongKham':'Phòng siêu âm Nội A','dsDichVu':[]}]},{'Khoa_Id':426,'MaKhoa':'TMH','TenKhoa':'Khoa Tai mũi họng','dsPhongKham':[{'PhongKham_Id':693,'MaPhongKham':'KB_TMH_D','TenPhongKham':'410 - TMH BS Hằng','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':18552,'MaDichVu':'017229 ','TenDichVu':'Khám trưởng khoa Tai Mũi Họng'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':896,'MaPhongKham':'KNSTMH','TenPhongKham':'412 - TMH Bs Phương','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':691,'MaPhongKham':'KB_TMH_E','TenPhongKham':'413 - TMH BS Minh','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':892,'MaPhongKham':'KB_TMH_H','TenPhongKham':'414 - TMH BS Chi','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':688,'MaPhongKham':'KB_TMH_B','TenPhongKham':'415 - TMH Bs Hà','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':690,'MaPhongKham':'KB_TMH_A','TenPhongKham':'416 - TMH BS Kiên','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':18552,'MaDichVu':'017229 ','TenDichVu':'Khám trưởng khoa Tai Mũi Họng'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':700,'MaPhongKham':'KB_TMH_DTL-NL','TenPhongKham':'DTLNL - TMH Đo Thị Lực ,Nhị Lượng','dsDichVu':[{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'}]},{'PhongKham_Id':699,'MaPhongKham':'KB_TMH_KD','TenPhongKham':'KD - TMH Khí Dung','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':694,'MaPhongKham':'KB_TMH_NS','TenPhongKham':'NS - TMH Nội Soi','dsDichVu':[{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'}]},{'PhongKham_Id':684,'MaPhongKham':'KB_TMH_HC','TenPhongKham':'Phòng Hành Chính (TMH)','dsDichVu':[]},{'PhongKham_Id':681,'MaPhongKham':'KB_TMH_TD','TenPhongKham':'Phòng Tiếp Đón (Khoa TMH)','dsDichVu':[]},{'PhongKham_Id':696,'MaPhongKham':'KB_TMH_TK','TenPhongKham':'TK - TMH Trưởng Khoa','dsDichVu':[{'DichVu_Id':20502,'MaDichVu':'239097','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tai Mũi Họng]'},{'DichVu_Id':1311,'MaDichVu':'01311','TenDichVu':'Khám Tai mũi họng'},{'DichVu_Id':4946,'MaDichVu':'04946','TenDichVu':'Khám Tai mũi họng [lần 2]'},{'DichVu_Id':4947,'MaDichVu':'04947','TenDichVu':'Khám Tai mũi họng [lần 3]'},{'DichVu_Id':4948,'MaDichVu':'04948','TenDichVu':'Khám Tai mũi họng [lần 4]'},{'DichVu_Id':4950,'MaDichVu':'04950','TenDichVu':'Khám Tai mũi họng [lần 5]'},{'DichVu_Id':4951,'MaDichVu':'04951','TenDichVu':'Khám Tai mũi họng [lần 6  trở lên]'},{'DichVu_Id':14169,'MaDichVu':'14169','TenDichVu':'Khám Tai mũi họng [Tiếp đón]'},{'DichVu_Id':20526,'MaDichVu':'239120','TenDichVu':'Khám TS, BSCKII [Tai mũi họng]'}]},{'PhongKham_Id':794,'MaPhongKham':'NSTMH','TenPhongKham':'Phòng nội soi tai mũi họng','dsDichVu':[]}]},{'Khoa_Id':413,'MaKhoa':'TTN','TenKhoa':'Khoa Thận tiết niệu - lọc máu (điều trị)','dsPhongKham':[{'PhongKham_Id':889,'MaPhongKham':'T4_TTN','TenPhongKham':'212 - Khám Thận tiết niệu','dsDichVu':[{'DichVu_Id':20497,'MaDichVu':'239092','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Thận tiết niệu lọc máu]'},{'DichVu_Id':4926,'MaDichVu':'04926','TenDichVu':'Khám Nội [Thận Tiết Niệu - lần 2]'},{'DichVu_Id':4164,'MaDichVu':'04164','TenDichVu':'Khám Nội [Thận Tiết Niệu - lần 3]'},{'DichVu_Id':4165,'MaDichVu':'04165','TenDichVu':'Khám Nội [Thận Tiết Niệu - lần 4]'},{'DichVu_Id':2398,'MaDichVu':'02398','TenDichVu':'Khám Nội [Thận Tiết Niệu - lần 5]'},{'DichVu_Id':2892,'MaDichVu':'02892','TenDichVu':'Khám Nội [Thận Tiết Niệu - lần 6 trở lên]'},{'DichVu_Id':4890,'MaDichVu':'04890','TenDichVu':'Khám Nội [Thận Tiết Niệu]'},{'DichVu_Id':4930,'MaDichVu':'04930','TenDichVu':'Khám trưởng khoa Can thiệp tim mạch'},{'DichVu_Id':20521,'MaDichVu':'239115','TenDichVu':'Khám TS, BSCKII Thận tiết niệu Lọc Máu'}]}]},{'Khoa_Id':424,'MaKhoa':'TNT','TenKhoa':'Khoa Thận tiết niệu - lọc máu (lọc máu)','dsPhongKham':[{'PhongKham_Id':780,'MaPhongKham':'KB_KB_CKLM','TenPhongKham':'18 Lọc Máu','dsDichVu':[{'DichVu_Id':20497,'MaDichVu':'239092','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Thận tiết niệu lọc máu]'},{'DichVu_Id':4963,'MaDichVu':'04963','TenDichVu':'Khám Nội [Lọc Máu - lần 2]'},{'DichVu_Id':4966,'MaDichVu':'04966','TenDichVu':'Khám Nội [Lọc Máu - lần 3]'},{'DichVu_Id':4967,'MaDichVu':'04967','TenDichVu':'Khám Nội [Lọc Máu - lần 4]'},{'DichVu_Id':4968,'MaDichVu':'04968','TenDichVu':'Khám Nội [Lọc Máu - lần 5]'},{'DichVu_Id':4969,'MaDichVu':'04969','TenDichVu':'Khám Nội [Lọc Máu - lần 6 trở lên]'},{'DichVu_Id':2391,'MaDichVu':'02391','TenDichVu':'Khám Nội [Lọc Máu]'},{'DichVu_Id':20521,'MaDichVu':'239115','TenDichVu':'Khám TS, BSCKII Thận tiết niệu Lọc Máu'}]}]},{'Khoa_Id':726,'MaKhoa':'PCM','TenKhoa':'Khoa Tim mạch can thiệp','dsPhongKham':[{'PhongKham_Id':779,'MaPhongKham':'KB_KB_CKMV','TenPhongKham':'212 - Khám Mạch vành','dsDichVu':[{'DichVu_Id':20501,'MaDichVu':'239096','TenDichVu':'Khám Chuyên Khoa (BS, ThS, BSCKI) [Tim mạch can thiệp]'},{'DichVu_Id':4972,'MaDichVu':'04972','TenDichVu':'Khám Nội [Mạch Vành - lần 2]'},{'DichVu_Id':4973,'MaDichVu':'04973','TenDichVu':'Khám Nội [Mạch Vành - lần 3]'},{'DichVu_Id':4975,'MaDichVu':'04975','TenDichVu':'Khám Nội [Mạch Vành - lần 4]'},{'DichVu_Id':4978,'MaDichVu':'04978','TenDichVu':'Khám Nội [Mạch Vành - lần 5]'},{'DichVu_Id':4979,'MaDichVu':'04979','TenDichVu':'Khám Nội [Mạch Vành - lần 6 trở lên]'},{'DichVu_Id':2403,'MaDichVu':'02403','TenDichVu':'Khám Nội [Mạch Vành]'},{'DichVu_Id':20525,'MaDichVu':'239119','TenDichVu':'Khám TS, BSCKII [Tim mạch can thiệp]'}]}]},{'Khoa_Id':771,'MaKhoa':'UB','TenKhoa':'Khoa Ung bướu xạ trị','dsPhongKham':[{'PhongKham_Id':747,'MaPhongKham':'KB_KB_CXK_DU','TenPhongKham':'Ung Bướu','dsDichVu':[{'DichVu_Id':2367,'MaDichVu':'02367','TenDichVu':'Khám Ung bướu'},{'DichVu_Id':4958,'MaDichVu':'04958','TenDichVu':'Khám Ung bướu [lần 2]'},{'DichVu_Id':4961,'MaDichVu':'04961','TenDichVu':'Khám Ung bướu [lần 3]'},{'DichVu_Id':4962,'MaDichVu':'04962','TenDichVu':'Khám Ung bướu [lần 4]'},{'DichVu_Id':4964,'MaDichVu':'04964','TenDichVu':'Khám Ung bướu [lần 5]'},{'DichVu_Id':4965,'MaDichVu':'04965','TenDichVu':'Khám Ung bướu [lần 6 trở lên]'}]},{'PhongKham_Id':903,'MaPhongKham':'T6UB','TenPhongKham':'Ung Bướu Tầng 6','dsDichVu':[{'DichVu_Id':20515,'MaDichVu':'239109','TenDichVu':'Khám CK Ung bướu xạ trị (BS, ThS, BSCKI)'},{'DichVu_Id':18483,'MaDichVu':'017213 ','TenDichVu':'Khám Trưởng khoa Ung Bướu Xạ Trị'},{'DichVu_Id':20539,'MaDichVu':'239132','TenDichVu':'Khám TS, BSCKII [Ung bướu xạ trị]'},{'DichVu_Id':2367,'MaDichVu':'02367','TenDichVu':'Khám Ung bướu'},{'DichVu_Id':4958,'MaDichVu':'04958','TenDichVu':'Khám Ung bướu [lần 2]'},{'DichVu_Id':4961,'MaDichVu':'04961','TenDichVu':'Khám Ung bướu [lần 3]'},{'DichVu_Id':4962,'MaDichVu':'04962','TenDichVu':'Khám Ung bướu [lần 4]'},{'DichVu_Id':4964,'MaDichVu':'04964','TenDichVu':'Khám Ung bướu [lần 5]'},{'DichVu_Id':4965,'MaDichVu':'04965','TenDichVu':'Khám Ung bướu [lần 6 trở lên]'}]}]},{'Khoa_Id':414,'MaKhoa':'YHCT','TenKhoa':'Khoa Y học cổ truyền','dsPhongKham':[{'PhongKham_Id':538,'MaPhongKham':'KB_KA_CKDY','TenPhongKham':'ADY - Đông Y BS. Lý','dsDichVu':[{'DichVu_Id':20510,'MaDichVu':'239104','TenDichVu':'Khám CK Y Học cổ truyền (BS, ThS, BSCKI)'},{'DichVu_Id':18484,'MaDichVu':'017214 ','TenDichVu':'khám trưởng khoa Y học Cổ Truyền'},{'DichVu_Id':20531,'MaDichVu':'239125','TenDichVu':'Khám TS, BSCKII [Y học cổ truyền]'},{'DichVu_Id':1317,'MaDichVu':'01317','TenDichVu':'Khám YHCT'},{'DichVu_Id':4925,'MaDichVu':'04925','TenDichVu':'Khám YHCT [lần 2]'},{'DichVu_Id':4928,'MaDichVu':'04928','TenDichVu':'Khám YHCT [lần 3]'},{'DichVu_Id':4931,'MaDichVu':'04931','TenDichVu':'Khám YHCT [lần 4]'},{'DichVu_Id':4933,'MaDichVu':'04933','TenDichVu':'Khám YHCT [lần 5]'},{'DichVu_Id':4934,'MaDichVu':'04934','TenDichVu':'Khám YHCT [lần 6 trở lên]'}]},{'PhongKham_Id':776,'MaPhongKham':'KB_KB_CKDY ','TenPhongKham':'Đông Y 1','dsDichVu':[{'DichVu_Id':20510,'MaDichVu':'239104','TenDichVu':'Khám CK Y Học cổ truyền (BS, ThS, BSCKI)'},{'DichVu_Id':18484,'MaDichVu':'017214 ','TenDichVu':'khám trưởng khoa Y học Cổ Truyền'},{'DichVu_Id':20531,'MaDichVu':'239125','TenDichVu':'Khám TS, BSCKII [Y học cổ truyền]'},{'DichVu_Id':1317,'MaDichVu':'01317','TenDichVu':'Khám YHCT'},{'DichVu_Id':4925,'MaDichVu':'04925','TenDichVu':'Khám YHCT [lần 2]'},{'DichVu_Id':4928,'MaDichVu':'04928','TenDichVu':'Khám YHCT [lần 3]'},{'DichVu_Id':4931,'MaDichVu':'04931','TenDichVu':'Khám YHCT [lần 4]'},{'DichVu_Id':4933,'MaDichVu':'04933','TenDichVu':'Khám YHCT [lần 5]'},{'DichVu_Id':4934,'MaDichVu':'04934','TenDichVu':'Khám YHCT [lần 6 trở lên]'}]},{'PhongKham_Id':878,'MaPhongKham':'KB_KB_CKDY 2','TenPhongKham':'Đông Y 2','dsDichVu':[{'DichVu_Id':20510,'MaDichVu':'239104','TenDichVu':'Khám CK Y Học cổ truyền (BS, ThS, BSCKI)'},{'DichVu_Id':18484,'MaDichVu':'017214 ','TenDichVu':'khám trưởng khoa Y học Cổ Truyền'},{'DichVu_Id':20531,'MaDichVu':'239125','TenDichVu':'Khám TS, BSCKII [Y học cổ truyền]'},{'DichVu_Id':1317,'MaDichVu':'01317','TenDichVu':'Khám YHCT'},{'DichVu_Id':4925,'MaDichVu':'04925','TenDichVu':'Khám YHCT [lần 2]'},{'DichVu_Id':4928,'MaDichVu':'04928','TenDichVu':'Khám YHCT [lần 3]'},{'DichVu_Id':4931,'MaDichVu':'04931','TenDichVu':'Khám YHCT [lần 4]'},{'DichVu_Id':4933,'MaDichVu':'04933','TenDichVu':'Khám YHCT [lần 5]'},{'DichVu_Id':4934,'MaDichVu':'04934','TenDichVu':'Khám YHCT [lần 6 trở lên]'}]},{'PhongKham_Id':910,'MaPhongKham':'KB_KB_CKDY 3','TenPhongKham':'Đông Y 3','dsDichVu':[{'DichVu_Id':20510,'MaDichVu':'239104','TenDichVu':'Khám CK Y Học cổ truyền (BS, ThS, BSCKI)'},{'DichVu_Id':18484,'MaDichVu':'017214 ','TenDichVu':'khám trưởng khoa Y học Cổ Truyền'},{'DichVu_Id':20531,'MaDichVu':'239125','TenDichVu':'Khám TS, BSCKII [Y học cổ truyền]'},{'DichVu_Id':1317,'MaDichVu':'01317','TenDichVu':'Khám YHCT'},{'DichVu_Id':4925,'MaDichVu':'04925','TenDichVu':'Khám YHCT [lần 2]'},{'DichVu_Id':4928,'MaDichVu':'04928','TenDichVu':'Khám YHCT [lần 3]'},{'DichVu_Id':4931,'MaDichVu':'04931','TenDichVu':'Khám YHCT [lần 4]'},{'DichVu_Id':4933,'MaDichVu':'04933','TenDichVu':'Khám YHCT [lần 5]'},{'DichVu_Id':4934,'MaDichVu':'04934','TenDichVu':'Khám YHCT [lần 6 trở lên]'}]}]}]";
            var khoas = JsonConvert.DeserializeObject<List<khoaCls>>(json);
            if (khoas != null && khoas.Count > 0)
            {

                var _dichvu = new List<dichvuCls>();
                var _phongkham = new List<phongkhamCls>();
                foreach (var kh in khoas)
                {
                    var khObj = new KhoaModel() { Id = kh.Khoa_Id, MaKhoa = kh.MaKhoa, TenKhoa = kh.TenKhoa };

                    foreach (var pk in kh.dsPhongKham)
                    {
                        foreach (var dv in pk.dsDichVu)
                        {
                            kh.dichvu_Ids.Add(dv.DichVu_Id);
                            _dichvu.Add(dv);
                        }
                        _phongkham.Add(pk);
                    }
                    kh.dichvu_Ids = kh.dichvu_Ids.Distinct().ToList();
                    khObj.DichVus = _dichvu.Where(x => kh.dichvu_Ids.Contains(x.DichVu_Id))
                        .Select(x => new DichVuModel() { Id = x.DichVu_Id, MaDichVu = x.MaDichVu, TenDichVu = x.TenDichVu })
                        .GroupBy(x => x.Id).Select(x => x.First()).ToList();
                    khoaModels.Add(khObj);
                }

                var dvids = _dichvu.Select(x => x.DichVu_Id).Distinct().ToList();
                _dichvu = _dichvu.Where(x => dvids.Contains(x.DichVu_Id)).ToList();

                foreach (var kh in khoaModels)
                {
                    foreach (var dv in kh.DichVus)
                    {
                        foreach (var pk in _phongkham)
                        {
                            var found = pk.dsDichVu.FirstOrDefault(x => x.DichVu_Id == dv.Id);
                            if (found != null)
                                dv.PhongKhams.Add(new PhongKhamModel() { Id = pk.PhongKham_Id, MaPK = pk.MaPhongKham, TenPK = pk.TenPhongKham });
                        }
                    }
                }

                int index = 31;
                using (var db = new QMSSystemEntities(connectString))
                {
                    try
                    {
                        var query = "";
                        if (clearOldData)
                        {
                            query = @"  
                                    DELETE [Q_CounterDayInfo]  
                                    DBCC CHECKIDENT('Q_CounterDayInfo', RESEED, 0); 
                                    DELETE [Q_ServiceStep]    
                                    DBCC CHECKIDENT('Q_ServiceStep', RESEED, 1);  
                                    DELETE [Q_Service] where id > 30 
                                    DBCC CHECKIDENT('Q_Service', RESEED, 30); 
                                    DELETE [Q_Major] where id > 30 
                                    DBCC CHECKIDENT('Q_Major', RESEED, 30); 
                                    DELETE [Q_User] where id > 30 
                                    DBCC CHECKIDENT('Q_User', RESEED, 30); 
                                    DELETE [Q_Counter] where id > 30 
                                    DBCC CHECKIDENT('Q_Counter', RESEED, 30); 
                                    DELETE [Q_Equipment] where id > 30 
                                    DBCC CHECKIDENT('Q_Equipment', RESEED, 30); 
                                   
                                    ";
                            db.Database.ExecuteSqlCommand(query);
                        }
                        else
                        {
                            var ct = db.Q_Counter.OrderByDescending(x => x.Index).FirstOrDefault();
                            index = ct.Index + 1;
                        }

                        Q_Service service;
                        Q_Major major;
                        Q_ServiceStep serviceStep;
                        Q_User user;
                        Q_Counter counter;
                        Q_Equipment equipment;
                        foreach (var pk in _phongkham)
                        {
                            service = new Q_Service();
                            service.Name = pk.TenPhongKham;
                            service.Code = pk.MaPhongKham;
                            service.StartNumber = 1;
                            service.EndNumber = 1999;
                            service.showBenhVien = true;
                            service.TimeProcess = DateTime.Now;
                            service.AutoEnd = false;
                            service.isKetLuan = false;
                            service.Q_ServiceStep = new List<Q_ServiceStep>();

                            major = new Q_Major();
                            major.Name = service.Name;
                            major.IsShow = false;
                            major.Q_ServiceStep = new List<Q_ServiceStep>();

                            serviceStep = new Q_ServiceStep();
                            serviceStep.Q_Major = major;
                            serviceStep.Q_Service = service;
                            serviceStep.Index = 1;

                            service.Q_ServiceStep.Add(serviceStep);
                            major.Q_ServiceStep.Add(serviceStep);
                            db.Q_Service.Add(service);
                            db.Q_Major.Add(major);

                            user = new Q_User();
                            user.Name = service.Name;
                            user.UserName = service.Code.ToLower();
                            user.Password = service.Code.ToLower();
                            user.Sex = true;
                            user.Counters = "0";
                            db.Q_User.Add(user);


                            equipment = new Q_Equipment();
                            equipment.Name = "counter_" + service.Code;
                            equipment.EquipTypeId = 1;
                            equipment.Code = index;
                            equipment.StatusId = (int)eStatus.HOTAT;

                            counter = new Q_Counter();
                            counter.Name = service.Name;
                            counter.ShortName = service.Code;
                            counter.IsRunning = true;
                            counter.Index = index;
                            counter.Q_Equipment = new List<Q_Equipment>();
                            counter.Q_Equipment.Add(equipment);

                            equipment.Q_Counter = counter;
                            db.Q_Counter.Add(counter);

                            index++;
                        }

                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        rs.IsSuccess = false;
                        return rs;
                        //   MessageBox.Show("Chuyển hóa dữ liệu từ HIS sang QMS thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            rs.IsSuccess = true;
            rs.Data = JsonConvert.SerializeObject(khoaModels);
            return rs;
        }

        /// <summary>
        /// Print ticket
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseBase PrintNewTicket(string connectString,
            int serviceId,
            int serviceNumber,
            int businessId,
            DateTime printTime,
            int? printType,
            TimeSpan? serveTimeAllow,
            string Name,
            string Address,
            int? DOB,
            string MaBenhNhan,
            string MaPhongKham,
            string SttPhongKham,
            string SoXe,
            string maCongViec,
            string maLoaiCongViec,
            int serviceType,
            string phoneNumber
            )
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    Q_DailyRequire rq = null,
                       modelObj = new Q_DailyRequire();
                    int socu = 0;
                    int _serviceNumber = 0;
                    TimeSpan startWork = TimeSpan.Parse("07:00:00");
                    var configs = db.Q_Config.ToList();
                    Q_Config config;
                    if (!printType.HasValue)
                    {
                        var _printType = 1;
                        config = configs.FirstOrDefault(x => x.Code == eConfigCode.PrintType);
                        if (config != null)
                            int.TryParse(config.Value, out _printType);
                        printType = _printType;
                    }

                    config = configs.FirstOrDefault(x => x.Code == eConfigCode.StartWork);
                    if (config != null)
                        TimeSpan.TryParse(config.Value, out startWork);

                    var lastTicket = db.Q_DailyRequire.Where(x => x.ServiceId == serviceId && x.Type == (int)eDailyRequireType.KhamBenh && x.Q_Service.ServiceType == (int)eServiceType.PhongKham).OrderByDescending(x => x.TicketNumber).FirstOrDefault();

                    if (!string.IsNullOrEmpty(maCongViec) && !string.IsNullOrEmpty(maLoaiCongViec))
                    {
                        modelObj = LayGioPhucVuDuKien(db, maCongViec, maLoaiCongViec, modelObj);
                        if (serveTimeAllow.HasValue)
                            modelObj.ServeTimeAllow = modelObj.ServeTimeAllow.Add(serveTimeAllow.Value);
                        serveTimeAllow = modelObj.ServeTimeAllow;
                    }
                    if (printType == (int)ePrintType.TheoTungDichVu)
                    {
                        rq = db.Q_DailyRequire.Where(x => x.ServiceId == serviceId && x.Type == serviceType).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                        var ser = db.Q_Service.FirstOrDefault(x => x.Id == serviceId);
                        if (ser != null)
                        {
                            _serviceNumber = ser.StartNumber;
                            if (!serveTimeAllow.HasValue)
                                serveTimeAllow = ser.TimeProcess.TimeOfDay;
                        }
                    }
                    else if (printType == (int)ePrintType.BatDauChung)
                    {
                        rq = db.Q_DailyRequire.OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                        var cf = db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.StartNumber);
                        if (cf != null)
                            int.TryParse(cf.Value, out _serviceNumber);
                    }
                    socu = ((rq == null) ? serviceNumber : rq.TicketNumber);

                    var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == serviceId).OrderBy(x => x.Index).FirstOrDefault();
                    if (nv == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                    }
                    else
                    {
                        int somoi = 0;
                        var _found = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.ServiceId == serviceId && x.Q_DailyRequire.MaBenhNhan == MaBenhNhan && x.Q_DailyRequire.Type == serviceType);
                        if (_found != null && !string.IsNullOrEmpty(MaBenhNhan))
                        {
                            var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == serviceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                            var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                            var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();

                            var tq = (from x in db.Q_Equipment
                                      where
                                     (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                      !x.IsDeleted &&
                                      !x.Q_Counter.IsDeleted
                                      select new { CounterName = x.Q_Counter.Name, CounterId = x.CounterId }).ToList();
                           
                            somoi = _found.Q_DailyRequire.TicketNumber;
                            rs.IsSuccess = true;
                            rs.Data = socu;
                            rs.Data_1 = _found.MajorId;
                            rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                            rs.Data_2 = String.Join("@", tq.Select(x=>x.CounterName).ToArray()).Replace("@", ("" + System.Environment.NewLine)); 
                            rs.Data_3 = somoi;
                            rs.Data_4 = tq.Select(x => x.CounterId).ToList();
                        }
                        else
                        {
                            somoi = (socu + 1);
                            modelObj.TicketNumber = somoi;
                            modelObj.ServiceId = serviceId;
                            modelObj.BusinessId = null;
                            if (businessId != 0)
                                modelObj.BusinessId = businessId;
                            modelObj.PrintTime = printTime;
                            modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                            modelObj.CustomerName = Name;
                            modelObj.CustomerDOB = DOB;
                            modelObj.CustomerAddress = Address;
                            modelObj.MaBenhNhan = MaBenhNhan;
                            modelObj.MaPhongKham = MaPhongKham;
                            modelObj.STT_PhongKham = SttPhongKham;
                            modelObj.CarNumber = SoXe;
                            modelObj.Type = serviceType;
                            modelObj.PhoneNumber = phoneNumber;

                            modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                            var foundService = db.Q_Service.FirstOrDefault(x => x.Id == serviceId);

                            var detail = new Q_DailyRequire_Detail();
                            detail.Q_DailyRequire = modelObj;
                            detail.MajorId = nv.MajorId;
                            detail.StatusId = (int)eStatus.CHOXL;

                            if (foundService != null && foundService.AutoEnd)
                            {
                                detail.StatusId = (int)eStatus.HOTAT;
                                var now = DateTime.Now;
                                var timeend = foundService.TimeAutoEnd != null ? foundService.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                                detail.ProcessTime = now;
                                detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                            }
                            modelObj.Q_DailyRequire_Detail.Add(detail);
                            db.Q_DailyRequire.Add(modelObj);

                            var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == serviceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                            var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                            var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();
                            var tq = (from x in db.Q_Equipment
                                      where
                                     (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                      !x.IsDeleted &&
                                      !x.Q_Counter.IsDeleted
                                      select new { CounterName = x.Q_Counter.Name, CounterId = x.CounterId }).ToList();
                           
                            db.SaveChanges();
                            rs.IsSuccess = true;
                            rs.Data = socu;
                            rs.Data_1 = detail.MajorId;
                            rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                            rs.Data_2 = String.Join("@", tq.Select(x=>x.CounterName)).Replace("@", ("" + System.Environment.NewLine)) ;
                            rs.Data_3 = somoi;
                            rs.Data_4 = tq.Select(x=>x.CounterId).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "Lỗi Exception", Message = "Lỗi thực thi. " + ex.Message });
            }
            return rs;
        }

        public ResponseBase CapSoKetLuan(string connectString, string MaBenhNhan, DateTime printTime)
        {
            var rs = new ResponseBase();
            var modelObj = new Q_DailyRequire();
            int _serviceStartNumber = 0;
            TimeSpan? serveTimeAllow = new TimeSpan(0, 0, 0),
              tgGoiDK = new TimeSpan(0, 0, 0);
            using (var db = new QMSSystemEntities(connectString))
            {
                var _found = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.MaBenhNhan == MaBenhNhan && x.Q_DailyRequire.Type == (int)eDailyRequireType.KetLuan);
                if (_found != null)
                {
                    //co so ket luan roi 
                    var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == _found.Q_DailyRequire.ServiceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                    var userIds = (from x in db.Q_UserMajor where x.MajorId == _found.MajorId && !x.IsDeleted select x.UserId).ToList();
                    var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();

                    var tq = (from x in db.Q_Equipment
                              where
                             (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                              !x.IsDeleted &&
                              !x.Q_Counter.IsDeleted
                              select x.Q_Counter.Name).ToArray();
                    var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                    rs.IsSuccess = true;
                    rs.Data = _found.Q_DailyRequire.TicketNumber - 1;
                    rs.Data_1 = _found.MajorId;
                    rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                    rs.Data_2 = tqs;
                    rs.Data_3 = _found.Q_DailyRequire.TicketNumber;
                    rs.str1 = _found.Q_DailyRequire.Q_Service.Name;
                }
                else
                {
                    int stt = 1;
                    var pkTicket = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.MaBenhNhan == MaBenhNhan && x.Q_DailyRequire.Type == (int)eDailyRequireType.KhamBenh && x.Q_DailyRequire.Q_Service.ServiceType == (int)eServiceType.PhongKham).OrderBy(x => x.Q_DailyRequire.PrintTime).FirstOrDefault();
                    if (pkTicket != null)
                    {
                        var lastTicket = db.Q_DailyRequire
                        .Where(x => x.ServiceId == pkTicket.Q_DailyRequire.ServiceId &&
                        x.Type == (int)eDailyRequireType.KetLuan &&
                        x.Q_Service.ServiceType == (int)eServiceType.PhongKham)
                        .OrderByDescending(x => x.TicketNumber)
                        .FirstOrDefault();
                        DateTime emptyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime lastTGDKien = emptyDate;
                        if (lastTicket != null)
                        {
                            stt = lastTicket.TicketNumber + 1;
                        }
                        else
                        {
                            var ser = db.Q_Service.FirstOrDefault(x => x.Id == pkTicket.Q_DailyRequire.ServiceId);
                            if (ser != null)
                                stt = ser.StartNumber;
                        }

                        modelObj.TicketNumber = stt;
                        modelObj.ServiceId = pkTicket.Q_DailyRequire.ServiceId;
                        modelObj.BusinessId = null;
                        modelObj.PrintTime = printTime;
                        modelObj.ServeTimeAllow = pkTicket.Q_DailyRequire.ServeTimeAllow;
                        modelObj.CustomerName = pkTicket.Q_DailyRequire.CustomerName;
                        modelObj.CustomerDOB = pkTicket.Q_DailyRequire.CustomerDOB;
                        modelObj.CustomerAddress = pkTicket.Q_DailyRequire.CustomerAddress;
                        modelObj.MaBenhNhan = pkTicket.Q_DailyRequire.MaBenhNhan;
                        modelObj.MaPhongKham = pkTicket.Q_DailyRequire.MaPhongKham;
                        modelObj.STT_PhongKham = "";
                        modelObj.CarNumber = "";
                        modelObj.TGDKien = null;
                        modelObj.Type = (int)eDailyRequireType.KetLuan;
                        modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                        var detail = new Q_DailyRequire_Detail();
                        detail.Q_DailyRequire = modelObj;
                        detail.MajorId = pkTicket.MajorId;
                        detail.StatusId = (int)eStatus.CHOXL;

                        modelObj.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(modelObj);

                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == pkTicket.Q_DailyRequire.ServiceId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == pkTicket.MajorId && !x.IsDeleted select x.UserId).ToList();
                        var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();
                        var tq = (from x in db.Q_Equipment
                                  where
                                 (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                  !x.IsDeleted &&
                                  !x.Q_Counter.IsDeleted
                                  select x.Q_Counter.Name).ToArray();
                        var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Data = (stt - 1);
                        rs.Data_1 = detail.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = modelObj.TicketNumber;
                        rs.str1 = pkTicket.Q_DailyRequire.Q_Service.Name;
                    }
                }
            }
            return rs;
        }


        public ResponseBase CapSoPhongKhamCoHen(string connectString, int dichvuId, string TenBN, string Address, int? DOB, string MaBenhNhan, string MaPhongKham, string SttPhongKham, string SoXe, DateTime printTime, DateTime gioHen)
        {
            var rs = new ResponseBase();
            Q_DailyRequire rq = null,
                     modelObj = new Q_DailyRequire();
            int _serviceStartNumber = 0;
            TimeSpan? serveTimeAllow = new TimeSpan(0, 0, 0);
            TimeSpan? tgGoiDK = new TimeSpan(0, 0, 0);
            using (var db = new QMSSystemEntities(connectString))
            {
                var dichvu = db.Q_Service.FirstOrDefault(x => x.Id == dichvuId);
                if (dichvu != null)
                {
                    _serviceStartNumber = dichvu.StartNumber;
                    serveTimeAllow = dichvu.TimeProcess.TimeOfDay;
                }

                var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == dichvuId).OrderBy(x => x.Index).FirstOrDefault();
                if (nv == null)
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                }
                else
                {
                    var _found = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.ServiceId == dichvuId && x.Q_DailyRequire.MaBenhNhan == MaBenhNhan && x.Q_DailyRequire.Type == (int)eDailyRequireType.KhamBenh);
                    if (_found != null)
                    {
                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                        var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();

                        var tq = (from x in db.Q_Equipment
                                  where
                                 (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                  !x.IsDeleted &&
                                  !x.Q_Counter.IsDeleted
                                  select x.Q_Counter.Name).ToArray();
                        var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                        rs.IsSuccess = true;
                        rs.Data = _found.Q_DailyRequire.TicketNumber - 1;
                        rs.Data_1 = _found.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = _found.Q_DailyRequire.TicketNumber;
                    }
                    else
                    {
                        int stt = 0;
                        var lastTicket = db.Q_DailyRequire
                            .Where(x => x.ServiceId == dichvuId &&
                            x.Type == (int)eDailyRequireType.KhamBenh &&
                            x.Q_Service.ServiceType == (int)eServiceType.PhongKham &&
                            x.TGDKien.HasValue && x.TGDKien <= gioHen
                            )
                            .OrderByDescending(x => x.TicketNumber)
                            .FirstOrDefault();
                        DateTime emptyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        if (lastTicket != null)
                        {
                            stt = lastTicket.TicketNumber + 1;
                            emptyDate = lastTicket.TGDKien.Value.AddSeconds(dichvu.TimeProcess.TimeOfDay.TotalSeconds);
                        }
                        else
                        {
                            stt = _serviceStartNumber;
                            TimeSpan startWork = TimeSpan.Parse("07:00:00");
                            var configs = db.Q_Config.ToList();
                            Q_Config config;
                            config = configs.FirstOrDefault(x => x.Code == eConfigCode.StartWork);
                            if (config != null)
                                TimeSpan.TryParse(config.Value, out startWork);
                            emptyDate = emptyDate.AddSeconds(startWork.TotalSeconds);
                        }

                        modelObj.TicketNumber = stt;
                        modelObj.ServiceId = dichvuId;
                        modelObj.BusinessId = null;
                        modelObj.PrintTime = printTime;
                        modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                        modelObj.CustomerName = TenBN;
                        modelObj.CustomerDOB = DOB;
                        modelObj.CustomerAddress = Address;
                        modelObj.MaBenhNhan = MaBenhNhan;
                        modelObj.MaPhongKham = MaPhongKham;
                        modelObj.STT_PhongKham = SttPhongKham;
                        modelObj.CarNumber = "";// SoXe;
                        modelObj.TGDKien = emptyDate;
                        modelObj.Type = (int)eDailyRequireType.KhamBenh;
                        modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                        var detail = new Q_DailyRequire_Detail();
                        detail.Q_DailyRequire = modelObj;
                        detail.MajorId = nv.MajorId;
                        detail.StatusId = (int)eStatus.CHOXL;

                        if (dichvu != null && dichvu.AutoEnd)
                        {
                            detail.StatusId = (int)eStatus.HOTAT;
                            var now = DateTime.Now;
                            var timeend = dichvu.TimeAutoEnd != null ? dichvu.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                            detail.ProcessTime = now;
                            detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                        }
                        modelObj.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(modelObj);

                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                        var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();
                        var tq = (from x in db.Q_Equipment
                                  where
                                 (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                  !x.IsDeleted &&
                                  !x.Q_Counter.IsDeleted
                                  select x.Q_Counter.Name).ToArray();
                        var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Data = (stt - 1);
                        rs.Data_1 = detail.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = stt;
                    }
                }
            }
            return rs;
        }

        public ResponseBase CapSoPhongKhamKhongHen(string connectString, int dichvuId, string TenBN, string Address, int? DOB, string MaBenhNhan, string MaPhongKham, string SttPhongKham, string SoXe, DateTime printTime, List<DateTime> gioHens)
        {
            var rs = new ResponseBase();
            var modelObj = new Q_DailyRequire();
            int _serviceStartNumber = 0;
            TimeSpan? serveTimeAllow = new TimeSpan(0, 0, 0),
              tgGoiDK = new TimeSpan(0, 0, 0);
            using (var db = new QMSSystemEntities(connectString))
            {
                var dichvu = db.Q_Service.FirstOrDefault(x => x.Id == dichvuId);
                if (dichvu != null)
                {
                    _serviceStartNumber = dichvu.StartNumber;
                    serveTimeAllow = dichvu.TimeProcess.TimeOfDay;
                }

                var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == dichvuId).OrderBy(x => x.Index).FirstOrDefault();
                if (nv == null)
                {
                    rs.IsSuccess = false;
                    rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                }
                else
                {
                    var _found = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.ServiceId == dichvuId && x.Q_DailyRequire.MaBenhNhan == MaBenhNhan && x.Q_DailyRequire.Type == (int)eDailyRequireType.KhamBenh);
                    if (_found != null)
                    {
                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                        var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();

                        var tq = (from x in db.Q_Equipment
                                  where
                                 (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                  !x.IsDeleted &&
                                  !x.Q_Counter.IsDeleted
                                  select x.Q_Counter.Name).ToArray();
                        var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                        rs.IsSuccess = true;
                        rs.Data = _found.Q_DailyRequire.TicketNumber - 1;
                        rs.Data_1 = _found.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = _found.Q_DailyRequire.TicketNumber;
                    }
                    else
                    {
                        int stt = 0;
                        var lastTicket = db.Q_DailyRequire
                            .Where(x => x.ServiceId == dichvuId &&
                            x.Type == (int)eDailyRequireType.KhamBenh &&
                            x.Q_Service.ServiceType == (int)eServiceType.PhongKham)
                            .OrderByDescending(x => x.TicketNumber)
                            .FirstOrDefault();
                        DateTime emptyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime lastTGDKien = emptyDate;
                        if (lastTicket != null)
                        {
                            stt = lastTicket.TicketNumber + 1;
                            emptyDate = lastTicket.TGDKien.Value.AddSeconds(dichvu.TimeProcess.TimeOfDay.TotalSeconds);
                            lastTGDKien = lastTicket.TGDKien.Value;
                        }
                        else
                        {
                            stt = _serviceStartNumber;
                            TimeSpan startWork = TimeSpan.Parse("07:00:00");
                            var configs = db.Q_Config.ToList();
                            Q_Config config;
                            config = configs.FirstOrDefault(x => x.Code == eConfigCode.StartWork);
                            if (config != null)
                                TimeSpan.TryParse(config.Value, out startWork);
                            emptyDate = emptyDate.AddSeconds(startWork.TotalSeconds);
                            lastTGDKien = emptyDate;
                        }

                        if (gioHens.Count > 0)
                        {
                            repeat:
                            var foundHen = gioHens.FirstOrDefault(x => x >= lastTGDKien && x <= emptyDate);
                            if (foundHen != null && foundHen.Year == DateTime.Now.Year)
                            {
                                stt++;
                                lastTGDKien = emptyDate;
                                emptyDate = emptyDate.AddSeconds(dichvu.TimeProcess.TimeOfDay.TotalSeconds);
                                goto repeat;
                            }
                        }

                        modelObj.TicketNumber = stt;
                        modelObj.ServiceId = dichvuId;
                        modelObj.BusinessId = null;
                        modelObj.PrintTime = printTime;
                        modelObj.ServeTimeAllow = serveTimeAllow ?? new TimeSpan(0, 0, 0);
                        modelObj.CustomerName = TenBN;
                        modelObj.CustomerDOB = DOB;
                        modelObj.CustomerAddress = Address;
                        modelObj.MaBenhNhan = MaBenhNhan;
                        modelObj.MaPhongKham = MaPhongKham;
                        modelObj.STT_PhongKham = "";
                        modelObj.CarNumber = "";// SoXe;
                        modelObj.TGDKien = emptyDate;
                        modelObj.Type = (int)eDailyRequireType.KhamBenh;
                        modelObj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                        var detail = new Q_DailyRequire_Detail();
                        detail.Q_DailyRequire = modelObj;
                        detail.MajorId = nv.MajorId;
                        detail.StatusId = (int)eStatus.CHOXL;

                        if (dichvu != null && dichvu.AutoEnd)
                        {
                            detail.StatusId = (int)eStatus.HOTAT;
                            var now = DateTime.Now;
                            var timeend = dichvu.TimeAutoEnd != null ? dichvu.TimeAutoEnd.Value.TimeOfDay : new TimeSpan(0, 10, 00);
                            detail.ProcessTime = now;
                            detail.EndProcessTime = now.AddMinutes(timeend.TotalMinutes);
                        }
                        modelObj.Q_DailyRequire_Detail.Add(detail);
                        db.Q_DailyRequire.Add(modelObj);

                        var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == dichvuId && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();

                        var userIds = (from x in db.Q_UserMajor where x.MajorId == nv.MajorId && !x.IsDeleted select x.UserId).ToList();
                        var equipCodes = (from x in db.Q_Login where x.StatusId == (int)eStatus.LOGIN && userIds.Contains(x.UserId) && !x.Q_User.IsDeleted select x.EquipCode).ToList();
                        var tq = (from x in db.Q_Equipment
                                  where
                                 (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)) &&
                                  !x.IsDeleted &&
                                  !x.Q_Counter.IsDeleted
                                  select x.Q_Counter.Name).ToArray();
                        var tqs = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));

                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Data = (stt - 1);
                        rs.Data_1 = detail.MajorId;
                        rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                        rs.Data_2 = tqs;
                        rs.Data_3 = stt;
                    }
                }
            }
            return rs;
        }

        private Q_DailyRequire LayGioPhucVuDuKien(QMSSystemEntities db, string maCongViec, string maLoaiCongViec, Q_DailyRequire model)
        {
            DateTime processTime = new DateTime(2020, 1, 1, 00, 00, 00);
            string[] codes = maCongViec.Split(',').Select(x => x.Trim().ToUpper()).ToArray();
            if (codes != null && codes.Length > 0)
            {
                var a = (from x in db.Q_WorkDetail
                         where !x.IsDeleted
                         select new ModelSelectItem() { Code = x.Q_Works.Code, Name = x.Q_WorkType.Code }).ToList();
                var congviecs = (from x in db.Q_WorkDetail
                                 where !x.IsDeleted &&
                                 codes.Contains(x.Q_Works.Code.Trim().ToUpper()) &&
                                 maLoaiCongViec.Trim().ToUpper().Equals(x.Q_WorkType.Code.Trim().ToUpper())
                                 select new { TimeProcess = x.TimeProcess, Id = x.Id }).ToArray();
                model.Q_DailyRequire_WorkDetail = new List<Q_DailyRequire_WorkDetail>();
                for (int i = 0; i < congviecs.Count(); i++)
                {
                    processTime = processTime.AddSeconds(congviecs[i].TimeProcess.TimeOfDay.TotalSeconds);
                    model.Q_DailyRequire_WorkDetail.Add(new Q_DailyRequire_WorkDetail()
                    {
                        Q_DailyRequire = model,
                        WorkDetailId = congviecs[i].Id
                    });
                }
            }
            model.ServeTimeAllow = processTime.TimeOfDay;
            return model;
        }

        public int QuaLuot(string connectString, int ticket, string maPK, string maBN, int serviceType)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var equipInfo = db.Q_Equipment.Where(x => x.Q_Counter.ShortName == maPK).Select(x => new { EquipCode = x.Code, CounterId = x.CounterId }).FirstOrDefault();
                    if (equipInfo != null)
                    {
                        var loginInfo = db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN && x.EquipCode == equipInfo.EquipCode).OrderByDescending(x => x.Id).Select(x => new { UserId = x.UserId, ECode = x.EquipCode }).FirstOrDefault();
                        if (loginInfo != null)
                        {
                            var objs = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.TicketNumber == ticket &&
                                                                            x.Q_DailyRequire.MaBenhNhan == maBN &&
                                                                            x.UserId == loginInfo.UserId &&
                                                                            x.EquipCode == loginInfo.ECode &&
                                                                            x.StatusId == (int)eStatus.DAGXL &&
                                                                            x.Q_DailyRequire.Type == serviceType);
                            if (objs != null && objs.Count() > 0)
                            {
                                foreach (var item in objs)
                                {
                                    item.StatusId = (int)eStatus.QUALUOT;
                                    item.UserId = null;
                                    item.ProcessTime = null;
                                    item.EquipCode = null;
                                }
                                switch (serviceType)
                                {
                                    case (int)eDailyRequireType.KhamBenh:
                                        db.Database.ExecuteSqlCommand("update Q_Counter set LastCall =0, CurrentNumber=0 where Id =" + equipInfo.CounterId);
                                        break;
                                    case (int)eDailyRequireType.KetLuan:
                                        db.Database.ExecuteSqlCommand("update Q_Counter set LastCallKetLuan =0  where Id =" + equipInfo.CounterId);
                                        break;
                                }
                                db.SaveChanges();
                                return ticket;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return 0;
        }

        public WebBVHuuNghiModel GetCounterDayInfo_Web(string connectString, int userId, int equipCode, int[] counterIds, bool isPhongKham)
        {
            var webInfo = new WebBVHuuNghiModel();
            using (var db = new QMSSystemEntities(connectString))
            {
                var user = db.Q_Login.FirstOrDefault(x => x.StatusId == (int)eStatus.LOGIN && x.UserId == userId && x.EquipCode == equipCode);
                if (user != null)
                {
                    var equipment = db.Q_Equipment.FirstOrDefault(x => x.Code == user.EquipCode);
                    var majorIds = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_Major.IsDeleted && !x.Q_User.IsDeleted && x.UserId == userId).Select(x => x.MajorId).ToList();
                    webInfo.STTDangKham = equipment.Q_Counter.LastCall.ToString();
                    webInfo.STTDangKetLuan = equipment.Q_Counter.LastCallKetLuan.ToString();

                    var daysKB = db.Q_DailyRequire_Detail.Where(x => majorIds.Contains(x.MajorId) && x.StatusId != (int)eStatus.HOTAT && x.Q_DailyRequire.Type == (int)eDailyRequireType.KhamBenh)
                        .Take(10)
                        .Select(x => new
                        {
                            STT = x.Q_DailyRequire.TicketNumber,
                            DOB = x.Q_DailyRequire.CustomerDOB,
                            TenBN = x.Q_DailyRequire.CustomerName,
                            StatusId = x.StatusId,
                            Type = x.Q_DailyRequire.Type,
                            STT_PhongKham = x.Q_DailyRequire.STT_PhongKham
                        })
                        .ToList();
                    if (daysKB.Count > 0)
                    {
                        int tuoi = 0;
                        foreach (var item in daysKB)
                        {
                            tuoi = (item.DOB == null ? 0 : item.DOB.Value);
                            if (tuoi > 0)
                                tuoi = DateTime.Now.Year - tuoi;

                            if (item.StatusId == (int)eStatus.CHOXL)
                            {
                                webInfo.DSChoKham.Add(new ModelSelectItem()
                                {
                                    Name = (string.IsNullOrEmpty(item.STT_PhongKham) ? item.STT.ToString() : item.STT_PhongKham),
                                    Code = (string.IsNullOrEmpty(item.TenBN) ? " " : item.TenBN),
                                    Data = tuoi,
                                });
                            }
                            else if (item.StatusId == (int)eStatus.QUALUOT)
                            {
                                webInfo.DSQuaLuotKham.Add(new ModelSelectItem()
                                {
                                    Name = (string.IsNullOrEmpty(item.STT_PhongKham) ? item.STT.ToString() : item.STT_PhongKham),
                                    Code = (string.IsNullOrEmpty(item.TenBN) ? " " : item.TenBN),
                                    Data = tuoi,
                                });
                            }
                        }
                    }

                    if (isPhongKham)
                    {
                        var daysKL = db.Q_DailyRequire_Detail.Where(x => majorIds.Contains(x.MajorId) && x.StatusId != (int)eStatus.HOTAT && x.Q_DailyRequire.Type == (int)eDailyRequireType.KetLuan)
                                                .Take(10)
                                                .Select(x => new
                                                {
                                                    STT = x.Q_DailyRequire.TicketNumber,
                                                    DOB = x.Q_DailyRequire.CustomerDOB,
                                                    TenBN = x.Q_DailyRequire.CustomerName,
                                                    StatusId = x.StatusId,
                                                    Type = x.Q_DailyRequire.Type,
                                                    STT_PhongKham = x.Q_DailyRequire.STT_PhongKham
                                                })
                                                .ToList();
                        if (daysKL.Count > 0)
                        {
                            int tuoi = 0;
                            foreach (var item in daysKL)
                            {
                                tuoi = (item.DOB == null ? 0 : item.DOB.Value);
                                if (tuoi > 0)
                                    tuoi = DateTime.Now.Year - tuoi;

                                if (item.StatusId == (int)eStatus.CHOXL)
                                {
                                    webInfo.DSChoKL.Add(new ModelSelectItem()
                                    {
                                        Name = (string.IsNullOrEmpty(item.STT_PhongKham) ? item.STT.ToString() : item.STT_PhongKham),
                                        Code = (string.IsNullOrEmpty(item.TenBN) ? " " : item.TenBN),
                                        Data = tuoi,
                                    });
                                }
                                else if (item.StatusId == (int)eStatus.QUALUOT)
                                {
                                    webInfo.DSQuaLuotKL.Add(new ModelSelectItem()
                                    {
                                        Name = (string.IsNullOrEmpty(item.STT_PhongKham) ? item.STT.ToString() : item.STT_PhongKham),
                                        Code = (string.IsNullOrEmpty(item.TenBN) ? " " : item.TenBN),
                                        Data = tuoi,
                                    });
                                }
                            }
                        }

                    }
                }
                webInfo.Sounds = BLLTVReadSound.Instance.Gets(connectString, counterIds, userId);
            }
            return webInfo;
        }

        /// <summary>
        ///  Counter Soft
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="equipCode"></param>
        /// <param name="ticket"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseBaseModel CallAny(string connectString, int userId, int equipCode, int ticket, DateTime date, int serviceType)
        {
            ResponseBaseModel res = new ResponseBaseModel();
            bool hasChange = false;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var oldTickets = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.TicketNumber != ticket && x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL && x.Q_DailyRequire.Type == serviceType);
                    if (oldTickets != null && oldTickets.Count() > 0)
                    {
                        foreach (var item in oldTickets)
                        {
                            item.StatusId = (int)eStatus.HOTAT;
                            item.EndProcessTime = DateTime.Now;
                        }
                        hasChange = true;
                    }
                    var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                    var userMajorIds = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted && x.UserId == userId).OrderBy(x => x.Index).Select(x => x.MajorId).ToList();
                    if (userMajorIds.Count > 0)
                    {
                        var check = db.Q_DailyRequire_Detail.FirstOrDefault(x => userMajorIds.Contains(x.MajorId) && x.Q_DailyRequire.TicketNumber == ticket && (x.StatusId == (int)eStatus.CHOXL || x.StatusId == (int)eStatus.DAGXL || x.StatusId == (int)eStatus.QUALUOT) && x.Q_DailyRequire.Type == serviceType);
                        if (check != null)
                        {
                            if (check.StatusId != (int)eStatus.DAGXL)
                            {
                                check.UserId = userId;
                                check.EquipCode = equipCode;
                                check.StatusId = (int)eStatus.DAGXL;
                                check.ProcessTime = DateTime.Now;
                            }
                            hasChange = true;
                            res.Data_3 = new TicketInfo()
                            {
                                RequireDetailId = check.Id,
                                RequireId = check.DailyRequireId,
                                StartTime = check.ProcessTime.Value.TimeOfDay,
                                TimeServeAllow = check.Q_DailyRequire.ServeTimeAllow,
                                TicketNumber = check.Q_DailyRequire.TicketNumber,
                                CounterId = equip.CounterId,
                                PrintTime = check.Q_DailyRequire.PrintTime,
                                EquipCode = equipCode,
                                Note = (check.Q_DailyRequire.MaPhongKham + "," + check.Q_DailyRequire.MaBenhNhan + "," + check.Q_DailyRequire.CustomerName + "," + check.Q_DailyRequire.CustomerDOB)
                            };
                            switch (serviceType)
                            {
                                case (int)eDailyRequireType.KhamBenh:
                                    db.Database.ExecuteSqlCommand("update Q_Counter set LastCall =" + check.Q_DailyRequire.TicketNumber + ", CurrentNumber=" + check.Q_DailyRequire.TicketNumber + " where Id =" + equip.CounterId);
                                    break;
                                case (int)eDailyRequireType.KetLuan:
                                    db.Database.ExecuteSqlCommand("update Q_Counter set LastCallKetLuan =" + check.Q_DailyRequire.TicketNumber + "  where Id =" + equip.CounterId);
                                    break;
                            }
                        }
                        else
                        {
                            var rq = db.Q_DailyRequire.FirstOrDefault(x => x.TicketNumber == ticket && x.Type == serviceType);
                            if (rq != null)
                            {
                                var newobj = new Q_DailyRequire_Detail();
                                newobj.DailyRequireId = rq.Id;
                                newobj.MajorId = userMajorIds[0];
                                newobj.UserId = userId;
                                newobj.EquipCode = equipCode;
                                newobj.StatusId = (int)eStatus.DAGXL;
                                newobj.ProcessTime = DateTime.Now;
                                db.Q_DailyRequire_Detail.Add(newobj);
                                hasChange = true;

                                res.Data_3 = new TicketInfo()
                                {
                                    RequireDetailId = newobj.Id,
                                    RequireId = newobj.DailyRequireId,
                                    StartTime = newobj.ProcessTime.Value.TimeOfDay,
                                    TimeServeAllow = rq.ServeTimeAllow,
                                    TicketNumber = rq.TicketNumber,
                                    CounterId = equip.CounterId,
                                    PrintTime = rq.PrintTime,
                                    EquipCode = equipCode,
                                    Note = (check.Q_DailyRequire.MaPhongKham + "," + check.Q_DailyRequire.MaBenhNhan + "," + check.Q_DailyRequire.CustomerName + "," + check.Q_DailyRequire.CustomerDOB)
                                };

                                switch (serviceType)
                                {
                                    case (int)eDailyRequireType.KhamBenh:
                                        db.Database.ExecuteSqlCommand("update Q_Counter set LastCall =" + rq.TicketNumber + ", CurrentNumber=" + rq.TicketNumber + " where Id =" + equip.CounterId);
                                        break;
                                    case (int)eDailyRequireType.KetLuan:
                                        db.Database.ExecuteSqlCommand("update Q_Counter set LastCallKetLuan =" + rq.TicketNumber + "  where Id =" + equip.CounterId);
                                        break;
                                }
                            }
                            else
                            {
                                res.IsSuccess = false;
                                res.sms = "Không tìm thấy số phiếu " + ticket;
                                res.Title = "Lỗi";
                            }
                        }
                        if (hasChange)
                        {
                            db.SaveChanges();
                            res.IsSuccess = true;
                        }
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.sms = "Nhân viên chưa được phân nghiệp vụ";
                        res.Title = "Lỗi";
                    }
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.sms = "Lỗi thực thi dữ liệu";
                res.Title = "Lỗi";
            }
            return res;
        }

        public class WebBVHuuNghiModel
        {
            public string STTDangKham { get; set; }
            public string STTDangKetLuan { get; set; }
            public List<ModelSelectItem> DSChoKham { get; set; }
            public List<ModelSelectItem> DSQuaLuotKham { get; set; }
            public List<ModelSelectItem> DSChoKL { get; set; }
            public List<ModelSelectItem> DSQuaLuotKL { get; set; }
            public List<string> Sounds { get; set; }
            public List<ViewDetailModel> Details { get; set; }
            public WebBVHuuNghiModel()
            {
                DSChoKham = new List<ModelSelectItem>();
                DSChoKL = new List<ModelSelectItem>();
                DSQuaLuotKham = new List<ModelSelectItem>();
                DSQuaLuotKL = new List<ModelSelectItem>();
                Details = new List<ViewDetailModel>();
                STTDangKham = "0";
                STTDangKetLuan = "0";
                Sounds = new List<string>();
            }
        }

        public ResponseBaseModel KiemTraDKKham(string connectString, string maBN)
        {
            var rs = new ResponseBaseModel();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var oldTickets = db.Q_DailyRequire.Where(x => x.MaBenhNhan == maBN && x.Type == (int)eDailyRequireType.KhamBenh && x.Q_Service.ServiceType == (int)eServiceType.PhongKham).OrderBy(x => x.PrintTime).FirstOrDefault();
                    if (oldTickets != null)
                    {
                        rs.IsSuccess = true;
                        rs.Data = oldTickets.ServiceId;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return rs;
        }

        public DataSet DSCho(SqlConnection sqlConnection, string maPK, int dailyRequireType)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            //var list = new List<KetLuanModel>();
            var dataset = new DataSet();
            try
            {
                EquipmentModel equipInfo = findEquipInfo(sqlConnection, maPK);
                if (equipInfo != null)
                {
                    Login loginInfo = findLoginInfo(sqlConnection, equipInfo.Id);
                    if (loginInfo != null)
                    {
                        var majorIds = findUserMajorIds(sqlConnection, loginInfo.UserId);

                        var query = string.Format(@"select dr.TicketNumber as STT,  dr.MaBenhNhan as MaBN, dr.CustomerName as TenBN, dr.CustomerDOB as NamSinh, dr.CustomerAddress as DiaChi , CASE WHEN  dd.StatusId ={0} THEN N'Đang chờ' WHEN  dd.StatusId ={1} THEN N'Qua lượt' END AS TrangThai , dr.MaPhongKham as MaPK    
                            from Q_DailyRequire dr ,Q_Service s  , Q_DailyRequire_Detail dd  
                            where dd.DailyRequireId =dr.Id and s.IsDeleted=0 and dr.[Type] ={2} and s.Id = dr.ServiceId and dd.MajorId in ({3}) and ( dd.StatusId={4} or dd.StatusId={5} ) 
                            order by dr.TicketNumber", (int)eStatus.CHOXL, (int)eStatus.QUALUOT, dailyRequireType, string.Join(",", majorIds), (int)eStatus.CHOXL, (int)eStatus.QUALUOT);
                        var adap = new SqlDataAdapter(query, sqlConnection);
                        adap.Fill(dataset, "TableDSKetLuan");
                        //var dt = new DataTable();
                        //adap.Fill(dt);
                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow row in dt.Rows)
                        //    {
                        //        list.Add(new KetLuanModel()
                        //        {
                        //            NamSinh = getIntValue(row["CustomerDOB"]),
                        //            STT = getIntValue(row["TicketNumber"]),
                        //            MaBN = getStringValue(row["MaBenhNhan"]),
                        //            TenBN = getStringValue(row["CustomerName"]),
                        //            DiaChi = getStringValue(row["CustomerAddress"])
                        //        });
                        //    }
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dataset;
        }

        public List<BNChoModel> DSBNCho(SqlConnection sqlConnection, string maPK, int dailyRequireType)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            var list = new List<BNChoModel>();
            var dataset = new DataSet();
            try
            {
                EquipmentModel equipInfo = findEquipInfo(sqlConnection, maPK);
                if (equipInfo != null)
                {
                    Login loginInfo = findLoginInfo(sqlConnection, equipInfo.Id);
                    if (loginInfo != null)
                    {
                        var majorIds = findUserMajorIds(sqlConnection, loginInfo.UserId);
                        var query = string.Format(@"select dr.TicketNumber as STT,  dr.MaBenhNhan as MaBN, dr.CustomerName as TenBN, dr.CustomerDOB as NamSinh, dr.CustomerAddress as DiaChi , CASE WHEN  dd.StatusId ={0} THEN N'Đang chờ' WHEN  dd.StatusId ={1} THEN N'Qua lượt' END AS TrangThai    
                            from Q_DailyRequire dr ,Q_Service s  , Q_DailyRequire_Detail dd  
                            where dd.DailyRequireId =dr.Id and s.IsDeleted=0 and dr.[Type] ={2} and s.Id = dr.ServiceId and dd.MajorId in ({3}) and ( dd.StatusId={4} or dd.StatusId={5} ) 
                            order by dr.TicketNumber", (int)eStatus.CHOXL, (int)eStatus.QUALUOT, dailyRequireType, string.Join(",", majorIds), (int)eStatus.CHOXL, (int)eStatus.QUALUOT);
                        var adap = new SqlDataAdapter(query, sqlConnection);
                        adap.Fill(dataset, "TableDSKetLuan");
                        var dt = new DataTable();
                        adap.Fill(dt);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                list.Add(new BNChoModel()
                                {
                                    NamSinh = getIntValue(row["NamSinh"]),
                                    STT = getIntValue(row["STT"]),
                                    MaBN = getStringValue(row["MaBN"]),
                                    TenBN = getStringValue(row["TenBN"]),
                                    DiaChi = getStringValue(row["DiaChi"]),
                                    TrangThai = getStringValue(row["TrangThai"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public EquipmentModel findEquipInfo(SqlConnection sqlConnection, string counterShortname)
        {
            EquipmentModel item = null;
            string query = @"select c.ShortName, e.Id, e.Code, e.CounterId
                                from Q_Equipment e, Q_Counter c
                                where e.CounterId = c.Id and ShortName =N'" + counterShortname + "' and EquipTypeId =" + (int)eEquipType.Counter;
            var da = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                item = new EquipmentModel()
                {
                    Id = getIntValue(dataTable.Rows[0]["Code"]),
                    CounterId = getIntValue(dataTable.Rows[0]["CounterId"])
                };
            }
            return item;
        }

        public Login findLoginInfo(SqlConnection sqlConnection, int equipCode)
        {
            Login item = null;
            string query = @"select top 1 UserId,EquipCode from Q_Login where EquipCode =" + equipCode + " and StatusId =" + (int)eStatus.LOGIN + " order by id desc";
            var da = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                item = new Login()
                {
                    UserId = getIntValue(dataTable.Rows[0]["UserId"]),
                    EquipCode = getIntValue(dataTable.Rows[0]["EquipCode"])
                };
            }
            return item;
        }

        public List<int> findUserMajorIds(SqlConnection sqlConnection, int userId)
        {
            List<int> majorIds = new List<int>();
            string query = @"select um.MajorId 
                                from Q_UserMajor um, Q_User u, Q_Major m 
                                where u.IsDeleted = 0 and um.IsDeleted = 0 and m.IsDeleted = 0 and um.MajorId = m.Id and um.UserId = u.Id and UM.UserId =" + userId;
            var da = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    int intType = 0;
                    int.TryParse(row["MajorId"].ToString(), out intType);
                    majorIds.Add(intType);
                }
            }
            return majorIds;
        }

        private int getIntValue(object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return Convert.ToInt32(value);
            return 0;
        }

        private string getStringValue(object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return value.ToString();
            return "";
        }

        public ModelSelectItem SLPhieuCuaDichVu(string connectString, int serviceId)
        {
            var rs = new ModelSelectItem();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var ticket = db.Q_DailyRequire.Where(x => x.ServiceId == serviceId && x.Type == (int)eDailyRequireType.KhamBenh && x.Q_Service.ServiceType == (int)eServiceType.PhongKham).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                    if (ticket != null)
                    {
                        rs.Id = ticket.TicketNumber;
                        rs.Data = ticket.ServiceId;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return rs;
        }

        public ResponseBaseModel GetCurrentTicket(string connectString, int userId, int equipCode)
        {
            ResponseBaseModel res = new ResponseBaseModel();
            res.IsSuccess = false;
            int ticket = 0;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var last = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.UserId == userId && x.EquipCode == equipCode && x.StatusId == (int)eStatus.DAGXL);
                    if (last != null)
                    {
                        try
                        {
                            ticket = int.Parse(last.Q_DailyRequire.STT_PhongKham);
                        }
                        catch (Exception)
                        {
                            ticket = last.Q_DailyRequire.TicketNumber;
                        }

                        var equip = db.Q_Equipment.FirstOrDefault(x => !x.IsDeleted && x.Code == equipCode);
                        res.IsSuccess = true;
                        res.Data = ticket;
                        res.Data_3 = new TicketInfo()
                        {
                            RequireDetailId = last.Id,
                            RequireId = last.DailyRequireId,
                            StartTime = last.ProcessTime.Value.TimeOfDay,
                            TimeServeAllow = last.Q_DailyRequire.ServeTimeAllow,
                            TicketNumber = ticket,
                            QMSNumber = last.Q_DailyRequire.TicketNumber,
                            ThirdNumber = last.Q_DailyRequire.STT_PhongKham,
                            CounterId = (equip != null ? equip.CounterId : 0),
                            PrintTime = last.Q_DailyRequire.PrintTime,
                            Note = (last.Q_DailyRequire.MaPhongKham + "," + last.Q_DailyRequire.MaBenhNhan + "," + last.Q_DailyRequire.CustomerName + "," + last.Q_DailyRequire.CustomerDOB),
                            EquipCode = equipCode,
                        };
                    }
                }
            }
            catch (Exception)
            { }
            return res;
        }

        public List<PhongKhamModel> GetCurrentPKInfo(string connectString, List<string> mapks)
        {
            var list = new List<PhongKhamModel>();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var datas = db.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.HOTAT && mapks.Contains(x.Q_DailyRequire.MaPhongKham)).Select(x => new { serId = x.Q_DailyRequire.ServiceId, maPK = x.Q_DailyRequire.MaPhongKham }).ToList();
                    var drs = db.Q_DailyRequire.Where(x => mapks.Contains(x.MaPhongKham)).Select(x => x.MaPhongKham).ToList();
                    var sers = db.Q_Service.Where(x => mapks.Contains(x.Code)).Select(x => new { serId = x.Id, maPK = x.Code }).ToList();
                    for (int i = 0; i < mapks.Count; i++)
                    {
                        var _pk = mapks[i];
                        var _fSer = sers.FirstOrDefault(x => x.maPK == _pk);
                        list.Add(new PhongKhamModel()
                        {
                            Id = _fSer != null ? _fSer.serId : 0,
                            MaPK = mapks[i],
                            TenPK = datas.Where(x => x.maPK == _pk).Count().ToString() + "/" + drs.Where(x => x == _pk).Count().ToString()
                        });
                    }
                }
            }
            catch (Exception)
            { }
            return list;
        }

        public WebBVHuuNghiModel GetWeb_VP_PT(string connectString, int[] counterIds, int[] services, int userId)
        {
            using (var db_ = new QMSSystemEntities(connectString))
            {
                var webInfo = new WebBVHuuNghiModel();
                try
                {
                    var daysKB = db_.Q_DailyRequire_Detail.Where(x => services.Contains(x.Q_DailyRequire.ServiceId) && x.StatusId != (int)eStatus.HOTAT && x.Q_DailyRequire.Type == (int)eDailyRequireType.KhamBenh)
                        .Take(10)
                        .Select(x => new
                        {
                            STT = x.Q_DailyRequire.TicketNumber,
                            DOB = x.Q_DailyRequire.CustomerDOB,
                            TenBN = x.Q_DailyRequire.CustomerName,
                            StatusId = x.StatusId,
                            Type = x.Q_DailyRequire.Type,
                            STT_PhongKham = x.Q_DailyRequire.STT_PhongKham
                        })
                        .ToList();
                    if (daysKB.Count > 0)
                    {
                        int tuoi = 0;
                        foreach (var item in daysKB)
                        {
                            tuoi = (item.DOB == null ? 0 : item.DOB.Value);
                            if (tuoi > 0)
                                tuoi = DateTime.Now.Year - tuoi;

                            if (item.StatusId == (int)eStatus.CHOXL)
                            {
                                webInfo.DSChoKham.Add(new ModelSelectItem()
                                {
                                    Name = (string.IsNullOrEmpty(item.STT_PhongKham) ? item.STT.ToString() : item.STT_PhongKham),
                                    Code = (string.IsNullOrEmpty(item.TenBN) ? " " : item.TenBN),
                                    Data = tuoi,
                                });
                            }
                            else if (item.StatusId == (int)eStatus.QUALUOT)
                            {
                                webInfo.DSQuaLuotKham.Add(new ModelSelectItem()
                                {
                                    Name = (string.IsNullOrEmpty(item.STT_PhongKham) ? item.STT.ToString() : item.STT_PhongKham),
                                    Code = (string.IsNullOrEmpty(item.TenBN) ? " " : item.TenBN),
                                    Data = tuoi,
                                });
                            }
                        }
                    }

                    webInfo.Sounds = BLLTVReadSound.Instance.Gets(connectString, counterIds, userId);


                    var counters = db_.Q_Equipment.Where(x => !x.IsDeleted && !x.Q_Counter.IsDeleted && counterIds.Contains(x.CounterId) && x.EquipTypeId == (int)eEquipType.Counter)
                        .Select(x => new ViewDetailModel()
                        {
                            STT = (!x.Q_Counter.IsRunning ? 1 : 0),
                            CarNumber = "0",
                            TableId = x.Q_Counter.Id,
                            TableName = x.Q_Counter.Name,
                            TableCode = x.Q_Counter.ShortName,
                            Start = null,
                            TicketNumber = x.Q_Counter.CurrentNumber,
                            TimeProcess = "0",
                            StartStr = (!x.Q_Counter.IsRunning ? ("Mời bệnh nhân " + x.Q_Counter.CurrentNumber + " đến phòng : " + x.Q_Counter.Name) : ""),
                            EquipCode = x.Code
                        }).ToList();

                    var dangXL = db_.Q_DailyRequire_Detail.Where(x => x.StatusId == (int)eStatus.DAGXL).ToList();

                    foreach (var item in counters)
                    {
                        var current = dangXL.Where(x => x.StatusId == (int)eStatus.DAGXL && x.EquipCode == item.EquipCode).OrderByDescending(x => x.ProcessTime).FirstOrDefault();
                        if (current != null)
                        {
                            item.TicketNumber = current.Q_DailyRequire.TicketNumber;
                        }
                        webInfo.Details.Add(item);
                    }

                    if (webInfo.Details.Count > 0)
                    {
                        webInfo.Details[0].RuningText = webInfo.Details.Where(x => x.STT == 1).Select(x => x.StartStr).ToArray();
                        db_.Database.ExecuteSqlCommand("update q_counter set isrunning =1");
                        db_.SaveChanges();
                    }
                }
                catch (Exception ex)
                { }
                return webInfo;
            }
        }

        public ResponseBase API_PrintNewTicket(string connectString,
           string Name,
           string Address,
           int? DOB,
           string MaBenhNhan,
           string MaPhongKham,
           bool isKetLuan
           )
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var DV = db.Q_Service.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(MaPhongKham.Trim().ToUpper()));
                    if (DV != null)
                    {
                        var CF = db.Q_Config.FirstOrDefault(x => x.Code.Equals(eConfigCode.PrintType));
                        if (CF != null)
                        {
                            Q_DailyRequire rq = null;
                            int socu = 0;
                            if (Convert.ToInt32(CF.Value) == (int)ePrintType.TheoTungDichVu)
                            {
                                rq = db.Q_DailyRequire.Where(x => x.ServiceId == DV.Id).OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                                socu = ((rq == null) ? (DV.StartNumber - 1) : rq.TicketNumber);
                            }
                            else if (Convert.ToInt32(CF.Value) == (int)ePrintType.BatDauChung)
                            {
                                rq = db.Q_DailyRequire.OrderByDescending(x => x.TicketNumber).FirstOrDefault();
                                socu = ((rq == null) ? (int.Parse(db.Q_Config.FirstOrDefault(x => x.Code == eConfigCode.StartNumber).Value) - 1) : rq.TicketNumber);
                            }

                            var nv = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted && x.ServiceId == DV.Id).OrderBy(x => x.Index).FirstOrDefault();
                            if (nv == null)
                            {
                                rs.IsSuccess = false;
                                rs.Errors.Add(new Error() { MemberName = "Lỗi Nghiệp vụ", Message = "Lỗi: Dịch vụ này chưa được phân nghiệp vụ. Vui lòng liên hệ người quản lý hệ thống. Xin cám ơn!.." });
                            }
                            else
                            {
                                var sodanggoi = db.Q_DailyRequire_Detail.Where(x => x.Q_DailyRequire.ServiceId == DV.Id && x.StatusId == (int)eStatus.DAGXL).OrderByDescending(x => x.ProcessTime).FirstOrDefault();
                                var _type = (isKetLuan ? (int)eDailyRequireType.KetLuan : (int)eDailyRequireType.KhamBenh);
                                var _found = db.Q_DailyRequire_Detail.FirstOrDefault(x => x.Q_DailyRequire.ServiceId == DV.Id && x.Q_DailyRequire.MaBenhNhan == MaBenhNhan && x.Q_DailyRequire.Type == _type);
                                if (_found != null)
                                {
                                    rs.IsSuccess = true;
                                    rs.Data = _found.Q_DailyRequire.TicketNumber - 1;
                                    rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                                    rs.Data_3 = _found.Q_DailyRequire.TicketNumber;
                                }
                                else
                                {
                                    var lastTicket = db.Q_DailyRequire
                                        .Where(x => x.ServiceId == DV.Id &&
                                        x.Type == (int)eDailyRequireType.KhamBenh &&
                                        x.Q_Service.ServiceType == (int)eServiceType.PhongKham)
                                        .OrderByDescending(x => x.TicketNumber)
                                        .FirstOrDefault();
                                    DateTime emptyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                                    DateTime lastTGDKien = emptyDate;
                                    if (lastTicket != null)
                                    {
                                        emptyDate = lastTicket.TGDKien.Value.AddSeconds(DV.TimeProcess.TimeOfDay.TotalSeconds);
                                        lastTGDKien = lastTicket.TGDKien.Value;
                                    }
                                    else
                                    {
                                        TimeSpan startWork = TimeSpan.Parse("07:00:00");
                                        var configs = db.Q_Config.ToList();
                                        Q_Config config;
                                        config = configs.FirstOrDefault(x => x.Code == eConfigCode.StartWork);
                                        if (config != null)
                                            TimeSpan.TryParse(config.Value, out startWork);
                                        emptyDate = emptyDate.AddSeconds(startWork.TotalSeconds);
                                        lastTGDKien = emptyDate;
                                    }

                                    var obj = new Q_DailyRequire();
                                    obj.TicketNumber = (socu + 1);
                                    obj.ServiceId = DV.Id;
                                    obj.BusinessId = null;
                                    obj.PrintTime = DateTime.Now;
                                    obj.ServeTimeAllow = DV.TimeProcess.TimeOfDay;
                                    obj.CustomerName = Name;
                                    obj.CustomerDOB = DOB;
                                    obj.CustomerAddress = Address;
                                    obj.MaBenhNhan = MaBenhNhan;
                                    obj.MaPhongKham = MaPhongKham;
                                    obj.STT_PhongKham = "";
                                    obj.Type = _type;
                                    obj.TGDKien = emptyDate;
                                    obj.Q_DailyRequire_Detail = new Collection<Q_DailyRequire_Detail>();

                                    var detail = new Q_DailyRequire_Detail();
                                    detail.Q_DailyRequire = obj;
                                    detail.MajorId = (nv != null ? nv.MajorId : 1);
                                    detail.StatusId = (int)eStatus.CHOXL;
                                    obj.Q_DailyRequire_Detail.Add(detail);
                                    db.Q_DailyRequire.Add(obj);
                                    db.SaveChanges();
                                    rs.IsSuccess = true;
                                    rs.Data = socu;
                                    rs.Records = (sodanggoi != null ? sodanggoi.Q_DailyRequire.TicketNumber : 0);
                                    rs.Data_3 = obj.TicketNumber;
                                }
                            }
                        }
                        else
                        {
                            rs.IsSuccess = false;
                            rs.Errors.Add(new Error() { MemberName = "Lỗi", Message = "Không tìm thấy cấu hình kiểu in phiếu trong hệ thống. Vui lòng kiểm tra lại." });
                        }
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Lỗi", Message = "Không tìm thấy mã phòng khám (" + MaPhongKham + ") trong hệ thống. Vui lòng kiểm tra lại." });
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "Lỗi Exception", Message = "Lỗi thực thi. " + ex.Message });
            }
            return rs;
        }

        /// <summary>
        /// api
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="ticket"></param>
        /// <param name="maBN"></param>
        /// <param name="maPK"></param>
        /// <returns></returns>
        public int DeleteTicket(string connectString, int ticket, string maBN, string maPK, int type)
        {
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var objs = db.Q_DailyRequire_Detail
                        .Where(x => x.Q_DailyRequire.TicketNumber == ticket &&
                                    maBN.Trim() == x.Q_DailyRequire.MaBenhNhan.Trim() &&
                                    x.Q_DailyRequire.MaPhongKham.Trim() == maPK.Trim() &&
                                    x.Q_DailyRequire.Type == type);
                    if (objs != null && objs.Count() > 0)
                    {
                        int parentId = 0;
                        foreach (var item in objs)
                        {
                            parentId = item.DailyRequireId;
                            db.Q_DailyRequire_Detail.Remove(item);
                        }
                        if (parentId != 0)
                        {
                            var parentObj = db.Q_DailyRequire.FirstOrDefault(x => x.Id == parentId);
                            if (parentObj != null)
                            {
                                ticket = parentObj.TicketNumber;
                                db.Q_DailyRequire.Remove(parentObj);
                            }
                        }
                        db.SaveChanges();
                        return ticket;
                    }
                }
            }
            catch (Exception)
            { }
            return 0;
        }

        public int DoneTicket(string connectString, string maBN, string maPK, int _ticket, int type)
        {
            int ticket = 0;
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    var objs = db.Q_DailyRequire_Detail
                        .Where(x => x.Q_DailyRequire.TicketNumber == _ticket &&
                                    maBN.Trim() == x.Q_DailyRequire.MaBenhNhan.Trim() &&
                                    x.Q_DailyRequire.MaPhongKham.Trim() == maPK.Trim() &&
                                    x.Q_DailyRequire.Type == type);
                    if (objs != null && objs.Count() > 0)
                    {
                        foreach (var item in objs)
                        {
                            item.StatusId = (int)eStatus.HOTAT;
                            item.EndProcessTime = DateTime.Now;
                            ticket = item.Q_DailyRequire.TicketNumber;
                        }
                        db.SaveChanges();
                        return ticket;
                    }
                }
            }
            catch (Exception ex)
            { }
            return ticket;
        }

        public List<PrintModel> FindTickets(string connectString, string phoneNumer)
        {
            var rs = new List<PrintModel>();
            try
            {
                using (db = new QMSSystemEntities(connectString))
                {
                    rs.AddRange(db.Q_DailyRequire.Where(x => x.PhoneNumber.Trim().Equals(phoneNumer.Trim()))
                        .OrderByDescending(x => x.TicketNumber).ToList()
                        .Select(x => new PrintModel()
                        {
                            STT = x.TicketNumber,
                            ServiceId = x.ServiceId,
                            TenDichVu = x.Q_Service.Name,
                            NoteDV = x.Q_Service.Note,

                            TenKH = x.CustomerName,
                            DOB = x.CustomerDOB ?? 0,
                            DiaChi = x.CustomerAddress,
                            MaKH = x.MaBenhNhan,
                            Phone = x.PhoneNumber,
                            MaDV = x.MaPhongKham,
                            SoXe = x.PrintTime.ToString("dd/MM/yyyy hh:mm tt")
                        })
                        .ToList());
                    if (rs.Count > 0)
                    {
                        var serviceSteps = db.Q_ServiceStep.Where(x => !x.IsDeleted && !x.Q_Service.IsDeleted)
                            .Select(x => new { Index = x.Index, MajorId = x.MajorId, ServiceId = x.ServiceId })
                            .ToList();
                        var userMajors = db.Q_UserMajor.Where(x => !x.IsDeleted && !x.Q_User.IsDeleted && !x.Q_Major.IsDeleted)
                            .Select(x => new { MajorId = x.MajorId, UserId = x.UserId })
                            .ToList();
                        var logins = db.Q_Login.Where(x => x.StatusId == (int)eStatus.LOGIN && !x.Q_User.IsDeleted)
                            .Select(x => new { EquipCode = x.EquipCode, UserId = x.UserId })
                            .ToList();
                        var equipments = db.Q_Equipment.Where(x => !x.IsDeleted && !x.Q_Counter.IsDeleted)
                            .Select(x => new { CounterId = x.CounterId, Code = x.Code, CounterName = x.Q_Counter.Name })
                            .ToList();
                        foreach (var item in rs)
                        {
                            var nv = serviceSteps.Where(x => x.ServiceId == item.ServiceId).OrderBy(x => x.Index).FirstOrDefault();
                            var userIds = userMajors.Where(x => x.MajorId == nv.MajorId).Select(x => x.UserId).ToList();
                            var equipCodes = logins.Where(x => userIds.Contains(x.UserId)).Select(x => x.EquipCode).ToList();

                            var tq = equipments.Where(x => (equipCodes.Count == 0 && userIds.Count > 0 ? userIds.Contains(x.Code) : equipCodes.Contains(x.Code)))
                                      .Select(x => x.CounterName).ToArray();
                            item.TenQuay = String.Join("@", tq).Replace("@", ("" + System.Environment.NewLine));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return rs;
        }
    }
}
