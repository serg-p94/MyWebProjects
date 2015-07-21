using System.Net;
using System.Net.Mail;

namespace BL.Users
{
    public class MailSender : IMailSender
    {
        private readonly string _from;
        private readonly SmtpClient _client;

        public MailSender(string from, string password)
        {
            _from = from;
            _client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(@from, password)
            };
        }

        public void Send(string to, string subject, string body)
        {
            var msg = new MailMessage(_from, to)
            {
                Subject = subject,
                Body = body
            };
            Send(msg);
        }

        public void Send(MailMessage msg)
        {
            _client.Send(msg);
        }

        public void SendAsync(MailMessage msg)
        {
            _client.SendAsync(msg, null);
        }

        public void SendAsync(string to, string subject, string body)
        {
            var msg = new MailMessage(_from, to)
            {
                Subject = subject,
                Body = body
            };
            SendAsync(msg);
        }
    }
}