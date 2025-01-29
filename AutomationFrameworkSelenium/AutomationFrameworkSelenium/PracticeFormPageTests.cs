using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace AutomationFrameworkSelenium
{
    public class PracticeFormPageTests
    {
        IWebDriver driver;

        [Test]
        public void PracticeFormTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demoqa.com");

            ClickOnFormsMenu(driver);

            Assert.That(driver.Url.EndsWith("forms"));

            driver.FindElement(By.XPath("//span[text()='Practice Form']")).Click();

            Assert.That(driver.Url.EndsWith("automation-practice-form"));

            List<string> listOfHobbies = new List<string> { "Sports", "Reading", "Music" };

            CheckHobbies(driver, listOfHobbies);

            List<string> listOfSubjects = new List<string> { "Maths", "English", "Arts" };

            FillInSubjectsTextBox(driver, listOfSubjects);

            List<string> listOfSubjectsToBeRemoved = new List<string> { "English", "Arts" };

            RemoveOneByOneSubjects(driver, listOfSubjectsToBeRemoved);

            RemoveAllSubjects(driver);
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

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
