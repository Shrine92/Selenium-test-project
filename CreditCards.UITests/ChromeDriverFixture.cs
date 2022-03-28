using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CreditCards.UITests
{
    public class ChromeDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; }

        public ChromeDriverFixture()
        {
            Driver = new ChromeDriver();
        }

        public void Dispose()
        {
            Driver.Dispose();
        }
    }
}
