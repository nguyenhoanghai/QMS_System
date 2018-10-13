using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using QMS_System.Data;
using QMS_System.Data.Enum;

namespace QMS_System
{
    public partial class frmSound : DevExpress.XtraEditors.XtraForm
    {
        public frmSound()
        {
            InitializeComponent();
        }

        private void frmSound_Load(object sender, EventArgs e)
        {
            //GetDrives();
            GetGridSound();
        }

        #region Sound
        private void GetGridSound()
        {
            //DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
            //string soundpath = BLLConfig.Instance.GetConfigByCode(eConfigCode.SoundPath);
            //DirectoryInfo dirInfo = new DirectoryInfo(soundpath);
            //if(dirInfo.Exists)
            //{
            //    FileInfo[] listFile = dirInfo.GetFiles("*.wav");
            //    if(listFile.Length >0)
            //    {
            //        List<string> listName = new List<string>();
            //        foreach( FileInfo f in listFile)
            //        {
            //            listName.Add(f.Name);
            //        }
            //        if (listName.Count > 0)
            //        {
            //            lookUpName.DataSource = null;
            //            lookUpName.DataSource = listName;
            //            lookUpName.PopulateViewColumns();
            //            lookUpName.View.Columns[0].Caption = "File";
            //        }
            //        else
            //            lookUpName.DataSource = null;
            //    }                
            //}
            lookUpLanguage.DataSource = null;
            lookUpLanguage.DataSource = BLLLanguage.Instance.GetLookUp();
            lookUpLanguage.DisplayMember = "Name";
            lookUpLanguage.ValueMember = "Id";
            lookUpLanguage.PopulateViewColumns();
            lookUpLanguage.View.Columns[0].Visible = false;
            lookUpLanguage.View.Columns[2].Visible = false;
            lookUpLanguage.View.Columns[3].Visible = false;
            lookUpLanguage.View.Columns[1].Caption = "Ngôn ngữ";

            var list = BLLSound.Instance.Gets();
            list.Add(new SoundModel() { Id = 0, Name = "", LanguageId = 1, Note = "" });
            gridSound.DataSource = list;
        }
        private void repbtn_deleteSound_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLSound.Instance.Delete(Id);
                GetGridSound();
                frmMain.lib_Sounds = BLLSound.Instance.Gets();
            }
        }
        private void gridViewSound_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Name").ToString()))
                    goto End;
                if (Id == 0 && int.Parse(gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "LanguageId").ToString()) == 0)
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập mã trạng thái.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (Id != 0 && int.Parse(gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "LanguageId").ToString()) == 0)
                    MessageBox.Show("Vui lòng chọn ngôn ngữ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Sound();
                    obj.Id = Id;
                    obj.Name = gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Name").ToString();
                    obj.LanguageId = int.Parse(gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "LanguageId").ToString());
                    obj.Code = gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Code") != null ? gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Code").ToString() : "";
                    obj.Note = gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Note") != null ? gridViewSound.GetRowCellValue(gridViewSound.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                        BLLSound.Instance.Insert(obj);
                    else
                        BLLSound.Instance.Update(obj);
                    GetGridSound();
                    frmMain.lib_Sounds = BLLSound.Instance.Gets();
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
            GetGridSound();
        }
   }
}