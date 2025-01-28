using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFrameworkSelenium
{
    public class InteractionsPageTests
    {
        IWebDriver driver;

        [Test]
        public void TestSortable()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demoqa.com");

            IWebElement interactionsMenu = driver.FindElement(By.XPath(".//h5[text()='Interactions']"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", interactionsMenu);

            interactionsMenu.Click();

            Assert.That(driver.Url.EndsWith("interaction"));

            ClickOnOption(driver, "Sortable");

            Assert.That(driver.Url.EndsWith("sortable"));
            Assert.That(driver.FindElement(By.Id("demo-tab-list")).GetAttribute("aria-selected").Equals("true"));
            Assert.That(driver.FindElement(By.Id("demo-tab-grid")).GetAttribute("aria-selected").Equals("false"));

            GetValuesFromSortableList(driver);
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

        private List<string> GetValuesFromSortableList(IWebDriver driver)
        {
            List<IWebElement> optionsFromSortableListPage = driver.FindElements(By.XPath("//div[contains(@class,'vertical-list-container')]/div")).ToList();
            List<string> stringValues = optionsFromSortableListPage.Select(option => option.Text).ToList();
            Console.WriteLine("New List of Texts:");
            foreach (string optionValue in stringValues)
            {
                Console.WriteLine(string.Format("\t{0}", optionValue));
            }
            return stringValues;
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
