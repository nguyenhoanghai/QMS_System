using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Configuration;
using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Enum;

namespace QMS_System.IssueTicketScreen
{
    public partial class frmSetupInterface : DevExpress.XtraEditors.XtraForm
    {
        QMSSystemEntities db;
        frmIssueTicketScreen frm;
        public frmSetupInterface()
        {
            InitializeComponent();
        }

        public frmSetupInterface(frmIssueTicketScreen _frm)
        {
            InitializeComponent();
            frm = _frm;
        }
        private void frmSetupInterface_Load(object sender, EventArgs e)
        {
         
        }

        private void btnSetBackGroundImg_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();

                dlg.Filter = "JPEG Format Picture (*.jpg) | *.jpg|All files (*.*)| *.*";
                dlg.DefaultExt = "*.jpg";
                dlg.InitialDirectory = BLLConfig.Instance.GetConfigByCode(eConfigCode.ImagePath);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(dlg.FileName))
                    {
                        db = new QMSSystemEntities();
                        Q_Config model = db.Q_Config.FirstOrDefault(x => !x.IsDeleted && x.IsActived && x.Code.Equals(eConfigCode.Background));
                        if (model != null)
                        {
                            model.Value = dlg.FileName;
                            BLLConfig.Instance.UpdateConfigValueFromInterface(model);
                        }
                        Image img = new Bitmap(dlg.FileName);
                        this.frm.BackgroundImage = img;
                        this.frm.BackgroundImageLayout = ImageLayout.Stretch;
                        this.frm.Refresh();
                    }
                }
                dlg.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi" + ex.Message);
            }
            
        }

        private void btnButtonStyle_Click(object sender, EventArgs e)
        {
            frmButtonStyle frmButtonStyle = new frmButtonStyle(this.frm);
            frmButtonStyle.StartPosition = FormStartPosition.CenterScreen;
            frmButtonStyle.Show();
        }

        private void btnNumOfColumn_Click(object sender, EventArgs e)
        {
            frmNumOfColumn frmNumOfCol = new frmNumOfColumn(this.frm);
            //frm.Owner = frmMain;
            frmNumOfCol.StartPosition = FormStartPosition.CenterScreen;
            frmNumOfCol.Show();
        }
    }
}