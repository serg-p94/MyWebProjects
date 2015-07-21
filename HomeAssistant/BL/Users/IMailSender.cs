using System.Net.Mail;

namespace BL.Users
{
    public interface IMailSender
    {
        void Send(MailMessage msg);
        void Send(string to, string subject, string body);

        void SendAsync(MailMessage msg);
        void SendAsync(string to, string subject, string body);
    }
}
