using Back_end.Data;
using Back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Back_end.Controllers
{
    /// <summary>
    /// Controlador para gerenciar serviços disponíveis.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ServicoController : ControllerBase
    {
        /// <summary>
        /// Contexto do banco de dados para acessar os serviços.
        /// </summary>
        private readonly ApiDbContext _context;

        /// <summary>
        /// Construtor do ServicoController.
        /// </summary>
        /// <param name="context">Contexto do banco de dados injetado.</param>
        public ServicoController(ApiDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cadastra um novo serviço.
        /// </summary>
        /// <param name="servicoDto">Dados do serviço a ser cadastrado.</param>
        /// <returns>Um resultado contendo o serviço cadastrado.</returns>
        // POST: api/Servico/cadastrar
        [HttpPost("cadastrar")]
        public async Task<ActionResult<ServicoDisponivel>> CadastrarServico(ServicoDto servicoDto)
        {
            var novoServico = new ServicoDisponivel
            {
                Tipo = servicoDto.Tipo,
                status = servicoDto.status
            };

            _context.ServicoDisponivel.Add(novoServico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServico), new { id = novoServico.IdServico }, novoServico);
        }

        /// <summary>
        /// Obtém um serviço específico pelo ID.
        /// </summary>
        /// <param name="id">ID do serviço a ser obtido.</param>
        /// <returns>Um resultado contendo o serviço, se encontrado.</returns>
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

        /// <summary>
        /// Lista todos os serviços disponíveis.
        /// </summary>
        /// <returns>Uma lista de serviços disponíveis.</returns>
        // GET: api/Servico/listar
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<ServicoDisponivel>>> ListarServicos()
        {
            return await _context.ServicoDisponivel.ToListAsync();
        }
    }
}
