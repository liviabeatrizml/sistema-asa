using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Controllers;
using Back_end.Data;
using Back_end.Dtos;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace testeDeUnidade.controller
{
    [TestFixture]
    public class AgendamentoControllerTests
    {
        private AgendamentoController controller;
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
            var agendamentoService = new AgendamentoService(context);
            controller = new AgendamentoController(agendamentoService);

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
            var discente = new Discente
            {
                IdDiscente = 1,
                Nome = "Geísa",
                Email = "geisa.gabriel@alunos.ufersa.edu.br",
                Senha = "Geisa07.",
                Salt = "Geisa07.",
                Curso = "Engenharia de Software"
            };

            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Pedagogo",
                Descricao = "Servico prestado pela pedagoga Geisa Morais",
                TipoAtendimento = "Consulta"
            };

            var profissional = new Profissional
            {
                IdProfissional = 1,
                Nome = "Eriky",
                Email = "eriky.abreu@ufersa.edu.br",
                Senha = "Eriky07.",
                Salt = "Eriky07.",
                ServicoId = servico.IdServico
            };

            await context.Discentes.AddAsync(discente);
            await context.ServicoDisponivel.AddAsync(servico);
            await context.Profissionais.AddAsync(profissional);
            await context.SaveChangesAsync();

            var dto = new SolicitarAgendamentoDto
            {
                DiscenteId = discente.IdDiscente,
                ProfissionalId = profissional.IdProfissional,
                ServicoId = servico.IdServico,
                HorarioId = 1,
                Data = DateTime.Now
            };

            var result = await controller.SolicitarAgendamento(dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var resultado = result as BadRequestObjectResult;
            Assert.That(resultado?.Value, Is.Not.Null);
        }

        [Test]
        public async Task SolicitarAgendamento_Indisponivel()
        {
            var solicitarAgendamentoDto = new SolicitarAgendamentoDto
            {
                DiscenteId = 1,
                ProfissionalId = 1,
                ServicoId = 1,
                HorarioId = 1,
                Data = DateTime.Now
            };

            var result = await controller.SolicitarAgendamento(solicitarAgendamentoDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var resultado = result as BadRequestObjectResult;
            Assert.That(resultado?.Value, Is.EqualTo("Horário indisponível."));
        }

        [Test]
        public async Task CancelarAgendamento_Inexistente()
        {
            int idAgendamento = 4;

            var result = await controller.CancelarAgendamento(idAgendamento);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult?.Value, Is.EqualTo("Agendamento não encontrado."));
        }

        [Test]
        public async Task ListarHorariosDisponiveis()
        {
            int profissionalId = 1;

            var result = await controller.ListarHorariosDisponiveis(profissionalId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var resultado = result as OkObjectResult;
            Assert.That(resultado?.Value, Is.InstanceOf<List<HorarioDisponivel>>());
        }

        [Test]
        public async Task ListarAgendamentosPorDiscente()
        {
            var discente = new Discente
            {
                IdDiscente = 1,
                Nome = "Renan",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Senha = "Renan06.",
                Salt = "Renan06.",
                Curso = "Engenharia"
            };

            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Psicologo",
                Descricao = "Servico prestado por Geisa Morais",
                TipoAtendimento = "Retorno"
            };

            var profissional = new Profissional
            {
                IdProfissional = 1,
                Nome = "Geisa Morais",
                Email = "geisa.gabriel@alunos.ufersa.edu.br",
                Senha = "Geisa07.",
                Salt = "Geisa07.",
                ServicoId = servico.IdServico
            };

            var agendamentos = new List<Agendamento>
            {
            new Agendamento
                {
                    IdAgendamento = 1,
                    Data = DateTime.Now,
                    DiscenteId = discente.IdDiscente,
                    ProfissionalId = profissional.IdProfissional,
                    ServicoId = servico.IdServico,
                    HorarioId = 1
                },
            new Agendamento
                {
                    IdAgendamento = 2,
                    Data = DateTime.Now.AddDays(1),
                    DiscenteId = discente.IdDiscente,
                    ProfissionalId = profissional.IdProfissional,
                    ServicoId = servico.IdServico,
                    HorarioId = 2
                }
            };

            context.ServicoDisponivel.Add(servico);
            context.Discentes.Add(discente);
            context.Profissionais.Add(profissional);
            context.Agendamento.AddRange(agendamentos);
            await context.SaveChangesAsync();

            var result = await controller.ListarAgendamentosPorDiscente(discente.IdDiscente);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var resultado = result as OkObjectResult;
            var agendamentosRetornados = resultado?.Value as List<Agendamento>;

            Assert.That(agendamentosRetornados, Is.Not.Null);
            Assert.That(agendamentosRetornados.Count, Is.EqualTo(2));
        }
    }
}