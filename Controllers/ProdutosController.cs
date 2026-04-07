using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Produtos;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class ProdutosController : BaseController
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodos()
    {
        var produtos = await _produtoService.ObterTodosAsync();
        return Success(produtos);
    }

    [HttpGet("{id:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);
        
        if (produto == null)
            return NotFound("Produto não encontrado");

        return Success(produto);
    }

    [HttpGet("categoria/{categoriaId:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorCategoria(Guid categoriaId)
    {
        var produtos = await _produtoService.ObterPorCategoriaAsync(categoriaId);
        return Success(produtos);
    }

    [HttpGet("estoque-minimo")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterAbaixoEstoqueMinimo()
    {
        var produtos = await _produtoService.ObterAbaixoEstoqueMinimoAsync();
        return Success(produtos);
    }

    [HttpPost]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoRequestDto request)
    {
        var produto = await _produtoService.CriarAsync(request);
        return Created(produto, "Produto criado com sucesso");
    }

    [HttpPut("{id:guid}")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarProdutoRequestDto request)
    {
        var produto = await _produtoService.AtualizarAsync(id, request);
        return Success(produto, "Produto atualizado com sucesso");
    }

    [HttpPatch("{id:guid}/ativar")]
    [LogExecutionTime]
    public async Task<IActionResult> Ativar(Guid id)
    {
        await _produtoService.AtivarAsync(id);
        return NoContent("Produto ativado com sucesso");
    }

    [HttpPatch("{id:guid}/desativar")]
    [LogExecutionTime]
    public async Task<IActionResult> Desativar(Guid id)
    {
        await _produtoService.DesativarAsync(id);
        return NoContent("Produto desativado com sucesso");
    }
}
