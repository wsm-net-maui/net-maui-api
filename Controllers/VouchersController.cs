using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Vouchers;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

[Authorize]
public class VouchersController : BaseController
{
    private readonly IVoucherService _voucherService;

    public VouchersController(IVoucherService voucherService)
    {
        _voucherService = voucherService;
    }

    [HttpGet]
    [LogExecutionTime]
    public async Task<IActionResult> ObterTodos()
    {
        var vouchers = await _voucherService.ObterTodosAsync();
        return Success(vouchers);
    }

    [HttpGet("{id:guid}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var voucher = await _voucherService.ObterPorIdAsync(id);

        if (voucher is null)
            return NotFound("Voucher não encontrado");

        return Success(voucher);
    }

    [HttpGet("codigo/{codigo}")]
    [LogExecutionTime]
    public async Task<IActionResult> ObterPorCodigo(string codigo)
    {
        var voucher = await _voucherService.ObterPorCodigoAsync(codigo);

        if (voucher is null)
            return NotFound("Voucher não encontrado");

        return Success(voucher);
    }

    [HttpPost]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Criar([FromBody] CriarVoucherRequestDto request)
    {
        var voucher = await _voucherService.CriarAsync(request);
        return Created(voucher, "Voucher criado com sucesso");
    }

    [HttpPatch("{id:guid}/ativar")]
    [LogExecutionTime]
    public async Task<IActionResult> Ativar(Guid id)
    {
        await _voucherService.AtivarAsync(id);
        return NoContent("Voucher ativado com sucesso");
    }

    [HttpPatch("{id:guid}/desativar")]
    [LogExecutionTime]
    public async Task<IActionResult> Desativar(Guid id)
    {
        await _voucherService.DesativarAsync(id);
        return NoContent("Voucher desativado com sucesso");
    }
}
