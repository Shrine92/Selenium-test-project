using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
#pragma warning disable CS0618 // Type or member is obsolete
using static OpenQA.Selenium.Support.PageObjects.PageFactory;
#pragma warning restore CS0618 // Type or member is obsolete

namespace CreditCards.UITests.PageObjectModels
{
    public class ApplicationCompletePage : Page
    {
        public ApplicationCompletePage(IWebDriver driver)
        {
            _driver = driver;
            InitElements(driver, this);
        }

        protected override string PageUrl => "http://localhost:44108/Apply";
        protected override string PageTitle => "Application Complete - Credit Cards";

        [FindsBy(How = How.Id, Using = "Decision")]
        private IWebElement ApplicationDecision { get; set; }

        public string Decision => ApplicationDecision.Text;
        public string ReferenceNumber => _driver.FindElement(By.Id("ReferenceNumber")).Text;
        public string FullName => _driver.FindElement(By.Id("FullName")).Text;
        public string Age => _driver.FindElement(By.Id("Age")).Text;
        public string Income => _driver.FindElement(By.Id("Income")).Text;
        public string RelationshipStatus => _driver.FindElement(By.Id("RelationshipStatus")).Text;
        public string BusinessSource => _driver.FindElement(By.Id("BusinessSource")).Text;
    }
}
