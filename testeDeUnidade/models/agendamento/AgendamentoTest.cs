namespace testeDeUnidade.models.agendamento
{
    using NUnit.Framework;
    using Back_end.Models;

    public class AgendamentoTests
    {
        private Agendamento agendamento;

        [SetUp]
        public void Setup()
        {
            agendamento = new Agendamento
            {
                IdAgendamento = 1,
                DiscenteId = 1,
                ProfissionalId = 1,
                ServicoId = 1,
                HorarioId = 1,
                Status = "Concluido"
            };
        }

        [Test]
        public void TestGetSetIdAgendamento()
        {
            agendamento.IdAgendamento = 1;
            Assert.That(agendamento.IdAgendamento, Is.EqualTo(1));
        }

        [Test]
        public void TestGetSetData()
        {
            var data = new DateTime(2024, 10, 11);
            agendamento.Data = data;
            Assert.That(agendamento.Data, Is.EqualTo(data));
        }

        [Test]
        public void TestGetSetDiscenteId()
        {
            agendamento.DiscenteId = 12;
            Assert.That(agendamento.DiscenteId, Is.EqualTo(12));
        }

        [Test]
        public void TestGetSetDiscente()
        {
            var discente = new Discente();
            agendamento.Discente = discente;
            Assert.That(agendamento.Discente, Is.EqualTo(discente));
        }

        [Test]
        public void TestGetSetProfissionalId()
        {
            agendamento.ProfissionalId = 2;
            Assert.That(agendamento.ProfissionalId, Is.EqualTo(2));
        }

        [Test]
        public void TestGetSetProfissional()
        {
            var profissional = new Profissional();
            agendamento.Profissional = profissional;
            Assert.That(agendamento.Profissional, Is.EqualTo(profissional));
        }

        [Test]
        public void TestGetSetServicoId()
        {
            agendamento.ServicoId = 3;
            Assert.That(agendamento.ServicoId, Is.EqualTo(3));
        }

        [Test]
        public void TestGetSetHorarioId()
        {
            agendamento.HorarioId = 123;
            Assert.That(agendamento.HorarioId, Is.EqualTo(123));
        }

        [Test]
        public void TestGetSetStatus()
        {
            agendamento.Status = "Cancelado";
            Assert.That(agendamento.Status, Is.EqualTo("Cancelado"));
        }
    }
}
