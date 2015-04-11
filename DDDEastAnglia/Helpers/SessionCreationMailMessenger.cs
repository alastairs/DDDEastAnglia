using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using MailMessage = DDDEastAnglia.Helpers.Email.MailMessage;

namespace DDDEastAnglia.Helpers
{
    public class SessionCreationMailMessenger : IMessenger<Session>
    {
        private readonly IPostman postman;
        private readonly IMailTemplate mailTemplate;

        private const string SessionAbstractToken = "[SessionAbstract]";
        private const string SessionTitleToken = "[SessionTitle]";

        public SessionCreationMailMessenger(IPostman postman, IMailTemplate mailTemplate)
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

        public void Notify(UserProfile user, Session session)
        {
            mailTemplate.AddTokenSubstitution(SessionTitleToken, session.Title);
            mailTemplate.AddTokenSubstitution(SessionAbstractToken, session.Abstract);

            MailMessage message = new MailMessage
            {
                To = new MailAddress(user.EmailAddress),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = "DDD East Anglia Session Submission: " + session.Title,
                Body = mailTemplate.Render()
            };

            postman.Deliver(message);
        }
    }
}