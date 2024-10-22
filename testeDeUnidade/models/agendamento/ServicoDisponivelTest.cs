namespace testeDeUnidade.models.agendamento
{
    using NUnit.Framework;
    using Back_end.Models;

    public class ServicoDisponivelTests
    {
        private ServicoDisponivel servicoDisponivel;

        [SetUp]
        public void Setup()
        {
            servicoDisponivel = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Psicologo",
                TipoAtendimento = "Consulta"
            };
        }

        [Test]
        public void TestGetSetIdServico()
        {
            servicoDisponivel.IdServico = 4;
            Assert.That(servicoDisponivel.IdServico, Is.EqualTo(4));
        }

        [Test]
        public void TestGetSetTipo()
        {
            servicoDisponivel.Tipo = "Psicologo";
            Assert.That(servicoDisponivel.Tipo, Is.EqualTo("Psicologo"));
        }

        [Test]
        public void TestGetSetTipoAtendimento()
        {
            servicoDisponivel.TipoAtendimento = "Consulta";
            Assert.That(servicoDisponivel.TipoAtendimento, Is.EqualTo("Consulta"));
        }
    }
}
