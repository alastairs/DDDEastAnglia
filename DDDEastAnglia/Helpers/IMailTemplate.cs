using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public interface IMailTemplate
    {
        void AddTokenSubstitution(string token, string substitution);
        string Render(IDictionary<string, string> replacements);
    }
}