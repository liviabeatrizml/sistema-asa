using Back_end.Data;
using Back_end.Dtos;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Diagnostics.Contracts;

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

            Contract.Requires(dto != null, "O objeto de solicitação de agendamento não pode ser nulo.");
            Contract.Requires(dto.DiscenteId > 0, "O ID do discente deve ser válido.");
            Contract.Requires(dto.HorarioId > 0, "O ID do horário deve ser válido.");
            Contract.Requires(dto.ProfissionalId > 0, "O ID do profissional deve ser válido.");
            Contract.Requires(dto.ServicoId > 0, "O ID do serviço deve ser válido.");
            Contract.Requires(dto.Data != default(DateTime), "A data do agendamento deve ser válida.");
            Contract.Requires(!string.IsNullOrEmpty(dto.Status), "O status do agendamento não pode ser nulo ou vazio.");

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
                ServicoId = dto.ServicoId,
                Status = dto.Status
            };

            _context.Agendamento.Add(novoAgendamento);
            await _context.SaveChangesAsync();

            Contract.Ensures(novoAgendamento != null, "O novo agendamento deve ter sido criado com sucesso.");

            return novoAgendamento;
        }

        // Cancelar agendamento
        public async Task<bool> CancelarAgendamentoAsync(int agendamentoId)
        {
            Contract.Requires(agendamentoId > 0, "O ID do agendamento deve ser maior que zero.");

            var agendamento = await _context.Agendamento.FindAsync(agendamentoId);
            if (agendamento == null)
            {
                return false; // Agendamento não encontrado
            }

            _context.Agendamento.Remove(agendamento);
            await _context.SaveChangesAsync();

            Contract.Ensures(agendamento == null, "O agendamento deve ter sido removido do banco de dados.");

            return true;
        }

        // Listar agendamentos por discente
        public async Task<List<Agendamento>> ListarAgendamentosPorDiscenteAsync(int discenteId)
        {
            Contract.Requires(discenteId > 0, "O ID do discente deve ser maior que zero.");

            return await _context.Agendamento
                .Where(a => a.DiscenteId == discenteId)
                .ToListAsync();
        }

        // Listar horários disponíveis de um profissional
        public async Task<List<HorarioDisponivel>> ListarHorariosDisponiveisAsync(int profissionalId)
        {
            Contract.Requires(profissionalId > 0, "O ID do profissional deve ser maior que zero.");

            return await _context.HorarioDisponivel
                .Where(h => h.ProfissionalId == profissionalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgendamentoDto>> BuscarAgendamentosPorProfissionalAsync(int profissionalId)
        {
            Contract.Requires(profissionalId > 0, "O ID do profissional deve ser maior que zero.");

            var agendamentos = await _context.Agendamento
                .Where(a => a.ProfissionalId == profissionalId && a.Data >= DateTime.Today)
                .Join(_context.HorarioDisponivel, 
                    a => a.HorarioId, 
                    h => h.IdHorario, 
                    (a, h) => new AgendamentoDto
                    {
                        IdAgendamento = a.IdAgendamento,
                        Data = a.Data,
                        DiscenteId = a.DiscenteId,
                        ProfissionalId = a.ProfissionalId,
                        ServicoId = a.ServicoId,
                        HorarioId = a.HorarioId,
                        HoraInicio = h.HoraInicio,
                        HoraFim = h.HoraFim,
                        Status = a.Status
                    })
                .ToListAsync();

            return agendamentos;
        }

    }
}