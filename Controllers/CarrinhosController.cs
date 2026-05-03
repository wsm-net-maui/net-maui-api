using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Carrinhos;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class CarrinhosController : BaseController
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly IPedidoService _pedidoService;

    public CarrinhosController(ICarrinhoService carrinhoService, IPedidoService pedidoService)
    {
        _carrinhoService = carrinhoService;
        _pedidoService = pedidoService;
    }

    [HttpGet("meu")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterMeuCarrinho()
    {
        var usuarioId = ObterUsuarioId();
        var carrinho = await _carrinhoService.ObterMeuCarrinhoAsync(usuarioId);
        return Success(carrinho);
    }

    [HttpPost("itens")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> AdicionarItem([FromBody] AdicionarItemCarrinhoRequestDto request)
    {
        var usuarioId = ObterUsuarioId();
        var carrinho = await _carrinhoService.AdicionarItemAsync(usuarioId, request);
        return Success(carrinho, "Item adicionado ao carrinho");
    }

    [HttpPatch("itens/{produtoId:guid}")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> AtualizarQuantidadeItem(Guid produtoId, [FromBody] AtualizarQuantidadeCarrinhoRequestDto request)
    {
        var usuarioId = ObterUsuarioId();
        var carrinho = await _carrinhoService.AtualizarQuantidadeItemAsync(usuarioId, produtoId, request);
        return Success(carrinho, "Quantidade atualizada no carrinho");
    }

    [HttpDelete("itens/{produtoId:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> RemoverItem(Guid produtoId)
    {
        var usuarioId = ObterUsuarioId();
        var carrinho = await _carrinhoService.RemoverItemAsync(usuarioId, produtoId);
        return Success(carrinho, "Item removido do carrinho");
    }

    [HttpPatch("voucher")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> AplicarVoucher([FromBody] AplicarVoucherCarrinhoRequestDto request)
    {
        var usuarioId = ObterUsuarioId();
        var carrinho = await _carrinhoService.AplicarVoucherAsync(usuarioId, request);
        return Success(carrinho, "Voucher aplicado com sucesso");
    }

    [HttpDelete("voucher")]
    [LogExecutionTime]
    public async Task<IActionResult> RemoverVoucher()
    {
        var usuarioId = ObterUsuarioId();
        var carrinho = await _carrinhoService.RemoverVoucherAsync(usuarioId);
        return Success(carrinho, "Voucher removido com sucesso");
    }

    [HttpDelete]
    [LogExecutionTime]
    public async Task<IActionResult> Limpar()
    {
        var usuarioId = ObterUsuarioId();
        await _carrinhoService.LimparAsync(usuarioId);
        return NoContent("Carrinho limpo com sucesso");
    }

    [HttpPost("checkout")]
    [LogExecutionTime]
    public async Task<IActionResult> Checkout()
    {
        var usuarioId = ObterUsuarioId();
        var pedido = await _pedidoService.CriarDoCarrinhoAsync(usuarioId);
        return Created(pedido, "Pedido criado a partir do carrinho com sucesso");
    }

    }
