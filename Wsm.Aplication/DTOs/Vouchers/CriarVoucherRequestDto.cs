using Wsm.Domain.Enums;

namespace Wsm.Aplication.DTOs.Vouchers;

public class CriarVoucherRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public TipoDescontoVoucher TipoDesconto { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal? ValorMinimoPedido { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int? UsoMaximo { get; set; }
}
