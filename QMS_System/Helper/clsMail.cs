using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMS_System.Helper
{
    internal class clsMail
    {
        private string strType = "Google";
        private string strHost = "smtp.gmail.com";
        private int intPort = 25;
        private string strFrom = "gpro.alert2016@gmail.com";
        private string strDisplayName = "gproteam";
        private string strPassword = "A123456!@#b";
        private string strTo = "trivodai@yahoo.com,gpro.alert2016@gmail.com";
        private string strSubject = "GPRO - QMS";
        private string strBody = "Báo cáo hệ thống QMS";
        private ArrayList alAttachments;
        public string Type
        {
            get
            {
                return this.strType;
            }
            set
            {
                this.strType = value;
            }
        }
        public string Host
        {
            get
            {
                return this.strHost;
            }
            set
            {
                this.strHost = value;
            }
        }
        public int Port
        {
            get
            {
                return this.intPort;
            }
            set
            {
                this.intPort = value;
            }
        }
        public string From
        {
            get
            {
                return this.strFrom;
            }
            set
            {
                this.strFrom = value;
            }
        }
        public string DisplayName
        {
            get
            {
                return this.strDisplayName;
            }
            set
            {
                this.strDisplayName = value;
            }
        }
        public string Password
        {
            get
            {
                return this.strPassword;
            }
            set
            {
                this.strPassword = value;
            }
        }
        public string To
        {
            get
            {
                return this.strTo;
            }
            set
            {
                this.strTo = value;
            }
        }
        public string Subject
        {
            get
            {
                return this.strSubject;
            }
            set
            {
                this.strSubject = value;
            }
        }
        public string Body
        {
            get
            {
                return this.strBody;
            }
            set
            {
                this.strBody = value;
            }
        }
        public clsMail()
        {
            this.alAttachments = new ArrayList();
        }
        public void AddAttachment(string fName)
        {
            this.alAttachments.Add(fName);
        }
        public bool SendMail()
        {
            bool result;
            try
            {
                if (!(this.strType == "Outlook"))
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.To.Add(this.To);
                    mailMessage.From = new MailAddress(this.strFrom, this.strDisplayName, Encoding.UTF8);
                    mailMessage.Subject = this.strSubject;
                    mailMessage.SubjectEncoding = Encoding.UTF8;
                    mailMessage.Body = this.strBody;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Priority = MailPriority.High;
                    for (int i = 0; i <= this.alAttachments.Count - 1; i++)
                    {
                        mailMessage.Attachments.Add(new Attachment(this.alAttachments[i].ToString()));
                    }
                    if (this.strType == "Yahoo" || this.strType == "AOL")
                    {
                        new SmtpClient
                        {
                            Credentials = new NetworkCredential(this.strFrom, this.strPassword),
                            Port = this.intPort,
                            Host = this.strHost,
                            EnableSsl = false
                        }.Send(mailMessage);
                    }
                    else
                    {
                        if (this.strType == "Google" || this.strType == "Hotmail")
                        {
                            new SmtpClient
                            {
                                Credentials = new NetworkCredential(this.strFrom, this.strPassword),
                                Port = this.intPort,
                                Host = this.strHost,
                                EnableSsl = true
                            }.Send(mailMessage);
                        }
                        else
                        {
                            SmtpClient smtpClient = new SmtpClient();
                            smtpClient.Credentials = new NetworkCredential(this.strFrom, this.strPassword);
                            smtpClient.Port = this.intPort;
                            smtpClient.Host = this.strHost;
                            smtpClient.EnableSsl = true;
                            smtpClient.EnableSsl = true;
                            ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
                            smtpClient.Send(mailMessage);
                        }
                    }
                    mailMessage.Dispose();
                }
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMail");
                result = false;
            }
            return result;
        }
    }
}
