using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Data;
using Back_end.Models;
using Back_end.Services;
using Back_end.Controllers;
using Back_end.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Text;

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
        public async Task RegistrarDiscente()
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

            var discente = await discenteService.RegistrarDiscenteAsync(registro);

            Assert.That(discente, Is.Not.Null);
            Assert.That(discente.Nome, Is.EqualTo("Livia"));
            Assert.That(discente.Email, Is.EqualTo("livia.lima30332@alunos.ufersa.edu.br"));
        }

        [Test]
        public async Task LoginDiscente()
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
            Assert.That(response.UserId, Is.Not.Empty);
        }

        [Test]
        public async Task LoginDiscente_EmailInexistente()
        {
            var login = new LoginDiscente
            {
                Email = "antonio.caue@ufersa.edu.br",
                Senha = "Antonio00."
            };

            var response = await discenteService.LoginDiscenteAsync(login);
            Assert.That(response, Is.Null);
        }

        [Test]
        public async Task LoginDiscente_SenhaIncorreta()
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
                Senha = "Biatriz10."
            };
            var response = await discenteService.LoginDiscenteAsync(login);

            Assert.That(response, Is.Null);
        }


        [Test]
        public async Task EmailJaCadastrado()
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
        public async Task RegistrarProfissional()
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

            var registro = new RegistrarProfissional
            {
                Nome = "Livia Lima",
                Email = "livia.lima@ufersa.edu.br",
                Senha = "Livia10.",
                ServicoId = servico.IdServico
            };

            var resultado = await discenteService.RegistrarProfissionalAsync(registro);

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Nome, Is.EqualTo("Livia Lima"));
            Assert.That(resultado.Email, Is.EqualTo("livia.lima@ufersa.edu.br"));
        }

        [Test]
        public async Task LoginProfissional()
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

            var registro = new RegistrarProfissional
            {
                Nome = "Livia Lima",
                Email = "livia.lima@ufersa.edu.br",
                Senha = "Livia10.",
                ServicoId = servico.IdServico
            };

            await discenteService.RegistrarProfissionalAsync(registro);

            var login = new LoginProfissional
            {
                Email = "livia.lima@ufersa.edu.br",
                Senha = "Livia10."
            };

            var response = await discenteService.LoginProfissionalAsync(login);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.UserId, Is.Not.Empty);
            Assert.That(response.Token, Is.Not.Empty);
        }

        [Test]
        public async Task AtualizarPerfil()
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
                Email = registro.Email,
                Nome = "Livia Beatriz",
                Telefone = "40028922"
            };

            var resultado = await discenteService.AtualizarPerfilAsync(atualizarPerfil);

            Assert.That(resultado, Is.True);

            var discenteAtualizado = await discenteService.ObterDiscentePorIdAsync(1);
            Assert.That(discenteAtualizado.Nome, Is.EqualTo("Livia Beatriz"));
            Assert.That(discenteAtualizado.Telefone, Is.EqualTo("40028922"));
        }

        [Test]
        public async Task AlterarSenha()
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

            var alterarSenhaDto = new AlterarSenhaDto
            {
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                SenhaAtual = "Livia10.",
                NovaSenha = "Beatriz10."
            };

            var resultado = await discenteService.AlterarSenhaAsync(alterarSenhaDto);

            Assert.That(resultado, Is.True);

            var discente = await context.Discentes.SingleOrDefaultAsync(d => d.Email == alterarSenhaDto.Email);

            Assert.That(discente, Is.Not.Null);

            Assert.That(discente.Senha, Is.Not.EqualTo("Livia10."));
            Assert.That(discente.Senha, Is.Not.Null);
        }

        [Test]
        public async Task ObterDiscentePorId()
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

            var resultado = await discenteService.ObterProfissionalPorIdAsync(2);

            Assert.IsNotNull(resultado);
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Nome, Is.EqualTo("Livia Lima"));
            Assert.That(resultado.Email, Is.EqualTo("livia.lima@ufersa.edu.br"));
            Assert.That(resultado.ServicoId, Is.EqualTo(1));
            Assert.That(resultado.ServicoNome, Is.EqualTo("Psicologa"));
        }

        [Test]
        public async Task AtualizarPerfilCompleto()
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
                Nome = "Livia Beatriz",
                Email = "livia.lima30332@alunos.ufersa.edu.br",
                Telefone = "40028922",
                Matricula = 2021010872,
                Curso = "Engenharia de Software"
            };

            var resultado = await discenteService.AtualizarPerfilCompletoAsync(atualizarPerfil);

            Assert.That(resultado, Is.True);
            var discenteAtualizado = await discenteService.ObterDiscentePorIdAsync(1);
            Assert.That(discenteAtualizado.Nome, Is.EqualTo("Livia Beatriz"));
            Assert.That(discenteAtualizado.Telefone, Is.EqualTo("40028922"));
            Assert.That(discenteAtualizado.Matricula, Is.EqualTo(2021010872));
            Assert.That(discenteAtualizado.Curso, Is.EqualTo("Engenharia de Software"));
        }

        [Test]
        public async Task AtualizarPerfilParcial()
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

            var resultado = await discenteService.AtualizarPerfilParcialAsync(atualizarPerfil);

            Assert.That(resultado, Is.True);
            var discente = await discenteService.ObterDiscentePorIdAsync(1);
            Assert.That(discente.Nome, Is.EqualTo("Livia Beatriz"));
        }

        [Test]
        public async Task DeletarProfissional()
        {
            var servico = new ServicoDisponivel
            {
                IdServico = 1,
                Tipo = "Psicóloga",
                Descricao = "Atendimento prestado por Lívia Lima",
                TipoAtendimento = "Consulta"
            };
            context.ServicoDisponivel.Add(servico);
            await context.SaveChangesAsync();

            var profissional = new Profissional
            {
                IdProfissional = 1,
                Nome = "Livia Lima",
                Email = "livia.lima@ufersa.edu.br",
                Senha = "Livia10.",
                Salt = "Livia10.",
                ServicoId = servico.IdServico 
            };

            context.Profissionais.Add(profissional);
            await context.SaveChangesAsync();

            var resultado = await discenteService.DeletarUsuarioAsync(1);

            Assert.That(resultado, Is.True);
            var profissionalRemovido = await context.Profissionais.FindAsync(1);
            Assert.That(profissionalRemovido, Is.Null);
        }

        [Test]
        public async Task DeletarDiscente()
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

            var resultado = await discenteService.DeletarUsuarioAsync(1);

            Assert.That(resultado, Is.True);
            var discente = await discenteService.ObterDiscentePorIdAsync(1);
            Assert.That(discente, Is.Null);
        }
    }
}