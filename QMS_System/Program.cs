using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                ModelStatic.dateCheckActive = DateTime.Now.Date;
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
                        if (!string.IsNullOrEmpty(modelCheckKey.message))
                            MessageBox.Show(modelCheckKey.message);

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new frmMain());
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
            } 
        }
    }
}
