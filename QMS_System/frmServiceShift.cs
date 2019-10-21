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
    public partial class frmServiceShift : Form
    {
        int serviceId = 0;
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmServiceShift()
        {
            InitializeComponent();
        }
        private void frmServiceShift_Load(object sender, EventArgs e)
        {
            GetService();
            GetShift();

        }
        private void lookUpService_EditValueChanged(object sender, EventArgs e)
        {
            var obj = (ModelSelectItem)lookUpService.GetSelectedDataRow();
            if (obj == null)
            {
                serviceId = 0;
                MessageBox.Show("Vui lòng chọn dịch vụ", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                serviceId = obj.Id;
                GetGrid();
            }
        }

        private void GetGrid()
        {
            if (serviceId == 0)
                MessageBox.Show("Vui lòng chọn dịch vụ", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                GetShift();
                var list = BLLServiceShift.Instance.Gets(connect, serviceId);
                list.Add(new ServiceShiftModel() { Id = 0, Index = (list.Count + 1), ShiftId = 0 });
                gridServiceShift.DataSource = list;
            }
        }

        private void GetService()
        {
            lookUpService.Properties.DataSource = null;
            lookUpService.Properties.DataSource = BLLService.Instance.GetLookUp(connect);
            lookUpService.Properties.DisplayMember = "Name";
            lookUpService.Properties.ValueMember = "Id";
            lookUpService.Properties.PopulateColumns();
            lookUpService.Properties.Columns[0].Visible = false;
            lookUpService.Properties.Columns[2].Visible = false;
            lookUpService.Properties.Columns[3].Visible = false;
            lookUpService.Properties.Columns[1].Caption = "Tên dịch vụ";
        }

        private void GetShift()
        {
            gridLookUpShift.DataSource = null;
            gridLookUpShift.DataSource = BLLShift.Instance.GetLookUp(connect);
            gridLookUpShift.DisplayMember = "Name";
            gridLookUpShift.ValueMember = "Id";
            gridLookUpShift.PopulateViewColumns();
            gridLookUpShift.View.Columns[0].Visible = false; 
            gridLookUpShift.View.Columns[3].Visible = false;
            gridLookUpShift.View.Columns[1].Caption = "Ca làm việc";
            gridLookUpShift.View.Columns[2].Caption = "Thời gian";
        }
        private void btnResetService_Click(object sender, EventArgs e)
        {
            GetService();
        }

        private void gridViewServiceShift_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "Index").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "ShiftId").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "ShiftId").ToString()))
                    MessageBox.Show("Vui lòng chọn thời gian cấp phiếu dịch vụ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_ServiceShift();
                    obj.Id = Id;
                    obj.ShiftId = int.Parse(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "ShiftId").ToString());
                    obj.ServiceId = serviceId;
                    obj.Index = int.Parse(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "Index").ToString());

                    if (obj.Id == 0)
                    {
                        int result = BLLServiceShift.Instance.Insert(connect,obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại thời gian cấp phiếu này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLServiceShift.Instance.Update(connect,obj);
                        if (result == false)
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại thời gian cấp phiếu này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int Id = int.Parse(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLServiceShift.Instance.Delete(connect,Id);
                GetGrid();
            }
        }

        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            GetGrid();
        }
    }
}
