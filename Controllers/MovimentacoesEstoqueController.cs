using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Movimentacoes;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class MovimentacoesEstoqueController : BaseController
{
    private readonly IMovimentacaoEstoqueService _movimentacaoService;

    public MovimentacoesEstoqueController(IMovimentacaoEstoqueService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    [HttpGet]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodas()
    {
        var movimentacoes = await _movimentacaoService.ObterTodasAsync();
        return Success(movimentacoes);
    }

    [HttpGet("{id:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var movimentacao = await _movimentacaoService.ObterPorIdAsync(id);
        
        if (movimentacao == null)
            return NotFound("Movimentação não encontrada");

        return Success(movimentacao);
    }

    [HttpGet("produto/{produtoId:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorProduto(Guid produtoId)
    {
        var movimentacoes = await _movimentacaoService.ObterPorProdutoAsync(produtoId);
        return Success(movimentacoes);
    }

    [HttpPost]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] CriarMovimentacaoRequestDto request)
    {
        var usuarioId = ObterUsuarioId();
        var movimentacao = await _movimentacaoService.CriarAsync(request, usuarioId);
        return Created(movimentacao, "Movimentação registrada com sucesso");
    }

    private Guid ObterUsuarioId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}
