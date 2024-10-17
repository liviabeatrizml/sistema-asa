using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Data;
using Back_end.Dtos;
using Back_end.Models;
using Back_end.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace testeDeUnidade.services.profissionalService
{
    [TestFixture]
    public class ProfissionalServiceTests
    {
        private ProfissionalService profissionalService;
        private ApiDbContext context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseMySql("Server=localhost;Database=asa;User=root;Password=admin;",
                          new MySqlServerVersion(new Version(8, 0, 21)))
                .Options;

            context = new ApiDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            profissionalService = new ProfissionalService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context?.Dispose();
        }

        [Test]
        public async Task ObterProfissionalPorId()
        {
            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Psicologa",
                Descricao = "Serviço prestado por Livia Lima",
                TipoAtendimento = "Consulta"
            };
            context.ServicoDisponivel.Add(servico);

            context.Profissionais.Add(new Profissional
            {
                IdProfissional = 2,
                Nome = "Livia Lima",
                Email = "livia.lima@ufersa.edu.br",
                Senha = "Livia10.",
                Salt = "Livia10.",
                ServicoId = 1
            });

            await context.SaveChangesAsync();

            var resultado = await profissionalService.ObterProfissionalPorIdAsync(2);

            Assert.IsNotNull(resultado);
            Assert.That(resultado.Nome, Is.EqualTo("Livia Lima"));
            Assert.That(resultado.Email, Is.EqualTo("livia.lima@ufersa.edu.br"));
        }

        [Test]
public async Task ObterProfissionalPorId_NaoEncontrado()
{
    var resultado = await profissionalService.ObterProfissionalPorIdAsync(3); 
    Assert.IsNull(resultado);
}


        [Test]
        public async Task ListarProfissionais()
        {
            context.ServicoDisponivel.AddRange(new List<ServicoDisponivel>
            {
                new ServicoDisponivel { IdServico = 1, Tipo = "Assistente Social", Descricao = "Atendimento prestado por Renan Costa", TipoAtendimento = "Consulta" },
                new ServicoDisponivel { IdServico = 2, Tipo = "Pedagogo", Descricao = "Atendimento prestado por Eriky Abreu", TipoAtendimento = "Consulta" },
            });

            context.Profissionais.AddRange(new List<Profissional>
            {
                new Profissional { IdProfissional = 1, Nome = "Renan Costa", Email = "renan.costa@ufersa.edu.br", Senha = "Renan06.", Salt = "Renan06.", ServicoId = 1 },
                new Profissional { IdProfissional = 2, Nome = "Eriky Abreu", Email = "eriky.abreu@ufersa.edu.br", Senha = "Eriky10.", Salt = "Eriky10.", ServicoId = 2 },
            });

            await context.SaveChangesAsync();
            var resultado = await profissionalService.ListarProfissionaisAsync();
            Assert.IsNotNull(resultado);
            Assert.That(resultado.Count(), Is.EqualTo(2));
            Assert.IsTrue(resultado.Any(p => p.Nome == "Renan Costa"));
            Assert.IsTrue(resultado.Any(p => p.Nome == "Eriky Abreu"));
        }
    }
}