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
    public partial class frmAlert : Form
    {
        public frmAlert()
        {
            InitializeComponent();
        }
        private void frmAlert_Load(object sender, EventArgs e)
        {
            GetGridAlert();
            GetGridSound();
        }
        #region Alert
        private void GetGridAlert()
        {
            var list = BLLAlert.Instance.Gets();
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            list.Add(new AlertModel() { Id = 0, SoundId = 0, Start = date, End = date, Note = "" });
            gridAlert.DataSource = list;
        }
        private void repbtn_deleteAlert_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLAlert.Instance.Delete(Id);
                GetGridAlert();
            }
        }
        private void gridViewAlert_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Id").ToString(), out Id);

                if (Id == 0 && (string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "SoundId").ToString()) || gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "SoundId").ToString() == "0"))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Start").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "End").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Note").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "SoundId").ToString()))
                    MessageBox.Show("Vui lòng chọn âm thanh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Start").ToString()))
                    MessageBox.Show("Vui lòng chọn thời gian bắt đầu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "End").ToString()))
                    MessageBox.Show("Vui lòng chọn thời gian kết thúc.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Note").ToString()))
                    MessageBox.Show("Vui lòng nhập diễn giải.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Alert();
                    obj.Id = Id;
                    obj.SoundId = int.Parse(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "SoundId").ToString());
                    obj.Start =  DateTime.Parse(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Start").ToString());
                    obj.End = DateTime.Parse(gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "End").ToString());
                    obj.Note = gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Note") != null ? gridViewAlert.GetRowCellValue(gridViewAlert.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLAlert.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Câu hướng dẫn này đã tồn tại. Xin kiểm tra lại.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLAlert.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Câu hướng dẫn này đã tồn tại. Xin kiểm tra lại.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridAlert();
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }
       
        private void GetGridSound()
        {
            lookUpSound.DataSource = null;
            lookUpSound.DataSource = BLLSound.Instance.GetLookUp();
            lookUpSound.DisplayMember = "Name";
            lookUpSound.ValueMember = "Id";
            lookUpSound.PopulateViewColumns();
            lookUpSound.View.Columns[0].Visible = false;
            lookUpSound.View.Columns[1].Caption = "File Âm thanh";
            lookUpSound.View.Columns[2].Visible = false;
            lookUpSound.View.Columns[3].Visible = false;
        }

        private void btnResetSound_Click(object sender, EventArgs e)
        {
            GetGridSound();
        }

        private void btnResetAlert_Click(object sender, EventArgs e)
        {
            GetGridAlert();
        }
        #endregion
    }
}
