using GPRO.Core.Hai;
using QMS_System.Data.BLL;
using QMS_System.Helper;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace QMS_System
{
    public partial class frmVideo : Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmVideo( )
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
            gridChild.DataSource = BLLVideo.Instance.Gets(connect);
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

                    string fakeName = (DateTime.Now.ToString("ddMMyyyyyHHmmss") + "" + openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf('.')));
                    File.Copy(path, (savePath+ fakeName),true ); 
                    var player = new WindowsMediaPlayer();
                    var clip = player.newMedia(path);
                    var time = TimeSpan.FromSeconds(clip.duration);
                    
                    BLLVideo.Instance.AddFile(connect,new Data.Q_Video() { FileName = openFileDialog1.SafeFileName, FakeName = fakeName ,Duration = time});
                    LoadData();
                }
                catch (Exception ex)
                {
                }
            }

        }

        private void repbtnDeleteChild_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridViewChild.GetRowCellValue(gridViewChild.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLVideo.Instance.Delete(connect,Id);
                LoadData();
            }
        }
    }
}
