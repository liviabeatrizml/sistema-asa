using Back_end.Data;
using Back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ServicoController(ApiDbContext context)
        {
            _context = context;
        }

        // POST: api/Servico/cadastrar
        [HttpPost("cadastrar")]
        public async Task<ActionResult<ServicoDisponivel>> CadastrarServico(ServicoDto servicoDto)
        {
            var novoServico = new ServicoDisponivel
            {
                Tipo = servicoDto.Tipo,
                Descricao = servicoDto.Descricao,
                TipoAtendimento = servicoDto.TipoAtendimento
            };

            _context.ServicoDisponivel.Add(novoServico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServico), new { id = novoServico.IdServico }, novoServico);
        }

        // GET: api/Servico/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicoDisponivel>> GetServico(int id)
        {
            var servico = await _context.ServicoDisponivel.FindAsync(id);

            if (servico == null)
            {
                return NotFound();
            }

            return servico;
        }

        // GET: api/Servico/listar
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<ServicoDisponivel>>> ListarServicos()
        {
            return await _context.ServicoDisponivel.ToListAsync();
        }
    }
}
