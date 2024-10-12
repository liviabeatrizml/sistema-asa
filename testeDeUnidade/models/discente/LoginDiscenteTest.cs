namespace testeDeUnidade.models.discente
{
    using NUnit.Framework;
    using Back_end.Models;

    public class LoginDiscenteTests
    {
        private LoginDiscente login;

        [SetUp]
        public void Setup()
        {
            login = new LoginDiscente();
        }

        [Test]
        public void LoginDiscenteGetSetEmail()
        {
            login.Email = "livia.lima30332@alunos.ufersa.edu.br"; 
            Assert.That(login.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public void LoginDiscenteGetSetSenha()
        {
            login.Senha = "Livia10."; 
            Assert.That(login.Senha, Is.EqualTo("Livia10."));
        }
    }
}
