using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmService : Form
    {
        public frmService()
        {
            InitializeComponent();
        }
        int serId = 0;
        private void frmService_Load(object sender, EventArgs e)
        {
            GetGridService();
            GetShift(); 
            GetMajor(); 
        }         

        #region Service
        private void repbtn_deleteService_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLService.Instance.Delete(Id);
                GetGridService();
            }
        }
        private void gridViewService_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int start = 0;
                int end = 0;
                bool isNumber1 = int.TryParse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "StartNumber").ToString(), out start);
                bool isNumber2 = int.TryParse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "EndNumber").ToString(), out end);

                int.TryParse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Name").ToString()))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "StartNumber").ToString()) || isNumber1 == false || start <= 0))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "EndNumber").ToString()) || isNumber2 == false || end <= 0 || end < start))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "TimeProcess").ToString()))
                    goto End;
                 

                if (Id != 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên dịch vụ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "StartNumber").ToString()))
                    MessageBox.Show("Vui lòng nhập số phiếu bắt đầu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && isNumber1 == false)
                    MessageBox.Show("Số phiếu bắt đầu phải là chữ số. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && start <= 0)
                    MessageBox.Show("Số phiếu bắt đầu có giá trị không hợp lệ. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "EndNumber").ToString()))
                    MessageBox.Show("Vui lòng nhập số phiếu kết thúc.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && isNumber2 == false)
                    MessageBox.Show("Số phiếu kết thúc phải là chữ số. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && (end <= 0 || end < start))
                    MessageBox.Show("Số phiếu kết thúc có giá trị không hợp lệ. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "TimeProcess").ToString()))
                    MessageBox.Show("Vui lòng nhập thời gian xử lý.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 else
                {
                    var obj = new Q_Service();
                    obj.Id = Id;
                    obj.Name = gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Name").ToString();
                    obj.StartNumber = int.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "StartNumber").ToString());
                    obj.EndNumber = int.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "EndNumber").ToString());
                    obj.TimeProcess = DateTime.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "TimeProcess").ToString());
                    obj.IsActived = bool.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "IsActived").ToString());
                    obj.Note = gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Note") != null ? gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLService.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên dịch vụ đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLService.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Tên dịch vụ đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridService();
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }
        private void GetGridService()
        {
            var list = BLLService.Instance.Gets();
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            list.Add(new ServiceModel() { Id = 0, Name = "", StartNumber = 0, EndNumber = 0, TimeProcess = date });
            gridService.DataSource = list;
        }
        private void btnReGridService_Click(object sender, EventArgs e)
        {
            GetGridService();
        }
        #endregion

        #region Service Step
        private void repbtnDetail_Click(object sender, EventArgs e)
        {
            int.TryParse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Id").ToString(), out serId);
            GetGridServiceStep();
            groupControl2.Text = "Danh sách bước xử lý của Dịch vụ : " + gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Name").ToString();

        }

        private void GetGridServiceStep()
        {      
            var list = BLLServiceStep.Instance.Gets(serId);
            list.Add(new ServiceStepModel() { Id = 0, ServiceId = 0, MajorId = 0, Index = 1 });
            gridStep.DataSource = list;
        }

        private void GetMajor()
        {
            gridLookUpMajor.DataSource = null;
            gridLookUpMajor.DataSource = BLLMajor.Instance.GetLookUp();
            gridLookUpMajor.DisplayMember = "Name";
            gridLookUpMajor.ValueMember = "Id";
            gridLookUpMajor.PopulateViewColumns();
            gridLookUpMajor.View.Columns[0].Caption = "Id";
            gridLookUpMajor.View.Columns[0].Visible = false;
            gridLookUpMajor.View.Columns[1].Caption = "Nghiệp vụ";
            gridLookUpMajor.View.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
        }
       
        private void gridViewStep_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "Index").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "MajorId").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự ưu tiên.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "MajorId").ToString()))
                    MessageBox.Show("Vui lòng chọn nghiệp vụ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_ServiceStep();
                    obj.Id = Id;
                    obj.MajorId = int.Parse(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "MajorId").ToString());
                    obj.ServiceId = serId;
                    obj.Index = int.Parse(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "Index").ToString());

                    if (obj.Id == 0)
                    {
                        int result = BLLServiceStep.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại nghiệp vụ này. Xin chọn lại nghiệp vụ khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLServiceStep.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại nghiệp vụ này. Xin chọn lại nghiệp vụ khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridServiceStep();
                }
            }
            catch (Exception ex) { }
        End: { }
        }
          
        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewStep.GetRowCellValue(gridViewStep.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLServiceStep.Instance.Delete(Id);
                GetGridServiceStep();
            }
        }
        private void btnReMajor_Click(object sender, EventArgs e)
        {
            GetMajor();
        }
        #endregion

        #region Shift
         private void repbtnTime_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int.TryParse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Id").ToString(), out serId);
            GetGridServiceShift();
        }

        private void GetGridServiceShift()
        {
            var list = BLLServiceShift.Instance.Gets(serId);
            list.Add(new ServiceShiftModel() { Id = 0, Index = (list.Count + 1), ShiftId = 0 });
            gridServiceShift.DataSource = list;
        }

        private void GetShift()
        {
            gridLookUpShift.DataSource = null;
            gridLookUpShift.DataSource = BLLShift.Instance.GetLookUp();
            gridLookUpShift.DisplayMember = "Name";
            gridLookUpShift.ValueMember = "Id";
            gridLookUpShift.PopulateViewColumns();
            gridLookUpShift.View.Columns[0].Visible = false;
            gridLookUpShift.View.Columns[3].Visible = false;
            gridLookUpShift.View.Columns[1].Caption = "Ca làm việc";
            gridLookUpShift.View.Columns[2].Caption = "Thời gian";
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
                    obj.ServiceId = serId;
                    obj.Index = int.Parse(gridViewServiceShift.GetRowCellValue(gridViewServiceShift.FocusedRowHandle, "Index").ToString());

                    if (obj.Id == 0)
                    {
                        int result = BLLServiceShift.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại thời gian cấp phiếu này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLServiceShift.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại thời gian cấp phiếu này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridServiceShift();
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
                BLLServiceShift.Instance.Delete(Id);
                GetGridServiceShift();
            }
        }
                 
        private void btnReShift_Click(object sender, EventArgs e)
        {
            GetShift();
        }
        #endregion
        
    }
}
