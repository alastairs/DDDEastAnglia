using System;
using System.Collections.Generic;
using DDDEastAnglia.Helpers.File;

namespace DDDEastAnglia.Helpers
{
    internal abstract class TokenSubstitutingMailTemplate : IMailTemplate
    {
        private readonly string templatePath;
        private readonly IFileContentsProvider fileContentsProvider;
        private readonly IDictionary<string, string> substitutions = new Dictionary<string, string>();

        protected TokenSubstitutingMailTemplate(string templatePath, IFileContentsProvider fileContentsProvider)
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

        public void AddTokenSubstitution(string token, string substitution)
        {
            substitutions[token] = substitution;
        }

        public string Render()
        {
            var content = fileContentsProvider.GetFileContents(templatePath);

            foreach (var token in substitutions)
            {
                content = content.Replace(token.Key, token.Value);
            }

            return content;
        }
    }
}