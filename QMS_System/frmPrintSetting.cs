using GPRO.Core.Hai;
using QMS_System.Data.BLL;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmPrintSetting : Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        int Id = 0;
        public frmPrintSetting()
        {
            InitializeComponent();
        }

        #region event
        private void btNoteDichVu_Click(object sender, EventArgs e)
        {
            addEvent("[ghi-chu-dich-vu]");
        }

        private void btTenDichVu_Click(object sender, EventArgs e)
        {
            addEvent("[ten-dich-vu]");
        }

        private void btLeft_Click(object sender, EventArgs e)
        {
            addEvent("[canh-trai]");
        }

        private void btCenter_Click(object sender, EventArgs e)
        {
            addEvent("[canh-giua]");
        }

        private void btRight_Click(object sender, EventArgs e)
        {
            addEvent("[canh-phai]");
        }

        private void bt11_Click(object sender, EventArgs e)
        {
            addEvent("[1x1]");
        }

        private void bt21_Click(object sender, EventArgs e)
        {
            addEvent("[2x1]");
        }

        private void bt31_Click(object sender, EventArgs e)
        {
            addEvent("[3x1]");
        }

        private void bt22_Click(object sender, EventArgs e)
        {
            addEvent("[2x2]");
        }

        private void bt33_Click(object sender, EventArgs e)
        {
            addEvent("[3x3]");
        }

        private void btenter_Click(object sender, EventArgs e)
        {
            addEvent("\n");
        }

        private void btcut_Click(object sender, EventArgs e)
        {
            addEvent("[cat-giay]");
        }

        private void btnngay_Click(object sender, EventArgs e)
        {
            addEvent("[ngay]");
        }

        private void btgio_Click(object sender, EventArgs e)
        {
            addEvent("[gio]");
        }

        private void btDangGoi_Click(object sender, EventArgs e)
        {
            addEvent("[dang-goi]");
        }

        private void btSTT_Click(object sender, EventArgs e)
        {
            addEvent("[STT]");
        }

        private void btTenQuay_Click(object sender, EventArgs e)
        {
            addEvent("[ten-quay]");
        }

        private void btnCarNumber_Click(object sender, EventArgs e)
        {
            addEvent("[so-xe]");
        }

        private void btnMaKH_Click(object sender, EventArgs e)
        {
            addEvent("[ma-kh]");
        }

        private void btnTenKH_Click(object sender, EventArgs e)
        {
            addEvent("[ten-kh]");
        }

        private void btnDOB_Click(object sender, EventArgs e)
        {
            addEvent("[dob]");
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            addEvent("[dia-chi]");
        }

        private void btnServiceCode_Click(object sender, EventArgs e)
        {
            addEvent("[ma-dv]");
        }

        private void btnPhone_Click(object sender, EventArgs e)
        {
            addEvent("[phone]");
        }
        #endregion

        private void addEvent(string text)
        {
            string content = txtContent.Text;
            content += text;
            txtContent.Text = content;
        }

        private void btThemVB_Click(object sender, EventArgs e)
        {
            addEvent(txtVB.Text);
            txtVB.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btncancel.Enabled = true;
            btprintTest.Enabled = true;
            btsave.Enabled = true;
            btnAdd.Enabled = false;
            txtContent.Text = "";
            numIndex.Value = 1;
            numsolien.Value = 1;
            txtVB.Text = "";
            Id = 0;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            btncancel.Enabled = false;
            btprintTest.Enabled = false;
            btsave.Enabled = false;
            btnAdd.Enabled = true;
            txtContent.Text = "";
            txtname.Text = "";
            numIndex.Value = 1;
            numsolien.Value = 1;
            txtVB.Text = "";
            Id = 0;
        }

        private void frmPrintSetting_Load(object sender, EventArgs e)
        {
            LoadGrid();

            var services = BLLService.Instance.GetLookUp(connect, false);
            for (int i = 0; i < services.Count; i++)
                cbService.Properties.Items.Add(services[i].Id, services[i].Name, CheckState.Unchecked, true);
        }

        private void LoadGrid()
        {
            var objs = BLLPrintTemplate.Instance.Gets(connect);
            gridPrint.DataSource = objs;

            switch (QMSAppInfo.Version)
            {
                case 1:
                    //frmMain.ticketTemplate = txtContent.Text;
                    //frmMain.solien = (int)txtsolien.Value;
                    break;
                case 3:
                    frmMain_ver3.printTemplates = objs;
                    //frmMain_ver3.solien = (int)txtsolien.Value;
                    break;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void btsave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtname.Text))
            {
                MessageBox.Show("Vui lòng nhập tên mẫu phiếu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtname.Focus();
            }
            else if (string.IsNullOrEmpty(txtContent.Text))
            {
                MessageBox.Show("Vui lòng soạn nội dung phiếu in.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContent.Focus();
            }
            else if (string.IsNullOrEmpty(cbService.EditValue.ToString()))
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbService.Focus();
            }
            else
            {
                var rs = BLLPrintTemplate.Instance.InsertOrUpdate(connect, new Data.Q_PrintTicket() { Id = Id, Name = txtname.Text, PrintTemplate = txtContent.Text, IsActive = true, PrintIndex = (int)numIndex.Value, PrintPages = (int)numsolien.Value }, cbService.EditValue.ToString());
                if (rs.IsSuccess)
                {
                    LoadGrid();
                    btncancel.PerformClick();
                }
                else
                    MessageBox.Show(rs.sms, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btprintTest_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            string content = txtContent.Text;
            content = content.Replace("[canh-giua]", "\x1b\x61\x01|+|");
            content = content.Replace("[canh-trai]", "\x1b\x61\x00|+|");
            content = content.Replace("[1x1]", "\x1d\x21\x00|+|");
            content = content.Replace("[2x1]", "\x1d\x21\x01|+|");
            content = content.Replace("[3x1]", "\x1d\x21\x02|+|");
            content = content.Replace("[2x2]", "\x1d\x21\x11|+|");
            content = content.Replace("[3x3]", "\x1d\x21\x22|+|");

            content = content.Replace("[STT]", "1001");
            content = content.Replace("[ten-quay]", "quay 1");
            content = content.Replace("[ten-dich-vu]", "dich vu 1");
            content = content.Replace("[ghi-chu-dich-vu]", "ghi chú dich vu 1");
            content = content.Replace("[ngay]", ("ngay: " + now.ToString("dd/MM/yyyy")));
            content = content.Replace("[gio]", (" gio: " + now.ToString("HH/mm")));
            content = content.Replace("[dang-goi]", " dang goi 1000");
            content = content.Replace("[so-xe]", " Số xe");
            content = content.Replace("[phone]", " số điện thoại");
            content = content.Replace("[ma-kh]", " mã khách hàng");
            content = content.Replace("[ten-kh]", " tên khách hàng");
            content = content.Replace("[dia-chi]", " địa chỉ");
            content = content.Replace("[ma-dv]", " mã dịch vụ");
            content = content.Replace("[dob]", " năm sinh");
            content = content.Replace("[cat-giay]", "\x1b\x69|+|");

            var arr = content.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            for (int ii = 0; ii < numsolien.Value; ii++)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    switch (QMSAppInfo.Version)
                    {
                        case 1:
                            BaseCore.Instance.PrintTicketTCVN3(frmMain.COM_Printer, arr[i]); break;
                        case 3:
                            BaseCore.Instance.PrintTicketTCVN3(frmMain_ver3.COM_Printer, arr[i]); break;
                    }
                }
            }
        }

        private void repbtnEdit_Click(object sender, EventArgs e)
        {
            int.TryParse(gridViewPrint.GetRowCellValue(gridViewPrint.FocusedRowHandle, "Id").ToString(), out Id);
            txtContent.Text = gridViewPrint.GetRowCellValue(gridViewPrint.FocusedRowHandle, "PrintTemplate").ToString();
            txtname.Text = gridViewPrint.GetRowCellValue(gridViewPrint.FocusedRowHandle, "Name").ToString();
            numIndex.Value = int.Parse(gridViewPrint.GetRowCellValue(gridViewPrint.FocusedRowHandle, "PrintIndex").ToString());
            numsolien.Value = int.Parse(gridViewPrint.GetRowCellValue(gridViewPrint.FocusedRowHandle, "PrintPages").ToString());
            cbService.EditValue =  gridViewPrint.GetRowCellValue(gridViewPrint.FocusedRowHandle, "ServiceNames").ToString() ;
            
            btncancel.Enabled = true;
            btprintTest.Enabled = true;
            btsave.Enabled = true;
            btnAdd.Enabled = false;
        }

    }
}
