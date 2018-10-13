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
    public partial class frmCopyRegisterCmd : Form
    {
        public frmCopyRegisterCmd()
        {
            InitializeComponent();
        }

        private void frmCopyRegisterCmd_Load(object sender, EventArgs e)
        {
            GetUser(2);
        }

        private void GetUser(int type)
        {
            var users = BLLUser.Instance.GetLookUp();
            switch (type)
            {
                case 0:
                    lkUser.Properties.DataSource = null;
                    lkUser.Properties.DataSource = users;
                    lkUser.Properties.DisplayMember = "Name";
                    lkUser.Properties.ValueMember = "Id";
                    lkUser.Properties.PopulateColumns();
                    lkUser.Properties.Columns[0].Visible = false;
                    lkUser.Properties.Columns[2].Visible = false;
                    lkUser.Properties.Columns[3].Visible = false;
                    lkUser.Properties.Columns[1].Caption = "Tên nhân viên";
                    break;
                case 1:
                    lkUserRevice.Properties.DataSource = null;
                    lkUserRevice.Properties.DataSource = users;
                    lkUserRevice.Properties.DisplayMember = "Name";
                    lkUserRevice.Properties.ValueMember = "Id";
                    lkUserRevice.Properties.PopulateColumns();
                    lkUserRevice.Properties.Columns[0].Visible = false;
                    lkUserRevice.Properties.Columns[2].Visible = false;
                    lkUserRevice.Properties.Columns[3].Visible = false;
                    lkUserRevice.Properties.Columns[1].Caption = "Tên nhân viên";
                    break;
                case 2:
                    lkUser.Properties.DataSource = null;
                    lkUser.Properties.DataSource = users;
                    lkUser.Properties.DisplayMember = "Name";
                    lkUser.Properties.ValueMember = "Id";
                    lkUser.Properties.PopulateColumns();
                    lkUser.Properties.Columns[0].Visible = false;
                    lkUser.Properties.Columns[2].Visible = false;
                    lkUser.Properties.Columns[3].Visible = false;
                    lkUser.Properties.Columns[1].Caption = "Tên nhân viên";

                    lkUserRevice.Properties.DataSource = null;
                    lkUserRevice.Properties.DataSource = users;
                    lkUserRevice.Properties.DisplayMember = "Name";
                    lkUserRevice.Properties.ValueMember = "Id";
                    lkUserRevice.Properties.PopulateColumns();
                    lkUserRevice.Properties.Columns[0].Visible = false;
                    lkUserRevice.Properties.Columns[2].Visible = false;
                    lkUserRevice.Properties.Columns[3].Visible = false;
                    lkUserRevice.Properties.Columns[1].Caption = "Tên nhân viên";
                    break;
            }

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (gridViewRegister.VisibleColumns.Count > 0)
            {
                var uRecive = (ModelSelectItem)lkUserRevice.GetSelectedDataRow();
                if (uRecive != null)
                {
                    var selectR = gridViewRegister.GetSelectedRows();
                    if (selectR != null && selectR.Length > 0)
                    {
                        List<int> Ids = new List<int>();
                        for (int i = 0; i < selectR.Length; i++)
                            Ids.Add(int.Parse(gridViewRegister.GetRowCellValue(i, "Id").ToString()));

                        if (!BLLRegisterUserCmd.Instance.Copy(Ids, uRecive.Id))
                            MessageBox.Show("Lỗi trong quá trình thực thi dữ liệu không thể sao chép. Vui lòng kiểm tra lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            this.Close();
                    }
                    else
                        MessageBox.Show("Không có lệnh nào được chọn không thể sao chép. Vui lòng kiểm tra lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Vui lòng chọn nhân viên nhận lệnh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Không có lệnh nào được chọn không thể sao chép. Vui lòng kiểm tra lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnResetUser_EditValueChanged(object sender, EventArgs e)
        {
      
        }

        private void btnReset_EditValueChanged(object sender, EventArgs e)
        {
            GetUser(1);
        }

        private void lkUser_EditValueChanged(object sender, EventArgs e)
        {
            GetGridView();
        }
        private void GetGridView()
        {
            var userObj = (ModelSelectItem)lkUser.GetSelectedDataRow();
            if (userObj == null)
                MessageBox.Show("Vui lòng chọn nhân viên", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                gridRegister.DataSource = BLLRegisterUserCmd.Instance.Gets(userObj.Id);
        }

        private void btnResetUser_Click(object sender, EventArgs e)
        {
            GetUser(0);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            GetUser(1);
        }
    }
}
