using OpenQA.Selenium;

namespace CreditCards.UITests.PageObjectModels
{
    public class ApplicationCompletePage : Page
    {
        public ApplicationCompletePage(IWebDriver driver)
        {
            _driver = driver;
        }

        protected override string PageUrl => "http://localhost:44108/Apply";
        protected override string PageTitle => "Application Complete - Credit Cards";

        public string Decision => _driver.FindElement(By.Id("Decision")).Text;
        public string ReferenceNumber => _driver.FindElement(By.Id("ReferenceNumber")).Text;
        public string FullName => _driver.FindElement(By.Id("FullName")).Text;
        public string Age => _driver.FindElement(By.Id("Age")).Text;
        public string Income => _driver.FindElement(By.Id("Income")).Text;
        public string RelationshipStatus => _driver.FindElement(By.Id("RelationshipStatus")).Text;
        public string BusinessSource => _driver.FindElement(By.Id("BusinessSource")).Text;
    }
}
