using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFrameworkSelenium
{
    public class WebTablesPageTests
    {
        IWebDriver driver;

        public class TableRow
        {

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Age { get; set; }
            public string Salary { get; set; }
            public string Department { get; set; }

        }

        [Test]
        public void WebTablesTests()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demoqa.com");

            IWebElement elementsMenu = driver.FindElement(By.XPath(".//h5[text()='Elements']"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", elementsMenu);

            elementsMenu.Click();

            Assert.That(driver.Url.EndsWith("elements"));

            //click on the first option from elements page
            driver.FindElement(By.XPath("//span[text()='Web Tables']")).Click();

            Assert.That(driver.Url.EndsWith("webtables"));

            var noOfCompletedRows = GetNoOfCompletedRows();

            //Add a new row
            driver.FindElement(By.Id("addNewRecordButton")).Click();

            //Assert that the form modal is displayed
            Assert.That(driver.FindElement(By.Id("registration-form-modal")).Displayed);

            //Complete form
            CompleteFirstName(driver, "Alexandra");
            CompleteLastName(driver, "Munteanu");
            CompleteEmail(driver, "alexandra.munteanu@endava.com");
            CompleteAge(driver, "30");
            CompleteSalary(driver, "150000");
            CompleteDepartment(driver, "HR");
            SubmitForm(driver);

            Assert.That(GetNoOfCompletedRows().Equals(noOfCompletedRows + 1));
            StoreRowsAsObjects(driver);
        }

        private void CompleteFirstName(IWebDriver driver, string firstName)
        {
            IWebElement firstNameTextBox = driver.FindElement(By.Id("firstName"));
            firstNameTextBox.Click();
            firstNameTextBox.Clear();
            firstNameTextBox.SendKeys(firstName);
        }

        private void CompleteLastName(IWebDriver driver, string lastName)
        {
            IWebElement lastNameTextBox = driver.FindElement(By.Id("lastName"));
            lastNameTextBox.Click();
            lastNameTextBox.Clear();
            lastNameTextBox.SendKeys(lastName);
        }

        private static void CompleteEmail(IWebDriver driver, string email)
        {
            IWebElement emailTextBox = driver.FindElement(By.Id("userEmail"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", emailTextBox);
            emailTextBox.Click();
            emailTextBox.Clear();
            emailTextBox.SendKeys(email);
        }

        private void CompleteAge(IWebDriver driver, string age)
        {
            IWebElement ageTextBox = driver.FindElement(By.Id("age"));
            ageTextBox.Click();
            ageTextBox.Clear();
            ageTextBox.SendKeys(age);
        }
        private void CompleteSalary(IWebDriver driver, string salary)
        {
            IWebElement ageTextBox = driver.FindElement(By.Id("salary"));
            ageTextBox.Click();
            ageTextBox.Clear();
            ageTextBox.SendKeys(salary);
        }
        private void CompleteDepartment(IWebDriver driver, string department)
        {
            IWebElement ageTextBox = driver.FindElement(By.Id("department"));
            ageTextBox.Click();
            ageTextBox.Clear();
            ageTextBox.SendKeys(department);
        }

        private void SubmitForm(IWebDriver driver)
        {
            driver.FindElement(By.Id("submit")).Submit();
        }

        private int GetNoOfCompletedRows()
        {
            return driver.FindElements(By.XPath("//div[@class='rt-table']//div[@class='action-buttons']/ancestor::div[@role='rowgroup']")).Count;
        }

        private void StoreRowsAsObjects(IWebDriver driver)
        {
            //Identify the headers table
            IList<IWebElement> headers = driver.FindElements(By.XPath("//div[@class='rt-resizable-header-content' and not(text()='Action')]"));

            //Identify all row that have data
            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@class='rt-table']//div[@class='action-buttons']/ancestor::div[@role='rowgroup']"));

            List<Dictionary<string, string>> tableData = new List<Dictionary<string, string>>();

            foreach (IWebElement row in rows)
            {
                IList<IWebElement> cells = row.FindElements(By.XPath("//div[@class='rt-td']"));

                //Create a dictionary to store column data for this row
                Dictionary<string, string> valuesRow = new Dictionary<string, string>();

                for (int i = 0; i < cells.Count && i < headers.Count; i++)

                {
                    //Use header text as the key and cell text as the value
                    string columnName = headers[i].Text;
                    string cellValue = cells[i].Text;
                    valuesRow[columnName] = cellValue;
                }

                // Map the dictionary to the TableRow class
                TableRow tableRow = MapDictionaryToTableRow(valuesRow);

                tableData.Add(valuesRow);
            }

            for (int i = 0; i < tableData.Count; i++)
            {
                Console.WriteLine($"Row {i + 1} Data:");
                foreach (var keyValuePair in tableData[i])
                {
                    Console.WriteLine($"{keyValuePair.Key}: {keyValuePair.Value}");
                }
                //Add spacing between the rows
                Console.WriteLine();
            }
        }

        // Method to map a dictionary to the TableRow class
        static TableRow MapDictionaryToTableRow(Dictionary<string, string> data)
        {
            TableRow tableRow = new TableRow();
            PropertyInfo[] properties = typeof(TableRow).GetProperties();

            foreach (var property in properties)
            {
                if (data.TryGetValue(property.Name, out string value)) // Case-sensitive by default
                {
                    property.SetValue(tableRow, value);
                }
            }

            return tableRow;
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
