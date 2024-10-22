namespace testeDeUnidade.models.agendamento
{
    using NUnit.Framework;
    using Back_end.Models;
    using Back_end.Dtos;

    public class SolicitarAgendamentoDTOTests
    {
        private SolicitarAgendamentoDto solicitarAgendamentoDto;

        [SetUp]
        public void Setup()
        {
            solicitarAgendamentoDto = new SolicitarAgendamentoDto
            {
                DiscenteId = 1,
                ProfissionalId = 1,
                ServicoId = 1,
                HorarioId = 123
            };
        }

        [Test]
        public void SolicitarAgendamentoDTOGetSetDiscenteId()
        {
            solicitarAgendamentoDto.DiscenteId = 1; 
            Assert.That(solicitarAgendamentoDto.DiscenteId, Is.EqualTo(1));
        }

        [Test]
        public void SolicitarAgendamentoDTOGetSetProfissionalId()
        {
            solicitarAgendamentoDto.ProfissionalId = 12; 
            Assert.That(solicitarAgendamentoDto.ProfissionalId, Is.EqualTo(12));
        }

        [Test]
        public void SolicitarAgendamentoDTOGetSetServicoId()
        {
            solicitarAgendamentoDto.ServicoId = 3; 
            Assert.That(solicitarAgendamentoDto.ServicoId, Is.EqualTo(3));
        }

        [Test]
        public void SolicitarAgendamentoDTOGetSetHorarioId()
        {
            solicitarAgendamentoDto.HorarioId = 43221; 
            Assert.That(solicitarAgendamentoDto.HorarioId, Is.EqualTo(43221));
        }

        [Test]
        public void SolicitarAgendamentoDTOGetSetData()
        {
            var data = new DateTime(2024, 10, 11);
            solicitarAgendamentoDto.Data = data;
            Assert.That(solicitarAgendamentoDto.Data, Is.EqualTo(data));
        }
    }
}
