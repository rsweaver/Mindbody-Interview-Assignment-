
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Threading;

namespace Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void CheckParkCostandTimeCase1()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            // Select Short-term Parking from Lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("STP");

            // Enter 10:00 PM in Entry Time, select PM in radio button
            Driver.FindElement(By.Id("EntryTime")).Clear();
            Driver.FindElement(By.Id("EntryTime")).SendKeys("10:00");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td[2]/font/input[3]")).Click();


            //Enter date as 01/01/2014 In Entry Date
            Driver.FindElement(By.Id("EntryDate")).Clear();
            Driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            // Enter 11:00 in Exit Time, select PM in radio button
            Driver.FindElement(By.Id("ExitTime")).Clear();
            Driver.FindElement(By.Id("ExitTime")).SendKeys("11:00");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td[2]/font/input[3]")).Click();

            //Enter date as 01/01/2014 In Exit Date
            Driver.FindElement(By.Id("ExitDate")).Clear();
            Driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Click calculate button, Check cost is equal to $ 2.00, Check duration of stay is (0 Days, 1 Hours, 0 Minutes)
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("$ 2.00", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b")).Text);
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            try
            {
                Assert.AreEqual("(0 Days, 1 Hours, 0 Minutes)", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b")).Text.Trim());
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            // Quit Chrome
            Driver.Quit();
        }

        [TestMethod]
        public void CheckParkCostandTimeCase2()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            //Get current window handle so we can switch when clicking the calendar icon
            String OriginalHandle = Driver.CurrentWindowHandle;

            // Select Long Term Surface Parking from Lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("LTS");

            //Click calendar icon to enter entry date
            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td[2]/font/a/img")).Click();
            Thread.Sleep(2000);

            //Switch Driver to new calendar frame for entry date
            string EntryDateHandle = string.Empty;
            ReadOnlyCollection<string> EntryWindowHandles = Driver.WindowHandles;

            foreach (string Handle in EntryWindowHandles)
            {
                if (Handle != OriginalHandle)
                {
                    EntryDateHandle = Handle; break;
                }
            }

            Driver.SwitchTo().Window(EntryDateHandle);


            //Click the year descend button twice to get the year to be 2014 for entry date, click 1 for 01/01/2014
            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]")).Click();
            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]")).Click();


            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[4]/font/a")).Click();

            //Switch back to old frame to click calendar icon for exit date
            Driver.SwitchTo().Window(OriginalHandle);

            //Click calendar icon to enter exit date
            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td[2]/font/a/img")).Click();
            Thread.Sleep(2000);

            string ExitDateHandle = string.Empty;
            ReadOnlyCollection<string> ExitWindowHandles = Driver.WindowHandles;

            foreach (string Handle in ExitWindowHandles)
            {
                if (Handle != OriginalHandle)
                {
                    ExitDateHandle = Handle; break;
                }
            }

            Driver.SwitchTo().Window(ExitDateHandle);

            //Click the year descend button twice to get the year to be 2014 for exit date, click on the 1st to get 02/01/2014 for exit date
            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]")).Click();
            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]")).Click();

            SelectElement ExitMonth = new SelectElement(Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[1]/select")));
            ExitMonth.SelectByText("February");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[7]/font/a")).Click();

            //Switch back to original frame in order to click the calculate button
            Driver.SwitchTo().Window(OriginalHandle);

            //Click calculate button
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            //Check cost is equal to $ 270.00
            try
            {
                Assert.AreEqual("$ 270.00", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b")).Text);
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            //Check duration of stay is (0 Days, 1 Hours, 0 Minutes)
            try
            {
                Assert.AreEqual("(31 Days, 0 Hours, 0 Minutes)", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b")).Text.Trim());
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }
            

            // Quit Chrome
            Driver.Quit();
        }
        [TestMethod]
        public void CheckParkCostandTimeCase3()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            // Select Short-term Parking from Lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("STP");

            //01/02/2014 In Entry Date
            Driver.FindElement(By.Id("EntryDate")).Clear();
            Driver.FindElement(By.Id("EntryDate")).SendKeys("01/02/2014");

            //01/01/2014 In Exit Date
            Driver.FindElement(By.Id("ExitDate")).Clear();
            Driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Click calculate button, check error message appears
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span/font/b")).Text.Trim());       
            }

            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }
            
            // Quit Chrome
            Driver.Quit();
        }
        [TestMethod]
        public void CheckParkCostandTimeCase4()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            // Select Valet Parking from Lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("VP");

            // Enter 10:00 in Entry Time, select PM in radio button
            Driver.FindElement(By.Id("EntryTime")).Clear();
            Driver.FindElement(By.Id("EntryTime")).SendKeys("10:00");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td[2]/font/input[3]")).Click();

            //Enter date as 01/01/2016 In Entry Date
            Driver.FindElement(By.Id("EntryDate")).Clear();
            Driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2016");

            // Enter 11:00 AM in Exit Time 
            Driver.FindElement(By.Id("ExitTime")).Clear();
            Driver.FindElement(By.Id("ExitTime")).SendKeys("11:00");

            //Enter date as 01/01/2016 In Exit Date
            Driver.FindElement(By.Id("ExitDate")).Clear();
            Driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2016");

            //Click calculate button, Check error message appears
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span/font/b")).Text.Trim());
               
            }

            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            // Quit Chrome
            Driver.Quit();
        }

        [TestMethod]
        public void CheckParkCostandTimeCase5()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            // Select Long Term Garage Parking from Lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("LTG");

            // Enter 9:00 AM in Entry Time
            Driver.FindElement(By.Id("EntryTime")).Clear();
            Driver.FindElement(By.Id("EntryTime")).SendKeys("9:00");

            //Enter date as 1999 In Entry Date
            Driver.FindElement(By.Id("EntryDate")).Clear();
            Driver.FindElement(By.Id("EntryDate")).SendKeys("1999");

            // Enter 3:00 PM in Exit Time 
            Driver.FindElement(By.Id("ExitTime")).Clear();
            Driver.FindElement(By.Id("ExitTime")).SendKeys("3:00");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td[2]/font/input[3]")).Click();

            //Enter date as 2000 In Exit Date
            Driver.FindElement(By.Id("ExitDate")).Clear();
            Driver.FindElement(By.Id("ExitDate")).SendKeys("2000");

            //Click calculate button, check for error
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! ENTER A CORRECTLY FORMATTED DATE", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b")).Text.Trim());
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }
           
            // Quit Chrome
            Driver.Quit();
        }

        [TestMethod]
        public void CheckParkCostandTimeCase6()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            // Select Economy Parking from Lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("EP");

            // Enter 12:00 PM in Entry Time, select PM in radio button
            Driver.FindElement(By.Id("EntryTime")).Clear();
            Driver.FindElement(By.Id("EntryTime")).SendKeys("12:00");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td[2]/font/input[3]")).Click();

            //Enter date as 01/01/2016 In Entry Date
            Driver.FindElement(By.Id("EntryDate")).Clear();
            Driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2016");

            // Enter 3:00PM in Exit Time, select PM in radio button
            Driver.FindElement(By.Id("ExitTime")).Clear();
            Driver.FindElement(By.Id("ExitTime")).SendKeys("3:00");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td[2]/font/input[3]")).Click();

            //Leave Exit date as MM/DD/YYYY

            //Click calculate button, check error message
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! ENTER A CORRECTLY FORMATTED DATE", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span/font/b")).Text.Trim());
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            // Quit Chrome
            Driver.Quit();
        }

        [TestMethod]
        public void CheckParkCostandTimeCase7()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            String OriginalHandle = Driver.CurrentWindowHandle;

            // Select Long Term Surface Parking from Lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("LTS");

            // Enter 4:00 PM in Entry Time, select PM in radio button
            Driver.FindElement(By.Id("EntryTime")).Clear();
            Driver.FindElement(By.Id("EntryTime")).SendKeys("4:00");

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td[2]/font/input[3]")).Click();

            //Click calendar icon to enter entry date
            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td[2]/font/a/img")).Click();
            Thread.Sleep(2000);

            //Switch Driver to new calendar frame for entry date 01/02/2016
            string EntryDateHandle = string.Empty;
            ReadOnlyCollection<string> EntryWindowHandles = Driver.WindowHandles;

            foreach (string Handle in EntryWindowHandles)
            {
                if (Handle != OriginalHandle)
                {
                    EntryDateHandle = Handle; break;
                }
            }

            Driver.SwitchTo().Window(EntryDateHandle);

            Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[7]/font/a")).Click();

            Driver.SwitchTo().Window(OriginalHandle);

            //Check date is formatted correctly from calendar
            try
            {
                String DateValue = Driver.FindElement(By.Id("EntryDate")).GetAttribute("value");
                Assert.AreEqual("1/2/2016", DateValue);
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            // Enter 3:00 AM in Exit Time
            Driver.FindElement(By.Id("ExitTime")).Clear();
            Driver.FindElement(By.Id("ExitTime")).SendKeys("3:00");


            //Enter date as 01/07/2016 In Exit Date
            Driver.FindElement(By.Id("ExitDate")).Clear();
            Driver.FindElement(By.Id("ExitDate")).SendKeys("01/07/2016");

            //Click calculate button, Check cost is equal to $ 50.00, Check duration of stay is (4 Days, 11 Hours, 0 Minutes)
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("$ 50.00", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b")).Text);
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            try
            {
                Assert.AreEqual("(4 Days, 11 Hours, 0 Minutes)", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b")).Text.Trim());
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            // Quit Chrome
            Driver.Quit();
        }
        [TestMethod]
        public void CheckParkCostandTimeCase8()
        {
            IWebDriver Driver = new ChromeDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Chrome Driver");
            DateTime Time = DateTime.Now;

            Driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");
            Thread.Sleep(2000);

            // Select Long Term Garage Parking from lot drop down menu
            SelectElement Lot = new SelectElement(Driver.FindElement(By.Id("Lot")));
            Lot.SelectByValue("LTG");

            // Enter right now for the time
            Driver.FindElement(By.Id("EntryTime")).Clear();
            Driver.FindElement(By.Id("EntryTime")).SendKeys(Time.ToString(@"hh:mm"));

            if (Time.ToString(@"tt").Equals("PM"))
            {
                Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td[2]/font/input[3]")).Click();
            }

            //Enter today In Entry Date
            Driver.FindElement(By.Id("EntryDate")).Clear();
            Driver.FindElement(By.Id("EntryDate")).SendKeys(Time.ToString(@"MM\/ dd\/ yyyy"));

            Time = Time.AddHours(1);

            // Enter an hour from now in exit time
            Driver.FindElement(By.Id("ExitTime")).Clear();
            Driver.FindElement(By.Id("ExitTime")).SendKeys(Time.ToString(@"hh:mm"));

            if (Time.ToString(@"tt").Equals("PM"))
            {
                Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td[2]/font/input[3]")).Click();
            }
            
            //Enter tomorrow in exit date
            Driver.FindElement(By.Id("ExitDate")).Clear();
            Driver.FindElement(By.Id("ExitDate")).SendKeys(Time.AddDays(1).ToString(@"MM\/ dd\/ yyyy"));

            //Click calculate button, Check cost is equal to $ 14.00, Check duration of stay is (1 Days, 1 Hours, 0 Minutes)
            Driver.FindElement(By.XPath("/html/body/form/input[2]")).Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("$ 14.00", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b")).Text);
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            try
            {
                Assert.AreEqual("(1 Days, 1 Hours, 0 Minutes)", Driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b")).Text.Trim());
            }
            catch (Exception ex)
            {
                Driver.Quit();
                Assert.Fail();
            }

            // Quit Chrome
            Driver.Quit();
        }

    }

}
