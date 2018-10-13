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
    public partial class frmUserMajor : Form
    {
        int userId = 0;
        public frmUserMajor()
        {
            InitializeComponent();
        }

        private void lkUser_EditValueChanged(object sender, EventArgs e)
        {
            var obj = (ModelSelectItem)lkUser.GetSelectedDataRow();
            if (obj == null)
            {
                userId = 0;
                MessageBox.Show("Vui lòng chọn nhân viên", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                userId = obj.Id;
                GetGrid();
            }
        }

        private void GetGrid()
        {
            if (userId == 0)
                MessageBox.Show("Vui lòng chọn nhân viên", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                gridLookUpMajor.DataSource = null;
                gridLookUpMajor.DataSource = BLLMajor.Instance.GetLookUp();
                gridLookUpMajor.DisplayMember = "Name";
                gridLookUpMajor.ValueMember = "Id";
                gridLookUpMajor.PopulateViewColumns();
                  gridLookUpMajor.View.Columns[0].Visible = false;
                    gridLookUpMajor.View.Columns[2].Visible = false;
                    gridLookUpMajor.View.Columns[3].Visible = false;
                    gridLookUpMajor.View.Columns[1].Caption = "Nghiệp vụ";
                 var list = BLLUserMajor.Instance.Gets(userId);
                list.Add(new UserMajorModel() { Id = 0, Index = (list.Count + 1), MajorId = 0 });
                gridUserMajor.DataSource = list;

            }
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

        private void frmUserMajor_Load(object sender, EventArgs e)
        {
            GetUser();

            gridLookUpMajor.DataSource = null;
            gridLookUpMajor.DataSource = BLLMajor.Instance.GetLookUp();
            gridLookUpMajor.DisplayMember = "Name";
            gridLookUpMajor.ValueMember = "Id";
            gridLookUpMajor.PopulateViewColumns();
            gridLookUpMajor.View.Columns[0].Visible = false;
             gridLookUpMajor.View.Columns[2].Visible = false;
           gridLookUpMajor.View.Columns[3].Visible = false;
            gridLookUpMajor.View.Columns[1].Caption = "Nghiệp vụ";
        }

        private void btnResetUser_Click(object sender, EventArgs e)
        {
            GetUser();
        }

        private void gridViewUserMajor_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "Index").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "MajorId").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự ưu tiên.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "MajorId").ToString()))
                    MessageBox.Show("Vui lòng chọn nghiệp vụ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_UserMajor();
                    obj.Id = Id;
                    obj.MajorId = int.Parse(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "MajorId").ToString());
                    obj.UserId = userId;
                    obj.Index = int.Parse(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "Index").ToString());

                    if (obj.Id == 0)
                    {
                        int result = BLLUserMajor.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Nhân viên đã tồn tại nghiệp vụ này. Xin chọn lại nghiệp vụ khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLUserMajor.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Nhân viên đã tồn tại nghiệp vụ này. Xin chọn lại nghiệp vụ khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGrid();
                   frmMain.lib_UserMajors = BLLUserMajor.Instance.Gets();
                }
            }
            catch (Exception ex) { }
        End: { }
        }

        private void repbtnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewUserMajor.GetRowCellValue(gridViewUserMajor.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLUserMajor.Instance.Delete(Id);
                GetGrid();
                frmMain.lib_UserMajors = BLLUserMajor.Instance.Gets();

            }
        }

        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            GetGrid();
        }

    }
}
