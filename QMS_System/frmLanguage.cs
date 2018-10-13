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
    public partial class frmLanguage : Form
    {
        public frmLanguage()
        {
            InitializeComponent();
        }

        private void frmLanguage_Load(object sender, EventArgs e)
        {
            GetGridLanguage();
        }

        #region Language
        private void GetGridLanguage()
        {
            var list = BLLLanguage.Instance.Gets();
            list.Add(new LanguageModel() { Id = 0, Name = "", Note = "" });
            gridLanguage.DataSource = list;
        }
        private void repbtn_deleteLanguage_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewLanguage.GetRowCellValue(gridViewLanguage.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLLanguage.Instance.Delete(Id);
                GetGridLanguage();
            }
        }
        private void gridViewLanguage_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewLanguage.GetRowCellValue(gridViewLanguage.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewLanguage.GetRowCellValue(gridViewLanguage.FocusedRowHandle, "Name").ToString()))
                    goto End;
              
                if (Id != 0 && string.IsNullOrEmpty(gridViewLanguage.GetRowCellValue(gridViewLanguage.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên ngôn ngữ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Language();
                    obj.Id = Id;
                    obj.Name = gridViewLanguage.GetRowCellValue(gridViewLanguage.FocusedRowHandle, "Name").ToString();
                    obj.Note = gridViewLanguage.GetRowCellValue(gridViewLanguage.FocusedRowHandle, "Note") != null ? gridViewLanguage.GetRowCellValue(gridViewLanguage.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLLanguage.Instance.Insert(obj);
                        if(result == 0)
                        {
                            MessageBox.Show("Tên ngôn ngữ này đã tồn tại. Xin nhập lại.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }  
                    else
                    {
                        bool result = BLLLanguage.Instance.Update(obj);
                        if(result == false)
                        {
                            MessageBox.Show("Tên ngôn ngữ này đã tồn tại. Xin nhập lại.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                       
                    GetGridLanguage();
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

        private void btnResetMajor_Click(object sender, EventArgs e)
        {
            GetGridLanguage();
        }
    }
}
