using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class FlightSearchTests
{
    private IWebDriver driver;
    private const string BaseURL = "https://blazedemo.com";

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        // options.AddArgument("--headless"); // uncomment for CI
        driver = new ChromeDriver(options);
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void FlightSearch_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        // Select departure: Mexico City
        wait.Until(ExpectedConditions.ElementIsVisible(By.Name("fromPort")));
        driver.FindElement(By.XPath("//select[@name='fromPort']/option[@value='Mexico City']")).Click();

        // Select destination: Dublin
        driver.FindElement(By.XPath("//select[@name='toPort']/option[@value='Dublin']")).Click();

        // Act
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        // Assert - wait for results page to load
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.table tbody tr")));

        var flightRows = driver.FindElements(By.CssSelector("table.table tbody tr"));

        flightRows.Should().HaveCountGreaterThanOrEqualTo(3,
            because: "there should be at least 3 flights available between Mexico City and Dublin");
    }
}