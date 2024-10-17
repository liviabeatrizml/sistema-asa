using Back_end.Controllers;
using Back_end.Data;
using Back_end.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace testeDeUnidade.controller
{
    [TestFixture]
    public class ServicoControllerTests
    {
        private ServicoController controller;
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
            controller = new ServicoController(context);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            context?.Dispose();
        }

        [Test]
        public async Task CadastrarServico()
        {
            var servicoDto = new ServicoDto
            {
                Tipo = "Pedagogo",
                Descricao = "Atendimento prestado por Geísa Morais Gabriel",
                TipoAtendimento = "Consulta"
            };

            var result = await controller.CadastrarServico(servicoDto);

            var actionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actionResult);
            var servicoCadastrado = actionResult.Value as ServicoDisponivel;
            Assert.IsNotNull(servicoCadastrado);
            Assert.That(servicoCadastrado.Tipo, Is.EqualTo("Pedagogo"));
            Assert.That(await context.ServicoDisponivel.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetServicoExistente()
        {
            var servico = new ServicoDisponivel 
            { 
                IdServico = 1, 
                Tipo = "Psicologa", 
                Descricao = "Atendimento prestado por Lívia Beatriz", 
                TipoAtendimento = "Retorno" 
            };
            context.ServicoDisponivel.Add(servico);
            await context.SaveChangesAsync();

            var result = await controller.GetServico(1);

            var actionResult = result as ActionResult<ServicoDisponivel>;
            var servicoEncontrado = actionResult.Value;
            Assert.IsNotNull(servicoEncontrado);
            Assert.That(servicoEncontrado.IdServico, Is.EqualTo(1));
        }

        [Test]
        public async Task GetServicoNaoExistente()
        {
            var result = await controller.GetServico(1);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task ListarServicos()
        {
            context.ServicoDisponivel.Add(new ServicoDisponivel { IdServico = 1, Tipo = "Pedagogo", Descricao = "Atendimento prestado por Geísa Morais", TipoAtendimento = "Retorno" });
            context.ServicoDisponivel.Add(new ServicoDisponivel { IdServico = 2, Tipo = "Psicologo", Descricao = "Atendimento prestado por Livia Lima", TipoAtendimento = "Consulta" });
            await context.SaveChangesAsync();

            var result = await controller.ListarServicos();

            var actionResult = result as ActionResult<IEnumerable<ServicoDisponivel>>;
            var servicos = actionResult.Value;
            Assert.That(servicos?.Count() ?? 0, Is.EqualTo(2));
        }
    }
}