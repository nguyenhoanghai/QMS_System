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
    public partial class frmPolicy : Form
    {
        public frmPolicy()
        {
            InitializeComponent();
        }

        private void frmPolicy_Load(object sender, EventArgs e)
        {
            GetGridPolicy();
        }
        private void GetGridPolicy()
        {
            var list = BLLPolicy.Instance.Gets();
            list.Add(new PolicyModel() { Id = 0, Name = "", IsActived = false, Note = "" });
            gridPolicy.DataSource = list;
        }
        private void repbtn_deletePolicy_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLPolicy.Instance.Delete(Id);
                GetGridPolicy();
            }
        }
        private void gridViewPolicy_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "Name").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên chính sách.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Policy();
                    obj.Id = Id;
                    obj.Name = gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "Name").ToString();
                    obj.IsActived = (bool)gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "IsActived");
                    obj.Note = gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "Note") != null ? gridViewPolicy.GetRowCellValue(gridViewPolicy.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLPolicy.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên chính sách này đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLPolicy.Instance.Update(obj);
                        if(result==false)
                        {
                            MessageBox.Show("Tên chính sách này đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridPolicy();
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }

        private void btnResetMajor_Click(object sender, EventArgs e)
        {
            GetGridPolicy();
        }
    }
}
