using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Dtos;
using Back_end.Models;

namespace Back_end.Services
{   
    /// <summary>
    /// Interface que define os contratos que devem ser implementados pela
    /// classe AgendamentoService
    /// </summary>
    public interface IAgendamentoService
    {
        Task<Agendamento> SolicitarAgendamentoAsync(SolicitarAgendamentoDto dto);
        Task<bool> CancelarAgendamentoAsync(int agendamentoId);
        Task<List<Agendamento>> ListarAgendamentosPorDiscenteAsync(int discenteId);
        Task<List<HorarioDisponivel>> ListarHorariosDisponiveisAsync(int profissionalId);
    }
}
