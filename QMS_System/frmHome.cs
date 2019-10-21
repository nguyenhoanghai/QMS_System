using GPRO.Core.Hai;
using QMS_System.Data.BLL;
using QMS_System.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmHome : Form
    {
        frmMain fMain;
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmHome( frmMain _frmMain)
        {
            fMain = _frmMain;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (frmMain.IsDatabaseChange)
                GetGridview();
        }

        private void GetGridview()
        {
            try
            {
                gridHome.DataSource = null;
                gridHome.DataSource = BLLLoginHistory.Instance.GetForHome(connect, frmMain.today, fMain.UseWithThirdPattern);
                frmMain.IsDatabaseChange = false;
            }
            catch (Exception)
            {
            }
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            gridHome.DataSource = null;
            gridHome.DataSource = BLLLoginHistory.Instance.GetForHome(connect, frmMain.today, fMain.UseWithThirdPattern);
        }


        private void gridHome_MouseHover(object sender, EventArgs e)
        {
            tmCountClose.Enabled = false;
        }

        private void gridHome_MouseLeave(object sender, EventArgs e)
        {
            tmCountClose.Enabled = true;
        }

        private void tmCountClose_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
