using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using MailMessage = DDDEastAnglia.Helpers.Email.MailMessage;

namespace DDDEastAnglia.Helpers
{
    public interface IMailTemplate
    {
        string Render(IDictionary<string, string> replacements);
    }

    internal class TokenSubstitutingMailTemplate : IMailTemplate
    {
        private readonly string templatePath;
        private readonly IFileContentsProvider fileContentsProvider;

        public TokenSubstitutingMailTemplate(string templatePath, IFileContentsProvider fileContentsProvider)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                throw new ArgumentNullException("templatePath");
            }

            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }

            this.templatePath = templatePath;
            this.fileContentsProvider = fileContentsProvider;
        }

        public string Render(IDictionary<string, string> replacements)
        {
            var content = fileContentsProvider.GetFileContents(templatePath);

            foreach (var token in replacements)
            {
                content = content.Replace(token.Key, token.Value);
            }

            return content;
        }
    }

    internal class MarkdownConvertingMailTemplate : IMailTemplate
    {
        private readonly IMailTemplate mailTemplate;

        public MarkdownConvertingMailTemplate(IMailTemplate mailTemplate)
        {
            if (mailTemplate == null)
            {
                throw new ArgumentNullException("mailTemplate");
            }

            this.mailTemplate = mailTemplate;
        }

        public string Render(IDictionary<string, string> replacements)
        {
            string markdownContent = mailTemplate.Render(replacements);
            return new Markdown().Transform(markdownContent);
        }
    }

    public class SessionCreationMailMessenger : IMessenger<Session>
    {
        private readonly IPostman postman;
        private readonly IMailTemplate plainTextTemplate;
        private readonly IMailTemplate htmlTemplate;

        private const string htmlTemplatePath =
            @"C:\Users\ukpxp003\Documents\Visual Studio 2012\Projects\DDDEastAnglia\DDDEastAnglia\SessionSubmissionTemplate.html";

        private const string textTemplatePath =
            @"C:\Users\ukpxp003\Documents\Visual Studio 2012\Projects\DDDEastAnglia\DDDEastAnglia\SessionSubmissionTemplate.txt";

        private const string sessionAbstractToken = "[SessionAbstract]";

        private const string sessionTitleToken = "[SessionTitle]";

        public SessionCreationMailMessenger(IPostman postman, IMailTemplate plainTextTemplate, IMailTemplate htmlTemplate)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            if (plainTextTemplate == null)
            {
                throw new ArgumentNullException("plainTextTemplate");
            }

            if (htmlTemplate == null)
            {
                throw new ArgumentNullException("htmlTemplate");
            }

            this.postman = postman;
            this.plainTextTemplate = plainTextTemplate;
            this.htmlTemplate = htmlTemplate;
        }

        public void Notify(UserProfile user, Session session)
        {
            var replacementTokens = new Dictionary<string, string>
            {
                {sessionAbstractToken, session.Abstract},
                {sessionTitleToken, session.Title}
            };
            MailMessage message = new MailMessage
            {
                To = new MailAddress(user.EmailAddress),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = "DDD East Anglia Session Submission: " + session.Title,
                Html = htmlTemplate.Render(replacementTokens),
                Text = plainTextTemplate.Render(replacementTokens)
            };

            postman.Deliver(message);
        }
    }
}