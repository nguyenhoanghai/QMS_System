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
    public partial class frmAction : Form
    {
        int actionId = 0;
        public frmAction()
        {
            InitializeComponent();
        }

        private void frmAction_Load(object sender, EventArgs e)
        {
            GetGridAction();
            GetGridActionParameter();
        }

        #region Action
        private void GetGridAction()
        { 
            var list = BLLAction.Instance.Gets();
            list.Add(new ActionModel() { Id = 0, Index = BLLAction.Instance.GetLastIndex() + 1, Code = "", Function = "", Note = "" });
            gridAction.DataSource = list;
        }
        private void repbtn_deleteAction_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLAction.Instance.Delete(Id);
                GetGridAction();
            }
        }
        private void gridViewAction_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Code").ToString()))
                    goto End;
                if (Id != 0 && string.IsNullOrEmpty(gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Code").ToString()))
                    MessageBox.Show("Vui lòng nhập mã hành động.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_Action();
                    obj.Id = Id;
                    obj.Index = int.Parse(gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Index").ToString());
                    obj.Code = gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Code").ToString();
                    obj.Function = gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Function") != null ? gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Function").ToString() : "";
                    obj.Note = gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Note") != null ? gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                    {
                        int result = BLLAction.Instance.Insert(obj);
                        if (result == 0)
                        {
                            MessageBox.Show("Mã hành động đã tồn tại. Xin nhập mã khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }
                    else
                    {
                        bool result = BLLAction.Instance.Update(obj);
                        if (result == false)
                        {
                            MessageBox.Show("Mã hành động đã tồn tại. Xin nhập mã khác", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto End;
                        }
                    }

                    GetGridAction();
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

        #region ActionParameter
        private void GetGridActionParameter()
        {
            var list = BLLActionParameter.Instance.Gets(actionId);
            list.Add(new ActionParamModel() { Id = 0, ActionId = 0, ParameterCode = "", Note = "" });
            gridActionParameter.DataSource = list;
        }
        private void repbtn_deleteActionParam_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewActionParameter.GetRowCellValue(gridViewActionParameter.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLActionParameter.Instance.Delete(Id);
                GetGridActionParameter();
            }
        }
        private void gridViewActionParameter_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewActionParameter.GetRowCellValue(gridViewActionParameter.FocusedRowHandle, "Id").ToString(), out Id);
                 if (Id == 0 && string.IsNullOrEmpty(gridViewActionParameter.GetRowCellValue(gridViewActionParameter.FocusedRowHandle, "ParameterCode").ToString()))
                    goto End;

                 if (Id != 0 && string.IsNullOrEmpty(gridViewActionParameter.GetRowCellValue(gridViewActionParameter.FocusedRowHandle, "ParameterCode").ToString()))
                    MessageBox.Show("Vui lòng nhập tham số.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_ActionParameter();
                    obj.Id = Id;
                    obj.ParameterCode = gridViewActionParameter.GetRowCellValue(gridViewActionParameter.FocusedRowHandle, "ParameterCode").ToString();
                    obj.ActionId = actionId;
                    obj.Note = gridViewActionParameter.GetRowCellValue(gridViewActionParameter.FocusedRowHandle, "Note") != null ? gridViewActionParameter.GetRowCellValue(gridViewActionParameter.FocusedRowHandle, "Note").ToString() : "";

                    if (obj.Id == 0)
                        BLLActionParameter.Instance.Insert(obj);
                    else
                        BLLActionParameter.Instance.Update(obj);
                    GetGridActionParameter();
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

        private void btnResetAction_Click(object sender, EventArgs e)
        {
            GetGridAction();
        }

        private void btnResetActionParam_Click(object sender, EventArgs e)
        {
            GetGridActionParameter();
        }

        private void repbtnDetail_Click(object sender, EventArgs e)
        {
  int.TryParse(gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Id").ToString(), out actionId);
            GetGridActionParameter();
            groupControl2.Text = "Danh sách tham số của hành động : " + gridViewAction.GetRowCellValue(gridViewAction.FocusedRowHandle, "Code").ToString();
        
        }
        
    }
}
