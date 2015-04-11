using System;
using DDDEastAnglia.Helpers.Email.SendGrid;
using MarkdownSharp;

namespace DDDEastAnglia.Helpers
{
    internal class HtmlRenderer : IRenderer
    {
        private readonly string htmlTemplate;

        public HtmlRenderer(string htmlTemplate)
        {
            if (htmlTemplate == null)
            {
                throw new ArgumentNullException("htmlTemplate");
            }

            this.htmlTemplate = htmlTemplate;
        }

        public string Render(string content)
        {
            var renderedContent = new Markdown().Transform(content);
            var htmlMessage = htmlTemplate.Replace("[MessageBody]", renderedContent);

            return htmlMessage;
        }
    }
}
