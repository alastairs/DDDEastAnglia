﻿using DDDEastAnglia.Models;
using DDDEastAnglia.Services.Messenger.Email.Templates;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Services.Messengers.Email.Templates
{
    [TestFixture]
    public class SessionCreatedMailTemplateFacts
    {
        [TestFixture]
        public class The_Template_Should
        {
            [Test]
            public void Substitute_The_Session_Title()
            {
                var template = SessionCreatedMailTemplate.Create(@"Services\Messengers\Email\Templates\SessionCreated.txt", new Session { Title = "My awesome session" });

                var renderedTemplate = template.Render();

                Assert.That(renderedTemplate, Is.Not.StringContaining("[SessionTitle]"));
                Assert.That(renderedTemplate, Contains.Substring("My awesome session"));
            }
        }
    }
}
