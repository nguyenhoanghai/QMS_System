using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace QMS_System.Helper
{
    public class GPRO_Helper
    {
        #region constructor
        static object key = new object();
        private static volatile GPRO_Helper _Instance;  //volatile =>  tranh dung thread
        public static GPRO_Helper Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new GPRO_Helper();

                return _Instance;
            }
        }
        private GPRO_Helper() { }
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

        public bool CONNECT_STATUS()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(GetStringConnect());
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

        public string GetStringConnect()
        {
            try
            {
                string filename = Application.StartupPath + "\\DATA.XML";
                if (File.Exists(filename))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filename);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SQLServer");
                    string innerText = elementsByTagName.Item(0).ChildNodes[0].InnerText;
                    string innerText2 = elementsByTagName.Item(0).ChildNodes[1].InnerText;
                    string innerText3 = elementsByTagName.Item(0).ChildNodes[2].InnerText;
                    string innerText4 = elementsByTagName.Item(0).ChildNodes[3].InnerText;
                    string innerText5 = elementsByTagName.Item(0).ChildNodes[4].InnerText;
                    if (Boolean.Parse(innerText5))
                        return string.Concat(new string[]
                        {
                    "Server=",
                    innerText,
                    ";Database=",
                    innerText2,
                    ";uid=",
                    innerText3,
                    ";pwd=",
                    innerText4
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

        public string GetEntityConnectString()
        {
            try
            {
                string filename = Application.StartupPath + "\\DATA.XML";
                if (File.Exists(filename))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filename);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SQLServer");
                    string innerText = elementsByTagName.Item(0).ChildNodes[0].InnerText;
                    string innerText2 = elementsByTagName.Item(0).ChildNodes[1].InnerText;
                    string innerText3 = elementsByTagName.Item(0).ChildNodes[2].InnerText;
                    string innerText4 = elementsByTagName.Item(0).ChildNodes[3].InnerText;
                    string innerText5 = elementsByTagName.Item(0).ChildNodes[4].InnerText;
                    //Build an Entity Framework connection string
                    var entityString = new EntityConnectionStringBuilder()
                    {
                        Provider = "System.Data.SqlClient",
                        Metadata = "res://*/QMSModel.csdl|res://*/QMSModel.ssdl|res://*/QMSModel.msl"
                    };
                    if (!Boolean.Parse(innerText5))
                    {
                        entityString.ProviderConnectionString = @"data source=" + innerText + ";initial catalog=" + innerText2 + ";user id=" + innerText3 + ";password=" + innerText4;
                    }
                    else
                    {
                        entityString.ProviderConnectionString = @"data source=" + innerText + ";initial catalog=" + innerText2 + ";integrated security=True;";
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
