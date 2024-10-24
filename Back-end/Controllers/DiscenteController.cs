﻿using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscenteController : ControllerBase
    {
        private readonly IDiscenteService _discenteService;
        private readonly IProfissionalService _profissionalService;

        public DiscenteController(IDiscenteService discenteService, IProfissionalService profissionalService)
        {
            _discenteService = discenteService;
            _profissionalService = profissionalService;
        }

        // Rota para registrar um novo discente
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

        // Rota para login de discente
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

        // Rota para registrar um novo profissional
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

        // Rota para login de profissional
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

        // Rota para obter informações do usuário atual autenticado
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

        // Rota para alteração de senha
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

        // Rota para atualização de dados pessoais
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
        
        // Endpoint para listar todos os profissionais
        [HttpGet("listar-profissionais")]
        public async Task<ActionResult<IEnumerable<ProfissionalDto>>> ListarProfissionais()
        {
            var profissionais = await _profissionalService.ListarProfissionaisAsync();
            return Ok(profissionais);
        }

       [HttpDelete("deletar-usuario")]
        public async Task<IActionResult> DeletarUsuario([FromBody] DeleteUserDto dto)
        {
            var resultado = await _discenteService.DeletarUsuarioAsync(dto.IdUsuario, dto.Senha);

            if (resultado)
            {
                return Ok(new { mensagem = "Usuário deletado com sucesso" });
            }
            else
            {
                return NotFound(new { mensagem = "Usuário não encontrado ou senha incorreta" });
            }
        }
    }   
}
