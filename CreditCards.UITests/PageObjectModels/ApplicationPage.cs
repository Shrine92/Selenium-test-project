using System;
using OpenQA.Selenium;

namespace CreditCards.UITests.PageObjectModels
{
    public class ApplicationPage
    {
        private readonly IWebDriver _driver;
        private const string PageUrl = "http://localhost:44108/Apply";
        private const string PageTitle = "Credit Card Application - Credit Cards";

        public ApplicationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateTo()
        {
            _driver.Navigate().GoToUrl(PageUrl);
            EnsurePageLoaded();
        }

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
