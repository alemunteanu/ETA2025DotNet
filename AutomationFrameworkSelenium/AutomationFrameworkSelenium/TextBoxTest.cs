using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFrameworkSelenium
{
    public class TextBoxTest

    {
        IWebDriver driver;

        [Test]
        public void TestDemoQA()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demoqa.com");

            IWebElement elementsMenu = driver.FindElement(By.XPath(".//h5[text()='Elements']"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", elementsMenu);

            elementsMenu.Click();

            Assert.That(driver.Url.EndsWith("elements"));

            //click on the first option from elements page
            driver.FindElement(By.XPath("//span[text()='Text Box']")).Click();

            Assert.That(driver.Url.EndsWith("text-box"));

            CompleteFullName(driver, "Alexandra Munteanu");
            CompleteEmail(driver, "alexandra.munteanu@mail.com");
            CompleteCurrentAddress(driver, "strada George Emil Palade");
            CompletePermanentAddress(driver, "strada Palat");

            //submit the form
            driver.FindElement(By.Id("submit")).Click();

            //Assert that the form has been successfully submitted
            Assert.That(driver.FindElement(By.Id("output")).Displayed);
        }

        private static void CompleteFullName(IWebDriver driver, string fullName)
        {
            IWebElement fullNameTextBox = driver.FindElement(By.Id("userName"));
            fullNameTextBox.Click();
            fullNameTextBox.Clear();
            fullNameTextBox.SendKeys(fullName);
        }
        private static void CompleteEmail(IWebDriver driver, string email)
        {
            IWebElement emailTextBox = driver.FindElement(By.Id("userEmail"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", emailTextBox);
            emailTextBox.Click();
            emailTextBox.Clear();
            emailTextBox.SendKeys(email);
        }

        private static void CompleteCurrentAddress(IWebDriver driver, string currentAddress)
        {
            IWebElement currentAddressTextBox = driver.FindElement(By.Id("currentAddress"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", currentAddressTextBox);
            currentAddressTextBox.Click();
            currentAddressTextBox.Clear();
            currentAddressTextBox.SendKeys(currentAddress);
        }

        private static void CompletePermanentAddress(IWebDriver driver, string permanentAddress)
        {
            IWebElement permanentAddressTextBox = driver.FindElement(By.Id("permanentAddress"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", permanentAddressTextBox);
            permanentAddressTextBox.Click();
            permanentAddressTextBox.Clear();
            permanentAddressTextBox.SendKeys(permanentAddress);
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

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
