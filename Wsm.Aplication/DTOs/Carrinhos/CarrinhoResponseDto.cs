namespace Wsm.Aplication.DTOs.Carrinhos;

public class CarrinhoResponseDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid? VoucherId { get; set; }
    public string? VoucherCodigo { get; set; }
    public decimal ValorBruto { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CarrinhoItemResponseDto> Itens { get; set; } = new();
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
