using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Back_end.Data;
using Back_end.Models;
using Back_end.Helpers;
using System;//Contratos
using System.Diagnostics.Contracts;

namespace Back_end.Services
{
    public class DiscenteService : IDiscenteService
    {
        /// <summary>
        /// Serviço responsável por gerenciar as operações relacionadas a discentes.
        /// </summary>
        /// <param name="context">Contexto da base de dados.</param>
        /// <param name="config">Configurações da aplicação.</param>
        private readonly ApiDbContext _context;
        private readonly IConfiguration _config;
        
        public DiscenteService(ApiDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // RF010 e RF011 - Método para registrar um novo discente
        /// <summary>
        /// Registra um novo discente no sistema.
        /// </summary>
        /// <param name="registro">Os dados do discente a serem registrados.</param>
        /// <returns>Um objeto <see cref="Discente"/> representando o discente registrado.</returns>
        /// <exception cref="ArgumentNullException">Lançado quando <paramref name="registro"/> é nulo.</exception>
        /// <remarks>
        /// Este método realiza as seguintes validações:
        /// - O nome do discente não pode ser nulo.
        /// - O e-mail deve ser um endereço válido com o domínio '@alunos.ufersa.edu.br'.
        /// - A senha deve ter pelo menos 8 caracteres, incluindo um caractere especial e um número.
        /// - A matrícula deve ter exatamente 10 caracteres.
        /// </remarks>
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
            Contract.Requires(ValidarSenha(registro.Senha),
                "A senha deve ter no mínimo 8 caracteres, incluindo um caractere especial e um número.");
            Contract.Requires(registro.Matricula != null, nameof(registro.Matricula));
            Contract.Requires(registro.Matricula.Length == 10, "A matrícula deve ter exatamente 10 caracteres.");

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

            return discente;
        }

        // ---------------------------RF0 VALIDADORES--------------------------------
        /// <summary>
        /// Valida as invariantes do discente, garantindo que os dados essenciais estejam corretos.
        /// </summary>
        /// <remarks>
        /// Este método verifica as seguintes condições:
        /// - O nome do discente não pode ser nulo.
        /// - O e-mail do discente não pode ser nulo e deve conter o domínio '@alunos.ufersa.edu.br'.
        /// - A senha do discente não pode ser nula e deve atender aos critérios de complexidade.
        /// - A matrícula do discente não pode ser nula e deve ter exatamente 10 caracteres.
        /// Caso alguma das invariantes não seja atendida, uma exceção será lançada.
        /// </remarks>
        public void ValidarInvariantes(){
            Contract.Invariant(Nome != null, "O nome do discente não pode ser nulo.");
            Contract.Invariant(Email != null, "O email do discente não pode ser nulo.");
            Contract.Invariant(
                Email.EndsWith("@alunos.ufersa.edu.br"),
                "O email do discente deve conter o domínio '@alunos.ufersa.edu.br'."
            );
            Contract.Invariant(Senha != null, "A senha do discente não pode ser nula.");
            Contract.Invariant(ValidarSenha(Senha),
                "A senha deve ter no mínimo 8 caracteres, incluindo um caractere especial e um número."
            );
            Contract.Invariant(Matricula != null, "A matrícula do discente não pode ser nula.");
            Contract.Invariant(Matricula.Length == 10, "A matrícula deve ter exatamente 10 caracteres.");
        }

        /// <summary>
        /// Valida a complexidade da senha de acordo com os critérios estabelecidos.
        /// </summary>
        /// <param name="senha">A senha a ser validada.</param>
        /// <returns>Retorna <c>true</c> se a senha for válida; caso contrário, <c>false</c>.</returns>
        /// <remarks>
        /// A senha é considerada válida se atender aos seguintes critérios:
        /// - Ter no mínimo 8 caracteres.
        /// - Contém pelo menos um caractere especial (ex.: !@#$%^&*(),.?":{}|<>).
        /// - Contém pelo menos um número.
        /// </remarks>
        private bool ValidarSenha(string senha){
            // A senha deve ter no mínimo 8 caracteres
            if (senha.Length < 8) return false;

            // A senha deve conter pelo menos um caractere especial
            if (!Regex.IsMatch(senha, @"[!@#$%^&*(),.?\":{ }|<>]")) return false;
       
            // A senha deve conter pelo menos um número
            if (!Regex.IsMatch(senha, @"\d")) return false;

            return true;
        }
        // ---------------------------RF0 VALIDADORES--------------------------------

        // RF0Extra Método para login de discente
        /// <summary>
        /// Realiza o login de um discente, validando suas credenciais e gerando um token JWT.
        /// </summary>
        /// <param name="login">O objeto contendo as informações de login do discente.</param>
        /// <returns>Um objeto <see cref="LoginResponseDto"/> contendo o ID do usuário e o token JWT, ou <c>null</c> se as credenciais forem inválidas.</returns>
        /// <exception cref="ArgumentNullException">Lançado quando o parâmetro <paramref name="login"/> é nulo.</exception>
        public async Task<LoginResponseDto> LoginDiscenteAsync(LoginDiscente login)
        {
            Contract.Requires(login != null, nameof(login));

            if (login == null) throw new ArgumentNullException(nameof(login));

            var discente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == login.Email);
            
            Contract.Requires(discente != null);
            Contract.Requires(discente.Senha != null);
            Contract.Requires(discente.Salt);

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
        /// Criptografa a senha fornecida usando um algoritmo HMACSHA512 e gera um salt único.
        /// </summary>
        /// <param name="senha">A senha que será criptografada.</param>
        /// <returns>Uma tupla contendo a senha criptografada e o salt gerado.</returns>
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
        /// Verifica se a senha fornecida corresponde à senha criptografada usando o salt.
        /// </summary>
        /// <param name="senha">A senha que será verificada.</param>
        /// <param name="senhaCriptografada">A senha criptografada armazenada no banco de dados.</param>
        /// <param name="salt">O salt usado para criptografar a senha.</param>
        /// <returns>Um valor booleano indicando se a senha fornecida é válida.</returns>
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
        /// Gera um token JWT (JSON Web Token) para autenticação de um usuário.
        /// </summary>
        /// <param name="id">O ID do usuário para o qual o token está sendo gerado.</param>
        /// <param name="email">O email do usuário que será incluído nas reivindicações do token.</param>
        /// <returns>O token JWT gerado.</returns>
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
        /// Verifica se um email já está cadastrado no sistema, tanto para discentes quanto para profissionais.
        /// </summary>
        /// <param name="email">O email a ser verificado.</param>
        /// <returns>Um valor booleano indicando se o email já está cadastrado.</returns>
        // Método para verificar se um email já está cadastrado
        public async Task<bool> EmailJaCadastradoAsync(string email)
        {
            return await _context.Discentes.AnyAsync(d => d.Email == email)
                   || await _context.Profissionais.AnyAsync(p => p.Email == email);
        }

        // RF010 e RF011 - Método que Registra um Profissional
        /// <summary>
        /// Registra um novo profissional no sistema.
        /// </summary>
        /// <param name="registro">O objeto contendo os dados do profissional a ser registrado.</param>
        /// <returns>Um objeto do tipo <see cref="Profissional"/> representando o profissional registrado.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o objeto <paramref name="registro"/> é nulo.</exception>
        /// <exception cref="ArgumentException">Lançada quando o <paramref name="registro.ServicoId"/> fornecido não é válido.</exception>
        public async Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registro)
        {
            if (registro == null) throw new ArgumentNullException(nameof(registro));

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
                ServicoId = registro.ServicoId // Associar o ServicoId ao profissional
            };

            _context.Profissionais.Add(profissional);
            await _context.SaveChangesAsync();

            return profissional;
        }

        // RF0Extra - Login do Profissional
        /// <summary>
        /// Realiza o login de um profissional no sistema.
        /// </summary>
        /// <param name="login">O objeto contendo os dados de login do profissional.</param>
        /// <returns>Um objeto do tipo <see cref="LoginResponseDto"/> contendo o ID do usuário e o token de autenticação.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o objeto <paramref name="login"/> é nulo.</exception>
        public async Task<LoginResponseDto> LoginProfissionalAsync(LoginProfissional login)
        {
            Contract.Requires(login != null, nameof(login));

            if (login == null) throw new ArgumentNullException(nameof(login));

            var profissional = await _context.Profissionais.SingleOrDefaultAsync(p => p.Email == login.Email);

            Contract.Requires(profissional != null);
            Contract.Requires(profissional.Senha != null);
            Contract.Requires(profissional.Salt);

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

        //RF012
        /// <summary>
        /// Atualiza as informações do perfil de um discente no sistema.
        /// </summary>
        /// <param name="atualizarPerfil">O objeto contendo os novos dados para atualização do perfil.</param>
        /// <returns>Um valor booleano que indica se a atualização foi bem-sucedida.</returns>
        public async Task<bool> AtualizarPerfilAsync(AtualizarPerfilDto atualizarPerfil)
        {
            // Buscando o discente no banco de dados
            var usuarioDiscente = await _context.Discentes.SingleOrDefaultAsync(d => d.Email == atualizarPerfil.Email);

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

            return false; // Se o usuário não for encontrado
        }

        //RF012
        /// <summary>
        /// Altera a senha de um discente ou profissional no sistema.
        /// </summary>
        /// <param name="alterarSenha">O objeto contendo as informações necessárias para a alteração de senha.</param>
        /// <returns>Um valor booleano que indica se a alteração de senha foi bem-sucedida.</returns>
        public async Task<bool> AlterarSenhaAsync(AlterarSenhaDto alterarSenha)
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

        /// <summary>
        /// Obtém os detalhes de um discente com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do discente a ser buscado.</param>
        /// <returns>Um objeto <see cref="DiscenteDto"/> contendo as informações do discente, ou null se não for encontrado.</returns>
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

        /// <summary>
        /// Obtém os detalhes de um profissional com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do profissional a ser buscado.</param>
        /// <returns>Um objeto <see cref="ProfissionalDto"/> contendo as informações do profissional, ou null se não for encontrado.</returns>
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

        /// <summary>
        /// Atualiza o perfil completo de um discente com base nas informações fornecidas.
        /// </summary>
        /// <param name="atualizarPerfil">Um objeto <see cref="AtualizarPerfilDto"/> contendo os novos dados do discente.</param>
        /// <returns>Um valor booleano indicando se a atualização foi bem-sucedida.</returns>
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

        /// <summary>
        /// Atualiza parcialmente o perfil de um discente com base nas informações fornecidas.
        /// Somente os campos que não são nulos ou vazios serão atualizados.
        /// </summary>
        /// <param name="atualizarPerfil">Um objeto <see cref="AtualizarPerfilDto"/> contendo os novos dados do discente.</param>
        /// <returns>Um valor booleano indicando se a atualização foi bem-sucedida.</returns>

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

        ///<summary>
        ///Exclui um usuário com base no ID fornecido.
        ///</summary>
        ///<param name="id">O ID do usuário a ser excluído.</param>
        ///<returns>True se o usuário foi excluído com sucesso, false caso contrário.</returns>
        public async Task<bool> DeletarUsuarioAsync(int id)
        {
            var discente = await _context.Discentes.FindAsync(id);
            if (discente != null)
            {
                _context.Discentes.Remove(discente);
                await _context.SaveChangesAsync();
                return true;
            }

            var profissional = await _context.Profissionais.FindAsync(id);
            if (profissional != null)
            {
                _context.Profissionais.Remove(profissional);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

    }
    
}
