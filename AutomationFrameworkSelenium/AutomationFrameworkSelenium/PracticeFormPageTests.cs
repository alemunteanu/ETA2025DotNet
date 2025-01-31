using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AutomationFrameworkSelenium
{
    public class PracticeFormPageTests
    {
        IWebDriver driver;

        [Test]
        public void PracticeFormTests()
        {
            this.driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demoqa.com");

            ((IJavaScriptExecutor)driver).ExecuteScript("document.body.style.zoom='80%'");

            ClickOnFormsMenu(driver);

            Assert.That(driver.Url.EndsWith("forms"));

            ClickOnPracticeForm(driver);

            Assert.That(driver.Url.EndsWith("automation-practice-form"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.Id("userForm")));

            FillInName(driver, "Alexandra", "Munteanu");

            FillInEmailAddress(driver, "alexandra.munteanu@test.com");

            SelectGender(driver, "Female");

            FillInPhoneNumber(driver, "07994587890");

            SelectDateOfBirth(driver, "1994", "September", "2");

            List<string> listOfHobbies = new List<string> { "Sports", "Reading", "Music" };

            CheckHobbies(driver, listOfHobbies);

            List<string> listOfSubjects = new List<string> { "Maths", "English", "Arts" };

            FillInSubjectsTextBox(driver, listOfSubjects);

            List<string> listOfSubjectsToBeRemoved = new List<string> { "English", "Arts" };

            RemoveOneByOneSubjects(driver, listOfSubjectsToBeRemoved);

            RemoveAllSubjects(driver);

            FillInCurrentAddress(driver, "Palat street 3E");

            SelectStateAndCity(driver, "NCR", "Delhi");

            SubmitForm(driver);

            Assert.True(driver.FindElement(By.Id("example-modal-sizes-title-lg")).Displayed);
            Assert.That(driver.FindElement(By.Id("example-modal-sizes-title-lg")).Text.Trim().Equals("Thanks for submitting the form"));

            CloseForm(driver);
        }

        private void ClickOnPracticeForm(IWebDriver driver)
        {
            driver.FindElement(By.XPath("//span[text()='Practice Form']")).Click();
        }

        private void ClickOnFormsMenu(IWebDriver driver)
        {
            IWebElement practiceMenu = driver.FindElement(By.XPath(".//h5[text()='Forms']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", practiceMenu);
            practiceMenu.Click();
        }

        private void CheckHobbies(IWebDriver driver, List<string> checkBoxToSelectList)
        {
            List<IWebElement> hobbyCheckBoxList = driver.FindElements(By.XPath("//input[@type='checkbox']/parent::div")).ToList();

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", hobbyCheckBoxList.First());

            Thread.Sleep(1000);

            hobbyCheckBoxList.Where(checkbox =>
            checkBoxToSelectList.Contains(checkbox.FindElement(By.TagName("label")).Text)
            && !checkbox.Selected).ToList().ForEach(checkbox => checkbox.Click());

            Thread.Sleep(1000);

            foreach (var checkbox in hobbyCheckBoxList)
            {
                if (checkBoxToSelectList.Contains(checkbox.Text))
                {
                    Assert.True(checkbox.FindElement(By.TagName("input")).Selected, $"Checkbox with value '{checkbox.Text}' is not selected.");
                }
            }
        }

        private void FillInSubjectsTextBox(IWebDriver driver, List<string> subjectNames)
        {
            IWebElement subjectsTextBox = driver.FindElement(By.Id("subjectsContainer"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", subjectsTextBox);

            Actions actions = new Actions(driver);

            subjectNames.ForEach(subject => actions.Click(subjectsTextBox)
                                                    .SendKeys(subject)
                                                    .Pause(TimeSpan.FromMilliseconds(500))
                                                    .SendKeys(Keys.Enter)
                                                    .Build()
                                                    .Perform());
            Thread.Sleep(1000);
        }

        private void RemoveAllSubjects(IWebDriver driver)
        {
            List<IWebElement> subjectsExamples = driver.FindElements(By.XPath("//div[@id='subjectsContainer']//div[contains(@class,'multi-value') and not(contains(@class,'remove') or contains(@class,'label'))]")).ToList();
            driver.FindElement(By.XPath("//div[contains(@class,'clear')]")).Click();

            Thread.Sleep(1000);

            IWebElement subjectsTextBox = driver.FindElement(By.Id("subjectsContainer"));
            Assert.IsEmpty(subjectsTextBox.FindElements((By.XPath("//div[contains(@class,'multi-value') and not(contains(@class,'remove') or contains(@class,'label'))]"))));
        }

        private void RemoveOneByOneSubjects(IWebDriver driver, List<string> subjectToBeRemovedList)
        {
            IWebElement subjectsTextBox = driver.FindElement(By.Id("subjectsContainer"));
            List<IWebElement> subjectsExamples = subjectsTextBox.FindElements(By.XPath("//div[contains(@class,'multi-value__label') and not(contains(@class,'remove'))]")).ToList();
            subjectsExamples.Where(subject => subjectToBeRemovedList.Contains(subject.Text))
                .ToList()
                .ForEach(subject => subject.FindElement(By.XPath("//div[contains(@class,'remove')]"))
                .Click());

            Thread.Sleep(1000);
        }

        private void FillInName(IWebDriver driver, string firstName, string lastName)
        {
            IWebElement firstNameTextBox = driver.FindElement(By.Id("firstName"));
            firstNameTextBox.Clear();
            firstNameTextBox.SendKeys(firstName);

            IWebElement lastNameTextBox = driver.FindElement(By.Id("lastName"));
            lastNameTextBox.Clear();
            lastNameTextBox.SendKeys(lastName);
        }

        private void FillInEmailAddress(IWebDriver driver, string emailAddress)
        {
            IWebElement phoneNumber = driver.FindElement(By.Id("userEmail"));
            phoneNumber.Clear();
            phoneNumber.SendKeys(emailAddress);
        }

        private void FillInPhoneNumber(IWebDriver driver, string phoneNumber)
        {
            IWebElement phoneNumberTextBox = driver.FindElement(By.Id("userNumber"));
            phoneNumberTextBox.Clear();
            phoneNumberTextBox.SendKeys(phoneNumber);
        }

        private void SelectGender(IWebDriver driver, string gender)
        {
            switch (gender)
            {
                case "Male":
                    Console.WriteLine("Male gender is selected!");
                    driver.FindElement(By.XPath("//label[@for='gender-radio-1']")).Click();
                    break;
                case "Female":
                    Console.WriteLine("Female gender is selected!");
                    driver.FindElement(By.XPath("//label[@for='gender-radio-2']")).Click();
                    break;
                case "Other":
                    Console.WriteLine("Other option is selected!");
                    driver.FindElement(By.XPath("//label[@for='gender-radio-3']")).Click();
                    break;
                default:
                    Console.WriteLine("No value matched the criteria!");
                    break;
            }
        }

        private void SelectDateOfBirth(IWebDriver driver, string year, string month, string desiredDate)
        {
            IWebElement dateOfBirthTextBox = driver.FindElement(By.Id("dateOfBirthInput"));
            dateOfBirthTextBox.Click();

            IWebElement yearDropdownList = driver.FindElement(By.XPath("//select[@class='react-datepicker__year-select']"));
            SelectElement selectYear = new SelectElement(yearDropdownList);
            selectYear.SelectByText(year);

            IWebElement monthDropdownList = driver.FindElement(By.XPath("//select[@class='react-datepicker__month-select']"));
            SelectElement selectMonth = new SelectElement(monthDropdownList);
            selectMonth.SelectByText(month);

            List<IWebElement> daysOfTheMonthDatePicker = driver.FindElements(By.XPath("//div[@class='react-datepicker__week']/div[contains(@class,'react-datepicker__day') and not(contains(@class,'outside-month'))]"))
                .ToList();

            daysOfTheMonthDatePicker.FirstOrDefault(day => day.Text.Trim().Equals(desiredDate)).Click();

            Thread.Sleep(1000);
        }

        private void FillInCurrentAddress(IWebDriver driver, string currentAddress)
        {
            IWebElement currentAddressTextBox = driver.FindElement(By.Id("currentAddress"));
            currentAddressTextBox.Click();
            currentAddressTextBox.Clear();
            currentAddressTextBox.SendKeys(currentAddress);
        }

        private void SelectStateAndCity(IWebDriver driver, string state, string city)
        {
            IWebElement stateDropdownList = driver.FindElement(By.Id("state"));
            stateDropdownList.Click();

            var listOfStates = driver.FindElements(By.XPath("//div[contains(@class,'option')]")).ToList();
            listOfStates.Where(element => element.Text.Trim().Equals(state)).FirstOrDefault().Click();

            Thread.Sleep(500);

            IWebElement cityDropdownList = driver.FindElement(By.XPath("//div[text()='Select City']"));
            cityDropdownList.Click();

            var listOfCities = driver.FindElements(By.XPath("//div[contains(@class,'option')]")).ToList();
            listOfCities.Where(element => element.Text.Trim().Equals(city)).FirstOrDefault().Click();

            Thread.Sleep(500);
        }

        private void SubmitForm(IWebDriver driver)
        {
            driver.FindElement(By.Id("submit")).Submit();
        }

        private void CloseForm(IWebDriver driver)
        {
            IWebElement closeButton = driver.FindElement(By.Id("closeLargeModal"));
            closeButton.Click();
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
