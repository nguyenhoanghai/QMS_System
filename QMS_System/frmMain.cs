﻿using GPRO.Core.Hai;
using Newtonsoft.Json;
using QMS_System.Data.BLL;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using QMS_System.Helper;
using QMS_System.IssueTicketScreen;
using QMS_System.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace QMS_System
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        SoundPlayer player;
        List<string> playlist, dataSendToComport, equipmentIds, arrStr,
          temp = new List<string>();
        Thread playThread;

        List<ConfigModel> configs;
        int q = -1,
            TVReadSound = 0,
            startNumber = 0,
            timeQuetComport = 50,
         PrintTicketCode = 50,
            system = 0,
            timesRepeatReadServeOver = 1,
            autoCallFollowMajorOrder = 0,
            silenceTime = 100,
            printTicketReturnCurrentNumberOrServiceCode = 1,
            _8cUseFor = 1,
            UsePrintBoard = 1;

        public static int
            UseWithThirdPattern = 0,
            printType = 1,
            AutoCallIfUserFree = 0;

        string soundPath = "",

            SoundLockPrintTicket = "khoa.wav",

            CheckTimeBeforePrintTicket = "0";

        bool isSendDataKeyPad = false,
            isFinishRead = true,
            isFirstLoad = false,
            isErorr = false,
            isRunning = false,
            sqlStatus = false,
            autoCall = false;
        public static List<ServiceDayModel> lib_Services;
        public static List<RegisterUserCmdModel> lib_RegisterUserCmds;
        public static List<LoginHistoryModel> lib_Users;
        public static List<UserMajorModel> lib_UserMajors;
        public static List<UserCmdReadSoundModel> lib_UserCMDReadSound;
        public static List<EquipmentModel> lib_Equipments;
        public static List<SoundModel> lib_Sounds;
        public static List<ReadTemplateModel> lib_ReadTemplates;
        public static List<CounterSoundModel> lib_CounterSound;

        public static DateTime today = DateTime.Now;
        public static bool IsDatabaseChange = false;
        public static SerialPort
            comPort = new SerialPort(),
            COM_Printer = new SerialPort();
        public string lbSendrequest, lbSendData, lbPrinRequire;
        public static string
            ticketTemplate = string.Empty,
             errorsms = "",
            connectString;
        public static int
            solien = 1,
            appVersion = 1;


        Thread sendMailThread;
        string mailSend = "gpro.alert2016@gmail.com",
            password = "A123456!@#b",
            mailRecieve = "trivodai@yahoo.com,gpro.alert2016@gmail.com",
            tieuDeMail = "GPRO - QMS";
        TimeSpan? TimeSendMail = TimeSpan.Parse("15:10:00");
        bool mailSending = false,
            useSendMail = false;

        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            getSystemConfig();
        }

        public void getSystemConfig()
        {
            try
            {
                connectString = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
                // MessageBox.Show(connectString);
                if (isRunning)
                    btRunProcess.PerformClick();
                sqlStatus = BaseCore.Instance.CONNECT_STATUS(Application.StartupPath + "\\DATA.XML");
                if (sqlStatus)
                {
                    connectString = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
                    //MessageBox.Show(connectString);
                    QMSAppInfo.ConnectString = connectString;
                    QMSAppInfo.sqlConnection = DatabaseConnection.Instance.Connect(Application.StartupPath + "\\DATA.XML");
                    configs = BLLConfig.Instance.Gets(connectString, true);
                    soundPath = GetConfigByCode(eConfigCode.SoundPath);
                    ticketTemplate = GetConfigByCode(eConfigCode.TicketTemplate);
                    SoundLockPrintTicket = GetConfigByCode(eConfigCode.SoundLockPrintTicket);
                    CheckTimeBeforePrintTicket = GetConfigByCode(eConfigCode.CheckTimeBeforePrintTicket);

                    int.TryParse(GetConfigByCode(eConfigCode.PrintType), out printType);
                    int.TryParse(GetConfigByCode(eConfigCode.PrintTicketReturnCurrentNumberOrServiceCode), out printTicketReturnCurrentNumberOrServiceCode);
                    int.TryParse(GetConfigByCode(eConfigCode.NumberOfLinePerTime), out solien);
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

                    if (Settings.Default.Today != DateTime.Now.Day)
                    {
                        try
                        {
                            errorsms = "sang ngày mới " + DateTime.Now.Day;
                            BLLLoginHistory.Instance.ResetDailyLoginInfor(connectString, today, GetConfigByCode(eConfigCode.AutoSetLoginFromYesterday));
                            BLLDailyRequire.Instance.CopyHistory(connectString, false);
                            Settings.Default.Today = DateTime.Now.Day;
                            Settings.Default.Save();
                            //ko su dung GO 
                            var query = @"  UPDATE [Q_COUNTER] set lastcall = 0 ,isrunning =1, CurrentNumber=0, LastCallKetLuan= 0    
                                            DELETE [Q_CounterSoftSound]  
                                            DBCC CHECKIDENT('Q_CounterSoftSound', RESEED, 1);                                             
                                            DELETE [dbo].[Q_RequestTicket]  
                                            DBCC CHECKIDENT('Q_RequestTicket', RESEED, 1); 
                                            DELETE [dbo].[Q_TVReadSound] 
                                            DBCC CHECKIDENT('Q_TVReadSound', RESEED, 1);   ";
                            BLLSQLBuilder.Instance.Excecute(connectString, query);
                        }
                        catch (Exception ex) { }
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

                    CounterProcess(("AA 03 AA 03 8A 34 56").Split(' ').ToArray(), 0);
                }
                else
                {
                    errorsms = "Kết nối máy chủ SQL thất bại. Vui lòng kiểm tra lại.";
                    Form form = new FrmSQLConnect();
                    form.ShowDialog();
                }
            }
            catch (Exception)
            {
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
            frmIssueTicketScreen frm = new frmIssueTicketScreen(this, null);
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
            Form frm = IsActive(typeof(frmTicketTemplate));
            if (frm == null)
            {
                var forms = new frmTicketTemplate();
                forms.MdiParent = this;
                forms.Show();
            }
            else
                frm.Activate();
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

        private void btReloadApp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getSystemConfig();
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string content = "nguyễn hoàng hải";
            BaseCore.Instance.PrintTicketTCVN3(COM_Printer, content);
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
                        //  MessageBox.Show(temp.Count.ToString());
                        // MessageBox.Show(temp[0]);
                        var _path = soundPath + temp[0];
                        if (File.Exists(_path))
                        {
                            //  MessageBox.Show(_path + " _ Exists");
                            player.SoundLocation = (_path);

                            // MessageBox.Show(SoundInfo.GetSoundLength(player.SoundLocation.Trim()).ToString());
                            int iTime = SoundInfo.GetSoundLength(player.SoundLocation.Trim()) + silenceTime;
                            player.Play();
                            Thread.Sleep(iTime);
                            if (temp.Count > 0)
                                temp.Remove(temp[0]);
                        }
                        else
                        {
                            // MessageBox.Show(_path + " _ not Exists");
                            if (temp.Count > 0)
                            {
                                temp.Remove(temp[0]);
                                errorsms = "Không tìm được tệp âm thanh " + temp[0] + " ngưng cấp phiếu dịch vụ. Vui lòng kiểm tra lại đường dẫn thư mục âm thanh hoặc tệp âm thanh không tồn tại.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //  MessageBox.Show(ex.Message);
                    }
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
                frmMain.comPort.PortName = GetConfigByCode(eConfigCode.ComportName);
                frmMain.comPort.BaudRate = 9600;
                frmMain.comPort.DataBits = 8;
                frmMain.comPort.Parity = Parity.None;
                frmMain.comPort.StopBits = StopBits.One;
                try
                {
                    frmMain.comPort.ReadTimeout = 1;
                    frmMain.comPort.WriteTimeout = 1;
                    frmMain.comPort.Open();
                    barButtonItem8.Glyph = global::QMS_System.Properties.Resources.if_port_64533;
                    frmMain.comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
                }
                catch (Exception)
                {
                    // MessageBox.Show("Lỗi: không thể kết nối với cổng COM Keypad, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Lấy thông tin Com Keypad bị lỗi.\n" + ex.Message, "Lỗi Com Keypad");
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
                int length = frmMain.comPort.BytesToRead;
                byte[] buf = new byte[length];
                frmMain.comPort.Read(buf, 0, length);
                //  MessageBox.Show(BitConverter.ToString(buf));

                var hexStr = BitConverter.ToString(buf).Split('-').ToArray();
                lbRecieve.Caption = hexStr[1] + "," + hexStr[2];
                lib_Users = BLLLoginHistory.Instance.GetsForMain(connectString);
                //  
                if (hexStr.Length > 1)
                {
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
            catch (Exception) { }
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
            IsDatabaseChange = true;
            int lastTicket = 0,
                newNumber = 0,
            nghiepVu = 0;
            string printStr = string.Empty,
                tenQuay = string.Empty;
            bool err = false;
            ServiceDayModel serObj = null;
            DateTime now = DateTime.Now;
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
                                newNumber = ((int)rs.Data + 1);
                                tenQuay = rs.Data_2;
                                lastTicket = (int)rs.Records;
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
                                newNumber = ((int)rs.Data + 1);
                                tenQuay = rs.Data_2;
                                lastTicket = (int)rs.Records;
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
                                tenQuay = rs.Data_2;
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
                        dataSendToComport.Add("AA " + printerId);
                    dataSendToComport.Add(printStr);
                }
                else
                {
                    PrintWithNoBorad(newNumber, lastTicket, tenQuay, (serObj != null ? serObj.Name : ""), (serObj != null ? serObj.Note : ""));
                }
            }
            else if (newNumber != 0)
                if (UsePrintBoard == 0)
                    PrintWithNoBorad(newNumber, lastTicket, tenQuay, (serObj != null ? serObj.Name : ""), (serObj != null ? serObj.Note : ""));

            if (AutoCallIfUserFree == 1 && nghiepVu > 0)
            {
                var freeUser = (int)BLLDailyRequire.Instance.CheckUserFree(connectString, nghiepVu, serviceId, newNumber, autoCallFollowMajorOrder).Data;
                if (freeUser > 0)
                {
                    var counter = lib_Users.FirstOrDefault(x => x.UserId == freeUser).EquipCode;// freeUser < 10 ? ("0" + freeUser) : freeUser.ToString();
                    var str = ("AA," + BaseCore.Instance.ConvertIntToStringWith0Number(counter) + ",8B,00,00");
                    autoCall = true;
                    CounterProcess(str.Split(',').ToArray(), 0);
                }
            }
            return true;
        }

        private void PrintWithNoBorad(int newNum, int oldNum, string tenQuay, string tendichvu, string noteDichVu)
        {
            if (COM_Printer.IsOpen)
            {
                var template = ticketTemplate;
                var now = DateTime.Now;
                template = template.Replace("[canh-giua]", "\x1b\x61\x01|+|");
                template = template.Replace("[canh-trai]", "\x1b\x61\x00|+|");
                template = template.Replace("[1x1]", "\x1d\x21\x00|+|");
                template = template.Replace("[2x1]", "\x1d\x21\x01|+|");
                template = template.Replace("[3x1]", "\x1d\x21\x02|+|");
                template = template.Replace("[2x2]", "\x1d\x21\x11|+|");
                template = template.Replace("[3x3]", "\x1d\x21\x22|+|");

                template = template.Replace("[STT]", newNum.ToString());
                template = template.Replace("[ten-quay]", tenQuay);
                template = template.Replace("[ten-dich-vu]", tendichvu);
                template = template.Replace("[ghi-chu-dich-vu]", noteDichVu);
                template = template.Replace("[ngay]", ("Ngày: " + now.ToString("dd/MM/yyyy")));
                template = template.Replace("[gio]", ("Giờ: " + now.ToString("HH:mm")));
                template = template.Replace("[dang-goi]", "đang gọi: " + oldNum);
                template = template.Replace("[cat-giay]", "\x1b\x69|+|");

                var arr = template.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                for (int ii = 0; ii < solien; ii++)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        BaseCore.Instance.PrintTicketTCVN3(COM_Printer, arr[i]);
                    }
                }
            }
            else
                errorsms = "Cổng COM máy in hiện tại chưa kết nối. Vui lòng kiểm tra lại COM máy in";

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
                            // lbSendData = dataSendToComport[q];
                            lbQuet.Caption = dataSendToComport[q].ToString().Replace(' ', ',');
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
                            lbSendrequest = equipmentIds[q];
                            SendRequest(("AA " + equipmentIds[q]));
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
                    //  barButtonItem8.Caption = "ThreadPlay Start";
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
                if (frmMain.comPort.IsOpen)
                {
                    byte[] newMsg = BaseCore.Instance.HexStringToByteArray(value);
                    frmMain.comPort.Write(newMsg, 0, newMsg.Length);
                    Thread.Sleep(20);
                }
                // else
                //comPort.Open();
            }
            catch
            { }
        }

        private void CounterProcess(string[] _hexStr, int requireId)
        {
            try
            {
                var hexStr = _hexStr.ToList();
                if (hexStr.Count > 5)
                    hexStr.RemoveRange(0, 2);

                LogWriter.LogWrite(string.Format("func CounterProcess: hexStr {0}", string.Join(" ", hexStr)));

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
                                            break;
                                        case eActionParam.HuyKH:
                                            currentTicket = BLLDailyRequire.Instance.DeleteTicket(connectString, int.Parse((hexStr[3] + hexStr[4])), today);
                                            break;
                                        case eActionParam.HITHI:
                                            currentTicket = BLLDailyRequire.Instance.CurrentTicket(connectString, userId, equipCode, today, UseWithThirdPattern);
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
                                                    break;
                                            }
                                            break;
                                        case eActionParam.GDENQ:
                                        case eActionParam.GLAYP:
                                            ticketInfo = BLLDailyRequire.Instance.CallNewTicket_GLP_NghiepVu(connectString, userMajors.Select(x => x.MajorId).ToArray(), userId, equipCode, DateTime.Now, UseWithThirdPattern);
                                            break;
                                        case eActionParam.COUNT: // goi bat ky 
                                            int num = int.Parse((hexStr[3] + "" + hexStr[4]));
                                            var rs = BLLDailyRequire.Instance.CallAny(connectString, userMajors[0].MajorId, userId, equipCode, num, DateTime.Now, UseWithThirdPattern);
                                            if (rs.IsSuccess)
                                                ticketInfo = rs.Data_3;// new TicketInfo() { TicketNumber = num, TicketNumber_3 = rs.Data_2, StartTime = rs.Data_1, TimeServeAllow = rs.Data };

                                            break;
                                        case eActionParam.GNVYC: // goi theo Nghiep vu  
                                            var found = BLLDailyRequire.Instance.CallByMajor(connectString, int.Parse(userRight[i].Param), userId, equipCode, DateTime.Now, UseWithThirdPattern);
                                            if (found.IsSuccess)
                                                ticketInfo = found.Data_3;// new TicketInfo() { TicketNumber = found.Data, StartTime = found.Data_2, TimeServeAllow = found.Data_1 };
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
                                                        ticketInfo = callInfo.Data_3;// new TicketInfo() { TicketNumber = callInfo.Data, StartTime = callInfo.Data_2, TimeServeAllow = callInfo.Data_1 };

                                                    break;
                                                }
                                            }
                                            break;
                                        case eActionParam.GOI_PHIEU_TRONG: // goi phieu trống số phiếu =-1
                                            var c = BLLDailyRequire.Instance.InsertAndCallEmptyTicket(connectString, equipCode);
                                            if (c.IsSuccess)
                                                ticketInfo = c.Data_3;// new TicketInfo() { TicketNumber = c.Data, StartTime = c.Data_2, TimeServeAllow = c.Data_1 };
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
                                                dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
                                                autoCall = false;
                                            }
                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.TKHDG:
                                            arrStr = BaseCore.Instance.ChangeNumber(BLLDailyRequire.Instance.CountTicketDoneProcessed(connectString, equipCode));

                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.THKCG:
                                            arrStr = BaseCore.Instance.ChangeNumber(BLLDailyRequire.Instance.CountTicketWatingProcessed(connectString, equipCode));

                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.HITHI: // gui so len counter
                                            arrStr = BaseCore.Instance.ChangeNumber(currentTicket);
                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
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
                                                if (frmMain.comPort.IsOpen)
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 00 00"));
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
                                                if (frmMain.comPort.IsOpen)
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 00 00"));
                                                currentTicket = 0;
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                        End: { }
                    }
                }
                else
                    errorsms = ("Không tìm thấy thông tin đăng nhập của thiết bị " + equipCode);
                //  MessageBox.Show("Không tìm thấy thông tin đăng nhập của thiết bị " + equipCode, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Lỗi hàm CounterProcess => " + ex.Message);
            }
            if (requireId != 0)
                BLLCounterSoftRequire.Instance.Delete(requireId, connectString);
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
                try
                {
                    errorsms = "sang ngày mới " + DateTime.Now.Day;
                    BLLLoginHistory.Instance.ResetDailyLoginInfor(connectString, today, GetConfigByCode(eConfigCode.AutoSetLoginFromYesterday));
                    BLLDailyRequire.Instance.CopyHistory(connectString, false);
                    Settings.Default.Today = DateTime.Now.Day;
                    Settings.Default.Save();
                    //ko su dung GO 
                    var query = @"  DELETE from Q_CounterSoftSound
                                    DELETE from Q_CounterSoftRequire  
                                    DBCC CHECKIDENT('Q_CounterSoftSound', RESEED, 1);
                                    UPDATE [Q_COUNTER] set lastcall = 0 ,isrunning =1, CurrentNumber=0 , LastCallKetLuan= 0  
                                    DELETE FROM [dbo].[Q_RequestTicket]  
                                    DBCC CHECKIDENT('Q_RequestTicket', RESEED, 1); 
                                    DELETE FROM [dbo].[Q_TVReadSound] 
                                    DBCC CHECKIDENT('Q_TVReadSound', RESEED, 1); ";
                    BLLSQLBuilder.Instance.Excecute(connectString, query);
                    lib_Users = BLLLoginHistory.Instance.GetsForMain(connectString);
                }
                catch (Exception ex)
                {
                    //   MessageBox.Show(ex.ToString(), "New day");
                }
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
                        switch (requires[i].Type)
                        {
                            case (int)eCounterSoftRequireType.inPhieu:
                                requireObj = JsonConvert.DeserializeObject<PrinterRequireModel>(requires[i].Content);
                                PrintWithNoBorad(requireObj.newNumber, requireObj.oldNumber, requireObj.TenQuay, requireObj.TenDichVu, "");
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
                    frmMain.comPort.Close();
                    frmMain.comPort.Dispose();
                    COM_Printer.Close();
                    COM_Printer.Dispose();
                    timerDo.Enabled = false;
                    tmerQuetServeOver.Enabled = false;
                    tmerCheckSendMail.Enabled = false;
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

                    equipmentIds = BLLEquipment.Instance.Gets(connectString);
                    lib_RegisterUserCmds = BLLRegisterUserCmd.Instance.Gets(connectString);
                    lib_UserMajors = BLLUserMajor.Instance.Gets(connectString);
                    lib_UserCMDReadSound = BLLUserCmdReadSound.Instance.Gets(connectString);
                    lib_Equipments = BLLEquipment.Instance.Gets(connectString, (int)eEquipType.Counter);
                    lib_Sounds = BLLSound.Instance.Gets(connectString);
                    lib_ReadTemplates = BLLReadTemplate.Instance.GetsForMain(connectString);
                    lib_CounterSound = BLLCounterSound.Instance.Gets(connectString);
                    lib_Services = BLLService.Instance.GetsForMain(connectString);

                    int time = 1000, _int = 0;
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

                    int.TryParse(GetConfigByCode(eConfigCode.SendMail), out _int);
                    if (_int == 1)
                    {
                        tmerCheckSendMail.Enabled = true;
                        mailSend = GetConfigByCode(eConfigCode.MailFrom);
                        password = GetConfigByCode(eConfigCode.MailFromPass);
                        mailRecieve = GetConfigByCode(eConfigCode.MailTo);
                        TimeSendMail = TimeSpan.Parse(GetConfigByCode(eConfigCode.TimeSendMail));
                        tieuDeMail = GetConfigByCode(eConfigCode.MailSubject);
                    }
                }
                lbRecieve.Caption = "";
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show(ex.Message);
            }
            //  DatabaseConnection.Instance.GetConnectionString("");
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

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmMain.comPort.IsOpen)
                frmMain.comPort.Close();
            if (COM_Printer.IsOpen)
                COM_Printer.Close();
        }


        #region Send mail 12/11/2018 
        private void tmerCheckSendMail_Tick(object sender, EventArgs e)
        {
            //LogWriter.LogWrite(TimeSendMail.Value.ToString());
            //LogWriter.LogWrite(("mailSending: "+mailSending.ToString()));
            if (TimeSendMail.HasValue && !mailSending)
            {
                //  TimeSendMail = TimeSpan.Parse("11:00:00");
                // mailSending = false;
                //LogWriter.LogWrite(TimeSendMail.Value.ToString());
                TimeSpan dateTimeNow = DateTime.Now.TimeOfDay;
                TimeSpan timeNow = TimeSpan.Parse(dateTimeNow.Hours.ToString() + ":" + dateTimeNow.Minutes.ToString() + ":00");
                if (timeNow == TimeSendMail.Value)
                {
                    this.lbsendmailStatus.Caption = "bắt đầu gửi mail.";

                    sendMailThread = new Thread(this.SendMails);
                    sendMailThread.IsBackground = true;
                    sendMailThread.Start();
                }
            }
        }

        private void SendMails()
        {
            mailSending = true;
            this.Invoke((MethodInvoker)delegate
            {
                this.tmerCheckSendMail.Enabled = false;
            });
            try
            {
                clsMail mail = new clsMail();
                mail.Type = "Google";
                mail.Host = "smtp.gmail.com  ";
                mail.Port = 587;
                mail.From = mailSend;
                mail.DisplayName = "";
                mail.Password = password;
                mail.To = mailRecieve;
                mail.Subject = "GPRO - QMS";
                mail.Body = tieuDeMail;

                // bao cao ngay
                string fileCT = BaoCaoChiTietNgay();
                if (!string.IsNullOrEmpty(fileCT))
                    mail.AddAttachment(fileCT);

                string fileTH = BaoCaoTongHopNgay();
                if (!string.IsNullOrEmpty(fileTH))
                    mail.AddAttachment(fileTH);

                mail.SendMail();

                if (!string.IsNullOrEmpty(fileCT) && File.Exists(fileCT))
                    File.Delete(fileCT);

                if (!string.IsNullOrEmpty(fileTH) && File.Exists(fileTH))
                    File.Delete(fileTH);
            }
            catch (Exception ex)
            {
                // threadSendMail.Abort();
                MessageBox.Show("Lỗi gửi mail: " + ex.Message);
            }
            Thread.Sleep(60000);
            this.Invoke((MethodInvoker)delegate
            {
                this.tmerCheckSendMail.Enabled = true;
                mailSending = false;
                this.lbsendmailStatus.Caption = "kết thúc gửi mail.";
            });
            sendMailThread.Abort();
        }

        public void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private string BaoCaoTongHopNgay()
        {
            string templatePath = System.Windows.Forms.Application.StartupPath + @"\ReportTemplate\inDayNhanVien_th.xlsx";
            Excel.Application xlApp;
            Excel.Worksheet xlSheet;
            Excel.Workbook xlBook;
            Excel.Range oRng;
            //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
            object missValue = System.Reflection.Missing.Value;
            //khoi tao doi tuong Com Excel moi
            xlApp = new Excel.Application();
            xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlBook.CheckCompatibility = false;
            xlBook.DoNotPromptForConvert = true;
            //su dung Sheet dau tien de thao tac
            xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
            //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
            xlApp.Visible = false;
            try
            {
                //  var models = BLLReport.Instance.DetailReport(connectString, -1, 3, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateTime.Now);
                var models = BLLReport.Instance.GeneralReport(connectString, 0, 1, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateTime.Now);
                if (models != null)
                {
                    xlSheet.Cells[2, 2] = string.Format("Ngày {0} Tổng số lượt khách giao dịch: {1}", DateTime.Now.ToString("dd/MM/yyyy"), models.Count.ToString());

                    int row = 4;
                    int cell = 2;
                    for (int i = 0; i < models.Count; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            switch (j)
                            {
                                case 0: xlSheet.Cells[row, cell] = (i + 1); break; // "=\"" + DateTime.ParseExact(models[i].Date, "dd/MM/yyyy", CultureInfo.CurrentCulture).ToString("dd/MM/yyyy") + "\"";   break;
                                case 1: xlSheet.Cells[row, cell] = models[i].Name; break;
                                case 2: xlSheet.Cells[row, cell] = models[i].TotalTransaction; break;
                                case 3: xlSheet.Cells[row, cell] = models[i].TotalTransTime; break;
                                case 4: xlSheet.Cells[row, cell] = models[i].AverageTimePerTrans; break;
                            }
                            cell++;
                        }
                        cell = 2;
                        row++;
                    }
                }

                //save file
                if (Directory.Exists(Application.StartupPath + @"\Export"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Export");
                xlBook.SaveAs(Application.StartupPath + @"\Export\BaocaoTH_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".xlsx", Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                return Application.StartupPath + @"\Export\BaocaoTH_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".xlsx";
            }
            catch (System.Exception ex)
            {
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                MessageBox.Show(ex.Message);
            }
            return "";
        }

        private string BaoCaoChiTietNgay()
        {
            string templatePath = System.Windows.Forms.Application.StartupPath + @"\ReportTemplate\inDayNhanVien.xlsx";
            Excel.Application xlApp;
            Excel.Worksheet xlSheet;
            Excel.Workbook xlBook;
            Excel.Range oRng;
            //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
            object missValue = System.Reflection.Missing.Value;
            //khoi tao doi tuong Com Excel moi
            xlApp = new Excel.Application();
            xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlBook.CheckCompatibility = false;
            xlBook.DoNotPromptForConvert = true;
            //su dung Sheet dau tien de thao tac
            xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
            //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
            xlApp.Visible = false;
            try
            {
                var models = BLLReport.Instance.DetailReport(connectString, 0, 1, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateTime.Now);
                if (models != null)
                {
                    xlSheet.Cells[2, 2] = string.Format("Ngày {0} Tổng số lượt khách giao dịch: {1}", DateTime.Now.ToString("dd/MM/yyyy"), models.Count.ToString());

                    int row = 4;
                    int cell = 2;
                    for (int i = 0; i < models.Count; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            switch (j)
                            {
                                case 0: xlSheet.Cells[row, cell] = (i + 1); break; // "=\"" + DateTime.ParseExact(models[i].Date, "dd/MM/yyyy", CultureInfo.CurrentCulture).ToString("dd/MM/yyyy") + "\"";   break;
                                case 1: xlSheet.Cells[row, cell] = models[i].Number; break;
                                case 2: xlSheet.Cells[row, cell] = models[i].UserName; break;
                                case 3: xlSheet.Cells[row, cell] = models[i].MajorName; break;
                                case 4: xlSheet.Cells[row, cell] = models[i].PrintTime.ToString("dd/MM/yyyy HH:mm"); break;
                                case 5: xlSheet.Cells[row, cell] = (models[i].Start.HasValue ? models[i].Start.Value.ToString("dd/MM/yyyy HH:mm") : ""); break;
                                case 6: xlSheet.Cells[row, cell] = (models[i].End.HasValue ? models[i].End.Value.ToString("dd/MM/yyyy HH:mm") : ""); break;
                                case 7: xlSheet.Cells[row, cell] = models[i].ProcessTime; break;
                                case 8: xlSheet.Cells[row, cell] = models[i].WaitingTime; break;
                            }
                            cell++;
                        }
                        cell = 2;
                        row++;
                    }
                }

                //save file
                if (Directory.Exists(Application.StartupPath + @"\Export"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Export");
                xlBook.SaveAs(Application.StartupPath + @"\Export\BaocaoCT_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".xlsx", Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                return Application.StartupPath + @"\Export\BaocaoCT_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".xlsx";
            }
            catch (System.Exception ex)
            {
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                MessageBox.Show(ex.Message);
            }
            return "";
        }

        #endregion
    }
}
