namespace testeDeUnidade.models.agendamento
{
    using NUnit.Framework;
    using Back_end.Models;
    using Back_end.Dtos;

    public class AgendamentoDTOTests
    {
        private AgendamentoDto agendamentoDto;

        [SetUp]
        public void Setup()
        {
            agendamentoDto = new AgendamentoDto
            {
                IdAgendamento = 1,
                Data = new DateTime(2024, 10, 18),
                DiscenteId = 1,
                ProfissionalId = 123,
                ServicoId = 1,
                HorarioId = 1,
                Status = "Concluido",
                HoraInicio = new TimeSpan(9, 0, 0),
                HoraFim = new TimeSpan(10, 0, 0)
            };
        }

        [Test]
        public void AgendamentoDTOGetSetIdAgendamento()
        {
            agendamentoDto.IdAgendamento = 1; 
            Assert.That(agendamentoDto.IdAgendamento, Is.EqualTo(1));
        }

        [Test]
        public void AgendamentoDTOGetSetData()
        {
            var data = new DateTime(2024, 10, 11);
            agendamentoDto.Data = data;
            Assert.That(agendamentoDto.Data, Is.EqualTo(data));
        }

        [Test]
        public void AgendamentoDTOGetSetDiscenteId()
        {
            agendamentoDto.DiscenteId = 12; 
            Assert.That(agendamentoDto.DiscenteId, Is.EqualTo(12));
        }

        [Test]
        public void AgendamentoDTOGetSetProfissionalId()
        {
            agendamentoDto.ProfissionalId = 13; 
            Assert.That(agendamentoDto.ProfissionalId, Is.EqualTo(13));
        }

        [Test]
        public void AgendamentoDTOGetSetServicoId()
        {
            agendamentoDto.ServicoId = 3; 
            Assert.That(agendamentoDto.ServicoId, Is.EqualTo(3));
        }

        [Test]
        public void AgendamentoDTOGetSetHorarioId()
        {
            agendamentoDto.HorarioId = 43221; 
            Assert.That(agendamentoDto.HorarioId, Is.EqualTo(43221));
        }

        [Test]
        public void AgendamentoDTOGetSetStatus()
        {
            agendamentoDto.Status = "Concluido"; 
            Assert.That(agendamentoDto.Status, Is.EqualTo("Concluido"));
        }

        [Test]
        public void AgendamentoDTOGetSetHoraInicio()
        {
            var horaInicio = new TimeSpan(9, 0, 0);
            agendamentoDto.HoraInicio = horaInicio;
            Assert.That(agendamentoDto.HoraInicio, Is.EqualTo(horaInicio));
        }

        [Test]
        public void AgendamentoDTOGetSetHoraFim()
        {
            var horaFim = new TimeSpan(10, 0, 0);
            agendamentoDto.HoraFim = horaFim;
            Assert.That(agendamentoDto.HoraFim, Is.EqualTo(horaFim));
        }
    }
}
