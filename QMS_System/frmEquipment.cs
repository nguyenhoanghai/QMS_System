using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmEquipment : Form
    {
        public frmEquipment()
        {
            InitializeComponent();
        }
        int equipTypeId = 0;

        private void frmEquipment_Load(object sender, EventArgs e)
        {
            GetGridEquipType();
            GetGridEquipment();
            GetCounter();
            GetStatus();
        }

        #region Equipment
        private void GetGridEquipment()
        {
            var list = BLLEquipment.Instance.Gets(equipTypeId);
            list.Add(new EquipmentModel() { Id = 0, Code = 0, Name = "", EquipTypeId = 0, CounterId = 0, StatusId = 0, Position = "", EndTime = null, Note = "" });
            gridEquipment.DataSource = list;
        }

        private void gridViewEquipment_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        { 
            try
            {
                int Id = 0;
                int n = 0;
                int.TryParse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Id").ToString(), out Id);
                bool isNumber = int.TryParse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Code").ToString(), out n);
                if (Id == 0 && string.IsNullOrEmpty(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Name").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Code").ToString()))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "CounterId").ToString())|| gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "CounterId").ToString()=="0"))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "StatusId").ToString())|| gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "StatusId").ToString()=="0"))
                    goto End;
                if (Id != 0 && string.IsNullOrEmpty(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên thiết bị.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Code").ToString()))
                    MessageBox.Show("Vui lòng nhập mã thiết bị.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && isNumber == false)
                    MessageBox.Show("Mã thiết bị phải là dạng chữ số. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && n <= 0)
                    MessageBox.Show("Mã thiết bị phải là dạng chữ số lớn hơn 0. Xin nhập lại", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 else if (Id != 0 && int.Parse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "CounterId").ToString()) == 0)
                    MessageBox.Show("Vui lòng chọn quầy.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && int.Parse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "StatusId").ToString()) == 0)
                    MessageBox.Show("Vui lòng chọn trạng thái.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Equipment();
                    obj.Id = Id;
                    obj.Name = gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Name").ToString();
                    obj.Code = int.Parse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Code").ToString());
                    obj.EquipTypeId = equipTypeId;
                    obj.CounterId = int.Parse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "CounterId").ToString());
                    obj.StatusId = int.Parse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "StatusId").ToString());
                    obj.Position = gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Position") != null ? gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Position").ToString() : "";
                    
                    //DateTime? et = (DateTime)gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "EndTime");
                    //obj.EndTime = et != null ? et : null;
                    obj.Note = gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Note") != null ? gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLEquipment.Instance.Insert(obj);
                        if(result == 0)
                        {
                            MessageBox.Show("Tên thiết bị đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLEquipment.Instance.Update(obj);
                        if(result == false)
                        {
                            MessageBox.Show("Tên thiết bị đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridEquipment();
                   frmMain.lib_Equipments = BLLEquipment.Instance.Gets((int)eEquipType.Counter);
                }  
            }
            catch (Exception ex)
            {
            }
         End:
            {
                
            }
        }
        private void repbtnDeleteEquipment_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewEquipment.GetRowCellValue(gridViewEquipment.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLEquipment.Instance.Delete(Id);
                GetGridEquipment();
                frmMain.lib_Equipments = BLLEquipment.Instance.Gets((int)eEquipType.Counter);
            }
        }

        private void GetCounter()
        {
            //Load SelectBox Quầy
            lookUpCounter.DataSource = null;
            lookUpCounter.DataSource = BLLCounter.Instance.GetLookUp();
            lookUpCounter.DisplayMember = "Name";
            lookUpCounter.ValueMember = "Id";
            lookUpCounter.PopulateViewColumns();
            lookUpCounter.View.Columns[0].Visible = false;
            lookUpCounter.View.Columns[1].Caption = "Quầy";
        }

        private void GetStatus()
        {
            //Load SelectBox Trạng Thái
            lookUpStatus.DataSource = null;
            lookUpStatus.DataSource = BLLStatus.Instance.GetLookUp();
            lookUpStatus.DisplayMember = "Name";
            lookUpStatus.ValueMember = "Id";
            lookUpStatus.PopulateViewColumns();
            lookUpStatus.View.Columns[0].Visible = false;
            lookUpStatus.View.Columns[1].Caption = "Trạng thái";
        }
        #endregion

        #region EquipType
        private void GetGridEquipType()  // vua load len gridview, vua load len combobox Loai thiet bi
        {
            lookUpEquipType.DataSource = null;
            lookUpEquipType.DataSource = BLLEquipType.Instance.GetLookUp();
            lookUpEquipType.DisplayMember = "Name";
            lookUpEquipType.ValueMember = "Id";
            lookUpEquipType.PopulateViewColumns();
            lookUpEquipType.View.Columns[0].Visible = false;
            lookUpEquipType.View.Columns[1].Caption = "Loại thiết bị";

            var list = BLLEquipType.Instance.Gets();
            list.Add(new EquipTypeModel() { Id = 0, Name = "", Note = "" });
            gridEquipType.DataSource = list;
        }
        private void gridViewEquipType_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Name").ToString()))
                   goto End;
                if (Id != 0 && string.IsNullOrEmpty(gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Tên loại thiết bị đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_EquipmentType();
                    obj.Id = Id;
                    obj.Name = gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Name").ToString();
                    obj.Note = gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Note") != null ? gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLEquipType.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Tên loại thiết bị đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLEquipType.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Tên loại thiết bị đã tồn tại. Xin nhập tên khác.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    GetGridEquipType();
                }
            }
            catch (Exception ex)
            {
            }
            End:  // cai nay la Label
            {

            }
        }
        private void repbtnDeleteEquipType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLEquipType.Instance.Delete(Id);
                GetGridEquipType();
            }
        }
        #endregion

        private void btnResetEquipType_Click(object sender, EventArgs e)
        {
            GetGridEquipType();
        }

        private void btnResetEquip_Click(object sender, EventArgs e)
        {
            GetGridEquipment();
        }

        private void btnResetCounter_Click(object sender, EventArgs e)
        {
            GetCounter();
        }

        private void btnResetStatus_Click(object sender, EventArgs e)
        {
            GetStatus();
        }

        private void repbtnDetail_Click(object sender, EventArgs e)
        {
 int.TryParse(gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Id").ToString(), out equipTypeId);
            GetGridEquipment();
            groupControl2.Text = "Danh sách thiết bị của loại : " + gridViewEquipType.GetRowCellValue(gridViewEquipType.FocusedRowHandle, "Name").ToString();
        
        }
    }
}
