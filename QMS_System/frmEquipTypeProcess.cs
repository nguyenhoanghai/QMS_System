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
    public partial class frmEquipTypeProcess : Form
    {
        public frmEquipTypeProcess()
        {
            InitializeComponent();
        }

        private void frmEquipTypeProcess_Load(object sender, EventArgs e)
        {
            GetGridEquipTypeProcess();
            GetEquipType();
            GetProcess();
        }
        #region EquipTypeProcess
        private void GetGridEquipTypeProcess()
        {
            var list = BLLEquipTypeProcess.Instance.Gets();
            list.Add(new EquipTypeProcessModel() { Id = 0, EquipTypeId = 0, ProcessId = 0, Step = 0, Priority = 0, Count = 0 });
            gridEquipTypeProcess.DataSource = list;
        }
        private void repbtn_delete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLEquipTypeProcess.Instance.Delete(Id);
                GetGridEquipTypeProcess();
            }
        }
        private void gridViewEquipTypeProcess_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && (string.IsNullOrEmpty(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "EquipTypeId").ToString()) || int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "EquipTypeId").ToString()) == 0))
                    goto End;
                if (Id == 0 && (string.IsNullOrEmpty(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "ProcessId").ToString()) || int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "ProcessId").ToString()) == 0))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Step").ToString()) || int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Step").ToString()) <= 0))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Priority").ToString()) || int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Priority").ToString()) <= 0))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Count").ToString()) || int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Count").ToString()) <= 0))
                    goto End;

                if (Id != 0 && (string.IsNullOrEmpty(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "EquipTypeId").ToString()) || int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "EquipTypeId").ToString()) == 0))
                    MessageBox.Show("Vui lòng chọn loại thiết bị.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && (string.IsNullOrEmpty(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "ProcessId").ToString()) || int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "ProcessId").ToString()) == 0))
                    MessageBox.Show("Vui lòng chọn tiến trình.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Step").ToString()) <= 0)
                    MessageBox.Show("Giá trị của Bước không hợp hệ. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Priority").ToString()) <= 0)
                    MessageBox.Show("Giá trị của Ưu tiên không hợp hệ. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Count").ToString()) <= 0)
                    MessageBox.Show("Giá trị của Đếm không hợp hệ. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_EquipTypeProcess();
                    obj.Id = Id;
                    obj.EquipTypeId = int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "EquipTypeId").ToString());
                    obj.ProcessId = int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "ProcessId").ToString());
                    obj.Step = int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Step").ToString());
                    obj.Priority = int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Priority").ToString());
                    obj.Count = int.Parse(gridViewEquipTypeProcess.GetRowCellValue(gridViewEquipTypeProcess.FocusedRowHandle, "Count").ToString());

                    if (obj.Id == 0)
                        BLLEquipTypeProcess.Instance.Insert(obj);
                    else
                        BLLEquipTypeProcess.Instance.Update(obj);
                    GetGridEquipTypeProcess();
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }

        private void GetEquipType()
        {
            //Load SelectBox Loại thiết bị
            lookUpEquipType.DataSource = null;
            lookUpEquipType.DataSource = BLLEquipType.Instance.GetLookUp();
            lookUpEquipType.DisplayMember = "Name";
            lookUpEquipType.ValueMember = "Id";
            lookUpEquipType.PopulateViewColumns(); // Thao tác với tất cả column của datasource được bind vào control
            lookUpEquipType.View.Columns[0].Visible = false;
            lookUpEquipType.View.Columns[1].Caption = "Loại thiết bị";
        }

        private void GetProcess()
        {
            //Load SelectBox Tiến trình
            lookUpProcess.DataSource = null;
            lookUpProcess.DataSource = BLLProcess.Instance.GetLookUp();
            lookUpProcess.DisplayMember = "Name";
            lookUpProcess.ValueMember = "Id";
            lookUpProcess.PopulateViewColumns(); // Thao tác với tất cả column của datasource được bind vào control
            lookUpProcess.View.Columns[0].Visible = false;
            lookUpProcess.View.Columns[1].Caption = "Tiến trình";
        }
       
        private void btnResetProcess_Click(object sender, EventArgs e)
        {
            GetProcess();
        }

        private void btnResetEquipType_Click(object sender, EventArgs e)
        {
            GetEquipType();
        }

        private void btnResetEquipTypeProcess_Click(object sender, EventArgs e)
        {
            GetGridEquipTypeProcess();
        }

        #endregion
    }
}
