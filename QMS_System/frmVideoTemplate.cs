using DevExpress.XtraEditors.Repository;
using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmVideoTemplate : Form
    {
        int temId = 0;
        public frmVideoTemplate()
        {
            InitializeComponent();
        }

        #region Video template 
        private void frmVideoTemplate_Load(object sender, EventArgs e)
        {
            LoadGridVideoTemplate();
        }

        private void LoadGridVideoTemplate()
        {
            gridVideo.DataSource = null;
            var templates = new List<VideoTemplateModel>();
            templates.Add(new VideoTemplateModel() { Id = 0, TemplateName = "", Note = "", IsActive = true });
            templates.AddRange(BLLVideoTemplate.Instance.Gets());
            gridVideo.DataSource = templates;

            repLKVideo.DataSource = null;
            repLKVideo.DataSource = BLLVideo.Instance.GetLookUp();
            repLKVideo.DisplayMember = "Name";
            repLKVideo.ValueMember = "Id";
            repLKVideo.PopulateViewColumns();
            repLKVideo.View.Columns[0].Caption = "Id";
            repLKVideo.View.Columns[0].Visible = false;
            repLKVideo.View.Columns[2].Visible = false;
            repLKVideo.View.Columns[3].Visible = false;
            repLKVideo.View.Columns[1].Caption = "Video";
            repLKVideo.View.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
        }

        private void gridViewVideo_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (Convert.ToInt32(gridViewVideo.GetRowCellValue(e.RowHandle, "Id")) == 0 && e.Column.Name == "colEdit")
            {
                var ritem = new RepositoryItemButtonEdit();
                ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                ritem.ReadOnly = true;
                ritem.Buttons[0].Visible = false;
                e.RepositoryItem = ritem;
            }
            else if (Convert.ToInt32(gridViewVideo.GetRowCellValue(e.RowHandle, "Id")) == 0 && e.Column.Name == "ColDelete")
            {
                var ritem = new RepositoryItemButtonEdit();
                ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                ritem.ReadOnly = true;
                ritem.Buttons[0].Image = global::QMS_System.Properties.Resources.add;
                ritem.Buttons[0].ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
                ritem.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                ritem.Buttons[0].Enabled = true;
                ritem.Click += ritem_Click;
                e.RepositoryItem = ritem;
            }
        }

        private void ritem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "Id").ToString(), out Id);
                if (string.IsNullOrEmpty(gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "TemplateName").ToString()))
                    MessageBox.Show("Vui lòng nhập tên mẫu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_VideoTemplate();
                    obj.Id = Id;
                    obj.TemplateName = gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "TemplateName").ToString();
                    obj.IsActive = Convert.ToBoolean(gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "IsActive").ToString());
                    obj.Note = gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "Note") != null ? gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "Note").ToString() : "";
                    var rs = BLLVideoTemplate.Instance.InsertOrUpdate(obj);
                    if (rs.IsSuccess)
                    {
                        LoadGridVideoTemplate();
                    }
                    else
                        MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void gridViewVideo_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int Id = 0;
            int.TryParse(gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "Id").ToString(), out Id);
            if (Id != 0)
                Save();
        }

        private void repbtn_deleteCounter_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLVideoTemplate.Instance.Delete(Id);
                LoadGridVideoTemplate();
            }
        }

        private void repbtnDetail_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewVideo.GetRowCellValue(gridViewVideo.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                temId = Id;
                LoadGridDetail();
            }
        }

        #endregion

        #region Detail
        private void LoadGridDetail()
        {
            gridChild.DataSource = null;
            var details = new List<VideoTemplate_DeModel>();
            details.Add(new VideoTemplate_DeModel() { Id = 0, Index = 0, TemplateId = 0, VideoId = 0 });
            details.AddRange(BLLVideoTemplate_De.Instance.Gets(temId));
            gridChild.DataSource = details;
        }

        private void gridViewChild_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int Id = 0;
            int.TryParse(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Id").ToString(), out Id);
            if (Id != 0)
                SaveDetail();
        }

        private void gridViewChild_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (Convert.ToInt32(gridViewChild.GetRowCellValue(e.RowHandle, "Id")) == 0 && e.Column.Caption == "")
            {
                var ritem = new RepositoryItemButtonEdit();
                ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                ritem.ReadOnly = true;
                ritem.Buttons[0].Image = global::QMS_System.Properties.Resources.add;
                ritem.Buttons[0].ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
                ritem.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                ritem.Buttons[0].Enabled = true;
                ritem.Click += SaveDetail_Click;
                e.RepositoryItem = ritem;
            }
        }

        private void SaveDetail_Click(object sender, EventArgs e)
        {
            SaveDetail();
        }

        private void SaveDetail()
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Id").ToString(), out Id);
                if (string.IsNullOrEmpty(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (string.IsNullOrEmpty(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "VideoId").ToString()))
                    MessageBox.Show("Vui lòng chọn video.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_VideoTemplate_De();
                    obj.Id = Id;
                    obj.TemplateId = temId;
                    obj.Index = Convert.ToInt32(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Index").ToString());
                    obj.VideoId = Convert.ToInt32(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "VideoId").ToString());
                    var rs = BLLVideoTemplate_De.Instance.InsertOrUpdate(obj);
                    if (rs.IsSuccess)
                    {
                        LoadGridDetail();
                    }
                    else
                        MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void repbtnDeleteChild_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLVideoTemplate.Instance.Delete(Id);
                LoadGridVideoTemplate();
            }
        }
        #endregion

        private void btnResetTemplate_Click(object sender, EventArgs e)
        {
            LoadGridVideoTemplate();
        }

        private void btnResetDetailGrid_Click(object sender, EventArgs e)
        {
            LoadGridDetail();
        }
    }
}
