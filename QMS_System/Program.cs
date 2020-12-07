using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using SystemGPRO.Serial;

namespace QMS_System
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                SerialKey serialKey = new SerialKey();
                //  ModelStatic.dateCheckActive = DateTime.Now.Date;
                ModelCheckKey modelCheckKey = serialKey.CheckActive("GPRO_QMS", Application.StartupPath);
                if (modelCheckKey != null)
                {
                    if (!modelCheckKey.checkResult)
                    {
                        // if (!string.IsNullOrEmpty(modelCheckKey.message))
                        //       MessageBox.Show(modelCheckKey.message);
                        //   else
                        MessageBox.Show("Phần mềm QMS đã hết hạn sử dụng.\nQuý khách vui lòng liên hệ theo Hotline : Võ Đại Trí 0918319714 hoặc Email : vodaitri@yahoo.com để được tư vấn và kích hoạt sử dụng.\nXin cám ơn quý khách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    else
                    {
                        //Single application
                        string[] commandLineArgs = Environment.GetCommandLineArgs();
                        if (commandLineArgs.Length == 1 || commandLineArgs[1] != "/new")
                        {
                            if (SingleInstanceApplication.NotifyExistingInstance(Process.GetCurrentProcess().Id))
                            {
                                return;
                            }
                        }


                        if (!string.IsNullOrEmpty(modelCheckKey.message))
                            MessageBox.Show(modelCheckKey.message);

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        SingleInstanceApplication.Initialize();

                        QMSAppInfo.Version = (ConfigurationManager.AppSettings["AppVersion"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["AppVersion"].ToString()) ? Convert.ToInt32(ConfigurationManager.AppSettings["AppVersion"].ToString()) : 1);
                        try
                        {
                            switch (QMSAppInfo.Version)
                            {
                                case 1: Application.Run(new frmMain()); break;
                                case 3: Application.Run(new frmMain_ver3()); break;
                                default: Application.Run(new frmMain()); break;
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorsms = "Kết nối máy chủ SQL thất bại. Vui lòng kiểm tra lại.";
                            MessageBox.Show(errorsms + "- " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Run(new FrmSQLConnect());
                        }
                        SingleInstanceApplication.Close();
                        Process[] processe;
                        processe = Process.GetProcessesByName("QMS_System");
                        foreach (Process dovi in processe)
                            dovi.Kill();
                    }
                }
                else
                {
                    MessageBox.Show("Phần mềm QMS chưa được kích hoạt sử dụng.\nQuý khách vui lòng liên hệ theo Hotline : Võ Đại Trí 0918319714 hoặc Email : vodaitri@yahoo.com để được tư vấn và kích hoạt sử dụng.\nXin cám ơn quý khách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("lỗi khác" + ex.Message);
                throw ex;
            }
        }
    }
}
