using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Mail;

namespace MvcCalculator.Helper
{
    public class EmailManager
    {
        private string from;
        private SmtpClient client;
        public EmailManager(string from, string password)
        {
            this.from = from;
            client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(from, password);
        }

        public void Send(string to, string subject, string body)
        {
            var msg = new MailMessage(from, to);
            msg.Subject = subject;
            msg.Body = body;
            Send(msg);
        }

        public void Send(MailMessage msg)
        {
            client.Send(msg);
        }
    }
}