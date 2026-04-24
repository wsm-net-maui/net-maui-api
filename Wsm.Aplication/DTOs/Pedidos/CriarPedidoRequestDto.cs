namespace Wsm.Aplication.DTOs.Pedidos;

public class CriarPedidoRequestDto
{
    public string? CodigoVoucher { get; set; }
    public List<CriarPedidoItemRequestDto> Itens { get; set; } = new();
}
