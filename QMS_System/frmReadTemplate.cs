using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmReadTemplate : Form
    {
        public frmReadTemplate()
        {
            InitializeComponent();
        }

        private void frmReadTemplate_Load(object sender, EventArgs e)
        {
            GetGridView();
            GetLanguages();
            GetSounds();
        }
        private void btnResetLang_Click(object sender, EventArgs e)
        {
            GetLanguages();
        }
        private void btnResetSound_Click(object sender, EventArgs e)
        {
            GetSounds();
        }
        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            GetGridView();
        } 
        private void GetLanguages()
        {
            repLookUpLanguage.DataSource = null;
            repLookUpLanguage.DataSource = BLLLanguage.Instance.GetLookUp();
            repLookUpLanguage.DisplayMember = "Name";
            repLookUpLanguage.ValueMember = "Id";
            repLookUpLanguage.PopulateViewColumns();
            repLookUpLanguage.View.Columns[0].Visible = false;
            repLookUpLanguage.View.Columns[2].Visible = false;
            repLookUpLanguage.View.Columns[3].Visible = false;
            repLookUpLanguage.View.Columns[1].Caption = "Ngôn ngữ";
        }
        private void GetSounds()
        {
            repLookUpSound.DataSource = null;
            var objs = BLLSound.Instance.GetLookUp();
            objs.Add(new ModelSelectItem() { Id = 9999, Name = "Số phiếu" });
            objs.Add(new ModelSelectItem() { Id = 10000, Name = "Tên quầy" });
            repLookUpSound.DataSource = objs;
            repLookUpSound.DisplayMember = "Name";
            repLookUpSound.ValueMember = "Id";
            repLookUpSound.PopulateViewColumns();
            repLookUpSound.View.Columns[0].Visible = false;
            repLookUpSound.View.Columns[2].Visible = false;
            repLookUpSound.View.Columns[3].Visible = false;
            repLookUpSound.View.Columns[1].Caption = "Âm thanh";
        }
        private void GetGridView()
        {
            var list = BLLReadTemplate.Instance.Gets();
            list.Add(new ReadTemplateModel() { Id = 0 });
            gridReadTemplate.DataSource = list;
        }

        private void repbtnEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Id").ToString());
            GetGridDetail(Id);
        }

        private void GetGridDetail(int Id)
        {
            gridDetail.DataSource = null;
            if (Id != 0)
            {
                var list = BLLReadTempDetail.Instance.Gets(Id);
                list.Add(new ReadTemplateDetailModel() { Id = 0, ReadTemplateId = Id, Index = 0 });
                gridDetail.DataSource = list;
                GetSounds();
            }
        }

        private void repbtnDeleteReadTemp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLReadTempDetail.Instance.Delete(Id);
                GetGridView();
               frmMain.lib_ReadTemplates = BLLReadTemplate.Instance.GetsForMain();
            }
        }
        private void gridViewReadTemplate_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Name").ToString()))
                    goto End;
                if (Id == 0 && int.Parse(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "LanguageId").ToString()) == 0)
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên mẫu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (Id != 0 && int.Parse(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "LanguageId").ToString()) == 0)
                    MessageBox.Show("Vui lòng chọn ngôn ngữ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_ReadTemplate();
                    obj.Id = Id;
                    obj.Name = gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Name").ToString();
                    obj.LanguageId = int.Parse(gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "LanguageId").ToString());
                    obj.Note = gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Note") != null ? gridViewReadTemplate.GetRowCellValue(gridViewReadTemplate.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                        BLLReadTemplate.Instance.Insert(obj);
                    else
                        BLLReadTemplate.Instance.Update(obj);
                    GetGridView();
                    frmMain.lib_ReadTemplates = BLLReadTemplate.Instance.GetsForMain();
                }
            }
            catch (Exception ex)
            { }
        End: { }
        }



        private void repbtnDeleteDetail_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLReadTempDetail.Instance.Delete(Id);
                GetGridDetail(Id);
                frmMain.lib_ReadTemplates = BLLReadTemplate.Instance.GetsForMain();
            }
        }
        private void gridViewDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "Index").ToString()))
                    goto End;
                if (Id == 0 && int.Parse(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "SoundId").ToString()) == 0)
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự đọc âm thanh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (Id != 0 && int.Parse(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "SoundId").ToString()) == 0)
                    MessageBox.Show("Vui lòng chọn âm thanh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_ReadTemp_Detail();
                    obj.Id = Id;
                    obj.Index = int.Parse(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "Index").ToString());
                    obj.SoundId = int.Parse(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "SoundId").ToString());
                    obj.ReadTemplateId = int.Parse(gridViewDetail.GetRowCellValue(gridViewDetail.FocusedRowHandle, "ReadTemplateId").ToString());

                    if (obj.Id == 0)
                        BLLReadTempDetail.Instance.Insert(obj);
                    else
                        BLLReadTempDetail.Instance.Update(obj);
                    GetGridDetail(obj.ReadTemplateId);
                    frmMain.lib_ReadTemplates = BLLReadTemplate.Instance.GetsForMain();
                }
            }
            catch (Exception ex)
            { }
        End: { }
        }


    }
}
