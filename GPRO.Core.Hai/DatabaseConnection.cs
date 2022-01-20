using System;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace GPRO.Core.Hai
{
    public class DatabaseConnection
    {
        public SqlConnection con = null;
        #region constructor
        static object key = new object();
        private static volatile DatabaseConnection _Instance;
        public static DatabaseConnection Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new DatabaseConnection();

                return _Instance;
            }
        }
        private DatabaseConnection() { }
        #endregion


        public string GetConnectionString(string configFilePath)
        {
            string connectionString = string.Empty;
            try
            {
                if (File.Exists(configFilePath))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(configFilePath);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SQLServer");
                    string server = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[0].InnerText);
                    string data = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[1].InnerText);
                    string user = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[2].InnerText);
                    string pass = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[3].InnerText);
                    string windowAuthen = elementsByTagName.Item(0).ChildNodes[4].InnerText;
                    if (Boolean.Parse(windowAuthen))
                    {
                        connectionString = "Data Source=" + server + ";Initial Catalog=" + data + ";Integrated Security=True;";
                    }
                    else
                        connectionString = "Data Source=" + server + ";Initial Catalog=" + data + "; User Id=" + user + ";Password=" + pass + ";";
                }
            }
            catch (Exception ex) { }
            return connectionString;
        }

        public SqlConnection Connect(string configFilePath)
        {
            try
            {
                con = new SqlConnection(GetConnectionString(configFilePath));
                con.Open();
            }
            catch (Exception e)
            { }
            return con;
        }

        public string ConnectStringOfSQLConnection(string configFilePath)
        {
            return (GetConnectionString(configFilePath));
        }

    }
}
