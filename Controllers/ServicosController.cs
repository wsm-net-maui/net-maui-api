using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Servicos;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class ServicosController : BaseController
{
    private readonly IServicoService _servicoService;

    public ServicosController(IServicoService servicoService)
    {
        _servicoService = servicoService;
    }

    [HttpGet]
    [AllowAnonymous]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodos()
    {
        var servicos = await _servicoService.ObterTodosAsync();
        return Success(servicos);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var servico = await _servicoService.ObterPorIdAsync(id);
        if (servico == null) return NotFound("Serviço não encontrado");
        return Success(servico);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] ServicoCreateDto dto)
    {
        var servico = await _servicoService.CriarAsync(dto);
        return Created(servico, "Serviço criado com sucesso");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ServicoUpdateDto dto)
    {
        await _servicoService.AtualizarAsync(id, dto);
        return NoContent("Serviço atualizado com sucesso");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [LogExecutionTime]
    public async Task<IActionResult> Remover(Guid id)
    {
        await _servicoService.RemoverAsync(id);
        return NoContent("Serviço removido com sucesso");
    }
}

