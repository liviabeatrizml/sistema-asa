using Back_end.Data;
using Back_end.Dtos;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly ApiDbContext _context;

        public AgendamentoService(ApiDbContext context)
        {
            _context = context;
        }

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

        // Listar agendamentos por discente
        public async Task<List<Agendamento>> ListarAgendamentosPorDiscenteAsync(int discenteId)
        {
            return await _context.Agendamento
                .Where(a => a.DiscenteId == discenteId)
                .ToListAsync();
        }

        // Listar horários disponíveis de um profissional
        public async Task<List<HorarioDisponivel>> ListarHorariosDisponiveisAsync(int profissionalId)
        {
            return await _context.HorarioDisponivel
                .Where(h => h.ProfissionalId == profissionalId)
                .ToListAsync();
        }
    }
}
