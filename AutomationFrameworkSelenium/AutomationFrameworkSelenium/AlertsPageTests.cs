using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace AutomationFrameworkSelenium
{
    public class AlertsPageTests
    {
        IWebDriver driver;

        [Test]
        public void AlertsTests()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demoqa.com");

            IWebElement AlertsFrameWindowsMenu = driver.FindElement(By.XPath(".//h5[text()='Alerts, Frame & Windows']"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", AlertsFrameWindowsMenu);

            AlertsFrameWindowsMenu.Click();

            Assert.That(driver.Url.EndsWith("alertsWindows"));

            ClickOnOption(driver, "Alerts");

            Assert.That(driver.Url.EndsWith("alerts"));

            IWebElement alertButton = driver.FindElement(By.Id("alertButton"));

            alertButton.Click();

            AcceptAlert();

            IWebElement alertWithDelayButton = driver.FindElement(By.Id("timerAlertButton"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", alertWithDelayButton);

            alertWithDelayButton.Click();

            WebDriverWait secondWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            secondWait.Until(ExpectedConditions.AlertIsPresent());

            AcceptAlert();

            IWebElement alertConfirm = driver.FindElement(By.Id("confirmButton"));
            alertConfirm.Click();
            DismissAlert();

            IWebElement promptAlert = driver.FindElement(By.Id("promtButton"));
            promptAlert.Click();
            IAlert alertPrompt = driver.SwitchTo().Alert();
            alertPrompt.SendKeys("text alert");
            alertPrompt.Accept();
        }

        private void DismissAlert()
        {
            IAlert alertCancel = driver.SwitchTo().Alert();
            alertCancel.Dismiss();
        }

        private void AcceptAlert()
        {
            IAlert alert = this.driver.SwitchTo().Alert();
            alert.Accept();
        }

        private void ClickOnOption(IWebDriver driver, string desiredOption)
        {
            List<IWebElement> interactionsOptions = driver.FindElements(By.XPath("//div[@class='element-list collapse show']//li")).ToList();

            IWebElement option = interactionsOptions.FirstOrDefault(element => element.Text.Contains(desiredOption));

            if (option != null)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", option);
                option.Click();
                Console.WriteLine("Option selected successfully.");
            }
            else
            {
                Console.WriteLine("No element matched the criteria.");
            }
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
