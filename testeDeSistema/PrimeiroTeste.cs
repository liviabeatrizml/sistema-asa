namespace testeDeSistema;

using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

[TestFixture]
public class PrimeiroTesteTest{
    private IWebDriver driver;
    public IDictionary<string, object> vars { get; private set; }
    private IJavaScriptExecutor js;

    [SetUp]
    public void SetUp(){
        driver = new ChromeDriver();
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }

    [TearDown]
    public void TearDown(){
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void primeiroTeste(){
        driver.Navigate().GoToUrl("http://localhost:5156/");
        Thread.Sleep(1000);
        driver.FindElement(By.LinkText("Entrar")).Click();

        string currentUrl = driver.Url;
        Assert.That(currentUrl, Is.EqualTo("http://localhost:5156/entrar"));
    }
}