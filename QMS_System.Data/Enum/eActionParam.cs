using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace QMS_System.Data.Enum
{
    public static class eActionParam
    {
        public const string HITHI = "HITHI";
        /// <summary>
        /// Done current ticket
        /// </summary>
        public const string HoanTat = "HOTAT";  
        /// <summary>
        /// Delete ticket out of database
        /// </summary>
        public const string HuyKH = "HUYKH";
        public const string NhoKH = "NHOKH";
        public const string NHOPN = "NHOPN";//NhoKH_PhanNhanh
        public const string SAUNK = "SAUNK"; //NhoKHSauN_KH
        public const string BTPNL = "BTPNL"; //GoiBT_PhanNhanhGLP
        /// <summary>
        /// Call ticket from counter
        /// </summary>
        public const string COUNT = "COUNT";  
        public const string DEGDQ = "DEGDQ";  //lay phieu phan dieu
        public const string DEGLP = "DEGLP";  // lay phieu phan dieu glp
        public const string GCULP = "GCULP";  // goi cung luc noi bo + bt
        /// <summary>
        /// Call new ticket follow user major
        /// </summary>
        public const string NGVU = "1NGVU";   
        /// <summary>
        /// Call new ticket follow Time get ticket
        /// </summary>
        public const string GLAYP = "GLAYP";   
        /// <summary>
        /// Call new ticket follow Time get ticket
        /// </summary>
        public const string GDENQ = "GDENQ";   

        public const string MOIKH = "MOIKH";  // goi moi GLP 
        /// <summary>
        /// Count all ticket have done processed
        /// </summary>
        public const string TKHDG = "TKHDG";   
        /// <summary>
        /// Count all ticket are waiting for processed
        /// </summary>
        public const string THKCG = "THKCG";
        /// <summary>
        /// User login or logout of system
        /// </summary>
        public const string LOGIO = "LOGIO"; 

        /// <summary>
        /// Call number recieve from counter
        /// </summary>
        public const string NHAKH = "NHAKH";
        /// <summary>
        /// Tranfer current Number to another major
        /// </summary>
        public const string NGHVU = "NGHVU";
        /// <summary>
        /// Call ticket follow the major requirement
        /// </summary>
        public const string GNVYC = "GNVYC";

        public const string SOUNDONLY = "SOUNDONLY";
        public const string DANHGIA = "DANHGIA";

        /// <summary>
        /// goi mới phân điều nghiệp vụ theo nhân viên
        /// </summary>
        public const string PHANDIEUNHANVIEN = "PHANDIEUNHANVIEN";
        /// <summary>
        /// Thay thế thời gian phục vụ DK
        /// </summary>
        public const string THAYTHE_TGIAN_PVU = "THAYTHE_TGIAN_PVU";
       
        /// <summary>
        /// cộng thêm thời gian phục vụ DK
        /// </summary>
        public const string CONGTHEM_TGIAN_PVU = "CONGTHEM_TGIAN_PVU";

        /// <summary>
        /// gửi thời gian phục vụ dk cho counter 
        /// </summary>
        public const string GUI_TGIAN_PVU_DKIEN = "GUI_TGIAN_PVU_DKIEN";
        /// <summary>
        /// gửi thời gian kết thúc dk cho counter 
        /// </summary>
        public const string GUI_TGIAN_KTHUC_DKIEN = "GUI_TGIAN_KTHUC_DKIEN";
    }
}
