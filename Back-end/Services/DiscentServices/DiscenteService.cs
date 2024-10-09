using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Back_end.Data;
using Back_end.Models;
using Back_end.Helpers;

namespace Back_end.Services
{
    /// <summary>
    /// Responsavel por gerenciar as operações relacionadas ao usuario
    /// </summary>
    public class DiscenteService : IDiscenteService
    {
        /// <summary>
        /// Atributo privado que representa o contexto do banco de dados
        /// </summary>
        /// <param name="context">Construtor que recebe o banco de dados</param>
        /// <param name="config">Construtor que recebe o banco de dados</param>
        private readonly ApiDbContext _context;
        private readonly IConfiguration _config;

        public DiscenteService(ApiDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Recebe as informações do discente e registra no banco de dados
        /// </summary>
        /// <param name="registro">Representa um parametro do tipo registrardiscente</param>
        /// <returns>Retorna o registro do usuario</returns>
        /// <exception cref="ArgumentNullException">Exceção para tratar informações nulas</exception>
        // Método para registrar um novo discente
        public async Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registro)
        {
            // Verificação de nulidade
            if (registro == null) throw new ArgumentNullException(nameof(registro));

            // Criptografar a senha e gerar o salt
            var (senhaCriptografada, salt) = CriptografarSenha(registro.Senha);

            var discente = new Discente
            {
                Nome = registro.Nome,
                Email = registro.Email,
                Senha = senhaCriptografada,
                Salt = salt, // Armazenar o salt no banco
                Matricula = registro.Matricula,
                Telefone = registro.Telefone,
                Cpf = registro.Cpf,
                Curso = registro.Curso
            };

            _context.Discentes.Add(discente);
            await _context.SaveChangesAsync();

            return discente;
        }

        /// <summary>
        /// Recebe as informações do discente e faz a validação do usuario
        /// </summary>
        /// <param name="login">Representa um parametro do tipo logindiscente</param>
        /// <returns>Se for verdadeiro entra no sistema, se for falso da erro</returns>
        /// <exception cref="ArgumentNullException">Exceção para tratar informações nulas</exception>
        // Método para login de discente
        public async Task<LoginResponseDto> LoginDiscenteAsync(LoginDiscente login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

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

        /// <summary>
        /// Criptografa a senha a partir do algoritmo HMACSHA512
        /// </summary>
        /// <param name="senha">Representa um parametro do tipo senhadiscente</param>
        /// <returns>Retorna a senha criptografada</returns>
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

        /// <summary>
        /// Verifica a senha fornecida corresponde a senha no banco de dados
        /// </summary>
        /// <param name="senha">Representa um parametro do tipo senhadiscente</param>
        /// <param name="senhaCriptografada">Senha criptografada</param>
        /// <param name="salt">Variavel utilizada para fazer comparação de Hash</param>
        /// <returns>Retorna verdadeiro se a senha estiver no banco de dados, e se nao retorna falso</returns>
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

        /// <summary>
        /// Gera token jwt para autenticar discentes e profissionais
        /// </summary>
        /// <param name="id">id do usuario ou profissional</param>
        /// <param name="email">email do usuario ou profissional</param>
        /// <returns>Retorna o id do usuario definido a partir de uma chave simetrica definido nas variaveis do sistema</returns>
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

        /// <summary>
        /// Verifica se o email ja foi cadastrado
        /// </summary>
        /// <param name="email">email do usuario ou profissional</param>
        /// <returns>Retorna verdadeiro caso o email ja tiver no banco de dados</returns>
        // Método para verificar se um email já está cadastrado
        public async Task<bool> EmailJaCadastradoAsync(string email)
        {
            return await _context.Discentes.AnyAsync(d => d.Email == email)
                   || await _context.Profissionais.AnyAsync(p => p.Email == email);
        }

        /// <summary>
        /// Recebe as informações do profissional e registra no banco de dados
        /// </summary>
        /// <param name="registro">Representa um parametro do tipo registrarprofissional</param>
        /// <returns>Retorna o registro do profissional</returns>
        /// <exception cref="ArgumentNullException">Exceção para tratar informações nulas</exception>
        public async Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registro)
        {
            // Verificação de nulidade
            if (registro == null) throw new ArgumentNullException(nameof(registro));

            var (senhaCriptografada, salt) = CriptografarSenha(registro.Senha);

            var profissional = new Profissional
            {
                Nome = registro.Nome,
                Email = registro.Email,
                Senha = senhaCriptografada,
                Salt = salt,
            };

            _context.Profissionais.Add(profissional);
            await _context.SaveChangesAsync();

            return profissional;
        }

<<<<<<< HEAD
        public async Task<LoginResponseDto> LoginProfissionalAsync(LoginProfissional login)
=======
        /// <summary>
        /// Recebe as informações do profissional e faz a sua validação
        /// </summary>
        /// <param name="login">Representa um parametro do ipo loginprofissional</param>
        /// <returns>Se for verdadeiro entra no sistema, se for falso da erro</returns>
        /// <exception cref="ArgumentNullException">Exceção para tratar informações nulas</exception>
        public async Task<string> LoginProfissionalAsync(LoginProfissional login)
>>>>>>> origin/back-end
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

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
<<<<<<< HEAD
=======

        /// <summary>
        /// Atualiza o perfil do discente ou profissional do sistema
        /// </summary>
        /// <param name="atualizarPerfil">Eh um parametro que recebe a model atualizarperfil</param>
        /// <returns>Se verdadeiro ele atualiza as informações do perfil</returns>
>>>>>>> origin/back-end
        public async Task<bool> AtualizarPerfilAsync(AtualizarPerfilDto atualizarPerfil)
        {
            // Buscando em ambas as tabelas: Discentes e Profissionais separadamente
            var usuarioDiscente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == atualizarPerfil.Email);
            var usuarioProfissional = await _context.Profissionais.SingleOrDefaultAsync(p => p.Email == atualizarPerfil.Email);

            if (usuarioDiscente != null)
            {
                // Verificando valores de campos string e int separadamente
                if (!string.IsNullOrEmpty(atualizarPerfil.Nome)) // Verifique se a string não está nula ou vazia
                {
                    usuarioDiscente.Nome = atualizarPerfil.Nome;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            else if (usuarioProfissional != null)
            {
                // Profissional pode não ter a propriedade Telefone, então você não a atribui aqui
                if (!string.IsNullOrEmpty(atualizarPerfil.Nome)) // Verifique se a string não está nula ou vazia
                {
                    usuarioProfissional.Nome = atualizarPerfil.Nome;
                }

                // Não há campo Telefone em Profissional, então isso não é necessário aqui

                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Nenhum usuário encontrado
        }
<<<<<<< HEAD
=======

        /// <summary>
        /// Permite que o discente ou profissional altere sua senha
        /// </summary>
        /// <param name="alterarSenha">Eh um parametro que recebe a model alterarsenha</param>
        /// <returns>Retorna se verdadeiro a senha sera alterada, se não, não altera</returns>
>>>>>>> origin/back-end
        public async Task<bool> AlterarSenhaAsync(AlterarSenhaDto alterarSenha)
        {
            // Buscando em ambas as tabelas: Discentes e Profissionais separadamente
            var usuarioDiscente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == alterarSenha.Email);
            var usuarioProfissional = await _context.Profissionais.SingleOrDefaultAsync(p => p.Email == alterarSenha.Email);

            if (usuarioDiscente != null && VerificarSenha(alterarSenha.SenhaAtual, usuarioDiscente.Senha, usuarioDiscente.Salt))
            {
                var (novaSenhaCriptografada, novoSalt) = CriptografarSenha(alterarSenha.NovaSenha);

                usuarioDiscente.Senha = novaSenhaCriptografada;
                usuarioDiscente.Salt = novoSalt;

                await _context.SaveChangesAsync();
                return true;
            }
            else if (usuarioProfissional != null && VerificarSenha(alterarSenha.SenhaAtual, usuarioProfissional.Senha, usuarioProfissional.Salt))
            {
                var (novaSenhaCriptografada, novoSalt) = CriptografarSenha(alterarSenha.NovaSenha);

                usuarioProfissional.Senha = novaSenhaCriptografada;
                usuarioProfissional.Salt = novoSalt;

                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Nenhum usuário encontrado ou senha incorreta
        }
    }
}
