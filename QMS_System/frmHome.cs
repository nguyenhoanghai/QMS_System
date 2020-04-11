using QMS_System.Data.BLL;
using System;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmHome : Form
    { 
        public frmHome( )
        { 
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
                loadData();
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
            switch (QMSAppInfo.Version)
            {
                case 1: gridHome.DataSource = BLLLoginHistory.Instance.GetForHome(frmMain.connectString, frmMain.today, frmMain.UseWithThirdPattern); break;
                case 3: gridHome.DataSource = BLLLoginHistory.Instance.GetForHome_ver3(frmMain_ver3.connectString, frmMain_ver3.today, frmMain_ver3.UseWithThirdPattern); break;
            }
        }
    }
}
