using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Data;
using Back_end.Dtos;
using Back_end.Models;
using Back_end.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace testeDeUnidade.services.agendamentoService
{
    [TestFixture]
    public class AgendamentoServiceTests
    {
        private AgendamentoService agendamentoService;
        private DiscenteService discenteService;
        private ApiDbContext context;
        private IConfiguration config;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseMySql("Server=localhost;Database=asa;User=root;Password=admin;",
                          new MySqlServerVersion(new Version(8, 0, 21)))
                .Options;

            context = new ApiDbContext(options);
            var inMemoryCollection = new Dictionary<string, string?>
            {
                { "Jwt:Key", "A2js#h7Kd920Dld9FjSjk39!@kdkfjSJd0298Dk" },
                { "Jwt:Issuer", "http://localhost" }
            };

            config = new ConfigurationBuilder().AddInMemoryCollection(inMemoryCollection).Build();

            agendamentoService = new AgendamentoService(context);
            discenteService = new DiscenteService(context, config);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            context?.Dispose();
        }

        [Test]
        public async Task SolicitarAgendamento()
        {
            var profissionalId = 2;
            var horarioId = 1;
            var discenteId = 1;
            var servicoId = 3;

            context.ServicoDisponivel.Add(new ServicoDisponivel
            {
                IdServico = servicoId,
                Tipo = "Psicologa",
                TipoAtendimento = "Consulta"
            });
            await context.SaveChangesAsync();

            context.Profissionais.Add(new Profissional
            {
                IdProfissional = profissionalId,
                Nome = "Geisa Morais",
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = "Geisa07.",
                Salt = "Geisa07.",
                ServicoId = servicoId,
                Descricao = "Servico prestado por Geisa Morais"
            });
            await context.SaveChangesAsync();

            context.Discentes.Add(new Discente
            {
                IdDiscente = discenteId,
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Matricula = 2021010871,
                Senha = "Renan06.",
                Salt = "Renan06.",
                Telefone = "40028922",
                Curso = "Engenharia"
            });
            await context.SaveChangesAsync();

            context.HorarioDisponivel.Add(new HorarioDisponivel
            {
                IdHorario = horarioId,
                ProfissionalId = profissionalId,
                HoraInicio = new TimeSpan(9, 0, 0),
                HoraFim = new TimeSpan(11, 0, 0),
                DiaDaSemana = 1
            });

            await context.SaveChangesAsync();

            var agendamentoDto = new SolicitarAgendamentoDto
            {
                DiscenteId = discenteId,
                ProfissionalId = profissionalId,
                ServicoId = servicoId,
                HorarioId = horarioId,
                Data = DateTime.Now,
                Status = "Concluido"
            };

            var agendamento = await agendamentoService.SolicitarAgendamentoAsync(agendamentoDto);

            Assert.That(agendamento, Is.Not.Null);
            Assert.That(agendamento.DiscenteId, Is.EqualTo(1));
            Assert.That(agendamento.ProfissionalId, Is.EqualTo(2));
            Assert.That(agendamento.ServicoId, Is.EqualTo(3));
            Assert.That(agendamento.HorarioId, Is.EqualTo(1));
        }

        [Test]
        public async Task CancelarAgendamento()
        {

            var profissionalId = 2;
            var horarioId = 1;
            var discenteId = 1;
            var servicoId = 3;

            context.ServicoDisponivel.Add(new ServicoDisponivel
            {
                IdServico = servicoId,
                Tipo = "Psicologa",
                TipoAtendimento = "Consulta"
            });
            await context.SaveChangesAsync();

            context.Profissionais.Add(new Profissional
            {
                IdProfissional = profissionalId,
                Nome = "Geisa Morais",
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = "Geisa07.",
                Salt = "Geisa07.",
                ServicoId = servicoId,
                Descricao = "Servico prestado por Geisa Morais"
            });
            await context.SaveChangesAsync();

            context.Discentes.Add(new Discente
            {
                IdDiscente = discenteId,
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Senha = "Renan06.",
                Salt = "Renan06.",
                Curso = "Engenharia"
            });
            await context.SaveChangesAsync();

            context.HorarioDisponivel.Add(new HorarioDisponivel
            {
                IdHorario = horarioId,
                ProfissionalId = profissionalId,
                HoraInicio = new TimeSpan(9, 0, 0),
                HoraFim = new TimeSpan(10, 0, 0),
                DiaDaSemana = 1
            });

            await context.SaveChangesAsync();

            var agendamentoDto = new SolicitarAgendamentoDto
            {
                DiscenteId = discenteId,
                ProfissionalId = profissionalId,
                ServicoId = servicoId,
                HorarioId = horarioId,
                Data = DateTime.Now,
                Status = "Cancelado"
            };
            var agendamento = await agendamentoService.SolicitarAgendamentoAsync(agendamentoDto);

            var result = await agendamentoService.CancelarAgendamentoAsync(agendamento.IdAgendamento);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CancelarAgendamento_SemAgendamento()
        {
            var agendamentoId = 123;
            var result = await agendamentoService.CancelarAgendamentoAsync(agendamentoId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task SolicitarAgendamento_HorarioIndisponivel()
        {
            var profissionalId = 2;
            var horarioIdInexistente = 1234;
            var discenteId = 1;
            var servicoId = 3;

            context.ServicoDisponivel.Add(new ServicoDisponivel
            {
                IdServico = servicoId,
                Tipo = "Psicologa",
                TipoAtendimento = "Consulta"
            });
            await context.SaveChangesAsync();

            context.Profissionais.Add(new Profissional
            {
                IdProfissional = profissionalId,
                Nome = "Geisa Morais",
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = "Geisa07.",
                Salt = "Geisa07.",
                ServicoId = servicoId,
                Descricao = "Servico prestado por Geisa Morais"
            });
            await context.SaveChangesAsync();

            context.Discentes.Add(new Discente
            {
                IdDiscente = discenteId,
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Matricula = 2021010871,
                Senha = "Renan06.",
                Salt = "Renan06.",
                Telefone = "40028922",
                Curso = "Engenharia"
            });
            await context.SaveChangesAsync();

            var agendamentoDto = new SolicitarAgendamentoDto
            {
                DiscenteId = discenteId,
                ProfissionalId = profissionalId,
                ServicoId = servicoId,
                HorarioId = horarioIdInexistente,
                Data = DateTime.Now,
                Status = "Concluido"
            };

            var agendamento = await agendamentoService.SolicitarAgendamentoAsync(agendamentoDto);
            Assert.That(agendamento, Is.Null);
        }

        [Test]
        public async Task ListarAgendamentos()
        {
            var discente = new Discente
            {
                IdDiscente = 2,
                Nome = "Antonio Caue",
                Email = "antonio.caue@alunos.ufersa.edu.br",
                Senha = "Caue123.",
                Salt = "Caue123.",
                Matricula = 2021010871,
                Telefone = "40028922",
                Curso = "Engenharia"
            };

            var profissional = new Profissional
            {
                IdProfissional = 1,
                Nome = "Renan Costa",
                Email = "renan.costa@ufersa.edu.br",
                Senha = "Renan06.",
                Salt = "Renan06.",
                ServicoId = 1,
                Descricao = "Servico prestado por Renan Costa"
            };

            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Psicologa",
                TipoAtendimento = "Retorno"
            };

            var horario = new HorarioDisponivel
            {
                IdHorario = 1,
                HoraInicio = TimeSpan.FromHours(9),
                HoraFim = TimeSpan.FromHours(10),
                DiaDaSemana = 1,
                ProfissionalId = profissional.IdProfissional
            };

            context.Discentes.Add(discente);
            context.Profissionais.Add(profissional);
            context.ServicoDisponivel.Add(servico);
            context.HorarioDisponivel.Add(horario);
            await context.SaveChangesAsync();

            var agendamento = new Agendamento
            {
                Data = DateTime.Now.AddDays(1),
                DiscenteId = discente.IdDiscente,
                ProfissionalId = profissional.IdProfissional,
                ServicoId = servico.IdServico,
                HorarioId = horario.IdHorario,
                Status = "Concluido"
            };

            context.Agendamento.Add(agendamento);
            await context.SaveChangesAsync();

            var result = await agendamentoService.ListarAgendamentosPorDiscenteAsync(discente.IdDiscente);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ListarHorariosDisponiveis()
        {
            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Psicóloga",
                TipoAtendimento = "Consulta"
            };

            var profissional = new Profissional
            {
                IdProfissional = 1,
                Nome = "Renan Costa",
                Email = "renan.costa@ufersa.edu.br",
                Senha = "Renan06.",
                Salt = "Renan06.",
                ServicoId = servico.IdServico,
                Descricao = "Servico prestado por Renan Costa"
            };

            var horarios = new List<HorarioDisponivel>
            {
                new HorarioDisponivel { IdHorario = 1, HoraInicio = TimeSpan.FromHours(9), HoraFim = TimeSpan.FromHours(10), DiaDaSemana = 1, ProfissionalId = profissional.IdProfissional },
                new HorarioDisponivel { IdHorario = 2, HoraInicio = TimeSpan.FromHours(10), HoraFim = TimeSpan.FromHours(11), DiaDaSemana = 1, ProfissionalId = profissional.IdProfissional },
                new HorarioDisponivel { IdHorario = 3, HoraInicio = TimeSpan.FromHours(14), HoraFim = TimeSpan.FromHours(15), DiaDaSemana = 2, ProfissionalId = profissional.IdProfissional }
            };

            context.ServicoDisponivel.Add(servico);
            context.Profissionais.Add(profissional);
            context.HorarioDisponivel.AddRange(horarios);
            await context.SaveChangesAsync();

            var result = await agendamentoService.ListarHorariosDisponiveisAsync(profissional.IdProfissional);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(horarios.Count));
        }

        [Test]
        public async Task BuscarAgendamentosPorProfissionalAsync()
        {
            var profissionalId = 2;
            var horarioId = 1;
            var discenteId = 1;
            var servicoId = 3;

            context.ServicoDisponivel.Add(new ServicoDisponivel
            {
                IdServico = servicoId,
                Tipo = "Pedagoga",
                TipoAtendimento = "Consulta"
            });
            await context.SaveChangesAsync();

            context.Profissionais.Add(new Profissional
            {
                IdProfissional = profissionalId,
                Nome = "Geisa Morais",
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = "Geisa07.",
                Salt = "Geisa07.",
                ServicoId = servicoId,
                Descricao = "Servico prestado por Geisa Morais"
            });
            await context.SaveChangesAsync();

            context.Discentes.Add(new Discente
            {
                IdDiscente = discenteId,
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Senha = "Renan06.",
                Salt = "Renan06.",
                Telefone = "40028922",
                Curso = "Engenharia"
            });
            await context.SaveChangesAsync();

            context.HorarioDisponivel.Add(new HorarioDisponivel
            {
                IdHorario = horarioId,
                ProfissionalId = profissionalId,
                HoraInicio = new TimeSpan(9, 0, 0),
                HoraFim = new TimeSpan(10, 0, 0),
                DiaDaSemana = 1
            });
            await context.SaveChangesAsync();

            context.Agendamento.Add(new Agendamento
            {
                DiscenteId = discenteId,
                ProfissionalId = profissionalId,
                ServicoId = servicoId,
                HorarioId = horarioId,
                Data = DateTime.Now.AddDays(1),
                Status = "Concluido"
            });
            await context.SaveChangesAsync();

            var result = await agendamentoService.BuscarAgendamentosPorProfissionalAsync(profissionalId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().ProfissionalId, Is.EqualTo(profissionalId));
        }
    }
}