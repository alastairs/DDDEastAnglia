using DDDEastAnglia.Helpers.Email.SendGrid;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email.SendGrid
{
    [TestFixture]
    public class HtmlRendererFacts
    {
        [TestFixture]
        public class Render_Should
        {
            [Test]
            public void Convert_Markdown_To_HTML()
            {
                var sut = CreateSut();

                var renderedHtml = sut.Render("*italicised text*");

                Assert.That(renderedHtml, Contains.Substring("<p><em>italicised text</em></p>"));
            }

            private static HtmlRenderer CreateSut()
            {
                return new HtmlRenderer("<html><body>[MessageBody]</body></html>");
            }
        }
    }
}
