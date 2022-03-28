using CreditCards.UITests.PageObjectModels;
using Xunit;

namespace CreditCards.UITests
{
    [Trait("Category", "Smoke")]
    public class CreditCardWebAppShouldPagePattern : IClassFixture<ChromeDriverFixture>
    {
        private const string AboutUrl = "http://localhost:44108/Home/About";

        private readonly ChromeDriverFixture _chromeDriverFixture;

        public CreditCardWebAppShouldPagePattern(ChromeDriverFixture chromeDriverFixture)
        {
            _chromeDriverFixture = chromeDriverFixture;
            _chromeDriverFixture.Driver.Manage().Cookies.DeleteAllCookies();
            _chromeDriverFixture.Driver.Navigate().GoToUrl("about:blank");
        }

        [Fact]
        public void ReloadHomePage()
        {
            // Arrange
            var homePage = new HomePage(_chromeDriverFixture.Driver);
            homePage.NavigateTo();

            var generatedToken = homePage.GenerationToken;

            _chromeDriverFixture.Driver.Navigate().GoToUrl(AboutUrl);

            // Act
            _chromeDriverFixture.Driver.Navigate().Back();
            homePage.EnsurePageLoaded();

            Assert.NotEqual(generatedToken, homePage.GenerationToken);
        }

        [Fact]
        public void DisplayProductsAndRates()
        {
            // Arrange
            var homePage = new HomePage(_chromeDriverFixture.Driver);
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
