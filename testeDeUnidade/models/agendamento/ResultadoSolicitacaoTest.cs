namespace testeDeUnidade.models.agendamento
{
    using NUnit.Framework;
    using Back_end.Models;

    public class ResultadoSolicitacaoTests
    {
        private ResultadoSolicitacao resultadoSolicitacao;

        [SetUp]
        public void Setup()
        {
            resultadoSolicitacao = new ResultadoSolicitacao
            {
                Sucesso = true,
                Mensagem = "Agendamento aceito com sucesso",
                Agendamento = new Agendamento()
            };
        }

        [Test]
        public void TestGetSetSucesso()
        {
            resultadoSolicitacao.Sucesso = true;
            Assert.That(resultadoSolicitacao.Sucesso, Is.EqualTo(true));
        }

        [Test]
        public void TestGetSetMensagem()
        {
            resultadoSolicitacao.Mensagem = "Agendamento aceito com sucesso";
            Assert.That(resultadoSolicitacao.Mensagem, Is.EqualTo("Agendamento aceito com sucesso"));
        }

        [Test]
        public void TestGetSetAgendamento()
        {
            var agendamento = new Agendamento();
            resultadoSolicitacao.Agendamento = agendamento;
            Assert.That(resultadoSolicitacao.Agendamento, Is.EqualTo(agendamento));
        }
    }
}
