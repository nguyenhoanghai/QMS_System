using GPRO.Core.Hai;
using QMS_System.Data.BLL;
using QMS_System.Data.BLL.IssueTicketScreen;
using QMS_System.Data.Enum;
using QMS_System.Helper;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QMS_System.IssueTicketScreen
{
    public partial class frmIssueTicketScreen : DevExpress.XtraEditors.XtraForm
    {
        dynamic frmain; 
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmIssueTicketScreen(dynamic _frmain)
        {
            InitializeComponent();
            frmain = _frmain;
        }
        public void frmIssueTicketScreen_Load(object sender, EventArgs e)
        {
            try
            {
                string imgPath = BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.Background);
            if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
            {
                Image img = new Bitmap(imgPath);
                this.BackgroundImage = img;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.WindowState = FormWindowState.Maximized;
            }
            }
            catch (Exception)
            { 
            } 

            GetButton(); //phải để sau cùng
            this.KeyPreview = true; // kích hoạt loạt sự kiên nhấn Keyboard trên form
        }
        private void menuSetupInterface_Click(object sender, EventArgs e)
        {
            frmSetupInterface frm = new frmSetupInterface(this);
            //frm.MdiParent = this;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }

        private void menuHideInterface_Click(object sender, EventArgs e)
        {
            if (menuStrip1.Visible == true)
            {
                menuStrip1.Visible = false;
            }
            FormState formstate = new FormState();
            formstate.FullScreen(this);
            //this.IsMdiContainer = false; //Form phai co IsMdiContainer=false thi moi bat duoc su kien MouseEvent (de doubleclick thoat FullScreen)
        }
        private void frmIssueTicketScreen_KeyDown(object sender, KeyEventArgs e)
        {
            FormState formstate = new FormState();

            switch (e.KeyCode)
            {
                case Keys.F11:
                    {
                        if (menuStrip1.Visible == true)
                        {
                            menuStrip1.Visible = false;
                        }
                        formstate.FullScreen(this);
                        //this.IsMdiContainer = false;
                        break;
                    }
                case Keys.Escape:
                    {
                        if (menuStrip1.Visible == false)
                        {
                            menuStrip1.Visible = true;
                        }
                        formstate.EscapeFullScreen(this);
                        //this.IsMdiContainer = true;
                        break;
                    }
            }
        }
        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetButton()
        {
            var list = BLLSetupInterface.Instance.GetButtonService(connect);

            if (list.Count > 0)
            {
                // chiều dài, rộng của form cha
                int width = this.ClientRectangle.Width;
                int height = this.ClientRectangle.Height;

                // Tọa độ điểm giữa màn hình
                int midX = width / 2;
                int midY = height / 2;

                int numbutton = list.Count;
                int numcol = int.Parse(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.Column));  // số cột hiển thị
                if (numcol > numbutton)
                    numcol = numbutton;
                int numrow = (numbutton % numcol == 0) ? numbutton / numcol : (numbutton / numcol) + 1;  // số dòng hiển thị

                int buttonwidth = int.Parse(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonWidth));
                int buttonheight = int.Parse(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonHeight)); ;
                int space = int.Parse(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonSpace)); ; // khoảng cách giữa 2 nút với nhau

                // Tọa độ của button đầu tiên
                int x = midX - (((numcol - 1) * space + (numcol * buttonwidth)) / 2);
                int y = midY - (((numrow - 1) * space + (numrow * buttonheight)) / 2);

                int tmpX = x;
                int tmpY = y;

                FontConverter converter = new FontConverter();
                Font font = (Font)converter.ConvertFromString(BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonFont));

                string forecolor = BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonForeColor);
                string backcolor = BLLConfig.Instance.GetConfigByCode(connect, eConfigCode.ButtonBackColor);
                int k = 0; // biến đếm chỉ số của nút tăng dần
                for (int i = 0; i < numrow; i++)  // dòng
                {
                    for (int j = 0; j < numcol; j++)  //cột
                    {
                        Button button = new Button();
                        button.Name = eConfigCode.ButtonName + "_" + list[k].Id;
                        button.Size = new Size(buttonwidth, buttonheight);
                        button.Text = list[k].Name;
                        button.Font = font;
                        button.ForeColor = ColorTranslator.FromHtml(forecolor);
                        button.BackColor = ColorTranslator.FromHtml(backcolor);
                        button.TextAlign = ContentAlignment.MiddleCenter;
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderSize = 0;
                        button.Enabled = true;
                        button.Location = new Point(tmpX + j * buttonwidth + j * space, tmpY);  // hoành độ biến thiên khi chỉ số cột tăng
                        button.Parent = this;
                        button.Click += (s, e) => ShowMessage(button.Name);

                        //button.SendToBack();
                        this.Controls.Add(button);
                        if (k < (numbutton - 1))
                            k++;
                        else
                            break;

                    }
                    tmpY = y + ((i + 1) * buttonheight + (i + 1) * space);  // tung độ biến thiên khi chỉ số hàng tăng
                }
            }
        }


        private void ShowMessage(string buttonName)
        {
            frmain.PrintNewTicket(10, int.Parse(buttonName.Split('_')[1]), 0, true, false, null, null, null, null, null, null, null, null,null,null);
        }

        private void frmIssueTicketScreen_ClientSizeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is Button && this.Controls[i].Name.StartsWith(eConfigCode.ButtonName))
                {
                    this.Controls.RemoveAt(i);
                    i--;
                }
            }
            GetButton();
            this.Refresh();
        }

    }
}
