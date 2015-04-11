using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.File;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net.Mail;
using MailMessage = DDDEastAnglia.Helpers.Email.SendGrid.MailMessage;

namespace DDDEastAnglia.Tests.Helpers.Email
{
    [TestFixture]
    public class ResetPasswordEmailSender_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedEmailSenderIsNull()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(null, messageFactory, fileContentsProvider));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedMessageFactoryIsNull()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(emailSender, null, fileContentsProvider));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedFileContentsProviderIsNull()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(emailSender, messageFactory, null));
        }

        [Test]
        public void SendAnEmailFromTheCorrectEmailAddressAndName()
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("admin@dddeastanglia.com"),
                To = new[] { new MailAddress("test@example.com") },
                Subject = "Reset Password",
                Html = string.Empty,
                Text = string.Empty
            };

            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            messageFactory.Create(new MailAddress(ResetPasswordEmailSender.FromEmailAddress, ResetPasswordEmailSender.FromEmailName),
                                    Arg.Any<MailAddress>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(mailMessage);
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            emailSender.Received().Send(mailMessage);
        }

        [Test]
        public void SendAnEmailToTheCorrectEmail()
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("admin@dddeastanglia.com"),
                To = new[] { new MailAddress("test@example.com") },
                Subject = "Reset Password",
                Html = string.Empty,
                Text = string.Empty
            };

            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            messageFactory.Create(Arg.Any<MailAddress>(), new MailAddress("test@example.com"),
                                    Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(mailMessage);
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            emailSender.Received().Send(mailMessage);
        }

        [Test]
        public void LoadTheHtmlContentsOfTheEmail_FromTheSpecifiedHtmlTemplatePath()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            fileContentsProvider.Received().GetFileContents("htmlTemplatePath");
        }

        [Test]
        public void LoadTheTextContentsOfTheEmail_FromTheSpecifiedTextTemplatePath()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            fileContentsProvider.Received().GetFileContents("textTemplatePath");
        }

        [Test]
        public void SubstituteTheResetPasswordLink_IntoTheHtmlTemplate()
        {
            const string contentTemplate = "test {0} email";
            const string resetPasswordUrl = "http://reset/Password.Url";
            string expectedContent = string.Format(contentTemplate, resetPasswordUrl);
            var mailMessage = new MailMessage
            {
                From = new MailAddress("admin@dddeastanglia.com"),
                To = new[] { new MailAddress("user@dddeastanglia.com") },
                Subject = "Reset Password",
                Html = expectedContent,
                Text = string.Empty
            };

            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            messageFactory.Create(Arg.Any<MailAddress>(), Arg.Any<MailAddress>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>()).Returns(mailMessage);
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            string content = string.Format(contentTemplate, ResetPasswordEmailSender.ResetLinkToken);
            fileContentsProvider.GetFileContents("htmlTemplatePath").ReturnsForAnyArgs(content);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", resetPasswordUrl);

            emailSender.Received()
                          .Send(mailMessage);
        }

        [Test]
        public void SubstituteTheResetPasswordLink_IntoTheTextTemplate()
        {
            const string resetPasswordUrl = "http://reset/Password.Url";
            const string contentTemplate = "test {0} email";
            string expectedContent = string.Format(contentTemplate, resetPasswordUrl);
            var mailMessage = new MailMessage
            {
                From = new MailAddress("admin@dddeastanglia.com"),
                To = new[] { new MailAddress("user@dddeastanglia.com") },
                Subject = "Reset Password",
                Html = "",
                Text = expectedContent
            };

            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            messageFactory.Create(
                Arg.Any<MailAddress>(),
                Arg.Any<MailAddress>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>()).Returns(mailMessage);
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            string content = string.Format(contentTemplate, ResetPasswordEmailSender.ResetLinkToken);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", resetPasswordUrl);

            emailSender.Received()
                          .Send(mailMessage);
        }
    }
}
