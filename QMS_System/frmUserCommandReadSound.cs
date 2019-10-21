using GPRO.Core.Hai;
using QMS_System.Data;
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
    public partial class frmUserCommandReadSound : Form
    {
        int userId = 0, cmdId = 0;
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmUserCommandReadSound() { InitializeComponent(); }
        private void btnResetUser_Click(object sender, EventArgs e) { GetUser(); }
        private void btnResetCmd_Click(object sender, EventArgs e) { GetCMD(); }
        private void btnResetGrid_Click(object sender, EventArgs e) { GetReadTemplate(); GetGrid(); }
        private void lkUser_EditValueChanged(object sender, EventArgs e)
        {
            var user = (ModelSelectItem)lkUser.GetSelectedDataRow();
            if (user != null)
            {
                userId = user.Id;
                GetGrid();
            }
            else
                userId = 0;
        }
        private void lkCmd_EditValueChanged(object sender, EventArgs e)
        {
            var obj = (ModelSelectItem)lkCmd.GetSelectedDataRow();
            if (obj != null)
            {
                cmdId = obj.Id;
                GetGrid();
            }
            else
                cmdId = 0;
        }
        private void frmUserCommandReadSound_Load(object sender, EventArgs e) { GetUser(); GetCMD(); GetReadTemplate(); }
        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Index").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ReadTemplateId").ToString()))
                    goto End;
                else if (Id != 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự ưu tiên.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ReadTemplateId").ToString()))
                    MessageBox.Show("Vui lòng chọn mẫu đọc âm thanh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_UserCommandReadSound();
                    obj.Id = Id;
                    obj.UserId = userId;
                    obj.CommandId = cmdId;
                    obj.Index = (int)gridView.GetRowCellValue(gridView.FocusedRowHandle, "Index");
                    obj.ReadTemplateId = (int)gridView.GetRowCellValue(gridView.FocusedRowHandle, "ReadTemplateId");
                    obj.Note = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Note") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "Note").ToString() : "";

                    int kq = 0;
                    if (obj.Id == 0)
                        kq = BLLUserCmdReadSound.Instance.Insert(connect,obj);
                    else
                        kq = BLLUserCmdReadSound.Instance.Update(connect,obj);

                    if (kq == 0)
                    {
                        MessageBox.Show("Tên chính sách này đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto End;
                    }
                    else
                    {
                        GetGrid();
                       frmMain.lib_UserCMDReadSound = BLLUserCmdReadSound.Instance.Gets(connect);
                    }
                }
            }
            catch (Exception ex)
            { }
        End: { }
        }
        private void repbtn_delete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLUserCmdReadSound.Instance.Delete(connect,Id);
                GetGrid();
                frmMain.lib_UserCMDReadSound = BLLUserCmdReadSound.Instance.Gets(connect);

            }
        }
        private void GetUser()
        {
            lkUser.Properties.DataSource = null;
            lkUser.Properties.DataSource = BLLUser.Instance.GetLookUp(connect);
            lkUser.Properties.DisplayMember = "Name";
            lkUser.Properties.ValueMember = "Id";
            lkUser.Properties.PopulateColumns();
            lkUser.Properties.Columns[0].Visible = false;
            lkUser.Properties.Columns[2].Visible = false;
            lkUser.Properties.Columns[3].Visible = false;
            lkUser.Properties.Columns[1].Caption = "Tên nhân viên";
        }
        private void GetCMD()
        {
            lkCmd.Properties.DataSource = null;
            lkCmd.Properties.DataSource = BLLCommand.Instance.GetLookUp(connect);
            lkCmd.Properties.DisplayMember = "Name";
            lkCmd.Properties.ValueMember = "Id";
            lkCmd.Properties.PopulateColumns();
            lkCmd.Properties.Columns[0].Visible = false;
            lkCmd.Properties.Columns[3].Visible = false;
            lkCmd.Properties.Columns[1].Caption = "Mã lệnh";
            lkCmd.Properties.Columns[2].Caption = "Ghi chú";

        }
        private void GetReadTemplate()
        {
            gridLookUpRead.DataSource = null;
            gridLookUpRead.DataSource = BLLReadTemplate.Instance.GetLookUp(connect);
            gridLookUpRead.DisplayMember = "Name";
            gridLookUpRead.ValueMember = "Id";
            gridLookUpRead.PopulateViewColumns();
            gridLookUpRead.View.Columns[0].Visible = false;
            gridLookUpRead.View.Columns[3].Visible = false;
            gridLookUpRead.View.Columns[1].Caption = "Tên mẫu";
            gridLookUpRead.View.Columns[2].Visible = false;
        }
        private void GetGrid()
        {
            if (userId != 0 && cmdId != 0)
            {
                var list = BLLUserCmdReadSound.Instance.Gets(connect, userId, cmdId);
                list.Add(new UserCmdReadSoundModel() { Id = 0, UserId = userId, CommandId = cmdId, Index = (list.Count + 1) });
                gridcontrol.DataSource = list;
            }
        }
    }
}
