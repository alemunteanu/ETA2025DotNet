using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFrameworkSelenium
{
    public class AlertsFramesWindowsPageTests
    {
        IWebDriver driver;

        [Test]
        public void AlertsFramesWindowsTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demoqa.com");

            IWebElement AlertsFrameWindowsMenu = driver.FindElement(By.XPath(".//h5[text()='Alerts, Frame & Windows']"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", AlertsFrameWindowsMenu);

            AlertsFrameWindowsMenu.Click();

            Assert.That(driver.Url.EndsWith("alertsWindows"));

            ClickOnOption(driver, "Frames");

            Assert.That(driver.Url.EndsWith("frames"));

            IWebElement frame1 = driver.FindElement(By.Id("frame1"));

            driver.SwitchTo().Frame(frame1);

            IWebElement frame1Body = driver.FindElement(By.Id("sampleHeading"));

            Console.WriteLine($"The header has the following text: {frame1Body.Text}");

            driver.SwitchTo().DefaultContent();

            IWebElement frame2 = driver.FindElement(By.Id("frame2"));

            driver.SwitchTo().Frame(frame2);

            IWebElement frame2Body = driver.FindElement(By.Id("sampleHeading"));

            Console.WriteLine($"The header has the following text: {frame2Body.Text}");

            driver.SwitchTo().DefaultContent();

            ClickOnOption(driver, "Browser Windows");

            Assert.That(driver.Url.EndsWith("browser-windows"));

            IWebElement newTabButton = driver.FindElement(By.Id("tabButton"));

            newTabButton.Click();

            driver.SwitchTo().Window(driver.WindowHandles.Last());

            Assert.That(driver.Url.EndsWith("sample"));

            IWebElement headerNewTab = driver.FindElement(By.Id("sampleHeading"));

            Console.WriteLine($"The header has the following text: {headerNewTab.Text}");

            driver.Close();

            driver.SwitchTo().Window(driver.WindowHandles.First());

            IWebElement windowButton = driver.FindElement(By.Id("windowButton"));

            windowButton.Click();

            driver.SwitchTo().Window(driver.WindowHandles.Last());

            driver.Manage().Window.Maximize();

            Assert.That(driver.Url.EndsWith("sample"));

            IWebElement headerNewWindow = driver.FindElement(By.Id("sampleHeading"));

            Console.WriteLine($"The header has the following text: {headerNewWindow.Text}");

            driver.Close();

            driver.SwitchTo().Window(driver.WindowHandles.First());
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
