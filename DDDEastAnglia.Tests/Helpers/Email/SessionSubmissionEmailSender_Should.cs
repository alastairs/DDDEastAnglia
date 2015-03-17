using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net.Mail;

namespace DDDEastAnglia.Tests.Helpers.Email
{
    [TestFixture]
    public class SessionSubmissionEmailSender_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedEmailSenderIsNull()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new SessionSubmissionEmailSender(null, messageFactory, fileContentsProvider));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedMessageFactoryIsNull()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new SessionSubmissionEmailSender(emailSender, null, fileContentsProvider));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedFileContentsProviderIsNull()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            Assert.Throws<ArgumentNullException>(() => new SessionSubmissionEmailSender(emailSender, messageFactory, null));
        }

        [Test]
        public void SendAnEmailFromTheCorrectEmailAddressAndName()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };
            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, false);

            messageFactory.Received()
                          .Create(new MailAddress(SessionSubmissionEmailSender.FromEmailAddress, SessionSubmissionEmailSender.FromEmailName),
                                    Arg.Any<MailAddress>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void SendAnEmailToTheCorrectEmail()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };
            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, false);

            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), new MailAddress("speaker@dddeastanglia.com"),
                                    Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void LoadTheHtmlContentsOfTheEmail_FromTheSpecifiedHtmlTemplatePath()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, false);

            fileContentsProvider.Received().GetFileContents("htmlTemplatePath");
        }

        [Test]
        public void LoadTheTextContentsOfTheEmail_FromTheSpecifiedTextTemplatePath()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, false);

            fileContentsProvider.Received().GetFileContents("textTemplatePath");
        }

        [Test]
        public void SubstituteTheSessionAbstract_IntoTheHtmlTemplate()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("htmlTemplatePath").ReturnsForAnyArgs(content);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = string.Format(contentTemplate, "abstract");
            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), Arg.Any<MailAddress>(),
                                    Arg.Any<string>(), expectedContent, Arg.Any<string>());
        }

        [Test]
        public void SubstituteTheSessionAbstract_IntoTheTextTemplate()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = string.Format(contentTemplate, "abstract");
            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), Arg.Any<MailAddress>(),
                                    Arg.Any<string>(), Arg.Any<string>(), expectedContent);
        }

        [Test]
        public void SendANewEmailForANewSession()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = "DDD East Anglia Session Submission: title";
            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), Arg.Any<MailAddress>(),
                                    expectedContent, Arg.Any<string>(), Arg.Any<string>());

        }

        [Test]
        public void SendAnUpdateEMailForAnUpdatedSession()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            var sender = new SessionSubmissionEmailSender(emailSender, messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", session, profile, true);

            string expectedContent = "DDD East Anglia Updated Session: title";
            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), Arg.Any<MailAddress>(),
                                    expectedContent, Arg.Any<string>(),Arg.Any<string>() );

        }
    }
}
