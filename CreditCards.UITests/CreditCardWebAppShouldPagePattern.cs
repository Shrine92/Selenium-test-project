using CreditCards.UITests.PageObjectModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace CreditCards.UITests
{
    [Trait("Category", "Smoke")]
    public class CreditCardWebAppShouldPagePattern
    {
        private const string AboutUrl = "http://localhost:44108/Home/About";
        [Fact]
        public void ReloadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                var generatedToken = homePage.GenerationToken;

                driver.Navigate().GoToUrl(AboutUrl);

                // Act
                driver.Navigate().Back();
                homePage.EnsurePageLoaded();

                Assert.NotEqual(generatedToken, homePage.GenerationToken);
            }
        }

        [Fact]
        public void DisplayProductsAndRates()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();


                DemoHelper.Pause();
                var products = homePage.Products;

                // Act

                // Assert
                Assert.Equal("Easy Credit Card", products[0].name);
                Assert.Equal("20% APR", products[0].interestRate);
            }
        }
    }
}
