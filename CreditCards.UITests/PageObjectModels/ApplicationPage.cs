using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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

        public IEnumerable<string> ValidationErrors() => _driver
            .FindElements(By.CssSelector(".validation-summary-errors > ul > li")).Select(e => e.Text);

        public void ClickTermsAccepted() => _driver.FindElement(By.Id("TermsAccepted")).Click();

        public ApplicationCompletePage SubmitFormulaire()
        {
            _driver.FindElement(By.Id("SubmitApplication")).Click();
            return new ApplicationCompletePage(_driver);
        } 

        public void EnterFirstName(string firstName = "Sarah") 
            => SendKeys("FirstName" , firstName);

        public void EnterLastName(string lastName = "Smith") 
            => SendKeys("LastName", lastName);

        public void EnterFrequentFlyerNumber(string frequentFlyerNumber = "123456-A") 
            => SendKeys("FrequentFlyerNumber", frequentFlyerNumber);

        public void EnterAge(string age = "18")
            => SendKeys("Age", age);

        public void EnterGrossAnnualIncome(string grossAnnualIncome = "50000") 
            => SendKeys("GrossAnnualIncome", grossAnnualIncome);

        public void ChooseGenderRadio(string genderId = "Single") => _driver.FindElement(By.Id(genderId)).Click();

        public void ChooseBusinessSource(string businessSourceValue = "TV")
        {
            var businessSourceElement = _driver.FindElement(By.Id("BusinessSource"));
            var businessSource = new SelectElement(businessSourceElement);

            businessSource.SelectByValue(businessSourceValue);
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

        private void SendKeys(string id, string value)
        {
            var element = _driver.FindElement(By.Id(id));

            element.Clear();
            element.SendKeys(value);
        }
    }
}
