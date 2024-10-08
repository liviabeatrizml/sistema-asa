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
public class EntrarTest {
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

  // CASO DE SUCESSO PERCURSO PERFEITO
  [Test]
  public void entrar() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO EMAIL INVALIDO
  [Test]
  public void entrarComEmailInvalido() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@gmail.com");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO EMAIL NÃO REGISTRADO
  [Test]
  public void entrarComEmailNaoRegistrado() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("vascodagama@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO SENHA INCORRETA
  [Test]
  public void entrarComSenhaIncorreta() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO EMAIL VAZIO
  [Test]
  public void entrarComEmailVazio() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O campo Email é obrigatório."));
    driver.Close();
  }

  // CASO DE FRACASSO SENHA VAZIA
  [Test]
  public void entrarComSenhaVazia() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Entrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("");
    driver.FindElement(By.CssSelector(".btn-primary")).Click();
    
    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O campo Senha é obrigatório."));

    driver.Close();
  }
}
