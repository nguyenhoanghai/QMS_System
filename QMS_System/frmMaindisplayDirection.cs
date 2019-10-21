using DevExpress.XtraEditors;
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
    public partial class frmMaindisplayDirection : Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        int counterId = 0;
        public frmMaindisplayDirection()
        {
            InitializeComponent();
        }
        private void frmMaindisplayDirection_Load(object sender, EventArgs e)
        {
            GetCounter();
            GetEquipment();

        }
        private void lookUpCounter_EditValueChanged(object sender, EventArgs e)
        {
            var obj = (ModelSelectItem)lookUpCounter.GetSelectedDataRow();
            if (obj == null)
            {
                counterId = 0;
                MessageBox.Show("Vui lòng chọn quầy", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                counterId = obj.Id;
                GetGrid();
            }
        }

        private void GetGrid()
        {
            if (counterId == 0)
                MessageBox.Show("Vui lòng chọn quầy", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                GetEquipment();
                var list = BLLMaindisplayDirection.Instance.Gets(connect, counterId);
                list.Add(new MaindisplayDirectionModel() { Id = 0, Index = (list.Count + 1), EquipmentId = 0, Direction = false });
                gridMaindisplayDirection.DataSource = list;
                

            }
        }

        private void GetCounter()
        {
            lookUpCounter.Properties.DataSource = null;
            lookUpCounter.Properties.DataSource = BLLCounter.Instance.GetLookUp(connect);
            lookUpCounter.Properties.DisplayMember = "Name";
            lookUpCounter.Properties.ValueMember = "Id";
            lookUpCounter.Properties.PopulateColumns();
            lookUpCounter.Properties.Columns[0].Visible = false;
            lookUpCounter.Properties.Columns[2].Visible = false;
            lookUpCounter.Properties.Columns[3].Visible = false;
            lookUpCounter.Properties.Columns[1].Caption = "Tên Quầy";
        }

        private void GetEquipment()
        {
            gridLookUpEquipment.DataSource = null;
            gridLookUpEquipment.DataSource = BLLEquipment.Instance.GetMaindisplay(connect);
            gridLookUpEquipment.DisplayMember = "Name";
            gridLookUpEquipment.ValueMember = "Id";
            gridLookUpEquipment.View.PopulateColumns();
            if (gridLookUpEquipment.View.Columns.Count > 0)
            {
                gridLookUpEquipment.View.Columns[0].Visible = false;
                gridLookUpEquipment.View.Columns[2].Visible = false;
                gridLookUpEquipment.View.Columns[3].Visible = false;
                gridLookUpEquipment.View.Columns[1].Caption = "Thiết bị";
            }
        }
         private void gridViewMaindisplayDirection_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "EquipmentId").ToString()=="0" || int.Parse(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "EquipmentId").ToString()) == 0)
                    goto End;
                //else if (Id == 0 && string.IsNullOrEmpty(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "Index").ToString()) || int.Parse(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "Index").ToString()) == 0)
                //    goto End;

                if (Id != 0 && (gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "EquipmentId").ToString()=="0") || int.Parse(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "EquipmentId").ToString()) == 0)
                    MessageBox.Show("Vui lòng chọn thiết bị", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else if (Id != 0 && string.IsNullOrEmpty(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "ShiftId").ToString()))
                //    MessageBox.Show("Vui lòng chọn thời gian cấp phiếu dịch vụ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_MaindisplayDirection();
                    obj.Id = Id;
                    obj.EquipmentId = int.Parse(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "EquipmentId").ToString());
                    obj.CounterId = counterId;
                    obj.Direction = (bool)gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "Direction");
                    obj.Note = gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "Note") != null ? gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "Note").ToString() : null;
                    if (obj.Id == 0)
                    {
                        int result = BLLMaindisplayDirection.Instance.Insert(connect,obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Đã tồn tại Quầy có hướng đi Maindisplay này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLMaindisplayDirection.Instance.Update(connect,obj);
                        if (result == false)
                        {
                            MessageBox.Show("Đã tồn tại Quầy có hướng đi Maindisplay này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGrid();
                }
            }
            catch (Exception ex) { }
        End: { }
        }

        private void repbtnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewMaindisplayDirection.GetRowCellValue(gridViewMaindisplayDirection.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLMaindisplayDirection.Instance.Delete(connect,Id);
                GetGrid();
            }
        }

        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            GetGrid();
        }

        private void btnResetCounter_Click(object sender, EventArgs e)
        {
            GetCounter();
        }

        //private void gridLookUpEquipment_QueryPopUp(object sender, CancelEventArgs e)
        //{
        //    var lookUp = (GridLookUpEdit)sender;
        //    if(lookUp.Properties.View.Columns.Count >0)
        //    {
        //        lookUp.Properties.View.PopulateColumns();
        //        lookUp.Properties.View.Columns[0].Visible = false;
        //        lookUp.Properties.View.Columns[2].Visible = false;
        //        lookUp.Properties.View.Columns[3].Visible = false;
        //        lookUp.Properties.View.Columns[1].Caption = "Thiết bị";
        //    }
        //}
    }
}
