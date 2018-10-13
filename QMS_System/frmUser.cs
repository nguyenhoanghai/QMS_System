using DevExpress.XtraEditors.Controls;
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
    public partial class frmUser : Form
    {
        public frmUser()
        {
            InitializeComponent();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            GetCounter();
            GetGridUser();

        }

        #region User
        private void GetGridUser()
        {
            lookUpSex.DataSource = null;
            var listSex = new List<ModelSelectItem>();
            listSex.Add(new ModelSelectItem() { Id = 0, Name = "Nữ" });
            listSex.Add(new ModelSelectItem() { Id = 1, Name = "Nam" });
            lookUpSex.DataSource = listSex;
            lookUpSex.DisplayMember = "Name";
            lookUpSex.ValueMember = "Id";
            lookUpSex.PopulateViewColumns();
            lookUpSex.View.Columns[0].Caption = "Id";
            lookUpSex.View.Columns[0].Visible = false;
            lookUpSex.View.Columns[1].Caption = "Giới tính";
            lookUpSex.View.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;


            var list = BLLUser.Instance.Gets();
            list.Add(new UserModel()
            {
                Id = 0,
                Name = "",
                Sex = false,
                Address = "",
                UserName = "",
                Password = "",
                Help = "",
                Avatar = "",
                Position = "",
                Professional = "",
                WorkingHistory = "",
            });
            gridUser.DataSource = list;
        }
        private void repbtn_deleteUser_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLUser.Instance.Delete(Id);
                GetGridUser();
            }
        }
        private void gridViewUser_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Name").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên nhân viên.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_User();
                    obj.Id = Id;
                    obj.Name = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Name").ToString();
                    obj.Sex = (bool)gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Sex");
                    obj.Address = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Address") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Address").ToString() : "";
                    obj.Help = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Help") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Help").ToString() : "";
                    obj.Position = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Position") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Position").ToString() : "";
                    obj.UserName = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "UserName") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "UserName").ToString() : "";
                    obj.Password = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Password") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Password").ToString() : "";
                    obj.Professional = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Professional") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Professional").ToString() : "";
                    obj.WorkingHistory = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "WorkingHistory") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "WorkingHistory").ToString() : "";
                    obj.Counters = gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Counters") != null ? gridViewUser.GetRowCellValue(gridViewUser.FocusedRowHandle, "Counters").ToString() : "0";
                     if (obj.Id == 0)
                        BLLUser.Instance.Insert(obj);
                    else
                        BLLUser.Instance.Update(obj);
                    GetGridUser();
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            GetGridUser();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GetCounter();
        }
        private void GetCounter()
        {
            repcbCounter.Items.Clear();
            var objs = BLLCounter.Instance.GetLookUp();
            if (objs.Count > 0)
                for (int i = 0; i < objs.Count; i++)
                    repcbCounter.Items.Add(new CheckedListBoxItem(objs[i].Id, objs[i].Name));
        }
    }
}
