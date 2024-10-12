namespace testeDeUnidade.models.profissional
{
    using NUnit.Framework;
    using Back_end.Models;

    public class ProfissionalTests
    {
        private Profissional profissional;

        [SetUp]
        public void Setup()
        {
            profissional = new Profissional();
        }

        [Test]
        public void TestGetSetIdProfissional()
        {
            profissional.IdProfissional = 1; 
            Assert.That(profissional.IdProfissional, Is.EqualTo(1));
        }

        [Test]
        public void TestGetSetNome()
        {
            profissional.Nome = "Geísa";
            Assert.That(profissional.Nome, Is.EqualTo("Geísa"));
        }

        [Test]
        public void TestGetSetEmail()
        {
            profissional.Email = "geisa.gabriel@ufersa.edu.br";
            Assert.That(profissional.Email, Is.EqualTo("geisa.gabriel@ufersa.edu.br"));
        }

        [Test]
        public void TestGetSetSenha()
        {
            profissional.Senha = "Geisa07.";
            Assert.That(profissional.Senha, Is.EqualTo("Geisa07."));
        }

        // ISSO DAQUI É ALGUMA COISA DO BANCO DE DADOS PARA A LÓGICA DE LÁ -- Não sei direito
        [Test]
        public void TestGetSetSalt()
        {
            profissional.Salt = "Geisa07.";
            Assert.That(profissional.Salt, Is.EqualTo("Geisa07."));
        }
    }
}
