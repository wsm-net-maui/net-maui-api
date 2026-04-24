using Wsm.Aplication.DTOs.Pedidos;
using Wsm.Domain.Entities;

namespace Wsm.Aplication.Mappers;

public static class PedidoMapper
{
    public static PedidoResponseDto ToResponseDto(Pedido pedido)
    {
        return new PedidoResponseDto
        {
            Id = pedido.Id,
            Numero = pedido.Numero,
            UsuarioId = pedido.UsuarioId,
            UsuarioNome = pedido.Usuario?.Nome ?? string.Empty,
            Status = pedido.Status,
            StatusDescricao = pedido.Status.ToString(),
            VoucherId = pedido.VoucherId,
            VoucherCodigo = pedido.Voucher?.Codigo,
            ValorBruto = pedido.ValorBruto,
            ValorDesconto = pedido.ValorDesconto,
            ValorTotal = pedido.ValorTotal,
            CriadoEm = pedido.CriadoEm,
            AtualizadoEm = pedido.AtualizadoEm,
            Itens = pedido.Itens.Select(ToItemResponseDto).ToList()
        };
    }

    public static PedidoItemResponseDto ToItemResponseDto(PedidoItem item)
    {
        return new PedidoItemResponseDto
        {
            ProdutoId = item.ProdutoId,
            ProdutoNome = item.ProdutoNome,
            Quantidade = item.Quantidade,
            PrecoUnitario = item.PrecoUnitario,
            Subtotal = item.CalcularSubtotal()
        };
    }
}
