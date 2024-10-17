namespace testeDeUnidade.models.agendamento
{
    using NUnit.Framework;
    using Back_end.Models;

    public class HorarioDisponivelTests
    {
        private HorarioDisponivel horarioDisponivel;

        [SetUp]
        public void Setup()
        {
            horarioDisponivel = new HorarioDisponivel();
        }

        [Test]
        public void TestGetSetIdHorario()
        {
            horarioDisponivel.IdHorario = 1;
            Assert.That(horarioDisponivel.IdHorario, Is.EqualTo(1));
        }

        [Test]
        public void TestGetSetHoraInicio()
        {
            var horaInicio = new TimeSpan(9, 0, 0);
            horarioDisponivel.HoraInicio = horaInicio;
            Assert.That(horarioDisponivel.HoraInicio, Is.EqualTo(horaInicio));
        }

        [Test]
        public void TestGetSetHoraFim()
        {
            var horaFim = new TimeSpan(17, 0, 0);
            horarioDisponivel.HoraFim = horaFim;
            Assert.That(horarioDisponivel.HoraFim, Is.EqualTo(horaFim));
        }

        [Test]
        public void TestGetSetDiaDaSemana()
        {
            horarioDisponivel.DiaDaSemana = 1;
            Assert.That(horarioDisponivel.DiaDaSemana, Is.EqualTo(1));
        }

        [Test]
        public void TestGetSetProfissionalId()
        {
            horarioDisponivel.ProfissionalId = 2;
            Assert.That(horarioDisponivel.ProfissionalId, Is.EqualTo(2));
        }

        [Test]
        public void TestGetSetProfissional()
        {
            var profissional = new Profissional();
            horarioDisponivel.Profissional = profissional;
            Assert.That(horarioDisponivel.Profissional, Is.EqualTo(profissional));
        }
    }
}
