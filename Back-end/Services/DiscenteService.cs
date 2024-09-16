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
            var discente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == login.Email);

            if (discente == null || !VerificarSenha(login.Senha, discente.Senha, discente.Salt))
            {
                return null; // Retorna null se o login falhar
            }

            // Gerar o token JWT
            return GerarTokenJwt(discente);
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

        // Método para gerar o token JWT
        private string GerarTokenJwt(Discente discente)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, discente.IdDiscente.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, discente.Email),
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
        public Task<bool> EmailJaCadastradoAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
