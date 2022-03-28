using System;
using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.Windows;
using CreditCards.UITests.PageObjectModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Abstractions;
#pragma warning disable CS0618 // Type or member is obsolete
using static OpenQA.Selenium.Support.UI.ExpectedConditions;
#pragma warning restore CS0618 // Type or member is obsolete

namespace CreditCards.UITests
{
    [Trait("Category", "Applications")]
    public class CreditCardApplicationShouldPagePattern
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string ApplyUrl = "http://localhost:44108/Apply";
        private const string AboutUrl = "http://localhost:44108/Home/About";

        private readonly ITestOutputHelper _output;

        public CreditCardApplicationShouldPagePattern(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewLowRate()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                // Act
                var applicationPage = homePage.ClickApplyLowRateLink();

                // Assert
                applicationPage.EnsurePageLoaded();
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewEasyApplication_PreBuiltConditions_WithWait()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                homePage.WaitForEasyApplicationCarouselPage();

                // Act
                var applicationPage = homePage.ClickApplyNowLink();

                // Assert
                applicationPage.EnsurePageLoaded();
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewEasyApplication_ExplicitWait()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                _output.WriteLine($"{DateTime.Now.ToLongTimeString()} navigating to {HomeUrl} ");
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                _output.WriteLine($"{DateTime.Now.ToLongTimeString()} finding element using explicit");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(35));

                var applyLink = wait.Until(ElementToBeClickable(By.ClassName("customer-service-apply-now")));

                _output.WriteLine($"{DateTime.Now.ToLongTimeString()} found element Displayed={applyLink.Displayed} Enabled={applyLink.Enabled}");

                // Act
                applyLink.Click();
                DemoHelper.Pause();

                // Assert
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }


        [Fact]
        public void BeInitiatedFromHomePage_CustomerService()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();
                DemoHelper.Pause();
                var rightCarouselLink = driver.FindElement(By.CssSelector("[data-slide='next']"));
                rightCarouselLink.Click();
                DemoHelper.Pause(); 
                rightCarouselLink.Click();
                DemoHelper.Pause();
                var applyLink = driver.FindElement(By.ClassName("customer-service-apply-now"));

                // Act
                applyLink.Click();
                DemoHelper.Pause();

                // Assert
                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();
                DemoHelper.Pause();
                var randomGreetingTextLink = driver.FindElement(By.PartialLinkText("- Apply Now!"));

                // Act
                randomGreetingTextLink.Click();
                DemoHelper.Pause(1000);

                // Assert
                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting_Using_XPATH()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();
                DemoHelper.Pause();
                var randomGreetingTextLink = driver.FindElement(By.XPath("//a[text()[contains(.,'- Apply Now!')]]"));

                // Act
                randomGreetingTextLink.Click();
                DemoHelper.Pause(1000);

                // Assert
                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeSubmittedWhenValid()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var applicationPage = new ApplicationPage(driver);
                applicationPage.NavigateTo();
                applicationPage.EnterFirstName();
                applicationPage.EnterLastName();
                applicationPage.EnterFrequentFlyerNumber();
                applicationPage.EnterAge();
                applicationPage.EnterGrossAnnualIncome();

                applicationPage.ChooseGenderRadio();
                applicationPage.ChooseBusinessSource();

                applicationPage.ClickTermsAccepted();
                
                // Act
                var applicationCompletePage = applicationPage.SubmitFormulaire();

                // Assert
                applicationCompletePage.EnsurePageLoaded();
                Assert.Equal("ReferredToHuman", applicationCompletePage.Decision);
                Assert.NotEmpty(applicationCompletePage.ReferenceNumber);
                Assert.Equal("Sarah Smith", applicationCompletePage.FullName);
                Assert.Equal("18", applicationCompletePage.Age);
                Assert.Equal("50000", applicationCompletePage.Income);
                Assert.Equal("Single", applicationCompletePage.RelationshipStatus);
                Assert.Equal("TV", applicationCompletePage.BusinessSource);
            }
        }

        [Fact]
        public void SubmittedWhenCorrected()
        {

            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var applicationPage = new ApplicationPage(driver);
                applicationPage.NavigateTo();
                applicationPage.EnterFirstName();
                applicationPage.EnterFrequentFlyerNumber();
                applicationPage.EnterAge("17");
                applicationPage.EnterGrossAnnualIncome();

                applicationPage.ChooseGenderRadio();
                applicationPage.ChooseBusinessSource();

                applicationPage.ClickTermsAccepted();

                applicationPage.SubmitFormulaire();
                

                // Assert that validation failed
                var validationErrors = applicationPage.ValidationErrors().ToList();
                Assert.Equal(2, validationErrors.Count);
                Assert.Equal("Please provide a last name", validationErrors.First());
                Assert.Equal("You must be at least 18 years old", validationErrors[1]);

                // Fix errors
                applicationPage.EnterLastName();
                applicationPage.EnterAge();

                // Act
                var applicationCompletePage = applicationPage.SubmitFormulaire();

                // Assert
                applicationCompletePage.EnsurePageLoaded();
                Assert.Equal("ReferredToHuman", applicationCompletePage.Decision);
                Assert.NotEmpty(applicationCompletePage.ReferenceNumber);
                Assert.Equal("Sarah Smith", applicationCompletePage.FullName);
                Assert.Equal("18", applicationCompletePage.Age);
                Assert.Equal("50000", applicationCompletePage.Income);
                Assert.Equal("Single", applicationCompletePage.RelationshipStatus);
                Assert.Equal("TV", applicationCompletePage.BusinessSource);
            }
        }

        [Fact]
        public void OpenContactFooterInANewPage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                // Act
                homePage.ClickContactFooterLink();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                DemoHelper.Pause();

                // Assert
                Assert.EndsWith("/Home/Contact", driver.Url);
            }
        }

        [Fact]
        public void AlertIfLiveChatClosed()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();
                homePage.ClickLiveChatFooterLink();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                // Act
                var alert = wait.Until(AlertIsPresent());
                DemoHelper.Pause();

                // Assert
                Assert.Equal("Live chat is currently closed.", alert.Text);

                alert.Accept();
                DemoHelper.Pause();
            }
        }

        [Fact]
        public void NotNavigateToAboutUsIfCancelClicked()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();
                homePage.ClickLearnAboutUsLink();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                // Act
                var alert = wait.Until(AlertIsPresent());
                DemoHelper.Pause();

                // Assert
                alert.Dismiss();

                homePage.EnsurePageLoaded();
            }
        }

        [Fact]
        public void NotDisplayCookieUseMessage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                driver.Manage().Cookies.AddCookie(new Cookie("acceptedCookies", "true"));

                driver.Navigate().Refresh();

                Assert.False(homePage.IsCookieMessagePresent);

                driver.Manage().Cookies.DeleteCookieNamed("acceptedCookies");
                driver.Navigate().Refresh();

                Assert.True(homePage.IsCookieMessagePresent);
            }
        }

        // /!\ require beyond compare 4 installed on computer
        // need to set the size of the screen
        // doesn't work with dynamic field ( ex : datetime.now )
        [Fact (Skip = "required on machine installation")]
        [UseReporter(typeof(BeyondCompare4Reporter))]
        public void RenderAboutPage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(AboutUrl);

                var screenshotDriver = (ITakesScreenshot)driver;
                var screenshot = screenshotDriver.GetScreenshot();

                screenshot.SaveAsFile("aboutpage.bmp", ScreenshotImageFormat.Bmp);

                var file = new FileInfo("aboutpage.bmp");

                Approvals.Verify(file);
            }
        }

        [Fact]
        public void UseActions()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);
                var applyLink = driver.FindElement(By.Name("ApplyLowRate"));

                var actions = new Actions(driver);

                // Act
                actions.MoveToElement(applyLink);
                actions.Click();
                actions.Perform();

                // Assert
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }
    }
}
