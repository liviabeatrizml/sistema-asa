namespace testeDeUnidade.models.discente
{
    using NUnit.Framework;
    using Back_end.Models;

    public class ProfissionalDTOTests
    {
        private ProfissionalDto profissionalDTO;

        [SetUp]
        public void Setup()
        {
            profissionalDTO = new ProfissionalDto
            {
                Nome = "Eriky",
                Email = "eriky.abreu@alunos.ufersa.edu.br",
                ServicoId = 1,
                ServicoNome = "Assistente Social",
                Descricao = "Servico prestado por Eriky Abreu"
            };
        }

        [Test]
        public void TestGetSetNome()
        {
            profissionalDTO.Nome = "Eriky";
            Assert.That(profissionalDTO.Nome, Is.EqualTo("Eriky"));
        }

        [Test]
        public void TestGetSetEmail()
        {
            profissionalDTO.Email = "eriky.abreu@alunos.ufersa.edu.br";
            Assert.That(profissionalDTO.Email, Is.EqualTo("eriky.abreu@alunos.ufersa.edu.br"));
        }

        [Test]
        public void TestGetSetServicoId()
        {
            profissionalDTO.ServicoId = 1;
            Assert.That(profissionalDTO.ServicoId, Is.EqualTo(1));
        }

        [Test]
        public void TestGetSetServicoNome()
        {
            profissionalDTO.ServicoNome = "Assistente Social"; 
            Assert.That(profissionalDTO.ServicoNome, Is.EqualTo("Assistente Social"));
        }

        [Test]
        public void TestGetSetDescricao()
        {
            profissionalDTO.Descricao = "Servico prestado por Eriky Abreu"; 
            Assert.That(profissionalDTO.Descricao, Is.EqualTo("Servico prestado por Eriky Abreu"));
        }
    }
}
