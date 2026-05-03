using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.FuncionarioPerfis;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class FuncionariosController : BaseController
{
    private readonly IFuncionarioPerfilService _funcionarioService;

    public FuncionariosController(IFuncionarioPerfilService funcionarioService)
    {
        _funcionarioService = funcionarioService;
    }

    [HttpGet]
    [AllowAnonymous]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodos()
    {
        var perfis = await _funcionarioService.ObterTodosAsync();
        return Success(perfis);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var perfil = await _funcionarioService.ObterPorIdAsync(id);
        if (perfil == null) return NotFound("Funcionário não encontrado");
        return Success(perfil);
    }

    [HttpGet("usuario/{usuarioId:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorUsuarioId(Guid usuarioId)
    {
        var perfil = await _funcionarioService.ObterPorUsuarioIdAsync(usuarioId);
        if (perfil == null) return NotFound("Perfil de funcionário não encontrado");
        return Success(perfil);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] FuncionarioPerfilCreateDto dto)
    {
        var perfil = await _funcionarioService.CriarAsync(dto);
        return Created(perfil, "Perfil de funcionário criado com sucesso");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] FuncionarioPerfilUpdateDto dto)
    {
        await _funcionarioService.AtualizarAsync(id, dto);
        return NoContent("Perfil de funcionário atualizado com sucesso");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [LogExecutionTime]
    public async Task<IActionResult> Remover(Guid id)
    {
        await _funcionarioService.RemoverAsync(id);
        return NoContent("Perfil de funcionário removido com sucesso");
    }
}

