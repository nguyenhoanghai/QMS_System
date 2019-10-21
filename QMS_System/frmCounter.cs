using DevExpress.XtraEditors.Controls;
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
    public partial class frmCounter : Form
    {
        int counterId = 0;
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmCounter()
        {
            InitializeComponent();
        }

        private void frmCounter_Load(object sender, EventArgs e)
        {
            GetGridCounter();
            // GetSound();
        }

        #region Counter
        private void GetGridCounter()
        {
            var list = BLLCounter.Instance.Gets(connect);
            list.Add(new CounterModel() { Id = 0, ShortName = "", Name = "",   Position = "", Acreage = "" });
            gridCounter.DataSource = list;
        } 
         
        private void gridViewCounter_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Name").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "ShortName").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Index").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên quầy.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "ShortName").ToString()))
                    MessageBox.Show("Vui lòng nhập tên rút gọn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Counter();
                    obj.Id = Id;
                    obj.ShortName = gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "ShortName").ToString();
                    obj.Name = gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Name").ToString();
                    obj.Index = int.Parse(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Index").ToString());
                    obj.Position = gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Position") != null ? gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Position").ToString() : "";
                    obj.Acreage = gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Acreage") != null ? gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Acreage").ToString() : "";
                    int kq = 0;
                    if (obj.Id == 0)
                        kq = BLLCounter.Instance.Insert(connect,obj);
                    else
                        kq = BLLCounter.Instance.Update(connect,obj);

                    if (kq == 0)
                    {
                        MessageBox.Show("Tên quầy đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto End;
                    }
                    else
                        GetGridCounter();
                }
            }
            catch (Exception ex)
            {
            }
        End: { }
        }

        private void repbtn_deleteCounter_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLCounter.Instance.Delete(connect,Id);
                GetGridCounter();
            }
        }
        private void btnResetCounter_Click(object sender, EventArgs e)
        {
            GetGridCounter();
        }
        private void repbtnDetail_Click(object sender, EventArgs e)
        {
            GetGridChild();
        }


        #endregion

        private void btnResetSound_Click(object sender, EventArgs e)
        {
            //  GetSound();
        }

        #region Child
        private void GetGridChild()
        {
            GetLanguage();
            int Id = int.Parse(gridViewCounter.GetRowCellValue(gridViewCounter.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                var list = BLLCounterSound.Instance.Gets(connect, Id);
                list.Add(new CounterSoundModel() { Id = 0, SoundName = "", });
                gridChild.DataSource = list;
                counterId = Id;
            }
            else
            {
                gridChild.DataSource = null;
                counterId = 0;
            }
        }

        private void repbtnDeleteChild_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLCounterSound.Instance.Delete(connect,Id);
                GetGridChild();
            }
        }

        private void gridViewChild_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "SoundName").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "LanguageId").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "SoundName").ToString()))
                    MessageBox.Show("Vui lòng nhập tên tệp âm thanh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "LanguageId").ToString()))
                    MessageBox.Show("Vui lòng chọn ngôn ngữ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_CounterSound();
                    obj.Id = Id;
                    obj.SoundName = gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "SoundName").ToString();
                    obj.LanguageId = (int)gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "LanguageId");
                    obj.CounterId = counterId;
                    obj.Note = gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Note") != null ? gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Note").ToString() : "";
                    int kq = 0;
                    if (obj.Id == 0)
                        kq = BLLCounterSound.Instance.Insert(connect,obj);
                    else
                        kq = BLLCounterSound.Instance.Update(connect,obj);

                    if (kq == 0)
                    {
                        MessageBox.Show("Tên tệp với ngôn ngữ này đã tồn tại. Xin nhập tên khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto End;
                    }
                    else
                        GetGridChild();
                }
            }
            catch (Exception ex)
            {
            }
        End: { }
        }

        private void GetLanguage(){
             gridLookUpLanguage.DataSource = null;
             gridLookUpLanguage.DataSource = BLLLanguage.Instance.GetLookUp(connect);
             gridLookUpLanguage.DisplayMember = "Name";
             gridLookUpLanguage.ValueMember = "Id";
             gridLookUpLanguage.PopulateViewColumns();
             gridLookUpLanguage.View.Columns[0].Caption = "Id";
             gridLookUpLanguage.View.Columns[0].Visible = false;
             gridLookUpLanguage.View.Columns[2].Visible = false;
             gridLookUpLanguage.View.Columns[3].Visible = false;
             gridLookUpLanguage.View.Columns[1].Caption = "Ngôn ngữ";
             gridLookUpLanguage.View.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
        } 
        #endregion





    }
}
