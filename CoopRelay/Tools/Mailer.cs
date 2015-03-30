using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Tools
{
    public class Mailer : IDisposable
    {
        private System.Net.Mail.SmtpClient c;
        private System.Net.Mail.MailMessage m;

        private String To
        {
            set
            {
                m.To.Clear();
                var tos = value.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var to in tos)
                {
                    m.To.Add(to);
                }
            }
        }

        private String From
        {
            set
            {
                m.From = new System.Net.Mail.MailAddress(value);
            }
        }

        public String Attachment
        {
            set
            {
                m.Attachments.Add(new System.Net.Mail.Attachment(value));
            }
        }

        private String Subject
        {
            set
            {
                m.Subject = value;
            }
        }

        private String Body
        {
            set
            {
                m.Body = value;
            }
        }

        private Boolean IsHtml
        {
            set
            {
                m.IsBodyHtml = value;
            }
        }

        public Mailer(string host, int port, bool ssl, string user, string password, string to, string from, string subject, string body, bool ishtml)
        {
            Initialize(host, port, ssl, user, password);
            To = to;
            From = from;
            Subject = subject;
            Body = body;
            IsHtml = ishtml;
        }

        private void Initialize(string host, int port, bool ssl, string user, string password)
        {
            c = new System.Net.Mail.SmtpClient(host, port);
            c.EnableSsl = ssl;
            c.Credentials = new System.Net.NetworkCredential(user, password);
            m = new System.Net.Mail.MailMessage();
        }

        public void Send()
        {
            c.Send(m);
        }

        public void Dispose()
        {
            m.Dispose();
            c.Dispose();
        }
    }
}
