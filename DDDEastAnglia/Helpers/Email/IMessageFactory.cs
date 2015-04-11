using System.Net.Mail;
using MailMessage = DDDEastAnglia.Helpers.Email.SendGrid.MailMessage;

namespace DDDEastAnglia.Helpers.Email
{
    public interface IMessageFactory
    {
        MailMessage Create(MailAddress from, MailAddress to, string subject, string htmlContent, string textContent);
    }
}