using GPRO.Core.Hai;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using QMS_System.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmLoginHistory : Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmLoginHistory()
        {
            InitializeComponent();
        }

        private void frmLoginHistory_Load(object sender, EventArgs e)
        {
            GetGridLoginHistory();
        }

        private void GetGridLoginHistory()
        {

            //Load SelectBox Nhan vien
            lookUpUser.DataSource = null;
            lookUpUser.DataSource = BLLUser.Instance.GetLookUp(connect);
            lookUpUser.DisplayMember = "Name";
            lookUpUser.ValueMember = "Id";

            //Load SelectBox Thiet bi
            lookUpEquip.DataSource = null;
            lookUpEquip.DataSource = BLLEquipment.Instance.GetLookUp(connect);
            lookUpEquip.DisplayMember = "Name";
            lookUpEquip.ValueMember = "Id";

            //Load SelectBox Trạng Thái
            lookUpStatus.DataSource = null;
            lookUpStatus.DataSource = BLLStatus.Instance.GetLookUp(connect);
            lookUpStatus.DisplayMember = "Name";
            lookUpStatus.ValueMember = "Id";

            var list = BLLLoginHistory.Instance.Gets(connect);
            gridLoginHistory.DataSource = list;
        }

        private void btnResetMajor_Click(object sender, EventArgs e)
        {
            GetGridLoginHistory();
        }
    }
}
