using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace CreditCards.UITests
{
    [Trait("Category", "Smoke")]
    public class CreditCardWebAppShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string AboutUrl = "http://localhost:44108/Home/About";
        private const string HomeTitle = "Home Page - Credit Cards";

        [Fact]
        public void LoadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);

                driver.Manage().Window.Maximize();
                DemoHelper.Pause();
                driver.Manage().Window.Minimize();
                DemoHelper.Pause();
                driver.Manage().Window.Size = new Size(300, 400);
                DemoHelper.Pause();
                driver.Manage().Window.Position = new Point(1, 1);
                DemoHelper.Pause();
                driver.Manage().Window.FullScreen();
                DemoHelper.Pause();

                // Act

                // Assert
                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        public void ReloadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);
                var generationTokenElement = driver.FindElement(By.Id("GenerationToken")).Text;

                // Act
                driver.Navigate().Refresh();

                // Assert
                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
                Assert.NotEqual(generationTokenElement, driver.FindElement(By.Id("GenerationToken")).Text);
            }
        }

        [Fact]
        public void ReloadHomePageOnBack()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);
                
                driver.Navigate().GoToUrl(AboutUrl);

                // Act
                driver.Navigate().Back();

                // Assert
                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        public void ReloadHomePageOnForward()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(AboutUrl);
                driver.Navigate().GoToUrl(HomeUrl);
                
                driver.Navigate().Back();

                // Act
                driver.Navigate().Forward();

                // Assert
                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        public void DisplayHomePage_shouldAppear_beforeTimeOut()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);


                // Act
                driver.Navigate().GoToUrl(HomeUrl);

                // Assert
                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        public void DisplayHomePage_javaScriptShouldBeFinished_beforeTimeOut()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);


                // Act
                driver.Navigate().GoToUrl(HomeUrl);

                // Assert
                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }
    }
}
