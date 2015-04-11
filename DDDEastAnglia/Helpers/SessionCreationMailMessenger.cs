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
        private readonly IMailTemplate plainTextTemplate;

        private const string SessionAbstractToken = "[SessionAbstract]";
        private const string SessionTitleToken = "[SessionTitle]";

        public SessionCreationMailMessenger(IPostman postman, IMailTemplate plainTextTemplate)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            if (plainTextTemplate == null)
            {
                throw new ArgumentNullException("plainTextTemplate");
            }

            this.postman = postman;
            this.plainTextTemplate = plainTextTemplate;
        }

        public void Notify(UserProfile user, Session session)
        {
            var replacementTokens = new Dictionary<string, string>
            {
                {SessionAbstractToken, session.Abstract},
                {SessionTitleToken, session.Title}
            };
            MailMessage message = new MailMessage
            {
                To = new MailAddress(user.EmailAddress),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = "DDD East Anglia Session Submission: " + session.Title,
                Body = plainTextTemplate.Render(replacementTokens)
            };

            postman.Deliver(message);
        }
    }
}