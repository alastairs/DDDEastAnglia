using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using MarkdownSharp;
using System;
using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email
{
    public interface ISessionSubmissionEmailSender
    {
        void SendEmail(string htmlTemplatePath, string textTemplatePath, Session session, UserProfile speakerProfile, bool updated);
    }

    public class SessionSubmissionEmailSender : ISessionSubmissionEmailSender
    {
        public const string FromEmailAddress = "admin@dddeastanglia.com";
        public const string FromEmailName = "DDD East Anglia";
        private const string sessionAbstractToken = "[SessionAbstract]";
        private const string sessionTitleToken = "[SessionTitle]";
        private const string votingOpenDateToken = "[VotingOpenDateToken]";

        private readonly IEmailSender _emailSender;
        private readonly IMessageFactory _messageFactory;
        private readonly IFileContentsProvider _fileContentsProvider;

        public SessionSubmissionEmailSender(IEmailSender emailSender, IMessageFactory messageFactory, IFileContentsProvider fileContentsProvider)
        {
            if (emailSender == null)
            {
                throw new ArgumentNullException("emailSender");
            }

            if (messageFactory == null)
            {
                throw new ArgumentNullException("messageFactory");
            }

            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }

            _emailSender = emailSender;
            _messageFactory = messageFactory;
            _fileContentsProvider = fileContentsProvider;
        }

        public void SendEmail(string htmlTemplatePath, string textTemplatePath, Session session, UserProfile speakerProfile, bool updated)
        {
            string htmlTemplate = _fileContentsProvider.GetFileContents(htmlTemplatePath);
            string textTemplate = _fileContentsProvider.GetFileContents(textTemplatePath);

            var from = new MailAddress(FromEmailAddress, FromEmailName);
            var to = new MailAddress(speakerProfile.EmailAddress, speakerProfile.Name);

            Markdown converter = new Markdown();

            var html = htmlTemplate.Replace(sessionTitleToken, session.Title).Replace(sessionAbstractToken, converter.Transform(session.Abstract));

            var text = textTemplate.Replace(sessionTitleToken, session.Title).Replace(sessionAbstractToken, session.Abstract);

            string emailSubject = updated ? "DDD East Anglia Updated Session: " + session.Title : "DDD East Anglia Session Submission: " + session.Title;
            var message = _messageFactory.Create(from, to, emailSubject, html, text);
            _emailSender.Send(message);
        }
    }
}