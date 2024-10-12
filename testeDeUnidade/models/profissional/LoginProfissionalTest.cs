namespace testeDeUnidade.models.profissional
{
    using NUnit.Framework;
    using Back_end.Models;

    public class LoginProfissionalTests
    {
        private LoginProfissional login;

        [SetUp]
        public void Setup()
        {
            login = new LoginProfissional();
        }

        [Test]
        public void LoginProfissionalGetSetEmail()
        {
            login.Email = "geisa.gabriel@ufersa.edu.br"; 
            Assert.That(login.Email, Is.EqualTo("geisa.gabriel@ufersa.edu.br"));
        }

        [Test]
        public void LoginProfissionalGetSetSenha()
        {
            login.Senha = "Geisa07."; 
            Assert.That(login.Senha, Is.EqualTo("Geisa07."));
        }
    }
}
