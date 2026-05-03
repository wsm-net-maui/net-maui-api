using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.ClientePerfis;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class ClientesController : BaseController
{
    private readonly IClientePerfilService _clienteService;

    public ClientesController(IClientePerfilService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodos()
    {
        var perfis = await _clienteService.ObterTodosAsync();
        return Success(perfis);
    }

    [HttpGet("{id:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var perfil = await _clienteService.ObterPorIdAsync(id);
        if (perfil == null) return NotFound("Cliente não encontrado");
        return Success(perfil);
    }

    [HttpGet("usuario/{usuarioId:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorUsuarioId(Guid usuarioId)
    {
        var perfil = await _clienteService.ObterPorUsuarioIdAsync(usuarioId);
        if (perfil == null) return NotFound("Perfil de cliente não encontrado");
        return Success(perfil);
    }

    [HttpPost]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] ClientePerfilCreateDto dto)
    {
        var perfil = await _clienteService.CriarAsync(dto);
        return Created(perfil, "Perfil de cliente criado com sucesso");
    }

    [HttpPut("{id:guid}")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ClientePerfilUpdateDto dto)
    {
        await _clienteService.AtualizarAsync(id, dto);
        return NoContent("Perfil de cliente atualizado com sucesso");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [LogExecutionTime]
    public async Task<IActionResult> Remover(Guid id)
    {
        await _clienteService.RemoverAsync(id);
        return NoContent("Perfil de cliente removido com sucesso");
    }
}

