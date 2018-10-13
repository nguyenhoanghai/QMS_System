using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMS_System.IssueTicketScreen
{
    public class FormState
    {
        //private FormWindowState winState;
        //private FormBorderStyle brdStyle;
        //private bool topMost;
        //private Rectangle bounds;

        private bool IsMaximized = false;

        public void FullScreen(Form targetForm)
        {
            if (!IsMaximized)
            {
                IsMaximized = true;
                //Save(targetForm);
                targetForm.WindowState = FormWindowState.Maximized;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.TopMost = true;
                WinApi.SetWinFullScreen(targetForm.Handle);
            }
        }

        //public void Save(Form targetForm)
        //{
        //    winState = targetForm.WindowState;
        //    brdStyle = targetForm.FormBorderStyle;
        //    topMost = targetForm.TopMost;
        //    //bounds = targetForm.Bounds;
        //}

        public void EscapeFullScreen(Form targetForm)
        {
            targetForm.WindowState = FormWindowState.Normal;
            targetForm.FormBorderStyle = FormBorderStyle.Sizable;
            targetForm.TopMost = false;
            IsMaximized = false;
            //targetForm.Bounds = bounds;
           
        }
    }
}
