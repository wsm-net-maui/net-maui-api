using Wsm.Domain.Enums;

namespace Wsm.Aplication.DTOs.Vouchers;

public class VoucherResponseDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public TipoDescontoVoucher TipoDesconto { get; set; }
    public string TipoDescontoDescricao { get; set; } = string.Empty;
    public decimal ValorDesconto { get; set; }
    public decimal? ValorMinimoPedido { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int? UsoMaximo { get; set; }
    public int UsoAtual { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
