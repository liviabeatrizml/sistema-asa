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
public class CadastrarTest {
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
  public void cadastrar() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    // Faço uma rolagem para poder aparecer o botão
    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010732");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO - EMAIL SEM DOMINIO
  [Test]
  public void cadastrarEmailSemDominio() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@gmail.com");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    Thread.Sleep(1000);

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O Email deve ser do domínio @ufersa.edu.br."));

    driver.Close();
  }

  // CASO DE FRACASSO - EMAIL SEM ARROBA
  [Test]
  public void cadastrarEmailSemArroba() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("liviagmail.com");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    Thread.Sleep(1000);

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O Email deve ser do domínio @ufersa.edu.br."));

    driver.Close();
  }

  // CASO DE FRACASSO - EMAIL COM TRÊS CARACTERE
  [Test]
  public void cadastrarEmailComTresCaractere() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("uai@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.Close();
  }

  // CASO DE FRACASSO - EMAIL MENOR QUE TRÊS CARACTERE
  [Test]
  public void cadastrarEmailMenorQueTresCaractere() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("ui@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.Close();
  }

  // CASO DE FRACASSO - SENHA MENOR QUE OITO CARACTERE
  [Test]
  public void cadastrarSenhaMenorQueOitoCaractere() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia1.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.Close();
  }

  // CASO DE FRACASSO - SENHA MENOR QUE OITO
  [Test]
  public void cadastrarSenhaMenorQueOito() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("1234567");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.Close();
  }

  // CASO DE FRACASSO - SENHA APENAS NUMEROS
  [Test]
  public void cadastrarSenhaApenasNumeros() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("12345678");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.Close();
  }

  // CASO DE FRACASSO - SENHA APENAS LETRAS
  [Test]
  public void cadastrarSenhaApenasLetras() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("ABCDEFGH");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    Thread.Sleep(1000);

    driver.Close();
  }

   // CASO DE FRACASSO NOME UM CARACTERE
  [Test]
  public void cadastrarNomeApenasUmCaractere() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("L");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010732");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

   // CASO DE FRACASSO NOME COM 200 CARACTERE
  [Test]
  public void cadastrarNomeMaiorQue200Caractere() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    // 220
    driver.FindElement(By.Id("nome")).SendKeys("Antonio_Cauê_Oliveira_Morais_Cristiana_de_Paulo_Eriky_Abreu_Veloso_Francisco_Renan_Leite_da_Costa_Geísa_Morais_Gabriel_Lavinia_Dantas_de_Mesquita_Lívia_Beatriz_Maia_de_Lima_Maria_Lanuza_dos_Santos_Silva_Tiago_Amaro_Nunes");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010732");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

   // CASO DE FRACASSO NOME VAZIO
  [Test]
  public void cadastrarNomeVazio() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010732");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O campo Nome é obrigatório."));

    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

   // CASO DE FRACASSO SOBRENOME UM CARACTERE
  [Test]
  public void cadastrarSobrenomeApenasUmCaractere() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("M");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010732");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

   // CASO DE FRACASSO SOBRENOME COM 200 CARACTERE
  [Test]
  public void cadastrarSobrenomeMaiorQue200Caractere() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    // 220
    driver.FindElement(By.Id("sobrenome")).SendKeys("Antonio_Cauê_Oliveira_Morais_Cristiana_de_Paulo_Eriky_Abreu_Veloso_Francisco_Renan_Leite_da_Costa_Geísa_Morais_Gabriel_Lavinia_Dantas_de_Mesquita_Lívia_Beatriz_Maia_de_Lima_Maria_Lanuza_dos_Santos_Silva_Tiago_Amaro_Nunes");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010732");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

   // CASO DE FRACASSO SOBRENOME VAZIO
  [Test]
  public void cadastrarSobrenomeVazio() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010732");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O campo Sobrenome é obrigatório."));
    Thread.Sleep(2000);

    driver.Close();
  }

  // CASO DE FRACASSO MATRICULA MAIOR QUE 10
  [Test]
  public void cadastrarMatriculaMaiorQue10() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("20210108711");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO MATRICULA MENOR QUE 10
  [Test]
  public void cadastrarMatriculaMenorQue10() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("202101087");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("A Matrícula deve ter exatamente 10 dígitos."));
    
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO MATRICULA APENAS LETRAS
  [Test]
  public void cadastrarMatriculaApenasLetras() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("ABCDefgh");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("A Matrícula deve ter exatamente 10 dígitos."));
    Thread.Sleep(2000);
    driver.Close();
  }

  // CASO DE FRACASSO MATRICULA VAZIO
  [Test]
  public void cadastrarMatriculaVazio() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-70");
    driver.FindElement(By.CssSelector(".btn")).Click();

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O campo Matrícula é obrigatório."));

    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Sair")).Click();
    driver.Close();
  }

  // CASO DE FRACASSO CPF FORMATO INVÁLIDO
  [Test]
  public void cadastrarCPFFormatoInvalido() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010871");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424");

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O formato deve ser 000.000.000-00."));

    driver.FindElement(By.CssSelector(".btn")).Click();
    Thread.Sleep(2000);
    driver.Close();
  }

  // CASO DE FRACASSO CPF COM LETRAS
  [Test]
  public void cadastrarCPFComLetras() {
    driver.Navigate().GoToUrl("http://localhost:5156/");
    Thread.Sleep(2000);
    driver.FindElement(By.LinkText("Cadastrar")).Click();
    driver.FindElement(By.Id("email")).Click();
    driver.FindElement(By.Id("email")).SendKeys("livia@ufersa.edu.br");
    driver.FindElement(By.Id("password")).Click();
    driver.FindElement(By.Id("password")).SendKeys("Livia10.");

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")));
    Thread.Sleep(1000);

    driver.FindElement(By.XPath("//button[text()='Aceite e cadastre-se']")).Click();

    driver.FindElement(By.Id("nome")).Click();
    driver.FindElement(By.Id("nome")).SendKeys("Livia");
    driver.FindElement(By.Id("sobrenome")).SendKeys("Maia");
    driver.FindElement(By.Id("matricula")).SendKeys("2021010871");
    driver.FindElement(By.Id("cpf")).SendKeys("095.111.424-AB");
    driver.FindElement(By.CssSelector(".btn")).Click();

    Assert.That(driver.FindElement(By.ClassName("validation-message")).Text, Is.EqualTo("O formato deve ser 000.000.000-00."));
    
    Thread.Sleep(2000);
    driver.Close();
  }
}