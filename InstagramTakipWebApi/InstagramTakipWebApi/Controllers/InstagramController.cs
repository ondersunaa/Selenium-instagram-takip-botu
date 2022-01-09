using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstagramTakipWebApi.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InstagramTakipWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstagramController : ControllerBase
    {
        [HttpPost]
        public IActionResult FollowPageUsers(User user)
        {

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.instagram.com");
            Thread.Sleep(2000);
            IWebElement userName = driver.FindElement(By.Name("username"));
            IWebElement password = driver.FindElement(By.Name("password"));
            IWebElement btnLogin = driver.FindElement(By.CssSelector(".sqdOP.L3NKy.y3zKF"));

            try
            {
                userName.SendKeys(user.UserName);
                password.SendKeys(user.Password);
                btnLogin.Click();

                //IWebElement passwordMessage = driver.FindElement(By.Id("slfErrorAlert"));



                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(user.FollowPage);
                Thread.Sleep(4000);

                IWebElement searchField = driver.FindElement(By.CssSelector("#react-root > section > main > div > header > section > ul > li:nth-child(2) > a"));
                searchField.Click();

                Thread.Sleep(5000);

                string jsCommand = " sayfa = document.querySelector('.isgrP'); " +
                                   " sayfa.scrollTo(0,sayfa.scrollHeight); " +
                                   " var sayfaSonu = sayfa.scrollHeight; " +
                                   " return sayfaSonu ";
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                var sayfaSonu = Convert.ToInt32(js.ExecuteScript(jsCommand));
                int followCount = 0;
                while (true)
                {
                    var son = sayfaSonu;
                    Thread.Sleep(2000);
                    sayfaSonu = Convert.ToInt32(js.ExecuteScript(jsCommand));
                    IReadOnlyCollection<IWebElement> followers =
                        driver.FindElements(By.CssSelector(".sqdOP.L3NKy.y3zKF"));
                    foreach (IWebElement follow in followers)
                    {
                        if (son == sayfaSonu)
                        {
                            break;
                        }

                        int sayac = 0;
                        if (follow.Text == "Takip Et")
                        {
                            follow.Click();
                            followCount++;
                            sayac++;
                            if (sayac == 10)
                            {
                                Thread.Sleep(50000);
                                sayac = 0;
                            }

                        }

                        if (followCount == user.FollowCount)
                        {
                            return Ok();
                        }

                    }

                    if (son == sayfaSonu)
                    {
                        return Ok();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("başarılı");
        }
    }
}
