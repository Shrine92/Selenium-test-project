using System;
using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.Windows;
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
    public class CreditCardApplicationShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string ApplyUrl = "http://localhost:44108/Apply";
        private const string AboutUrl = "http://localhost:44108/Home/About";

        private readonly ITestOutputHelper _output;

        public CreditCardApplicationShould(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewLowRate()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);
                var applyLink = driver.FindElement(By.Name("ApplyLowRate"));

                // Act
                applyLink.Click();

                // Assert
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewEasyApplication()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();
                var rightCarouselLink = driver.FindElement(By.CssSelector("[data-slide='next']"));
                rightCarouselLink.Click();
                DemoHelper.Pause();
                var applyLink = driver.FindElement(By.LinkText("Easy: Apply Now!"));

                // Act
                applyLink.Click();
                DemoHelper.Pause();

                // Assert
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewEasyApplication_WithWait()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();
                var rightCarouselLink = driver.FindElement(By.CssSelector("[data-slide='next']"));
                rightCarouselLink.Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                var applyLink = wait.Until(d => d.FindElement(By.LinkText("Easy: Apply Now!")));

                // Act
                applyLink.Click();
                DemoHelper.Pause();

                // Assert
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewEasyApplication_PreBuiltConditions_WithWait()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);
                driver.Manage().Window.Minimize();
                DemoHelper.Pause();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(11));

                var applyLink = wait.Until(ElementToBeClickable(By.LinkText("Easy: Apply Now!")));

                // Act
                applyLink.Click();
                DemoHelper.Pause();

                // Assert
                Assert.Equal(ApplyUrl, driver.Url);
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
                driver.Navigate().GoToUrl(HomeUrl);
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
                driver.Navigate().GoToUrl(HomeUrl);
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
                driver.Navigate().GoToUrl(HomeUrl);
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
                driver.Navigate().GoToUrl(ApplyUrl);
                DemoHelper.Pause(1000);
                driver.FindElement(By.Id("FirstName")).SendKeys("Sarah");
                DemoHelper.Pause(1000);
                driver.FindElement(By.Id("LastName")).SendKeys("Smith");
                DemoHelper.Pause(1000);
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                DemoHelper.Pause(1000);
                driver.FindElement(By.Id("Age")).SendKeys("18");
                DemoHelper.Pause(1000);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("50000");
                DemoHelper.Pause(1000);

                driver.FindElement(By.Id("Single")).Click();
                DemoHelper.Pause(1000);

                var businessSourceElement = driver.FindElement(By.Id("BusinessSource"));
                var businessSource = new SelectElement(businessSourceElement);

                businessSource.Options[1].Click();
                DemoHelper.Pause(500);
                businessSource.SelectByValue("Email");
                DemoHelper.Pause(500);
                businessSource.SelectByText("Internet Search");
                DemoHelper.Pause(500);
                businessSource.SelectByIndex(4);
                DemoHelper.Pause(500);

                driver.FindElement(By.Id("TermsAccepted")).Click();
                DemoHelper.Pause();

                // Act
                // driver.FindElement(By.Id("SubmitApplication")).Click();
                driver.FindElement(By.Id("TermsAccepted")).Submit();
                DemoHelper.Pause();

                // Assert
                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Sarah Smith", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("18", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("50000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("TV", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }

        [Fact]
        public void SubmittedWhenCorrected()
        {
            const string firstName = "Sarah";
            const string invalidAge = "17";
            const string validAge = "18";

            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(ApplyUrl);

                driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
                DemoHelper.Pause(500);
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                DemoHelper.Pause(500);
                driver.FindElement(By.Id("Age")).SendKeys(invalidAge);
                DemoHelper.Pause(500);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("50000");
                DemoHelper.Pause(500);

                driver.FindElement(By.Id("Single")).Click();
                DemoHelper.Pause(500);

                var businessSourceElement = driver.FindElement(By.Id("BusinessSource"));
                var businessSource = new SelectElement(businessSourceElement);

                businessSource.SelectByIndex(4);
                DemoHelper.Pause(500);

                driver.FindElement(By.Id("TermsAccepted")).Click();
                DemoHelper.Pause(500);

                driver.FindElement(By.Id("TermsAccepted")).Submit();
                DemoHelper.Pause(1000);

                // Assert that validation failed
                var validationErrors = driver.FindElements(By.CssSelector(".validation-summary-errors > ul > li"));
                Assert.Equal(2, validationErrors.Count);
                Assert.Equal("Please provide a last name", validationErrors[0].Text);
                Assert.Equal("You must be at least 18 years old", validationErrors[1].Text);

                // Fix errors
                driver.FindElement(By.Id("LastName")).SendKeys("Smith");
                DemoHelper.Pause(500);
                driver.FindElement(By.Id("Age")).Clear();
                driver.FindElement(By.Id("Age")).SendKeys(validAge);
                DemoHelper.Pause(500);


                // driver.FindElement(By.Id("SubmitApplication")).Click();
                driver.FindElement(By.Id("TermsAccepted")).Submit();
                DemoHelper.Pause(1000);

                // Assert
                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Sarah Smith", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("18", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("50000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("TV", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }

        [Fact]
        public void OpenContactFooterInANewPage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);



                // Act
                driver.FindElement(By.Id("ContactFooter")).Click();
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
                driver.Navigate().GoToUrl(HomeUrl);
                driver.FindElement(By.Id("LiveChat")).Click();

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
                driver.Navigate().GoToUrl(HomeUrl);
                driver.FindElement(By.Id("LearnAboutUs")).Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                // Act
                var alert = wait.Until(AlertIsPresent());
                DemoHelper.Pause();

                // Assert
                alert.Dismiss();

                Assert.StartsWith("Home Page", driver.Title);
            }
        }

        [Fact]
        public void NotDisplayCookieUseMessage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Arrange
                driver.Navigate().GoToUrl(HomeUrl);

                driver.Manage().Cookies.AddCookie(new Cookie("acceptedCookies", "true"));

                driver.Navigate().Refresh();

                var messages = driver.FindElements(By.Id("CookiesBeingUsed"));

                Assert.Empty(messages);

                driver.Manage().Cookies.DeleteCookieNamed("acceptedCookies");
                driver.Navigate().Refresh();

                Assert.NotNull(driver.FindElements(By.Id("CookiesBeingUsed")));
            }
        }

        // /!\ require beyond compare 4 installed on computer
        // need to set the size of the screen
        // doesn't work with dynamic field ( ex : datetime.now )
        [Fact]
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
