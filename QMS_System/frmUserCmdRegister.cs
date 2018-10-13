using QMS_System.Data;
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
    public partial class frmUserCmdRegister : Form
    {
        int Id = 0, userId = 0, cmdId = 0, cmdParamId = 0, acParamId = 0;
        string cmdParamName = "", acParamName = "";
        public frmUserCmdRegister()
        {
            InitializeComponent();
        }

        private void frmUserCmdRegister_Load(object sender, EventArgs e)
        {
            GetUser();
            GetCMD();
            GetCMDParam();
            GetAction();
            btnSave.Enabled = false;
        }

        private void GetAction()
        {
            lkAction.Properties.DataSource = null;
            lkAction.Properties.DataSource = BLLAction.Instance.GetLookUp();
            lkAction.Properties.DisplayMember = "Name";
            lkAction.Properties.ValueMember = "Id";
            lkAction.Properties.PopulateColumns();
            lkAction.Properties.Columns["Id"].Visible = false;
            lkAction.Properties.Columns["Code"].Visible = false;
            lkAction.Properties.Columns["Data"].Visible = false;
            lkAction.Properties.Columns["Name"].Caption = "Tên hành động";
        }

        private void GetCMDParam()
        {
            var obj = (ModelSelectItem)lkCmd.GetSelectedDataRow();
            lkCmdParam.Properties.DataSource = null;
            if (obj != null)
            {
                lkCmdParam.Properties.DataSource = BLLCommandParameter.Instance.GetLookUp(obj.Id);
                lkCmdParam.Properties.DisplayMember = "Name";
                lkCmdParam.Properties.ValueMember = "Id";
                lkCmdParam.Properties.PopulateColumns();
                lkCmdParam.Properties.Columns["Id"].Visible = false;
                lkCmdParam.Properties.Columns["Data"].Visible = false;
                lkCmdParam.Properties.Columns["Name"].Caption = "Tham số hành động";
                lkCmdParam.Properties.Columns["Code"].Caption = "Ghi chú";
                if (!string.IsNullOrEmpty(cmdParamName))
                {
                    lkCmdParam.Text = cmdParamName;
                    cmdParamName = "";
                }
            }
        }

        private void GetCMD()
        {
            lkCmd.Properties.DataSource = null;
            lkCmd.Properties.DataSource = BLLCommand.Instance.GetLookUp();
            lkCmd.Properties.DisplayMember = "Name";
            lkCmd.Properties.ValueMember = "Id";
            lkCmd.Properties.PopulateColumns();
            lkCmd.Properties.Columns[0].Visible = false;
            lkCmd.Properties.Columns[3].Visible = false;
            lkCmd.Properties.Columns[1].Caption = "Mã lệnh";
            lkCmd.Properties.Columns[2].Caption = "Mã lệnh HEX";
        }

        private void GetUser()
        {
            lkUser.Properties.DataSource = null;
            lkUser.Properties.DataSource = BLLUser.Instance.GetLookUp();
            lkUser.Properties.DisplayMember = "Name";
            lkUser.Properties.ValueMember = "Id";
            lkUser.Properties.PopulateColumns();
            lkUser.Properties.Columns[0].Visible = false;
            lkUser.Properties.Columns[2].Visible = false;
            lkUser.Properties.Columns[3].Visible = false;
            lkUser.Properties.Columns[1].Caption = "Tên nhân viên";
        }

        private void lkCmd_TextChanged(object sender, EventArgs e)
        {
            var obj = (ModelSelectItem)lkCmd.GetSelectedDataRow();
            if (obj != null)
            {
                cmdId = obj.Id;
                GetGridView();
            }
            else
                cmdId = 0;
            GetCMDParam();
        }


        private void GetGridView()
        {
            // if (userId == 0)
            //     MessageBox.Show("Vui lòng chọn nhân viên", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //else if (cmdParamId == 0)
            //     MessageBox.Show("Vui lòng chọn tham số hành động", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);


            if (userId != 0)
            {
                gridRegister.DataSource = null;
                var list = BLLRegisterUserCmd.Instance.Gets(userId,cmdId, cmdParamId);
                gridRegister.DataSource = list;
                txtStt.Value = (list.Count + 1);
            }
        }

        private void repbtn_deleteService_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLRegisterUserCmd.Instance.Delete(Id);
                GetGridView();
                frmMain.lib_RegisterUserCmds = BLLRegisterUserCmd.Instance.Gets();
            }
        }

        private void lkUser_EditValueChanged(object sender, EventArgs e)
        {
            var userObj = (ModelSelectItem)lkUser.GetSelectedDataRow();
            if (userObj == null)
                userId = 0;
            else
            {
                userId = userObj.Id;
                GetGridView();
            }
        }

        private void lkAction_EditValueChanged(object sender, EventArgs e)
        {
            GetActionParam();
        }

        private void GetActionParam()
        {
            var actionObj = (ModelSelectItem)lkAction.GetSelectedDataRow();
            if (actionObj != null)
            {
                lkActionParam.Properties.DataSource = null;
                lkActionParam.Properties.DataSource = BLLActionParameter.Instance.GetLookUp(actionObj.Id);
                lkActionParam.Properties.DisplayMember = "Name";
                lkActionParam.Properties.ValueMember = "Id";
                lkActionParam.Properties.PopulateColumns();
                lkActionParam.Properties.Columns[0].Visible = false;
                lkActionParam.Properties.Columns[3].Visible = false;
                lkActionParam.Properties.Columns[1].Caption = "Tham số hành động";
                lkActionParam.Properties.Columns[2].Caption = "Ghi chú";
                if (!string.IsNullOrEmpty(acParamName))
                {
                    lkActionParam.Text = acParamName;
                    acParamName = "";
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (userId == 0)
                    MessageBox.Show("Vui lòng chọn nhân viên.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (cmdParamId == 0)
                    MessageBox.Show("Vui lòng chọn tham số lệnh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (acParamId == 0)
                    MessageBox.Show("Vui lòng chọn tham số hành động.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (txtStt.Value == null)
                    MessageBox.Show("Vui lòng nhập số thứ tự thực hiện.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_UserCmdRegister() { Id = Id, CmdParamId = cmdParamId, ActionParamId = acParamId, Index = (int)txtStt.Value, Param = txtparam.Text, Note = "", UserId = userId };
                    int kq = 0;
                    if (obj.Id == 0)
                        kq = BLLRegisterUserCmd.Instance.Insert(obj);
                    else
                        kq = BLLRegisterUserCmd.Instance.Update(obj);
                    if (kq == 0)
                        MessageBox.Show("Tham số hành động này đã tồn tại. Vui lòng chọn tham số hành động khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        GetGridView();
                        btnCancel_Click(sender, e);
                        frmMain.lib_RegisterUserCmds = BLLRegisterUserCmd.Instance.Gets();
                    }
                }
            }
            catch (Exception)
            { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Id = 0;
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            lkUser.Enabled = true;
            lkCmd.Enabled = true;
            lkCmdParam.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnAdd.Enabled = true;
            btnCopy.Enabled = true;
            lkUser.Enabled = true;
            lkCmd.Enabled = true;
            lkCmdParam.Enabled = true;
            Id = 0;
        }

        private void lkcmdParam_EditValueChanged(object sender, EventArgs e)
        {
            var obj = (ModelSelectItem)lkCmdParam.GetSelectedDataRow();
            if (obj != null)
            {
                cmdParamId = obj.Id;
                GetGridView();
            }
            else
                cmdParamId = 0;
        }

        private void lkActionParam_EditValueChanged(object sender, EventArgs e)
        {
            var obj = (ModelSelectItem)lkActionParam.GetSelectedDataRow();
            if (obj != null)
                acParamId = obj.Id;
            else
                acParamId = 0;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var f = new frmCopyRegisterCmd();
            f.ShowDialog();
        }

        private void btnResetAcParam_Click(object sender, EventArgs e)
        {
            GetActionParam();
        }

        private void btnResetAction_Click(object sender, EventArgs e)
        {
            GetAction();
        }

        private void btnCmdParam_Click(object sender, EventArgs e)
        {
            GetCMDParam();
        }

        private void btnCmd_Click(object sender, EventArgs e)
        {
            GetCMD();
        }

        private void btnResetUser_Click(object sender, EventArgs e)
        {
            GetUser();
        }

        private void repbtnEdit_Click(object sender, EventArgs e)
        {
            int.TryParse(gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "Id").ToString(), out Id);
            acParamName = gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "ActionParamName").ToString();
            cmdParamName = gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "CMDParamName").ToString();
            lkCmd.Text = gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "CMDName").ToString();
            lkAction.Text = gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "ActionName").ToString();
            lkActionParam.Text = acParamName;
            txtparam.Text = (gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "Param") != null ? gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "Param").ToString() : "");
            txtNote.Text = (gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "Note") != null ? gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "Note").ToString() : "");
            txtStt.Value = int.Parse(gridViewRegister.GetRowCellValue(gridViewRegister.FocusedRowHandle, "Index").ToString());
            btnAdd.Enabled = false;
            btnCopy.Enabled = false;
            btnSave.Enabled = true;
            lkUser.Enabled = false;
            lkCmd.Enabled = false;
            lkCmdParam.Enabled = false;
        }

        private void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            GetGridView();
        }


    }
}
