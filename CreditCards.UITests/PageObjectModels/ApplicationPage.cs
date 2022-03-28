using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CreditCards.UITests.PageObjectModels
{
    public class ApplicationPage : Page
    {
        public ApplicationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        protected override string PageUrl => "http://localhost:44108/Apply";
        protected override string PageTitle => "Credit Card Application - Credit Cards";


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

        private void SendKeys(string id, string value)
        {
            var element = _driver.FindElement(By.Id(id));

            element.Clear();
            element.SendKeys(value);
        }
    }
}
