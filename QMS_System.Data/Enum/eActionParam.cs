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
        /// User login or logout out of system
        /// </summary>
        public const string LOGIN = "LOGIN";

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
    }
}
