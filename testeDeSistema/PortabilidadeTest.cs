namespace testeDeSistema;

using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using NUnit.Framework;

[TestFixture]
public class PostabilidadeTest{
    private IWebDriver driver;
    public IDictionary<string, object> vars { get; private set; }
    private IJavaScriptExecutor js;

    [SetUp]
    public void SetUp(){
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }

    [TearDown]
    public void TearDown(){
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void entrarGoogle(){
        driver = new ChromeDriver();

        driver.Navigate().GoToUrl("http://localhost:5156/");

        Thread.Sleep(1000);
        driver.FindElement(By.LinkText("Entrar")).Click();

        string currentUrl = driver.Url;
        Assert.That(currentUrl, Is.EqualTo("http://localhost:5156/entrar"));
    }

    [Test]
    public void entrarFirefox(){
        driver = new FirefoxDriver();

        driver.Navigate().GoToUrl("http://localhost:5156/");
        Thread.Sleep(1000);
        driver.FindElement(By.LinkText("Entrar")).Click();

        string currentUrl = driver.Url;
        Assert.That(currentUrl, Is.EqualTo("http://localhost:5156/entrar"));
    }

    [Test]
    public void entrarEdge(){
        driver = new EdgeDriver();

        driver.Navigate().GoToUrl("http://localhost:5156/");
        Thread.Sleep(1000);
        driver.FindElement(By.LinkText("Entrar")).Click();

        string currentUrl = driver.Url;
        Assert.That(currentUrl, Is.EqualTo("http://localhost:5156/entrar"));
    }
}