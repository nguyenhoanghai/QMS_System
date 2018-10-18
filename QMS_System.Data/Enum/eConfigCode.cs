using System;
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
        public const string  CheckServeOverTime= "CheckServeOverTime";

        public const string VideoUrl = "VideoUrl";

    }
}
