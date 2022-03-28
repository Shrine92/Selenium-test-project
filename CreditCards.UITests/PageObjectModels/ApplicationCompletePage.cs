using System;
using OpenQA.Selenium;

namespace CreditCards.UITests.PageObjectModels
{
    public class ApplicationCompletePage
    {
        private readonly IWebDriver _driver;
        private const string PageUrl = "http://localhost:44108/Apply";
        private const string PageTitle = "Application Complete - Credit Cards";

        public ApplicationCompletePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string Decision => _driver.FindElement(By.Id("Decision")).Text;
        public string ReferenceNumber => _driver.FindElement(By.Id("ReferenceNumber")).Text;
        public string FullName => _driver.FindElement(By.Id("FullName")).Text;
        public string Age => _driver.FindElement(By.Id("Age")).Text;
        public string Income => _driver.FindElement(By.Id("Income")).Text;
        public string RelationshipStatus => _driver.FindElement(By.Id("RelationshipStatus")).Text;
        public string BusinessSource => _driver.FindElement(By.Id("BusinessSource")).Text;



        public void EnsurePageLoaded(bool onlyCheckUrlStartsWith = true)
        {
            var urlIsCorrect = onlyCheckUrlStartsWith ? _driver.Url.StartsWith(PageUrl) : _driver.Url == PageUrl;
            var pageHasLoaded = urlIsCorrect && _driver.Title == PageTitle;

            if (!pageHasLoaded)
            {
                throw new Exception(
                    $"The page is not loaded. Page URL = '{_driver.Url}' Page Source: \r\n {_driver.PageSource}");
            }
        }
    }
}
