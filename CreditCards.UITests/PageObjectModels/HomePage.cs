using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace CreditCards.UITests.PageObjectModels
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private const string PageUrl = "http://localhost:44108/";
        private const string PageTitle = "Home Page - Credit Cards";

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string GenerationToken => _driver.FindElement(By.Id("GenerationToken")).Text;

        public bool IsCookieMessagePresent => _driver.FindElements(By.Id("CookiesBeingUsed")).Any();

        public void ClickContactFooterLink() => _driver.FindElement(By.Id("ContactFooter")).Click();

        public void ClickLiveChatFooterLink() => _driver.FindElement(By.Id("LiveChat")).Click();

        public void ClickLearnAboutUsLink() => _driver.FindElement(By.Id("LearnAboutUs")).Click();

        public ReadOnlyCollection<(string name, string interestRate)> Products
        {
            get
            {
                var products = new List<(string name, string interestRate)>();

                var productCells = _driver.FindElements(By.TagName("td"));

                for (var i = 0; i < productCells.Count - 1; i+= 2)
                {
                    var name = productCells[i].Text;

                    var interestRate = productCells[i + 1].Text;

                    products.Add((name, interestRate));
                }

                return products.AsReadOnly();
            }
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
