using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Dtos;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controlador para gerenciar agendamentos.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AgendamentoController : ControllerBase
{
    /// <summary>
    /// Serviço de agendamento utilizado para operações relacionadas a agendamentos.
    /// </summary>
    private readonly IAgendamentoService _agendamentoService;

    /// <summary>
    /// Construtor do AgendamentoController.
    /// </summary>
    /// <param name="agendamentoService">Serviço de agendamento injetado.</param>
    public AgendamentoController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    /// <summary>
    /// Solicita um novo agendamento.
    /// </summary>
    /// <param name="dto">Dados do agendamento a serem solicitados.</param>
    /// <returns>Um resultado indicando o status da solicitação de agendamento.</returns>
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
    /// <param name="id">ID do agendamento a ser cancelado.</param>
    /// <returns>Um resultado indicando o status da operação de cancelamento.</returns>
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
    /// <param name="discenteId">ID do discente cujos agendamentos devem ser listados.</param>
    /// <returns>Uma lista de agendamentos do discente.</returns>
    [HttpGet("listar/discente/{discenteId}")]
    public async Task<IActionResult> ListarAgendamentosPorDiscente(int discenteId)
    {
        var result = await _agendamentoService.ListarAgendamentosPorDiscenteAsync(discenteId);
        return Ok(result);
    }

    /// <summary>
    /// Lista os horários disponíveis de um profissional específico.
    /// </summary>
    /// <param name="profissionalId">ID do profissional cujos horários devem ser listados.</param>
    /// <returns>Uma lista de horários disponíveis do profissional.</returns>
    [HttpGet("horarios/{profissionalId}")]
    public async Task<IActionResult> ListarHorariosDisponiveis(int profissionalId)
    {
        var result = await _agendamentoService.ListarHorariosDisponiveisAsync(profissionalId);
        return Ok(result);
    }
}
