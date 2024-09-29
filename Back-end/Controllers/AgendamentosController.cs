using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Dtos;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

/// <summary>
/// Controlador responsável pelas operações de agendamento,
/// como solicitar, cancelar e listar agendamentos
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AgendamentoController : ControllerBase
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="AgendamentoController"/>.
    /// </summary>
    /// <param name="agendamentoService">Serviço de agendamento que será utilizado para as operações.</param>
    private readonly IAgendamentoService _agendamentoService;
    public AgendamentoController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    /// <summary>
    /// Solicita um novo agendamento.
    /// </summary>
    /// <param name="dto">Os dados do agendamento a ser solicitado.</param>
    /// <returns>Um resultado da operação de solicitação de agendamento, incluindo os detalhes do agendamento solicitado.</returns>
    [HttpPost("solicitar")]
    public async Task<IActionResult> SolicitarAgendamento(SolicitarAgendamentoDto dto)
    {
        var result = await _agendamentoService.SolicitarAgendamentoAsync(dto);
        if (result == null)
        {
            return BadRequest("Horário indisponível.");
        }
        return Ok(result);
    }

    /// <summary>
    /// Cancela um agendamento existente.
    /// </summary>
    /// <param name="id">O ID do agendamento a ser cancelado.</param>
    /// <returns>Um resultado da operação de cancelamento. Retorna 404 se o agendamento não for encontrado.</returns>
    [HttpDelete("cancelar/{id}")]
    public async Task<IActionResult> CancelarAgendamento(int id)
    {
        var result = await _agendamentoService.CancelarAgendamentoAsync(id);
        if (!result)
        {
            return NotFound("Agendamento não encontrado.");
        }
        return NoContent();
    }

    /// <summary>
    /// Lista todos os agendamentos de um discente específico.
    /// </summary>
    /// <param name="discenteId"></param>
    /// <returns>Uma lista de agendamentos associados ao discente.</returns>
    [HttpGet("listar/discente/{discenteId}")]
    public async Task<IActionResult> ListarAgendamentosPorDiscente(int discenteId)
    {
        var result = await _agendamentoService.ListarAgendamentosPorDiscenteAsync(discenteId);
        return Ok(result);
    }

    /// <summary>
    /// Lista os horários disponíveis para um profissional específico.
    /// </summary>
    /// <param name="profissionalId">O ID do profissional cujos horários serão listados.</param>
    /// <returns>Uma lista de horários disponíveis para o profissional.</returns>
    [HttpGet("horarios/{profissionalId}")]
    public async Task<IActionResult> ListarHorariosDisponiveis(int profissionalId)
    {
        var result = await _agendamentoService.ListarHorariosDisponiveisAsync(profissionalId);
        return Ok(result);
    }
}
