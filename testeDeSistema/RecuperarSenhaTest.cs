namespace testeDeSistema;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;

[TestFixture]
public class RecuperarSenhaTest {
  private IWebDriver driver;
  public IDictionary<string, object> vars {get; private set;}
  private IJavaScriptExecutor js;
  [SetUp]
  public void SetUp() {
    driver = new ChromeDriver();
    js = (IJavaScriptExecutor)driver;
    vars = new Dictionary<string, object>();
    driver.Manage().Window.Maximize();
  }
  [TearDown]
  protected void TearDown() {
    driver?.Quit();
    driver?.Dispose();
  }

  // CASO DE SUCESSO FLUXO PERFEITO
  [Test]
  public void recuperarSenha() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.LinkText("Esqueceu sua senha?")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO EMAIL SEM REGISTRO
  [Test]
  public void recuperarSenhaEmailSemRegistro() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.LinkText("Esqueceu sua senha?")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livinhaVasco@ufersa.edu.br");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO EMAIL VAZIO
  [Test]
  public void recuperarSenhaEmailVazio() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.LinkText("Esqueceu sua senha?")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O campo Email é obrigatório."));
    Thread.Sleep(2000);

    driver.Close();
  }

}
