using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO; 
using System.Text; 
using System.Xml;

namespace GPRO.Core.Hai
{
    public class BaseCore
    {
        #region constructor
        static object key = new object();
        private static volatile BaseCore _Instance;   
        public static BaseCore Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BaseCore();

                return _Instance;
            }
        }
        private BaseCore() { }
        #endregion

        public string Ascii2HexStringNull(string sInput)
        {
            string hex = "";
            try
            {
                foreach (char c in sInput)
                {
                    int tmp = c;
                    hex += String.Format("{0:X2}", (uint)System.Convert.ToUInt32(tmp.ToString())) + " ";
                }
            }
            catch (Exception)
            {
            }
            return hex.Trim();
        }

        public string HexString2Ascii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                hexString = hexString.Replace(" ", ""); // Xóa khoảng trắng
                hexString = hexString.Replace("-", ""); // Xóa gạch ngang
                for (int i = 0; i <= hexString.Length - 2; i += 2)
                {
                    sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
                }
            }
            catch (Exception)
            {
            }
            return sb.ToString();
        }

        public string XOR(string s)
        {
            string sResult = "";
            try
            {
                char[] arrayC;
                byte buffer = 0;
                arrayC = new char[s.Length];
                arrayC = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    buffer ^= System.Convert.ToByte(arrayC[i]);
                }
                sResult = Convert.ToString(buffer, 16).PadLeft(2, '0').PadRight(3, ' ');
            }
            catch (Exception)
            {
            }
            return sResult.ToUpper().Trim();
        }

        public string ConvertVN(string chucodau)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = chucodau.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(chucodau[index]);
                chucodau = chucodau.Replace(chucodau[index], ReplText[index2]);
            }
            return chucodau;
        }

        public byte[] HexStringToByteArray(string s)
        {
            try
            {
                s = s.Replace(" ", "");
                if (s.Length % 2 != 0)
                    return new byte[] { 0x00 };
                byte[] buffer = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                    buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Hash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            byte[] hashBytes;
            using (var algorithm = new System.Security.Cryptography.SHA512Managed())
            {
                hashBytes = algorithm.ComputeHash(bytes);
            }
            return Convert.ToBase64String(hashBytes);
        }

        public List<string> ChangeNumber(int number)
        {
            var abc = new List<string>();
            if (number <= 9)
            {
                abc.Add("00");
                abc.Add("0" + number);
            }
            else if (number <= 99)
            {
                abc.Add("00");
                abc.Add(number.ToString());
            }
            else if (number <= 999)
            {
                string nStr = number.ToString();
                abc.Add("0" + nStr.Substring(0, 1));
                abc.Add(nStr.Substring(1, 2));
            }
            else if (number >= 1000)
            {
                string nStr = number.ToString();
                abc.Add(nStr.Substring(0, 2));
                abc.Add(nStr.Substring(2, 2));
            }
            return abc;
        }

        /// <summary>
        /// lấy chuỗi connnect sql
        /// </summary>
        /// <param name="filename">connect sql info file path</param>
        /// <returns></returns>
        public bool CONNECT_STATUS(string filename)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(GetStringConnect(filename));
                sqlConnection.Open();
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// lấy chuỗi connnect sql
        /// </summary>
        /// <param name="filename">connect sql info file path</param>
        /// <returns></returns>
        public string GetStringConnect(string filename)
        {
            try
            {
                //string filename = Application.StartupPath + "\\DATA.XML";
                if (File.Exists(filename))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filename);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SQLServer");
                    string innerText = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[0].InnerText);
                    string databaseName = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[1].InnerText);
                    string login = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[2].InnerText);
                    string password = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[3].InnerText);
                    string windowAuthen = elementsByTagName.Item(0).ChildNodes[4].InnerText;
                    if (Boolean.Parse(windowAuthen))
                        return string.Concat(new string[]
                        {
                    "Server=",
                    innerText,
                    ";Database=",
                    databaseName,
                    ";uid=",
                    login,
                    ";pwd=",
                    password
                        });
                    else
                    {
                        return string.Concat(new string[]
                           {
                    "Server = ",
                    innerText,
                    ";Trusted_Connection=true;",
                           });
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// lấy chuỗi connnect sql
        /// </summary>
        /// <param name="filename">connect sql info file path</param>
        /// <returns></returns>
        public string GetStringConnectInfo(string filename)
        {
            try
            {
               // string filename = Application.StartupPath + "\\DATA.XML";
                if (File.Exists(filename))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filename);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SQLServer");
                    string serverName = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[0].InnerText);
                    string databaseName = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[1].InnerText);
                    string login = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[2].InnerText);
                    string password = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[3].InnerText);
                    string windowAuthen = elementsByTagName.Item(0).ChildNodes[4].InnerText;
                    return string.Format("{0},{1},{2},{3},{4}", serverName, databaseName, login, password, windowAuthen);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// lấy chuỗi connnect sql entity
        /// </summary>
        /// <param name="filename">connect sql info file path</param>
        /// <returns></returns>
        public string GetEntityConnectString(string filename)
        {
            try
            {
               // string filename = Application.StartupPath + "\\DATA.XML";
                if (File.Exists(filename))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filename);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SQLServer");
                    string serverName = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[0].InnerText);
                    string databaseName = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[1].InnerText);
                    string login = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[2].InnerText);
                    string password = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[3].InnerText);
                    string windowAuthen = elementsByTagName.Item(0).ChildNodes[4].InnerText;
                    //Build an Entity Framework connection string
                    var entityString = new EntityConnectionStringBuilder()
                    {
                        Provider = "System.Data.SqlClient",
                        Metadata = "res://*/QMSModel.csdl|res://*/QMSModel.ssdl|res://*/QMSModel.msl"
                    };
                    if (!Boolean.Parse(windowAuthen))
                    {
                        entityString.ProviderConnectionString = @"data source=" + serverName + ";initial catalog=" + databaseName + ";user id=" + login + ";password=" + password + ";MultipleActiveResultSets=true;";
                    }
                    else
                    {
                        entityString.ProviderConnectionString = @"data source=" + serverName + ";initial catalog=" + databaseName + ";integrated security=True;MultipleActiveResultSets=true;";
                    }
                    return entityString.ConnectionString;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

}
