namespace testeDeUnidade.models.discente
{
    using NUnit.Framework;
    using Back_end.Models;

    public class DiscenteTests
    {
        private Discente discente;

        [SetUp]
        public void Setup()
        {
            discente = new Discente
            {
                IdDiscente = 1,
                Nome = "Livia",
                Email = "livia.lima@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Salt = "Livia10.",
                Matricula = 2021010871,
                Telefone = "40028922",
                Curso = "Engenharia"
            };
        }

        [Test]
        public void TestGetSetIdDiscente()
        {
            discente.IdDiscente = 1; 
            Assert.That(discente.IdDiscente, Is.EqualTo(1));
        }

        [Test]
        public void TestGetSetNome()
        {
            discente.Nome = "Livia";
            Assert.That(discente.Nome, Is.EqualTo("Livia"));
        }

        [Test]
        public void TestGetSetEmail()
        {
            discente.Email = "livia.lima30332@alunos.ufersa.edu.br";
            Assert.That(discente.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public void TestGetSetSenha()
        {
            discente.Senha = "Livia10.";
            Assert.That(discente.Senha, Is.EqualTo("Livia10."));
        }

        // ISSO DAQUI É ALGUMA COISA DO BANCO DE DADOS PARA A LÓGICA DE LÁ
        [Test]
        public void TestGetSetSalt()
        {
            discente.Salt = "Livia10.";
            Assert.That(discente.Salt, Is.EqualTo("Livia10."));
        }

        [Test]
        public void TestGetSetMatricula()
        {
            discente.Matricula = 2021010871; 
            Assert.That(discente.Matricula, Is.EqualTo(2021010871));
        }

        [Test]
        public void TestGetSetTelefone()
        {
            discente.Telefone = "40028922";
            Assert.That(discente.Telefone, Is.EqualTo("40028922"));
        }

        [Test]
        public void TestGetSetCurso()
        {
            discente.Curso = "Engenharia De Software";
            Assert.That(discente.Curso, Is.EqualTo("Engenharia De Software"));
        }
    }
}
