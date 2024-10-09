using System.Collections.Generic;
using System.Threading.Tasks;
using Back_end.Dtos;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AgendamentoController : ControllerBase
{
    private readonly IAgendamentoService _agendamentoService;

    public AgendamentoController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

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

    [HttpGet("listar/discente/{discenteId}")]
    public async Task<IActionResult> ListarAgendamentosPorDiscente(int discenteId)
    {
        var result = await _agendamentoService.ListarAgendamentosPorDiscenteAsync(discenteId);
        return Ok(result);
    }

    [HttpGet("horarios/{profissionalId}")]
    public async Task<IActionResult> ListarHorariosDisponiveis(int profissionalId)
    {
        var result = await _agendamentoService.ListarHorariosDisponiveisAsync(profissionalId);
        return Ok(result);
    }
}
