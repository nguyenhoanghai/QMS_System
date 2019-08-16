using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using System;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmServiceLimit : Form
    {
        int userId = 0;
        public frmServiceLimit()
        {
            InitializeComponent();
        }
        private void frmServiceLimit_Load(object sender, EventArgs e)
        {
            LoadUser();
            loadService();
            LoadGrid();
        }
         
        private void repbtn_deleteService_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLServiceLimit.Instance.Delete(Id);
                LoadGrid();
            }
        }

        private void LoadGrid()
        {
            if (userId != 0)
            {
                gridService.DataSource = null;
                var list = BLLServiceLimit.Instance.Gets(userId);
                list.Add(new ServiceLimitModel() { Id = 0, ServiceId = 0, Quantity = 0, UserId = userId });
                gridService.DataSource = list;
            }
        }

        private void gridViewService_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Quantity").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "ServiceId").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "ServiceId").ToString()))
                    MessageBox.Show("Vui lòng chọn dịch vụ", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Quantity").ToString()))
                    MessageBox.Show("Vui lòng nhập số phiếu giới hạn cho phép.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_ServiceLimit();
                    obj.Id = Id;
                    obj.ServiceId = int.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "ServiceId").ToString());
                    obj.UserId = userId;
                    obj.Quantity = int.Parse(gridViewService.GetRowCellValue(gridViewService.FocusedRowHandle, "Quantity").ToString());

                    if (obj.Id == 0)
                    {
                        bool result = BLLServiceLimit.Instance.InsertOrUpdate(obj);
                        if (!result  )
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại thời gian cấp phiếu này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLServiceLimit.Instance.InsertOrUpdate(obj);
                        if (!result )
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại thời gian cấp phiếu này. Xin chọn lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    LoadGrid();
                }
            }
            catch (Exception ex) { }
            End: { }
        }

        #region user
        private void btnReUser_Click(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void LoadUser()
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

        private void lkUser_EditValueChanged(object sender, EventArgs e)
        {
            var userObj = (ModelSelectItem)lkUser.GetSelectedDataRow();
            if (userObj == null)
                userId = 0;
            else
            {
                userId = userObj.Id;
                LoadGrid();
            }
        }
        #endregion

        #region service
        private void btnReService_Click(object sender, EventArgs e)
        {
            loadService();
        }

        private void loadService()
        {
            repLKService.DataSource = null;
            repLKService.DataSource = BLLService.Instance.GetLookUp();
            repLKService.DisplayMember = "Name";
            repLKService.ValueMember = "Id";
            repLKService.PopulateViewColumns();
            repLKService.View.Columns[0].Visible = false;
            repLKService.View.Columns[3].Visible = false;
            repLKService.View.Columns[1].Caption = "Dịch vụ";
            repLKService.View.Columns[2].Visible = false;
        }
        #endregion

    }
}
