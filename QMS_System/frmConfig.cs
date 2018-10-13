using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using QMS_System.Data;

namespace QMS_System
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }
        private void frmConfig_Load(object sender, EventArgs e)
        {
            GetGridConfig();
        }
        private void GetGridConfig()
        {
            var list = BLLConfig.Instance.Gets();
            //list.Add(new ConfigModel() { Id = 0, Code = "", Value = "", IsActived = false, Note = "" });
            gridConfig.DataSource = list;
        }

        private void gridViewConfig_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
               int Id = 0;
                int.TryParse(gridViewConfig.GetRowCellValue(gridViewConfig.FocusedRowHandle, "Id").ToString(), out Id);

                if ((Id != 0 || Id == 0) && string.IsNullOrEmpty(gridViewConfig.GetRowCellValue(gridViewConfig.FocusedRowHandle, "Value").ToString()))
                    MessageBox.Show("Vui lòng nhập giá trị.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Config();
                    obj.Id = Id;
                    obj.Code = gridViewConfig.GetRowCellValue(gridViewConfig.FocusedRowHandle, "Code").ToString();
                    obj.Value = gridViewConfig.GetRowCellValue(gridViewConfig.FocusedRowHandle, "Value").ToString();
                    obj.IsActived = (bool)gridViewConfig.GetRowCellValue(gridViewConfig.FocusedRowHandle, "IsActived");
                    obj.Note = gridViewConfig.GetRowCellValue(gridViewConfig.FocusedRowHandle, "Note") != null ? gridViewConfig.GetRowCellValue(gridViewConfig.FocusedRowHandle, "Note").ToString() : "";

                    BLLConfig.Instance.Update(obj);
                    GetGridConfig();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnResetMajor_Click(object sender, EventArgs e)
        {
            GetGridConfig();
        }
    }
}
