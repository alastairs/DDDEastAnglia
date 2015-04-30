using System;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Services.Messenger.Email;

namespace DDDEastAnglia.Controllers
{
    public class EmailMessengerFactory
    {
        private readonly IPostman _postman;

        public EmailMessengerFactory(IPostman postman)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            _postman = postman;
        }

        public EmailMessenger CreateEmailMessenger(IMailTemplate mailTemplate)
        {
            return new EmailMessenger(_postman, mailTemplate);
        }
    }
}