namespace testeDeUnidade.models.discente
{
    using NUnit.Framework;
    using Back_end.Models;

    public class DiscenteDTOTests
    {
        private DiscenteDto discenteDTO;

        [SetUp]
        public void Setup()
        {
            discenteDTO = new DiscenteDto();
        }

        [Test]
        public void TestGetSetNome()
        {
            discenteDTO.Nome = "Livia";
            Assert.That(discenteDTO.Nome, Is.EqualTo("Livia"));
        }

        [Test]
        public void TestGetSetEmail()
        {
            discenteDTO.Email = "livia.lima30332@alunos.ufersa.edu.br";
            Assert.That(discenteDTO.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public void TestGetSetSenha()
        {
            discenteDTO.Senha = "Livia10.";
            Assert.That(discenteDTO.Senha, Is.EqualTo("Livia10."));
        }

        [Test]
        public void TestGetSetMatricula()
        {
            discenteDTO.Matricula = 2021010871; 
            Assert.That(discenteDTO.Matricula, Is.EqualTo(2021010871));
        }

        [Test]
        public void TestGetSetTelefone()
        {
            discenteDTO.Telefone = "40028922";
            Assert.That(discenteDTO.Telefone, Is.EqualTo("40028922"));
        }

        [Test]
        public void TestGetSetCurso()
        {
            discenteDTO.Curso = "Engenharia De Software";
            Assert.That(discenteDTO.Curso, Is.EqualTo("Engenharia De Software"));
        }
    }
}
