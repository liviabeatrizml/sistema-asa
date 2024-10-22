namespace testeDeUnidade.models.perfil
{
    using NUnit.Framework;
    using Back_end.Models;

    public class AtualizarPerfilDTOTests
    {
        private AtualizarPerfilDto atualizarPerfilDTO;

        [SetUp]
        public void Setup()
        {
            atualizarPerfilDTO = new AtualizarPerfilDto
            {
                Email = "livia@alunos.ufersa.edu.br",
                Nome = "Livia",
                Telefone = "40028922",
                Matricula = 2021010871,
                Curso = "Engenharia"
            };
        }

        [Test]
        public void TestGetSetEmail()
        {
            atualizarPerfilDTO.Email = "livia.lima30332@alunos.ufersa.edu.br";
            Assert.That(atualizarPerfilDTO.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public void TestGetSetNome()
        {
            atualizarPerfilDTO.Nome = "Livia Beatriz";
            Assert.That(atualizarPerfilDTO.Nome, Is.EqualTo("Livia Beatriz"));
        }

        [Test]
        public void TestGetSetTelefone()
        {
            atualizarPerfilDTO.Telefone = "998429564";
            Assert.That(atualizarPerfilDTO.Telefone, Is.EqualTo("998429564"));
        }

        [Test]
        public void TestGetSetMatricula()
        {
            atualizarPerfilDTO.Matricula = 2021010871;
            Assert.That(atualizarPerfilDTO.Matricula, Is.EqualTo(2021010871));
        }

        [Test]
        public void TestGetSetCurso()
        {
            atualizarPerfilDTO.Curso = "Engenharia";
            Assert.That(atualizarPerfilDTO.Curso, Is.EqualTo("Engenharia"));
        }
    }
}
