using QMS_System.Data;
using QMS_System.Data.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMS_System.Data.Enum;

namespace QMS_System.IssueTicketScreen
{
    public partial class frmNumOfColumn : Form
    {
        QMSSystemEntities db;
        frmIssueTicketScreen frm;
        string numcol = "";
        public frmNumOfColumn()
        {
            InitializeComponent();
        }
        public frmNumOfColumn(frmIssueTicketScreen _frm)
        {
            InitializeComponent();
            frm = _frm;
        }
        private void frmNumOfColumn_Load(object sender, EventArgs e)
        {
            UpDownNumOfColumn.Value = int.Parse(BLLConfig.Instance.GetConfigByCode(eConfigCode.Column));
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                numcol = UpDownNumOfColumn.Value.ToString();
                if (numcol != "")
                {
                    db = new QMSSystemEntities();
                    Q_Config model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Trim().ToUpper().Equals(eConfigCode.Column));
                    if (model != null)
                    {
                        model.Value = numcol;
                        BLLConfig.Instance.UpdateConfigValueFromInterface(model);
                        //if (MessageBox.Show("Điều chỉnh này chỉ có tác dụng khi bạn khởi động lại chương trình", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        //    this.Close();

                        for (int i = 0; i < frm.Controls.Count; i++)
                        {
                            if (frm.Controls[i] is Button && frm.Controls[i].Name.StartsWith(eConfigCode.ButtonName))
                            {
                                frm.Controls.RemoveAt(i);
                                i--;
                            }
                        }
                        frm.frmIssueTicketScreen_Load(sender, e);
                    }
                }
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
