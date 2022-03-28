using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace CreditCards.UITests
{
    public class JavaScriptExamples
    {
        [Fact]
        public void ClickOverlayedLink()
        {
            using (var driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("http://localhost:44108/jsoverlay.html");

                DemoHelper.Pause(1000);

                const string script = "document.getElementById('HiddenLink').click();";

                var js = (IJavaScriptExecutor)driver;
                js.ExecuteScript(script);

                Assert.Equal("https://www.pluralsight.com/", driver.Url);
            }
        }

        [Fact]
        public void ClickOverlayedLinkText()
        {
            using (var driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("http://localhost:44108/jsoverlay.html");

                DemoHelper.Pause(1000);

                const string script = "return document.getElementById('HiddenLink').innerHTML;";

                var js = (IJavaScriptExecutor)driver;
                var linktext = (string)js.ExecuteScript(script);

                Assert.Equal("Go to Pluralsight", linktext);
            }
        }
    }
}
