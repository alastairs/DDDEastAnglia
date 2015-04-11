using System;
using System.Net.Mail;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger.Email
{
    public class EmailMessenger : IMessenger
    {
        private readonly IPostman postman;
        private readonly IMailTemplate mailTemplate;

        public EmailMessenger(IPostman postman, IMailTemplate mailTemplate)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            if (mailTemplate == null)
            {
                throw new ArgumentNullException("mailTemplate");
            }

            this.postman = postman;
            this.mailTemplate = mailTemplate;
        }

        public void Notify(UserProfile user)
        {
            MailMessage message = new MailMessage
            {
                To = new MailAddress(user.EmailAddress, user.Name),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = mailTemplate.RenderSubjectLine(),
                Body = mailTemplate.RenderBody()
            };

            postman.Deliver(message);
        }
    }
}