using Back_end.Controllers;
using Back_end.Data;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace testeDeUnidade.controller
{
    [TestFixture]
    public class DiscenteControllerTests
    {
        private ApiDbContext context;
        private DiscenteController controller;
        private IDiscenteService discenteService;
        private IProfissionalService profissionalService;

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

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryCollection)
                .Build();

            discenteService = new DiscenteService(context, configuration);
            profissionalService = new ProfissionalService(context);

            controller = new DiscenteController(discenteService, profissionalService);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task RegistrarDiscente()
        {
            var discenteDto = new RegistrarDiscente
            {
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Matricula = 2021010871,
                Senha = "Renan06.",
                Telefone = "40028922",
                Curso = "Engenharia"
            };

            var discenteRegistrado = await discenteService.RegistrarDiscenteAsync(discenteDto);

            Assert.IsNotNull(discenteRegistrado);
            Assert.That(discenteRegistrado.Nome, Is.EqualTo("Renan Costa"));
            Assert.That(discenteRegistrado.Email, Is.EqualTo("renan.costa@alunos.ufersa.edu.br"));
            Assert.That(discenteRegistrado.Matricula, Is.EqualTo(2021010871));
            Assert.That(discenteRegistrado.Curso, Is.EqualTo("Engenharia"));
        }

        [Test]
        public async Task AtualizarPerfilParcial()
        {
            var discente = new Discente
            {
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Matricula = 2021010871,
                Telefone = "40028922",
                Curso = "Engenharia",
                Senha = "Renan10.",
                Salt = "Renan10."
            };

            context.Discentes.Add(discente);
            await context.SaveChangesAsync();

            var atualizarPerfilDto = new AtualizarPerfilDto
            {
                Nome = "Francisco Renan Leite",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Telefone = "98765432",
                Matricula = 2021010871,
                Curso = "Engenharia de Software"
            };

            var result = await controller.AtualizarPerfilParcial(atualizarPerfilDto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var resultado = result as OkObjectResult;

            Assert.That(resultado?.Value, Is.EqualTo("Perfil atualizado com sucesso."));
        }

        [Test]
        public async Task LoginDiscente_Null()
        {
            var result = await controller.Login(null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var resultado = result as BadRequestObjectResult;

            Assert.IsNotNull(resultado);
            Assert.That(resultado.Value, Is.EqualTo("Dados inválidos."));
        }

        [Test]
        public async Task LoginDiscente_Invalido()
        {
            var loginDiscente = new LoginDiscente
            {
                Email = "geisa.gabriel@alunos.ufersa.edu.br",
                Senha = "gagaBriel10."
            };

            var result = await controller.Login(loginDiscente);

            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            var resultado = result as UnauthorizedObjectResult;

            Assert.IsNotNull(resultado);
            Assert.That(resultado.Value, Is.EqualTo("Login inválido"));
        }

        [Test]
        public async Task Login()
        {
            var discenteDto = new RegistrarDiscente
            {
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Matricula = 2021010871,
                Telefone = "40028922",
                Curso = "Engenharia",
                Senha = "Renan10."
            };

            await discenteService.RegistrarDiscenteAsync(discenteDto);

            var loginDiscente = new LoginDiscente
            {
                Email = "renan.costa@alunos.ufersa.edu.br",
                Senha = "Renan10."
            };

            var result = await controller.Login(loginDiscente);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task LoginProfissional_Invalido()
        {
            var loginProfissional = new LoginProfissional
            {
                Email = "antonio.caue@ufersa.edu.br",
                Senha = "Caca.8432"
            };

            var result = await controller.LoginProfissional(loginProfissional);
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            var resultado = result as UnauthorizedObjectResult;

            Assert.IsNotNull(resultado);
            Assert.That(resultado.Value, Is.EqualTo("Login inválido"));
        }

        [Test]
        public async Task LoginProfissional_Null()
        {
            var result = await controller.LoginProfissional(null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var resultado = result as BadRequestObjectResult;

            Assert.IsNotNull(resultado);
            Assert.That(resultado.Value, Is.EqualTo("Dados inválidos."));
        }

        [Test]
        public async Task RegistrarProfissional_Invalido()
        {
            var result = await controller.RegistrarProfissional(null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Dados inválidos."));
        }

        [Test]
        public async Task ObterDiscentePorId_Invalido()
        {
            var result = await controller.ObterDiscentePorId(123);
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task RegistrarDiscente_Invalido()
        {
            var result = await controller.Registrar(null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Dados inválidos."));
        }

        [Test]
        public async Task RegistrarProfissional_EmailJaCadastrado()
        {
            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Psicologo",
                TipoAtendimento = "Consulta"
            };

            context.ServicoDisponivel.Add(servico);
            await context.SaveChangesAsync();

            var profissionalExistente = new Profissional
            {
                Nome = "Renan Costa",
                Email = "renan.costa@ufersa.edu.br",
                Senha = "Renan10.",
                Salt = "Renan10.",
                ServicoId = servico.IdServico,
                Descricao = "Servico prestado por Renan Costa"
            };

            context.Profissionais.Add(profissionalExistente);
            await context.SaveChangesAsync();

            var registrarProfissional = new RegistrarProfissional
            {
                Nome = "Antonio Caue",
                Email = "renan.costa@ufersa.edu.br",
                Senha = "Antonio00.",
                ServicoId = servico.IdServico
            };

            var result = await controller.RegistrarProfissional(registrarProfissional);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var resultado = result as BadRequestObjectResult;
            Assert.That(resultado.Value, Is.EqualTo("Email já cadastrado"));
        }

        [Test]
        public async Task ObterProfissional()
        {
            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Pedagogo",
                TipoAtendimento = "Consulta"
            };

            context.ServicoDisponivel.Add(servico);
            await context.SaveChangesAsync();

            var profissional = new Profissional
            {
                Nome = "Eriky",
                Email = "eriky.abreu@ufersa.edu.br",
                Senha = "Eriky07.",
                Salt = "Eriky07.",
                Descricao = "Serviço prestado por Eriky Abreu",
                ServicoId = servico.IdServico
            };

            context.Profissionais.Add(profissional);
            await context.SaveChangesAsync();

            var result = await profissionalService.ObterProfissionalPorIdAsync(profissional.IdProfissional);

            Assert.IsNotNull(result);
            Assert.That(result.Nome, Is.EqualTo("Eriky"));
        }

        [Test]
        public async Task ObterProfissional_NaoEncontrado()
        {
            int idInexistente = 123;

            var result = await controller.ObterProfissional(idInexistente);
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var resultado = result as NotFoundObjectResult;
            Assert.That(resultado.Value, Is.EqualTo("Profissional não encontrado."));
        }

        [Test]
        public async Task AtualizarPerfil()
        {
            var discente = new Discente
            {
                Nome = "Renan Costa",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Matricula = 2021010871,
                Telefone = "40028922",
                Curso = "Engenharia",
                Senha = "Renan10.",
                Salt = "Renan10."
            };

            context.Discentes.Add(discente);
            await context.SaveChangesAsync();

            var atualizarPerfilDto = new AtualizarPerfilDto
            {
                Nome = "Renan Costa Leite",
                Email = "renan.costa@alunos.ufersa.edu.br",
                Telefone = "999997766",
                Matricula = 2021010871,
                Curso = "Engenharia de Software"
            };

            var result = await controller.AtualizarPerfil(atualizarPerfilDto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var resultado = result as OkObjectResult;

            Assert.IsNotNull(resultado);
            Assert.That(resultado.Value, Is.EqualTo("Perfil atualizado com sucesso."));
        }

        [Test]
        public async Task AtualizarPerfil_NaoEncontrado()
        {
            var atualizarPerfilDto = new AtualizarPerfilDto
            {
                Nome = "Antonio Caue",
                Email = "antoniocaue@ufersa.edu.br",
                Telefone = "123456789",
                Matricula = 2020202020,
                Curso = "Engenharia"
            };

            var result = await controller.AtualizarPerfil(atualizarPerfilDto);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.That(notFoundResult.Value, Is.EqualTo("Discente não encontrado."));
        }

        [Test]
        public async Task ListarProfissionais_ListaVazia()
        {
            var result = await controller.ListarProfissionais();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var resultado = result.Result as OkObjectResult;

            Assert.IsNotNull(resultado);
            Assert.That(((IEnumerable<ProfissionalDto>)resultado.Value).Count(), Is.EqualTo(0));
        }
    }
}