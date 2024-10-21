using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Back_end.Controllers
{
    /// <summary>
    /// Controlador para gerenciar discentes e profissionais.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DiscenteController : ControllerBase
    {
        /// <summary>
        /// Serviço para gerenciar discentes.
        /// </summary>
        private readonly IDiscenteService _discenteService;
        
        /// <summary>
        /// Serviço para gerenciar profissionais.
        /// </summary>
        private readonly IProfissionalService _profissionalService;

        /// <summary>
        /// Construtor do DiscenteController.
        /// </summary>
        /// <param name="discenteService">Serviço injetado para operações de discentes.</param>
        /// <param name="profissionalService">Serviço injetado para operações de profissionais.</param>
        public DiscenteController(IDiscenteService discenteService, IProfissionalService profissionalService)
        {
            _discenteService = discenteService;
            _profissionalService = profissionalService;
        }

        /// <summary>
        /// Rota para registrar um novo discente.
        /// </summary>
        /// <param name="registrarDiscente">Dados do discente a ser registrado.</param>
        /// <returns>Resultado da operação de registro.</returns>
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
        /// Rota para login de discente.
        /// </summary>
        /// <param name="loginDiscente">Dados para o login do discente.</param>
        /// <returns>Resultado do login.</returns>
        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginDiscente loginDiscente)
        {
            if (loginDiscente == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var response = await _discenteService.LoginDiscenteAsync(loginDiscente);

            if (response == null)
                return Unauthorized("Login inválido");

            return Ok(response);
        }

        /// <summary>
        /// Rota para registrar um novo profissional.
        /// </summary>
        /// <param name="registrarProfissional">Dados do profissional a ser registrado.</param>
        /// <returns>Resultado do registro do profissional.</returns>
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
        /// Rota para login de profissional.
        /// </summary>
        /// <param name="loginProfissional">Dados para o login do profissional.</param>
        /// <returns>Resultado do login.</returns>
        [HttpPost("login-profissional")]
        public async Task<IActionResult> LoginProfissional([FromBody] LoginProfissional loginProfissional)
        {
            if (loginProfissional == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var response = await _discenteService.LoginProfissionalAsync(loginProfissional);

            if (response == null)
                return Unauthorized("Login inválido");

            return Ok(response);
        }

        /// <summary>
        /// Rota para obter informações do usuário atual autenticado.
        /// </summary>
        /// <returns>Dados do usuário autenticado.</returns>
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

        /// <summary>
        /// Rota para alteração de senha.
        /// </summary>
        /// <param name="alterarSenha">Dados para a alteração da senha.</param>
        /// <returns>Resultado da alteração de senha.</returns>
        [HttpPut("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDto alterarSenha)
        {
            if (alterarSenha == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var resultado = await _discenteService.AlterarSenhaAsync(alterarSenha);

            if (!resultado)
            {
                return BadRequest("Falha ao alterar a senha. Verifique suas credenciais.");
            }

            return Ok("Senha alterada com sucesso.");
        }

        /// <summary>
        /// Rota para atualização de dados pessoais.
        /// </summary>
        /// <param name="atualizarPerfil">Dados do perfil a ser atualizado.</param>
        /// <returns>Resultado da atualização do perfil.</returns>
        [HttpPut("atualizar-perfil")]
        public async Task<IActionResult> AtualizarPerfil([FromBody] AtualizarPerfilDto atualizarPerfil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _discenteService.AtualizarPerfilCompletoAsync(atualizarPerfil);

            if (resultado)
            {
                return Ok("Perfil atualizado com sucesso.");
            }

            return NotFound("Discente não encontrado.");
        }

        /// <summary>
        /// Rota para atualização parcial de dados pessoais.
        /// </summary>
        /// <param name="atualizarPerfil">Dados do perfil a serem atualizados.</param>
        /// <returns>Resultado da atualização parcial do perfil.</returns>
        [HttpPatch("atualizar-perfil-parcial")]
        public async Task<IActionResult> AtualizarPerfilParcial([FromBody] AtualizarPerfilDto atualizarPerfil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _discenteService.AtualizarPerfilParcialAsync(atualizarPerfil);

            if (resultado)
            {
                return Ok("Perfil atualizado com sucesso.");
            }

            return NotFound("Discente não encontrado.");
        }

        /// <summary>
        /// Rota para obter um discente específico pelo ID.
        /// </summary>
        /// <param name="id">ID do discente a ser obtido.</param>
        /// <returns>Dados do discente, se encontrado.</returns>
        [HttpGet("obter-discente/{id}")]
        public async Task<IActionResult> ObterDiscentePorId(int id)
        {
            // Procurar o discente pelo ID
            var discente = await _discenteService.ObterDiscentePorIdAsync(id);

            if (discente == null)
            {
                return NotFound("Discente não encontrado.");
            }

            // Retornar os dados do discente
            return Ok(discente);
        }

        /// <summary>
        /// Rota para obter um profissional específico pelo ID.
        /// </summary>
        /// <param name="id">ID do profissional a ser obtido.</param>
        /// <returns>Dados do profissional, se encontrado.</returns>
        [HttpGet("obter-profissional/{id}")]
        public async Task<IActionResult> ObterProfissional(int id)
        {
            var profissional = await _profissionalService.ObterProfissionalPorIdAsync(id);

            if (profissional == null)
            {
                return NotFound("Profissional não encontrado.");
            }

            return Ok(profissional);
        }
        
        /// <summary>
        /// Endpoint para listar todos os profissionais.
        /// </summary>
        /// <returns>Uma lista de profissionais disponíveis.</returns>
        [HttpGet("listar-profissionais")]
        public async Task<ActionResult<IEnumerable<ProfissionalDto>>> ListarProfissionais()
        {
            var profissionais = await _profissionalService.ListarProfissionaisAsync();
            return Ok(profissionais);
        }
        
        /// <summary>
        /// Rota para deletar um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado.</param>
        /// <returns>Resultado da operação de deleção.</returns>
        [HttpDelete("deletar-usuario/{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            var resultado = await _discenteService.DeletarUsuarioAsync(id);
            
            if (resultado)
            {
                return Ok(new { mensagem = "Usuário deletado com sucesso" });
            }
            else
            {
                return NotFound(new { mensagem = "Usuário não encontrado" });
            }
        }
    }
}
