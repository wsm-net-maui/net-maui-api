using Wsm.Aplication.DTOs.Carrinhos;
using Wsm.Domain.Entities;

namespace Wsm.Aplication.Mappers;

public static class CarrinhoMapper
{
    public static CarrinhoResponseDto ToResponseDto(Carrinho carrinho)
    {
        return new CarrinhoResponseDto
        {
            Id = carrinho.Id,
            UsuarioId = carrinho.UsuarioId,
            VoucherId = carrinho.VoucherId,
            VoucherCodigo = carrinho.Voucher?.Codigo,
            ValorBruto = carrinho.ValorBruto,
            ValorDesconto = carrinho.ValorDesconto,
            ValorTotal = carrinho.ValorTotal,
            Itens = carrinho.Itens.Select(ToItemResponseDto).ToList(),
            CriadoEm = carrinho.CriadoEm,
            AtualizadoEm = carrinho.AtualizadoEm
        };
    }

    public static CarrinhoItemResponseDto ToItemResponseDto(CarrinhoItem item)
    {
        return new CarrinhoItemResponseDto
        {
            ProdutoId = item.ProdutoId,
            ProdutoNome = item.ProdutoNome,
            Quantidade = item.Quantidade,
            PrecoUnitario = item.PrecoUnitario,
            Subtotal = item.CalcularSubtotal()
        };
    }
}
