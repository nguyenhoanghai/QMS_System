using GPRO.Core.Hai;
using Microsoft.Reporting.WinForms;
using Newtonsoft.Json;
using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.BLL.VietThaiQuan;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using QMS_System.Helper;
using QMS_System.IssueTicketScreen;
using QMS_System.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace QMS_System
{
    public partial class frmMain_ver3 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        SoundPlayer player;
        List<string> playlist, dataSendToComport, arrStr,
          temp = new List<string>(), tempHex = new List<string>(),
            strEquipmentIds = new List<string>();
        Thread playThread;

        List<ConfigModel> configs;
        int q = -1,
            printType = 1,
            startNumber = 0,
            timeQuetComport = 50,
         PrintTicketCode = 50,
            system = 0,
            timesRepeatReadServeOver = 1,
            autoCallFollowMajorOrder = 0,
            silenceTime = 100,
            printTicketReturnCurrentNumberOrServiceCode = 1,
            _8cUseFor = 1,
            TVReadSound = 0,
            UsePrintBoard = 1;

        public static int UseWithThirdPattern = 0,
            AutoCallIfUserFree = 0;

        string soundPath = "",
            errorsms = "",
            SoundLockPrintTicket = "khoa.wav",

            CheckTimeBeforePrintTicket = "0";

        bool isSendDataKeyPad = false,
            isFinishRead = true,
            isErorr = false,
            isRunning = false,
            autoCall = false,
            hasAA = false;
        public static List<ServiceDayModel> lib_Services;
        public static List<RegisterUserCmdModel> lib_RegisterUserCmds;
        public static List<LoginHistoryModel> lib_Users;
        public static List<UserMajorModel> lib_UserMajors;
        public static List<UserCmdReadSoundModel> lib_UserCMDReadSound;
        public static List<EquipmentModel> lib_Equipments;
        public static List<SoundModel> lib_Sounds;
        public static List<ReadTemplateModel> lib_ReadTemplates;
        public static List<CounterSoundModel> lib_CounterSound;
        public static List<int> equipmentIds;

        public static DateTime today = DateTime.Now;
        public static bool IsDatabaseChange = false;
        public static SerialPort
            comPort = new SerialPort(),
            COM_Printer = new SerialPort();
        public string lbSendrequest, lbSendData, lbPrinRequire;
        public static string connectString;
        public static List<PrintTicketModel> printTemplates;
        public static string logText = "";

        int so = 1000, gio = 1;

        public frmMain_ver3()
        {
            InitializeComponent();
        }

        private void ResetDayInfo()
        {
            try
            {
                errorsms = "sang ngày mới " + DateTime.Now.Day;
                BLLLoginHistory.Instance.ResetDailyLoginInfor(connectString, today, GetConfigByCode(eConfigCode.AutoSetLoginFromYesterday));
                BLLDailyRequire.Instance.CopyHistory(connectString, true);
                Settings.Default.Today = DateTime.Now.Day;
                Settings.Default.Save();
                //ko su dung GO 
                var query = @"  UPDATE [Q_COUNTER] set lastcall = 0 ,isrunning =1 , CurrentNumber=0 , LastCallKetLuan= 0 
                                        DELETE from Q_CounterSoftRequire  
                                        DELETE [Q_CounterSoftSound]  
                                        DBCC CHECKIDENT('Q_CounterSoftSound', RESEED, 1);                                             
                                        DELETE [dbo].[Q_RequestTicket]  
                                        DBCC CHECKIDENT('Q_RequestTicket', RESEED, 1); 
                                        DELETE [dbo].[Q_TVReadSound]  
                                        DBCC CHECKIDENT('Q_TVReadSound', RESEED, 1); 
                                        DELETE [Q_DailyRequire_WorkDetail]  
                                        DBCC CHECKIDENT('Q_DailyRequire_WorkDetail', RESEED, 1);
                                        UPDATE [Q_COUNTERDAYINFO] set [STT] = 0, [STT_3]='0',[STT_QMS]=0 ,[StatusSTT] = 0 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL     ";
                BLLSQLBuilder.Instance.Excecute(connectString, query);
                lib_Users = BLLLoginHistory.Instance.GetsForMain(connectString);
            }
            catch (Exception ex) { }
        }

        private void frmMain_ver3_Load(object sender, EventArgs e)
        {
            try
            {
                // docFileExcel();
                //sqlStatus = BaseCore.Instance.CONNECT_STATUS(Application.StartupPath + "\\DATA.XML");
                //if (sqlStatus)
                //{
                connectString = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
                QMSAppInfo.ConnectString = connectString;
                QMSAppInfo.sqlConnection = DatabaseConnection.Instance.Connect(Application.StartupPath + "\\DATA.XML");
                configs = BLLConfig.Instance.Gets(connectString, true);
                soundPath = GetConfigByCode(eConfigCode.SoundPath);

                printTemplates = BLLPrintTemplate.Instance.Gets(connectString).Where(x => x.IsActive).ToList();

                SoundLockPrintTicket = GetConfigByCode(eConfigCode.SoundLockPrintTicket);
                CheckTimeBeforePrintTicket = GetConfigByCode(eConfigCode.CheckTimeBeforePrintTicket);

                int.TryParse(GetConfigByCode(eConfigCode.PrintType), out printType);
                int.TryParse(GetConfigByCode(eConfigCode.PrintTicketReturnCurrentNumberOrServiceCode), out printTicketReturnCurrentNumberOrServiceCode);

                int.TryParse(GetConfigByCode(eConfigCode.SilenceTime), out silenceTime);
                int.TryParse(GetConfigByCode(eConfigCode.StartNumber), out startNumber);
                int.TryParse(GetConfigByCode(eConfigCode.TimeWaitForRecieveData), out timeQuetComport);
                int.TryParse(GetConfigByCode(eConfigCode.PrintTicketCode), out PrintTicketCode);
                int.TryParse(GetConfigByCode(eConfigCode.TimesRepeatReadServeOver), out timesRepeatReadServeOver);
                int.TryParse(GetConfigByCode(eConfigCode.AutoCallFollowMajorOrder), out autoCallFollowMajorOrder);
                int.TryParse(GetConfigByCode(eConfigCode.UseWithThirdPattern), out UseWithThirdPattern);
                int.TryParse(GetConfigByCode(eConfigCode.AutoCallIfUserFree), out AutoCallIfUserFree);
                int.TryParse(GetConfigByCode(eConfigCode.System), out system);
                int.TryParse(GetConfigByCode(eConfigCode._8cUseFor), out _8cUseFor);
                int.TryParse(GetConfigByCode(eConfigCode.UsePrintBoard), out UsePrintBoard);
                int.TryParse(GetConfigByCode(eConfigCode.TVReadSound), out TVReadSound);
                equipmentIds = ConfigurationManager.AppSettings["quetEquipments"].ToString().Split(',').Select(x => Convert.ToInt32(x)).ToList();
                strEquipmentIds.Clear();
                for (int i = 0; i < equipmentIds.Count; i++)
                {
                    strEquipmentIds.Add(equipmentIds[i] < 10 ? ("0" + equipmentIds[i]) : equipmentIds[i].ToString());
                }

                if (Settings.Default.Today != DateTime.Now.Day)
                {
                    ResetDayInfo();
                }
                else
                    errorsms = "";
                btRunProcess.PerformClick();
                try
                {
                    string autoLogin = ConfigurationManager.AppSettings["AutoLogin"].ToString();
                    if (!string.IsNullOrEmpty(autoLogin))
                        BLLLoginHistory.Instance.AutoLogin(connectString, autoLogin);
                }
                catch (Exception) { }
                //}
                //else
                //{
                //    errorsms = "Kết nối máy chủ SQL thất bại. Vui lòng kiểm tra lại.";
                //    Form form = new FrmSQLConnect();
                //    form.ShowDialog();
                //}


                //tao cong viec mau cho viet thai quan
                // string path = (Application.StartupPath + "\\dinh muc thoi gian.xlsx");    
                //  BLLWork.Instance.taoCongViec(path,connectString);



                //string json = "[{'PhongBan_Id':787,'MaPhongBan':'KHN1','TenPhongBan':'Khám hội nghị 1','TenKhongDau':'Kham hoi nghi 1','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':788,'MaPhongBan':'KHN2','TenPhongBan':'Khám hội nghị 2','TenKhongDau':'Kham hoi nghi 2','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':800,'MaPhongBan':'KHN3','TenPhongBan':'Khám Quốc Hội - Hội Nghị ','TenKhongDau':'Kham Quoc Hoi - Hoi Nghi ','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':432,'MaPhongBan':'KA','TenPhongBan':'Khoa khám bệnh A','TenKhongDau':'Khoa kham benh A','TenPhongBan_En':'K01.2','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':433,'MaPhongBan':'KB','TenPhongBan':'Khoa Khám bệnh B','TenKhongDau':'Khoa Kham benh B','TenPhongBan_En':'K01.1','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':425,'MaPhongBan':'VLTL-PHCN','TenPhongBan':'Khoa Phục hồi chức năng','TenKhongDau':'Khoa Phuc hoi chuc nang','TenPhongBan_En':'K31','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':430,'MaPhongBan':'RMH','TenPhongBan':'Khoa Răng hàm mặt','TenKhongDau':'Khoa Rang ham mat','TenPhongBan_En':'RMH','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':676,'MaPhongBan':'BVSKTWII','TenPhongBan':'Phòng Bảo vệ sức khoẻ TW2','TenKhongDau':'Phong Bao ve suc khoe TW2','TenPhongBan_En':'K01.3','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':677,'MaPhongBan':'BVSKTWIII','TenPhongBan':'Phòng Bảo vệ sức khoẻ TW3','TenKhongDau':'Phong Bao ve suc khoe TW3','TenPhongBan_En':'K01.4','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':678,'MaPhongBan':'BVSKTWV','TenPhongBan':'Phòng Bảo vệ sức khoẻ TW5','TenKhongDau':'Phong Bao ve suc khoe TW5','TenPhongBan_En':'K01.5','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':668,'MaPhongBan':'PBVSKCB','TenPhongBan':'Phòng khám bảo vệ sức khoẻ cán bộ','TenKhongDau':'Phong kham bao ve suc khoe can bo','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':742,'MaPhongBan':'PKQH','TenPhongBan':'Phòng khám phục vụ quốc hội','TenKhongDau':'Phong kham phuc vu quoc hoi','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':743,'MaPhongBan':'QHHC','TenPhongBan':'Phòng khám quốc hội (điểm hoàng cầu)','TenKhongDau':'Phong kham quoc hoi (diem hoang cau)','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':744,'MaPhongBan':'QHLT','TenPhongBan':'Phòng khám quốc hội (điểm la thành)','TenKhongDau':'Phong kham quoc hoi (diem la thanh)','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':738,'MaPhongBan':'ytcq','TenPhongBan':'Y tế cơ quan','TenKhongDau':'Y te co quan','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':227,'MaPhongBan':'CDHA','TenPhongBan':'Khoa Cdha','TenKhongDau':'Khoa Cdha','TenPhongBan_En':'K39','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':656,'MaPhongBan':'GPBL','TenPhongBan':'Khoa Giải phẫu bệnh','TenKhongDau':'Khoa Giai phau benh','TenPhongBan_En':'K42','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':419,'MaPhongBan':'SH','TenPhongBan':'Khoa Hóa sinh','TenKhongDau':'Khoa Hoa sinh','TenPhongBan_En':'K37','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':422,'MaPhongBan':'VS','TenPhongBan':'Khoa Vi sinh','TenKhongDau':'Khoa Vi sinh','TenPhongBan_En':'K38','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':905,'MaPhongBan':'VTC256','TenPhongBan':'Phòng chụp vct256','TenKhongDau':'Phong chup vct256','TenPhongBan_En':'VTC256','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':728,'MaPhongBan':'VCT64','TenPhongBan':'Phòng chụp VCT64 (tầng 1 - nhà 7)','TenKhongDau':'Phong chup VCT64 (tang 1 - nha 7)','TenPhongBan_En':'VCT64','Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':736,'MaPhongBan':'SCT64','TenPhongBan':'Phòng Soi  VCT64 (tầng 1 - nhà 7)','TenKhongDau':'Phong Soi  VCT64 (tang 1 - nha 7)','TenPhongBan_En':null,'Cap':1,'CapTren_Id':0,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':577,'MaPhongBan':'KB_KB_CKTM11','TenPhongBan':'11B - CK Tim Mạch ','TenKhongDau':'11B - CK Tim Mach ','TenPhongBan_En':'11','Cap':2,'CapTren_Id':409,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':683,'MaPhongBan':'KB_VLTL_PTK11','TenPhongBan':'11VLTL BS Đạt','TenKhongDau':'11VLTL BS dat','TenPhongBan_En':'11VL','Cap':2,'CapTren_Id':425,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':574,'MaPhongBan':'KB_KB_CKTK13','TenPhongBan':'13B - CK Thần kinh ','TenKhongDau':'13B - CK Than kinh ','TenPhongBan_En':'13','Cap':2,'CapTren_Id':411,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':778,'MaPhongBan':'KB_KB_CKHH','TenPhongBan':'18 Huyết Học','TenKhongDau':'18 Huyet Hoc','TenPhongBan_En':'18','Cap':2,'CapTren_Id':420,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':780,'MaPhongBan':'KB_KB_CKLM','TenPhongBan':'18 Lọc Máu','TenKhongDau':'18 Loc Mau','TenPhongBan_En':'18','Cap':2,'CapTren_Id':424,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':540,'MaPhongBan':'KB_KB_NOI18','TenPhongBan':'18B - Khám Nội','TenKhongDau':'18B - Kham Noi','TenPhongBan_En':'18','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':549,'MaPhongBan':'KB_KB_NOI7','TenPhongBan':'201 - Khám Nội BS. Loan','TenKhongDau':'7B - Kham Noi BS.Loan','TenPhongBan_En':'201','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':578,'MaPhongBan':'KB_KB_NOI5','TenPhongBan':'202 - Khám Nội BS. Hưng','TenKhongDau':'5B - Kham Noi BS.Hung','TenPhongBan_En':'202','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':879,'MaPhongBan':'KB_KB_NOI17','TenPhongBan':'203 - Khám Nội BS. Ngọc','TenKhongDau':'17B - Kham Noi BS.Ngoc','TenPhongBan_En':'203','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':576,'MaPhongBan':'KB_KB_NOI8','TenPhongBan':'204 - Khám Nội BS. Hồng','TenKhongDau':'8B - Kham Noi BS Hong','TenPhongBan_En':'204','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':551,'MaPhongBan':'KB_KB_NOI6','TenPhongBan':'207 - Khám Da Liễu BS. Hương','TenKhongDau':'6B - Kham Da Lieu BS.Huong','TenPhongBan_En':'207','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':775,'MaPhongBan':'KB_KB_CKN','TenPhongBan':'208 - Khám Ngoại','TenKhongDau':'15B - Ngoai','TenPhongBan_En':'208','Cap':2,'CapTren_Id':417,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':550,'MaPhongBan':'KB_KB_NOI10','TenPhongBan':'209 - Khám Nội BS.Hằng','TenKhongDau':'209 - Kham Noi BS.Hang','TenPhongBan_En':'209','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':575,'MaPhongBan':'KB_KB_NOI4','TenPhongBan':'210 - Khám nội BS. Hằng','TenKhongDau':'207 - Kham no BS. Hang','TenPhongBan_En':'210','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':580,'MaPhongBan':'KB_KB_NOI12','TenPhongBan':'211 - Khám Nội BS. Chung','TenKhongDau':'12B - Kham Noi','TenPhongBan_En':'211','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':779,'MaPhongBan':'KB_KB_CKMV','TenPhongBan':'212 - Khám Mạch vành','TenKhongDau':'211 - Kham Mach vanh','TenPhongBan_En':'212','Cap':2,'CapTren_Id':726,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':746,'MaPhongBan':'KB_KB_NOI212','TenPhongBan':'212 - Khám Nội','TenKhongDau':'212 - Kham Noi','TenPhongBan_En':'212B','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':889,'MaPhongBan':'T4_TTN','TenPhongBan':'212 - Khám Thận tiết niệu','TenKhongDau':'Kham Than Tiet Nieu','TenPhongBan_En':'212','Cap':2,'CapTren_Id':413,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':908,'MaPhongBan':'KB_KB_NOI_KD','TenPhongBan':'213 - Khám Da Liễu BS.Trang','TenKhongDau':'213 - Kham Da Lieu BS.Trang','TenPhongBan_En':'213','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':799,'MaPhongBan':'KB_KB_NOI213','TenPhongBan':'213 - Khám Nội Tổ','TenKhongDau':'213 - Kham Noi To','TenPhongBan_En':'213','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':595,'MaPhongBan':'KB_KB_CKTT','TenPhongBan':'213 - Khám Tâm thần','TenKhongDau':'18B - Tam Than','TenPhongBan_En':'213','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':567,'MaPhongBan':'KB_KB_PK3','TenPhongBan':'215 - Khám Phụ khoa','TenKhongDau':'16B - Kham  Phu khoa','TenPhongBan_En':'215','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':680,'MaPhongBan':'KB_VLTL_PK2','TenPhongBan':'2VLTL BS. Dần','TenKhongDau':'2VLTL BS. Dan','TenPhongBan_En':'K31','Cap':2,'CapTren_Id':425,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':708,'MaPhongBan':'KB_RHM_Ð','TenPhongBan':'301 - BS Phương','TenKhongDau':'301 - BS Phuong','TenPhongBan_En':'301','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':705,'MaPhongBan':'KB_RHM_C','TenPhongBan':'302 - BS Hiếu','TenKhongDau':'C Bs Hieu','TenPhongBan_En':'302','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':709,'MaPhongBan':'KB_RHM_E','TenPhongBan':'303 - BS Anh','TenKhongDau':'303 - BS Anh','TenPhongBan_En':'303','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':710,'MaPhongBan':'KB_RHM_G','TenPhongBan':'304 - BS Liên','TenKhongDau':'304 - BS Lien','TenPhongBan_En':'304','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':711,'MaPhongBan':'KB_RHM_H','TenPhongBan':'305 - BS Yến','TenKhongDau':'H Bs Yen','TenPhongBan_En':'305','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':679,'MaPhongBan':'KB_VLTL_DTCA3','TenPhongBan':'3VLTL BS Hiền','TenKhongDau':'3VLTL BS Hien','TenPhongBan_En':'3VL','Cap':2,'CapTren_Id':425,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':703,'MaPhongBan':'KB_RHM_A','TenPhongBan':'401 - BS Hội','TenKhongDau':'401 - BS Hoi','TenPhongBan_En':'401','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':704,'MaPhongBan':'KB_RHM_B','TenPhongBan':'402 - BS Khuê','TenKhongDau':'B Bs Khue','TenPhongBan_En':'402','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':693,'MaPhongBan':'KB_TMH_D','TenPhongBan':'410 - TMH BS Hằng','TenKhongDau':'DTMH BS Hang','TenPhongBan_En':'410','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':896,'MaPhongBan':'KNSTMH','TenPhongBan':'412 - TMH Bs Phương','TenKhongDau':'412 - TMH Bs Phuong','TenPhongBan_En':'412','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':691,'MaPhongBan':'KB_TMH_E','TenPhongBan':'413 - TMH BS Minh','TenKhongDau':'413 - TMH BS Minh','TenPhongBan_En':'413','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':892,'MaPhongBan':'KB_TMH_H','TenPhongBan':'414 - TMH BS Chi','TenKhongDau':'HTMHBS Chi','TenPhongBan_En':'414','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':688,'MaPhongBan':'KB_TMH_B','TenPhongBan':'415 - TMH Bs Hà','TenKhongDau':'BTMH Bs Ha','TenPhongBan_En':'415','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':690,'MaPhongBan':'KB_TMH_A','TenPhongBan':'416 - TMH BS Kiên','TenKhongDau':'ATMHBS Kien','TenPhongBan_En':'416','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':685,'MaPhongBan':'KB_VLTL_PK6','TenPhongBan':'6VLTL BS Linh','TenKhongDau':'6VLTL BS Linh','TenPhongBan_En':'K31','Cap':2,'CapTren_Id':425,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':682,'MaPhongBan':'KB_VLTL_TD8','TenPhongBan':'8VLTL','TenKhongDau':'8VLTL','TenPhongBan_En':'8VL','Cap':2,'CapTren_Id':425,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':782,'MaPhongBan':'T4 - DU','TenPhongBan':'9B - Hô Hấp Dị Ứng','TenKhongDau':'9B - Ho Hap Di ung','TenPhongBan_En':'9','Cap':2,'CapTren_Id':408,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':894,'MaPhongBan':'KB_KB_NOI9','TenPhongBan':'9B - Khám Nội','TenKhongDau':'9B - Kham Noi','TenPhongBan_En':'9','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':538,'MaPhongBan':'KB_KA_CKDY','TenPhongBan':'ADY - Đông Y BS. Lý','TenKhongDau':'ADY - dong Y BS. Ly','TenPhongBan_En':'ADY','Cap':2,'CapTren_Id':414,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':564,'MaPhongBan':'KB_KA_NOI1','TenPhongBan':'AN1 - Khám Nội 1 ','TenKhongDau':'AN1 - Kham Noi 1 ','TenPhongBan_En':'Nội1','Cap':2,'CapTren_Id':432,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':539,'MaPhongBan':'KB_KA_NOI2','TenPhongBan':'AN2 - Khám Nội 2 ','TenKhongDau':'AN2 - Kham Noi 2 ','TenPhongBan_En':'Nội2','Cap':2,'CapTren_Id':432,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':563,'MaPhongBan':'KB_KA_NOI3','TenPhongBan':'AN3 - Khám Nội 3','TenKhongDau':'AN3 - Kham Noi 3','TenPhongBan_En':'Nội3','Cap':2,'CapTren_Id':432,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':875,'MaPhongBan':'KB_KA_NOI4','TenPhongBan':'AN4 - Ra Viện','TenKhongDau':'AN4 - Ra Vien','TenPhongBan_En':'AN4','Cap':2,'CapTren_Id':432,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':594,'MaPhongBan':'KB_KB_CKTH15','TenPhongBan':'CK Tiêu Hóa ','TenKhongDau':'CK Tieu Hoa ','TenPhongBan_En':'TH','Cap':2,'CapTren_Id':412,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':781,'MaPhongBan':'T4 - CXK','TenPhongBan':'Cơ Xương Khớp','TenKhongDau':'Co Xuong Khop','TenPhongBan_En':'CXK','Cap':2,'CapTren_Id':410,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':707,'MaPhongBan':'KB_RHM_D','TenPhongBan':'D','TenKhongDau':'D','TenPhongBan_En':'D','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':776,'MaPhongBan':'KB_KB_CKDY ','TenPhongBan':'Đông Y 1','TenKhongDau':'dong Y 1','TenPhongBan_En':'ĐY1','Cap':2,'CapTren_Id':414,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':878,'MaPhongBan':'KB_KB_CKDY 2','TenPhongBan':'Đông Y 2','TenKhongDau':'dong Y 2','TenPhongBan_En':'ĐY2','Cap':2,'CapTren_Id':414,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':910,'MaPhongBan':'KB_KB_CKDY 3','TenPhongBan':'Đông Y 3','TenKhongDau':'dong Y 3','TenPhongBan_En':'DY3','Cap':2,'CapTren_Id':414,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':786,'MaPhongBan':'KB_NTDTD1','TenPhongBan':'ĐTĐ - Đái Tháo Đường 1','TenKhongDau':'dTd - dai Thao duong 1','TenPhongBan_En':'ĐĐ1','Cap':2,'CapTren_Id':774,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':891,'MaPhongBan':'KB_NTDTD2','TenPhongBan':'ĐTĐ2 Đái Tháo Đường 2','TenKhongDau':'dTd2 dai Thao duong 2','TenPhongBan_En':'ĐĐ2','Cap':2,'CapTren_Id':774,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':700,'MaPhongBan':'KB_TMH_DTL-NL','TenPhongBan':'DTLNL - TMH Đo Thị Lực ,Nhị Lượng','TenKhongDau':'DTLNL - TMH do Thi Luc ,Nhi Luong','TenPhongBan_En':'K28','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':712,'MaPhongBan':'KB_RHM_F','TenPhongBan':'F','TenKhongDau':'F','TenPhongBan_En':'F','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':699,'MaPhongBan':'KB_TMH_KD','TenPhongBan':'KD - TMH Khí Dung','TenKhongDau':'KD - TMH Khi Dung','TenPhongBan_En':'K28','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':911,'MaPhongBan':'KB_CC_CKCC','TenPhongBan':'Khám dịch vụ cấp cứu','TenKhongDau':'Kham dich vu cap cuu','TenPhongBan_En':'CC','Cap':2,'CapTren_Id':429,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':871,'MaPhongBan':'KB_DVNA','TenPhongBan':'Khám Dịch Vụ Nội A','TenKhongDau':'Kham Dich Vu Noi A','TenPhongBan_En':'DVNA','Cap':2,'CapTren_Id':432,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':777,'MaPhongBan':'KB_KB_CKB2','TenPhongBan':'Khám Nhiệt đới T1','TenKhongDau':'Kham Nhiet doi T1','TenPhongBan_En':'211','Cap':2,'CapTren_Id':233,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':785,'MaPhongBan':'KBTYC_NGOAI4','TenPhongBan':'Khoa KBYC - Khám Ngoại 4','TenKhongDau':'Khoa KBYC - Kham Ngoai 4','TenPhongBan_En':'K01.9','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':658,'MaPhongBan':'KBTYC_NOI1','TenPhongBan':'Khoa KBYC - Khám Nội 1 - BS. Cứu','TenKhongDau':'Khoa KBYC - Kham Noi 1 - BS. Cuu','TenPhongBan_En':'K01.9','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':659,'MaPhongBan':'KBTYC_NOI2','TenPhongBan':'Khoa KBYC - Khám Nội 2 ','TenKhongDau':'Khoa KBYC - Kham Noi 2 ','TenPhongBan_En':'KYC2','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':660,'MaPhongBan':'KBTYC_NOI3','TenPhongBan':'Khoa KBYC - Khám Nội 3 ','TenKhongDau':'Khoa KBYC - Kham Noi 3 ','TenPhongBan_En':'YC3','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':661,'MaPhongBan':'KBTYC_RHM','TenPhongBan':'Khoa KBYC - Khám Răng Hàm Mặt','TenKhongDau':'Khoa KBYC - Kham Rang Ham Mat','TenPhongBan_En':'K01.9','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':531,'MaPhongBan':'KBTYC_TD','TenPhongBan':'Khoa KBYC - Phòng tiếp đón','TenKhongDau':'Khoa KBYC - Phong tiep don','TenPhongBan_En':'K01.9','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':870,'MaPhongBan':'KB_DL1','TenPhongBan':'Nội Đại Lải 1','TenKhongDau':'Noi dai Lai 1','TenPhongBan_En':'DL1','Cap':2,'CapTren_Id':432,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':694,'MaPhongBan':'KB_TMH_NS','TenPhongBan':'NS - TMH Nội Soi','TenKhongDau':'NS - TMH Noi Soi','TenPhongBan_En':'K28','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':729,'MaPhongBan':'RHM_TD','TenPhongBan':'P 306 - Tiếp đón','TenKhongDau':'Phong Tiep don(Khoa RHM)','TenPhongBan_En':'K29','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':714,'MaPhongBan':'KB_KM_PK9T','TenPhongBan':'P 308 - BS Cường','TenKhongDau':'P 308 - BS Cuong','TenPhongBan_En':'308','Cap':2,'CapTren_Id':415,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':689,'MaPhongBan':'KB_KM_TD2','TenPhongBan':'P 309 - Tiếp đón mắt','TenKhongDau':'P 309 - Tiep don mat','TenPhongBan_En':'309','Cap':2,'CapTren_Id':415,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':692,'MaPhongBan':'KB_KM_TK5','TenPhongBan':'P 310 - BS Hà','TenKhongDau':'P 310 - BS Ha','TenPhongBan_En':'310','Cap':2,'CapTren_Id':415,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':695,'MaPhongBan':'KB_KM_PK7L','TenPhongBan':'P 311 - BS Lan','TenKhongDau':'P 311 - BS Lan','TenPhongBan_En':'311','Cap':2,'CapTren_Id':415,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':713,'MaPhongBan':'KB_KM_PK7H','TenPhongBan':'P 314 - BS Hiền','TenKhongDau':'P 314 - BS Hien','TenPhongBan_En':'314','Cap':2,'CapTren_Id':415,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':702,'MaPhongBan':'KB_KM_9PKN','TenPhongBan':'P 315 - BS Nguyệt','TenKhongDau':'P 315 - BS Nguyet','TenPhongBan_En':'315','Cap':2,'CapTren_Id':415,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':725,'MaPhongBan':'PB','TenPhongBan':'PB - Phòng Băng - Khoa Ngoại','TenKhongDau':'PB - Phong Bang - Khoa Ngoai','TenPhongBan_En':'K19','Cap':2,'CapTren_Id':417,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':906,'MaPhongBan':'VCT1','TenPhongBan':'Phòng Chụp VCT 256','TenKhongDau':'Phong Chup VCT 256','TenPhongBan_En':'VCT1','Cap':2,'CapTren_Id':905,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':895,'MaPhongBan':'VCT','TenPhongBan':'Phòng Chụp VCT 64','TenKhongDau':'Phong Chup VCT 64','TenPhongBan_En':'VCT','Cap':2,'CapTren_Id':728,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':684,'MaPhongBan':'KB_TMH_HC','TenPhongBan':'Phòng Hành Chính (TMH)','TenKhongDau':'Phong Hanh Chinh (TMH)','TenPhongBan_En':'K28','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':257,'MaPhongBan':'YC1','TenPhongBan':'Phòng khám bệnh theo yêu cầu - Ngoại Trú','TenKhongDau':'Phong kham benh theo yeu cau - Ngoai Tru','TenPhongBan_En':'K01.9','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':750,'MaPhongBan':'PKHNT2','TenPhongBan':'Phòng Khám Hội Nghị Tổ 2','TenKhongDau':'Phong Kham Hoi Nghi To 2','TenPhongBan_En':'K01.3','Cap':2,'CapTren_Id':676,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':751,'MaPhongBan':'PKHNT3','TenPhongBan':'Phòng Khám Hội Nghị Tổ 3','TenKhongDau':'Phong Kham Hoi Nghi To 3','TenPhongBan_En':'K01.4','Cap':2,'CapTren_Id':677,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':752,'MaPhongBan':'PKHNT5','TenPhongBan':'Phòng Khám Hội Nghị Tổ 5','TenKhongDau':'Phong Kham Hoi Nghi To 5','TenPhongBan_En':'K01.5','Cap':2,'CapTren_Id':678,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':745,'MaPhongBan':'QHTW2','TenPhongBan':'Phòng Khám Quốc Hội TW2','TenKhongDau':'Phong Kham Quoc Hoi TW2','TenPhongBan_En':'K01.3','Cap':2,'CapTren_Id':676,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':753,'MaPhongBan':'QHTW3','TenPhongBan':'Phòng Khám Quốc Hội TW3','TenKhongDau':'Phong Kham Quoc Hoi TW3','TenPhongBan_En':'K01.4','Cap':2,'CapTren_Id':677,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':754,'MaPhongBan':'QHTW5','TenPhongBan':'Phòng Khám Quốc Hội TW5','TenKhongDau':'Phong Kham Quoc Hoi TW5','TenPhongBan_En':'K01.5','Cap':2,'CapTren_Id':678,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':913,'MaPhongBan':'KSKCB_KBTYC','TenPhongBan':'Phòng khám sức khỏe cán bộ CNVC ','TenKhongDau':'Phong kham suc khoe can bo CNVC ','TenPhongBan_En':'YC5','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':718,'MaPhongBan':'KB_KA_TD','TenPhongBan':'Phòng tiếp đón (Khám A)','TenKhongDau':'Phong tiep don (Kham A)','TenPhongBan_En':'K01.2','Cap':2,'CapTren_Id':432,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':681,'MaPhongBan':'KB_TMH_TD','TenPhongBan':'Phòng Tiếp Đón (Khoa TMH)','TenKhongDau':'Phong Tiep don (Khoa TMH)','TenPhongBan_En':'K28','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':674,'MaPhongBan':'KB_KB_TD','TenPhongBan':'Phòng Tiếp Đón(Khám B)','TenKhongDau':'Phong Tiep don(Kham B)','TenPhongBan_En':'K01.1','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':662,'MaPhongBan':'VLTL_PK01','TenPhongBan':'Phòng Tiếp Đón(Khoa VLTL)','TenKhongDau':'Phong Tiep don(Khoa VLTL)','TenPhongBan_En':'K31','Cap':2,'CapTren_Id':425,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':797,'MaPhongBan':'YTCQ_1','TenPhongBan':'Phòng Y tế Cơ quan','TenKhongDau':'Phong Y te Co quan','TenPhongBan_En':null,'Cap':2,'CapTren_Id':738,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':760,'MaPhongBan':'PKCTDT2','TenPhongBan':'PK Công Tác Đoàn TW2','TenKhongDau':'PK Cong Tac doan TW2','TenPhongBan_En':'K01.3','Cap':2,'CapTren_Id':676,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':759,'MaPhongBan':'DHDTW5','TenPhongBan':'PK Đại Hội Đảng TW5','TenKhongDau':'PK dai Hoi dang TW5','TenPhongBan_En':'K01.5','Cap':2,'CapTren_Id':678,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':724,'MaPhongBan':'PX','TenPhongBan':'PX - Phòng xương - Khoa Ngoại','TenKhongDau':'PX - Phong xuong - Khoa Ngoai','TenPhongBan_En':'K19','Cap':2,'CapTren_Id':417,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':795,'MaPhongBan':'SATYC','TenPhongBan':'Siêu âm (KCBTYC)','TenKhongDau':'Sieu am (KCBTYC)','TenPhongBan_En':'K01.9','Cap':2,'CapTren_Id':416,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':719,'MaPhongBan':'BVSKTW2','TenPhongBan':'T2- Phòng Khám BVSKTW2','TenKhongDau':'T2- Phong Kham BVSKTW2','TenPhongBan_En':'K01.3','Cap':2,'CapTren_Id':676,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':720,'MaPhongBan':'PBVSKTW3','TenPhongBan':'T3- Phòng Khám BVSKTW3','TenKhongDau':'T3- Phong Kham BVSKTW3','TenPhongBan_En':'K01.4','Cap':2,'CapTren_Id':677,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':721,'MaPhongBan':'PBVSKTW5','TenPhongBan':'T5- Phòng Khám BVSKTW5','TenKhongDau':'T5- Phong Kham BVSKTW5','TenPhongBan_En':'K01.5','Cap':2,'CapTren_Id':678,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':696,'MaPhongBan':'KB_TMH_TK','TenPhongBan':'TK - TMH Trưởng Khoa','TenKhongDau':'TK - TMH Truong Khoa','TenPhongBan_En':'K28','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':747,'MaPhongBan':'KB_KB_CXK_DU','TenPhongBan':'Ung Bướu','TenKhongDau':'Ung Buou','TenPhongBan_En':'UB','Cap':2,'CapTren_Id':771,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':903,'MaPhongBan':'T6UB','TenPhongBan':'Ung Bướu Tầng 6','TenKhongDau':'Ung Buou Tang 6','TenPhongBan_En':'T6UB','Cap':2,'CapTren_Id':771,'LoaiPhongBan':'KhamBenh'},{'PhongBan_Id':880,'MaPhongBan':'CHT','TenPhongBan':'Phòng Chụp Cộng hưởng từ (Tầng 2 - Nhà số 10)','TenKhongDau':'Phong Chup Cong huong tu (Tang 2 - Nha so 10)','TenPhongBan_En':'K39','Cap':2,'CapTren_Id':227,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':792,'MaPhongBan':'X_Q','TenPhongBan':'Phòng chụp X Quang (Tầng 2 - Nhà số 10)','TenKhongDau':'Phong chup X Quang (Tang 2 - Nha so 10)','TenPhongBan_En':'K39','Cap':2,'CapTren_Id':227,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':717,'MaPhongBan':'DLX','TenPhongBan':'Phòng đo độ loãng xương (Tầng 2 - Nhà 10)','TenKhongDau':'Phong do do loang xuong (Tang 2 - Nha 10)','TenPhongBan_En':'K06','Cap':2,'CapTren_Id':410,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':737,'MaPhongBan':'LAS','TenPhongBan':'Phòng Laser - Khoa khám bệnh B','TenKhongDau':'Phong Laser - Khoa kham benh B','TenPhongBan_En':'K01.1','Cap':2,'CapTren_Id':433,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':881,'MaPhongBan':'PLM','TenPhongBan':'Phòng lấy máu (Tầng 1 - Nhà số 7)','TenKhongDau':'Phong lay mau (Tang 1 - Nha so 7)','TenPhongBan_En':'K37','Cap':2,'CapTren_Id':419,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':912,'MaPhongBan':'PSDU','TenPhongBan':'Phòng nội soi hô hấp dị ứng','TenKhongDau':'Phong noi soi ho hap di ung','TenPhongBan_En':'K09.2','Cap':2,'CapTren_Id':408,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':885,'MaPhongBan':'PSA','TenPhongBan':'Phòng nội soi Nội A','TenKhongDau':'Phong noi soi Noi A','TenPhongBan_En':'K03.1','Cap':2,'CapTren_Id':431,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':794,'MaPhongBan':'NSTMH','TenPhongBan':'Phòng nội soi tai mũi họng','TenKhongDau':'Phong noi soi tai mui hong','TenPhongBan_En':'K28','Cap':2,'CapTren_Id':426,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':727,'MaPhongBan':'PS','TenPhongBan':'Phòng nội soi tiêu hóa','TenKhongDau':'Phong noi soi tieu hoa','TenPhongBan_En':'K05','Cap':2,'CapTren_Id':412,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':791,'MaPhongBan':'SANGT','TenPhongBan':'Phòng Siêu âm ngoại trú (Tầng 2 - Nhà số 10) ','TenKhongDau':'Phong Sieu am ngoai tru (Tang 2 - Nha so 10) ','TenPhongBan_En':'K39','Cap':2,'CapTren_Id':227,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':898,'MaPhongBan':'SANA','TenPhongBan':'Phòng siêu âm Nội A','TenKhongDau':'Phong sieu am Noi A','TenPhongBan_En':'K03.1','Cap':2,'CapTren_Id':431,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':790,'MaPhongBan':'SANT','TenPhongBan':'Phòng Siêu âm Nội trú (Tầng 2 - Nhà số 10)','TenKhongDau':'Phong Sieu am Noi tru (Tang 2 - Nha so 10)','TenPhongBan_En':'K39','Cap':2,'CapTren_Id':227,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':715,'MaPhongBan':'SAM','TenPhongBan':'Sieu am mat','TenKhongDau':'Sieu am mat','TenPhongBan_En':'K30','Cap':2,'CapTren_Id':415,'LoaiPhongBan':'KhoaCLS'},{'PhongBan_Id':716,'MaPhongBan':'XQR','TenPhongBan':'XQ Rang','TenKhongDau':'XQ Rang','TenPhongBan_En':'K29','Cap':2,'CapTren_Id':430,'LoaiPhongBan':'KhoaCLS'}]";
                //List<newObj> newObjs = JsonConvert.DeserializeObject<List<newObj>>(json);
                //newObjs = newObjs.Where(x => x.LoaiPhongBan == "KhamBenh").OrderBy(x => x.PhongBan_Id).ToList();
                //string query = "";
                //foreach (var item in newObjs)
                //{
                //    query += " INSERT [dbo].[Q_Major] (  [Name], [Note], [IsDeleted], [IsShow]) VALUES (  N'"+item.TenPhongBan+"', N'', 0, 0)";
                //    query += " INSERT [dbo].[Q_Service] (  [Name], [StartNumber], [EndNumber], [TimeProcess], [Note], [IsDeleted], [IsActived], [Code], [showBenhVien], [autoend], [TimeAutoEnd], [isKetLuan]) VALUES (  N'"+item.TenPhongBan+"', 1000, 1999, CAST(0x0000A8AA00041EB0 AS DateTime), N' ', 0, 1, N'"+item.MaPhongBan+"', 0, 0, NULL, 0)";
                //}

                //foreach (var item in newObjs)
                //{
                //    query += " INSERT [dbo].[Q_Major] (  [Name], [Note], [IsDeleted], [IsShow]) VALUES (  N'" + item.TenPhongBan + "-KL', N'', 0, 0)";
                //    query += " INSERT [dbo].[Q_Service] (  [Name], [StartNumber], [EndNumber], [TimeProcess], [Note], [IsDeleted], [IsActived], [Code], [showBenhVien], [autoend], [TimeAutoEnd], [isKetLuan]) VALUES ( N'" + item.TenPhongBan + "', 1000, 1999, CAST(0x0000A8AA00041EB0 AS DateTime), N' ', 0, 1, N'" + item.MaPhongBan + "', 0, 0, NULL, 1)";
                //}
                //   BLLSQLBuilder.Instance.Excecute(connectString, query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Form form = new FrmSQLConnect();
                form.ShowDialog();
            }
        }

        /// <summary>
        /// khoi tao com cho may in ko su dung board in phieu
        /// </summary>
        private void InitCOM_Printer()
        {
            try
            {
                COM_Printer.PortName = GetConfigByCode(eConfigCode.COM_Print);
                COM_Printer.BaudRate = 9600;
                COM_Printer.DataBits = 8;
                COM_Printer.Parity = Parity.None;
                COM_Printer.StopBits = StopBits.One;

                try
                {
                    COM_Printer.ReadTimeout = 1;
                    COM_Printer.WriteTimeout = 1;
                    COM_Printer.Open();
                    barButtonItem14.Glyph = global::QMS_System.Properties.Resources.iconfinder_printer_remote_30279;
                    //  COM_Printer.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived); 
                }
                catch (Exception)
                {
                    // MessageBox.Show("Lỗi: không thể kết nối với cổng COM Máy in, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Lấy thông tin Com Máy in bị lỗi.\n" + ex.Message, "Lỗi Com Máy in");
            }
        }

        private Form IsActive(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == ftype)
                {
                    return f;
                }
            }
            return null;
        }

        public string GetConfigByCode(string code)
        {
            if (configs != null && configs.Count > 0)
            {
                var obj = configs.FirstOrDefault(x => x.Code.Trim().ToUpper().Equals(code.Trim().ToUpper()));
                return obj != null ? obj.Value : string.Empty;
            }
            return string.Empty;
        }

        #region Event
        private void btnNV_CMD_ReadSound_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form frm = IsActive(typeof(frmUserCommandReadSound));
            if (frm == null)
            {
                frmUserCommandReadSound forms = new frmUserCommandReadSound();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }
        private void btnRegisterUserCmd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form frm = IsActive(typeof(frmUserCmdRegister));
            if (frm == null)
            {
                frmUserCmdRegister forms = new frmUserCmdRegister();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }
        private void btnUserMajor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmUserMajor));
            if (frm == null)
            {
                frmUserMajor forms = new frmUserMajor();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }
        private void btnStatus_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmStatus));
            if (frm == null)
            {
                frmStatus forms = new frmStatus();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnService_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmService));
            if (frm == null)
            {
                frmService forms = new frmService();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnBusiness_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmBusiness));
            if (frm == null)
            {
                frmBusiness forms = new frmBusiness();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnUser_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmUser));
            if (frm == null)
            {
                frmUser forms = new frmUser();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnPolicy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmPolicy));
            if (frm == null)
            {
                frmPolicy forms = new frmPolicy();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnCounter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmCounter));
            if (frm == null)
            {
                frmCounter forms = new frmCounter();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnLoginHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmLoginHistory));
            if (frm == null)
            {
                frmLoginHistory forms = new frmLoginHistory();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnEquipment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmEquipment));
            if (frm == null)
            {
                frmEquipment forms = new frmEquipment();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnCommand_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmCommand));
            if (frm == null)
            {
                frmCommand forms = new frmCommand();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnAction_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmAction));
            if (frm == null)
            {
                frmAction forms = new frmAction();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnEquipTypeProcess_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmEquipTypeProcess));
            if (frm == null)
            {
                frmEquipTypeProcess forms = new frmEquipTypeProcess();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmMajor));
            if (frm == null)
            {
                frmMajor forms = new frmMajor();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnConfig_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmConfig));
            if (frm == null)
            {
                frmConfig forms = new frmConfig();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnSystem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmConfig));
            if (frm == null)
            {
                frmConfig forms = new frmConfig();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnSound_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmSound));
            if (frm == null)
            {
                frmSound forms = new frmSound();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmLanguage));
            if (frm == null)
            {
                frmLanguage forms = new frmLanguage();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnReadTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmReadTemplate));
            if (frm == null)
            {
                frmReadTemplate forms = new frmReadTemplate();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }
        private void btnPrinterWindow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmIssueTicketScreen frm = new frmIssueTicketScreen(null, this);
            frm.StartPosition = FormStartPosition.CenterScreen;
            // frm.TopMost = true;
            frm.Show();
        }

        private void btnDeleteTicketInDay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa hết các vé cấp trong ngày không?.", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
                if (!BLLDailyRequire.Instance.DeleteAllTicketInDay(connectString, today))
                    MessageBox.Show("Lỗi thực thi yêu cầu Xóa vé đã cấp trong ngày. Vui lòng kiểm tra lại.", "Lỗi thực thi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    IsDatabaseChange = true;
                    //ko su dung GO 
                    var query = @"  UPDATE [Q_COUNTER] set lastcall = 0 ,isrunning =1, CurrentNumber=0 , LastCallKetLuan= 0  
                                        DELETE [Q_CounterSoftSound]  
                                        DBCC CHECKIDENT('Q_CounterSoftSound', RESEED, 1);                                             
                                        DELETE [dbo].[Q_RequestTicket]  
                                        DBCC CHECKIDENT('Q_RequestTicket', RESEED, 1); 
                                        DELETE [dbo].[Q_TVReadSound]  
                                        DBCC CHECKIDENT('Q_TVReadSound', RESEED, 1); 
                                        DELETE [Q_DailyRequire_WorkDetail]  
                                        DBCC CHECKIDENT('Q_DailyRequire_WorkDetail', RESEED, 1);
                                        UPDATE [Q_COUNTERDAYINFO] set [STT] = 0, [STT_3]='0',[STT_QMS]=0 ,[StatusSTT] = 0 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL     ";
                    BLLSQLBuilder.Instance.Excecute(connectString, query);
                    for (int i = 0; i < strEquipmentIds.Count; i++)
                    {
                        dataSendToComport.Add(("AA " + strEquipmentIds[i] + " 01 00 00")); //tra counter 0
                        dataSendToComport.Add(("AA " + strEquipmentIds[i] + " 02 00 00")); // tra gio 0
                    }
                }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void btnShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form frm = IsActive(typeof(frmShift));
            if (frm == null)
            {
                frmShift forms = new frmShift();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnServiceShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmServiceShift));
            if (frm == null)
            {
                frmServiceShift forms = new frmServiceShift();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnRDetailDay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmR_DetailInDay));
            if (frm == null)
            {
                var forms = new frmR_DetailInDay();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnRAllDay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form frm = IsActive(typeof(frmR_GeneralInDay));
            if (frm == null)
            {
                var forms = new frmR_GeneralInDay();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnRDetailByTime_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmR_DetailByTimeRange));
            if (frm == null)
            {
                var forms = new frmR_DetailByTimeRange();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnRAllByTime_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmR_GeneralByTimeRange));
            if (frm == null)
            {
                var forms = new frmR_GeneralByTimeRange();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnR_Cus_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Form frm = IsActive(typeof(frmR_ReportByBusiness));
            //if (frm == null)
            //{
            //    var forms = new frmR_ReportByBusiness();
            //    forms.MdiParent = this;
            //    forms.Show();
            //}
            //else
            //    frm.Activate();
        }

        #endregion

        #region Sound
        public void PlaySound()
        {
            try
            {
                while (true)
                    if (temp.Count > 0)
                    {
                        if (isFinishRead)
                        {
                            isFinishRead = false;
                            ReadSound(temp);
                        }
                    }
            }
            catch (Exception)
            {
            }
        }

        private void barbtmauphieu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //  Form frm = IsActive(typeof(frmTicketTemplate));
            Form frm = IsActive(typeof(frmPrintSetting));
            if (frm == null)
            {
                //var forms = new frmTicketTemplate();
                var forms = new frmPrintSetting();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string content = "nguyễn hoàng hải";
            // BaseCore.Instance.PrintTicketTCVN3(COM_Printer, content);
            //logText = "";
            //Form frm = IsActive(typeof(testfrom));
            //if (frm == null)
            //{
            //    var forms = new testfrom();
            //    forms.MdiParent = this;
            //    forms.Show();
            //}
            //else
            //    frm.Activate();
        }

        private void barButtonCOMSetup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmCOMSetting));
            if (frm == null)
            {
                var forms = new frmCOMSetting();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void barServiceDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmWork));
            if (frm == null)
            {
                var forms = new frmWork();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void timerCapNhatGioCounter_Tick(object sender, EventArgs e)
        {
            var counters = BLLTivi.Instance.Gets(connectString, equipmentIds);
            if (counters != null && counters.Count > 0)
            {
                List<string> arrStr;
                for (int i = 0; i < counters.Count; i++)
                {
                    try
                    {
                        switch (counters[i].TrangThai)
                        {
                            case "Process":
                                // arrStr = BaseCore.Instance.ChangeNumber(counters[i].STT);
                                // dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 01 " + arrStr[0] + " " + arrStr[1])); //stt
                                string[] gio = counters[i].TGConLai.Value.ToString(@"hh\:mm").Split(':').ToArray();
                                dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 02 " + gio[0] + " " + gio[1])); //gio countdown
                                logText += "process \n" + ("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 02 00 00 \n");
                                break;
                            case "Complete":
                                //  arrStr = BaseCore.Instance.ChangeNumber(counters[i].STT);
                                //  dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 01 " + arrStr[0] + " " + arrStr[1])); //stt 
                                dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 03 00 00")); //hoan thanh
                                logText += "complete \n" + ("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 03 00 00 \n");
                                break;
                            case "Over":
                                //  arrStr = BaseCore.Instance.ChangeNumber(counters[i].STT);
                                // dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 01 " + arrStr[0] + " " + arrStr[1])); //stt 
                                logText += "Over \n" + ("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 04 00 00 \n");
                                dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(counters[i].EquipCode) + " 04 00 00")); //phat sinh
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            arrStr = BaseCore.Instance.ChangeNumber(so);
            dataSendToComport.Add(("AA 01 01 " + arrStr[0] + " " + arrStr[1])); //stt
            arrStr = BaseCore.Instance.ChangeNumber(gio);
            dataSendToComport.Add(("AA 01 02 " + arrStr[0] + " " + arrStr[1])); // gio
            so++;
            gio++;
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (player == null)
                    player = new SoundPlayer();
                player.SoundLocation = ((soundPath + txtDeleteNumber.EditValue.ToString()));
                player.Play();
            }
            catch (Exception ex)
            {
                errorsms = ex.Message;
            }
        }

        private void btnDeleteTicket_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BLLDailyRequire.Instance.DeleteTicket(connectString, int.Parse(txtDeleteNumber.EditValue.ToString()), today) == 0)
                errorsms = "Không tìm thấy được số phiếu cần hủy. Vui lòng kiểm tra lại.";
            else
                errorsms = "Hủy phiếu thành công.";
        }

        private void btnConnectSQL_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(FrmSQLConnect));
            //  if (frm == null)
            //  {
            var forms = new FrmSQLConnect();
            // forms.MdiParent = this;
            forms.Show();
            //  }
            //  else
            //     frm.Activate();
        }

        private void ReadSound(List<string> sounds)
        {
            if (temp.Count > 0)
            {
                while (temp.Count > 0)
                {
                    try
                    {
                        if (File.Exists(soundPath + temp[0]))
                        {
                            player.SoundLocation = (soundPath + temp[0]);

                            // MessageBox.Show(SoundInfo.GetSoundLength(player.SoundLocation.Trim()).ToString());
                            int iTime = SoundInfo.GetSoundLength(player.SoundLocation.Trim()) + silenceTime;
                            player.Play();
                            Thread.Sleep(iTime);
                            temp.Remove(temp[0]);
                        }
                        else
                        {
                            temp.Remove(temp[0]);
                            errorsms = "Không tìm được tệp âm thanh " + temp[0] + " ngưng cấp phiếu dịch vụ. Vui lòng kiểm tra lại đường dẫn thư mục âm thanh hoặc tệp âm thanh không tồn tại.";
                        }
                    }
                    catch (Exception)
                    { }
                }
            }
            isFinishRead = true;
            playThread.Abort();
            //   barButtonItem8.Caption = "ThreadPlay Abort";
        }

        private void GetSound(List<int> templateIds, string ticket, int counterId)
        {
            var readDetails = lib_ReadTemplates.Where(x => templateIds.Contains(x.Id)).ToList(); // BLLReadTempDetail.Instance.Gets(templateIds);
            if (readDetails.Count > 0)
            {
                //  var lib_Sounds = BLLSound.Instance.Gets(connect);
                SoundModel soundFound;
                CounterSoundModel ctSound;
                var soundStr = string.Empty;
                if (lib_Sounds.Count > 0)
                {
                    playlist.Clear();
                    for (int y = 0; y < readDetails.Count; y++)
                    {
                        if (readDetails[y].Details.Count > 0)
                        {
                            for (int i = 0; i < readDetails[y].Details.Count; i++)
                            {
                                if (readDetails[y].Details[i].SoundId == (int)eSoundConfig.Ticket)
                                {
                                    for (int j = 0; j < ticket.Length; j++)
                                    {
                                        soundFound = lib_Sounds.FirstOrDefault(x => x.Code != null && x.Code.Equals(ticket[j] + "") && x.LanguageId == readDetails[y].LanguageId);
                                        if (soundFound != null)
                                        {
                                            playlist.Add(soundFound.Name);
                                            soundStr += soundFound.Name + "|";
                                        }
                                    }
                                }
                                else if (readDetails[y].Details[i].SoundId == (int)eSoundConfig.Counter)
                                {
                                    ctSound = lib_CounterSound.FirstOrDefault(x => x.CounterId == counterId && x.LanguageId == readDetails[y].LanguageId);
                                    // playlist.Add((ctSound == null ? "" : ctSound.SoundName));//  BLLCounterSound.Instance.GetSoundName(counterId, ));
                                    if (ctSound != null)
                                    {
                                        playlist.Add(ctSound.SoundName);
                                        soundStr += ctSound.SoundName + "|";
                                    }
                                }
                                else
                                {
                                    soundFound = lib_Sounds.FirstOrDefault(x => x.Id == readDetails[y].Details[i].SoundId);
                                    if (soundFound != null)
                                    {
                                        playlist.Add(soundFound.Name);
                                        soundStr += soundFound.Name + "|";
                                    }
                                }
                            }
                        }
                    }
                    temp.AddRange(playlist);
                    if (!string.IsNullOrEmpty(soundStr) && TVReadSound == 1)
                    {
                        soundStr = soundStr.Substring(0, soundStr.Length - 1);
                        BLLCounterSoftRequire.Instance.Insert(connectString, soundStr, (int)eCounterSoftRequireType.TVReadSound, counterId);
                    }
                }
            }
        }

        private void btnServiceLimit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmServiceLimit));
            if (frm == null)
            {
                frmServiceLimit forms = new frmServiceLimit();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }
        #endregion

        #region Keypad COMPort
        private void InitComPort()
        {
            try
            {
                comPort.PortName = GetConfigByCode(eConfigCode.ComportName);
                comPort.BaudRate = 9600;
                comPort.DataBits = 8;
                comPort.Parity = Parity.None;
                comPort.StopBits = StopBits.One;
                try
                {
                    comPort.Open();
                    barButtonItem8.Glyph = global::QMS_System.Properties.Resources.if_port_64533;
                    comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
                }
                catch (Exception)
                {
                    // MessageBox.Show("Lỗi: không thể kết nối với cổng COM Keypad, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Lấy thông tin Com Keypad bị lỗi.\n" + ex.Message, "Lỗi Com Keypad");
            }
        }

        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(RecieverData));
        }

        private void RecieverData(object sender, EventArgs e)
        {
            try
            {
                int length = comPort.BytesToRead;
                if (length > 0)
                {
                    byte[] buf = new byte[length];
                    comPort.Read(buf, 0, length);
                    logText += "length của byte nhận dc : " + length + "\n";
                    logText += BitConverter.ToString(buf) + "\n";

                    var hexStr = BitConverter.ToString(buf).Split('-').ToArray();
                    if (hexStr.Length == 5)
                    {
                        tempHex.Clear();
                        hasAA = false;
                    }
                    else
                    {
                        if (String.Join("-", hexStr).IndexOf("AA") > 0)
                            if (hasAA)
                            {
                                tempHex.Clear();
                                tempHex = hexStr.ToList();
                            }
                            else
                            {
                                hasAA = false;
                                tempHex = tempHex.Concat(hexStr).ToList();
                                hexStr = tempHex.ToArray();
                                tempHex.Clear();
                            }
                        else
                        {
                            hasAA = false;
                            tempHex = tempHex.Concat(hexStr).ToList();
                            logText += "tempHex :" + string.Join(" ", tempHex) + "\n";
                            hexStr = tempHex.ToArray();
                            if (hexStr.Length >= 5)
                                tempHex.Clear();
                        }
                    }
                    lbRecieve.Caption = hexStr[1] + "," + hexStr[2];
                    logText += "hex cuối :" + string.Join(" ", hexStr) + "\n";
                    //  du 5 byte moi work
                    if (hexStr.Length == 5)
                    {
                        logText += "hex ok :" + string.Join(" ", hexStr) + "\n";
                        lib_Users = BLLLoginHistory.Instance.GetsForMain(connectString);
                        switch (BLLEquipment.Instance.GetEquipTypeId(connectString, int.Parse(hexStr[1])))
                        {
                            case (int)eEquipType.Counter:
                                if (hexStr.Contains("8C") && _8cUseFor == 1)   ///Login - Logout
                                {
                                    //  MessageBox.Show("VO 8C");
                                    var num = BLLLoginHistory.Instance.CounterLoginLogOut(connectString, int.Parse((hexStr[3] == "FF" ? "00" : hexStr[3]) + "" + hexStr[4]), int.Parse(hexStr[1]), DateTime.Now);
                                    var arrStr = BaseCore.Instance.ChangeNumber(num);
                                    dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));

                                    IsDatabaseChange = true;
                                }
                                else if (hexStr.Length >= 4 || hexStr.Contains("8C"))
                                {
                                    //  MessageBox.Show("VO COUNTER");
                                    CounterProcess(hexStr, 0);
                                }
                                break;
                            case (int)eEquipType.Printer:
                                switch (hexStr[2])
                                {
                                    case "0A": hexStr[2] = "10"; break;
                                    case "0B": hexStr[2] = "11"; break;
                                    case "0C": hexStr[2] = "12"; break;
                                    case "0D": hexStr[2] = "13"; break;
                                    case "0E": hexStr[2] = "14"; break;
                                    case "0F": hexStr[2] = "15"; break;
                                    case "10": hexStr[2] = "16"; break;
                                    case "11": hexStr[2] = "17"; break;
                                    case "12": hexStr[2] = "18"; break;
                                    case "13": hexStr[2] = "19"; break;
                                    case "14": hexStr[2] = "20"; break;
                                    case "15": hexStr[2] = "21"; break;
                                    case "16": hexStr[2] = "22"; break;
                                    case "17": hexStr[2] = "23"; break;
                                    case "18": hexStr[2] = "24"; break;
                                    case "19": hexStr[2] = "25"; break;
                                }

                                if (int.Parse(hexStr[2].ToString()) == 30)
                                {
                                    if (!isErorr)
                                    {
                                        isErorr = true;
                                        errorsms = "Lỗi: Máy in phiếu đang bị lỗi. Xin vui lòng kiểm tra lại máy in . Xin cám ơn.!";
                                    }
                                }
                                else
                                {
                                    isErorr = false;
                                    errorsms = "";
                                    PrintNewTicket(int.Parse(hexStr[1]), (hexStr[2] == "0A" ? 10 : int.Parse(hexStr[2])), 0, false, false, null, null, null, null, null, null, null, null, null, null);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public bool PrintNewTicket(int printerId,
           int serviceId,
           int businessId,
           bool isTouchScreen,
           bool isProgrammer,
           TimeSpan? timeServeAllow,
           string Name,
           string Address,
           int? DOB,
            string MaBenhNhan,
           string MaPhongKham,
           string SttPhongKham,
           string SoXe,
           string MaCongViec,
           string MaLoaiCongViec
           )
        {
            try
            {
                IsDatabaseChange = true;
                int lastTicket = 0,
                    newNumber = 0,
                nghiepVu = 0;
                string printStr = string.Empty,
                    tenQuay = string.Empty;
                bool err = false;
                ServiceDayModel serObj = null;
                DateTime now = DateTime.Now;

                var printModel = new PrintModel();
                switch (printType)
                {
                    case (int)ePrintType.TheoTungDichVu:
                        #region
                        serObj = lib_Services.FirstOrDefault(x => x.Id == serviceId);
                        if (serObj == null)
                            errorsms = "Dịch vụ số " + serviceId + " không tồn tại. Xin quý khách vui lòng chọn dịch vụ khác.";
                        else
                        {
                            if (CheckTimeBeforePrintTicket == "1" && serObj.Shifts.FirstOrDefault(x => now.TimeOfDay >= x.Start.TimeOfDay && now.TimeOfDay <= x.End.TimeOfDay) == null)
                                temp.Add(SoundLockPrintTicket);
                            else
                            {
                                printModel.TenDichVu = (serObj != null ? serObj.Name : "");
                                printModel.NoteDV = (serObj != null ? serObj.Note : "");
                                var rs = BLLDailyRequire.Instance.PrintNewTicket(connectString, serviceId, serObj.StartNumber, businessId, now, printType, (timeServeAllow != null ? timeServeAllow.Value : serObj.TimeProcess.TimeOfDay), Name, Address, DOB, MaBenhNhan, MaPhongKham, SttPhongKham, SoXe, MaCongViec, MaLoaiCongViec);
                                if (rs.IsSuccess)
                                {
                                    if (!isProgrammer)
                                    {
                                        var soArr = BaseCore.Instance.ChangeNumber(((int)rs.Data + 1));
                                        printStr = (soArr[0] + " " + soArr[1] + " ");
                                        if (printTicketReturnCurrentNumberOrServiceCode == 1)
                                        {
                                            soArr = BaseCore.Instance.ChangeNumber((int)rs.Records);
                                        }
                                        else
                                        {
                                            soArr = BaseCore.Instance.ChangeNumber(serviceId);
                                        }
                                        printStr += (soArr[0] + " " + soArr[1] + " " + now.ToString("dd") + " " + now.ToString("MM") + " " + now.ToString("yy") + " " + now.ToString("HH") + " " + now.ToString("mm"));
                                    }
                                    else if (isProgrammer)
                                        lbRecieve.Caption = printerId + "," + serviceId + "," + ((int)rs.Data + 1);
                                    barButtonItem10.Caption = "đang gọi :" + (int)rs.Records;
                                    nghiepVu = rs.Data_1;

                                    printModel.TenQuay = rs.Data_2;
                                    printModel.STT = ((int)rs.Data + 1);
                                    printModel.STTHienTai = ((int)rs.Records);
                                    printModel.SoXe = SoXe;
                                    printModel.MaKH = MaBenhNhan;
                                    printModel.TenKH = Name;
                                    printModel.DOB = DOB ?? 0;
                                    printModel.DiaChi = Address;
                                    printModel.MaDV = MaPhongKham;
                                    printModel.Phone = "";
                                }
                                else
                                    errorsms = rs.Errors[0].Message;
                                //   MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        #endregion
                        break;
                    case (int)ePrintType.BatDauChung:
                        #region MyRegion
                        serObj = lib_Services.FirstOrDefault(x => x.Id == serviceId);
                        if (serObj == null)
                            errorsms = "Dịch vụ số " + serviceId + " không tồn tại. Xin quý khách vui lòng chọn dịch vụ khác.";
                        else
                        {
                            if (CheckTimeBeforePrintTicket == "1" && serObj.Shifts.FirstOrDefault(x => now.TimeOfDay >= x.Start.TimeOfDay && now.TimeOfDay <= x.End.TimeOfDay) == null)
                                temp.Add(SoundLockPrintTicket);
                            else
                            {
                                printModel.TenDichVu = (serObj != null ? serObj.Name : "");
                                printModel.NoteDV = (serObj != null ? serObj.Note : "");
                                var rs = BLLDailyRequire.Instance.PrintNewTicket(connectString, serviceId, startNumber, businessId, now, printType, (timeServeAllow != null ? timeServeAllow.Value : serObj.TimeProcess.TimeOfDay), Name, Address, DOB, MaBenhNhan, MaPhongKham, SttPhongKham, SoXe, MaCongViec, MaLoaiCongViec);
                                if (rs.IsSuccess)
                                {
                                    if (!isProgrammer)
                                    {
                                        var soArr = BaseCore.Instance.ChangeNumber(((int)rs.Data + 1));
                                        printStr = (soArr[0] + " " + soArr[1] + " ");
                                        if (printTicketReturnCurrentNumberOrServiceCode == 1)
                                        {
                                            soArr = BaseCore.Instance.ChangeNumber((int)rs.Records);
                                        }
                                        else
                                        {
                                            soArr = BaseCore.Instance.ChangeNumber(serviceId);
                                        }
                                        printStr += (soArr[0] + " " + soArr[1] + " " + now.ToString("dd") + " " + now.ToString("MM") + " " + now.ToString("yy") + " " + now.ToString("HH") + " " + now.ToString("mm"));
                                    }
                                    else if (isProgrammer)
                                        lbRecieve.Caption = printerId + "," + serviceId + "," + ((int)rs.Data + 1);
                                    nghiepVu = rs.Data_1;

                                    printModel.TenQuay = rs.Data_2;
                                    printModel.STT = ((int)rs.Data + 1);
                                    printModel.STTHienTai = ((int)rs.Records);
                                    printModel.SoXe = SoXe;
                                    printModel.MaKH = MaBenhNhan;
                                    printModel.TenKH = Name;
                                    printModel.DOB = DOB ?? 0;
                                    printModel.DiaChi = Address;
                                    printModel.MaDV = MaPhongKham;
                                    printModel.Phone = "";
                                }
                                else
                                    errorsms = rs.Errors[0].Message;
                                //  MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        #endregion
                        break;
                    case (int)ePrintType.TheoGioiHanSoPhieu:
                        #region MyRegion
                        int slCP = BLLBusiness.Instance.GetTicketAllow(connectString, businessId);
                        int slDacap = BLLDailyRequire.Instance.CountTicket(connectString, businessId);
                        if (slDacap != null && slDacap == slCP)
                            errorsms = ("Doanh nghiệp của bạn đã được cấp đủ số lượng phiếu giới hạn trong ngày. Xin quý khách vui lòng quay lại sau.");
                        //  MessageBox.Show("Doanh nghiệp của bạn đã được cấp đủ số lượng phiếu giới hạn trong ngày. Xin quý khách vui lòng quay lại sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            lastTicket = BLLDailyRequire.Instance.GetLastTicketNumber(connectString, serviceId, today);
                            serObj = lib_Services.FirstOrDefault(x => x.Id == serviceId);
                            if (lastTicket == 0)
                            {
                                if (serObj != null)
                                {
                                    err = true;
                                    errorsms = ("Dịch vụ không tồn tại. Xin quý khách vui lòng chọn dịch vụ khác.");
                                    //  MessageBox.Show("Dịch vụ không tồn tại. Xin quý khách vui lòng chọn dịch vụ khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                    lastTicket = serObj.StartNumber;
                            }
                            else
                            {
                                lastTicket++;
                                if (serObj.EndNumber < lastTicket)
                                {
                                    err = true;
                                    errorsms = ("Dịch vụ này đã cấp hết phiếu trong ngày. Xin quý khách vui lòng chọn dịch vụ khác.");
                                    //  MessageBox.Show("Dịch vụ này đã cấp hết phiếu trong ngày. Xin quý khách vui lòng chọn dịch vụ khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            if (!err)
                            {
                                printModel.TenDichVu = (serObj != null ? serObj.Name : "");
                                printModel.NoteDV = (serObj != null ? serObj.Note : "");
                                var rs = BLLDailyRequire.Instance.Insert(connectString, lastTicket, serviceId, businessId, now, MaCongViec, MaLoaiCongViec);
                                if (rs.IsSuccess)
                                {
                                    newNumber = rs.Data;
                                    if (newNumber != 0 && !isProgrammer)
                                    {
                                        var soArr = BaseCore.Instance.ChangeNumber(lastTicket);
                                        printStr = (soArr[0] + " " + soArr[1] + " ");
                                        if (printTicketReturnCurrentNumberOrServiceCode == 1)
                                        {
                                            soArr = BaseCore.Instance.ChangeNumber(BLLDailyRequire.Instance.GetCurrentNumber(connectString, serviceId));
                                        }
                                        else
                                        {
                                            soArr = BaseCore.Instance.ChangeNumber(serviceId);
                                        }

                                        printStr += (soArr[0] + " " + soArr[1] + " " + now.ToString("dd") + " " + now.ToString("MM") + " " + now.ToString("yy") + " " + now.ToString("HH") + " " + now.ToString("mm"));
                                    }
                                    else if (newNumber != 0 && isProgrammer)
                                        lbRecieve.Caption = serviceId + "," + "1," + lastTicket;
                                    nghiepVu = rs.Data_1;

                                    printModel.TenQuay = rs.Data_2;
                                    printModel.STT = ((int)rs.Data);
                                    printModel.STTHienTai = lastTicket;
                                    printModel.SoXe = SoXe;
                                    printModel.MaKH = MaBenhNhan;
                                    printModel.TenKH = Name;
                                    printModel.DOB = DOB ?? 0;
                                    printModel.DiaChi = Address;
                                    printModel.MaDV = MaPhongKham;
                                    printModel.Phone = "";
                                }
                            }
                        }
                        #endregion
                        break;
                }

                if (!string.IsNullOrEmpty(printStr))
                {
                    errorsms = printStr.ToString();
                    if (UsePrintBoard == 1)
                    {
                        if (isTouchScreen)
                            dataSendToComport.Add("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(printerId));
                        dataSendToComport.Add(printStr);
                    }
                    else
                    {
                        PrintWithNoBorad(printModel);
                    }
                }
                else if (printModel.STT != 0)
                    if (UsePrintBoard == 0)
                        PrintWithNoBorad(printModel);

                if (AutoCallIfUserFree == 1 && nghiepVu > 0)
                {
                    var freeUser = (int)BLLDailyRequire.Instance.CheckUserFree(connectString, nghiepVu, serviceId, newNumber, autoCallFollowMajorOrder).Data;
                    if (freeUser > 0)
                    {
                        var counter = lib_Users.FirstOrDefault(x => x.UserId == freeUser).EquipCode;
                        var str = ("AA," + BaseCore.Instance.ConvertIntToStringWith0Number(counter) + ",8B,00,00");
                        autoCall = true;
                        CounterProcess(str.Split(',').ToArray(), 0);
                    }
                }
            }
            catch (Exception)
            {
            }
            return true;
        }

        public bool PrintNewTicket_VietThaiQuan(int serviceId,
          bool isTouchScreen,
          bool isProgrammer,
          TimeSpan? timeServeAllow,
          string Name,
          string Address,
          int? DOB,
           string MaBenhNhan,
          string MaPhongKham,
          string SttPhongKham,
          string SoXe,
          string MaCongViec,
          string MaLoaiCongViec,
          int stt,
          int printerId
          )
        {
            IsDatabaseChange = true;
            var printModel = new PrintModel();
            int nghiepVu = 0;
            string printStr = string.Empty,
                tenQuay = string.Empty;
            bool err = false;
            ServiceDayModel serObj = null;
            DateTime now = DateTime.Now;
            serObj = lib_Services.FirstOrDefault(x => x.Id == serviceId);
            if (serObj == null)
                errorsms = "Dịch vụ số " + serviceId + " không tồn tại. Xin quý khách vui lòng chọn dịch vụ khác.";
            else
            {
                printModel.TenDichVu = (serObj != null ? serObj.Name : "");
                printModel.NoteDV = (serObj != null ? serObj.Note : "");
                if (CheckTimeBeforePrintTicket == "1" && serObj.Shifts.FirstOrDefault(x => now.TimeOfDay >= x.Start.TimeOfDay && now.TimeOfDay <= x.End.TimeOfDay) == null)
                    temp.Add(SoundLockPrintTicket);
                else
                {
                    var rs = BLLVietThaiQuan.Instance.ThemPhieu(connectString, stt, serviceId, SttPhongKham, MaCongViec, MaLoaiCongViec, (timeServeAllow != null ? timeServeAllow.Value : serObj.TimeProcess.TimeOfDay), now);
                    if (rs.IsSuccess)
                    {
                        if (!isProgrammer)
                        {
                            var soArr = BaseCore.Instance.ChangeNumber(((int)rs.Data + 1));
                            printStr = (soArr[0] + " " + soArr[1] + " ");
                            if (printTicketReturnCurrentNumberOrServiceCode == 1)
                            {
                                soArr = BaseCore.Instance.ChangeNumber((int)rs.Records);
                            }
                            else
                            {
                                soArr = BaseCore.Instance.ChangeNumber(serviceId);
                            }
                            printStr += (soArr[0] + " " + soArr[1] + " " + now.ToString("dd") + " " + now.ToString("MM") + " " + now.ToString("yy") + " " + now.ToString("HH") + " " + now.ToString("mm"));
                        }
                        else if (isProgrammer)
                            lbRecieve.Caption = printerId + "," + serviceId + "," + ((int)rs.Data + 1);
                        nghiepVu = rs.Data_1;

                        var tk = (Q_DailyRequire)rs.Data_4;
                        printModel.TenQuay = rs.Data_2;
                        printModel.STT = ((int)rs.Data + 1);
                        printModel.SoXe = tk.CarNumber;
                        printModel.MaKH = tk.MaBenhNhan;
                        printModel.TenKH = tk.CustomerName;
                        printModel.DOB = tk.CustomerDOB ?? 0;
                        printModel.DiaChi = tk.CustomerAddress;
                        printModel.MaDV = tk.MaPhongKham;
                        printModel.Phone = tk.PhoneNumber;
                    }
                    else
                        errorsms = rs.Errors[0].Message;
                }
            }


            if (!string.IsNullOrEmpty(printStr))
            {
                errorsms = printStr.ToString();
                if (UsePrintBoard == 1)
                {
                    if (isTouchScreen)
                        dataSendToComport.Add("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(printerId));
                    dataSendToComport.Add(printStr);
                }
                else
                {
                    PrintWithNoBorad(printModel);
                }
            }
            else if (printModel.STT != 0)
                if (UsePrintBoard == 0)
                    PrintWithNoBorad(printModel);

            if (AutoCallIfUserFree == 1 && nghiepVu > 0)
            {
                var freeUser = (int)BLLDailyRequire.Instance.CheckUserFree(connectString, nghiepVu, serviceId, printModel.STT, autoCallFollowMajorOrder).Data;
                if (freeUser > 0)
                {
                    var counter = lib_Users.FirstOrDefault(x => x.UserId == freeUser).EquipCode;
                    var str = ("AA," + BaseCore.Instance.ConvertIntToStringWith0Number(counter) + ",8B,00,00");
                    autoCall = true;
                    CounterProcess(str.Split(',').ToArray(), 0);
                }
            }
            return true;
        }

        private void PrintWithNoBorad(PrintModel printModel)
        {
            var now = DateTime.Now;

            checkCOM:
            if (!COM_Printer.IsOpen)
            {
                COM_Printer.Open();
                LogWriter.LogWrite(string.Format("func PrintWithNoBorad: Restart COM Máy in {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:ss")));
                goto checkCOM;
            }

            if (COM_Printer.IsOpen && printTemplates.Count > 0)
            {
                for (int i = 0; i < printTemplates.Count; i++)
                {
                    var template = printTemplates[i].PrintTemplate;
                    template = template.Replace("[canh-giua]", "\x1b\x61\x01|+|");
                    template = template.Replace("[canh-trai]", "\x1b\x61\x00|+|");
                    template = template.Replace("[1x1]", "\x1d\x21\x00|+|");
                    template = template.Replace("[2x1]", "\x1d\x21\x01|+|");
                    template = template.Replace("[3x1]", "\x1d\x21\x02|+|");
                    template = template.Replace("[2x2]", "\x1d\x21\x11|+|");
                    template = template.Replace("[3x3]", "\x1d\x21\x22|+|");

                    template = template.Replace("[STT]", printModel.STT.ToString());
                    template = template.Replace("[ten-quay]", printModel.TenQuay);
                    template = template.Replace("[ten-dich-vu]", printModel.TenDichVu);
                    template = template.Replace("[ghi-chu-dich-vu]", printModel.NoteDV);
                    template = template.Replace("[ngay]", ("Ngày: " + now.ToString("dd/MM/yyyy")));
                    template = template.Replace("[gio]", ("Giờ: " + now.ToString("HH:mm")));
                    template = template.Replace("[dang-goi]", "đang gọi: " + printModel.STTHienTai);


                    template = template.Replace("[so-xe]", getStringValue(printModel.SoXe));
                    template = template.Replace("[phone]", getStringValue(printModel.Phone));
                    template = template.Replace("[ma-kh]", getStringValue(printModel.MaKH));
                    template = template.Replace("[ten-kh]", getStringValue(printModel.TenKH));
                    template = template.Replace("[dia-chi]", getStringValue(printModel.DiaChi));
                    template = template.Replace("[ma-dv]", getStringValue(printModel.MaDV));
                    template = template.Replace("[dob]", getIntValue(printModel.DOB));

                    template = template.Replace("[cat-giay]", "\x1b\x69|+|");

                    var arr = template.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    for (int ii = 0; ii < printTemplates[i].PrintPages; ii++)
                    {
                        for (int iii = 0; iii < arr.Length; iii++)
                        {
                            // frmMain.COM_Printer.Write(arr[i]);
                            try
                            {
                                BaseCore.Instance.PrintTicketTCVN3(COM_Printer, arr[iii]);
                            }
                            catch (Exception ex)
                            {
                                LogWriter.LogWrite(("COM write error: " + ex.Message));
                            }

                            //sleep
                            Thread.Sleep(50);
                        }
                    }
                }
            }
            else
                errorsms = "Cổng COM máy in hiện tại chưa kết nối. Vui lòng kiểm tra lại COM máy in";
        }

        private string getStringValue(string value)
        {
            return !string.IsNullOrEmpty(value) ? (value + "\n") : "";
        }

        private string getIntValue(int value)
        {
            return value != null && value > 0 ? (value.ToString() + "\n") : "";
        }

        private void TmerQuetComport_Tick(object sender, EventArgs e)
        {
            try
            {
                if (comPort.IsOpen)
                {
                    q++;
                    if (dataSendToComport.Count > 0)
                    {
                        if (!isSendDataKeyPad)
                        {
                            q = 0;
                            isSendDataKeyPad = true;
                        }
                        if (q < dataSendToComport.Count())
                        {
                            SendRequest(dataSendToComport[q]);
                            lbQuet.Caption = dataSendToComport[q].ToString().Replace(' ', ',');
                            //sleep
                            Thread.Sleep(100);
                        }
                        else
                        {
                            q = -1;
                            dataSendToComport.Clear();
                            isSendDataKeyPad = false;
                        }
                    }
                    else
                    {
                        if (q < equipmentIds.Count())
                        {
                            lbSendrequest = ("AA " + strEquipmentIds[q] + " 05 00 00");
                            SendRequest(("AA " + strEquipmentIds[q] + " 05 00 00"));

                            // lbSendrequest = ("AA " + strEquipmentIds[q]  );
                            // SendRequest(("AA " + strEquipmentIds[q]  ));
                            lbQuet.Caption = lbSendrequest;
                            if (q == (equipmentIds.Count() - 1))
                                q = -1;
                        }
                        else
                            q = -1;
                    }
                }

                if (temp.Count > 0 && isFinishRead)
                {
                    player = new SoundPlayer();
                    playThread = new Thread(PlaySound);
                    playThread.Start();
                }
                lbErrorsms.Caption = errorsms;
            }
            catch (Exception ex)
            {
            }
        }

        private void SendRequest(string value)
        {
            try
            {
                lbErrorsms.Caption = ("gui suong  comport " + value);
                byte[] newMsg = BaseCore.Instance.HexStringToByteArray(value);
                if (!comPort.IsOpen)
                    comPort.Open();
                comPort.Write(newMsg, 0, newMsg.Length);
                // comPort.Write(value);
                Thread.Sleep(20);
            }
            catch
            {
                MessageBox.Show("loi gui suong  comport " + value);
            }
        }

        private void CounterProcess(string[] _hexStr, int requireId)
        {
            //MessageBox.Show(string.Join("-", hexStr));
            try
            {
                var hexStr = _hexStr.ToList();
                if (hexStr.Count > 5)
                    hexStr.RemoveRange(0, 2);

                // lib_Users.Clear();
                //  lib_Users = BLLLoginHistory.Instance.GetsForMain();
                int equipCode = int.Parse(hexStr[1]);
                var userobj = lib_Users.FirstOrDefault(x => x.EquipCode == equipCode);
                if (userobj != null && userobj.UserId != 0)
                {
                    int userId = userobj.UserId;
                    IsDatabaseChange = true;
                    List<RegisterUserCmdModel> userRight;
                    if (hexStr[2] == eCodeHex.Thang)
                    {
                        int code = 0;
                        int.TryParse((hexStr[3] + hexStr[4]), out code);
                        var sohientai = BLLDailyRequire.Instance.CurentTicket(connectString, equipCode);
                        if (code == sohientai)
                            code = 0;

                        if (code > 100)
                            userRight = lib_RegisterUserCmds.Where(x => x.UserId == userId && x.CMDName == hexStr[2] && x.CMDParamName == "100").OrderBy(x => x.Index).ToList();
                        else
                            userRight = lib_RegisterUserCmds.Where(x => x.UserId == userId && x.CMDName == hexStr[2] && x.CMDParamName == code.ToString()).OrderBy(x => x.Index).ToList();
                    }
                    else if (hexStr[2] == eCodeHex.Sao && _8cUseFor == 2) //2=> đổi tg phuc vu
                    {
                        // nếu là * lấy cái tham số lệnh = 0 xử lý cho trường hợp đổi thời gian phục vụ DK
                        userRight = lib_RegisterUserCmds.Where(x => x.UserId == userId && x.CMDName == hexStr[2] && x.CMDParamName == "0").OrderBy(x => x.Index).ToList();
                    }
                    else if (hexStr[3] == "00")
                    {
                        userRight = lib_RegisterUserCmds.Where(x => x.UserId == userId && x.CMDName == hexStr[2] && x.CMDParamName == int.Parse(hexStr[4]).ToString()).OrderBy(x => x.Index).ToList();
                    }
                    else
                        userRight = lib_RegisterUserCmds.Where(x => x.UserId == userId && x.CMDName == hexStr[2] && x.CMDParamName == "0").OrderBy(x => x.Index).ToList();

                    var userMajors = lib_UserMajors.Where(x => x.UserId == userId).ToList();
                    if (userRight.Count > 0)
                    {
                        int currentTicket = 0, minutes = 0;
                        TicketInfo ticketInfo = null,
                            currentTicketInfo = null;
                        string num1 = "00", num2 = "00";

                        for (int i = 0; i < userRight.Count; i++)
                        {
                            switch (userRight[i].ActionName)
                            {
                                case eActionCode.HienHanh:
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.HoanTat:
                                            currentTicket = BLLDailyRequire.Instance.DoneTicket(connectString, userId, equipCode, today);
                                            dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(equipCode) + " 01 00 00"));
                                            dataSendToComport.Add(("AA " + BaseCore.Instance.ConvertIntToStringWith0Number(equipCode) + " 03 00 00"));
                                            if (currentTicket != 0)
                                            {
                                                BLLSQLBuilder.Instance.Excecute(connectString, (" UPDATE [Q_COUNTERDAYINFO] set [STT] = 0, [STT_3]='0',[STT_QMS]=0 ,[StatusSTT] = 2 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL WHERE [EquipCode]=" + equipCode));
                                            }
                                            break;
                                        case eActionParam.HuyKH:
                                            currentTicket = BLLDailyRequire.Instance.DeleteTicket(connectString, int.Parse((hexStr[3] + hexStr[4])), today);
                                            break;
                                        case eActionParam.HITHI:
                                            var current = BLLDailyRequire.Instance.GetCurrentTicket(connectString, userId, equipCode, today, UseWithThirdPattern);

                                            if (current.IsSuccess)
                                            {
                                                currentTicket = current.Data;
                                                ticketInfo = current.Data_3;
                                            }
                                            else
                                                currentTicket = 0;
                                            break;
                                    }
                                    break;
                                case eActionCode.GoiMoi:
                                    BLLDailyRequire.Instance.EndAllCallOfUserWithoutThisEquipment(connectString, userId, equipCode);
                                    #region xu ly goi moi
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.NGVU:
                                            int[] indexs = userMajors.Select(x => x.Index).Distinct().ToArray();
                                            for (int z = 0; z < indexs.Length; z++)
                                            {
                                                ticketInfo = BLLDailyRequire.Instance.CallNewTicket(connectString, userMajors.Where(x => x.Index == indexs[z]).Select(x => x.MajorId).ToArray(), userId, equipCode, true, DateTime.Now, UseWithThirdPattern);
                                                if (ticketInfo != null && ticketInfo.TicketNumber != 0)
                                                {
                                                    BLLDailyRequire.Instance.UpdateCounterDayInfo(connectString, ticketInfo);
                                                }
                                                else
                                                    BLLSQLBuilder.Instance.Excecute(connectString, (" UPDATE [Q_COUNTERDAYINFO] set [STT] = 0 , [StatusSTT] = 0 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL WHERE [EquipCode]=" + equipCode));
                                            }
                                            break;
                                        case eActionParam.GDENQ:
                                        case eActionParam.GLAYP:
                                            ticketInfo = BLLDailyRequire.Instance.CallNewTicket_GLP_NghiepVu(connectString, userMajors.Select(x => x.MajorId).ToArray(), userId, equipCode, DateTime.Now, UseWithThirdPattern);
                                            if (ticketInfo != null && ticketInfo.TicketNumber != 0)
                                            {
                                                BLLDailyRequire.Instance.UpdateCounterDayInfo(connectString, ticketInfo);
                                            }
                                            else
                                                BLLSQLBuilder.Instance.Excecute(connectString, (" UPDATE [Q_COUNTERDAYINFO] set [STT] = 0 ,[StatusSTT] = 0 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL WHERE [EquipCode]=" + equipCode));
                                            break;
                                        case eActionParam.COUNT: // goi bat ky 
                                            int num = int.Parse((hexStr[3] + "" + hexStr[4]));
                                            var rs = BLLDailyRequire.Instance.CallAny(connectString, userMajors[0].MajorId, userId, equipCode, num, DateTime.Now, UseWithThirdPattern);
                                            if (rs.IsSuccess)
                                            {
                                                ticketInfo = rs.Data_3;// new TicketInfo() { TicketNumber = num, StartTime = rs.Data_1, TimeServeAllow = rs.Data };
                                                BLLDailyRequire.Instance.UpdateCounterDayInfo(connectString, ticketInfo);
                                            }
                                            else
                                                BLLSQLBuilder.Instance.Excecute(connectString, (" UPDATE [Q_COUNTERDAYINFO] set [STT] = 0 ,[StatusSTT] = 0 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL WHERE [EquipCode]=" + equipCode));
                                            break;
                                        case eActionParam.GNVYC: // goi theo Nghiep vu  
                                            var found = BLLDailyRequire.Instance.CallByMajor(connectString, int.Parse(userRight[i].Param), userId, equipCode, DateTime.Now, UseWithThirdPattern);
                                            if (found.IsSuccess)
                                            {
                                                BLLDailyRequire.Instance.UpdateCounterDayInfo(connectString, ticketInfo);
                                                ticketInfo = found.Data_3;// new TicketInfo() { TicketNumber = found.Data, StartTime = found.Data_2, TimeServeAllow = found.Data_1 };
                                            }
                                            else
                                                BLLSQLBuilder.Instance.Excecute(connectString, (" UPDATE [Q_COUNTERDAYINFO] set [STT] = 0 ,[StatusSTT] = 0 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL WHERE [EquipCode]=" + equipCode));
                                            break;
                                        case eActionParam.PHANDIEUNHANVIEN: // goi phan dieu Nghiep vu cho nhan vien
                                            var allowCall = false;
                                            for (int ii = 0; ii < userMajors.Count; ii++)
                                            {
                                                allowCall = BLLDailyRequire.Instance.ChekCanCallNext(connectString, userMajors[ii].MajorId, userId);
                                                if (allowCall)
                                                {
                                                    var callInfo = BLLDailyRequire.Instance.CallByMajor(connectString, userMajors[ii].MajorId, userId, equipCode, DateTime.Now, UseWithThirdPattern);
                                                    if (callInfo.IsSuccess)
                                                    {
                                                        ticketInfo = callInfo.Data_3;// new TicketInfo() { TicketNumber = callInfo.Data, StartTime = callInfo.Data_2, TimeServeAllow = callInfo.Data_1 };
                                                        BLLDailyRequire.Instance.UpdateCounterDayInfo(connectString, ticketInfo);
                                                        break;
                                                    }
                                                    else
                                                        BLLSQLBuilder.Instance.Excecute(connectString, (" UPDATE [Q_COUNTERDAYINFO] set [STT] = 0 , [StatusSTT] = 0 ,[STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL WHERE [EquipCode]=" + equipCode));
                                                }
                                            }
                                            break;
                                        case eActionParam.GOI_PHIEU_TRONG: // goi phieu trống số phiếu =-1
                                            var c = BLLDailyRequire.Instance.InsertAndCallEmptyTicket(connectString, equipCode);
                                            if (c.IsSuccess)
                                            {
                                                ticketInfo = c.Data_3;// new TicketInfo() { TicketNumber = c.Data, StartTime = c.Data_2, TimeServeAllow = c.Data_1 };
                                                BLLDailyRequire.Instance.UpdateCounterDayInfo(connectString, ticketInfo);
                                            }
                                            else
                                                BLLSQLBuilder.Instance.Excecute(connectString, (" UPDATE [Q_COUNTERDAYINFO] set [STT] = 0 ,[StatusSTT] = 0 , [STT_UT]=0,[PrintTime]=NULL,[StartTime]=NULL,[ServeTime]=NULL WHERE [EquipCode]=" + equipCode));
                                            break;
                                    }
                                    #endregion
                                    break;
                                case eActionCode.Counter:
                                    #region xu ly gui so len counter
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.MOIKH: // gui so len counter
                                            arrStr = BaseCore.Instance.ChangeNumber((ticketInfo != null && ticketInfo.TicketNumber > 0 ? ticketInfo.TicketNumber : 0));
                                            if (autoCall)
                                            {
                                                dataSendToComport.Add(("AA " + hexStr[1] + " 05 00 00"));
                                                autoCall = false;
                                            }
                                            dataSendToComport.Add(("AA " + hexStr[1] + " 01 " + arrStr[0] + " " + arrStr[1]));
                                            if (ticketInfo != null)
                                                if (ticketInfo.CountDown == "-1")
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 04 00 00"));
                                                else
                                                {
                                                    var countdown = ticketInfo != null ? ticketInfo.CountDown.Split(':').ToArray() : new string[] { "00", "00" };
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 02 " + countdown[0] + " " + countdown[1]));
                                                }
                                            break;
                                        case eActionParam.TKHDG:
                                            arrStr = BaseCore.Instance.ChangeNumber(BLLDailyRequire.Instance.CountTicketDoneProcessed(connectString, equipCode));
                                            dataSendToComport.Add(("AA " + hexStr[1] + " 01 " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.THKCG:
                                            arrStr = BaseCore.Instance.ChangeNumber(BLLDailyRequire.Instance.CountTicketWatingProcessed(connectString, equipCode));
                                            dataSendToComport.Add(("AA " + hexStr[1] + " 01 " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.HITHI: // gui so len counter
                                            arrStr = BaseCore.Instance.ChangeNumber(currentTicket);
                                            dataSendToComport.Add(("AA " + hexStr[1] + " 01 " + arrStr[0] + " " + arrStr[1]));
                                            if (ticketInfo != null)
                                                if (ticketInfo.CountDown == "-1")
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 04 00 00"));
                                                else
                                                {
                                                    var countdown = ticketInfo != null ? ticketInfo.CountDown.Split(':').ToArray() : new string[] { "00", "00" };
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 02 " + countdown[0] + " " + countdown[1]));
                                                }
                                            break;
                                        case eActionParam.THAYTHE_TGIAN_PVU:
                                            minutes = 0;
                                            int.TryParse((hexStr[3] + "" + hexStr[4]), out minutes);
                                            num1 = "00";
                                            num2 = "00";
                                            currentTicketInfo = BLLDailyRequire.Instance.UpdateServeTime(connectString, userId, minutes, true);
                                            if (currentTicketInfo != null)
                                            {
                                                num1 = currentTicketInfo.TimeServeAllow.ToString("hh");
                                                num2 = currentTicketInfo.TimeServeAllow.ToString("mm");
                                            }
                                            dataSendToComport.Add(("AB " + hexStr[1] + " " + num1 + " " + num2));
                                            break;
                                        case eActionParam.CONGTHEM_TGIAN_PVU:
                                            minutes = 0;
                                            int.TryParse((hexStr[3] + "" + hexStr[4]), out minutes);
                                            num1 = "00";
                                            num2 = "00";
                                            currentTicketInfo = BLLDailyRequire.Instance.UpdateServeTime(connectString, userId, minutes, false);
                                            if (currentTicketInfo != null)
                                            {
                                                num1 = currentTicketInfo.TimeServeAllow.ToString("hh");
                                                num2 = currentTicketInfo.TimeServeAllow.ToString("mm");
                                            }
                                            dataSendToComport.Add(("AB " + hexStr[1] + " " + num1 + " " + num2));
                                            break;
                                        case eActionParam.GUI_TGIAN_PVU_DKIEN:
                                            num1 = "00";
                                            num2 = "00";
                                            currentTicketInfo = BLLDailyRequire.Instance.GetCurrentTicketInfo(connectString, userId, equipCode, today, UseWithThirdPattern);
                                            if (currentTicketInfo != null)
                                            {
                                                num1 = currentTicketInfo.TimeServeAllow.ToString("hh");
                                                num2 = currentTicketInfo.TimeServeAllow.ToString("mm");
                                            }
                                            dataSendToComport.Add(("AB " + hexStr[1] + " " + num1 + " " + num2));
                                            break;
                                        case eActionParam.GUI_TGIAN_KTHUC_DKIEN:
                                            num1 = "00";
                                            num2 = "00";
                                            currentTicketInfo = BLLDailyRequire.Instance.GetCurrentTicketInfo(connectString, userId, equipCode, today, UseWithThirdPattern);
                                            if (currentTicketInfo != null)
                                            {
                                                num1 = currentTicketInfo.StartTime.Add(currentTicketInfo.TimeServeAllow).ToString("hh");
                                                num2 = currentTicketInfo.StartTime.Add(currentTicketInfo.TimeServeAllow).ToString("mm");
                                            }
                                            dataSendToComport.Add(("AB " + hexStr[1] + " " + num1 + " " + num2));
                                            break;
                                        case eActionParam.HUY_DKY_LAYSO:
                                            BLLDailyRequire.Instance.RemoveRegisterAutocall(connectString, userId);
                                            dataSendToComport.Add(("AA " + hexStr[1] + " 00 00"));
                                            break;
                                    }
                                    #endregion
                                    break;
                                case eActionCode.HienThiChinh:
                                    #region Xu ly gui so len hien thi chinh
                                    List<MaindisplayDirectionModel> mains;
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.MOIKH: // gui so len hien thi chính
                                            mains = BLLMaindisplayDirection.Instance.Gets(connectString, equipCode);
                                            if (mains.Count > 0)
                                            {
                                                arrStr = BaseCore.Instance.ChangeNumber((ticketInfo != null ? ticketInfo.TicketNumber : 0));
                                                string id = "";
                                                for (int z = 0; z < mains.Count; z++)
                                                {
                                                    id = (mains[z].EquipmentId < 10 ? ("0" + mains[z].EquipmentCode) : mains[z].EquipmentCode.ToString());
                                                    int mainDiric = (mains[z].Direction ? 8 : 0);
                                                    if (hexStr[1].Length > 1)
                                                        mainDiric = mainDiric + int.Parse(hexStr[1].Substring(0, 1));
                                                    dataSendToComport.Add("AA " + id);
                                                    dataSendToComport.Add(("AA " + id + " " + arrStr[0] + " " + arrStr[1] + " " + mainDiric + "" + (hexStr[1].Length > 1 ? hexStr[1].Substring(1, 1) : hexStr[1])));
                                                    lbRecieve.Caption = dataSendToComport[1];
                                                }
                                            }
                                            break;
                                        case eActionParam.NHAKH: // gui so len hien thi chính
                                            mains = BLLMaindisplayDirection.Instance.Gets(connectString, equipCode);
                                            if (mains.Count > 0)
                                            {
                                                arrStr = BaseCore.Instance.ChangeNumber(currentTicket);
                                                string id = "";
                                                for (int z = 0; z < mains.Count; z++)
                                                {
                                                    id = (mains[z].EquipmentId < 10 ? ("0" + mains[z].EquipmentCode) : mains[z].EquipmentCode.ToString());

                                                    int mainDiric = (mains[z].Direction ? 8 : 0);
                                                    if (hexStr[1].Length > 1)
                                                        mainDiric = mainDiric + int.Parse(hexStr[1].Substring(0, 1));
                                                    dataSendToComport.Add("AA " + id);
                                                    dataSendToComport.Add(("AA " + id + " " + arrStr[0] + " " + arrStr[1] + " " + mainDiric + "" + (hexStr[1].Length > 1 ? hexStr[1].Substring(1, 1) : hexStr[1])));
                                                }
                                            }
                                            break;
                                    }
                                    #endregion
                                    break;
                                case eActionCode.AmThanh:
                                    #region xu ly doc am thanh
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.MOIKH:
                                            if (ticketInfo != null && ticketInfo.TicketNumber > 0)
                                            {
                                                var readTemplateIds = lib_UserCMDReadSound.Where(x => x.UserId == userId && x.Note.Equals(userRight[i].CMDName.ToUpper())).Select(x => x.ReadTemplateId).ToList();// BLLUserCmdReadSound.Instance.GetReadTemplateIds(userId, userRight[i].CMDName);
                                                if (readTemplateIds.Count > 0)
                                                    GetSound(readTemplateIds, ticketInfo.TicketNumber.ToString(), lib_Equipments.FirstOrDefault(x => x.Code == equipCode).CounterId);
                                            }
                                            break;
                                        case eActionParam.NHAKH:
                                            if (currentTicket != 0)
                                            {
                                                var readTemplateIds = lib_UserCMDReadSound.Where(x => x.UserId == userId && x.Note.Equals(userRight[i].CMDName.ToUpper())).Select(x => x.ReadTemplateId).ToList(); //BLLUserCmdReadSound.Instance.GetReadTemplateIds(userId, userRight[i].CMDName);
                                                if (readTemplateIds.Count > 0)
                                                    GetSound(readTemplateIds, currentTicket.ToString(), lib_Equipments.FirstOrDefault(x => x.Code == equipCode).CounterId);//  BLLEquipment.Instance.GetCounterId(equipCode));
                                            }
                                            break;
                                        case eActionParam.SOUNDONLY:
                                            var readTemplateIdSs = lib_UserCMDReadSound.Where(x => x.UserId == userId && x.Note.Equals(userRight[i].CMDName.ToUpper())).Select(x => x.ReadTemplateId).ToList(); //BLLUserCmdReadSound.Instance.GetReadTemplateIds(userId, userRight[i].CMDName);
                                            if (readTemplateIdSs.Count > 0)
                                                GetSound(readTemplateIdSs, "0", lib_Equipments.FirstOrDefault(x => x.Code == equipCode).CounterId);//  BLLEquipment.Instance.GetCounterId(equipCode));

                                            break;
                                    }
                                    #endregion
                                    break;
                                case eActionCode.Chuyen:
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.NGHVU:

                                            if (BLLDailyRequire.Instance.TranferTicket(connectString, equipCode, int.Parse(userRight[i].Param), currentTicket, today, false))
                                            {
                                                if (comPort.IsOpen)
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 01 00 00"));
                                                currentTicket = 0;
                                            }

                                            break;
                                    }
                                    break;
                                case eActionCode.DanhGia:
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.DANHGIA:
                                            if (BLLUserEvaluate.Instance.DanhGia_BP(connectString, equipCode, userRight[i].Param, system).IsSuccess)
                                            {
                                                if (comPort.IsOpen)
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 01 00 00"));
                                                currentTicket = 0;
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                        End:
                        { }
                    }
                    else
                    {
                        dataSendToComport.Add(("AA " + hexStr[1] + " 01 00 00"));
                    }
                }
                else
                    errorsms = ("Không tìm thấy thông tin đăng nhập của thiết bị " + equipCode);
                //  MessageBox.Show("Không tìm thấy thông tin đăng nhập của thiết bị " + equipCode, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hàm CounterProcess => " + ex.Message);
            }

            try
            {
                if (requireId != 0)
                    BLLCounterSoftRequire.Instance.Delete(requireId, connectString);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        private void btnVideo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmVideo));
            if (frm == null)
            {
                var forms = new frmVideo();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnReportDG_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmR_DanhGia));
            if (frm == null)
            {
                var forms = new frmR_DanhGia();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnVideoTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmVideoTemplate));
            if (frm == null)
            {
                var forms = new frmVideoTemplate();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnHome_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmHome));
            if (frm == null)
            {
                var f = new frmHome();
                f.MdiParent = this;
                f.Show();
            }
            else
                frm.Activate();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //  CounterProcess(new string[] { "AA", "11", "8B", "00", "00" });
            //  var kq = BLLDailyRequire.Instance.CallAny(1, 1, 1028, today);
        }

        private void timerDo_Tick(object sender, EventArgs e)
        {
            timerDo.Enabled = false;
            if (Settings.Default.Today != DateTime.Now.Day)
            {
                ResetDayInfo();
            }
            else
                errorsms = "";

            try
            {
                var requires = BLLCounterSoftRequire.Instance.Gets(connectString);
                if (requires.Count > 0)
                {
                    IsDatabaseChange = true;
                    bool hasSound = false;
                    List<MaindisplayDirectionModel> mains;
                    PrinterRequireModel requireObj = null;
                    for (int i = 0; i < requires.Count; i++)
                    {
                        try
                        {
                            switch (requires[i].Type)
                            {
                                case (int)eCounterSoftRequireType.inPhieu:
                                    requireObj = JsonConvert.DeserializeObject<PrinterRequireModel>(requires[i].Content);
                                    var serObj = lib_Services.FirstOrDefault(x => x.Id == requireObj.ServiceId);
                                    var printModel = new PrintModel()
                                    {
                                        STT = requireObj.newNumber,
                                        STTHienTai = requireObj.oldNumber,
                                        TenQuay = requireObj.TenQuay,
                                        TenDichVu = requireObj.TenDichVu,
                                        NoteDV = (serObj != null ? serObj.Note : ""),
                                        SoXe = requireObj.SoXe,
                                        DiaChi = requireObj.Address,
                                        DOB = requireObj.DOB ?? 0,
                                        MaDV = requireObj.MaPhongKham,
                                        MaKH = requireObj.MaBenhNhan,
                                        Phone = requireObj.Phone,
                                        TenKH = requireObj.Name
                                    };
                                    //in  trực tiếp ko board
                                    PrintWithNoBorad(printModel);
                                    // MessageBox.Show(JsonConvert.SerializeObject(printModel));

                                    //in theo driver
                                    //InPhieuDungDriver(requireObj.newNumber, requireObj.oldNumber, requireObj.TenQuay, requireObj.TenQuay, ("   Ngày: " + now.ToString("dd/MM/yyyy") + ("     Giờ: " + now.ToString("HH/mm"))), "VIỆT THÁI QUÂN 3");
                                    if (AutoCallIfUserFree == 1 && requireObj.MajorId > 0)
                                    {
                                        var freeUser = (int)BLLDailyRequire.Instance.CheckUserFree(connectString, requireObj.MajorId, requireObj.ServiceId, requireObj.newNumber, autoCallFollowMajorOrder).Data;
                                        if (freeUser > 0)
                                        {
                                            var counter = lib_Users.FirstOrDefault(x => x.UserId == freeUser).EquipCode;
                                            var str = ("AA," + BaseCore.Instance.ConvertIntToStringWith0Number(counter) + ",8B,00,00");
                                            autoCall = true;
                                            CounterProcess(str.Split(',').ToArray(), 0);
                                        }
                                    }
                                    BLLCounterSoftRequire.Instance.Delete(requires[i].Id, connectString);
                                    break;
                                case (int)eCounterSoftRequireType.ReadSound:
                                    temp.AddRange(requires[i].Content.Split('|').ToList());
                                    hasSound = true;
                                    break;
                                case (int)eCounterSoftRequireType.PrintTicket:
                                    requireObj = JsonConvert.DeserializeObject<PrinterRequireModel>(requires[i].Content);
                                    var result = PrintNewTicket(requireObj.PrinterId, requireObj.ServiceId, 0, true, false, requireObj.ServeTime.TimeOfDay, requireObj.Name, requireObj.Address, requireObj.DOB, requireObj.MaBenhNhan, requireObj.MaPhongKham, requireObj.SttPhongKham, requireObj.SoXe, requireObj.MaCongViec, requireObj.MaLoaiCongViec);
                                    if (result)
                                        BLLCounterSoftRequire.Instance.Delete(requires[i].Id, connectString);
                                    break;
                                case (int)eCounterSoftRequireType.CounterEvent:
                                    if (!string.IsNullOrEmpty(requires[i].Content))
                                    {
                                        var arr = requires[i].Content.Split(',').ToArray();
                                        if (arr != null && arr.Length == 5)
                                            CounterProcess(arr, requires[i].Id);
                                    }
                                    break;
                                case (int)eCounterSoftRequireType.SendNextToMainDisplay:  // gui so len hien thi chính
                                    var mainRequireObj = JsonConvert.DeserializeObject<RequireMainDisplay>(requires[i].Content);
                                    mains = BLLMaindisplayDirection.Instance.Gets(connectString, mainRequireObj.EquipCode);
                                    if (mains.Count > 0)
                                    {
                                        arrStr = BaseCore.Instance.ChangeNumber(mainRequireObj.TicketNumber);
                                        string id = "";
                                        for (int z = 0; z < mains.Count; z++)
                                        {
                                            id = (mains[z].EquipmentCode < 10 ? ("0" + mains[z].EquipmentCode) : mains[z].EquipmentCode.ToString());
                                            int mainDiric = (mains[z].Direction ? 8 : 0);
                                            if (mainRequireObj.EquipCode > 1)
                                                mainDiric = mainDiric + mainRequireObj.EquipCode;
                                            dataSendToComport.Add("AA " + id);
                                            dataSendToComport.Add(("AA " + id + " " + arrStr[0] + " " + arrStr[1] + " 0" + mainRequireObj.EquipCode));
                                            lbRecieve.Caption = dataSendToComport[1];
                                        }
                                    }
                                    break;
                                case (int)eCounterSoftRequireType.SendRecallToMainDisplay: // gui so len hien thi chính
                                    var _mainRequireObj = JsonConvert.DeserializeObject<RequireMainDisplay>(requires[i].Content);
                                    mains = BLLMaindisplayDirection.Instance.Gets(connectString, _mainRequireObj.EquipCode);
                                    if (mains.Count > 0)
                                    {
                                        arrStr = BaseCore.Instance.ChangeNumber(_mainRequireObj.TicketNumber);
                                        string id = "";
                                        for (int z = 0; z < mains.Count; z++)
                                        {
                                            id = (mains[z].EquipmentCode < 10 ? ("0" + mains[z].EquipmentCode) : mains[z].EquipmentCode.ToString());

                                            int mainDiric = (mains[z].Direction ? 8 : 0);
                                            if (_mainRequireObj.EquipCode > 1)
                                                mainDiric = mainDiric + _mainRequireObj.EquipCode;
                                            dataSendToComport.Add("AA " + id);
                                            dataSendToComport.Add(("AA " + id + " " + arrStr[0] + " " + arrStr[1] + " 0" + _mainRequireObj.EquipCode));
                                        }
                                    }
                                    break;
                                case (int)eCounterSoftRequireType.CheckUserFree: // check user free use for autocall
                                    var checkRequireObj = JsonConvert.DeserializeObject<ModelSelectItem>(requires[i].Content);
                                    int nghiepVu = checkRequireObj.Id,
                                        serviceId = checkRequireObj.Data,
                                        newNumber = Convert.ToInt32(checkRequireObj.Code);
                                    if (AutoCallIfUserFree == 1 && nghiepVu > 0)
                                    {
                                        var freeUser = (int)BLLDailyRequire.Instance.CheckUserFree(connectString, nghiepVu, serviceId, newNumber, autoCallFollowMajorOrder).Data;
                                        if (freeUser > 0)
                                        {
                                            var counter = lib_Users.FirstOrDefault(x => x.UserId == freeUser).EquipCode;
                                            var str = ("AA," + BaseCore.Instance.ConvertIntToStringWith0Number(counter) + ",8B,00,00");
                                            autoCall = true;
                                            CounterProcess(str.Split(',').ToArray(), 0);
                                        }
                                    }
                                    break;
                                case (int)eCounterSoftRequireType.PrintTicket_VietThaiQuan:
                                    requireObj = JsonConvert.DeserializeObject<PrinterRequireModel>(requires[i].Content);
                                    var printResult = PrintNewTicket_VietThaiQuan(requireObj.ServiceId, false, false, requireObj.ServeTime.TimeOfDay, requireObj.Name, requireObj.Address, requireObj.DOB, requireObj.MaBenhNhan, requireObj.MaPhongKham, requireObj.SttPhongKham, requireObj.SoXe, requireObj.MaCongViec, requireObj.MaLoaiCongViec, requireObj.newNumber, requireObj.PrinterId);
                                    if (printResult)
                                        BLLCounterSoftRequire.Instance.Delete(requires[i].Id, connectString);
                                    break;

                            }

                        }
                        catch (Exception ex)
                        {
                            LogWriter.LogWrite(ex.Message);
                        }
                    }

                    if (isFinishRead && hasSound)
                    {
                        player = new SoundPlayer();
                        playThread = new Thread(PlaySound);
                        playThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            timerDo.Enabled = true;
        }

        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtPrint.EditValue == null && string.IsNullOrEmpty(txtPrint.EditValue.ToString()))
                MessageBox.Show("Lỗi: Vui lòng nhập thông tin cấp phiếu vào ô bên trên. Xin cám ơn.!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var str = txtPrint.EditValue.ToString().Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                if (str.Length > 2)
                    PrintNewTicket(str[0], str[1], 0, false, true, null, null, null, null, null, null, null, null, "cv1", null);
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (radioLenh.EditValue.ToString().Contains("8C") && _8cUseFor == 1)   ///Login - Logout
            {
                var num = (BLLLoginHistory.Instance.CounterLoginLogOut(connectString, int.Parse((txtparam1.EditValue.ToString() + txtparam2.EditValue.ToString())), int.Parse(txtmatb.EditValue.ToString()), DateTime.Now));
                var arrStr = BaseCore.Instance.ChangeNumber(num);
                dataSendToComport.Add(("AA " + txtmatb.EditValue.ToString() + " " + arrStr[0] + " " + arrStr[1]));
                lib_Users = BLLLoginHistory.Instance.GetsForMain(connectString);
                IsDatabaseChange = true;
            }
            else
            {
                var str = "AA," + txtmatb.EditValue.ToString() + "," + radioLenh.EditValue.ToString() + "," + txtparam1.EditValue.ToString() + "," + txtparam2.EditValue.ToString();
                CounterProcess(str.Split(',').ToArray(), 0);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form frm = IsActive(typeof(frmRecieverSMS));
            if (frm == null)
            {
                frmRecieverSMS forms = new frmRecieverSMS();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnServiceS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmServiceShift));
            if (frm == null)
            {
                frmServiceShift forms = new frmServiceShift();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btnProcess_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnMainDirection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = IsActive(typeof(frmMaindisplayDirection));
            if (frm == null)
            {
                frmMaindisplayDirection forms = new frmMaindisplayDirection();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
        }

        private void btRunProcess_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (isRunning)
                {
                    isRunning = false;
                    btRunProcess.Caption = "Chạy tiến trình";
                    btRunProcess.LargeGlyph = global::QMS_System.Properties.Resources.if_our_process_2_45123;
                    comPort.Close();
                    comPort.Dispose();
                    COM_Printer.Close();
                    COM_Printer.Dispose();
                    timerDo.Enabled = false;
                    tmerQuetServeOver.Enabled = false;
                    timerCapNhatGioCounter.Enabled = false;
                }
                else
                {
                    btRunProcess.Enabled = false;
                    btRunProcess.LargeGlyph = global::QMS_System.Properties.Resources.if_Stop_131765;
                    isRunning = true;
                    btRunProcess.Caption = "Tắt tiến trình";
                    playlist = new List<string>();
                    dataSendToComport = new List<string>();
                    arrStr = new List<string>();

                    // equipmentIds = BLLEquipment.Instance.Gets(connectString);
                    lib_RegisterUserCmds = BLLRegisterUserCmd.Instance.Gets(connectString);
                    lib_UserMajors = BLLUserMajor.Instance.Gets(connectString);
                    lib_UserCMDReadSound = BLLUserCmdReadSound.Instance.Gets(connectString);
                    lib_Equipments = BLLEquipment.Instance.Gets(connectString, (int)eEquipType.Counter);
                    lib_Sounds = BLLSound.Instance.Gets(connectString);
                    lib_ReadTemplates = BLLReadTemplate.Instance.GetsForMain(connectString);
                    lib_CounterSound = BLLCounterSound.Instance.Gets(connectString);
                    lib_Services = BLLService.Instance.GetsForMain(connectString);

                    int time = 1000;
                    int.TryParse(GetConfigByCode(eConfigCode.TimerQuetServeOverTime), out time);
                    InitComPort();
                    if (UsePrintBoard == 0)
                        InitCOM_Printer();

                    lib_Users = BLLLoginHistory.Instance.GetsForMain(connectString);

                    if (Settings.Default.Program)
                    {
                        ribbonPage5.Visible = true;
                        txtPrint.EditValue = PrintTicketCode + ",1,0";
                    }
                    TmerQuetComport.Enabled = true;
                    TmerQuetComport.Interval = timeQuetComport;
                    timerDo.Enabled = true;
                    tmerQuetServeOver.Interval = time;
                    if (GetConfigByCode(eConfigCode.CheckServeOverTime) == "1")
                        tmerQuetServeOver.Enabled = true;
                    btRunProcess.Enabled = true;
                    timerCapNhatGioCounter.Enabled = true;
                }
                lbRecieve.Caption = "";
            }
            catch (Exception ex) { }
        }

        private void tmerQuetServeOver_Tick(object sender, EventArgs e)
        {
            tmerQuetServeOver.Enabled = false;
            try
            {
                var dayInfos = BLLDailyRequire.Instance.GetDayInfo(connectString, true, 16, new int[] { }, null);
                var overs = dayInfos.Details.Where(x => x.IsEndTime && x.ReadServeOverCounter < timesRepeatReadServeOver).ToList();
                if (overs != null && overs.Count > 0)
                {
                    var cfName = BLLConfig.Instance.GetConfigByCode(connectString, eConfigCode.TemplateSoundServeOverTime);
                    var readSoundCf = BLLReadTemplate.Instance.Get(connectString, cfName);
                    if (readSoundCf != null)
                    {
                        for (int i = 0; i < overs.Count; i++)
                            GetSound(new List<int>() { readSoundCf.Id }, "0", overs[i].TableId);

                        BLLDailyRequire.Instance.UpdateCounterRepeatServeOver(connectString, overs.Select(x => x.STT).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            tmerQuetServeOver.Enabled = true;
        }

        private void frmMain_ver3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (comPort.IsOpen)
                comPort.Close();
            if (COM_Printer.IsOpen)
                COM_Printer.Close();
        }

        #region Printer
        private void InPhieuDungDriver(int newNum, int oldNum, string tenquay, string tendichvu, string hoten, string tieude, string ngaygio)
        {
            LocalReport localReport = new LocalReport();
            try
            {
                //link cài report viewer cho máy client
                //https://www.microsoft.com/en-us/download/details.aspx?id=6442

                string fullPath = Application.StartupPath + "\\RDLC_Template\\Mau1.rdlc";
                // MessageBox.Show(fullPath);
                localReport.ReportPath = fullPath;
                ReportParameter[] reportParameters = new ReportParameter[4];
                reportParameters[0] = new ReportParameter("TenDV", tenquay.ToUpper());
                reportParameters[1] = new ReportParameter("TenBN", hoten.ToUpper());
                reportParameters[2] = new ReportParameter("Stt", newNum.ToString());
                reportParameters[3] = new ReportParameter("TieuDe", tieude.ToUpper());
                reportParameters[4] = new ReportParameter("NgayGio", ngaygio.ToUpper());

                localReport.SetParameters(reportParameters);
                for (int i = 0; i < 1; i++)
                {
                    PrintToPrinter(localReport);
                }
            }
            catch (Exception ex)
            {
                localReport.Dispose();
                throw ex;
            }
        }

        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;
        public static void PrintToPrinter(LocalReport report)
        {
            Export(report);
        }

        public static void Export(LocalReport report, bool print = true)
        {
            try
            {
                string deviceInfo =
                 @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>18.3in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>5in</MarginBottom>
            </DeviceInfo>";
                Warning[] warnings;
                m_streams = new List<Stream>();
                report.Render("Image", deviceInfo, CreateStream, out warnings);
                foreach (Stream stream in m_streams)
                    stream.Position = 0;

                if (print)
                {
                    Print();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void Print()
        {
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    throw new Exception("Error: no stream to print.");
                PrintDocument printDoc = new PrintDocument();
                if (!printDoc.PrinterSettings.IsValid)
                {
                    throw new Exception("Error: cannot find the default printer.");
                }
                else
                {
                    printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                    m_currentPageIndex = 0;
                    printDoc.Print();
                }
            }
            catch (Exception)
            {
            }
        }

        public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Metafile pageImage = new
                   Metafile(m_streams[m_currentPageIndex]);

                // Adjust rectangular area with printer margins.
                Rectangle adjustedRect = new Rectangle(
                    ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                    ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                    ev.PageBounds.Width,
                    ev.PageBounds.Height);

                // Draw a white background for the report
                ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

                // Draw the report content
                ev.Graphics.DrawImage(pageImage, adjustedRect);

                // Prepare for the next page. Make sure we haven't hit the end.
                m_currentPageIndex++;
                ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
            }
            catch (Exception)
            {
            }
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
        #endregion

        public void docFileExcel()
        {
            try
            {
                string path = @"C:\Users\DoNguyen\Desktop\1595493383628734.xlsx";
                ExcelApp.Application excelApp = new ExcelApp.Application();
                ExcelApp.Workbook excelBook = excelApp.Workbooks.Open(path);
                ExcelApp._Worksheet excelSheet = excelBook.Sheets[2];
                ExcelApp.Range excelRange = excelSheet.UsedRange;

                List<TinhModel> parents = new List<TinhModel>();
                var childs = new List<HuyenModel>();
                for (int ii = 10; ii < 73; ii++)
                {
                    string matinh = excelRange.Cells[ii, 3].Value2.ToString();
                    string tentinh = excelRange.Cells[ii, 4].Value2.ToString();
                    parents.Add(new TinhModel() { Code = matinh, Name = tentinh });
                }
                parents = parents.OrderBy(x => x.Name).ToList();

                excelSheet = excelBook.Sheets[1];
                excelRange = excelSheet.UsedRange;
                for (int ii = 10; ii < 705; ii++)
                {
                    string maquan = excelRange.Cells[ii, 3].Value2.ToString();
                    string tenquan = excelRange.Cells[ii, 6].Value2.ToString();
                    string tentinh = excelRange.Cells[ii, 4].Value2.ToString();
                    childs.Add(new HuyenModel()
                    {
                        Code = maquan,
                        Name = tenquan,
                        TName = tentinh
                    });
                }

                foreach (var item in parents)
                {
                    var founds = childs.Where(x => x.TName == item.Name).OrderBy(x => x.Name).ToList();
                    item.Huyens = founds;
                }

                string json = JsonConvert.SerializeObject(parents);
            }
            catch (Exception)
            {
            }
        }
    }

    public class newObj
    {
        public int PhongBan_Id { get; set; }
        public string MaPhongBan { get; set; }
        public string TenPhongBan { get; set; }
        public string TenKhongDau { get; set; }
        public string TenPhongBan_En { get; set; }
        public int Cap { get; set; }
        public int CapTren_Id { get; set; }
        public string LoaiPhongBan { get; set; }
    }

    public class TinhModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<HuyenModel> Huyens { get; set; }
        public TinhModel()
        {
            Huyens = new List<HuyenModel>();
        }
    }
    public class HuyenModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string TName { get; set; }
    }
}
