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
    public partial class frmBusiness : Form
    {
        public frmBusiness()
        {
            InitializeComponent();
        }
        int businessTypeId = 0;

        private void frmBusiness_Load(object sender, EventArgs e)
        {
            GetGridBusinessType();
            GetGridBusiness();
        }

        #region Business
        private void GetGridBusiness()
        {
            var list = BLLBusiness.Instance.Gets(businessTypeId);
            list.Add(new BusinessModel() { Id = 0, Name = "", BusinessTypeId = 0, Address = "", TotalTicket = 0, Note = "" });
            gridBusiness.DataSource = list;
        }

        private void gridViewBusiness_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int n = 0;
                int.TryParse(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Id").ToString(), out Id);
                bool isNumber = int.TryParse(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "TotalTicket").ToString(), out n);
                if (Id == 0 && string.IsNullOrEmpty(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Name").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "TotalTicket").ToString()))
                    goto End;
                else if (Id == 0 && isNumber == false)
                    goto End;
                else if (Id == 0 && n <= 0)
                    goto End;

                if (string.IsNullOrEmpty(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên doanh nghiệp.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (string.IsNullOrEmpty(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "TotalTicket").ToString()))
                    MessageBox.Show("Vui lòng nhập tổng vé.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (isNumber == false)
                    MessageBox.Show("Tổng vé phải là dạng chữ số. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (n <= 0)
                    MessageBox.Show("Tổng vé có giá trị không hợp lệ. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Business();
                    obj.Id = Id;
                    obj.Name = gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Name").ToString();
                    obj.BusinessTypeId = businessTypeId;
                    obj.Address = gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Address") != null ? gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Address").ToString() : "";
                    obj.TotalTicket = int.Parse(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "TotalTicket").ToString());
                    obj.Note = gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Note") != null ? gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLBusiness.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên doanh nghiệp đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLBusiness.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Tên doanh nghiệp đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridBusiness();
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }
        private void repbtn_deleteBusiness_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewBusiness.GetRowCellValue(gridViewBusiness.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLBusiness.Instance.Delete(Id);
                GetGridBusiness();
            }
        }
        #endregion

        #region BusinessType
        private void GetGridBusinessType()
        {
            lookUpBusinessType.DataSource = null;
            lookUpBusinessType.DataSource = BLLBusinessType.Instance.GetLookUp();
            lookUpBusinessType.DisplayMember = "Name";
            lookUpBusinessType.ValueMember = "Id";
            lookUpBusinessType.PopulateViewColumns();
            lookUpBusinessType.View.Columns[0].Visible = false;
            lookUpBusinessType.View.Columns[1].Caption = "Loại doanh nghiệp";

            var list = BLLBusinessType.Instance.Gets();
            list.Add(new BusinessTypeModel() { Id = 0, Name = "", Note = "" });
            gridBusinessType.DataSource = list;
        }
        private void gridViewBusinessType_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Name").ToString()))
                    goto End;
                if (string.IsNullOrEmpty(gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên loại doanh nghiệp.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_BusinessType();
                    obj.Id = Id;
                    obj.Name = gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Name").ToString();
                    obj.Note = gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Note") != null ? gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLBusinessType.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên loại doanh nghiệp đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLBusinessType.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Tên loại doanh nghiệp đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridBusinessType();
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }
        private void repbtn_deleteBusinessType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLBusinessType.Instance.Delete(Id);
                GetGridBusinessType();
            }
        }
        #endregion

        private void btnResetBusinessType_Click(object sender, EventArgs e)
        {
            GetGridBusinessType();
        }

        private void btnResetBusiness_Click(object sender, EventArgs e)
        {
            GetGridBusiness();
        }

        private void repbtnDetail_Click(object sender, EventArgs e)
        {
            int.TryParse(gridViewBusinessType.GetRowCellValue(gridViewBusinessType.FocusedRowHandle, "Id").ToString(), out businessTypeId);
            GetGridBusiness();
        }
    }
}
