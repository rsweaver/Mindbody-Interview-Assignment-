using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MindbodyHomework
{
    class WebDriver : IWebDriver
    {
        ChromeDriver Driver = null;
        String Url = "";

        public WebDriver()
        {
            this.Driver = new ChromeDriver();
        }

        public WebDriver(String Path)
        {
            this.Driver = new ChromeDriver(Path);
        }
            string IWebDriver.Url
        {
            get
            {
                return Driver.Url;
            }

            set
            {
                Driver.Url = value;
            }
        }

        public string Title
        {
            get
            {
                return Driver.Title;
            }
        }

        public string PageSource
        {
            get
            {
                return Driver.PageSource;
            }
        }

        public string CurrentWindowHandle
        {
            get
            {
                return Driver.CurrentWindowHandle;
            }
        }

        public ReadOnlyCollection<string> WindowHandles
        {
            get
            {
                return Driver.WindowHandles;
            }
        }

        public void Close()
        {
            Driver.Close();
        }

        public void Quit()
        {
            Driver.Quit();
        }

        public IOptions Manage()
        {
            return Driver.Manage();
        }

        public INavigation Navigate()
        {
            return Driver.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return Driver.SwitchTo();
        }

        public IWebElement FindElement(By MyBy)
        {
            return Driver.FindElement(MyBy);
         }

        public IWebElement FindElementByXPath(String XPath)
        {
            return Driver.FindElementByXPath(XPath);
        }

        public IWebElement FindElementById(String Id)
        {
            return Driver.FindElementById(Id);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By MyBy)
        {
            return Driver.FindElements(MyBy);
        }

        public void Dispose()
        {
            Driver.Dispose();
        }

        public String FindCalendarHandle(String OriginalWindowHandle)
        {
            string CalendarHandle = string.Empty;
            ReadOnlyCollection<string> AllWindowHandles = Driver.WindowHandles;

            foreach (string Handle in AllWindowHandles)
            {
                if (Handle != OriginalWindowHandle)
                {
                    CalendarHandle = Handle; break;
                }
            }
            return CalendarHandle;
        }

    }
}
