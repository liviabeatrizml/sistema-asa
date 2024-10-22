namespace testeDeUnidade.models.profissional
{
    using NUnit.Framework;
    using Back_end.Models;

    public class RegistrarProfissionalTests
    {
        private RegistrarProfissional registrarProfissional;

        [SetUp]
        public void Setup()
        {
            registrarProfissional = new RegistrarProfissional
            {
                Nome = "Eriky",
                Email = "eriky.abreu@alunos.ufersa.edu.br",
                Senha = "Eriky10.",
                AreaAtuacao = "Pedagogo",
                descricao = "Servico prestado por Eriky Abreu"
            };
        }

        [Test]
        public void RegistrarProfissionalGetSetNome()
        {
            registrarProfissional.Nome = "Geísa"; 
            Assert.That(registrarProfissional.Nome, Is.EqualTo("Geísa"));
        }

        [Test]
        public void RegistrarProfissionalGetSetEmail()
        {
            registrarProfissional.Email = "geisa.gabriel@ufersa.edu.br"; 
            Assert.That(registrarProfissional.Email, Is.EqualTo("geisa.gabriel@ufersa.edu.br"));
        }

        [Test]
        public void RegistrarProfissionalGetSetSenha()
        {
            registrarProfissional.Senha = "Geisa07."; 
            Assert.That(registrarProfissional.Senha, Is.EqualTo("Geisa07."));
        }

        [Test]
        public void RegistrarProfissionalGetSetAreaAtuacao()
        {
            registrarProfissional.AreaAtuacao = "Pedagoga"; 
            Assert.That(registrarProfissional.AreaAtuacao, Is.EqualTo("Pedagoga"));
        }

        [Test]
        public void RegistrarProfissionalGetSetDescricao()
        {
            registrarProfissional.descricao = "Servico prestado por Geisa Morais"; 
            Assert.That(registrarProfissional.descricao, Is.EqualTo("Servico prestado por Geisa Morais"));
        }
    }
}
