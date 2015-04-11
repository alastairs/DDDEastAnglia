﻿using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.Email.SendGrid;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;

namespace DDDEastAnglia.Tests.Helpers.Email
{
    [TestFixture]
    public class SessionSubmissionMessageFactory_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedMessageFactoryIsNull()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new SessionSubmissionMessageFactory(null, fileContentsProvider));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedFileContentsProviderIsNull()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            Assert.Throws<ArgumentNullException>(() => new SessionSubmissionMessageFactory(messageFactory, null));
        }

        [Test]
        public void SendAnEmailFromTheCorrectEmailAddressAndName()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };
            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            Assert.AreEqual(@"""DDD East Anglia"" <admin@dddeastanglia.com>", result.From.ToString());
        }

        [Test]
        public void SendAnEmailToTheCorrectEmail()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };
            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            Assert.AreEqual(profile.EmailAddress, result.To.ToString());
        }

        [Test]
        public void LoadTheHtmlContentsOfTheEmail_FromTheSpecifiedHtmlTemplatePath()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(string.Empty).ReturnsForAnyArgs("file contents");

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            fileContentsProvider.Received().GetFileContents("htmlTemplatePath");
        }

        [Test]
        public void LoadTheTextContentsOfTheEmail_FromTheSpecifiedTextTemplatePath()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(string.Empty).ReturnsForAnyArgs("file contents");

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            fileContentsProvider.Received().GetFileContents("textTemplatePath");
        }

        [Test]
        public void SubstituteTheSessionAbstract_IntoTheHtmlTemplate()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("htmlTemplatePath").ReturnsForAnyArgs(content);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = string.Format(contentTemplate, "abstract");
            Assert.AreEqual(expectedContent, result.Html);
        }

        [Test]
        public void SubstituteTheSessionAbstract_IntoTheTextTemplate()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = string.Format(contentTemplate, "abstract");
            Assert.AreEqual(expectedContent, result.Text);
        }

        [Test]
        public void SendANewEmailForANewSession()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = "DDD East Anglia Session Submission: title";
            Assert.AreEqual(expectedContent, result.Subject);
        }

        [Test]
        public void SendAnUpdateEMailForAnUpdatedSession()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, true);

            string expectedContent = "DDD East Anglia Updated Session: title";
            Assert.AreEqual(expectedContent, result.Subject);
        }
    }
}
