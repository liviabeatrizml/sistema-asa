namespace testeDeUnidade.models.discente
{
    using NUnit.Framework;
    using Back_end.Models;

    public class RegistrarDiscenteTests
    {
        private RegistrarDiscente registrarDiscente;

        [SetUp]
        public void Setup()
        {
            registrarDiscente = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "40028922",
                Curso = "Engenharia"
            };
        }

        [Test]
        public void RegistrarDiscenteGetSetNome()
        {
            registrarDiscente.Nome = "Livia"; 
            Assert.That(registrarDiscente.Nome, Is.EqualTo("Livia"));
        }

        [Test]
        public void RegistrarDiscenteGetSetEmail()
        {
            registrarDiscente.Email = "livia.lima30332@alunos.ufersa.edu.br"; 
            Assert.That(registrarDiscente.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public void RegistrarDiscenteGetSetSenha()
        {
            registrarDiscente.Senha = "Livia10."; 
            Assert.That(registrarDiscente.Senha, Is.EqualTo("Livia10."));
        }

        [Test]
        public void RegistrarDiscenteGetSetMatricula()
        {
            registrarDiscente.Matricula = 2021010871; 
            Assert.That(registrarDiscente.Matricula, Is.EqualTo(2021010871));
        }

        [Test]
        public void RegistrarDiscenteGetSetTelefone()
        {
            registrarDiscente.Telefone = "40028922"; 
            Assert.That(registrarDiscente.Telefone, Is.EqualTo("40028922"));
        }

        [Test]
        public void RegistrarDiscenteGetSetCurso()
        {
            registrarDiscente.Curso = "Engenharia"; 
            Assert.That(registrarDiscente.Curso, Is.EqualTo("Engenharia"));
        }
    }
}
