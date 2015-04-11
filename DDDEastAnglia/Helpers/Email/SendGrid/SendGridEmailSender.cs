﻿using SendGridMail.Transport;
using System;
using System.Net;
using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IMailHostSettingsProvider hostSettingsProvider;

        public SendGridEmailSender(IMailHostSettingsProvider hostSettingsProvider)
        {
            if (hostSettingsProvider == null)
            {
                throw new ArgumentNullException("hostSettingsProvider");
            }

            this.hostSettingsProvider = hostSettingsProvider;
        }

        public void Send(IMailMessage message)
        {
            var hostSettings = hostSettingsProvider.GetSettings();
            var credentials = new NetworkCredential(hostSettings.Username, hostSettings.Password);
            SMTP instance = SMTP.GetInstance(credentials, hostSettings.Host, hostSettings.Port);

            var sendGrid = SendGridMail.SendGrid.GetInstance(
                message.From,
                message.To,
                new MailAddress[0],
                new MailAddress[0],
                message.Subject,
                message.Html,
                message.Text);
            instance.Deliver(sendGrid);
        }
    }
}
