using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridMessageFactory : IMessageFactory
    {
        public MailMessage Create(MailAddress from, MailAddress to, string subject, string htmlContent, string textContent)
        {
            return new MailMessage
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
