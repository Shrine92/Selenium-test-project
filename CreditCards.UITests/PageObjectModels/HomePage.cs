using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
#pragma warning disable CS0618 // Type or member is obsolete
using static OpenQA.Selenium.Support.UI.ExpectedConditions;
#pragma warning restore CS0618 // Type or member is obsolete

namespace CreditCards.UITests.PageObjectModels
{
    public class HomePage : Page
    {
        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        protected override string PageUrl => "http://localhost:44108/";
        protected override string PageTitle => "Home Page - Credit Cards";

        public string GenerationToken => _driver.FindElement(By.Id("GenerationToken")).Text;

        public bool IsCookieMessagePresent => _driver.FindElements(By.Id("CookiesBeingUsed")).Any();

        public void ClickContactFooterLink() => _driver.FindElement(By.Id("ContactFooter")).Click();

        public void ClickLiveChatFooterLink() => _driver.FindElement(By.Id("LiveChat")).Click();

        public void ClickLearnAboutUsLink() => _driver.FindElement(By.Id("LearnAboutUs")).Click();

        public ApplicationPage ClickApplyLowRateLink()
        {
            _driver.FindElement(By.Name("ApplyLowRate")).Click();
            return new ApplicationPage(_driver);
        }

        public ApplicationPage ClickApplyNowLink()
        {
            _driver.FindElement(By.LinkText("Easy: Apply Now!")).Click();
            return new ApplicationPage(_driver);
        }

        public void WaitForEasyApplicationCarouselPage()
        {
            _driver.Manage().Window.Minimize();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(11));

            wait.Until(ElementToBeClickable(By.LinkText("Easy: Apply Now!")));
        }

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
    }
}
