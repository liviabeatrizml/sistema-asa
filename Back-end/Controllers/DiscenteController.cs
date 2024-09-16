using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscenteController : ControllerBase
    {
        private readonly IDiscenteService _discenteService;

        public DiscenteController(IDiscenteService discenteService)
        {
            _discenteService = discenteService;
        }

        [HttpPost("registrar")]
        [Consumes("application/json")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDiscente registrarDiscente)
        {
            var discente = await _discenteService.RegistrarDiscenteAsync(registrarDiscente);
            return Ok(new { Id = discente.IdDiscente, Email = discente.Email });
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginDiscente loginDiscente)
        {
            // Tenta logar o usuário e gerar o token
            var token = await _discenteService.LoginDiscenteAsync(loginDiscente);

            if (token == null)
                return Unauthorized("Login inválido");

            return Ok(new { Token = token });
        }
    }
}