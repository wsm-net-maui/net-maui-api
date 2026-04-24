using Wsm.Aplication.DTOs.Vouchers;
using Wsm.Domain.Entities;

namespace Wsm.Aplication.Mappers;

public static class VoucherMapper
{
    public static VoucherResponseDto ToResponseDto(Voucher voucher)
    {
        return new VoucherResponseDto
        {
            Id = voucher.Id,
            Codigo = voucher.Codigo,
            TipoDesconto = voucher.TipoDesconto,
            TipoDescontoDescricao = voucher.TipoDesconto.ToString(),
            ValorDesconto = voucher.ValorDesconto,
            ValorMinimoPedido = voucher.ValorMinimoPedido,
            DataInicio = voucher.DataInicio,
            DataFim = voucher.DataFim,
            UsoMaximo = voucher.UsoMaximo,
            UsoAtual = voucher.UsoAtual,
            Ativo = voucher.Ativo,
            CriadoEm = voucher.CriadoEm,
            AtualizadoEm = voucher.AtualizadoEm
        };
    }

    public static Voucher ToEntity(CriarVoucherRequestDto dto)
    {
        return new Voucher(
            codigo: dto.Codigo,
            tipoDesconto: dto.TipoDesconto,
            valorDesconto: dto.ValorDesconto,
            dataInicio: dto.DataInicio,
            dataFim: dto.DataFim,
            valorMinimoPedido: dto.ValorMinimoPedido,
            usoMaximo: dto.UsoMaximo
        );
    }
}
