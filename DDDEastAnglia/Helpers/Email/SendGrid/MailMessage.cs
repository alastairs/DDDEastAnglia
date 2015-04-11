using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class MailMessage
    {
        public MailAddress From { get; set; }
        public MailAddress[] To { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string Text { get; set; }
    }
}