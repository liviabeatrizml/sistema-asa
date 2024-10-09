using Back_end.Data;
using Back_end.Dtos;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Services
{
    /// <summary>
    /// Responsavel por gerenciar as operações relacionadas a agendamento de serviço no sistema
    /// </summary>
    public class AgendamentoService : IAgendamentoService
    {
        /// <summary>
        /// Atributo privado que representa o contexto do banco de dados
        /// </summary>
        /// <param name="context">Construtor que recebe o banco de dados</param>
        private readonly ApiDbContext _context;

        public AgendamentoService(ApiDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Permite que o discente solicita um novo agendamento com base no horario disponivel 
        /// para o profissional e serviço especificado
        /// </summary>
        /// <param name="dto">Representar conexão com banco de dados</param>
        /// <returns>Um novo agentademento ou um erro</returns>
        // Solicitar agendamento
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
        /// Cancela o agendamento
        /// </summary>
        /// <param name="agendamentoId">Busca o id agendamento para cancela-lo</param>
        /// <returns>Cancela o agendamente de acordo com o id, se ele não existir retorna falso</returns>
        // Cancelar agendamento
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
        /// Lista os agendamentos disponiveis
        /// </summary>
        /// <param name="discenteId">Id que o discente ira mandar para receber a lista</param>
        /// <returns>Retorna os agendamentos disponiveis de acordo com id dos discentes</returns>
        // Listar agendamentos por discente
        public async Task<List<Agendamento>> ListarAgendamentosPorDiscenteAsync(int discenteId)
        {
            return await _context.Agendamento
                .Where(a => a.DiscenteId == discenteId)
                .ToListAsync();
        }

        /// <summary>
        /// Lista os horarios disponiveis
        /// </summary>
        /// <param name="profissionalId">Id dos profissionais para saber quais estão disponiveis</param>
        /// <returns>Retorna os horarios disponiveis de um profissional especifico de acordo com seu id</returns>
        // Listar horários disponíveis de um profissional
        public async Task<List<HorarioDisponivel>> ListarHorariosDisponiveisAsync(int profissionalId)
        {
            return await _context.HorarioDisponivel
                .Where(h => h.ProfissionalId == profissionalId)
                .ToListAsync();
        }
    }
}
