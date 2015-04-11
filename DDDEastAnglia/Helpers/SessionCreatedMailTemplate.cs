using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    internal class SessionCreatedMailTemplate : TokenSubstitutingMailTemplate
    {
        private const string SessionAbstractToken = "[SessionAbstract]";
        private const string SessionTitleToken = "[SessionTitle]";

        private SessionCreatedMailTemplate(string templatePath, IFileContentsProvider fileContentsProvider) : base(templatePath, fileContentsProvider)
        {
        }

        public static IMailTemplate Create(string templatePath, Session session)
        {
            var template = new SessionCreatedMailTemplate(templatePath, new FileContentsProvider());
            template.AddTokenSubstitution(SessionTitleToken, session.Title);
            template.AddTokenSubstitution(SessionAbstractToken, session.Abstract);

            return template;
        }
    }
}