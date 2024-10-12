using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Data;
using Back_end.Models;
using Back_end.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace testeDeUnidade.services.discentServices
{
    public class DiscenteServiceTests
    {
        private ApiDbContext context;
        private IConfiguration config;
        private DiscenteService discenteService;

        [SetUp]
        public void Setup()
        {
            // CONFIGURACAO COM O BANCO
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseMySql("Server=localhost;Database=asa;User=root;Password=admin;",
                          new MySqlServerVersion(new Version(8, 0, 21)))
                .Options;

            // INSTANCIA DO BANCO
            context = new ApiDbContext(options);
            var inMemoryCollection = new Dictionary<string, string?>
            {
                { "Jwt:Key", "A2js#h7Kd920Dld9FjSjk39!@kdkfjSJd0298Dk" },
                { "Jwt:Issuer", "http://localhost" }
            };

            // CONSTRUCAO DAS CONFIGURACOES DO BANCO
            config = new ConfigurationBuilder().AddInMemoryCollection(inMemoryCollection).Build();

            discenteService = new DiscenteService(context, config);

            // EXCLUSAO DO BANCO
            context.Database.EnsureDeleted();

            // CRIACAO DO BANCO
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            context?.Dispose();
        }

        [Test]
        public async Task RegistrarDiscenteAsync()
        {
            var registro = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "998420066",
                Curso = "Engenharia"
            };

            // AWAIT EH UM TEMPORIZADOR DE CONCLUSAO DE ACAO
            var discente = await discenteService.RegistrarDiscenteAsync(registro);

            Assert.That(discente, Is.Not.Null);
            Assert.That(discente.Nome, Is.EqualTo("Livia"));
            Assert.That(discente.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public void RegistrarDiscenteAsync_RegistrarNulo()
        {
            RegistrarDiscente? RegistrarNulo = null;
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await discenteService.RegistrarDiscenteAsync(RegistrarNulo!));

            Assert.That(ex!.ParamName, Is.EqualTo("registro"));
        }

        [Test]
        public async Task LoginDiscenteAsync()
        {
            var registro = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "998420066",
                Curso = "Engenharia"
            };
            await discenteService.RegistrarDiscenteAsync(registro);

            var login = new LoginDiscente
            {
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10."
            };

            var response = await discenteService.LoginDiscenteAsync(login);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.UserId, Is.EqualTo("1"));
        }

        [Test]
        public void LoginDiscenteAsync_LoginNulo()
        {
            LoginDiscente? loginNulo = null;
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await discenteService.LoginDiscenteAsync(loginNulo!));

            Assert.That(ex!.ParamName, Is.EqualTo("login"));
        }

        [Test]
        public async Task LoginDiscenteAsync_DiscenteNaoEncontrado()
        {
            var login = new LoginDiscente
            {
                Email = "alysson@alunos.ufersa.edu.br",
                Senha = "Alysson08."
            };

            var resultado = await discenteService.LoginDiscenteAsync(login);

            Assert.IsNull(resultado);
        }

        [Test]
        public async Task LoginDiscenteAsync_SenhaVazia()
        {
            var registro = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "998420066",
                Curso = "Engenharia"
            };
            await discenteService.RegistrarDiscenteAsync(registro);

            var login = new LoginDiscente
            {
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = ""
            };

            var resultado = await discenteService.LoginDiscenteAsync(login);

            Assert.IsNull(resultado);
        }

        [Test]
        public async Task LoginDiscenteAsync_SaltVazio()
        {
            var registro = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "998420066",
                Curso = "Engenharia"
            };

            await discenteService.RegistrarDiscenteAsync(registro);

            var discente = await context.Discentes.FirstOrDefaultAsync(d => d.Email == "livia.lima30332@alunos.ufersa.edu.br");

            Assert.NotNull(discente);

            discente.Salt = string.Empty;
            await context.SaveChangesAsync();

            var login = new LoginDiscente
            {
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10."
            };

            var resultado = await discenteService.LoginDiscenteAsync(login);
            Assert.IsNull(resultado);
        }

        [Test]
        public async Task AtualizarPerfilAsync()
        {
            var registro = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "998420066",
                Curso = "Engenharia"
            };
            await discenteService.RegistrarDiscenteAsync(registro);

            var atualizarPerfil = new AtualizarPerfilDto
            {
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Nome = "Livia Beatriz"
            };

            var resultado = await discenteService.AtualizarPerfilAsync(atualizarPerfil);

            Assert.That(resultado, Is.True);
            var discente = await discenteService.ObterDiscentePorIdAsync(1);
            Assert.That(discente.Nome, Is.EqualTo("Livia Beatriz"));
        }

        [Test]
        public async Task RegistrarProfissionalAsync()
        {
            var registro = new RegistrarProfissional
            {
                Nome = "Eriky",
                Email = "eriky.veloso@ufersa.edu.br",
                Senha = "Eriky10."
            };

            var profissional = await discenteService.RegistrarProfissionalAsync(registro);

            Assert.That(profissional, Is.Not.Null);
            Assert.That(profissional.Nome, Is.EqualTo("Eriky"));
            Assert.That(profissional.Email, Is.EqualTo("eriky.veloso@ufersa.edu.br"));
        }

        [Test]
        public void RegistrarProfissionalAsync_RegistrarNulo()
        {
            RegistrarProfissional? RegistrarNulo = null;
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await discenteService.RegistrarProfissionalAsync(RegistrarNulo!));

            Assert.That(ex!.ParamName, Is.EqualTo("registro"));
        }

        [Test]
        public async Task LoginProfissionalAsync()
        {
            var registro = new RegistrarProfissional
            {
                Nome = "Eriky",
                Email = "eriky.veloso@ufersa.edu.br",
                Senha = "Eriky10."
            };
            await discenteService.RegistrarProfissionalAsync(registro);

            var login = new LoginProfissional
            {
                Email = "eriky.veloso@ufersa.edu.br",
                Senha = "Eriky10."
            };

            var response = await discenteService.LoginProfissionalAsync(login);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.UserId, Is.EqualTo("1"));
        }

        [Test]
        public async Task LoginProfissionalAsync_ProfissionalNull()
        {
            var login = new LoginProfissional
            {
                Email = "antonio.caue@ufersa.edu.br",
                Senha = "antonio05."
            };

            var resultado = await discenteService.LoginProfissionalAsync(login);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public async Task LoginProfissionalAsync_SenhaVazia()
        {
            var registro = new RegistrarProfissional
            {
                Nome = "Geisa Morais",
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = "Geisa07."
            };

            await discenteService.RegistrarProfissionalAsync(registro);

            var login = new LoginProfissional
            {
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = ""
            };

            var resultado = await discenteService.LoginProfissionalAsync(login);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public async Task LoginProfissionalAsync_SenhaIncorreta()
        {
            var registro = new RegistrarProfissional
            {
                Nome = "Geisa Morais",
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = "Geisa07."
            };

            await discenteService.RegistrarProfissionalAsync(registro);

            var login = new LoginProfissional
            {
                Email = "geisa.gabriel@ufersa.edu.br",
                Senha = "Geisa10."
            };

            var resultado = await discenteService.LoginProfissionalAsync(login);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public async Task EmailJaCadastradoAsync_EmailJaCadastrado()
        {
            var registroDiscente = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "998420066",
                Curso = "Engenharia"
            };

            await discenteService.RegistrarDiscenteAsync(registroDiscente);

            var resultado = await discenteService.EmailJaCadastradoAsync("livia.lima30332@alunos.ufersa.edu.br");

            Assert.That(resultado, Is.True);
        }

        [Test]
        public async Task EmailJaCadastradoAsync_EmailNaoCadastrado()
        {
            var emailNaoCadastrado = "geisa.gabriel@alunos.ufersa.edu.br";

            var resultado = await discenteService.EmailJaCadastradoAsync(emailNaoCadastrado);

            Assert.That(resultado, Is.False);
        }

        [Test]
        public async Task AtualizarPerfilAsync_ProfissionalNaoEncontrado()
        {
            var atualizarPerfil = new AtualizarPerfilDto
            {
                Email = "renan.costa@ufersa.edu.br",
                Nome = "Renan Costa"
            };

            var resultado = await discenteService.AtualizarPerfilAsync(atualizarPerfil);

            Assert.That(resultado, Is.False);
        }

        [Test]
        public async Task ObterDiscentePorIdAsync()
        {
            var registro = new RegistrarDiscente
            {
                Nome = "Livia",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Senha = "Livia10.",
                Matricula = 2021010871,
                Telefone = "998420066",
                Curso = "Engenharia"
            };
            await discenteService.RegistrarDiscenteAsync(registro);

            var discente = await discenteService.ObterDiscentePorIdAsync(1);

            Assert.That(discente, Is.Not.Null);
            Assert.That(discente.Nome, Is.EqualTo("Livia"));
        }

        [Test]
        public async Task ObterDiscentePorIdAsync_DiscenteNaoEncontrado()
        {
            var discente = await discenteService.ObterDiscentePorIdAsync(-1);

            Assert.IsNull(discente);
        }
    }
}