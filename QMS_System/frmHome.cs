using QMS_System.Data.BLL;
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
        public frmHome()
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
                gridHome.DataSource = null;
                gridHome.DataSource = BLLLoginHistory.Instance.GetForHome(frmMain.today);
                frmMain.IsDatabaseChange = false;
            }
            catch (Exception)
            {
            }
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            gridHome.DataSource = null;
            gridHome.DataSource = BLLLoginHistory.Instance.GetForHome(frmMain.today);
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
