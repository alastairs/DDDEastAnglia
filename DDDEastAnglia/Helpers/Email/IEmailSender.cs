using DDDEastAnglia.Helpers.Email.SendGrid;

namespace DDDEastAnglia.Helpers.Email
{
    public interface IEmailSender
    {
        void Deliver(MailMessage message);
    }
}