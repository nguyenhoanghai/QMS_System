using QMS_System.Data.BLL;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmVideo : Form
    {
        public frmVideo()
        {
            InitializeComponent();
        }

        private void frmVideo_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            gridChild.DataSource = null;
            gridChild.DataSource = BLLVideo.Instance.Gets();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // file types, that will be allowed to upload
            openFileDialog1.Filter = "Video files | *.mp4";
            // allow/deny user to upload more than one file at a time
            openFileDialog1.Multiselect = false;
            // if user clicked OK
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = openFileDialog1.FileName; // get name of file
                    var savePath = ConfigurationManager.AppSettings["SaveVideoRoot"];
                    if (!Directory.Exists(savePath))
                        Directory.CreateDirectory(savePath);

                    string fakeName = (DateTime.Now.ToString("ddMMyyyyyHHmmss") + "_" + openFileDialog1.SafeFileName);
                    File.Copy(path,  fakeName,true );
                    BLLVideo.Instance.AddFile(new Data.Q_Video() { FileName = openFileDialog1.SafeFileName, FakeName = fakeName });
                    LoadData();
                }
                catch (Exception ex)
                {
                }
            }

        }

    }
}
