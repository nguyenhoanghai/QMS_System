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
    public partial class frmProcess : Form
    {
        public frmProcess()
        {
            InitializeComponent();
        }

        private void frmProcess_Load(object sender, EventArgs e)
        {
            GetGridProcess();
        }

        #region Process
        private void GetGridProcess()
        {
            var list = BLLProcess.Instance.Gets();
            list.Add(new ProcessModel() { Id = 0, Name = "", Index = BLLProcess.Instance.GetLastIndex() +1 , Note = "" });
            gridProcess.DataSource = list;
        }
        private void repbtn_deleteProcess_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLProcess.Instance.Delete(Id);
                GetGridProcess();
            }
        }
        private void gridViewProcess_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Name").ToString()))
                    goto End;
                if (Id != 0 && string.IsNullOrEmpty(gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên tiến trình.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Process();
                    obj.Id = Id;
                    obj.Name = gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Name").ToString();
                    obj.Index = int.Parse(gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Index").ToString());
                    obj.Note = gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Note") != null ? gridViewProcess.GetRowCellValue(gridViewProcess.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLProcess.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên tiến trình đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }  
                    else
                    {
                        bool result = BLLProcess.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Tên tiến trình đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    } 
                    GetGridProcess();
                }
            }
            catch (Exception ex)
            {
            }
            End:
            {

            }
        }
        #endregion
    }
}
