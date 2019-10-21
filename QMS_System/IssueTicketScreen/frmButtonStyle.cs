using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPRO.Core.Hai;
using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Enum;
using QMS_System.Helper;

namespace QMS_System.IssueTicketScreen
{
    public partial class frmButtonStyle : Form
    {
        QMSSystemEntities db;
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        frmIssueTicketScreen frm;
        string backcolor = "";
        string forecolor = "";
        string fontstr = "";
        string width = "";
        string heigth = "";
        string space = "";
        FontConverter converter = new FontConverter();
        public frmButtonStyle()
        {
            InitializeComponent();
        }

        public frmButtonStyle(frmIssueTicketScreen _frm)
        {
            InitializeComponent();
            frm = _frm;
        }

        private void frmButtonStyle_Load(object sender, EventArgs e)
        {
            UpDownButtonWidth.Value = int.Parse(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonWidth));
            UpDownButtonHeight.Value = int.Parse(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonHeight));
            UpDownButtonSpace.Value = int.Parse(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonSpace));

            btnSampleButton.Size = new Size(int.Parse(UpDownButtonWidth.Value.ToString()), int.Parse(UpDownButtonHeight.Value.ToString()));
            btnSampleButton.Font = (Font)converter.ConvertFromString(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonFont));
            btnSampleButton.BackColor = ColorTranslator.FromHtml(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonBackColor));
            btnSampleButton.ForeColor = ColorTranslator.FromHtml(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonForeColor));
        }
        private void btnButtonBackColor_Click(object sender, EventArgs e)
        {

            ColorDialog colordlg = new ColorDialog();
            colordlg.Color = ColorTranslator.FromHtml(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonBackColor));
            if (colordlg.ShowDialog() == DialogResult.OK)
            {
                backcolor = colordlg.Color.ToArgb().ToString("x");  // chuyển màu sang dạng hex ffffffff
                backcolor = backcolor.Substring(2, 6); // cắt chuổi lấy 6 ký tự cuối chính là mã màu
                backcolor = "#" + backcolor; // thêm # vào trước mã màu được #ffffff
                btnSampleButton.BackColor = colordlg.Color;
            }
        }

        private void btnForeColor_Click(object sender, EventArgs e)
        {
            ColorDialog colordlg = new ColorDialog();
            colordlg.Color = ColorTranslator.FromHtml(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonForeColor));
            if (colordlg.ShowDialog() == DialogResult.OK)
            {
                forecolor = colordlg.Color.ToArgb().ToString("x");
                forecolor = forecolor.Substring(2, 6);
                forecolor = "#" + forecolor; // dạng #ffffff hex
                btnSampleButton.ForeColor = colordlg.Color;
            }

        }

        private void btnFontStyle_Click(object sender, EventArgs e)
        {
            
            FontDialog fontdlg = new FontDialog();
            fontdlg.Font = (Font)converter.ConvertFromString(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonFont));
            if (fontdlg.ShowDialog() == DialogResult.OK)
            {
                Font font = fontdlg.Font;
                fontstr = converter.ConvertToString(font);
                btnSampleButton.Font = fontdlg.Font;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            db = new QMSSystemEntities(connect);
            Q_Config model;

            width = UpDownButtonWidth.Value.ToString();
            heigth = UpDownButtonHeight.Value.ToString();
            space = UpDownButtonSpace.Value.ToString();
            if(backcolor !="")
            {
                model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(eConfigCode.ButtonBackColor));
                model.Value = backcolor;
                BLLConfig.Instance.UpdateConfigValueFromInterface(connect, model);
            }

            if (forecolor != "")
            {
                model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(eConfigCode.ButtonForeColor));
                model.Value = forecolor;
                BLLConfig.Instance.UpdateConfigValueFromInterface(connect, model);
            }

            if (fontstr != "")
            {
                model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(eConfigCode.ButtonFont));
                model.Value = fontstr;
                BLLConfig.Instance.UpdateConfigValueFromInterface(connect, model);
            }

            if (width != "")
            {
                model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(eConfigCode.ButtonWidth));
                model.Value = width;
                BLLConfig.Instance.UpdateConfigValueFromInterface(connect, model);
            }

            if (heigth != "")
            {
                model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(eConfigCode.ButtonHeight));
                model.Value = heigth;
                BLLConfig.Instance.UpdateConfigValueFromInterface(connect, model);
            }

            if (space != "")
            {
                model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(eConfigCode.ButtonSpace));
                model.Value = space;
                BLLConfig.Instance.UpdateConfigValueFromInterface(connect, model);
            }

            //if (MessageBox.Show("Điều chỉnh này chỉ có tác dụng khi bạn khởi động lại chương trình", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            //    this.Close();

            for (int i = 0; i < frm.Controls.Count;i++ )
            {
                if (frm.Controls[i] is Button && frm.Controls[i].Name.StartsWith(eConfigCode.ButtonName))
                {
                    frm.Controls.RemoveAt(i);
                    i--;
                }
            }
            frm.frmIssueTicketScreen_Load(sender, e);
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpDownButtonWidth_ValueChanged(object sender, EventArgs e)
        {
            btnSampleButton.Width = int.Parse(UpDownButtonWidth.Value.ToString());
        }

        private void UpDownButtonHeight_ValueChanged(object sender, EventArgs e)
        {
            btnSampleButton.Height = int.Parse(UpDownButtonHeight.Value.ToString());
        }
    }
}
