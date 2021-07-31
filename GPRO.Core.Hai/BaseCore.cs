using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
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
                    if (!Boolean.Parse(windowAuthen))
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

        public void PrintTicketTCVN3(SerialPort COM, string content)
        {
            try
            {
                if (COM.IsOpen)
                {
                    dynamic newMsg = null;
                    for (int i = 0; i < content.Length; i++)
                    {
                        var kytu = content[i];
                        switch (kytu)
                        {
                            #region MyRegion 
                            case 'à':
                            case 'À':
                                newMsg = BaseCore.Instance.HexStringToByteArray("B5");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'á':
                            case 'Á':
                                newMsg = BaseCore.Instance.HexStringToByteArray("B8");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'ả':
                            case 'Ả':
                                newMsg = BaseCore.Instance.HexStringToByteArray("B6");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ã':
                            case 'ã':
                                newMsg = BaseCore.Instance.HexStringToByteArray("B7");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ạ':
                            case 'ạ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("B9");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Â':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A2");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'â':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A9");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ấ':
                            case 'ấ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("CA");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ầ':
                            case 'ầ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("C7");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ẩ':
                            case 'ẩ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("C8");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ẫ':
                            case 'ẫ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("C9");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ậ':
                            case 'ậ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("CB");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ă':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A1");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'ă':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A8");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ắ':
                            case 'ắ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("BE");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ằ':
                            case 'ằ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("BB");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ẳ':
                            case 'ẳ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("BC");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ẵ':
                            case 'ẵ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("BD");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ặ':
                            case 'ặ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("C6");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'É':
                            case 'é':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D0");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'È':
                            case 'è':
                                newMsg = BaseCore.Instance.HexStringToByteArray("CC");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ẻ':
                            case 'ẻ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("CE");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ẽ':
                            case 'ẽ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("CF");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ẹ':
                            case 'ẹ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D1");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ê':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A3");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'ê':
                                newMsg = BaseCore.Instance.HexStringToByteArray("AA");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ế':
                            case 'ế':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D5");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ề':
                            case 'ề':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D2");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ể':
                            case 'ể':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D3");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ễ':
                            case 'ễ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D4");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ệ':
                            case 'ệ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D6");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;

                            case 'Ò':
                            case 'ò':
                                newMsg = BaseCore.Instance.HexStringToByteArray("DF");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ó':
                            case 'ó':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E3");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỏ':
                            case 'ỏ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E1");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Õ':
                            case 'õ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E2");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ọ':
                            case 'ọ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E4");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ô':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A4");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'ô':
                                newMsg = BaseCore.Instance.HexStringToByteArray("AB");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ố':
                            case 'ố':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E8");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ồ':
                            case 'ồ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E5");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ổ':
                            case 'ổ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E6");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỗ':
                            case 'ỗ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E7");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ộ':
                            case 'ộ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("E9");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ơ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A5");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'ơ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("AC");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ớ':
                            case 'ớ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("ED");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ờ':
                            case 'ờ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("EA");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ở':
                            case 'ở':
                                newMsg = BaseCore.Instance.HexStringToByteArray("EB");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỡ':
                            case 'ỡ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("EC");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ợ':
                            case 'ợ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("EE");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Í':
                            case 'í':
                                newMsg = BaseCore.Instance.HexStringToByteArray("DD");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ì':
                            case 'ì':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D7");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỉ':
                            case 'ỉ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("D8");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ĩ':
                            case 'ĩ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("DC");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ị':
                            case 'ị':
                                newMsg = BaseCore.Instance.HexStringToByteArray("DE");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ú':
                            case 'ú':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F3");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ù':
                            case 'ù':
                                newMsg = BaseCore.Instance.HexStringToByteArray("EF");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ủ':
                            case 'ủ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F1");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ũ':
                            case 'ũ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F2");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ụ':
                            case 'ụ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F4");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ư':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A6");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'ư':
                                newMsg = BaseCore.Instance.HexStringToByteArray("AD");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ứ':
                            case 'ứ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F8");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ừ':
                            case 'ừ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F5");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ử':
                            case 'ử':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F6");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ữ':
                            case 'ữ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F7");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ự':
                            case 'ự':
                                newMsg = BaseCore.Instance.HexStringToByteArray("F9");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ý':
                            case 'ý':
                                newMsg = BaseCore.Instance.HexStringToByteArray("FD");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỳ':
                            case 'ỳ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("FA");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỷ':
                            case 'ỷ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("FB");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỹ':
                            case 'ỹ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("FC");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Ỵ':
                            case 'ỵ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("FE");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'Đ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("A7");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            case 'đ':
                                newMsg = BaseCore.Instance.HexStringToByteArray("AE");
                                COM.Write(newMsg, 0, newMsg.Length);
                                break;
                            default:
                                COM.Write((content[i]).ToString());
                                break;
                                #endregion
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// from 1 => 01
        /// </summary>
        /// <param name="so"></param>
        /// <returns></returns>
        public string ConvertIntToStringWith0Number(int so)
        {
            return so < 10 ? ("0" + so) : so.ToString();
        }
    }
}
