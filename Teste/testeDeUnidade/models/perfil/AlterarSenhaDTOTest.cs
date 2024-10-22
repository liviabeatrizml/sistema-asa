namespace testeDeUnidade.models.perfil
{
    using NUnit.Framework;
    using Back_end.Models;

    public class AlterarSenhaDTOTests
    {
        private AlterarSenhaDto alterarSenhaDTO;

        [SetUp]
        public void Setup()
        {
            alterarSenhaDTO = new AlterarSenhaDto
            {
                Email = "livia.lima30332@ufersa.edu.br",
                SenhaAtual = "Livia10.",
                NovaSenha = "Beatriz10."
            };
        }

        [Test]
        public void TestGetSetEmail()
        {
            alterarSenhaDTO.Email = "livia.lima30332@alunos.ufersa.edu.br";
            Assert.That(alterarSenhaDTO.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public void TestGetSetSenhaAtual()
        {
            alterarSenhaDTO.SenhaAtual = "Livia10.";
            Assert.That(alterarSenhaDTO.SenhaAtual, Is.EqualTo("Livia10."));
        }

        [Test]
        public void TestGetSetNovaSenha()
        {
            alterarSenhaDTO.NovaSenha = "Livia09.";
            Assert.That(alterarSenhaDTO.NovaSenha, Is.EqualTo("Livia09."));
        }
    }
}
