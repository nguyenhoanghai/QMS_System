using QMS_System.Data.BLL;
using QMS_System.Helper;
using System;
using System.Data;
using System.Data.Entity.Core.EntityClient;
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
        private void CreateNewXMLFile( )
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
            xmlNode3.AppendChild(xmlDocument.CreateTextNode(txtServerName.Text));
            xmlNode2.AppendChild(xmlNode3);

            XmlNode xmlNode4 = xmlDocument.CreateElement("Database");
            xmlNode4.AppendChild(xmlDocument.CreateTextNode(cbDatabases.Text));
            xmlNode2.AppendChild(xmlNode4);

            XmlNode xmlNode5 = xmlDocument.CreateElement("User");
            xmlNode5.AppendChild(xmlDocument.CreateTextNode(txtLogin.Text));
            xmlNode2.AppendChild(xmlNode5);

            XmlNode xmlNode6 = xmlDocument.CreateElement("Password");
            xmlNode6.AppendChild(xmlDocument.CreateTextNode(txtPass.Text));
            xmlNode2.AppendChild(xmlNode6);

            XmlNode xmlNode7 = xmlDocument.CreateElement("Window_Authenticate");
            xmlNode7.AppendChild(xmlDocument.CreateTextNode(chkIsAuthen.Checked.ToString()));
            xmlNode2.AppendChild(xmlNode7);

            xmlDocument.Save(filename);
             


            var list = BLLMajor.Instance.Gets( GPRO_Helper.Instance.GetEntityConnectString());
         //   this.Close(); 
        }
    }
}
