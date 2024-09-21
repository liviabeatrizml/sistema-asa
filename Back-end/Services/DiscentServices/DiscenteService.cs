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

        // Método para login de discente
        public async Task<string> LoginDiscenteAsync(LoginDiscente login)
        {
            // Verificação de nulidade
            if (login == null) throw new ArgumentNullException(nameof(login));

            // Buscar o discente no banco de dados
            var discente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == login.Email);

            // Garantir que o discente não seja nulo e que os campos de senha e salt estejam preenchidos
            if (discente == null || string.IsNullOrEmpty(discente.Senha) || string.IsNullOrEmpty(discente.Salt))
            {
                return null; // Retorna null se os dados estiverem ausentes ou inválidos
            }

            // Verificar a senha
            if (!VerificarSenha(login.Senha, discente.Senha, discente.Salt))
            {
                return null; // Retorna null se a verificação de senha falhar
            }

            // Gerar o token JWT
            return GerarTokenJwt(discente.IdDiscente.ToString(), discente.Email ?? string.Empty);
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

        public async Task<string> LoginProfissionalAsync(LoginProfissional login)
        {
            // Verificação de nulidade
            if (login == null) throw new ArgumentNullException(nameof(login));

            var profissional = await _context.Profissionais
                .SingleOrDefaultAsync(p => p.Email == login.Email);

            // Garantir que os campos não sejam nulos antes de usá-los
            if (profissional == null || string.IsNullOrEmpty(profissional.Senha) || string.IsNullOrEmpty(profissional.Salt))
            {
                return null; // Retorna null se o login falhar devido a valores inválidos
            }

            if (!VerificarSenha(login.Senha, profissional.Senha, profissional.Salt))
            {
                return null; // Retorna null se a verificação de senha falhar
            }

            // Gerar o token JWT
            string idProfissional = profissional.IdProfissional.ToString();
            string email = profissional.Email ?? string.Empty;  // Garantir que o email não seja nulo

            return GerarTokenJwt(idProfissional, email);
        }

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
