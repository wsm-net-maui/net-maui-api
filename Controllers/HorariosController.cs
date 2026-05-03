using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.HorariosAtendimento;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class HorariosController : BaseController
{
    private readonly IHorarioAtendimentoService _horarioService;

    public HorariosController(IHorarioAtendimentoService horarioService)
    {
        _horarioService = horarioService;
    }

    [HttpGet("funcionario/{funcionarioId:guid}")]
    [AllowAnonymous]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorFuncionario(Guid funcionarioId)
    {
        var horarios = await _horarioService.ObterTodosPorFuncionarioAsync(funcionarioId);
        return Success(horarios);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var horario = await _horarioService.ObterPorIdAsync(id);
        if (horario == null) return NotFound("Horário não encontrado");
        return Success(horario);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] HorarioAtendimentoCreateDto dto)
    {
        var horario = await _horarioService.CriarAsync(dto);
        return Created(horario, "Horário criado com sucesso");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] HorarioAtendimentoUpdateDto dto)
    {
        await _horarioService.AtualizarAsync(id, dto);
        return NoContent("Horário atualizado com sucesso");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [LogExecutionTime]
    public async Task<IActionResult> Remover(Guid id)
    {
        await _horarioService.RemoverAsync(id);
        return NoContent("Horário removido com sucesso");
    }
}

