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
    public partial class frmShift : Form
    {
        public frmShift()
        {
            InitializeComponent();
        }
        private void frmShift_Load(object sender, EventArgs e)
        {
            GetGridShift();
        }
        #region Shift
        private void GetGridShift()
        {
            var list = BLLShift.Instance.Gets();
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            list.Add(new ShiftModel() { Id = 0, Name = "", Start = date, End = date, Note = "" });
            gridShift.DataSource = list;
        }
        
        private void gridViewShift_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Id").ToString(), out Id);

                if (Id == 0 && string.IsNullOrEmpty(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Name").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Start").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "End").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên ca làm việc.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Start").ToString()))
                    MessageBox.Show("Vui lòng chọn thời gian bắt đầu ca làm việc.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "End").ToString()))
                    MessageBox.Show("Vui lòng chọn thời gian kết thúc ca làm việc.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                else
                {
                    var obj = new Q_Shift();
                    obj.Id = Id;
                    obj.Name = gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Name").ToString();
                    obj.Start = DateTime.Parse(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Start").ToString());
                    obj.End = DateTime.Parse(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "End").ToString());
                    obj.Note = gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Note") != null ? gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLShift.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên ca làm việc này đã tồn tại. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLShift.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Tên ca làm việc này đã tồn tại. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridShift();
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }
        private void repbtn_deleteShift_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewShift.GetRowCellValue(gridViewShift.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLShift.Instance.Delete(Id);
                GetGridShift();
            }
        }
        private void btnResetShift_Click(object sender, EventArgs e)
        {
            GetGridShift();
        }
       
        #endregion
    }
}
