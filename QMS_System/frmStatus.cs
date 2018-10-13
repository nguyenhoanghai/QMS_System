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
    public partial class frmStatus : Form
    {
        int typeId = 0;
        public frmStatus()
        {
            InitializeComponent();
        }

        private void frmStatus_Load(object sender, EventArgs e)
        {
            GetGridType();
            GetGridStatus();
        }
        #region Status
        private void GetGridStatus()
        {
            var list = BLLStatus.Instance.Gets(typeId);
            list.Add(new StatusModel() { Id = 0, Code = "", StatusTypeId = 0 });
            gridStatus.DataSource = list;
        }
        private void repbtnDeleteStatus_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //int Id = int.Parse(gridViewStatus.GetRowCellValue(gridViewStatus.FocusedRowHandle, "Id").ToString());
            //if (Id != 0)
            //{
            //    BLLStatus.Instance.Delete(Id);
            //    GetGridStatus();
            //}
        }
        private void gridViewStatus_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewStatus.GetRowCellValue(gridViewStatus.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewStatus.GetRowCellValue(gridViewStatus.FocusedRowHandle, "Code").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewStatus.GetRowCellValue(gridViewStatus.FocusedRowHandle, "Code").ToString()))
                    MessageBox.Show("Vui lòng nhập mã trạng thái.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Status();
                    obj.Id = Id;
                    obj.Code = gridViewStatus.GetRowCellValue(gridViewStatus.FocusedRowHandle, "Code").ToString();
                    obj.StatusTypeId = typeId;
                    obj.Note = gridViewStatus.GetRowCellValue(gridViewStatus.FocusedRowHandle, "Note") != null ? gridViewStatus.GetRowCellValue(gridViewStatus.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                        BLLStatus.Instance.Insert(obj);
                    else
                        BLLStatus.Instance.Update(obj);
                    GetGridStatus();
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

        #region StatusType
        private void GetGridType()
        {
            lookUpType.DataSource = null;
            lookUpType.DataSource = BLLStatusType.Instance.GetLookUp();
            lookUpType.DisplayMember = "Name";
            lookUpType.ValueMember = "Id";
            lookUpType.PopulateViewColumns();
            lookUpType.View.Columns[0].Caption = "Id";
            lookUpType.View.Columns[0].Visible = false;
            lookUpType.View.Columns[1].Caption = "Loại trạng thái";
            lookUpType.View.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            var list = BLLStatusType.Instance.Gets();
            list.Add(new StatusTypeModel() { Id = 0, Name = "", Note = "" });
            gridStatusType.DataSource = list;
        }
        private void repbtn_deleteType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //int Id = int.Parse(gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Id").ToString());
            //if (Id != 0)
            //{
            //    BLLStatusType.Instance.Delete(Id);
            //    GetGridType();
            //}

            int.TryParse(gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Id").ToString(), out typeId);
            groupControl2.Text = "Danh mục trạng thái của loại : " + gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Name").ToString();
            GetGridStatus();
        }
        private void gridViewType_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Name").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên loại trạng thái.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_StatusType();
                    obj.Id = Id;
                    obj.Name = gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Name").ToString();
                    obj.Note = gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Note") != null ? gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLStatusType.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên loại thiết bị này đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLStatusType.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Tên loại thiết bị này đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridType();
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

        private void gridViewType_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int.TryParse(gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Id").ToString(), out typeId);
            groupControl2.Text ="Danh mục trạng thái của loại : "+ gridViewType.GetRowCellValue(gridViewType.FocusedRowHandle, "Name").ToString();
            GetGridStatus();
        }
    }
}
