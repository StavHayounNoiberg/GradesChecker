using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace GradesChecker
{
    public class WebControls
    {
        public IWebDriver CreateWebDriver()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless=new");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            IWebDriver driver = new ChromeDriver(service, options);
            return driver;
        }

        public void SignInAndGetPage(ref IWebDriver driver, UserData data)
        {
            IWebElement element = driver.FindElement(By.Id("input_1"));
            element.SendKeys(data.Username);
            element = driver.FindElement(By.Id("input_2"));
            element.SendKeys(data.Password);
            element = driver.FindElement(By.Id("submit_row"));
            element.Submit();
        }

        public void GoToGradesPage(ref IWebDriver driver, UserData data)
        {
            string parentWindowHandle = driver.CurrentWindowHandle;
            IWebElement element = driver.FindElement(By.Id("/Common/Yedion"));
            element.Click();

            List<string> windows_lst = driver.WindowHandles.ToList();
            foreach (var handle in windows_lst)
            {
                if (handle != parentWindowHandle)
                    driver.SwitchTo().Window(handle);
            }

            element = driver.FindElement(By.XPath("//*[@id=\"kt_toolbar_container\"]/div[1]/div/a[5]"));
            element.Click();

            element = driver.FindElement(By.Id("R1C1"));
            SelectElement selectYear = new SelectElement(element);
            selectYear.SelectByValue(data.Year);

            element = driver.FindElement(By.Id("R1C2"));
            SelectElement selectSemester = new SelectElement(element);
            var semester = data.Semester;
            if (semester == "Summer")
                semester = "3";
            selectSemester.SelectByValue(semester);
            element = driver.FindElement(By.XPath("//*[@id=\"kt_content\"]/div/div[2]/div/div/div/form/div[3]/a"));
            element.Click();
        }

        public List<Grade> FindAllGrades(ref IWebDriver driver)
        {
            IWebElement element = driver.FindElement(By.XPath("//*[@id=\"kt_content\"]/div/div[2]/div/div/div/div[7]/div[2]/a/h2"));
            element.Click();

            element = driver.FindElement(By.XPath("//*[@id=\"kt_content\"]/div/div[2]/div/div/div/div[7]"));
            List<IWebElement> allData = new List<IWebElement>(element.FindElements(By.TagName("div")));

            List<Grade> new_grades = new List<Grade>();

            foreach (var section in allData)
            {
                if (section.GetAttribute("id").StartsWith("MyFather"))
                {
                    Grade g = new Grade();
                    IWebElement subElement = section.FindElement(By.TagName("div"));
                    g.title = subElement.FindElement(By.TagName("h2")).GetAttribute("innerText");
                    string title = string.Join("", subElement.FindElement(By.TagName("h2")).GetAttribute("innerText").ToCharArray().Where(Char.IsDigit));
                    if (title.Length > 5)
                        title = title.Remove(5);
                    g.courseID = Convert.ToInt32(title);
                    string outcome = subElement.FindElement(By.TagName("b")).GetAttribute("innerText");
                    g.grade = 0;
                    outcome = string.Join("", outcome.ToCharArray().Where(Char.IsDigit));
                    if (outcome.Length > 0)
                        g.grade = Convert.ToInt32(outcome);
                    List<IWebElement> divs = new List<IWebElement>(section.FindElements(By.ClassName("InRange")));
                    foreach (var div in divs)
                    {
                        string attribute = div.GetAttribute("innerText");
                        if ((attribute.Contains("בחינה")) || (attribute.Contains("סופי")) || (attribute.Contains("שקלול")) || (attribute.Contains("בוחן")) || (attribute.Contains("פרויקט")) || (attribute.Contains("מעבדה")))
                        {
                            g.subID = attribute;
                            new_grades.Add(g);
                            break;
                        }
                    }
                }
            }
            return new_grades;
        }
    }
}
