using Back_end.Data;
using Back_end.Dtos;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        /// <summary>
        /// Contexto do banco de dados para operações de agendamento.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        private readonly ApiDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="AgendamentoService"/>.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        public AgendamentoService(ApiDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Solicita um novo agendamento.
        /// </summary>
        /// <param name="dto">Os dados do agendamento a serem solicitados.</param>
        /// <returns>Um objeto <see cref="Agendamento"/> resultante da solicitação.</returns>
        public async Task<Agendamento> SolicitarAgendamentoAsync(SolicitarAgendamentoDto dto)
        {
            // Verifica se o horário está disponível
            var horarioDisponivel = await _context.HorarioDisponivel
                .FirstOrDefaultAsync(h => h.IdHorario == dto.HorarioId && h.ProfissionalId == dto.ProfissionalId);

            if (horarioDisponivel == null)
            {
                return null; // Horário indisponível
            }

            // Cria um novo agendamento
            var novoAgendamento = new Agendamento
            {
                Data = dto.Data,
                DiscenteId = dto.DiscenteId,
                HorarioId = dto.HorarioId,
                ProfissionalId = dto.ProfissionalId,
                ServicoId = dto.ServicoId
            };

            _context.Agendamento.Add(novoAgendamento);
            await _context.SaveChangesAsync();

            return novoAgendamento;
        }

        /// <summary>
        /// Cancela um agendamento existente.
        /// </summary>
        /// <param name="agendamentoId">O ID do agendamento a ser cancelado.</param>
        /// <returns>Um valor indicando se o cancelamento foi bem-sucedido.</returns>
        public async Task<bool> CancelarAgendamentoAsync(int agendamentoId)
        {
            var agendamento = await _context.Agendamento.FindAsync(agendamentoId);
            if (agendamento == null)
            {
                return false; // Agendamento não encontrado
            }

            _context.Agendamento.Remove(agendamento);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Lista os agendamentos de um discente específico.
        /// </summary>
        /// <param name="discenteId">O ID do discente cujos agendamentos serão listados.</param>
        /// <returns>Uma lista de agendamentos do discente.</returns>
        public async Task<List<Agendamento>> ListarAgendamentosPorDiscenteAsync(int discenteId)
        {
            return await _context.Agendamento
                .Where(a => a.DiscenteId == discenteId)
                .ToListAsync();
        }

        /// <summary>
        /// Lista os horários disponíveis para um profissional específico.
        /// </summary>
        /// <param name="profissionalId">O ID do profissional cujos horários disponíveis serão listados.</param>
        /// <returns>Uma lista de horários disponíveis para o profissional.</returns>
        public async Task<List<HorarioDisponivel>> ListarHorariosDisponiveisAsync(int profissionalId)
        {
            return await _context.HorarioDisponivel
                .Where(h => h.ProfissionalId == profissionalId)
                .ToListAsync();
        }
    }
}
