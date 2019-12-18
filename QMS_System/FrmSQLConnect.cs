using GPRO.Core.Hai;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace QMS_System
{
    public partial class FrmSQLConnect : Form
    {
        string conString = "";
        public FrmSQLConnect()
        {
            InitializeComponent();
        }

        private void FrmSQLConnect_Load(object sender, EventArgs e)
        {
            try
            {
                string info = BaseCore.Instance.GetStringConnectInfo(Application.StartupPath + "\\DATA.XML");
                if (!string.IsNullOrEmpty(info))
                {
                    var infos = info.Split(',');
                    txtServerName.Text = infos[0];
                    cbDatabases.Text = infos[1];
                    txtLogin.Text = infos[2];
                    txtPass.Text = infos[3];
                    chkIsAuthen.Checked = bool.Parse(infos[4]);
                    chkIsAuthen_CheckedChanged(sender, e);
                }
            }
            catch (Exception)
            { }
        }

        private void chkIsAuthen_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsAuthen.Checked)
            {
                txtLogin.Enabled = false;
                txtPass.Enabled = false;
            }
            else
            {
                txtLogin.Enabled = true;
                txtPass.Enabled = true;
            }
        }

        private void getDatabases()
        {
            if (checkValid())
            {
                try
                {
                    var conn = new SqlConnection(conString);
                    conn.Open();
                    var ds = new DataSet();
                    string query = "select name from sysdatabases";
                    var da = new SqlDataAdapter(query, conn);
                    da.Fill(ds, "databasenames");
                    this.cbDatabases.DataSource = ds.Tables["databasenames"];
                    this.cbDatabases.DisplayMember = "name";
                }
                catch
                {
                    this.cbDatabases.DataSource = null;
                }
            }

        }

        private bool checkValid()
        {
            bool isPass = false;
            // Open connection to the database 
            if (!string.IsNullOrEmpty(txtServerName.Text))
            {
                if (chkIsAuthen.Checked)
                {
                    conString = string.Concat(new string[]
                     {
                    "Server = ",
                    this.txtServerName.Text,
                    ";Trusted_Connection=true;",
                     });
                    isPass = true;
                }
                else
                {
                    conString = string.Concat(new string[]
                    {
                    "Server = ",
                    this.txtServerName.Text,
                    " ; Uid = ",
                    this.txtLogin.Text,
                    " ;Pwd= ",
                    this.txtPass.Text
                    });
                    if (!string.IsNullOrEmpty(txtLogin.Text) &&
                        !string.IsNullOrEmpty(txtPass.Text))
                    {
                        isPass = true;
                    }
                }
            }
            return isPass;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getDatabases();
        }

        private void cbDatabases_Enter(object sender, EventArgs e)
        {
            getDatabases();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (checkValid())
                CreateNewXMLFile();

        }
        private void CreateNewXMLFile()
        {
            string filename = Application.StartupPath + "\\DATA.XML";
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(newChild);
            XmlNode xmlNode = xmlDocument.CreateElement("String_Connect");
            xmlDocument.AppendChild(xmlNode);

            XmlNode xmlNode2 = xmlDocument.CreateElement("SQLServer");
            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("id");
            xmlAttribute.Value = "1";
            xmlNode2.Attributes.Append(xmlAttribute);
            xmlNode.AppendChild(xmlNode2);

            XmlNode xmlNode3 = xmlDocument.CreateElement("Server_Name");
            xmlNode3.AppendChild(xmlDocument.CreateTextNode(EncryptionHelper.Instance.Encrypt(txtServerName.Text)));
            xmlNode2.AppendChild(xmlNode3);

            XmlNode xmlNode4 = xmlDocument.CreateElement("Database");
            xmlNode4.AppendChild(xmlDocument.CreateTextNode(EncryptionHelper.Instance.Encrypt(cbDatabases.Text)));
            xmlNode2.AppendChild(xmlNode4);

            XmlNode xmlNode5 = xmlDocument.CreateElement("User");
            xmlNode5.AppendChild(xmlDocument.CreateTextNode(EncryptionHelper.Instance.Encrypt(txtLogin.Text)));
            xmlNode2.AppendChild(xmlNode5);

            XmlNode xmlNode6 = xmlDocument.CreateElement("Password");
            xmlNode6.AppendChild(xmlDocument.CreateTextNode(EncryptionHelper.Instance.Encrypt(txtPass.Text)));
            xmlNode2.AppendChild(xmlNode6);

            XmlNode xmlNode7 = xmlDocument.CreateElement("Window_Authenticate");
            xmlNode7.AppendChild(xmlDocument.CreateTextNode(chkIsAuthen.Checked.ToString()));
            xmlNode2.AppendChild(xmlNode7);

            xmlDocument.Save(filename);

            Application.Restart();
            Environment.Exit(0);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
