using GPRO.Core.Hai;
using QMS_System.Data.BLL;
using QMS_System.Data.Enum;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmTicketTemplate : Form
    {

        public frmTicketTemplate()
        {
            InitializeComponent();
        }

        private void btThemVB_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += (txtVB.Text);
            txtContent.Text = content;
            txtVB.Text = "";
        }

        private void btLeft_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[canh-trai]";
            txtContent.Text = content;
        }

        private void btCenter_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[canh-giua]";
            txtContent.Text = content;
        }

        private void btRight_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[canh-phai]";
            txtContent.Text = content;
        }

        private void bt11_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[1x1]";
            txtContent.Text = content;
        }

        private void bt21_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[2x1]";
            txtContent.Text = content;
        }

        private void bt31_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[3x1]";
            txtContent.Text = content;
        }

        private void bt22_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[2x2]";
            txtContent.Text = content;
        }

        private void bt33_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[3x3]";
            txtContent.Text = content;
        }

        private void btenter_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "\n";
            txtContent.Text = content;
        }

        private void btcut_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[cat-giay]";
            txtContent.Text = content;
        }

        private void btnngay_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[ngay]";
            txtContent.Text = content;
        }

        private void btgio_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[gio]";
            txtContent.Text = content;
        }
        private void btDangGoi_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[dang-goi]";
            txtContent.Text = content;
        }

        private void btSTT_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[STT]";
            txtContent.Text = content;
        }

        private void btTenQuay_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[ten-quay]";
            txtContent.Text = content;
        }


        private void btTest_Click(object sender, EventArgs e)
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
            content = content.Replace("[ngay]", ("ngay: " + now.ToString("dd/MM/yyyy")));
            content = content.Replace("[gio]", (" gio: " + now.ToString("HH/mm")));
            content = content.Replace("[dang-goi]", " dang goi 1000");
            content = content.Replace("[cat-giay]", "\x1b\x69|+|");

            var arr = content.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            for (int ii = 0; ii < txtsolien.Value; ii++)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    //var ss = BaseCore.Instance.abc2unicode(arr[i]);
                    //frmMain.COM_Printer.Write(ss);
                    frmMain.COM_Printer.Write(arr[i]);
                }
            }
        }

        private void btsave_Click(object sender, EventArgs e)
        {
            if (BLLConfig.Instance.Update(frmMain.connectString, eConfigCode.TicketTemplate, txtContent.Text))
                if (BLLConfig.Instance.Update(frmMain.connectString, eConfigCode.NumberOfLinePerTime, txtsolien.Value.ToString()))
                    MessageBox.Show("Cập nhật thành công.!");
                else
                    MessageBox.Show("Cập nhật thất bại");
            else
                MessageBox.Show("Cập nhật thất bại");
        }

        private void frmTicketTemplate_Load(object sender, EventArgs e)
        {
            txtContent.Text = frmMain.ticketTemplate;
            txtsolien.Value = frmMain.solien;
        }

        private void btTenDichVu_Click(object sender, EventArgs e)
        {
            string content = txtContent.Text;
            content += "[ten-dich-vu]";
            txtContent.Text = content;
        }
    }
}
