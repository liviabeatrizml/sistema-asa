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
            profissional = new Profissional
            {
                IdProfissional = 1,
                Nome = "Eriky",
                Email = "eriky.abreu@alunos.ufersa.edu.br",
                Senha = "Eriky10.",
                Salt = "Eriky10.",
                ServicoId = 1,
                Descricao = "Servico ofertado por Eriky Abreu"
            };
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

        [Test]
        public void TestGetSetSalt()
        {
            profissional.Salt = "Geisa07.";
            Assert.That(profissional.Salt, Is.EqualTo("Geisa07."));
        }

        [Test]
        public void TestGetSetServicoId()
        {
            profissional.ServicoId = 1;
            Assert.That(profissional.ServicoId, Is.EqualTo(1));
        }

        [Test]
        public void TestGetSetServicoDisponivel()
        {
            var servico = new ServicoDisponivel();
            profissional.Servico = servico;
            Assert.That(profissional.Servico, Is.EqualTo(servico));
        }

        [Test]
        public void TestGetSetDescricao()
        {
            profissional.Descricao = "Servico prestado por Eriky Abreu";
            Assert.That(profissional.Descricao, Is.EqualTo("Servico prestado por Eriky Abreu"));
        }
    }
}
