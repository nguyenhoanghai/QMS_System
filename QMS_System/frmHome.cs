using GPRO.Core.Hai;
using QMS_System.Data.BLL;
using System;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmHome : Form
    {
        frmMain fMain;
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmHome(frmMain _frmMain)
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
            loadData();
        }

        private void gridHome_MouseHover(object sender, EventArgs e)
        {
           // tmCountClose.Enabled = false;
        }

        private void gridHome_MouseLeave(object sender, EventArgs e)
        {
           // tmCountClose.Enabled = true;
        }

        private void tmCountClose_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            gridHome.DataSource = null;
            gridHome.DataSource = BLLLoginHistory.Instance.GetForHome(connect, frmMain.today, fMain.UseWithThirdPattern);
        }
    }
}
