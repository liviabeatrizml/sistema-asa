using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Xml;

//namespace contendo API do sistema Back-end
namespace Back_end.Controllers
{
    /// <summary>
    /// Contém os controladores da API do sistema Back-end, 
    /// responsáveis por gerenciar as operações relacionadas aos discentes e profissionais.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DiscenteController : ControllerBase
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="DiscenteController"/> e configura o serviço de discentes.
        /// </summary>
        /// <param name="discenteService">O serviço de discente que será utilizado para as operações de registro e login.</param>
        private readonly IDiscenteService _discenteService;
        public DiscenteController(IDiscenteService discenteService)
        {
            _discenteService = discenteService;
        }

        /// <summary>
        /// Registra um novo discente no sistema.
        /// </summary>
        /// <param name="registrarDiscente">Objeto contendo os dados do discente a ser registrado.</param>
        /// <returns>Retorna o ID e o email do discente registrado ou um erro de registro.</returns>
        [HttpPost("registrar")]
        [Consumes("application/json")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDiscente registrarDiscente)
        {
            if (registrarDiscente == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var discente = await _discenteService.RegistrarDiscenteAsync(registrarDiscente);
            if (discente == null)
            {
                return StatusCode(500, "Erro ao registrar o discente.");
            }

            return Ok(new { Id = discente.IdDiscente, Email = discente.Email });
        }

        /// <summary>
        /// Realiza o login de um discente no sistema e gera um token de autenticação.
        /// </summary>
        /// <param name="loginDiscente">Objeto contendo as credenciais do discente para login.</param>
        /// <returns>Retorna um token de autenticação ou um erro se as credenciais forem inválidas.</returns>
        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginDiscente loginDiscente)
        {
            if (loginDiscente == null)
            {
                return BadRequest("Dados inválidos.");
            }

            // Tenta logar o usuário e gerar o token
            var token = await _discenteService.LoginDiscenteAsync(loginDiscente);

            if (token == null)
                return Unauthorized("Login inválido");

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Registra um novo profissional no sistema.
        /// </summary>
        /// <param name="registrarProfissional">Objeto contendo os dados do profissional a ser registrado.</param>
        /// <returns>Retorna os detalhes do profissional registrado ou um erro se os dados forem inválidos ou se o e-mail já estiver cadastrado.</returns>
        [HttpPost("registrar-profissional")]
        public async Task<IActionResult> RegistrarProfissional([FromBody] RegistrarProfissional registrarProfissional)
        {
            if (registrarProfissional == null)
            {
                return BadRequest("Dados inválidos.");
            }

            if (await _discenteService.EmailJaCadastradoAsync(registrarProfissional.Email))
            {
                return BadRequest("Email já cadastrado");
            }

            var profissional = await _discenteService.RegistrarProfissionalAsync(registrarProfissional);
            if (profissional == null)
            {
                return StatusCode(500, "Erro ao registrar o profissional.");
            }

            return Ok(new { Id = profissional.IdProfissional, Email = profissional.Email });
        }

        /// <summary>
        /// Realiza o login de um profissional no sistema e gera um token de autenticação.
        /// </summary>
        /// <param name="loginProfissional">Objeto contendo as credenciais do profissional (email e senha).</param>
        /// <returns>Retorna um token de autenticação se o login for bem-sucedido ou um erro se os dados forem inválidos ou o login falhar.</returns>
        [HttpPost("login-profissional")]
        public async Task<IActionResult> LoginProfissional([FromBody] LoginProfissional loginProfissional)
        {
            if (loginProfissional == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var token = await _discenteService.LoginProfissionalAsync(loginProfissional);

            if (token == null)
                return Unauthorized("Login inválido");

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Obtém as informações do usuário autenticado, retornando o ID do usuário.
        /// </summary>
        /// <returns>Retorna o ID do usuário autenticado ou um erro se o usuário não estiver autenticado.</returns>
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            // Obtendo o userId da claim "sub" (subject) no JWT
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            return Ok(new { UserId = userId });
        }
    }
}
