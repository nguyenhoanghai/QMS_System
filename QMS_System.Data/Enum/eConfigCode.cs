﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QMS_System.Data.Enum
{
    public static class eConfigCode
    {
        public const string SoundPath = "SoundPath";
        public const string ComportName = "ComportName";
        public const string BaudRate = "BaudRate";
        public const string DataBits = "DataBits";
        public const string Parity = "Parity";
        public const string StopBits = "StopBits";
        public const string TimeRepeat = "TimeRepeat";
        public const string PrintType = "PrintType";
        public const string SilenceTime = "SilenceTime";
        public const string NewCustomer = "NewCustomer";
        public const string TimeWaitForRecieveData = "TimeWaitForRecieveData";
        public const string StartNumber = "StartNumber";
        public const string SoundLockPrintTicket = "SoundLockPrintTicket";
        public const string CheckTimeBeforePrintTicket = "CheckTimeBeforePrintTicket"; 

        //Màn hình cấp vé
        public const string Background = "Background";
        public const string ImagePath = "ImagePath";
        public const string Column = "Column";
        public const string ButtonWidth = "ButtonWidth";
        public const string ButtonHeight = "ButtonHeight";
        public const string ButtonSpace = "ButtonSpace";
        public const string ButtonFont = "ButtonFont";
        public const string ButtonBackColor = "ButtonBackColor";
        public const string ButtonForeColor = "ButtonForeColor";
        public const string ButtonName = "btnService";
        public const string AutoSetLoginFromYesterday = "AutoSetLoginFromYesterday";
        public const string PrintTicketCode = "PrintTicketCode";

        /// <summary>
        /// Printer
        /// </summary>
        public const string ComName_Printer = "ComName_Printer";
        public const string BaudRate_Printer = "BaudRate_Printer";
        public const string DataBits_Printer = "DataBits_Printer";
        public const string Parity_Printer = "Parity_Printer";
        public const string StopBits_Printer = "StopBits_Printer";

        /// <summary>
        /// SMS
        /// </summary>
        public const string ComName_SMS = "ComName_SMS";
        public const string BaudRate_SMS = "BaudRate_SMS";
        public const string DataBits_SMS = "DataBits_SMS";
        public const string Parity_SMS = "Parity_SMS";
        public const string StopBits_SMS = "StopBits_SMS";

        public const string DistanceWarningTimeEnd = "DistanceWarningTimeEnd";
        public const string SendSMSWarningTime = "SendSMSWarningTime";

        /// <summary>
        /// name of read serve overtime  config sound  
        /// </summary>
        public const string TemplateSoundServeOverTime = "TemplateSoundServeOverTime";
        public const string TimerQuetServeOverTime = "TimerQuetServeOverTime";
        public const string TimesRepeatReadServeOver = "TimesRepeatReadServeOver"; 
        public const string  AutoCallFollowMajorOrder = "AutoCallFollowMajorOrder"; 
        public const string  CheckServeOverTime= "CheckServeOverTime";

        public const string VideoUrl = "VideoUrl";

        /// <summary>
        /// sử dụng chung với phần mềm bên thứ 3
        /// </summary>
        public const string UseWithThirdPattern = "UseWithThirdPattern";

        /// <summary>
        /// hệ thống
        /// 1=> xe máy
        /// 2=> khác
        /// </summary>
        public const string System = "System";

        /// <summary>
        /// khi cấp phiếu tự gọi số nếu User đang rảnh theo user rảnh lâu nhất
        /// </summary>
        public const string AutoCallIfUserFree = "AutoCallIfUserFree";

        /// <summary>
        /// Số lần đọc lại cảnh báo thời gian phục vụ sắp hết
        /// </summary>
        public const string RepeatTimesOfServeOverTime = "RepeatTimesOfServeOverTime";
        /// <summary>
        /// lệnh 8c sử dụng cho
        /// 1. login logout
        /// 2. đổi TG phục vụ DK
        /// </summary>
        public const string _8cUseFor = "8cUseFor";

        /// <summary>
        /// kết thúc giao dịch sau khi đánh giá
        /// </summary>
        public const string DoneTicketAfterEvaluate = "DoneTicketAfterEvaluate";

        /// <summary>
        /// ktra giới hạn số phiếu dc lấy theo dịch vụ
        /// </summary>
        public const string CheckServiceLimit = "CheckServiceLimit";

        //sau khi in phieu trả về so dang goi hay ma dich vu
        public const string PrintTicketReturnCurrentNumberOrServiceCode = "PrintTicketReturnCurrentNumberOrServiceCode";

        //mau phieu
        public const string TicketTemplate = "TicketTemplate";
        // so liên in phiếu mỗi lần
        public const string NumberOfLinePerTime = "NumberOfLinePerTime";
        // su dung máy in board
        public const string UsePrintBoard = "UsePrintBoard";
        //ten com may in ko su dung board 
        public const string COM_Print = "COMPrint";
        public const string TVReadSound = "TVReadSound";

        public const string SendMail = "SendMail";
        public const string TimeSendMail = "TimeSendMail";
        public const string MailFrom = "MailFrom";
        public const string MailFromPass = "MailFromPass";
        public const string MailTo = "MailTo";
        public const string MailSubject = "MailSubject";

        public const string HUUNGHI_JSON = "HUUNGHI_JSON"; 
        public const string StartWork = "StartWork";

        /// <summary>
        /// kiểu cấp phiếu khi nhận dc tin nhắn có mã phòng khám nằm ngoài ds DichVu
        /// 1: cấp chung ai lay truoc goi truoc
        /// 2: cấp thẳng cho người có số lượng phiếu ít nhất tại thời điểm hiện tại
        /// </summary>
        public const string TypeOfPrintTicketWithoutService = "TypeOfPrintTicketWithoutService";

        public const string NodeServerIP = "NodeServerIP";
        
        public const string UseFakeTiket = "UseFakeTiket";
        public const string NumOfFakeTiket = "NumOfFakeTiket";
        public const string NWaitting = "NWaitting";
        public const string IsSaveHistory = "IsSaveHistory";

        /// <summary>
        /// cách su dung lệnh 8f
        /// 0 bình thường
        /// 1 gọi ưu tiên
        /// </summary>
        public const string _8aUseFor = "8aUseFor";

    }
}
