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
    public partial class frmCommand : Form
    {
        int commandId = 0;
        public frmCommand()
        {
            InitializeComponent();
        }

        private void frmCommand_Load(object sender, EventArgs e)
        {
            GetGridCommand();
            GetGridCommandParameter();
        }

        #region Command
        private void GetGridCommand()
        {
            var list = BLLCommand.Instance.Gets();
            list.Add(new CommandModel() { Id = 0, Code = "", CodeHEX = "", Note = "" });
            gridCommand.DataSource = list;
        }
        private void repbtn_deleteCommand_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLCommand.Instance.Delete(Id);
                GetGridCommand();
            }
        }
        private void gridViewCommand_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Code").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "CodeHEX").ToString()))
                    goto End;
                if (Id != 0 && string.IsNullOrEmpty(gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Code").ToString()))
                    MessageBox.Show("Vui lòng nhập mã lệnh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "CodeHEX").ToString()))
                    MessageBox.Show("Vui lòng nhập mã lệnh Hex.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Command();
                    obj.Id = Id;
                    obj.Code = gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Code").ToString();
                    obj.CodeHEX = gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "CodeHEX").ToString();
                    obj.Note = gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Note") != null ? gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                        BLLCommand.Instance.Insert(obj);
                    else
                        BLLCommand.Instance.Update(obj);
                    GetGridCommand();
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

        #region CommandParameter
        private void GetGridCommandParameter()
        {
            var list = BLLCommandParameter.Instance.Gets(commandId);
            list.Add(new CommandParamModel() { Id = 0, CommandId = 0, Parameter = "0", Note = "" });
            gridCommandParameter.DataSource = list;
        }
        private void repbtn_deleteCommandParameter_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewCommandParameter.GetRowCellValue(gridViewCommandParameter.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLCommandParameter.Instance.Delete(Id);
                GetGridCommandParameter();
            }
        }
        private void gridViewCommandParameter_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewCommandParameter.GetRowCellValue(gridViewCommandParameter.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewCommandParameter.GetRowCellValue(gridViewCommandParameter.FocusedRowHandle, "Parameter").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridViewCommandParameter.GetRowCellValue(gridViewCommandParameter.FocusedRowHandle, "Parameter").ToString()))
                    MessageBox.Show("Vui lòng nhập tham số lệnh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_CommandParameter();
                    obj.Id = Id;
                    obj.Parameter =  gridViewCommandParameter.GetRowCellValue(gridViewCommandParameter.FocusedRowHandle, "Parameter").ToString() ;
                    obj.CommandId = commandId;
                    obj.Note = gridViewCommandParameter.GetRowCellValue(gridViewCommandParameter.FocusedRowHandle, "Note") != null ? gridViewCommandParameter.GetRowCellValue(gridViewCommandParameter.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                        BLLCommandParameter.Instance.Insert(obj);
                    else
                        BLLCommandParameter.Instance.Update(obj);
                    GetGridCommandParameter();
                }
            }
            catch (Exception ex)
            { }
        End:
            { }
        }
        #endregion

        private void btnResetCommand_Click(object sender, EventArgs e)
        {
            GetGridCommand();
        }

        private void btnResetCommandParam_Click(object sender, EventArgs e)
        {
            GetGridCommandParameter();
        }

        private void repbtnDetail_Click(object sender, EventArgs e)
        {
            int.TryParse(gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "Id").ToString(), out commandId);
            GetGridCommandParameter();
            groupControl2.Text = "Danh sách tham số của lệnh : " + gridViewCommand.GetRowCellValue(gridViewCommand.FocusedRowHandle, "CodeHEX").ToString();

        }
    }
}
