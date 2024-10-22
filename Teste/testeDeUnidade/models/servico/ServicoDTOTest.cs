namespace testeDeUnidade.models.servico
{
    using NUnit.Framework;
    using Back_end.Models;

    public class ServicoDTOTests
    {
        private ServicoDto servicoDTO;

        [SetUp]
        public void Setup()
        {
            servicoDTO = new ServicoDto
            {
                Tipo = "Psicologo",
                TipoAtendimento = "Consulta"
            };
        }

        [Test]
        public void TestGetSetTipo()
        {
            servicoDTO.Tipo = "Psicologo";
            Assert.That(servicoDTO.Tipo, Is.EqualTo("Psicologo"));
        }

        [Test]
        public void TestGetSetTipoAtendimento()
        {
            servicoDTO.TipoAtendimento = "Retorno";
            Assert.That(servicoDTO.TipoAtendimento, Is.EqualTo("Retorno"));
        }
    }
}
