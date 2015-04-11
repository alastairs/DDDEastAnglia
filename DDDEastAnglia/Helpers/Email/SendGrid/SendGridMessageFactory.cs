using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridMessageFactory : IMessageFactory
    {
        public IMailMessage Create(MailAddress from, MailAddress to, string subject, string htmlContent, string textContent)
        {
            return new SendGridMessageWrapper
            {
                From = from,
                To = new[] { to },
                Subject = subject,
                Html = htmlContent,
                Text = textContent
            };
        }
    }
}
