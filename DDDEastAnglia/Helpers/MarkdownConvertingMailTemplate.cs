using System;
using System.Collections.Generic;
using MarkdownSharp;

namespace DDDEastAnglia.Helpers
{
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
}