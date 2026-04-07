using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Categorias;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class CategoriasController : BaseController
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodas()
    {
        var categorias = await _categoriaService.ObterTodasAsync();
        return Success(categorias);
    }

    [HttpGet("{id:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var categoria = await _categoriaService.ObterPorIdAsync(id);
        
        if (categoria == null)
            return NotFound("Categoria não encontrada");

        return Success(categoria);
    }

    [HttpPost]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] CriarCategoriaRequestDto request)
    {
        var categoria = await _categoriaService.CriarAsync(request);
        return Created(categoria, "Categoria criada com sucesso");
    }

    [HttpPut("{id:guid}")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarCategoriaRequestDto request)
    {
        var categoria = await _categoriaService.AtualizarAsync(id, request);
        return Success(categoria, "Categoria atualizada com sucesso");
    }

    [HttpPatch("{id:guid}/ativar")]
    [LogExecutionTime]
    public async Task<IActionResult> Ativar(Guid id)
    {
        await _categoriaService.AtivarAsync(id);
        return NoContent("Categoria ativada com sucesso");
    }

    [HttpPatch("{id:guid}/desativar")]
    [LogExecutionTime]
    public async Task<IActionResult> Desativar(Guid id)
    {
        await _categoriaService.DesativarAsync(id);
        return NoContent("Categoria desativada com sucesso");
    }
}
