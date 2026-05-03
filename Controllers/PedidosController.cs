using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Pedidos;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class PedidosController : BaseController
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodos()
    {
        var pedidos = await _pedidoService.ObterTodosAsync();
        return Success(pedidos);
    }

    [HttpGet("{id:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);

        if (pedido is null)
            return NotFound("Pedido não encontrado");

        return Success(pedido);
    }

    [HttpGet("meus")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterMeusPedidos()
    {
        var usuarioId = ObterUsuarioId();
        var pedidos = await _pedidoService.ObterPorUsuarioAsync(usuarioId);
        return Success(pedidos);
    }

    [HttpPost]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoRequestDto request)
    {
        var usuarioId = ObterUsuarioId();
        var pedido = await _pedidoService.CriarAsync(request, usuarioId);
        return Created(pedido, "Pedido criado com sucesso");
    }

    [HttpPatch("{id:guid}/concluir")]
    [LogExecutionTime]
    public async Task<IActionResult> Concluir(Guid id)
    {
        var usuarioId = ObterUsuarioId();
        var pedido = await _pedidoService.ConcluirAsync(id, usuarioId);
        return Success(pedido, "Pedido concluído com sucesso");
    }

    [HttpPatch("{id:guid}/cancelar")]
    [LogExecutionTime]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        await _pedidoService.CancelarAsync(id);
        return NoContent("Pedido cancelado com sucesso");
    }

    }
