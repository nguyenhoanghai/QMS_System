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
    public partial class frmMajor :  Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmMajor()
        {
            InitializeComponent();
        }

        private void frmMajor_Load(object sender, EventArgs e)
        {
            GetGridMajor();
        }

        #region Major
        private void GetGridMajor()
        {
            var list = BLLMajor.Instance.Gets(connect);
            list.Add(new MajorModel() { Id = 0, Name = "", Note = "" });
            gridMajor.DataSource = list;
        }
        private void repbtn_deleteMajor_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewMajor.GetRowCellValue(gridViewMajor.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLMajor.Instance.Delete(connect,Id);
                GetGridMajor();
            }
        }
        private void gridViewMajor_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewMajor.GetRowCellValue(gridViewMajor.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewMajor.GetRowCellValue(gridViewMajor.FocusedRowHandle, "Name").ToString()))
                    goto End;
                if (Id != 0 && string.IsNullOrEmpty(gridViewMajor.GetRowCellValue(gridViewMajor.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên nghiệp vụ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Major();
                    obj.Id = Id;
                    obj.Name = gridViewMajor.GetRowCellValue(gridViewMajor.FocusedRowHandle, "Name").ToString();
                    obj.Note = gridViewMajor.GetRowCellValue(gridViewMajor.FocusedRowHandle, "Note") != null ? gridViewMajor.GetRowCellValue(gridViewMajor.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLMajor.Instance.Insert(connect, obj);
                        if(result == 0)
                        {
                            MessageBox.Show("Tên nghiệp vụ đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    } 
                    else
                    {
                        bool result = BLLMajor.Instance.Update(connect, obj);
                        if(result == false)
                        {
                            MessageBox.Show("Tên nghiệp vụ đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                        
                    GetGridMajor();
                }
            }
            catch (Exception ex)
            {
            }
            End:
            {

            }
        }
        private void btnResetMajor_Click(object sender, EventArgs e)
        {
            GetGridMajor();
        }
        #endregion
    }
}
