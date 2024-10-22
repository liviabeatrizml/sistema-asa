﻿using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Back_end.Data;
using Back_end.Models;
using Back_end.Helpers;
using System;
using System.Diagnostics.Contracts;

namespace Back_end.Services
{
    public class DiscenteService : IDiscenteService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _config;

        public DiscenteService(ApiDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Método para registrar um novo discente
        public async Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registro)
        {
            // Verificação de nulidade
            if (registro == null) throw new ArgumentNullException(nameof(registro));

            Contract.Requires(registro.Nome != null, nameof(registro.Nome));
            Contract.Requires(registro.Email != null, nameof(registro.Email));
            Contract.Requires(
                registro.Email.EndsWith("@alunos.ufersa.edu.br"),
                "O Email deve conter o domínio '@alunos.ufersa.edu.br'.");
            Contract.Requires(registro.Senha != null, nameof(registro.Senha));
            Contract.Requires(registro.Matricula != null, nameof(registro.Matricula));

            // Criptografar a senha e gerar o salt
            var (senhaCriptografada, salt) = CriptografarSenha(registro.Senha);

            var discente = new Discente
            {
                Nome = registro.Nome,
                Email = registro.Email,
                Senha = senhaCriptografada,
                Salt = salt, // Armazenar o salt no banco
                Matricula = registro.Matricula,
                Telefone = string.IsNullOrEmpty(registro.Telefone) ? null : registro.Telefone,
                Curso = registro.Curso
        };

            _context.Discentes.Add(discente);
            await _context.SaveChangesAsync();

            Contract.Ensures(discente != null, "O objeto Discente não deve ser nulo ao final do método.");
            Contract.Ensures(discente.Email.EndsWith("@alunos.ufersa.edu.br"), "O email deve conter o domínio correto após a inserção.");

            return discente;
        }

        // Método para login de discente
        public async Task<LoginResponseDto> LoginDiscenteAsync(LoginDiscente login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            Contract.Requires(login.Email != null, "Email não pode ser nulo ou vazio.");
            Contract.Requires(login.Senha != null, "Senha não pode ser nula ou vazia.");

            var discente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == login.Email);

            if (discente == null || string.IsNullOrEmpty(discente.Senha) || string.IsNullOrEmpty(discente.Salt))
            {
                return null;
            }

            if (!VerificarSenha(login.Senha, discente.Senha, discente.Salt))
            {
                return null;
            }

            var token = GerarTokenJwt(discente.IdDiscente.ToString(), discente.Email ?? string.Empty);

            return new LoginResponseDto
            {
                UserId = discente.IdDiscente.ToString(),
                Token = token
            };
        }

        // Método para criptografar a senha com salt
        private (string senhaCriptografada, string salt) CriptografarSenha(string senha)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key); // Gerar um salt único
                var senhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
                var senhaCriptografada = Convert.ToBase64String(senhaHash);
                return (senhaCriptografada, salt);
            }
        }

        // Método para verificar a senha usando o salt
        private bool VerificarSenha(string senha, string senhaCriptografada, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt); // Recuperar o salt
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var senhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
                var senhaHashString = Convert.ToBase64String(senhaHash);
                return senhaHashString == senhaCriptografada;
            }
        }

        // Método para gerar o token JWT (agora genérico para Discente e Profissional)
        private string GerarTokenJwt(string id, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Método para verificar se um email já está cadastrado
        public async Task<bool> EmailJaCadastradoAsync(string email)
        {
            return await _context.Discentes.AnyAsync(d => d.Email == email)
                   || await _context.Profissionais.AnyAsync(p => p.Email == email);
        }

       public async Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registro)
        {
            if (registro == null) throw new ArgumentNullException(nameof(registro));

            Contract.Requires(registro.Nome != null, nameof(registro.Nome));
            Contract.Requires(registro.Email != null, nameof(registro.Email));
            Contract.Requires(
                registro.Email.EndsWith("@ufersa.edu.br"),
                "O Email deve conter o domínio '@ufersa.edu.br'.");
            Contract.Requires(registro.Senha != null, nameof(registro.Senha));
            Contract.Requires(registro.ServicoId != null, nameof(registro.ServicoId));

            // Verificar se o ServicoId é válido
            var servicoExiste = await _context.ServicoDisponivel.AnyAsync(s => s.IdServico == registro.ServicoId);
            if (!servicoExiste)
            {
                throw new ArgumentException("O serviço fornecido não é válido.");
            }

            var (senhaCriptografada, salt) = CriptografarSenha(registro.Senha);

            var profissional = new Profissional
            {
                Nome = registro.Nome,
                Email = registro.Email,
                Senha = senhaCriptografada,
                Salt = salt,
                ServicoId = registro.ServicoId, // Associar o ServicoId ao profissional
                Descricao = registro.descricao
                
            };

            _context.Profissionais.Add(profissional);
            await _context.SaveChangesAsync();

            Contract.Ensures(profissional != null, "O objeto Profissional não deve ser nulo ao final do método.");
            Contract.Ensures(profissional.Email.EndsWith("@ufersa.edu.br"), "O email deve conter o domínio correto após a inserção.");

            return profissional;
        }
        public async Task<LoginResponseDto> LoginProfissionalAsync(LoginProfissional login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            Contract.Requires(login.Email != null, "Email não pode ser nulo ou vazio.");
            Contract.Requires(login.Senha != null, "Senha não pode ser nula ou vazia.");

            var profissional = await _context.Profissionais.SingleOrDefaultAsync(p => p.Email == login.Email);

            if (profissional == null || string.IsNullOrEmpty(profissional.Senha) || string.IsNullOrEmpty(profissional.Salt))
            {
                return null;
            }

            if (!VerificarSenha(login.Senha, profissional.Senha, profissional.Salt))
            {
                return null;
            }

            var token = GerarTokenJwt(profissional.IdProfissional.ToString(), profissional.Email ?? string.Empty);

            return new LoginResponseDto
            {
                UserId = profissional.IdProfissional.ToString(),
                Token = token
            };
        }
        public async Task<bool> AtualizarPerfilAsync(AtualizarPerfilDto atualizarPerfil)
        {
            // Buscando o discente no banco de dados
            var usuarioDiscente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == atualizarPerfil.Email);

            Contract.Requires(atualizarPerfil != null, "O objeto atualizarPerfil não pode ser nulo.");

            if (usuarioDiscente != null)
            {
                // Atualizando campos que não são nulos
                if (!string.IsNullOrEmpty(atualizarPerfil.Nome))
                {
                    usuarioDiscente.Nome = atualizarPerfil.Nome;
                }
                
                // Atualizando o telefone, que agora é uma string
                if (!string.IsNullOrEmpty(atualizarPerfil.Telefone))
                {
                    usuarioDiscente.Telefone = atualizarPerfil.Telefone;
                }

                // Salvar mudanças no banco de dados
                await _context.SaveChangesAsync();
                return true;
            }

            Contract.Ensures(usuarioDiscente != null, "O perfil do usuário foi atualizado com sucesso.");
            Contract.Ensures(usuarioDiscente.Nome != null, "O perfil do usuário foi atualizado com sucesso.");

            return false; // Se o usuário não for encontrado
        }
        public async Task<bool> AlterarSenhaAsync(AlterarSenhaDto alterarSenha)
        {
            // Buscando em ambas as tabelas: Discentes e Profissionais separadamente
            var usuarioDiscente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == alterarSenha.Email);
            var usuarioProfissional = await _context.Profissionais.SingleOrDefaultAsync(p => p.Email == alterarSenha.Email);

            Contract.Requires(alterarSenha != null, "O objeto alterarSenha não pode ser nulo.");

            if (usuarioDiscente != null && VerificarSenha(alterarSenha.SenhaAtual, usuarioDiscente.Senha, usuarioDiscente.Salt))
            {
                var (novaSenhaCriptografada, novoSalt) = CriptografarSenha(alterarSenha.NovaSenha);

                usuarioDiscente.Senha = novaSenhaCriptografada;
                usuarioDiscente.Salt = novoSalt;

                await _context.SaveChangesAsync();

                Contract.Ensures(usuarioDiscente.Senha != null, "A senha do usuário foi atualizada com sucesso.");

                return true;
            }
            else if (usuarioProfissional != null && VerificarSenha(alterarSenha.SenhaAtual, usuarioProfissional.Senha, usuarioProfissional.Salt))
            {
                var (novaSenhaCriptografada, novoSalt) = CriptografarSenha(alterarSenha.NovaSenha);

                usuarioProfissional.Senha = novaSenhaCriptografada;
                usuarioProfissional.Salt = novoSalt;

                await _context.SaveChangesAsync();

                Contract.Ensures(usuarioProfissional.Senha != null, "A senha do usuário foi atualizada com sucesso.");

                return true;
            }

            return false; // Nenhum usuário encontrado ou senha incorreta
        }
        public async Task<DiscenteDto> ObterDiscentePorIdAsync(int id)
        {
            // Buscar o discente pelo ID
            var discente = await _context.Discentes.SingleOrDefaultAsync(d => d.IdDiscente == id);

            // Caso o discente não seja encontrado, retornar null
            if (discente == null)
            {
                return null;
            }

            // Retornar os dados do discente no formato DiscenteDto
            return new DiscenteDto
            {
                Nome = discente.Nome,
                Email = discente.Email,
                Senha = discente.Senha,
                Matricula = discente.Matricula,
                Telefone = discente.Telefone,
                Curso = discente.Curso
            };
        }

        public async Task<ProfissionalDto> ObterProfissionalPorIdAsync(int id)
        {
            // Buscar o profissional pelo ID
            var profissional = await _context.Profissionais
                .Include(p => p.Servico)
                .SingleOrDefaultAsync(p => p.IdProfissional == id);

            // Caso o profissional não seja encontrado, retornar null
            if (profissional == null)
            {
                return null;
            }

            // Retornar os dados do profissional no formato ProfissionalDto
            return new ProfissionalDto
            {
                Nome = profissional.Nome,
                Email = profissional.Email,
                ServicoId = profissional.ServicoId,
                ServicoNome = profissional.Servico?.Tipo // Nome do serviço se disponível
            };
        }

        public async Task<bool> AtualizarPerfilCompletoAsync(AtualizarPerfilDto atualizarPerfil)
        {
            // Buscando o discente no banco de dados
            var usuarioDiscente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == atualizarPerfil.Email);

            if (usuarioDiscente != null)
            {
                // Atualizando todos os campos
                usuarioDiscente.Nome = atualizarPerfil.Nome;
                usuarioDiscente.Email = atualizarPerfil.Email;
                usuarioDiscente.Telefone = atualizarPerfil.Telefone;
                usuarioDiscente.Matricula = atualizarPerfil.Matricula;
                usuarioDiscente.Curso = atualizarPerfil.Curso;

                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Se o usuário não for encontrado
        }

        public async Task<bool> AtualizarPerfilParcialAsync(AtualizarPerfilDto atualizarPerfil)
        {
            // Buscando o discente no banco de dados
            var usuarioDiscente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == atualizarPerfil.Email);

            if (usuarioDiscente != null)
            {
                // Atualizando apenas os campos fornecidos
                if (!string.IsNullOrEmpty(atualizarPerfil.Nome))
                {
                    usuarioDiscente.Nome = atualizarPerfil.Nome;
                }
                
                if (!string.IsNullOrEmpty(atualizarPerfil.Email))
                {
                    usuarioDiscente.Email = atualizarPerfil.Email;
                }

                if (!string.IsNullOrEmpty(atualizarPerfil.Telefone))
                {
                    usuarioDiscente.Telefone = atualizarPerfil.Telefone;
                }

                if (atualizarPerfil.Matricula != null)
                {
                    usuarioDiscente.Matricula = atualizarPerfil.Matricula;
                }

                if (!string.IsNullOrEmpty(atualizarPerfil.Curso))
                {
                    usuarioDiscente.Curso = atualizarPerfil.Curso;
                }

                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Se o usuário não for encontrado
        }

       public async Task<bool> DeletarUsuarioAsync(int id, string senha)
        {
            // Verifica se o discente existe no banco de dados
            var discente = await _context.Discentes.FindAsync(id);
            if (discente == null)
            {
                return false; // Discente não encontrado
            }

            // Verifica se a senha fornecida corresponde ao hash e salt armazenados
            if (!VerificarSenha(senha, discente.Senha, discente.Salt))
            {
                return false; // Senha incorreta
            }

            // Se a senha estiver correta, remover o discente
            _context.Discentes.Remove(discente);
            await _context.SaveChangesAsync();

            return true; // Exclusão realizada com sucesso
        }
    }  
}