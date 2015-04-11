using DDDEastAnglia.Helpers.Email.SendGrid;

namespace DDDEastAnglia.Helpers.Email
{
    public interface IPostman
    {
        void Deliver(MailMessage message);
    }
}