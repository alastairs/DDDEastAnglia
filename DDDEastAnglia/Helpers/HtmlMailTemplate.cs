using DDDEastAnglia.Helpers.File;
using System;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    internal class HtmlMailTemplate : IMailTemplate
    {
        private readonly IMailTemplate mailTemplate;
        private readonly string htmlTemplatePath;
        private readonly IFileContentsProvider fileContentsProvider;

        public HtmlMailTemplate(IMailTemplate mailTemplate, string htmlTemplatePath, IFileContentsProvider fileContentsProvider)
        {
            if (mailTemplate == null)
            {
                throw new ArgumentNullException("mailTemplate");
            }

            if (htmlTemplatePath == null)
            {
                throw new ArgumentNullException("htmlTemplatePath");
            }

            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }

            this.mailTemplate = mailTemplate;
            this.htmlTemplatePath = htmlTemplatePath;
            this.fileContentsProvider = fileContentsProvider;
        }

        public string Render(IDictionary<string, string> replacements)
        {
            var messsageContent = mailTemplate.Render(replacements);

            string htmlFile = fileContentsProvider.GetFileContents(htmlTemplatePath);
            var htmlMessage = htmlFile.Replace("[MessageBody]", messsageContent);

            return htmlMessage;
        }
    }
}