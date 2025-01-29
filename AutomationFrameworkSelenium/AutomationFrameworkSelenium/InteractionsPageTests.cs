using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFrameworkSelenium
{
    public class InteractionsPageTests
    {
        IWebDriver driver;

        [Test]
        public void InteractionPageTests()
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

            ClickOnOption(driver, "Selectable");

            Assert.That(driver.Url.EndsWith("selectable"));

            ClickOnTabWithName(driver, "Grid");

            Assert.That(driver.FindElement(By.Id("demo-tab-list")).GetAttribute("aria-selected").Equals("false"));
            Assert.That(driver.FindElement(By.Id("demo-tab-grid")).GetAttribute("aria-selected").Equals("true"));

            SelectOddCells(driver);
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

        private void ClickOnTabWithName(IWebDriver driver, string tabName)
        {
            IWebElement tab = driver.FindElements(By.XPath("//a[@role='tab']")).ToList().FirstOrDefault(tab => tab.Text.Equals(tabName));
            if (tab != null)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", tab);
                tab.Click();
                Console.WriteLine("Tab is selected successfully.");
            }
            else
            {
                Console.WriteLine("Tab not found");
            }
        }

        private void SelectOddCells(IWebDriver driver)
        {
            List<IWebElement> listOfCells = driver.FindElements(By.XPath("//div[@id='gridContainer']//li")).ToList();
            List<Dictionary<int, IWebElement>> resultList = listOfCells.Select((cell, index) => new Dictionary<int, IWebElement> { { index + 1, cell } }).ToList()
                .Where(item => item.Keys.First() % 2 != 0).ToList();

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", listOfCells.First());

            foreach (var map in resultList)
            {
                foreach (var kvp in map)
                {
                    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value.Text}");
                    kvp.Value.Click();
                    Console.WriteLine($"Cell with value: {kvp.Value.Text} has been clicked!");
                }
            }

            resultList.SelectMany(cell => cell.Values)
                .ToList()
                .ForEach(value => Assert.True(value.GetAttribute("class").Contains("active")));
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
