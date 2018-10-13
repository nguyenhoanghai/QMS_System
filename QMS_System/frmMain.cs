using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using System.Collections;
using QMS_System.Helper;
using QMS_System.Data.BLL;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System.IO.Ports;
using System.Text.RegularExpressions;
using QMS_System.Data;
using System.Configuration;
using QMS_System.IssueTicketScreen;
using System.Diagnostics;
using System.IO;
using QMS_System.Properties;
using Newtonsoft.Json;
using QMS_System.WebApi;

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
            printType = 1,
            startNumber = 0,
            timeQuetComport = 50,
         PrintTicketCode = 50;

        string soundPath = "",
            errorsms = "",
            SoundLockPrintTicket = "khoa.wav",
            CheckTimeBeforePrintTicket = "0";

        bool isSendDataKeyPad = false,
            isFinishRead = true,
            isFirstLoad = false,
            isErorr = false,
            CheckServeOverTime = false,
            isRunning = false;
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
        public static SerialPort comPort = new SerialPort();
        public string lbSendrequest, lbSendData, lbPrinRequire;

        List<CountServeOverTimeModel> countServeOverTime = new List<CountServeOverTimeModel>();
        int timesRepeatReadServeOver = 1;

        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            BLLUserEvaluate.Instance.Evaluate("nv16", "1_1", 0, "0");

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
            frmIssueTicketScreen frm = new frmIssueTicketScreen(this);
            frm.StartPosition = FormStartPosition.CenterScreen;
            // frm.TopMost = true;
            frm.Show();
        }

        private void btnDeleteTicketInDay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa hết các vé cấp trong ngày không?.", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
                if (!BLLDailyRequire.Instance.DeleteAllTicketInDay(today))
                    MessageBox.Show("Lỗi thực thi yêu cầu Xóa vé đã cấp trong ngày. Vui lòng kiểm tra lại.", "Lỗi thực thi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    IsDatabaseChange = true;
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

        private void ReadSound(List<string> sounds)
        {
            if (temp.Count > 0)
            {
                while (temp.Count > 0)
                {
                    if (File.Exists(soundPath + temp[0]))
                    {
                        player.SoundLocation = (soundPath + temp[0]);
                        int iTime = SoundInfo.GetSoundLength(player.SoundLocation.Trim()) - 0;
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
                //  var lib_Sounds = BLLSound.Instance.Gets();
                SoundModel soundFound;
                CounterSoundModel ctSound;
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
                                            playlist.Add(soundFound.Name);
                                    }
                                }
                                else if (readDetails[y].Details[i].SoundId == (int)eSoundConfig.Counter)
                                {
                                    ctSound = lib_CounterSound.FirstOrDefault(x => x.CounterId == counterId && x.LanguageId == readDetails[y].LanguageId);
                                    playlist.Add((ctSound == null ? "" : ctSound.SoundName));//  BLLCounterSound.Instance.GetSoundName(counterId, ));
                                }
                                else
                                {
                                    soundFound = lib_Sounds.FirstOrDefault(x => x.Id == readDetails[y].Details[i].SoundId);
                                    if (soundFound != null)
                                        playlist.Add(soundFound.Name);
                                }
                            }
                        }
                    }
                    temp.AddRange(playlist);
                }
            }
        }
        #endregion

        #region Keypad COMPort
        private void InitComPort()
        {
            try
            {
                comPort.PortName = GetConfigByCode(eConfigCode.ComportName);
                comPort.BaudRate = Convert.ToInt32(GetConfigByCode(eConfigCode.BaudRate));
                comPort.DataBits = Convert.ToInt32(GetConfigByCode(eConfigCode.DataBits));
                int parity = int.Parse(GetConfigByCode(eConfigCode.Parity));
                switch (parity)
                {
                    case 0: frmMain.comPort.Parity = Parity.None; break;
                    case 1: frmMain.comPort.Parity = Parity.Odd; break;
                    case 2: frmMain.comPort.Parity = Parity.Even; break;
                    case 3: frmMain.comPort.Parity = Parity.Mark; break;
                    case 4: frmMain.comPort.Parity = Parity.Space; break;
                }

                int stopbit = int.Parse(GetConfigByCode(eConfigCode.StopBits));
                switch (stopbit)
                {
                    case 0: frmMain.comPort.StopBits = StopBits.None; break;
                    case 1: frmMain.comPort.StopBits = StopBits.One; break;
                    case 2: frmMain.comPort.StopBits = StopBits.Two; break;
                    case 3: frmMain.comPort.StopBits = StopBits.OnePointFive; break;
                }
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
                    MessageBox.Show("Lỗi: không thể kết nối với cổng COM Keypad, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lấy thông tin Com Keypad bị lỗi.\n" + ex.Message, "Lỗi Com Keypad");
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
                byte[] buf = new byte[length];
                comPort.Read(buf, 0, length);
                //  MessageBox.Show(BitConverter.ToString(buf));

                var hexStr = BitConverter.ToString(buf).Split('-').ToArray();
                lbRecieve.Caption = hexStr[1] + "," + hexStr[2];
                //  
                if (hexStr.Length > 1)
                {
                    switch (BLLEquipment.Instance.GetEquipTypeId(int.Parse(hexStr[1])))
                    {
                        case (int)eEquipType.Counter:
                            if (hexStr.Contains("8C"))   ///Login - Logout
                            {
                                if (BLLLoginHistory.Instance.CounterLoginLogOut(int.Parse((hexStr[3] == "FF" ? "00" : hexStr[3]) + "" + hexStr[4]), int.Parse(hexStr[1]), DateTime.Now))
                                    dataSendToComport.Add(("AA " + hexStr[1] + " 00 00"));
                                lib_Users = BLLLoginHistory.Instance.Gets_();
                                IsDatabaseChange = true;
                            }
                            else if (hexStr.Length >= 4)
                                CounterProcess(hexStr);
                            break;
                        case (int)eEquipType.Printer:
                            if (hexStr[2] == "0A")
                                hexStr[2] = "10";

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
                                PrintNewTicket(int.Parse(hexStr[1]), (hexStr[2] == "0A" ? 10 : int.Parse(hexStr[2])), 0, false, false, null);
                            }
                            break;
                    }                   
                }
            }
            catch (Exception) { }
        }

        public void PrintNewTicket(int printerId, int serviceId, int businessId, bool isTouchScreen, bool isProgrammer, TimeSpan? timeServeAllow)
        {
            IsDatabaseChange = true;
            int lastTicket = 0, kq = 0;
            string printStr = string.Empty;
            bool err = false;
            ServiceDayModel serObj;
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
                            var rs = BLLDailyRequire.Instance.PrintNewTicket(serviceId, serObj.StartNumber, businessId, now, printType, (timeServeAllow != null ? timeServeAllow.Value : serObj.TimeProcess.TimeOfDay));
                            if (rs.IsSuccess)
                            {
                                if (!isProgrammer)
                                {
                                    var soArr = GPRO_Helper.Instance.ChangeNumber(((int)rs.Data + 1));
                                    printStr = (soArr[0] + " " + soArr[1] + " ");
                                    soArr = GPRO_Helper.Instance.ChangeNumber((int)rs.Records);
                                    printStr += (soArr[0] + " " + soArr[1] + " " + now.ToString("dd") + " " + now.ToString("MM") + " " + now.ToString("yy") + " " + now.ToString("HH") + " " + now.ToString("mm"));
                                }
                                else if (isProgrammer)
                                    lbRecieve.Caption = printerId + "," + serviceId + "," + ((int)rs.Data + 1);
                                barButtonItem10.Caption = "đang gọi :" + (int)rs.Records;
                            }
                            else
                                MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            var rs = BLLDailyRequire.Instance.PrintNewTicket(serviceId, startNumber, businessId, now, printType, serObj.TimeProcess.TimeOfDay);
                            if (rs.IsSuccess)
                            {
                                if (!isProgrammer)
                                {
                                    var soArr = GPRO_Helper.Instance.ChangeNumber(((int)rs.Data + 1));
                                    printStr = (soArr[0] + " " + soArr[1] + " ");
                                    soArr = GPRO_Helper.Instance.ChangeNumber((int)rs.Records);
                                    printStr += (soArr[0] + " " + soArr[1] + " " + now.ToString("dd") + " " + now.ToString("MM") + " " + now.ToString("yy") + " " + now.ToString("HH") + " " + now.ToString("mm"));
                                }
                                else if (isProgrammer)
                                    lbRecieve.Caption = printerId + "," + serviceId + "," + ((int)rs.Data + 1);
                            }
                            else
                                MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                    break;
                case (int)ePrintType.TheoGioiHanSoPhieu:
                    #region MyRegion
                    int slCP = BLLBusiness.Instance.GetTicketAllow(businessId);
                    int slDacap = BLLDailyRequire.Instance.CountTicket(businessId);
                    if (slDacap != null && slDacap == slCP)
                        MessageBox.Show("Doanh nghiệp của bạn đã được cấp đủ số lượng phiếu giới hạn trong ngày. Xin quý khách vui lòng quay lại sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        lastTicket = BLLDailyRequire.Instance.GetLastTicketNumber(serviceId, today);
                        serObj = lib_Services.FirstOrDefault(x => x.Id == serviceId);
                        if (lastTicket == 0)
                        {
                            if (serObj != null)
                            {
                                err = true;
                                MessageBox.Show("Dịch vụ không tồn tại. Xin quý khách vui lòng chọn dịch vụ khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                MessageBox.Show("Dịch vụ này đã cấp hết phiếu trong ngày. Xin quý khách vui lòng chọn dịch vụ khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (!err)
                        {
                            kq = BLLDailyRequire.Instance.Insert(lastTicket, serviceId, businessId, now);
                            if (kq != 0 && !isProgrammer)
                            {
                                var soArr = GPRO_Helper.Instance.ChangeNumber(lastTicket);
                                printStr = (soArr[0] + " " + soArr[1] + " ");
                                soArr = GPRO_Helper.Instance.ChangeNumber(BLLDailyRequire.Instance.GetCurrentNumber(serviceId));
                                printStr += (soArr[0] + " " + soArr[1] + " " + now.ToString("dd") + " " + now.ToString("MM") + " " + now.ToString("yy") + " " + now.ToString("HH") + " " + now.ToString("mm"));
                            }
                            else if (kq != 0 && isProgrammer)
                                lbRecieve.Caption = serviceId + "," + "1," + lastTicket;
                        }
                    }
                    #endregion
                    break;
            }
            if (!string.IsNullOrEmpty(printStr))
            {
                if (isTouchScreen)
                    dataSendToComport.Add("AA " + printerId);
                dataSendToComport.Add(printStr); 
            } 
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
                byte[] newMsg = GPRO_Helper.Instance.HexStringToByteArray(value);
                comPort.Write(newMsg, 0, newMsg.Length);
                Thread.Sleep(20);
            }
            catch
            { }
        }

        private void CounterProcess(string[] hexStr)
        {
            try
            {
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
                        var sohientai = BLLDailyRequire.Instance.CurentTicket(equipCode);
                        if (code == sohientai)
                            code = 0;

                        if (code > 100)
                            userRight = lib_RegisterUserCmds.Where(x => x.UserId == userId && x.CMDName == hexStr[2] && x.CMDParamName == "100").OrderBy(x => x.Index).ToList();
                        else
                            userRight = lib_RegisterUserCmds.Where(x => x.UserId == userId && x.CMDName == hexStr[2] && x.CMDParamName == code.ToString()).OrderBy(x => x.Index).ToList();
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
                        int newTicket = 0, currentTicket = 0;

                        for (int i = 0; i < userRight.Count; i++)
                        {
                            switch (userRight[i].ActionName)
                            {
                                case eActionCode.HienHanh:
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.HoanTat:
                                            currentTicket = BLLDailyRequire.Instance.DoneTicket(userId, equipCode, today);
                                            break;
                                        case eActionParam.HuyKH:
                                            currentTicket = BLLDailyRequire.Instance.DeleteTicket(int.Parse((hexStr[3] + hexStr[4])), today);
                                            break;
                                        case eActionParam.HITHI:
                                            currentTicket = BLLDailyRequire.Instance.CurrentTicket(userId, equipCode, today);
                                            break;
                                    }
                                    break;
                                case eActionCode.GoiMoi:
                                    #region xu ly goi moi
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.NGVU:
                                            int[] indexs = userMajors.Select(x => x.Index).Distinct().ToArray();
                                            for (int z = 0; z < indexs.Length; z++)
                                            {
                                                newTicket = BLLDailyRequire.Instance.CallNewTicket(userMajors.Where(x => x.Index == indexs[z]).Select(x => x.MajorId).ToArray(), userId, equipCode, true, DateTime.Now);
                                                //  MessageBox.Show("so moi :" + newTicket);
                                                if (newTicket != 0)
                                                    break;
                                            }
                                            break;
                                        case eActionParam.GDENQ:
                                        case eActionParam.GLAYP:
                                            newTicket = BLLDailyRequire.Instance.CallNewTicket_GLP_NghiepVu(userMajors.Select(x => x.MajorId).ToArray(), userId, equipCode, DateTime.Now);
                                            break;
                                        case eActionParam.COUNT: // goi bat ky 
                                            int majorId = int.Parse((hexStr[3] + "" + hexStr[4]));
                                            if (BLLDailyRequire.Instance.CallAny(userMajors[0].MajorId, userId, equipCode, majorId, DateTime.Now).IsSuccess)
                                                newTicket = majorId;
                                            else
                                                newTicket = 0;
                                            break;
                                        case eActionParam.GNVYC: // goi theo Nghiep vu  
                                            var found = BLLDailyRequire.Instance.CallByMajor(int.Parse(userRight[i].Param), userId, equipCode, DateTime.Now);
                                            if (found.IsSuccess)
                                                newTicket = found.Data;
                                            else
                                                newTicket = 0;
                                            break;
                                    }
                                    #endregion
                                    break;
                                case eActionCode.Counter:
                                    #region xu ly gui so len counter
                                    switch (userRight[i].ActionParamName)
                                    {
                                        case eActionParam.MOIKH: // gui so len counter
                                            arrStr = GPRO_Helper.Instance.ChangeNumber(newTicket);
                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.TKHDG:
                                            arrStr = GPRO_Helper.Instance.ChangeNumber(BLLDailyRequire.Instance.CountTicketDoneProcessed(equipCode));
                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.THKCG:
                                            arrStr = GPRO_Helper.Instance.ChangeNumber(BLLDailyRequire.Instance.CountTicketWatingProcessed(equipCode));
                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
                                            break;
                                        case eActionParam.HITHI: // gui so len counter
                                            arrStr = GPRO_Helper.Instance.ChangeNumber(currentTicket);
                                            dataSendToComport.Add(("AA " + hexStr[1] + " " + arrStr[0] + " " + arrStr[1]));
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
                                            mains = BLLMaindisplayDirection.Instance.Gets(equipCode);
                                            if (mains.Count > 0)
                                            {
                                                arrStr = GPRO_Helper.Instance.ChangeNumber(newTicket);
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
                                            mains = BLLMaindisplayDirection.Instance.Gets(equipCode);
                                            if (mains.Count > 0)
                                            {
                                                arrStr = GPRO_Helper.Instance.ChangeNumber(currentTicket);
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
                                            if (newTicket != 0)
                                            {
                                                var readTemplateIds = lib_UserCMDReadSound.Where(x => x.UserId == userId && x.Note.Equals(userRight[i].CMDName.ToUpper())).Select(x => x.ReadTemplateId).ToList();// BLLUserCmdReadSound.Instance.GetReadTemplateIds(userId, userRight[i].CMDName);
                                                if (readTemplateIds.Count > 0)
                                                    GetSound(readTemplateIds, newTicket.ToString(), lib_Equipments.FirstOrDefault(x => x.Code == equipCode).CounterId);
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

                                            if (BLLDailyRequire.Instance.TranferTicket(equipCode, int.Parse(userRight[i].Param), currentTicket, today, false))
                                            {
                                                if (comPort.IsOpen)
                                                    dataSendToComport.Add(("AA " + hexStr[1] + " 00 00"));
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
                }
                else
                    MessageBox.Show("Không tìm thấy thông tin đăng nhập của thiết bị " + equipCode, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hàm CounterProcess => " + ex.Message);
            }
        }


        #endregion

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
            var query = @"delete from Q_CounterSoftSound ";
            if (Settings.Default.Today != DateTime.Now.Day)
            {
                try
                {
                    errorsms = "sang ngày mới " + DateTime.Now.Day;
                    BLLLoginHistory.Instance.ResetDailyLoginInfor(new DateTime(today.Year, today.Month, today.Day), GetConfigByCode(eConfigCode.AutoSetLoginFromYesterday));
                    BLLDailyRequire.Instance.CopyHistory();
                    Settings.Default.Today = DateTime.Now.Day;
                    Settings.Default.CountServeOverTime = "";
                    Settings.Default.Save();
                    query += "update q_counter set lastcall = 0 ,isrunning =1";
                    BLLSQLBuilder.Instance.Excecute(query);
                    lib_Users = BLLLoginHistory.Instance.GetsForMain();
                }
                catch (Exception ex)
                {
                    //   MessageBox.Show(ex.ToString(), "New day");
                }
            }
            else
                errorsms = "";

            if (isFirstLoad)
            {
                if (!string.IsNullOrEmpty(Settings.Default.CountServeOverTime))
                {
                    try
                    {
                        countServeOverTime = JsonConvert.DeserializeObject<List<CountServeOverTimeModel>>(Settings.Default.CountServeOverTime);
                    }
                    catch (Exception ex)
                    {
                        countServeOverTime = new List<CountServeOverTimeModel>();
                    }
                }
                isFirstLoad = false;
            }

            try
            {
                var requires = BLLCounterSoftRequire.Instance.Gets();
                if (requires.Count > 0)
                {
                    IsDatabaseChange = true;
                    bool hasSound = false;
                    for (int i = 0; i < requires.Count; i++)
                    {
                        switch (requires[i].Type)
                        {
                            case (int)eCounterSoftRequireType.ReadSound:
                                temp.AddRange(requires[i].Content.Split('|').ToList());
                                hasSound = true;
                                break;
                            case (int)eCounterSoftRequireType.PrintTicket:
                                string[] arr = requires[i].Content.Split('|').ToArray();
                                int[] timeArr = arr[2].Split(':').Select(x => Convert.ToInt32(x)).ToArray();
                                PrintNewTicket(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 0, true, false, new TimeSpan(timeArr[0], timeArr[1], timeArr[2]));
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
                if (str.Length > 2) PrintNewTicket(str[0], str[1], 0, false, true, null);

            }
        }


        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var str = "AA," + txtmatb.EditValue.ToString() + "," + radioLenh.EditValue.ToString() + "," + txtparam1.EditValue.ToString() + "," + txtparam2.EditValue.ToString();
            CounterProcess(str.Split(',').ToArray());
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
            if (isRunning)
            {
                isRunning = false;
                btRunProcess.Caption = "Chạy tiến trình";
                btRunProcess.LargeGlyph = global::QMS_System.Properties.Resources.if_our_process_2_45123;
                comPort.Close();
                comPort.Dispose();
                timerDo.Enabled = false;
                tmerQuetServeOver.Enabled = false;
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

                equipmentIds = BLLEquipment.Instance.Gets();
                lib_RegisterUserCmds = BLLRegisterUserCmd.Instance.Gets();
                lib_UserMajors = BLLUserMajor.Instance.Gets();
                lib_UserCMDReadSound = BLLUserCmdReadSound.Instance.Gets();
                lib_Equipments = BLLEquipment.Instance.Gets((int)eEquipType.Counter);
                lib_Sounds = BLLSound.Instance.Gets();
                lib_ReadTemplates = BLLReadTemplate.Instance.GetsForMain();
                lib_CounterSound = BLLCounterSound.Instance.Gets();
                lib_Services = BLLService.Instance.GetsForMain();
                configs = BLLConfig.Instance.Gets();
                soundPath = GetConfigByCode(eConfigCode.SoundPath);
                SoundLockPrintTicket = GetConfigByCode(eConfigCode.SoundLockPrintTicket);
                CheckTimeBeforePrintTicket = GetConfigByCode(eConfigCode.CheckTimeBeforePrintTicket);
                int.TryParse(GetConfigByCode(eConfigCode.PrintType), out printType);
                int.TryParse(GetConfigByCode(eConfigCode.StartNumber), out startNumber);
                int.TryParse(GetConfigByCode(eConfigCode.TimeWaitForRecieveData), out timeQuetComport);
                int.TryParse(GetConfigByCode(eConfigCode.PrintTicketCode), out PrintTicketCode);
                int.TryParse(GetConfigByCode(eConfigCode.TimesRepeatReadServeOver), out timesRepeatReadServeOver);
                int time = 1000;
                int.TryParse(GetConfigByCode(eConfigCode.TimerQuetServeOverTime), out time);
                InitComPort();
                lib_Users = BLLLoginHistory.Instance.GetsForMain();

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
            }
            lbRecieve.Caption = "";
        }

        private void tmerQuetServeOver_Tick(object sender, EventArgs e)
        {
            tmerQuetServeOver.Enabled = false;
            try
            {
                var dayInfos = BLLDailyRequire.Instance.GetDayInfo(true, 16, new int[] { }, null);
                var overs = dayInfos.Details.Where(x => x.IsEndTime).ToList();
                if (overs != null && overs.Count > 0)
                {
                    var cfName = BLLConfig.Instance.GetConfigByCode(eConfigCode.TemplateSoundServeOverTime);
                    var readSoundCf = BLLReadTemplate.Instance.Get(cfName);
                    if (readSoundCf != null)
                    {
                        for (int i = 0; i < overs.Count; i++)
                        {
                            var found = countServeOverTime.FirstOrDefault(x => x.CounterId == overs[i].TableId && x.Ticket == overs[i].TicketNumber);
                            if (found != null)
                            {
                                if (found.ReadTimes < timesRepeatReadServeOver)
                                {
                                    found.ReadTimes++;
                                    GetSound(new List<int>() { readSoundCf.Id }, "0", overs[i].TableId);
                                }
                            }
                            else
                            {
                                countServeOverTime.Add(new CountServeOverTimeModel() { ReadTimes = 1, CounterId = overs[i].TableId, Ticket = overs[i].TicketNumber });
                                GetSound(new List<int>() { readSoundCf.Id }, "0", overs[i].TableId);
                            }
                        }
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
            Settings.Default.CountServeOverTime = JsonConvert.SerializeObject(countServeOverTime);
            Settings.Default.Save();
        }
    }
}
