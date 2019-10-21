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
    public partial class frmCounterSound : Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmCounterSound()
        {
            InitializeComponent();
        }

        private void frmCounterSound_Load(object sender, EventArgs e)
        {
            GetGridCounterSound();
            GetCounter();
            GetSound();
        }

        #region CounterSound
        private void GetGridCounterSound()
        {
            var list = BLLCounterSound.Instance.Gets(connect, 1);
            list.Add(new CounterSoundModel() { Id = 0, CounterId = 0,   Note = "" });
            gridCounterSound.DataSource = list;
        }

        private void GetCounter()
        {
            lookUpCounter.DataSource = null;
            lookUpCounter.DataSource = BLLCounter.Instance.GetLookUp(connect);
            lookUpCounter.DisplayMember = "Name";
            lookUpCounter.ValueMember = "Id";
            lookUpCounter.PopulateViewColumns();
            lookUpCounter.View.Columns[0].Visible = false;
            lookUpCounter.View.Columns[1].Caption = "Quầy";
        }
        private void GetSound()
        {
            lookUpSound.DataSource = null;
            lookUpSound.DataSource = BLLSound.Instance.GetLookUp(connect);
            lookUpSound.DisplayMember = "Name";
            lookUpSound.ValueMember = "Id";
            lookUpSound.PopulateViewColumns();
            lookUpSound.View.Columns[0].Visible = false;
            lookUpSound.View.Columns[1].Caption = "Âm thanh";
        }
        private void repbtn_deleteCounterSound_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLCounterSound.Instance.Delete(connect, Id);
                GetGridCounterSound();
                frmMain.lib_CounterSound = BLLCounterSound.Instance.Gets(connect);
            }
        }
        private void gridViewCounterSound_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && (string.IsNullOrEmpty(gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "CounterId").ToString()) || gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "CounterId").ToString() == "0"))
                    goto End;
                else if (Id == 0 && (string.IsNullOrEmpty(gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "SoundId").ToString()) || gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "SoundId").ToString() == "0"))
                    goto End;

                if (Id != 0 && (string.IsNullOrEmpty(gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "CounterId").ToString()) || gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "CounterId").ToString() == "0"))
                    MessageBox.Show("Vui lòng chọn Quầy.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && (string.IsNullOrEmpty(gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "SoundId").ToString()) || gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "SoundId").ToString() == "0"))
                    MessageBox.Show("Vui lòng chọn Âm thanh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_CounterSound();
                    obj.Id = Id;
                    obj.CounterId = int.Parse(gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "CounterId").ToString());
                    obj.SoundName =  gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "SoundId").ToString() ;
                    obj.Note = gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "Note") != null ? gridViewCounterSound.GetRowCellValue(gridViewCounterSound.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                        BLLCounterSound.Instance.Insert(connect, obj);
                    else
                        BLLCounterSound.Instance.Update(connect, obj);
                    GetGridCounterSound();
                    frmMain.lib_CounterSound = BLLCounterSound.Instance.Gets(connect);
                }
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }
        private void btnResetCounterSound_Click(object sender, EventArgs e)
        {
            GetGridCounterSound();
        }

        private void btnResetCounter_Click(object sender, EventArgs e)
        {
            GetCounter();
        }
        private void btnResetSound_Click(object sender, EventArgs e)
        {
            GetSound();
        }
        #endregion

        
    }
}
