using QMS_System.Data.BLL;
using QMS_System.Data.Model;
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
            lookUpUser.DataSource = BLLUser.Instance.GetLookUp();
            lookUpUser.DisplayMember = "Name";
            lookUpUser.ValueMember = "Id";

            //Load SelectBox Thiet bi
            lookUpEquip.DataSource = null;
            lookUpEquip.DataSource = BLLEquipment.Instance.GetLookUp();
            lookUpEquip.DisplayMember = "Name";
            lookUpEquip.ValueMember = "Id";

            //Load SelectBox Trạng Thái
            lookUpStatus.DataSource = null;
            lookUpStatus.DataSource = BLLStatus.Instance.GetLookUp();
            lookUpStatus.DisplayMember = "Name";
            lookUpStatus.ValueMember = "Id";

            var list = BLLLoginHistory.Instance.Gets();
            gridLoginHistory.DataSource = list;
        }

        private void btnResetMajor_Click(object sender, EventArgs e)
        {
            GetGridLoginHistory();
        }
    }
}
