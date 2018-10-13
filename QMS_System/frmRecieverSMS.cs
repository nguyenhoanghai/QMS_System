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
    public partial class frmRecieverSMS : Form
    {
        public frmRecieverSMS()
        {
            InitializeComponent();
        }

        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            GetGrid();
        }

        private void gridViewSMS_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "PhoneNumber").ToString()))
                    goto End;
                if (Id != 0 && string.IsNullOrEmpty(gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "PhoneNumber").ToString()))
                    MessageBox.Show("Vui lòng nhập số điện thoại nhận tin nhắn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new Q_RecieverSMS();
                    obj.Id = Id;
                    obj.PhoneNumber = gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "PhoneNumber").ToString();
                    obj.IsActive = bool.Parse(gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "IsActive").ToString());
                    obj.Note = gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "Note") != null ? gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "Note").ToString() : "";

                    var rs = BLLRecieverSMS.Instance.InsertOrUpdate(obj);
                    if (rs.IsSuccess)
                        GetGrid();
                    else
                    {
                        MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        goto End;
                    }
                }
            }
            catch (Exception ex)
            { }
        End: { }
        }

        private void repbtn_delete_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewSMS.GetRowCellValue(gridViewSMS.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                var rs = BLLRecieverSMS.Instance.Delete(Id);
                if (rs.IsSuccess)
                    GetGrid();
                MessageBox.Show(rs.Errors[0].Message, rs.Errors[0].MemberName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmRecieverSMS_Load(object sender, EventArgs e)
        {
            GetGrid();
        }

        private void GetGrid()
        {
            var list = BLLRecieverSMS.Instance.Gets();
            list.Add(new RecieverSMSModel() { Id = 0 });
            gridSMS.DataSource = list;
        }
         
    }
}
